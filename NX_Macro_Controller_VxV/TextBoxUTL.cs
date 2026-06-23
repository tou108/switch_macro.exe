using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

internal class TextBoxUTL
{
	private const int SB_HORZ = 0;

	private const int SB_VERT = 1;

	private const int WM_HSCROLL = 276;

	private const int WM_VSCROLL = 277;

	private const int SB_THUMBPOSITION = 4;

	private static Point pos = new Point(0, 0);

	[DllImport("USER32.DLL", CharSet = CharSet.Auto)]
	private static extern int GetScrollPos(IntPtr hWnd, int nBar);

	[DllImport("user32.dll")]
	private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

	[DllImport("user32.dll")]
	private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

	public static int GetYpos(TextBox textBox)
	{
		return GetScrollPos(textBox.Handle, 1);
	}

	public static void SetYpos(TextBox textBox, int y)
	{
		SendMessage(textBox.Handle, 277, (y << 16) + 4, 0);
	}

	public static int GetLineHeight(TextBox textBox)
	{
		return TextRenderer.MeasureText("test", textBox.Font).Height;
	}

	public static int GetTextSize(TextBox textBox)
	{
		int i = 0;
		int lineHeight = GetLineHeight(textBox);
		for (; textBox.GetFirstCharIndexFromLine(i) != -1; i++)
		{
		}
		return lineHeight * i;
	}

	public static Size GetTextSize(TextBox textBox, Size proposedSize)
	{
		using Graphics dc = textBox.CreateGraphics();
		return TextRenderer.MeasureText(dc, textBox.Text, textBox.Font, proposedSize, CreateTextFormatFlags(textBox));
	}

	public static TextFormatFlags CreateTextFormatFlags(TextBox textBox)
	{
		TextFormatFlags textFormatFlags = TextFormatFlags.ExpandTabs | TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
		if (!textBox.Multiline)
		{
			textFormatFlags |= TextFormatFlags.SingleLine;
		}
		else if (textBox.WordWrap)
		{
			textFormatFlags |= TextFormatFlags.WordBreak;
		}
		if (textBox.RightToLeft == RightToLeft.Yes)
		{
			textFormatFlags |= TextFormatFlags.RightToLeft;
			return textBox.TextAlign switch
			{
				HorizontalAlignment.Center => textFormatFlags | TextFormatFlags.HorizontalCenter, 
				HorizontalAlignment.Right => textFormatFlags | TextFormatFlags.Default, 
				_ => textFormatFlags | TextFormatFlags.Right, 
			};
		}
		return textBox.TextAlign switch
		{
			HorizontalAlignment.Center => textFormatFlags | TextFormatFlags.HorizontalCenter, 
			HorizontalAlignment.Right => textFormatFlags | TextFormatFlags.Right, 
			_ => textFormatFlags | TextFormatFlags.Default, 
		};
	}

	public static string GetString(byte[] data)
	{
		return GetEncoding(data).GetString(data);
	}

	public static Encoding GetEncoding(string filename)
	{
		byte[] array = new byte[4];
		using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
		{
			fileStream.Read(array, 0, 4);
		}
		if (array[0] == 43 && array[1] == 47 && array[2] == 118)
		{
			return Encoding.UTF7;
		}
		if (array[0] == 239 && array[1] == 187 && array[2] == 191)
		{
			return Encoding.UTF8;
		}
		if (array[0] == byte.MaxValue && array[1] == 254)
		{
			return Encoding.Unicode;
		}
		if (array[0] == 254 && array[1] == byte.MaxValue)
		{
			return Encoding.BigEndianUnicode;
		}
		if (array[0] == byte.MaxValue && array[1] == 254 && array[2] == 0 && array[3] == 0)
		{
			return Encoding.Unicode;
		}
		if (array[0] == 0 && array[1] == 0 && array[2] == 254 && array[3] == byte.MaxValue)
		{
			return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
		}
		return Encoding.ASCII;
	}

	public static Encoding GetEncoding(byte[] file)
	{
		if (file[0] == 43 && file[1] == 47 && file[2] == 118)
		{
			return Encoding.UTF7;
		}
		if (file[0] == 239 && file[1] == 187 && file[2] == 191)
		{
			return Encoding.UTF8;
		}
		if (file[0] == byte.MaxValue && file[1] == 254)
		{
			return Encoding.Unicode;
		}
		if (file[0] == 254 && file[1] == byte.MaxValue)
		{
			return Encoding.BigEndianUnicode;
		}
		if (file[0] == byte.MaxValue && file[1] == 254 && file[2] == 0 && file[3] == 0)
		{
			return Encoding.Unicode;
		}
		if (file[0] == 0 && file[1] == 0 && file[2] == 254 && file[3] == byte.MaxValue)
		{
			return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
		}
		return Encoding.ASCII;
	}
}

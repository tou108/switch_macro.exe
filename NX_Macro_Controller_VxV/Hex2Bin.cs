using System;
using System.Globalization;

namespace NX_Macro_Controller_VxV;

internal class Hex2Bin
{
	public static byte[] Convert(byte[] hexData)
	{
		return Convert(TextBoxUTL.GetString(hexData));
	}

	public static byte[] Convert(string hexData)
	{
		string[] array = hexData.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\n")
			.Split('\n');
		byte[] array2 = new byte[64512];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = byte.MaxValue;
		}
		byte[] array3 = new byte[64512];
		int num = 0;
		string[] array4 = array;
		foreach (string text in array4)
		{
			if (text.Length <= 0 || text[0] != ':')
			{
				continue;
			}
			try
			{
				int num2 = int.Parse(text.Substring(1, 2), NumberStyles.HexNumber);
				int num3 = int.Parse(text.Substring(3, 4), NumberStyles.HexNumber);
				int num4 = num3 & 0xFF;
				int num5 = (num3 >> 8) & 0xFF;
				int num6 = int.Parse(text.Substring(7, 2), NumberStyles.HexNumber);
				string text2 = text.Substring(9, num2 * 2);
				int num7 = num2 + num4 + num5 + num6;
				int num8 = int.Parse(text.Substring(9 + num2 * 2, 2), NumberStyles.HexNumber);
				for (int k = 0; k < num2; k++)
				{
					array3[k] = byte.Parse(text2.Substring(k * 2, 2), NumberStyles.HexNumber);
					num7 += array3[k];
				}
				if (((num7 + num8) & 0xFF) != 0)
				{
					continue;
				}
				for (int l = 0; l < num2; l++)
				{
					array2[num3 + l] = array3[l];
					if (num < num3 + l)
					{
						num = num3 + l;
					}
				}
			}
			catch
			{
			}
		}
		Array.Resize(ref array2, num + 1);
		return array2;
	}
}

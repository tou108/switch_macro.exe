using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

public class BitmapAccessor : IDisposable
{
	private Bitmap original_;

	private BitmapData data_;

	private bool disposed_;

	public int Width => data_.Width;

	public int Height => data_.Height;

	public static bool IsSupported(PixelFormat format)
	{
		switch (format)
		{
		case PixelFormat.Format24bppRgb:
		case PixelFormat.Format32bppRgb:
		case PixelFormat.Canonical:
		case PixelFormat.Format32bppArgb:
			return true;
		default:
			return false;
		}
	}

	public BitmapAccessor(Bitmap img)
	{
		original_ = img;
		data_ = original_.LockBits(new Rectangle(0, 0, original_.Width, original_.Height), ImageLockMode.ReadWrite, img.PixelFormat);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed_ && disposing && original_ != null && data_ != null)
		{
			original_.UnlockBits(data_);
			data_ = null;
		}
		disposed_ = true;
	}

	public Color GetPixel(int x, int y)
	{
		int offset = GetOffset(x, y);
		return Color.FromArgb(GetAlpha(offset), GetRed(offset), GetGreen(offset), GetBlue(offset));
	}

	public void SetPixel(int x, int y, Color c)
	{
		int offset = GetOffset(x, y);
		SetAlpha(offset, c.A);
		SetRed(offset, c.R);
		SetGreen(offset, c.G);
		SetBlue(offset, c.B);
	}

	public unsafe void SetPixelRange(int x, int y, int x2, int y2, BitmapAccessor bitmapAccessor)
	{
		int num = GetOffset(x, y);
		byte* ptr = (byte*)data_.Scan0.ToPointer();
		byte* ptr2 = (byte*)bitmapAccessor.data_.Scan0.ToPointer();
		if (data_.PixelFormat == PixelFormat.Format32bppArgb || data_.PixelFormat == PixelFormat.Format32bppPArgb || data_.PixelFormat == PixelFormat.Format32bppRgb)
		{
			for (int i = y; i < y2; i++)
			{
				for (int j = num; j < 4 * (x2 - x) + num; j += 4)
				{
					ptr[j] = ptr2[j];
					ptr[j + 1] = ptr2[j + 1];
					ptr[j + 2] = ptr2[j + 2];
					ptr[j + 3] = ptr2[j + 3];
				}
				num += data_.Stride;
			}
		}
		else
		{
			if (data_.PixelFormat != PixelFormat.Format24bppRgb)
			{
				return;
			}
			for (int k = y; k < y2; k++)
			{
				for (int l = num; l < 3 * (x2 - x) + num; l += 3)
				{
					ptr[l] = ptr2[l];
					ptr[l + 1] = ptr2[l + 1];
					ptr[l + 2] = ptr2[l + 2];
				}
				num += data_.Stride;
			}
		}
	}

	private int GetOffset(int x, int y)
	{
		switch (data_.PixelFormat)
		{
		case PixelFormat.Format64bppPArgb:
		case PixelFormat.Format64bppArgb:
			return x * 8 + data_.Stride * y;
		case PixelFormat.Format48bppRgb:
			return x * 6 + data_.Stride * y;
		case PixelFormat.Format32bppRgb:
		case PixelFormat.Format32bppPArgb:
		case PixelFormat.Canonical:
		case PixelFormat.Format32bppArgb:
			return x * 4 + data_.Stride * y;
		case PixelFormat.Format24bppRgb:
			return x * 3 + data_.Stride * y;
		case PixelFormat.Format16bppRgb555:
		case PixelFormat.Format16bppRgb565:
		case PixelFormat.Format16bppArgb1555:
		case PixelFormat.Format16bppGrayScale:
			return x * 2 + data_.Stride * y;
		case PixelFormat.Format8bppIndexed:
			return x + data_.Stride * y;
		case PixelFormat.Format4bppIndexed:
			return 2 / x + data_.Stride * y;
		case PixelFormat.Format1bppIndexed:
			return 8 / x + data_.Stride * y;
		default:
			return -1;
		}
	}

	private int GetAlpha(int offset)
	{
		if (data_.PixelFormat == PixelFormat.Format32bppArgb)
		{
			return Marshal.ReadByte(data_.Scan0, offset + 3);
		}
		return 255;
	}

	private byte GetRed(int offset)
	{
		return Marshal.ReadByte(data_.Scan0, offset + 2);
	}

	private byte GetGreen(int offset)
	{
		return Marshal.ReadByte(data_.Scan0, offset + 1);
	}

	private byte GetBlue(int offset)
	{
		return Marshal.ReadByte(data_.Scan0, offset);
	}

	private void SetAlpha(int offset, byte value)
	{
		if (data_.PixelFormat == PixelFormat.Format32bppArgb)
		{
			Marshal.WriteByte(data_.Scan0, offset + 3, value);
		}
	}

	private void SetRed(int offset, byte value)
	{
		Marshal.WriteByte(data_.Scan0, offset + 2, value);
	}

	private void SetGreen(int offset, byte value)
	{
		Marshal.WriteByte(data_.Scan0, offset + 1, value);
	}

	private void SetBlue(int offset, byte value)
	{
		Marshal.WriteByte(data_.Scan0, offset, value);
	}
}

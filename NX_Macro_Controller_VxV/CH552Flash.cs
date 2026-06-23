using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

internal class CH552Flash
{
	private byte[] Buff = new byte[64];

	private byte[] CmdDetect = new byte[21]
	{
		161, 18, 0, 0, 17, 77, 67, 85, 32, 73,
		83, 80, 32, 38, 32, 87, 67, 72, 46, 67,
		78
	};

	private int RespondDetect = 6;

	private byte[] CmdId = new byte[5] { 167, 2, 0, 31, 0 };

	private int RespondId = 30;

	private int DeviceID;

	private int FamilyID;

	private byte[] CmdWriteBootOptions = new byte[17]
	{
		168, 14, 0, 7, 0, 255, 255, 255, 255, 3,
		0, 0, 0, 255, 82, 0, 0
	};

	private int RespondWriteBootOptions = 6;

	private byte[] CmdNewBootkey = new byte[33]
	{
		163, 30, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0
	};

	private int RespondNewBootkey = 6;

	private byte[] CmdErase = new byte[4] { 164, 1, 0, 8 };

	private int RespondErase = 6;

	private byte[] CmdReset = new byte[4] { 162, 1, 0, 1 };

	private int RespondReset = 6;

	private byte[] CmdWrite = new byte[64]
	{
		165, 61, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0
	};

	private int RespondWrite = 6;

	private byte[] CmdVerify = new byte[64]
	{
		166, 61, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0
	};

	private int RespondVerify = 6;

	private byte[] CmdRead = new byte[1];

	private int RespondRead = 6;

	public static bool CH552Write(byte[] data, uint id = 0u)
	{
		uint ioLength = (uint)data.Length;
		return CH375.CH375WriteData(id, data, ref ioLength);
	}

	public static bool CH552Read(byte[] data, uint id = 0u)
	{
		uint ioLength = (uint)data.Length;
		return CH375.CH375ReadData(id, data, ref ioLength);
	}

	internal CH552Flash()
	{
	}

	internal bool FlashHex(byte[] hexData, long waitTime = 10000L)
	{
		byte[] binData = Hex2Bin.Convert(hexData);
		return FlashBin(binData, waitTime);
	}

	internal bool FlashHex(string path, long waitTime = 10000L)
	{
		byte[] binData = Hex2Bin.Convert(File.ReadAllText(path));
		return FlashBin(binData, waitTime);
	}

	internal bool FlashBin(byte[] binData, long waitTime = 10000L)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Restart();
		while (stopwatch.ElapsedMilliseconds < waitTime)
		{
			Application.DoEvents();
			if (_flashBin(binData))
			{
				return true;
			}
		}
		return false;
	}

	private bool _flashBin(byte[] binData)
	{
		uint num = 0u;
		try
		{
			if (CH375.CH375GetUsbID(num).ToString("X") == "55E04348" && (int)CH375.CH375OpenDevice(num) != 0)
			{
				if (!CH552Write(CmdDetect, num))
				{
					return false;
				}
				if (!CH552Read(Buff, num))
				{
					return false;
				}
				DeviceID = Buff[4];
				FamilyID = Buff[5];
				if (DeviceID == 82 && FamilyID == 17)
				{
					if (!CH552Write(CmdId, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					byte[] array = new byte[8];
					int num2 = Buff[22] + Buff[23] + Buff[24] + Buff[25];
					for (int i = 0; i < 8; i++)
					{
						array[i] = (byte)num2;
					}
					array[7] += (byte)DeviceID;
					if (!CH552Write(CmdWriteBootOptions, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					if (!CH552Write(CmdId, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					if (!CH552Write(CmdNewBootkey, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					CmdErase[3] = 8;
					if (!CH552Write(CmdErase, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					int num3 = binData.Length;
					int num4 = (num3 + 55) / 56;
					int num5 = num3 % 56;
					num5 = (num5 + 7) / 8 * 8;
					if (num5 == 0)
					{
						num5 = 56;
					}
					Array.Resize(ref binData, (num3 + 7) / 8 * 8);
					for (int j = 0; j < num4; j++)
					{
						int length = 56;
						if (j == num4 - 1)
						{
							length = num5;
						}
						Array.Copy(binData, j * 56, CmdWrite, 8, length);
						for (int k = 0; k < 7; k++)
						{
							for (int l = 0; l < 8; l++)
							{
								CmdWrite[8 + k * 8 + l] ^= array[l];
							}
						}
						int num6 = j * 56;
						if (j == num4 - 1)
						{
							CmdWrite[1] = (byte)(61 - (56 - num5));
						}
						CmdWrite[3] = (byte)(num6 & 0xFF);
						CmdWrite[4] = (byte)(num6 >> 8);
						if (!CH552Write(CmdWrite, num))
						{
							return false;
						}
						if (!CH552Read(Buff, num))
						{
							return false;
						}
					}
					if (!CH552Write(CmdNewBootkey, num))
					{
						return false;
					}
					if (!CH552Read(Buff, num))
					{
						return false;
					}
					for (int m = 0; m < num4; m++)
					{
						int length2 = 56;
						if (m == num4 - 1)
						{
							length2 = num5;
						}
						Array.Copy(binData, m * 56, CmdVerify, 8, length2);
						for (int n = 0; n < 7; n++)
						{
							for (int num7 = 0; num7 < 8; num7++)
							{
								CmdVerify[8 + n * 8 + num7] ^= array[num7];
							}
						}
						int num8 = m * 56;
						if (m == num4 - 1)
						{
							CmdVerify[1] = (byte)(61 - (56 - num5));
						}
						CmdVerify[3] = (byte)(num8 & 0xFF);
						CmdVerify[4] = (byte)(num8 >> 8);
						if (!CH552Write(CmdVerify, num))
						{
							return false;
						}
						if (!CH552Read(Buff, num))
						{
							return false;
						}
						if (Buff[4] != 0 || Buff[5] != 0)
						{
							return false;
						}
					}
					CH552Write(CmdReset, num);
					return true;
				}
			}
			return false;
		}
		finally
		{
			CH375.CH375CloseDevice(num);
		}
	}
}

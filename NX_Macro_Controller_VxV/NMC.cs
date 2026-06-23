using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NX_Macro_Controller_VxV.Properties;
using NxInterface;

namespace NX_Macro_Controller_VxV;

public class NMC
{
	internal class ch552ProgramLoop
	{
		internal int pos;

		internal int loopcnt;

		internal int length;
	}

	private NMC baseNmc;

	public string Code = "";

	public string FilePath = Path.GetFullPath(GlobalVar.BasePath ?? "");

	public string AllPath = "";

	public List<ResourcesImage> ResourcesImages = new List<ResourcesImage>();

	private List<ResourcesImage> exeResourcesImages = new List<ResourcesImage>();

	private ulong _padKeyFlag = 9259542121117908992uL;

	private ulong _keyBoardKeyFlag = 9259542121117908992uL;

	private ulong _nmcKeyFlag = 9259542121117908992uL;

	private ulong _nmcHoldKeyFlag = 9259542121117908992uL;

	private ulong _pythonKeyFlag = 9259542121117908992uL;

	private List<string> ch552Program = new List<string>();

	private bool ch552Analyze;

	private int ch552ProgramMaxSize = 100000;

	public bool Running;

	public bool RunningCsx;

	public string SubRunningNmc = "";

	public bool Cancel;

	private int counter_;

	public bool IsMain;

	private Stopwatch scrTimer = new Stopwatch();

	private ulong NmcKeyFlag
	{
		get
		{
			return _nmcKeyFlag;
		}
		set
		{
			_nmcKeyFlag = value;
			if (IsMain)
			{
				keyDataSend();
			}
		}
	}

	private ulong NmcHoldKeyFlag
	{
		get
		{
			return _nmcHoldKeyFlag;
		}
		set
		{
			_nmcHoldKeyFlag = value;
			if (IsMain)
			{
				keyDataSend();
			}
		}
	}

	public ulong PythonKeyFlag
	{
		get
		{
			return _pythonKeyFlag;
		}
		set
		{
			_pythonKeyFlag = value;
			keyDataSend();
		}
	}

	public ulong PadKeyFlag
	{
		get
		{
			return _padKeyFlag;
		}
		set
		{
			_padKeyFlag = value;
			keyDataSend();
		}
	}

	public ulong KeyBoardKeyFlag
	{
		get
		{
			return _keyBoardKeyFlag;
		}
		set
		{
			_keyBoardKeyFlag = value;
			keyDataSend();
		}
	}

	public string GetCH552Program()
	{
		ch552Analyze = true;
		ch552Program.Clear();
		try
		{
			baseNmc = this;
			Running = true;
			Cancel = false;
			SubRunningNmc = "";
			counter_ = 0;
			NmcCodeExecution(this);
		}
		catch (Exception)
		{
			Running = false;
			ch552Analyze = false;
			return null;
		}
		Running = false;
		ch552Analyze = false;
		_removeReport();
		string nXCtoC = Resources.NXCtoC;
		ch552Program.Insert(0, "void macro(){");
		ch552Program.Add("}");
		ch552Program.Insert(0, nXCtoC);
		return string.Join(Environment.NewLine, ch552Program);
	}

	private void _removeReport()
	{
		bool flag = false;
		for (int num = ch552Program.Count - 1; num >= 0; num--)
		{
			if (ch552Program[num].Length > 11 && ch552Program[num].Substring(0, 11) == "sendReport(")
			{
				if (flag)
				{
					ch552Program.RemoveAt(num);
				}
				else
				{
					flag = true;
				}
			}
			else if (ch552Program[num].Length > 6 && ch552Program[num].Substring(0, 6) == "delay(")
			{
				flag = false;
			}
			else if (ch552Program[num] == "{")
			{
				flag = false;
			}
		}
	}

	public void SendPythonSerial(string str)
	{
		string[] array = str.Split(' ');
		if (array[0] == "end")
		{
			PythonKeyFlag = 9259542121117908992uL;
		}
		else
		{
			if (array[0][0] < '0' || array[0][0] > '9')
			{
				return;
			}
			ulong num = 9259542121117908992uL;
			List<ulong> list = new List<ulong>();
			string[] array2 = array;
			foreach (string value in array2)
			{
				list.Add(Convert.ToUInt64(value, 16));
			}
			if (list[1] != 8)
			{
				if (list[1] == 0L)
				{
					num |= 0x20000;
				}
				if (list[1] == 1)
				{
					num |= 0x60000;
				}
				if (list[1] == 2)
				{
					num |= 0x40000;
				}
				if (list[1] == 3)
				{
					num |= 0x50000;
				}
				if (list[1] == 4)
				{
					num |= 0x10000;
				}
				if (list[1] == 5)
				{
					num |= 0x90000;
				}
				if (list[1] == 6)
				{
					num |= 0x80000;
				}
				if (list[1] == 7)
				{
					num |= 0xA0000;
				}
			}
			bool flag = (list[0] & 1) != 0;
			bool flag2 = (list[0] & 2) != 0;
			list[0] >>= 2;
			if ((list[0] & 4) != 0L)
			{
				num |= 8;
			}
			if ((list[0] & 2) != 0L)
			{
				num |= 4;
			}
			if ((list[0] & 8) != 0L)
			{
				num |= 2;
			}
			if ((list[0] & 1) != 0L)
			{
				num |= 1;
			}
			if ((list[0] & 0x40) != 0L)
			{
				num |= 0x800000;
			}
			if ((list[0] & 0x80) != 0L)
			{
				num |= 0x80;
			}
			if ((list[0] & 0x10) != 0L)
			{
				num |= 0x400000;
			}
			if ((list[0] & 0x20) != 0L)
			{
				num |= 0x40;
			}
			if ((list[0] & 0x200) != 0L)
			{
				num |= 0x200;
			}
			if ((list[0] & 0x100) != 0L)
			{
				num |= 0x100;
			}
			if ((list[0] & 0x1000) != 0L)
			{
				num |= 0x1000;
			}
			if ((list[0] & 0x2000) != 0L)
			{
				num |= 0x2000;
			}
			if ((list[0] & 0x400) != 0L)
			{
				num |= 0x800;
			}
			if ((list[0] & 0x800) != 0L)
			{
				num |= 0x400;
			}
			if (flag2)
			{
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= list[2] << 40;
				num |= list[3] << 32;
			}
			if (flag && flag2)
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= list[4] << 56;
				num |= list[5] << 48;
			}
			else if (flag)
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= list[2] << 56;
				num |= list[3] << 48;
			}
			PythonKeyFlag = num;
		}
	}

	public ulong GetPadAndKeyboardFlag()
	{
		ulong num = 0uL;
		ulong num2 = (_padKeyFlag >> 32) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 32) & 0xFF;
			num |= num2;
		}
		else
		{
			num |= num2;
		}
		num2 = (_padKeyFlag >> 40) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 40) & 0xFF;
			num |= num2 << 8;
		}
		else
		{
			num |= num2 << 8;
		}
		num2 = (_padKeyFlag >> 48) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 48) & 0xFF;
			num |= num2 << 16;
		}
		else
		{
			num |= num2 << 16;
		}
		num2 = (_padKeyFlag >> 56) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 56) & 0xFF;
			num |= num2 << 24;
		}
		else
		{
			num |= num2 << 24;
		}
		return ((_padKeyFlag | _keyBoardKeyFlag | _nmcKeyFlag | _pythonKeyFlag) & 0xFFFFFFFFu) | (num << 32);
	}

	private void keyDataSend()
	{
		ulong num = 0uL;
		ulong num2 = (_padKeyFlag >> 32) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 32) & 0xFF;
			if (num2 == 128)
			{
				num2 = (_nmcKeyFlag >> 32) & 0xFF;
				if (num2 == 128)
				{
					num2 = (_pythonKeyFlag >> 32) & 0xFF;
					if (num2 == 128)
					{
						num2 = (_nmcHoldKeyFlag >> 32) & 0xFF;
						num |= num2;
					}
					else
					{
						num |= num2;
					}
				}
				else
				{
					num |= num2;
				}
			}
			else
			{
				num |= num2;
			}
		}
		else
		{
			num |= num2;
		}
		num2 = (_padKeyFlag >> 40) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 40) & 0xFF;
			if (num2 == 128)
			{
				num2 = (_nmcKeyFlag >> 40) & 0xFF;
				if (num2 == 128)
				{
					num2 = (_pythonKeyFlag >> 40) & 0xFF;
					if (num2 == 128)
					{
						num2 = (_nmcHoldKeyFlag >> 40) & 0xFF;
						num |= num2 << 8;
					}
					else
					{
						num |= num2 << 8;
					}
				}
				else
				{
					num |= num2 << 8;
				}
			}
			else
			{
				num |= num2 << 8;
			}
		}
		else
		{
			num |= num2 << 8;
		}
		num2 = (_padKeyFlag >> 48) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 48) & 0xFF;
			if (num2 == 128)
			{
				num2 = (_nmcKeyFlag >> 48) & 0xFF;
				if (num2 == 128)
				{
					num2 = (_pythonKeyFlag >> 48) & 0xFF;
					if (num2 == 128)
					{
						num2 = (_nmcHoldKeyFlag >> 48) & 0xFF;
						num |= num2 << 16;
					}
					else
					{
						num |= num2 << 16;
					}
				}
				else
				{
					num |= num2 << 16;
				}
			}
			else
			{
				num |= num2 << 16;
			}
		}
		else
		{
			num |= num2 << 16;
		}
		num2 = (_padKeyFlag >> 56) & 0xFF;
		if (num2 == 128)
		{
			num2 = (_keyBoardKeyFlag >> 56) & 0xFF;
			if (num2 == 128)
			{
				num2 = (_nmcKeyFlag >> 56) & 0xFF;
				if (num2 == 128)
				{
					num2 = (_pythonKeyFlag >> 56) & 0xFF;
					if (num2 == 128)
					{
						num2 = (_nmcHoldKeyFlag >> 56) & 0xFF;
						num |= num2 << 24;
					}
					else
					{
						num |= num2 << 24;
					}
				}
				else
				{
					num |= num2 << 24;
				}
			}
			else
			{
				num |= num2 << 24;
			}
		}
		else
		{
			num |= num2 << 24;
		}
		SerialConnecter.KeyDataSend(((_padKeyFlag | _keyBoardKeyFlag | _nmcKeyFlag | _pythonKeyFlag | _nmcHoldKeyFlag) & 0xFFFFFFFFu) | (num << 32));
	}

	public void NMCRead(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			AllPath = path;
			NMCRead(File.ReadAllBytes(path));
			FilePath = Path.GetFullPath(Path.GetDirectoryName(path)) + "\\";
		}
	}

	public byte[] GetFileData()
	{
		bool flag = false;
		string text = Path.GetTempPath() + "\\NX";
		Random random = new Random(DateTime.Now.Millisecond);
		int num = random.Next();
		while (Directory.Exists(text + num))
		{
			num = random.Next();
		}
		text += num;
		Directory.CreateDirectory(text);
		Directory.CreateDirectory(text + "\\IMG");
		File.WriteAllText(text + "\\macro.nm2", Code, Encoding.Unicode);
		if (AllPath != "")
		{
			string text2 = Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath);
			if (Directory.Exists(text2))
			{
				string[] files = Directory.GetFiles(text2, "*", SearchOption.AllDirectories);
				flag = files.Length != 0;
				string[] array = files;
				foreach (string text3 in array)
				{
					string text4 = Util.GetRelativePath(text2, text3).Substring(2);
					Directory.CreateDirectory(Path.GetDirectoryName(text + "\\DATA\\" + text4));
					File.WriteAllBytes(text + "\\DATA\\" + text4, File.ReadAllBytes(text3));
				}
			}
		}
		if (flag)
		{
			foreach (ResourcesImage resourcesImage in ResourcesImages)
			{
				resourcesImage.image.Save(text + "\\IMG\\" + resourcesImage.label + ".png", ImageFormat.Png);
			}
		}
		else
		{
			foreach (ResourcesImage resourcesImage2 in ResourcesImages)
			{
				resourcesImage2.image.Save(text + "\\" + resourcesImage2.label + ".png", ImageFormat.Png);
			}
		}
		Util.SafelyCreateZipFromDirectory(text, text + "\\nx");
		FileStream fileStream = new FileStream(text + "\\nx", FileMode.Open, FileAccess.Read);
		byte[] array2 = new byte[fileStream.Length + 2];
		fileStream.Read(array2, 0, 4);
		fileStream.Read(array2, 6, array2.Length - 6);
		fileStream.Close();
		array2[0] = 231;
		array2[1] = (byte)(flag ? 136 : 72);
		array2[2] = 10;
		array2[3] = 131;
		int num2 = array2.Length / 2;
		uint num3 = 0u;
		for (int j = num2; j < array2.Length; j++)
		{
			num3 ^= array2[j];
		}
		for (int k = 6; k < num2; k++)
		{
			array2[k] ^= (byte)num3;
		}
		uint num4 = 160u;
		for (int l = 6; l < array2.Length; l++)
		{
			num4 += array2[l];
		}
		array2[4] = (byte)(num4 & 0xFF);
		array2[5] = (byte)((num4 >> 8) & 0xFF);
		Util.Delete(text);
		return array2;
	}

	public static string[] GetKeyList(ulong keyFlag)
	{
		List<string> list = new List<string>();
		if ((keyFlag & 8) != 0L)
		{
			list.Add("A");
		}
		if ((keyFlag & 4) != 0L)
		{
			list.Add("B");
		}
		if ((keyFlag & 2) != 0L)
		{
			list.Add("X");
		}
		if ((keyFlag & 1) != 0L)
		{
			list.Add("Y");
		}
		if ((keyFlag & 0x800000) != 0L)
		{
			list.Add("ZL");
		}
		if ((keyFlag & 0x80) != 0L)
		{
			list.Add("ZR");
		}
		if ((keyFlag & 0x400000) != 0L)
		{
			list.Add("L");
		}
		if ((keyFlag & 0x40) != 0L)
		{
			list.Add("R");
		}
		if ((keyFlag & 0x200) != 0L)
		{
			list.Add("START");
		}
		if ((keyFlag & 0x100) != 0L)
		{
			list.Add("SELECT");
		}
		if ((keyFlag & 0x1000) != 0L)
		{
			list.Add("HOME");
		}
		if ((keyFlag & 0x2000) != 0L)
		{
			list.Add("CAPTURE");
		}
		if ((keyFlag & 0x800) != 0L)
		{
			list.Add("CLICK_L");
		}
		if ((keyFlag & 0x400) != 0L)
		{
			list.Add("CLICK_R");
		}
		if ((keyFlag & 0xF0000) == 131072)
		{
			list.Add("UP");
		}
		if ((keyFlag & 0xF0000) == 393216)
		{
			list.Add("UPRIGHT");
		}
		if ((keyFlag & 0xF0000) == 262144)
		{
			list.Add("RIGHT");
		}
		if ((keyFlag & 0xF0000) == 327680)
		{
			list.Add("DOWNRIGHT");
		}
		if ((keyFlag & 0xF0000) == 65536)
		{
			list.Add("DOWN");
		}
		if ((keyFlag & 0xF0000) == 589824)
		{
			list.Add("DOWNLEFT");
		}
		if ((keyFlag & 0xF0000) == 524288)
		{
			list.Add("LEFT");
		}
		if ((keyFlag & 0xF0000) == 655360)
		{
			list.Add("UPLEFT");
		}
		ulong num = (keyFlag >> 32) & 0xFFFF;
		switch (num)
		{
		case 128uL:
			list.Add("LEFT_L");
			break;
		case 255uL:
			list.Add("DOWNLEFT_L");
			break;
		case 33023uL:
			list.Add("DOWN_L");
			break;
		case 65535uL:
			list.Add("DOWNRIGHT_L");
			break;
		case 65408uL:
			list.Add("RIGHT_L");
			break;
		case 65280uL:
			list.Add("UPRIGHT_L");
			break;
		case 32768uL:
			list.Add("UP_L");
			break;
		case 0uL:
			list.Add("UPLEFT_L");
			break;
		default:
		{
			double num2 = num >> 8;
			double num3 = num & 0xFF;
			num3 -= 127.5;
			num2 -= 127.5;
			num3 /= 127.5;
			num2 /= 127.5;
			string arg = Math.Min(1.0, Math.Abs(num2) + Math.Abs(num3)).ToString("F2");
			int num4 = (int)(Math.Atan2(num3, num2) * 180.0 / Math.PI);
			if (num4 < 0)
			{
				num4 += 360;
			}
			list.Add($"LS({num4}, {arg})");
			break;
		}
		case 32896uL:
			break;
		}
		ulong num5 = (keyFlag >> 48) & 0xFFFF;
		switch (num5)
		{
		case 128uL:
			list.Add("LEFT_R");
			break;
		case 255uL:
			list.Add("DOWNLEFT_R");
			break;
		case 33023uL:
			list.Add("DOWN_R");
			break;
		case 65535uL:
			list.Add("DOWNRIGHT_R");
			break;
		case 65408uL:
			list.Add("RIGHT_R");
			break;
		case 65280uL:
			list.Add("UPRIGHT_R");
			break;
		case 32768uL:
			list.Add("UP_R");
			break;
		case 0uL:
			list.Add("UPLEFT_R");
			break;
		default:
		{
			double num6 = num5 >> 8;
			double num7 = num5 & 0xFF;
			num7 -= 127.5;
			num6 -= 127.5;
			num7 /= 127.5;
			num6 /= 127.5;
			string arg2 = Math.Min(1.0, Math.Abs(num6) + Math.Abs(num7)).ToString("F2");
			int num8 = (int)(Math.Atan2(num7, num6) * 180.0 / Math.PI);
			if (num8 < 0)
			{
				num8 += 360;
			}
			list.Add($"RS({num8}, {arg2})");
			break;
		}
		case 32896uL:
			break;
		}
		return list.ToArray();
	}

	public void NMCRead(byte[] data)
	{
		data = (byte[])data.Clone();
		Code = "";
		ResourcesImages.Clear();
		if (data.Length <= 6)
		{
			return;
		}
		if (data[0] == 231 && data[1] == 72 && data[2] == 3 && data[3] == 131)
		{
			data[0] = 80;
			data[1] = 75;
			data[2] = 3;
			data[3] = 4;
			int num = data.Length / 2;
			uint num2 = 0u;
			for (int i = num; i < data.Length; i++)
			{
				num2 ^= data[i];
			}
			for (int j = 4; j < num; j++)
			{
				data[j] ^= (byte)num2;
			}
			string text = Path.GetTempPath() + "\\NX";
			Random random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
			int num3 = random.Next();
			while (Directory.Exists(text + num3))
			{
				num3 = random.Next();
			}
			text += num3;
			Directory.CreateDirectory(text);
			File.WriteAllBytes(text + "\\nx", data);
			ZipFile.ExtractToDirectory(text + "\\nx", text);
			File.Delete(text + "\\nx");
			Code = File.ReadAllText(text + "\\macro.nm2");
			File.Delete(text + "\\macro.nm2");
			string[] files = Directory.GetFiles(text);
			foreach (string path in files)
			{
				FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
				Image im = Image.FromStream(fileStream);
				fileStream.Close();
				ResourcesImages.Add(new ResourcesImage(im, Path.GetFileNameWithoutExtension(path)));
			}
			Util.Delete(text);
		}
		else if (data[0] == 231 && data[1] == 72 && data[2] == 10 && data[3] == 131)
		{
			data[0] = 80;
			data[1] = 75;
			data[2] = 3;
			data[3] = 4;
			uint num4 = data[4];
			num4 |= (uint)(data[5] << 8);
			uint num5 = 160u;
			for (int l = 6; l < data.Length; l++)
			{
				num5 += data[l];
			}
			num5 &= 0xFFFF;
			if (num5 != num4)
			{
				return;
			}
			int num6 = data.Length / 2;
			uint num7 = 0u;
			for (int m = num6; m < data.Length; m++)
			{
				num7 ^= data[m];
			}
			for (int n = 6; n < num6; n++)
			{
				data[n] ^= (byte)num7;
			}
			for (int num8 = 6; num8 < data.Length; num8++)
			{
				data[num8 - 2] = data[num8];
			}
			Array.Resize(ref data, data.Length - 2);
			string text2 = Path.GetTempPath() + "\\NX";
			Random random2 = new Random(DateTime.Now.Millisecond);
			int num9 = random2.Next();
			while (Directory.Exists(text2 + num9))
			{
				num9 = random2.Next();
			}
			text2 += num9;
			Directory.CreateDirectory(text2);
			File.WriteAllBytes(text2 + "\\nx", data);
			ZipFile.ExtractToDirectory(text2 + "\\nx", text2);
			File.Delete(text2 + "\\nx");
			Code = File.ReadAllText(text2 + "\\macro.nm2");
			File.Delete(text2 + "\\macro.nm2");
			string[] files = Directory.GetFiles(text2);
			foreach (string path2 in files)
			{
				FileStream fileStream2 = new FileStream(path2, FileMode.Open, FileAccess.Read);
				if (Path.GetExtension(path2) == ".png")
				{
					Image im2 = Image.FromStream(fileStream2);
					ResourcesImages.Add(new ResourcesImage(im2, Path.GetFileNameWithoutExtension(path2)));
				}
				fileStream2.Close();
			}
			Util.Delete(text2);
		}
		else if (data[0] == 231 && data[1] == 136 && data[2] == 10 && data[3] == 131)
		{
			data[0] = 80;
			data[1] = 75;
			data[2] = 3;
			data[3] = 4;
			uint num10 = data[4];
			num10 |= (uint)(data[5] << 8);
			uint num11 = 160u;
			for (int num12 = 6; num12 < data.Length; num12++)
			{
				num11 += data[num12];
			}
			num11 &= 0xFFFF;
			if (num11 != num10)
			{
				return;
			}
			int num13 = data.Length / 2;
			uint num14 = 0u;
			for (int num15 = num13; num15 < data.Length; num15++)
			{
				num14 ^= data[num15];
			}
			for (int num16 = 6; num16 < num13; num16++)
			{
				data[num16] ^= (byte)num14;
			}
			for (int num17 = 6; num17 < data.Length; num17++)
			{
				data[num17 - 2] = data[num17];
			}
			Array.Resize(ref data, data.Length - 2);
			string text3 = Path.GetTempPath() + "\\NX";
			Random random3 = new Random(DateTime.Now.Millisecond);
			int num18 = random3.Next();
			while (Directory.Exists(text3 + num18))
			{
				num18 = random3.Next();
			}
			text3 += num18;
			Directory.CreateDirectory(text3);
			File.WriteAllBytes(text3 + "\\nx", data);
			ZipFile.ExtractToDirectory(text3 + "\\nx", text3);
			File.Delete(text3 + "\\nx");
			Code = File.ReadAllText(text3 + "\\macro.nm2");
			File.Delete(text3 + "\\macro.nm2");
			try
			{
				string[] files;
				try
				{
					files = Directory.GetFiles(text3 + "\\IMG", "*", SearchOption.AllDirectories);
					foreach (string path3 in files)
					{
						FileStream fileStream3 = new FileStream(path3, FileMode.Open, FileAccess.Read);
						if (Path.GetExtension(path3) == ".png")
						{
							Image im3 = Image.FromStream(fileStream3);
							ResourcesImages.Add(new ResourcesImage(im3, Path.GetFileNameWithoutExtension(path3)));
						}
						fileStream3.Close();
					}
				}
				catch
				{
				}
				files = Directory.GetFiles(text3 + "\\DATA", "*", SearchOption.AllDirectories);
				foreach (string text4 in files)
				{
					FileStream fileStream4 = new FileStream(text4, FileMode.Open, FileAccess.Read);
					byte[] array = new byte[fileStream4.Length];
					fileStream4.Read(array, 0, array.Length);
					string text5 = Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath);
					text5 = text5 + "\\" + Util.GetRelativePath(text3 + "\\DATA", text4).Substring(2);
					Directory.CreateDirectory(Path.GetDirectoryName(text5));
					if (!File.Exists(text5))
					{
						File.WriteAllBytes(text5, array);
					}
					fileStream4.Close();
				}
			}
			catch
			{
			}
			Util.Delete(text3);
		}
		else
		{
			if (data[0] != 78 || data[1] != 88 || data[2] != 77 || data[3] != 65 || data[4] != 67 || data[5] != 82 || data[6] != 79)
			{
				return;
			}
			string[] array2 = new string[22]
			{
				"WAIT STATE", "BUTTON A", "BUTTON B", "BUTTON X", "BUTTON Y", "BUTTON L", "BUTTON R", "BUTTON START", "BUTTON HOME", "HAT UP",
				"HAT DOWN", "HAT LEFT", "HAT RIGHT", "HAT UP&LEFT", "HAT UP&RIGHT", "HAT DOWN&LEFT", "HAT DOWN&RIGHT", "ANALOG UP", "ANALOG DOWN", "ANALOG LEFT",
				"ANALOG RIGHT", "停止"
			};
			long num19 = 0L;
			Code = Code + "// 旧バージョンのNX Macro Controllerで作成されたデータです。" + Environment.NewLine + "// 現在のバージョンでは正しく動作しない可能性があります。" + Environment.NewLine + Environment.NewLine;
			for (int num20 = 7; num20 < data.Length; num20++)
			{
				if (data[num20] == 240)
				{
					num20++;
					int num21 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					int num22 = BitConverter.ToInt16(data, num20);
					num20 += 2;
					int num23 = BitConverter.ToInt16(data, num20);
					num20 += 2;
					Bitmap bitmap = new Bitmap(num22, num23);
					List<byte> list = new List<byte>();
					for (int num24 = 0; num24 < num21; num24++)
					{
						list.Add(data[num20 + num24]);
					}
					list = GZipData.GZipDecompress(list.ToArray()).ToList();
					using (BitmapAccessor bitmapAccessor = new BitmapAccessor(bitmap))
					{
						int num25 = 0;
						for (int num26 = 0; num26 < num22; num26++)
						{
							for (int num27 = 0; num27 < num23; num27++)
							{
								Color c = Color.FromArgb(list[num25], list[num25 + 1], list[num25 + 2]);
								num25 += 3;
								bitmapAccessor.SetPixel(num26, num27, c);
							}
						}
					}
					ResourcesImage resourcesImage = new ResourcesImage(bitmap, "NMC_IMAGE" + ResourcesImages.Count);
					ResourcesImages.Add(resourcesImage);
					Code = Code + "ImgCmp(" + resourcesImage.label + ")" + Environment.NewLine + "{" + Environment.NewLine;
					Code = Code + "\tWait(" + 0.1m + ")" + Environment.NewLine;
					num20 += num21;
					num20++;
					num19 = 1L;
					for (; data[num20] != 242; num20++)
					{
						if (data[num20] == 225)
						{
							num20++;
							int num28 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num29 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num30 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num31 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							Code = Code + "\tSnipping(" + num28 + ", " + num29 + ", " + num30 + ", " + num31 + ")" + Environment.NewLine;
							Code = Code + "\tWait(" + 0.1m + ")" + Environment.NewLine;
							continue;
						}
						if (data[num20] == data[num20 + 1])
						{
							num19++;
							continue;
						}
						if (array2[data[num20]] == "停止")
						{
							Code = Code + "\tStop" + Environment.NewLine;
						}
						else if (array2[data[num20]] == "WAIT STATE")
						{
							Code = Code + "\tWait(" + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "HAT")
						{
							Code += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								Code += "UP, ";
								break;
							case "DOWN":
								Code += "DOWN, ";
								break;
							case "LEFT":
								Code += "LEFT, ";
								break;
							case "RIGHT":
								Code += "RIGHT, ";
								break;
							case "UP&LEFT":
								Code += "UPLEFT, ";
								break;
							case "UP&RIGHT":
								Code += "UPRIGHT, ";
								break;
							case "DOWN&LEFT":
								Code += "DOWNLEFT, ";
								break;
							case "DOWN&RIGHT":
								Code += "DOWNRIGHT, ";
								break;
							}
							Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "ANALOG")
						{
							Code += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								Code += "UP_L, ";
								break;
							case "DOWN":
								Code += "DOWN_L, ";
								break;
							case "LEFT":
								Code += "LEFT_L, ";
								break;
							case "RIGHT":
								Code += "RIGHT_L, ";
								break;
							}
							Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else
						{
							Code = Code + "\tPress(" + array2[data[num20]].Split()[1] + ", " + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						num19 = 1L;
					}
					Code = Code + "}" + Environment.NewLine;
					num20 += 2;
					string text6 = "Not" + Environment.NewLine + "{" + Environment.NewLine;
					num19 = 1L;
					for (; data[num20] != 244; num20++)
					{
						if (data[num20] == 225)
						{
							num20++;
							int num32 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num33 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num34 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num35 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							text6 = text6 + "\tSnipping(" + num32 + ", " + num33 + ", " + num34 + ", " + num35 + ")" + Environment.NewLine;
							text6 = text6 + "\tWait(" + 0.1m + ")" + Environment.NewLine;
							continue;
						}
						if (data[num20] == data[num20 + 1])
						{
							num19++;
							continue;
						}
						if (array2[data[num20]] == "停止")
						{
							text6 = text6 + "\tStop" + Environment.NewLine;
						}
						else if (array2[data[num20]] == "WAIT STATE")
						{
							text6 = text6 + "\tWait(" + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "HAT")
						{
							text6 += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								text6 += "UP, ";
								break;
							case "DOWN":
								text6 += "DOWN, ";
								break;
							case "LEFT":
								text6 += "LEFT, ";
								break;
							case "RIGHT":
								text6 += "RIGHT, ";
								break;
							case "UP&LEFT":
								text6 += "UPLEFT, ";
								break;
							case "UP&RIGHT":
								text6 += "UPRIGHT, ";
								break;
							case "DOWN&LEFT":
								text6 += "DOWNLEFT, ";
								break;
							case "DOWN&RIGHT":
								text6 += "DOWNRIGHT, ";
								break;
							}
							text6 = text6 + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "ANALOG")
						{
							text6 += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								text6 += "UP_L, ";
								break;
							case "DOWN":
								text6 += "DOWN_L, ";
								break;
							case "LEFT":
								text6 += "LEFT_L, ";
								break;
							case "RIGHT":
								text6 += "RIGHT_L, ";
								break;
							}
							text6 = text6 + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else
						{
							text6 = text6 + "\tPress(" + array2[data[num20]].Split()[1] + ", " + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						num19 = 1L;
					}
					text6 = text6 + "}" + Environment.NewLine;
					if (text6 != "Not" + Environment.NewLine + "{" + Environment.NewLine + "}" + Environment.NewLine)
					{
						Code += text6;
					}
					continue;
				}
				if (data[num20] == 224)
				{
					num20++;
					int num36 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					Code = Code + "Loop(" + num36 + ")" + Environment.NewLine + "{" + Environment.NewLine;
					Code = Code + "\tWait(" + 0.1m + ")" + Environment.NewLine;
					num20++;
					num19 = 1L;
					for (; data[num20] != 242; num20++)
					{
						if (data[num20] == 225)
						{
							num20++;
							int num37 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num38 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num39 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							int num40 = BitConverter.ToInt32(data, num20);
							num20 += 4;
							Code = Code + "\tSnipping(" + num37 + ", " + num38 + ", " + num39 + ", " + num40 + ")" + Environment.NewLine;
							Code = Code + "\tWait(" + 0.1m + ")" + Environment.NewLine;
							continue;
						}
						if (data[num20] == data[num20 + 1])
						{
							num19++;
							continue;
						}
						if (array2[data[num20]] == "停止")
						{
							Code = Code + "\tStop" + Environment.NewLine;
						}
						else if (array2[data[num20]] == "WAIT STATE")
						{
							Code = Code + "\tWait(" + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "HAT")
						{
							Code += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								Code += "UP, ";
								break;
							case "DOWN":
								Code += "DOWN, ";
								break;
							case "LEFT":
								Code += "LEFT, ";
								break;
							case "RIGHT":
								Code += "RIGHT, ";
								break;
							case "UP&LEFT":
								Code += "UPLEFT, ";
								break;
							case "UP&RIGHT":
								Code += "UPRIGHT, ";
								break;
							case "DOWN&LEFT":
								Code += "DOWNLEFT, ";
								break;
							case "DOWN&RIGHT":
								Code += "DOWNRIGHT, ";
								break;
							}
							Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else if (array2[data[num20]].Split()[0] == "ANALOG")
						{
							Code += "\tPress(";
							switch (array2[data[num20]].Split()[1])
							{
							case "UP":
								Code += "UP_L, ";
								break;
							case "DOWN":
								Code += "DOWN_L, ";
								break;
							case "LEFT":
								Code += "LEFT_L, ";
								break;
							case "RIGHT":
								Code += "RIGHT_L, ";
								break;
							}
							Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						else
						{
							Code = Code + "\tPress(" + array2[data[num20]].Split()[1] + ", " + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
						}
						num19 = 1L;
					}
					Code = Code + "}" + Environment.NewLine;
					continue;
				}
				if (data[num20] == 225)
				{
					num20++;
					int num41 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					int num42 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					int num43 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					int num44 = BitConverter.ToInt32(data, num20);
					num20 += 4;
					Code = Code + "Snipping(" + num41 + ", " + num42 + ", " + num43 + ", " + num44 + ")" + Environment.NewLine;
					Code = Code + "Wait(" + 0.1m + ")" + Environment.NewLine;
					continue;
				}
				num19 = 1L;
				for (; num20 + 1 < data.Length && data[num20] == data[num20 + 1]; num20++)
				{
					num19++;
				}
				if (array2[data[num20]] == "停止")
				{
					Code = Code + "Stop" + Environment.NewLine;
				}
				else if (array2[data[num20]] == "WAIT STATE")
				{
					Code = Code + "Wait(" + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
				}
				else if (array2[data[num20]].Split()[0] == "HAT")
				{
					Code += "Press(";
					switch (array2[data[num20]].Split()[1])
					{
					case "UP":
						Code += "UP, ";
						break;
					case "DOWN":
						Code += "DOWN, ";
						break;
					case "LEFT":
						Code += "LEFT, ";
						break;
					case "RIGHT":
						Code += "RIGHT, ";
						break;
					case "UP&LEFT":
						Code += "UPLEFT, ";
						break;
					case "UP&RIGHT":
						Code += "UPRIGHT, ";
						break;
					case "DOWN&LEFT":
						Code += "DOWNLEFT, ";
						break;
					case "DOWN&RIGHT":
						Code += "DOWNRIGHT, ";
						break;
					}
					Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
				}
				else if (array2[data[num20]].Split()[0] == "ANALOG")
				{
					Code += "Press(";
					switch (array2[data[num20]].Split()[1])
					{
					case "UP":
						Code += "UP_L, ";
						break;
					case "DOWN":
						Code += "DOWN_L, ";
						break;
					case "LEFT":
						Code += "LEFT_L, ";
						break;
					case "RIGHT":
						Code += "RIGHT_L, ";
						break;
					}
					Code = Code + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
				}
				else
				{
					Code = Code + "Press(" + array2[data[num20]].Split()[1] + ", " + (decimal)num19 * 0.1m + ")" + Environment.NewLine;
				}
			}
		}
	}

	public string SubNmcRead(string code, int index)
	{
		string pattern = "(@(?:\"[^\"]*\")+|\"(?:[^\"\\n\\\\]+|\\\\.)*\"|'(?:[^'\\n\\\\]+|\\\\.)*')|//.*|/\\*(?s:.*?)\\*/";
		string[] source = Regex.Replace(code, pattern, "$1").Replace("\n", "\r\n").Replace("\r\r", "\r")
			.Replace("{", "\r\n{\r\n")
			.Replace("}", "\r\n}\r\n")
			.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		source = (from _ in source
			select _.Trim() into _
			where _ != ""
			select _).ToArray();
		for (int num = 0; num < source.Length; num++)
		{
			string[] array = AsmSplit(source[num]);
			string text = array[0];
			if (text == "ImgCmp")
			{
				string text2 = index + "<?>" + array[1];
				source[num] = "ImgCmp(" + text2 + ")";
			}
			else if (text == "ImgCmpRect")
			{
				string text3 = index + "<?>" + array[1];
				source[num] = "ImgCmpRect(" + text3 + ")";
			}
		}
		return string.Join("\r\n", source);
	}

	public void BackNmcExecution(string path, uint count = 1u)
	{
		NMC nm = new NMC();
		nm.NMCRead(path);
		SubRunningNmc = path;
		while (Running)
		{
			Thread.Sleep(8);
		}
		Running = true;
		Cancel = false;
		SubRunningNmc = path;
		for (int i = 0; i < count; i++)
		{
			nm.NmcExecution();
			Task.Factory.StartNew(delegate
			{
				while (true)
				{
					if (SubRunningNmc != path)
					{
						SubRunningNmc = "";
						break;
					}
					if (!nm.Running || Cancel)
					{
						break;
					}
					NmcKeyFlag = nm.NmcKeyFlag;
					NmcHoldKeyFlag = nm.NmcHoldKeyFlag;
					Thread.Sleep(1);
				}
				NmcKeyFlag = 9259542121117908992uL;
				NmcHoldKeyFlag = 9259542121117908992uL;
				nm.Cancel = true;
				Running = false;
			});
			while (Running)
			{
				Thread.Sleep(8);
			}
			Running = true;
		}
		Running = false;
	}

	public void NmcCodeExecution(NMC n, bool isMain = false, int startPos = 0, string callFuncName = "")
	{
		n.Code.Replace("\n", "\r\n").Replace("\r\r", "\r").Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		string text = Regex.Replace(n.Code, "//.*", "").Replace("\n", "\r\n").Replace("\r\r", "\r");
		Match match = Regex.Match(text, "/\\*(?s:.*?)\\*/");
		while (match.Success)
		{
			string value = match.Value;
			value = Regex.Replace(value, "[^\r\n]", "");
			text = text.Substring(0, match.Index) + value + text.Substring(match.Index + match.Length);
			match = Regex.Match(text, "/\\*(?s:.*?)\\*/");
		}
		string[] array = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
		text = text.Replace("{", "\r\n{\r\n").Replace("}", "\r\n}\r\n");
		List<string> source = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None).ToList();
		source = (from _ in source
			select _.Trim() into _
			where _ != ""
			select _).ToList();
		for (int num = 0; num < 10; num++)
		{
			source.Add("Stop");
		}
		int[] array2 = new int[source.Count];
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			array2[num2] = num2;
		}
		int num3 = 0;
		for (int num4 = 0; num4 < array.Length; num4++)
		{
			string text2 = array[num4].Trim();
			if (text2 == "")
			{
				for (int num5 = num4 - num3; num5 < array2.Length; num5++)
				{
					array2[num5]++;
				}
				num3++;
			}
			else
			{
				if (text2.Length < 2)
				{
					continue;
				}
				int num6 = text2.IndexOfAny(new char[2] { '{', '}' }, 1);
				while (num6 != -1)
				{
					num3--;
					for (int num7 = num4 - num3; num7 < array2.Length; num7++)
					{
						array2[num7]--;
					}
					if (num6 + 1 >= text2.Length)
					{
						break;
					}
					int num8 = num6;
					num6 = text2.IndexOfAny(new char[2] { '{', '}' }, num6 + 1);
					if (num6 > 0 && num8 + 1 != num6)
					{
						if (text2.Substring(num8 + 1, num6 - num8 - 1).Trim() != "")
						{
							num3--;
							for (int num9 = num4 - num3; num9 < array2.Length; num9++)
							{
								array2[num9]--;
							}
						}
					}
					else if (num6 < 0 && text2[text2.Length - 1] != '{' && text2[text2.Length - 1] != '}')
					{
						num3--;
						for (int num10 = num4 - num3; num10 < array2.Length; num10++)
						{
							array2[num10]--;
						}
					}
				}
			}
		}
		n.exeResourcesImages.Clear();
		n.exeResourcesImages.AddRange(n.ResourcesImages);
		List<int> list = new List<int>();
		list.AddRange(array2);
		array2 = list.ToArray();
		for (int num11 = 0; num11 < array2.Length; num11++)
		{
			if (array2[num11] >= startPos)
			{
				startPos = num11;
				break;
			}
		}
		int num12 = startPos;
		if (callFuncName != "")
		{
			string text3 = "Func(" + callFuncName + ")";
			for (int num13 = 0; num13 < source.Count; num13++)
			{
				if (source[num13] == text3)
				{
					num12 = num13 + 2;
					if (ch552Analyze)
					{
						ch552Program.Add("{");
					}
					break;
				}
			}
		}
		if (!ch552Analyze)
		{
			try
			{
				foreach (string item49 in source)
				{
					string[] array3 = AsmSplit(item49);
					if (array3[0] == "CallCsx")
					{
						string code = File.ReadAllText(_getResourcePath(array3[1]));
						_createRunCsScript(code);
					}
				}
			}
			catch (Exception value2)
			{
				Console.WriteLine(value2);
				return;
			}
		}
		List<string> list2 = new List<string>();
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		int num14 = 0;
		List<(string, long, int)> list6 = new List<(string, long, int)>();
		List<(string, decimal, int)> list7 = new List<(string, decimal, int)>();
		List<(string, string, int)> list8 = new List<(string, string, int)>();
		List<(string, bool, int)> list9 = new List<(string, bool, int)>();
		List<(string, int)> list10 = new List<(string, int)>();
		try
		{
			while (n.baseNmc.Running && !n.baseNmc.Cancel && !(n.baseNmc.SubRunningNmc != "") && num14 >= 0 && num12 < source.Count)
			{
				string text4 = source[num12].Trim();
				string[] array4 = AsmSplit(source[num12]);
				string text5 = array4[0];
				bool result;
				long result2;
				decimal result3;
				switch (text5)
				{
				case "{":
				{
					num14++;
					bool flag5 = false;
					try
					{
						switch (AsmSplit(source[num12 - 1])[0])
						{
						case "ImgCmp":
						case "ImgCmpRect":
						case "ImgCmp720p":
						case "ImgCmpRect720p":
						case "Loop":
						case "For":
						case "While":
						case "Func":
						case "Exec":
						case "Rumble":
						case "If":
						case "Else":
						case "ElseIf":
						case "Not":
							flag5 = true;
							break;
						}
					}
					catch
					{
					}
					if (ch552Analyze)
					{
						ch552Program.Add("{");
						num12++;
						continue;
					}
					if (!flag5)
					{
						list2.Insert(0, "Loop");
						list3.Insert(0, 1);
						list4.Insert(0, 0);
						list5.Insert(0, num12);
					}
					num12++;
					if (num12 < source.Count)
					{
						continue;
					}
					goto end_IL_04cd;
				}
				case "}":
				{
					string text22 = "";
					if (list10.Count > 0 && list10[0].Item2 == num14)
					{
						text22 = list10[0].Item1;
					}
					num14--;
					for (int num53 = 0; num53 < list6.Count; num53++)
					{
						if (list6[num53].Item3 > num14 && list6[num53].Item1 != text22)
						{
							list6.RemoveAt(num53);
							num53--;
						}
					}
					for (int num54 = 0; num54 < list7.Count; num54++)
					{
						if (list7[num54].Item3 > num14 && list7[num54].Item1 != text22)
						{
							list7.RemoveAt(num54);
							num54--;
						}
					}
					for (int num55 = 0; num55 < list8.Count; num55++)
					{
						if (list8[num55].Item3 > num14 && list8[num55].Item1 != text22)
						{
							list8.RemoveAt(num55);
							num55--;
						}
					}
					for (int num56 = 0; num56 < list9.Count; num56++)
					{
						if (list9[num56].Item3 > num14 && list9[num56].Item1 != text22)
						{
							list9.RemoveAt(num56);
							num56--;
						}
					}
					if (list2.Count != 0 && list2[0] == "Exec")
					{
						if (ch552Analyze)
						{
							ch552Program.Add("}");
						}
						num12 = list3[0];
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
						continue;
					}
					if (ch552Analyze)
					{
						ch552Program.Add("}");
						num12++;
						continue;
					}
					if (list2.Count == 0)
					{
						goto end_IL_04cd;
					}
					if (list2[0] == "While" || list2[0] == "For" || list2[0] == "Loop")
					{
						if (list4[0] == 0)
						{
							list3[0]--;
							if (list3[0] == 0)
							{
								list2.RemoveAt(0);
								list3.RemoveAt(0);
								list4.RemoveAt(0);
								list5.RemoveAt(0);
								num12++;
							}
							else
							{
								num12 = list5[0];
							}
						}
						else
						{
							num12 = list5[0];
						}
					}
					else if (list2[0] == "ImgCmp")
					{
						num12++;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						if (source[num12] == "Not")
						{
							num12++;
							if (num12 >= source.Count)
							{
								goto end_IL_04cd;
							}
							if (source[num12] == "{")
							{
								num12++;
								if (num12 >= source.Count)
								{
									goto end_IL_04cd;
								}
								int num57 = 0;
								while (num57 != 0 || source[num12] != "}")
								{
									if (source[num12] == "{")
									{
										num57++;
									}
									if (source[num12] == "}")
									{
										num57--;
									}
									num12++;
									if (num12 >= source.Count)
									{
										break;
									}
								}
								num12++;
								if (num12 >= source.Count)
								{
									goto end_IL_04cd;
								}
							}
						}
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					else if (list2[0] == "Rumble")
					{
						num12++;
						if (source[num12] == "Not")
						{
							num12++;
							if (source[num12] == "{")
							{
								num12++;
								for (int num58 = 0; num58 != 0 || source[num12] != "}"; num12++)
								{
									if (source[num12] == "{")
									{
										num58++;
									}
									if (source[num12] == "}")
									{
										num58--;
									}
								}
								num12++;
							}
						}
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					else if (list2[0] == "If" || list2[0] == "ElseIf")
					{
						num12++;
						while (source[num12] == "Else" || AsmSplit(source[num12])[0] == "ElseIf")
						{
							num12++;
							if (!(source[num12] == "{"))
							{
								continue;
							}
							num12++;
							for (int num59 = 0; num59 != 0 || source[num12] != "}"; num12++)
							{
								if (source[num12] == "{")
								{
									num59++;
								}
								if (source[num12] == "}")
								{
									num59--;
								}
							}
							num12++;
						}
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					else if (list2[0] == "Exec")
					{
						num12 = list3[0];
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					continue;
				}
				case "ImgCmp":
				case "ImgCmpRect":
				case "ImgCmp720p":
				case "ImgCmpRect720p":
				case "ImgCmpGray":
				case "ImgCmpRectGray":
				case "ImgCmpGray720p":
				case "ImgCmpRectGray720p":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("if(false)");
						num12++;
						continue;
					}
					list2.Insert(0, "ImgCmp");
					list3.Insert(0, -1);
					list4.Insert(0, -1);
					list5.Insert(0, -1);
					if (array4.Length >= 3 && isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					for (int num16 = 2; num16 < array4.Length; num16++)
					{
						array4[num16] = array4[num16].StrCalc(list7, list6, null, null, isDecimal: true);
					}
					bool flag2 = false;
					bool is720p = text5.Substring(text5.Length - 4, 4) == "720p";
					bool num17 = text5.Substring(0, Math.Min(text5.Length, 10)) == "ImgCmpRect";
					bool isGray = (text5.Substring(0, Math.Min(text5.Length, 10)) == "ImgCmpGray") | (text5.Substring(0, Math.Min(text5.Length, 14)) == "ImgCmpRectGray");
					if ((!num17) ? n.ScrImgCmp(array4, is720p, isGray) : n.ScrImgCmpRect(array4, is720p, isGray))
					{
						num12++;
						if (num12 < source.Count)
						{
							if (source[num12] != "{")
							{
								list2.RemoveAt(0);
								list3.RemoveAt(0);
								list4.RemoveAt(0);
								list5.RemoveAt(0);
							}
							continue;
						}
						goto end_IL_04cd;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (!(source[num12] == "{"))
					{
						continue;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num18 = 0;
					while (num18 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num18++;
						}
						if (source[num12] == "}")
						{
							num18--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (source[num12] == "Not")
					{
						num12++;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						if (source[num12] != "{")
						{
							list2.RemoveAt(0);
							list3.RemoveAt(0);
							list4.RemoveAt(0);
							list5.RemoveAt(0);
						}
					}
					else
					{
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					continue;
				}
				case "Rumble":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("if(false)");
						num12++;
						continue;
					}
					list2.Insert(0, "Rumble");
					list3.Insert(0, -1);
					list4.Insert(0, -1);
					list5.Insert(0, -1);
					if (array4.Length >= 2 && isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (n.ScrWaitRumble(array4))
					{
						num12++;
						if (num12 < source.Count)
						{
							if (source[num12] != "{")
							{
								list2.RemoveAt(0);
								list3.RemoveAt(0);
								list4.RemoveAt(0);
								list5.RemoveAt(0);
							}
							continue;
						}
						goto end_IL_04cd;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (!(source[num12] == "{"))
					{
						continue;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num43 = 0;
					while (num43 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num43++;
						}
						if (source[num12] == "}")
						{
							num43--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (source[num12] == "Not")
					{
						num12++;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						if (source[num12] != "{")
						{
							list2.RemoveAt(0);
							list3.RemoveAt(0);
							list4.RemoveAt(0);
							list5.RemoveAt(0);
						}
					}
					else
					{
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					continue;
				}
				case "If":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("if(" + array4[1] + ")");
						num12++;
						continue;
					}
					list2.Insert(0, "If");
					list3.Insert(0, -1);
					list4.Insert(0, -1);
					list5.Insert(0, -1);
					if (array4[1].StrCmp(list7, list6, list8, list9) == "True")
					{
						num12++;
						if (num12 < source.Count)
						{
							if (source[num12] != "{")
							{
								list2.RemoveAt(0);
								list3.RemoveAt(0);
								list4.RemoveAt(0);
								list5.RemoveAt(0);
							}
							continue;
						}
						goto end_IL_04cd;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (!(source[num12] == "{"))
					{
						continue;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num15 = 0;
					while (num15 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num15++;
						}
						if (source[num12] == "}")
						{
							num15--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					bool flag = false;
					while (AsmSplit(source[num12])[0] == "ElseIf")
					{
						if (AsmSplit(source[num12])[1].StrCmp(list7, list6, list8, list9) == "True")
						{
							flag = true;
							num12++;
							if (num12 >= source.Count)
							{
								break;
							}
							continue;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
						if (!(source[num12] == "{"))
						{
							continue;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
						num15 = 0;
						while (num15 != 0 || source[num12] != "}")
						{
							if (source[num12] == "{")
							{
								num15++;
							}
							if (source[num12] == "}")
							{
								num15--;
							}
							num12++;
							if (num12 >= source.Count)
							{
								break;
							}
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					if (source[num12] == "Else")
					{
						num12++;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						if (source[num12] != "{")
						{
							list2.RemoveAt(0);
							list3.RemoveAt(0);
							list4.RemoveAt(0);
							list5.RemoveAt(0);
						}
					}
					else
					{
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					continue;
				}
				case "Amiibo":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string text15 = array4[1];
					string text16 = "";
					text16 = ((text15[0] != 'R') ? array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ() : _getResourcePath(text15));
					NxControllerInterface.SendAmiibo(text16);
					num12++;
					continue;
				}
				case "For":
				{
					string text6 = "";
					bool flag3 = false;
					if (list5.Count == 0 || list5[0] != num12)
					{
						list2.Insert(0, "For");
						list3.Insert(0, 0);
						list4.Insert(0, 1);
						list5.Insert(0, num12);
						if (array4[1] != "" && array4[1].Substring(0, 4) == "Var ")
						{
							string text7 = array4[1].Substring(4).Trim();
							int num20 = text7.IndexOf('=');
							string text8 = "";
							string code2 = "0";
							if (num20 != -1)
							{
								text8 = text7.Substring(0, num20).Trim();
								code2 = text7.Substring(num20 + 1).Trim();
							}
							else
							{
								text8 = text7;
							}
							code2 = code2.StrCalc(list7, list6);
							foreach (var item50 in list9)
							{
								if (item50.Item1 == text8)
								{
									throw new Exception();
								}
							}
							foreach (var item51 in list8)
							{
								if (item51.Item1 == text8)
								{
									throw new Exception();
								}
							}
							foreach (var item52 in list6)
							{
								if (item52.Item1 == text8)
								{
									throw new Exception();
								}
							}
							foreach (var item53 in list7)
							{
								if (item53.Item1 == text8)
								{
									throw new Exception();
								}
							}
							if (code2[0] == '"')
							{
								if (code2[code2.Length - 1] != '"' || ch552Analyze)
								{
									throw new Exception();
								}
								list8.Add((text8, code2, num14 + 1));
								list10.Insert(0, (text8, num14 + 1));
							}
							else if (bool.TryParse(code2, out result))
							{
								text6 = "bool";
								list9.Add((text8, bool.Parse(code2), num14 + 1));
								list10.Insert(0, (text8, num14 + 1));
							}
							else if (long.TryParse(code2, out result2))
							{
								text6 = "long";
								list6.Add((text8, long.Parse(code2), num14 + 1));
								list10.Insert(0, (text8, num14 + 1));
							}
							else
							{
								if (!decimal.TryParse(code2, out result3))
								{
									throw new Exception();
								}
								text6 = "float";
								list7.Add((text8, decimal.Parse(code2), num14 + 1));
								list10.Insert(0, (text8, num14 + 1));
							}
						}
					}
					else if (array4[3] != "")
					{
						int num21 = array4[3].IndexOf("++");
						if (num21 != -1)
						{
							string text9 = array4[3].Substring(0, num21).Trim();
							if (num21 + 2 == array4[3].Length)
							{
								foreach (var item54 in list6)
								{
									if (!(item54.Item1 == text9))
									{
										continue;
									}
									flag3 = true;
									long num22 = long.Parse(text9.StrCalc(list7, list6));
									num22++;
									for (int num23 = 0; num23 < list6.Count; num23++)
									{
										if (list6[num23].Item1 == text9)
										{
											int item = list6[num23].Item3;
											list6.RemoveAt(num23);
											list6.Insert(num23, (text9, num22, item));
											break;
										}
									}
									break;
								}
								foreach (var item55 in list7)
								{
									if (flag3 || !(item55.Item1 == text9))
									{
										continue;
									}
									flag3 = true;
									decimal item2 = decimal.Parse(text9.StrCalc(list7, list6, null, null, isDecimal: true));
									++item2;
									for (int num24 = 0; num24 < list7.Count; num24++)
									{
										if (list7[num24].Item1 == text9)
										{
											int item3 = list7[num24].Item3;
											list7.RemoveAt(num24);
											list7.Insert(num24, (text9, item2, item3));
											break;
										}
									}
									break;
								}
							}
						}
						num21 = array4[3].IndexOf("--");
						if (num21 != -1)
						{
							string text10 = array4[3].Substring(0, num21).Trim();
							if (num21 + 2 == array4[3].Length)
							{
								foreach (var item56 in list6)
								{
									if (!(item56.Item1 == text10))
									{
										continue;
									}
									flag3 = true;
									long num25 = long.Parse(text10.StrCalc(list7, list6));
									num25--;
									for (int num26 = 0; num26 < list6.Count; num26++)
									{
										if (list6[num26].Item1 == text10)
										{
											int item4 = list6[num26].Item3;
											list6.RemoveAt(num26);
											list6.Insert(num26, (text10, num25, item4));
											break;
										}
									}
									break;
								}
								foreach (var item57 in list7)
								{
									if (flag3 || !(item57.Item1 == text10))
									{
										continue;
									}
									flag3 = true;
									decimal item5 = decimal.Parse(text10.StrCalc(list7, list6, null, null, isDecimal: true));
									--item5;
									for (int num27 = 0; num27 < list7.Count; num27++)
									{
										if (list7[num27].Item1 == text10)
										{
											int item6 = list7[num27].Item3;
											list7.RemoveAt(num27);
											list7.Insert(num27, (text10, item5, item6));
											break;
										}
									}
									break;
								}
							}
						}
						num21 = array4[3].IndexOf("+=");
						if (num21 != -1)
						{
							string text11 = array4[3].Substring(0, num21).Trim();
							string code3 = text11 + "+" + array4[3].Substring(num21 + 2).Trim();
							foreach (var item58 in list6)
							{
								if (!(item58.Item1 == text11))
								{
									continue;
								}
								flag3 = true;
								long item7 = long.Parse(code3.StrCalc(list7, list6));
								for (int num28 = 0; num28 < list6.Count; num28++)
								{
									if (list6[num28].Item1 == text11)
									{
										int item8 = list6[num28].Item3;
										list6.RemoveAt(num28);
										list6.Insert(num28, (text11, item7, item8));
										break;
									}
								}
								break;
							}
							foreach (var item59 in list7)
							{
								if (flag3 || !(item59.Item1 == text11))
								{
									continue;
								}
								flag3 = true;
								decimal item9 = decimal.Parse(code3.StrCalc(list7, list6, null, null, isDecimal: true));
								for (int num29 = 0; num29 < list7.Count; num29++)
								{
									if (list7[num29].Item1 == text11)
									{
										int item10 = list7[num29].Item3;
										list7.RemoveAt(num29);
										list7.Insert(num29, (text11, item9, item10));
										break;
									}
								}
								break;
							}
							foreach (var item60 in list8)
							{
								if (flag3 || !(item60.Item1 == text11))
								{
									continue;
								}
								flag3 = true;
								string item11 = code3.StrCalc(list7, list6, list8);
								for (int num30 = 0; num30 < list8.Count; num30++)
								{
									if (list8[num30].Item1 == text11)
									{
										int item12 = list8[num30].Item3;
										list8.RemoveAt(num30);
										list8.Insert(num30, (text11, item11, item12));
										break;
									}
								}
								break;
							}
						}
						num21 = array4[3].IndexOf("-=");
						if (num21 != -1)
						{
							string text12 = array4[3].Substring(0, num21).Trim();
							string code4 = text12 + "-" + array4[3].Substring(num21 + 2).Trim();
							foreach (var item61 in list6)
							{
								if (!(item61.Item1 == text12))
								{
									continue;
								}
								flag3 = true;
								long item13 = long.Parse(code4.StrCalc(list7, list6));
								for (int num31 = 0; num31 < list6.Count; num31++)
								{
									if (list6[num31].Item1 == text12)
									{
										int item14 = list6[num31].Item3;
										list6.RemoveAt(num31);
										list6.Insert(num31, (text12, item13, item14));
										break;
									}
								}
								break;
							}
							foreach (var item62 in list7)
							{
								if (flag3 || !(item62.Item1 == text12))
								{
									continue;
								}
								flag3 = true;
								decimal item15 = decimal.Parse(code4.StrCalc(list7, list6, null, null, isDecimal: true));
								for (int num32 = 0; num32 < list7.Count; num32++)
								{
									if (list7[num32].Item1 == text12)
									{
										int item16 = list7[num32].Item3;
										list7.RemoveAt(num32);
										list7.Insert(num32, (text12, item15, item16));
										break;
									}
								}
								break;
							}
						}
						num21 = array4[3].IndexOf('=');
						if (num21 != -1)
						{
							string text13 = array4[3].Substring(0, num21).Trim();
							string code5 = array4[3].Substring(num21 + 1).Trim();
							foreach (var item63 in list6)
							{
								if (!(item63.Item1 == text13))
								{
									continue;
								}
								flag3 = true;
								long item17 = long.Parse(code5.StrCalc(list7, list6));
								for (int num33 = 0; num33 < list6.Count; num33++)
								{
									if (list6[num33].Item1 == text13)
									{
										int item18 = list6[num33].Item3;
										list6.RemoveAt(num33);
										list6.Insert(num33, (text13, item17, item18));
										break;
									}
								}
								break;
							}
							foreach (var item64 in list7)
							{
								if (flag3 || !(item64.Item1 == text13))
								{
									continue;
								}
								flag3 = true;
								decimal item19 = decimal.Parse(code5.StrCalc(list7, list6, null, null, isDecimal: true));
								for (int num34 = 0; num34 < list7.Count; num34++)
								{
									if (list7[num34].Item1 == text13)
									{
										int item20 = list7[num34].Item3;
										list7.RemoveAt(num34);
										list7.Insert(num34, (text13, item19, item20));
										break;
									}
								}
								break;
							}
							foreach (var item65 in list8)
							{
								if (flag3 || !(item65.Item1 == text13))
								{
									continue;
								}
								flag3 = true;
								string item21 = code5.StrCmp(list7, list6, list8, list9);
								for (int num35 = 0; num35 < list8.Count; num35++)
								{
									if (list8[num35].Item1 == text13)
									{
										int item22 = list8[num35].Item3;
										list8.RemoveAt(num35);
										list8.Insert(num35, (text13, item21, item22));
										break;
									}
								}
								break;
							}
							foreach (var item66 in list9)
							{
								if (flag3 || !(item66.Item1 == text13))
								{
									continue;
								}
								flag3 = true;
								bool item23 = code5.StrCmp(list7, list6, list8, list9) == "True";
								for (int num36 = 0; num36 < list9.Count; num36++)
								{
									if (list9[num36].Item1 == text13)
									{
										int item24 = list9[num36].Item3;
										list9.RemoveAt(num36);
										list9.Insert(num36, (text13, item23, item24));
										break;
									}
								}
								break;
							}
						}
					}
					if (ch552Analyze)
					{
						if (text6 != "")
						{
							ch552Program.Add("for(" + text6 + " " + array4[1].Substring(4) + ";" + array4[2] + ";" + array4[3] + ")");
						}
						else
						{
							ch552Program.Add("for(;" + array4[2] + ";" + array4[3] + ")");
						}
						num12++;
						continue;
					}
					bool flag4 = true;
					if (array4[2] != "")
					{
						flag4 = array4[2].StrCmp(list7, list6, list8, list9) == "True";
					}
					if (!flag4)
					{
						num12 += 2;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						int num37 = 0;
						while (num37 != 0 || source[num12] != "}")
						{
							if (source[num12] == "{")
							{
								num37++;
							}
							if (source[num12] == "}")
							{
								num37--;
							}
							num12++;
							if (num12 >= source.Count)
							{
								break;
							}
						}
						string text14 = "";
						for (int num38 = 0; num38 < list6.Count; num38++)
						{
							if (list6[num38].Item3 > num14)
							{
								text14 = list6[num38].Item1;
								list6.RemoveAt(num38);
								num38--;
							}
						}
						for (int num39 = 0; num39 < list7.Count; num39++)
						{
							if (list7[num39].Item3 > num14)
							{
								text14 = list7[num39].Item1;
								list7.RemoveAt(num39);
								num39--;
							}
						}
						for (int num40 = 0; num40 < list8.Count; num40++)
						{
							if (list8[num40].Item3 > num14)
							{
								text14 = list8[num40].Item1;
								list8.RemoveAt(num40);
								num40--;
							}
						}
						for (int num41 = 0; num41 < list9.Count; num41++)
						{
							if (list9[num41].Item3 > num14)
							{
								text14 = list9[num41].Item1;
								list9.RemoveAt(num41);
								num41--;
							}
						}
						if (list10.Count > 0 && text14 == list10[0].Item1)
						{
							list10.RemoveAt(0);
						}
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					num12++;
					continue;
				}
				case "While":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("while(" + array4[1] + ")");
						num12++;
						continue;
					}
					bool num60 = array4[1].StrCmp(list7, list6, list8, list9) == "True";
					if (list5.Count == 0 || list5[0] != num12)
					{
						list2.Insert(0, "While");
						list3.Insert(0, 0);
						list4.Insert(0, 1);
						list5.Insert(0, num12);
					}
					if (!num60)
					{
						num12 += 2;
						if (num12 >= source.Count)
						{
							goto end_IL_04cd;
						}
						int num61 = 0;
						while (num61 != 0 || source[num12] != "}")
						{
							if (source[num12] == "{")
							{
								num61++;
							}
							if (source[num12] == "}")
							{
								num61--;
							}
							num12++;
							if (num12 >= source.Count)
							{
								break;
							}
						}
						for (int num62 = 0; num62 < list6.Count; num62++)
						{
							if (list6[num62].Item3 > num14)
							{
								list6.RemoveAt(num62);
								num62--;
							}
						}
						for (int num63 = 0; num63 < list7.Count; num63++)
						{
							if (list7[num63].Item3 > num14)
							{
								list7.RemoveAt(num63);
								num63--;
							}
						}
						for (int num64 = 0; num64 < list8.Count; num64++)
						{
							if (list8[num64].Item3 > num14)
							{
								list8.RemoveAt(num64);
								num64--;
							}
						}
						for (int num65 = 0; num65 < list9.Count; num65++)
						{
							if (list9[num65].Item3 > num14)
							{
								list9.RemoveAt(num65);
								num65--;
							}
						}
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
					}
					num12++;
					continue;
				}
				case "Loop":
				{
					int num42 = 0;
					if (array4.Length >= 2)
					{
						try
						{
							num42 = int.Parse(array4[1].StrCalc(list7, list6));
						}
						catch
						{
						}
					}
					if (ch552Analyze)
					{
						if (num42 != 0)
						{
							ch552Program.Add($"for(long ___nxloop{num12} = 0; ___nxloop{num12} < {num42}; ___nxloop{num12}++)");
						}
						else
						{
							ch552Program.Add("while(true)");
						}
						num12++;
					}
					else
					{
						list2.Insert(0, "Loop");
						list3.Insert(0, num42);
						if (num42 == 0)
						{
							list4.Insert(0, 1);
						}
						else
						{
							list4.Insert(0, 0);
						}
						list5.Insert(0, num12 + 1);
						num12++;
					}
					continue;
				}
				case "Continue":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("continue;");
						num12++;
						continue;
					}
					while (list2[0] != "While" && list2[0] != "For" && list2[0] != "Loop")
					{
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
						num14--;
					}
					num12 = list5[0] + 1;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (list2[0] != "Loop")
					{
						num12++;
					}
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num51 = 0;
					while (num51 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num51++;
						}
						if (source[num12] == "}")
						{
							num51--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					continue;
				}
				case "Break":
				{
					if (ch552Analyze)
					{
						ch552Program.Add("break;");
						num12++;
						continue;
					}
					while (list2[0] != "While" && list2[0] != "For" && list2[0] != "Loop")
					{
						list2.RemoveAt(0);
						list3.RemoveAt(0);
						list4.RemoveAt(0);
						list5.RemoveAt(0);
						num14--;
					}
					num12 = list5[0] + 1;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					if (list2[0] != "Loop")
					{
						num12++;
					}
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num50 = 0;
					while (num50 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num50++;
						}
						if (source[num12] == "}")
						{
							num50--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					list3[0] = 1;
					list4[0] = 0;
					continue;
				}
				case "Press":
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						_setCH552PressCommand(array4, list7, list6);
						num12++;
						continue;
					}
					array4[^1] = array4[^1].StrCalc(list7, list6, null, null, isDecimal: true);
					if (!_isKeyCommand(array4[^2]))
					{
						array4[^2] = array4[^2].StrCalc(list7, list6, null, null, isDecimal: true);
					}
					n.ScrPress(array4, list7, list6);
					num12++;
					continue;
				case "Hold":
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						_setCH552HoldCommand(array4, list7, list6);
						num12++;
					}
					else
					{
						n.ScrHold(array4, list7, list6);
						num12++;
					}
					continue;
				case "HoldRelease":
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						_setCH552HoldCommand(array4, list7, list6, isRelease: true);
						num12++;
					}
					else
					{
						n.ScrHold(array4, list7, list6, isRelease: true);
						num12++;
					}
					continue;
				case "Wait":
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						ch552Program.Add("delay(" + array4[^1] + ");");
						num12++;
					}
					else
					{
						array4[^1] = array4[^1].StrCalc(list7, list6, null, null, isDecimal: true);
						n.ScrWait(array4);
						num12++;
					}
					continue;
				case "Snipping":
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					n.ScrSnipping(array4);
					num12++;
					continue;
				case "Stop":
					if (ch552Analyze)
					{
						ch552Program.Add("return;");
						num12++;
						continue;
					}
					goto end_IL_04cd;
				case "Keyboard":
				{
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string str = "";
					if (array4.Length > 1)
					{
						str = array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ();
					}
					n.ScrKeyboard(Regex.Unescape(str));
					num12++;
					continue;
				}
				case "KeyboardMode":
				{
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string text23 = "";
					if (array4.Length > 1)
					{
						text23 = array4[1];
					}
					n.ScrKeyboardMode(text23);
					num12++;
					continue;
				}
				case "Notification":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string text19 = "";
					if (array4.Length > 1)
					{
						text19 = array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ();
					}
					NxCommand.SendWindowsNotify(text19);
					num12++;
					continue;
				}
				case "LineNotify":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string message2 = "";
					if (array4.Length > 1)
					{
						message2 = array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ();
					}
					NxCommand.SendLineNotify(message2);
					num12++;
					continue;
				}
				case "LineNotifyWithImage":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string message = "";
					if (array4.Length > 1)
					{
						message = array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ();
					}
					NxCommand.SendLineNotify(message, GlobalVar.MAINFORM.CaptureImage());
					num12++;
					continue;
				}
				case "Count":
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					if (isMain)
					{
						n.counter_++;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.MAINFORM.TaskViewLite();
					}
					num12++;
					continue;
				case "Call":
				{
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					string path3 = array4[1];
					NMC nMC2 = new NMC();
					nMC2.baseNmc = n.baseNmc;
					byte[] data2 = File.ReadAllBytes(_getResourcePath(path3));
					nMC2.AllPath = n.AllPath;
					nMC2.NMCRead(data2);
					nMC2.ch552Analyze = ch552Analyze;
					nMC2.ch552Program = ch552Program;
					nMC2.FilePath = Path.GetFullPath(Path.GetDirectoryName(n.AllPath)) + "\\";
					nMC2.NmcCodeExecution(nMC2);
					num12++;
					continue;
				}
				case "CallCsx":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					string path = array4[1];
					string code6 = File.ReadAllText(_getResourcePath(path));
					List<string> list11 = new List<string>();
					List<string> list12 = new List<string>();
					for (int num44 = 2; num44 < array4.Length; num44++)
					{
						string text17 = array4[num44].Trim();
						if (text17.Length >= 5 && text17.Substring(0, 4) == "Ref:")
						{
							text17 = text17.Substring(4, text17.Length - 4).Trim();
							list11.Add(text17.StrCmp(list7, list6, list8, list9).RemoveDQ());
							list12.Add(text17);
						}
						else
						{
							list11.Add(text17.StrCmp(list7, list6, list8, list9).RemoveDQ());
							list12.Add(null);
						}
					}
					string[] array5 = list11.ToArray();
					_runCsScript(code6, array5);
					for (int num45 = 0; num45 < list12.Count; num45++)
					{
						if (list12[num45] == null)
						{
							continue;
						}
						string text18 = list12[num45];
						for (int num46 = 0; num46 < list7.Count; num46++)
						{
							if (text18 == list7[num46].Item1)
							{
								list7.Insert(num46, (list7[num46].Item1, decimal.Parse(array5[num45]), list7[num46].Item3));
								list7.RemoveAt(num46 + 1);
							}
						}
						for (int num47 = 0; num47 < list6.Count; num47++)
						{
							if (text18 == list6[num47].Item1)
							{
								list6.Insert(num47, (list6[num47].Item1, long.Parse(array5[num45]), list6[num47].Item3));
								list6.RemoveAt(num47 + 1);
							}
						}
						for (int num48 = 0; num48 < list8.Count; num48++)
						{
							if (text18 == list8[num48].Item1)
							{
								list8.Insert(num48, (list8[num48].Item1, "\"" + array5[num45] + "\"", list8[num48].Item3));
								list8.RemoveAt(num48 + 1);
							}
						}
						for (int num49 = 0; num49 < list9.Count; num49++)
						{
							if (text18 == list9[num49].Item1)
							{
								list9.Insert(num49, (list9[num49].Item1, bool.Parse(array5[num45]), list9[num49].Item3));
								list9.RemoveAt(num49 + 1);
							}
						}
					}
					num12++;
					continue;
				}
				case "Print":
				{
					if (ch552Analyze)
					{
						num12++;
						continue;
					}
					string str2 = "";
					if (array4.Length > 1)
					{
						str2 = array4[1].StrCmp(list7, list6, list8, list9).Trim().RemoveDQ();
					}
					Console.WriteLine(Regex.Unescape(str2));
					num12++;
					continue;
				}
				case "Func":
				{
					num12 += 2;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					int num19 = 0;
					while (num19 != 0 || source[num12] != "}")
					{
						if (source[num12] == "{")
						{
							num19++;
						}
						if (source[num12] == "}")
						{
							num19--;
						}
						num12++;
						if (num12 >= source.Count)
						{
							break;
						}
					}
					num12++;
					continue;
				}
				case "Exec":
				{
					if (isMain)
					{
						GlobalVar.HighLightLine = array2[num12];
						GlobalVar.HighLightLineSet = true;
						GlobalVar.TaskName[5] = $"カウント : {n.counter_}";
						GlobalVar.TaskName[4] = $"マクロの実行中 - 行 : {array2[num12] + 1}";
						GlobalVar.MAINFORM.TaskViewLite();
						GlobalVar.MAINFORM.UpdateHighLightLite();
					}
					if (array4.Length == 3)
					{
						string path2 = array4[1];
						NMC nMC = new NMC();
						nMC.baseNmc = n.baseNmc;
						byte[] data = File.ReadAllBytes(_getResourcePath(path2));
						nMC.AllPath = n.AllPath;
						nMC.NMCRead(data);
						nMC.ch552Analyze = ch552Analyze;
						nMC.ch552Program = ch552Program;
						nMC.FilePath = Path.GetFullPath(Path.GetDirectoryName(n.AllPath)) + "\\";
						nMC.NmcCodeExecution(nMC, isMain: false, 0, array4[2]);
						num12++;
						continue;
					}
					num12++;
					if (num12 >= source.Count)
					{
						goto end_IL_04cd;
					}
					string text20 = array4[1];
					string text21 = "Func(" + text20 + ")";
					for (int num52 = 0; num52 < source.Count; num52++)
					{
						if (source[num52] == text21)
						{
							list2.Insert(0, "Exec");
							list3.Insert(0, num12);
							list4.Insert(0, -1);
							list5.Insert(0, -1);
							num14++;
							num12 = num52 + 2;
							if (num12 >= source.Count || !ch552Analyze)
							{
								break;
							}
							ch552Program.Add("{");
						}
					}
					continue;
				}
				case "Else":
					if (ch552Analyze)
					{
						ch552Program.Add("else");
						num12++;
					}
					else
					{
						num12++;
					}
					continue;
				case "ElseIf":
					if (ch552Analyze)
					{
						ch552Program.Add("else if(" + array4[1] + ")");
						num12++;
					}
					else
					{
						num12++;
					}
					continue;
				case "Not":
					if (ch552Analyze)
					{
						ch552Program.Add("else");
						num12++;
					}
					else
					{
						num12++;
					}
					continue;
				}
				bool flag6 = false;
				if (text4.Length > 5 && text4.Substring(0, 4) == "Var ")
				{
					string text24 = text4.Substring(4).Trim();
					int num66 = text24.IndexOf('=');
					string text25 = "";
					string code7 = "0";
					if (num66 != -1)
					{
						text25 = text24.Substring(0, num66).Trim();
						code7 = text24.Substring(num66 + 1).Trim();
					}
					else
					{
						text25 = text24;
					}
					if (Regex.IsMatch(text25, "[^a-zA-z0-9_]"))
					{
						throw new Exception();
					}
					code7 = code7.StrCalc(list7, list6);
					foreach (var item67 in list9)
					{
						if (item67.Item1 == text25)
						{
							throw new Exception();
						}
					}
					foreach (var item68 in list8)
					{
						if (item68.Item1 == text25)
						{
							throw new Exception();
						}
					}
					foreach (var item69 in list6)
					{
						if (item69.Item1 == text25)
						{
							throw new Exception();
						}
					}
					foreach (var item70 in list7)
					{
						if (item70.Item1 == text25)
						{
							throw new Exception();
						}
					}
					string text26 = "";
					if (code7[0] == '"')
					{
						if (code7[code7.Length - 1] != '"')
						{
							throw new Exception();
						}
						list8.Add((text25, code7, num14));
					}
					else if (bool.TryParse(code7, out result))
					{
						text26 = "bool";
						list9.Add((text25, bool.Parse(code7), num14));
					}
					else if (long.TryParse(code7, out result2))
					{
						text26 = "long";
						list6.Add((text25, long.Parse(code7), num14));
					}
					else
					{
						if (!decimal.TryParse(code7, out result3))
						{
							throw new Exception();
						}
						text26 = "float";
						list7.Add((text25, decimal.Parse(code7), num14));
					}
					flag6 = true;
					if (ch552Analyze && text26 != "")
					{
						ch552Program.Add("__xdata " + text26 + " " + text24 + ";");
					}
				}
				else
				{
					int num67 = text4.IndexOf("++");
					if (num67 != -1)
					{
						if (ch552Analyze)
						{
							ch552Program.Add(text4 + ";");
						}
						string text27 = text4.Substring(0, num67).Trim();
						if (num67 + 2 == text4.Length)
						{
							foreach (var item71 in list6)
							{
								if (!(item71.Item1 == text27))
								{
									continue;
								}
								flag6 = true;
								long num68 = long.Parse(text27.StrCalc(list7, list6));
								num68++;
								for (int num69 = 0; num69 < list6.Count; num69++)
								{
									if (list6[num69].Item1 == text27)
									{
										int item25 = list6[num69].Item3;
										list6.RemoveAt(num69);
										list6.Insert(num69, (text27, num68, item25));
										break;
									}
								}
								break;
							}
							foreach (var item72 in list7)
							{
								if (flag6 || !(item72.Item1 == text27))
								{
									continue;
								}
								flag6 = true;
								decimal item26 = decimal.Parse(text27.StrCalc(list7, list6, null, null, isDecimal: true));
								++item26;
								for (int num70 = 0; num70 < list7.Count; num70++)
								{
									if (list7[num70].Item1 == text27)
									{
										int item27 = list7[num70].Item3;
										list7.RemoveAt(num70);
										list7.Insert(num70, (text27, item26, item27));
										break;
									}
								}
								break;
							}
						}
					}
					num67 = text4.IndexOf("--");
					if (num67 != -1)
					{
						if (ch552Analyze)
						{
							ch552Program.Add(text4 + ";");
						}
						string text28 = text4.Substring(0, num67).Trim();
						if (num67 + 2 == text4.Length)
						{
							foreach (var item73 in list6)
							{
								if (!(item73.Item1 == text28))
								{
									continue;
								}
								flag6 = true;
								long num71 = long.Parse(text28.StrCalc(list7, list6));
								num71--;
								for (int num72 = 0; num72 < list6.Count; num72++)
								{
									if (list6[num72].Item1 == text28)
									{
										int item28 = list6[num72].Item3;
										list6.RemoveAt(num72);
										list6.Insert(num72, (text28, num71, item28));
										break;
									}
								}
								break;
							}
							foreach (var item74 in list7)
							{
								if (flag6 || !(item74.Item1 == text28))
								{
									continue;
								}
								flag6 = true;
								decimal item29 = decimal.Parse(text28.StrCalc(list7, list6, null, null, isDecimal: true));
								--item29;
								for (int num73 = 0; num73 < list7.Count; num73++)
								{
									if (list7[num73].Item1 == text28)
									{
										int item30 = list7[num73].Item3;
										list7.RemoveAt(num73);
										list7.Insert(num73, (text28, item29, item30));
										break;
									}
								}
								break;
							}
						}
					}
					num67 = text4.IndexOf("+=");
					if (num67 != -1)
					{
						if (ch552Analyze)
						{
							ch552Program.Add(text4 + ";");
						}
						string text29 = text4.Substring(0, num67).Trim();
						string code8 = text29 + "+" + text4.Substring(num67 + 2).Trim();
						foreach (var item75 in list6)
						{
							if (!(item75.Item1 == text29))
							{
								continue;
							}
							flag6 = true;
							long item31 = long.Parse(code8.StrCalc(list7, list6));
							for (int num74 = 0; num74 < list6.Count; num74++)
							{
								if (list6[num74].Item1 == text29)
								{
									int item32 = list6[num74].Item3;
									list6.RemoveAt(num74);
									list6.Insert(num74, (text29, item31, item32));
									break;
								}
							}
							break;
						}
						foreach (var item76 in list7)
						{
							if (flag6 || !(item76.Item1 == text29))
							{
								continue;
							}
							flag6 = true;
							decimal item33 = decimal.Parse(code8.StrCalc(list7, list6, null, null, isDecimal: true));
							for (int num75 = 0; num75 < list7.Count; num75++)
							{
								if (list7[num75].Item1 == text29)
								{
									int item34 = list7[num75].Item3;
									list7.RemoveAt(num75);
									list7.Insert(num75, (text29, item33, item34));
									break;
								}
							}
							break;
						}
						foreach (var item77 in list8)
						{
							if (flag6 || !(item77.Item1 == text29))
							{
								continue;
							}
							flag6 = true;
							string item35 = code8.StrCalc(list7, list6, list8);
							for (int num76 = 0; num76 < list8.Count; num76++)
							{
								if (list8[num76].Item1 == text29)
								{
									int item36 = list8[num76].Item3;
									list8.RemoveAt(num76);
									list8.Insert(num76, (text29, item35, item36));
									break;
								}
							}
							break;
						}
					}
					num67 = text4.IndexOf("-=");
					if (num67 != -1)
					{
						if (ch552Analyze)
						{
							ch552Program.Add(text4 + ";");
						}
						string text30 = text4.Substring(0, num67).Trim();
						string code9 = text30 + "-" + text4.Substring(num67 + 2).Trim();
						foreach (var item78 in list6)
						{
							if (!(item78.Item1 == text30))
							{
								continue;
							}
							flag6 = true;
							long item37 = long.Parse(code9.StrCalc(list7, list6));
							for (int num77 = 0; num77 < list6.Count; num77++)
							{
								if (list6[num77].Item1 == text30)
								{
									int item38 = list6[num77].Item3;
									list6.RemoveAt(num77);
									list6.Insert(num77, (text30, item37, item38));
									break;
								}
							}
							break;
						}
						foreach (var item79 in list7)
						{
							if (flag6 || !(item79.Item1 == text30))
							{
								continue;
							}
							flag6 = true;
							decimal item39 = decimal.Parse(code9.StrCalc(list7, list6, null, null, isDecimal: true));
							for (int num78 = 0; num78 < list7.Count; num78++)
							{
								if (list7[num78].Item1 == text30)
								{
									int item40 = list7[num78].Item3;
									list7.RemoveAt(num78);
									list7.Insert(num78, (text30, item39, item40));
									break;
								}
							}
							break;
						}
					}
					num67 = text4.IndexOf('=');
					if (num67 != -1)
					{
						if (ch552Analyze)
						{
							ch552Program.Add(text4 + ";");
						}
						string text31 = text4.Substring(0, num67).Trim();
						string code10 = text4.Substring(num67 + 1).Trim();
						foreach (var item80 in list6)
						{
							if (!(item80.Item1 == text31))
							{
								continue;
							}
							flag6 = true;
							long item41 = long.Parse(code10.StrCalc(list7, list6));
							for (int num79 = 0; num79 < list6.Count; num79++)
							{
								if (list6[num79].Item1 == text31)
								{
									int item42 = list6[num79].Item3;
									list6.RemoveAt(num79);
									list6.Insert(num79, (text31, item41, item42));
									break;
								}
							}
							break;
						}
						foreach (var item81 in list7)
						{
							if (flag6 || !(item81.Item1 == text31))
							{
								continue;
							}
							flag6 = true;
							decimal item43 = decimal.Parse(code10.StrCalc(list7, list6, null, null, isDecimal: true));
							for (int num80 = 0; num80 < list7.Count; num80++)
							{
								if (list7[num80].Item1 == text31)
								{
									int item44 = list7[num80].Item3;
									list7.RemoveAt(num80);
									list7.Insert(num80, (text31, item43, item44));
									break;
								}
							}
							break;
						}
						foreach (var item82 in list8)
						{
							if (flag6 || !(item82.Item1 == text31))
							{
								continue;
							}
							flag6 = true;
							string item45 = code10.StrCmp(list7, list6, list8, list9);
							for (int num81 = 0; num81 < list8.Count; num81++)
							{
								if (list8[num81].Item1 == text31)
								{
									int item46 = list8[num81].Item3;
									list8.RemoveAt(num81);
									list8.Insert(num81, (text31, item45, item46));
									if (ch552Analyze)
									{
										ch552Program.RemoveAt(ch552Program.Count - 1);
									}
									break;
								}
							}
							break;
						}
						foreach (var item83 in list9)
						{
							if (flag6 || !(item83.Item1 == text31))
							{
								continue;
							}
							flag6 = true;
							bool item47 = code10.StrCmp(list7, list6, list8, list9) == "True";
							for (int num82 = 0; num82 < list9.Count; num82++)
							{
								if (list9[num82].Item1 == text31)
								{
									int item48 = list9[num82].Item3;
									list9.RemoveAt(num82);
									list9.Insert(num82, (text31, item47, item48));
									break;
								}
							}
							break;
						}
					}
				}
				num12 = ((!flag6) ? (num12 + 1) : (num12 + 1));
				continue;
				end_IL_04cd:
				break;
			}
		}
		catch (Exception value3)
		{
			if (Running && !Cancel)
			{
				Console.WriteLine(value3);
			}
		}
		if (ch552Analyze)
		{
			ch552Program.Add("delay(0.04);");
			ch552Program.Add("releaseButton(0xFFFF);");
			ch552Program.Add("pressHatButton(8);");
			ch552Program.Add("moveLeftStick(0, 0);");
			ch552Program.Add("moveRightStick(0, 0);");
			ch552Program.Add("delay(0.04);");
		}
	}

	public void NmcExecution(int startPos = 0)
	{
		RunningCsx = false;
		baseNmc = this;
		Running = true;
		Cancel = false;
		SubRunningNmc = "";
		counter_ = 0;
		Task.Factory.StartNew(delegate
		{
			GlobalVar.NmcThread = Thread.CurrentThread;
			NmcCodeExecution(this, isMain: true, startPos);
			NmcEndSec();
		});
	}

	public void NmcEndSec()
	{
		NmcKeyFlag = 9259542121117908992uL;
		NmcHoldKeyFlag = 9259542121117908992uL;
		NxControllerInterface.ButtonFlag = 0u;
		NxControllerInterface.Hat = 8u;
		NxControllerInterface.LX = 128u;
		NxControllerInterface.LY = 128u;
		NxControllerInterface.RX = 128u;
		NxControllerInterface.RY = 128u;
		NxControllerInterface.AllReleaseKeyboard();
		if (IsMain)
		{
			GlobalVar.HighLightLine = -1;
			GlobalVar.HighLightLineSet = true;
			GlobalVar.TaskName[4] = "";
			GlobalVar.TaskName[5] = "";
			GlobalVar.MAINFORM.TaskView();
			GlobalVar.MAINFORM.UpdateHighLight();
		}
		NxCommand.ExitFlag = false;
		Running = false;
		Cancel = false;
		RunningCsx = false;
	}

	public string GetDataPath(string path)
	{
		return Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath) + "\\" + path;
	}

	private string _getResourcePath(string path)
	{
		if (path.Length > 2 && path[0] == 'R' && path[1] == '"' && path[path.Length - 1] == '"')
		{
			path = path.Substring(2, path.Length - 3);
			return Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath) + "\\" + path;
		}
		throw new Exception();
	}

	private Script<object> _createRunCsScript(string code)
	{
		try
		{
			List<Assembly> references = new List<Assembly>
			{
				Assembly.LoadFrom(GlobalVar.BasePath + "NxInterface.dll"),
				Assembly.LoadFrom(GlobalVar.BasePath + "OpenCvSharp.dll"),
				Assembly.LoadFrom(GlobalVar.BasePath + "OpenCvSharp.Extensions.dll")
			};
			List<string> imports = new List<string>();
			string text = Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath) + "\\";
			ScriptSourceResolver resolver = ScriptSourceResolver.Default.WithBaseDirectory(text);
			ScriptMetadataResolver resolver2 = ScriptMetadataResolver.Default.WithBaseDirectory(text);
			ScriptOptions options = ScriptOptions.Default.AddReferences(references).AddImports(imports).WithSourceResolver(resolver)
				.WithMetadataResolver(resolver2)
				.WithOptimizationLevel(OptimizationLevel.Release)
				.WithAllowUnsafe(allowUnsafe: true)
				.WithFilePath(text);
			return CSharpScript.Create(code, options, typeof(GlobalParams));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	private void _runCsScript(string code, string[] args = null)
	{
		try
		{
			Environment.CurrentDirectory = Path.GetDirectoryName(AllPath) + "\\" + Path.GetFileNameWithoutExtension(AllPath) + "\\";
			Script<object> script = _createRunCsScript(code);
			GlobalVar.MAINFORM.Nmc.RunningCsx = true;
			script.RunAsync(new GlobalParams(args));
		}
		catch (Exception ex)
		{
			GlobalVar.MAINFORM.Nmc.RunningCsx = false;
			if (ex.InnerException.ToString() != "System.Threading.ThreadAbortException" && Running && !Cancel)
			{
				Console.WriteLine(ex);
			}
		}
		GlobalVar.MAINFORM.Nmc.RunningCsx = false;
		Environment.CurrentDirectory = GlobalVar.BasePath;
	}

	public static List<string> AsmBaseSplit(string asmBase)
	{
		List<string> list = new List<string>();
		bool flag = false;
		int num = 0;
		string text = "";
		bool flag2 = false;
		for (int i = 0; i < asmBase.Length; i++)
		{
			char c = asmBase[i];
			if (c == ',')
			{
				if (flag || num != 0)
				{
					text += c;
					continue;
				}
				list.Add(text.Trim());
				text = "";
				continue;
			}
			text += c;
			if (c == '"' && !flag2)
			{
				flag = !flag;
			}
			else if (!flag && c == '(')
			{
				num++;
			}
			else if (!flag && c == ')')
			{
				num--;
			}
			flag2 = false;
			if (c == '\\')
			{
				flag2 = true;
			}
		}
		list.Add(text.Trim());
		return list;
	}

	public static string[] AsmSplit(string asm)
	{
		try
		{
			string text = asm.Substring(0, asm.IndexOf("("));
			List<string> list = AsmBaseSplit(asm.Substring(asm.IndexOf("(") + 1, asm.LastIndexOf(")") - asm.IndexOf("(") - 1));
			if (list.Count == 1 && list[0] == "")
			{
				list.Clear();
			}
			list.Insert(0, text.Trim());
			return list.ToArray();
		}
		catch
		{
			return new string[1] { asm.Trim() };
		}
	}

	public void ScrKeyboardMode(string text, string currentMode = "")
	{
		switch (currentMode)
		{
		case "":
			switch (text)
			{
			case "HIRAGANA":
				pushSpecialKeyboard('\u0088');
				if (baseNmc.Cancel)
				{
					break;
				}
				pushSpecialKeyboard('\u0088');
				if (!baseNmc.Cancel)
				{
					pushSpecialKeyboard('5');
					if (!baseNmc.Cancel)
					{
						pushSpecialKeyboard('\u0088');
					}
				}
				break;
			case "KATAKANA":
				pushSpecialKeyboard('\u0088');
				if (baseNmc.Cancel)
				{
					break;
				}
				pushSpecialKeyboard('\u0088');
				if (baseNmc.Cancel)
				{
					break;
				}
				pushSpecialKeyboard('5');
				if (!baseNmc.Cancel)
				{
					pushSpecialKeyboard('\u0088');
					if (!baseNmc.Cancel)
					{
						pushSpecialKeyboard('\u0088');
					}
				}
				break;
			case "ALPHANUMERIC":
				pushSpecialKeyboard('\u0088');
				if (!baseNmc.Cancel)
				{
					pushSpecialKeyboard('\u0088');
					if (!baseNmc.Cancel)
					{
						pushSpecialKeyboard('5');
					}
				}
				break;
			}
			break;
		case "HIRAGANA":
			if (text == "KATAKANA")
			{
				pushSpecialKeyboard('\u0088');
			}
			else if (text == "ALPHANUMERIC")
			{
				pushSpecialKeyboard('5');
			}
			break;
		case "KATAKANA":
			if (text == "HIRAGANA")
			{
				pushSpecialKeyboard('\u0088');
			}
			else if (text == "ALPHANUMERIC")
			{
				pushSpecialKeyboard('5');
			}
			break;
		case "ALPHANUMERIC":
			if (text == "HIRAGANA")
			{
				pushSpecialKeyboard('\u0088');
			}
			else if (text == "KATAKANA")
			{
				pushSpecialKeyboard('\u0088');
				if (!baseNmc.Cancel)
				{
					pushSpecialKeyboard('\u0088');
				}
			}
			break;
		}
	}

	public void ScrKeyboard(string text)
	{
		int length = text.Length;
		string currentMode = "";
		for (int i = 0; i < length; i++)
		{
			string inputMode = KeyboardUtil.GetInputMode(text[i]);
			if (baseNmc.Cancel)
			{
				break;
			}
			ScrKeyboardMode(inputMode, currentMode);
			currentMode = inputMode;
			if (baseNmc.Cancel)
			{
				break;
			}
			string text2 = KeyboardUtil.RomajiConvert(text[i]);
			foreach (char key in text2)
			{
				pushKeyboard(key);
				if (baseNmc.Cancel)
				{
					break;
				}
			}
			if (baseNmc.Cancel)
			{
				break;
			}
		}
	}

	private void waitFunc(int wait, Stopwatch sw = null)
	{
		try
		{
			if (sw == null)
			{
				sw = scrTimer;
			}
			if (sw.IsRunning)
			{
				sw.Reset();
			}
			sw.Restart();
			while (sw.ElapsedMilliseconds < wait && !baseNmc.Cancel)
			{
				NxCommand.Wait(0.001);
			}
			sw.Reset();
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void pushKeyboard(char key)
	{
		pressKeyboard(key);
		if (!baseNmc.Cancel)
		{
			releaseKeyboard(key);
		}
	}

	private void pushSpecialKeyboard(char key)
	{
		pressSpecialKeyboard(key);
		if (!baseNmc.Cancel)
		{
			releaseSpecialKeyboard(key);
		}
	}

	private void pressKeyboard(char key)
	{
		if (ch552Analyze)
		{
			ch552Program.Add($"pressKey('{key}');");
			return;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		NxControllerInterface.PressKeyboard(key);
		waitFunc(30);
	}

	private void releaseKeyboard(char key)
	{
		if (ch552Analyze)
		{
			ch552Program.Add($"releaseKey('{key}');");
			return;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		NxControllerInterface.ReleaseKeyboard(key);
		waitFunc(30);
	}

	private void pressSpecialKeyboard(char key)
	{
		if (ch552Analyze)
		{
			ch552Program.Add($"pressSpecialKey('{key}');");
			return;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		NxControllerInterface.PressSpecialKeyboard(key);
		waitFunc(40);
	}

	private void releaseSpecialKeyboard(char key)
	{
		if (ch552Analyze)
		{
			ch552Program.Add($"releaseSpecialKey('{key}');");
			return;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		NxControllerInterface.ReleaseSpecialKeyboard(key);
		waitFunc(40);
	}

	public bool ScrImgCmpRect(string[] args, bool is720p = false, bool isGray = false)
	{
		if (ch552Analyze)
		{
			return false;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		try
		{
			Bitmap template = new Bitmap(exeResourcesImages.Where((ResourcesImage _) => _.label == args[1]).ToArray()[0].image);
			Bitmap bitmap;
			lock (NxCommand.lockObject)
			{
				bitmap = NxCommand.GetCapture();
				if (is720p)
				{
					bitmap = (Bitmap)bitmap.ImageResize(1280, 720);
				}
			}
			Rectangle rect = new Rectangle(int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[4]), int.Parse(args[5]));
			bitmap = bitmap.Clone(rect, bitmap.PixelFormat);
			if (args.Length < 7)
			{
				return NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray);
			}
			if (scrTimer.IsRunning)
			{
				scrTimer.Reset();
			}
			scrTimer.Restart();
			int num = (int)(1000m * decimal.Parse(decimal.Parse(args[6]).ToString("F2")));
			if (num != 0)
			{
				while (scrTimer.ElapsedMilliseconds < num && !baseNmc.Cancel)
				{
					try
					{
						bool flag = false;
						if ((args.Length >= 8) ? NxCommand.IsContainTemplate(bitmap, template, double.Parse(args[7]), isGray) : NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray))
						{
							scrTimer.Reset();
							return true;
						}
					}
					catch (Exception)
					{
					}
					lock (NxCommand.lockObject)
					{
						bitmap = NxCommand.GetCapture();
						if (is720p)
						{
							bitmap = (Bitmap)bitmap.ImageResize(1280, 720);
						}
						bitmap = bitmap.Clone(rect, bitmap.PixelFormat);
					}
				}
				scrTimer.Reset();
				return false;
			}
			bool flag2 = false;
			flag2 = ((args.Length >= 8) ? NxCommand.IsContainTemplate(bitmap, template, double.Parse(args[7]), isGray) : NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray));
			scrTimer.Reset();
			return flag2;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool ScrImgCmp(string[] args, bool is720p = false, bool isGray = false)
	{
		if (ch552Analyze)
		{
			return false;
		}
		baseNmc.NmcKeyFlag = 9259542121117908992uL;
		try
		{
			Bitmap template = new Bitmap(exeResourcesImages.Where((ResourcesImage _) => _.label == args[1]).ToArray()[0].image);
			Bitmap bitmap;
			lock (NxCommand.lockObject)
			{
				bitmap = NxCommand.GetCapture();
				if (is720p)
				{
					bitmap = (Bitmap)bitmap.ImageResize(1280, 720);
				}
			}
			if (args.Length < 3)
			{
				return NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray);
			}
			if (scrTimer.IsRunning)
			{
				scrTimer.Reset();
			}
			scrTimer.Restart();
			int num = (int)(1000m * decimal.Parse(decimal.Parse(args[2]).ToString("F2")));
			if (num != 0)
			{
				while (scrTimer.ElapsedMilliseconds < num && !baseNmc.Cancel)
				{
					try
					{
						bool flag = false;
						if ((args.Length >= 4) ? NxCommand.IsContainTemplate(bitmap, template, double.Parse(args[3]), isGray) : NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray))
						{
							scrTimer.Reset();
							return true;
						}
					}
					catch (Exception)
					{
					}
					lock (NxCommand.lockObject)
					{
						bitmap = NxCommand.GetCapture();
						if (is720p)
						{
							bitmap = (Bitmap)bitmap.ImageResize(1280, 720);
						}
					}
				}
				scrTimer.Reset();
				return false;
			}
			bool flag2 = false;
			flag2 = ((args.Length >= 4) ? NxCommand.IsContainTemplate(bitmap, template, double.Parse(args[3]), isGray) : NxCommand.IsContainTemplate(bitmap, template, 0.8, isGray));
			scrTimer.Reset();
			return flag2;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void ScrWait(string[] args)
	{
		int wait = (int)(1000m * decimal.Parse(decimal.Parse(args[^1]).ToString("F2")));
		if (!ch552Analyze)
		{
			baseNmc.NmcKeyFlag = 9259542121117908992uL;
		}
		waitFunc(wait);
	}

	public bool ScrWaitRumble(string[] args)
	{
		if (ch552Analyze)
		{
			return false;
		}
		bool result = false;
		if (args.Length >= 2)
		{
			try
			{
				if (scrTimer.IsRunning)
				{
					scrTimer.Reset();
				}
				scrTimer.Restart();
				int num = (int)(1000m * decimal.Parse(decimal.Parse(args[^1]).ToString("F2")));
				baseNmc.NmcKeyFlag = 9259542121117908992uL;
				while (scrTimer.ElapsedMilliseconds < num && !baseNmc.Cancel)
				{
					if (NxControllerInterface.GetRumble())
					{
						result = true;
						break;
					}
					NxCommand.Wait(0.001);
				}
				scrTimer.Reset();
			}
			catch
			{
				result = NxControllerInterface.GetRumble();
			}
		}
		else
		{
			result = NxControllerInterface.GetRumble();
		}
		return result;
	}

	public void ScrSnipping(string[] args)
	{
		if (!ch552Analyze)
		{
			GlobalVar.MAINFORM.Snipping(int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[4]));
		}
	}

	private bool _isKeyCommand(string arg)
	{
		arg = arg.Trim();
		switch (arg)
		{
		case "A":
			return true;
		case "B":
			return true;
		case "X":
			return true;
		case "Y":
			return true;
		case "ZL":
			return true;
		case "ZR":
			return true;
		case "L":
			return true;
		case "R":
			return true;
		case "DOWN":
			return true;
		case "UP":
			return true;
		case "RIGHT":
			return true;
		case "DOWNRIGHT":
			return true;
		case "UPRIGHT":
			return true;
		case "LEFT":
			return true;
		case "DOWNLEFT":
			return true;
		case "UPLEFT":
			return true;
		case "START":
			return true;
		case "SELECT":
			return true;
		case "HOME":
			return true;
		case "CAPTURE":
			return true;
		case "CLICK_L":
			return true;
		case "CLICK_R":
			return true;
		case "UP_L":
			return true;
		default:
			if (arg.Substring(0, 3) == "LS(")
			{
				return true;
			}
			if (arg.Substring(0, 3) == "RS(")
			{
				return true;
			}
			return arg switch
			{
				"DOWN_L" => true, 
				"LEFT_L" => true, 
				"UPLEFT_L" => true, 
				"DOWNLEFT_L" => true, 
				"RIGHT_L" => true, 
				"UPRIGHT_L" => true, 
				"DOWNRIGHT_L" => true, 
				"UP_R" => true, 
				"DOWN_R" => true, 
				"LEFT_R" => true, 
				"UPLEFT_R" => true, 
				"DOWNLEFT_R" => true, 
				"RIGHT_R" => true, 
				"UPRIGHT_R" => true, 
				"DOWNRIGHT_R" => true, 
				_ => false, 
			};
		}
	}

	private void _setCH552PressCommand(string[] args, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null)
	{
		int num = 0;
		int num2 = 8;
		string text = "";
		string text2 = "1.0";
		string text3 = "";
		string text4 = "1.0";
		for (int i = 1; i < args.Length - 1; i++)
		{
			if (args[i] == "A")
			{
				num |= 4;
			}
			else if (args[i] == "B")
			{
				num |= 2;
			}
			else if (args[i] == "X")
			{
				num |= 8;
			}
			else if (args[i] == "Y")
			{
				num |= 1;
			}
			else if (args[i] == "ZL")
			{
				num |= 0x40;
			}
			else if (args[i] == "ZR")
			{
				num |= 0x80;
			}
			else if (args[i] == "L")
			{
				num |= 0x10;
			}
			else if (args[i] == "R")
			{
				num |= 0x20;
			}
			else if (args[i] == "DOWN")
			{
				num2 = 4;
			}
			else if (args[i] == "UP")
			{
				num2 = 0;
			}
			else if (args[i] == "RIGHT")
			{
				num2 = 2;
			}
			else if (args[i] == "DOWNRIGHT")
			{
				num2 = 3;
			}
			else if (args[i] == "UPRIGHT")
			{
				num2 = 1;
			}
			else if (args[i] == "LEFT")
			{
				num2 = 6;
			}
			else if (args[i] == "DOWNLEFT")
			{
				num2 = 5;
			}
			else if (args[i] == "UPLEFT")
			{
				num2 = 7;
			}
			else if (args[i] == "START")
			{
				num |= 0x200;
			}
			else if (args[i] == "SELECT")
			{
				num |= 0x100;
			}
			else if (args[i] == "HOME")
			{
				num |= 0x1000;
			}
			else if (args[i] == "CAPTURE")
			{
				num |= 0x2000;
			}
			else if (args[i] == "CLICK_L")
			{
				num |= 0x400;
			}
			else if (args[i] == "CLICK_R")
			{
				num |= 0x800;
			}
			else if (args[i] == "UP_L")
			{
				text = "270.0";
			}
			else if (args[i].Substring(0, 3) == "LS(")
			{
				string[] array = AsmSplit(args[i]);
				text = array[1];
				text2 = array[2];
			}
			else if (args[i].Substring(0, 3) == "RS(")
			{
				string[] array2 = AsmSplit(args[i]);
				text3 = array2[1];
				text4 = array2[2];
			}
			else if (args[i] == "DOWN_L")
			{
				text = "90.0";
			}
			else if (args[i] == "LEFT_L")
			{
				text = "180.0";
			}
			else if (args[i] == "UPLEFT_L")
			{
				text = "225.0";
			}
			else if (args[i] == "DOWNLEFT_L")
			{
				text = "135.0";
			}
			else if (args[i] == "RIGHT_L")
			{
				text = "0.0";
			}
			else if (args[i] == "UPRIGHT_L")
			{
				text = "315.0";
			}
			else if (args[i] == "DOWNRIGHT_L")
			{
				text = "45.0";
			}
			else if (args[i] == "UP_R")
			{
				text3 = "270.0";
			}
			else if (args[i] == "DOWN_R")
			{
				text3 = "90.0";
			}
			else if (args[i] == "LEFT_R")
			{
				text3 = "180.0";
			}
			else if (args[i] == "UPLEFT_R")
			{
				text3 = "225.0";
			}
			else if (args[i] == "DOWNLEFT_R")
			{
				text3 = "135.0";
			}
			else if (args[i] == "RIGHT_R")
			{
				text3 = "0.0";
			}
			else if (args[i] == "UPRIGHT_R")
			{
				text3 = "315.0";
			}
			else if (args[i] == "DOWNRIGHT_R")
			{
				text3 = "45.0";
			}
		}
		if (num != 0)
		{
			ch552Program.Add($"pressButton({num});");
		}
		if (num2 != 8)
		{
			ch552Program.Add($"pressHatButton({num2});");
		}
		if (text != "")
		{
			ch552Program.Add("moveLeftStick(" + text + ", " + text2 + ");");
		}
		if (text3 != "")
		{
			ch552Program.Add("moveRightStick(" + text3 + ", " + text4 + ");");
		}
		ch552Program.Add("sendReport();");
		if (!_isKeyCommand(args[^2]))
		{
			ch552Program.Add("delay(" + args[^2] + ");");
			if (num != 0)
			{
				ch552Program.Add($"releaseButton({num});");
			}
			if (num2 != 8)
			{
				ch552Program.Add("pressHatButton(8);");
			}
			if (text != "")
			{
				ch552Program.Add("moveLeftStick(0, 0);");
			}
			if (text3 != "")
			{
				ch552Program.Add("moveRightStick(0, 0);");
			}
			ch552Program.Add("sendReport();");
			ch552Program.Add("delay(" + args[^1] + ");");
		}
		else
		{
			ch552Program.Add("delay(" + args[^1] + ");");
			if (num != 0)
			{
				ch552Program.Add($"releaseButton({num});");
			}
			if (num2 != 8)
			{
				ch552Program.Add("pressHatButton(8);");
			}
			if (text != "")
			{
				ch552Program.Add("moveLeftStick(0, 0);");
			}
			if (text3 != "")
			{
				ch552Program.Add("moveRightStick(0, 0);");
			}
			ch552Program.Add("sendReport();");
		}
	}

	private void _setCH552HoldCommand(string[] args, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null, bool isRelease = false)
	{
		int num = 0;
		int num2 = 8;
		string text = "";
		string text2 = "1.0";
		string text3 = "";
		string text4 = "1.0";
		for (int i = 1; i < args.Length; i++)
		{
			if (args[i] == "A")
			{
				num |= 4;
			}
			else if (args[i] == "B")
			{
				num |= 2;
			}
			else if (args[i] == "X")
			{
				num |= 8;
			}
			else if (args[i] == "Y")
			{
				num |= 1;
			}
			else if (args[i] == "ZL")
			{
				num |= 0x40;
			}
			else if (args[i] == "ZR")
			{
				num |= 0x80;
			}
			else if (args[i] == "L")
			{
				num |= 0x10;
			}
			else if (args[i] == "R")
			{
				num |= 0x20;
			}
			else if (args[i] == "DOWN")
			{
				num2 = 4;
			}
			else if (args[i] == "UP")
			{
				num2 = 0;
			}
			else if (args[i] == "RIGHT")
			{
				num2 = 2;
			}
			else if (args[i] == "DOWNRIGHT")
			{
				num2 = 3;
			}
			else if (args[i] == "UPRIGHT")
			{
				num2 = 1;
			}
			else if (args[i] == "LEFT")
			{
				num2 = 6;
			}
			else if (args[i] == "DOWNLEFT")
			{
				num2 = 5;
			}
			else if (args[i] == "UPLEFT")
			{
				num2 = 7;
			}
			else if (args[i] == "START")
			{
				num |= 0x200;
			}
			else if (args[i] == "SELECT")
			{
				num |= 0x100;
			}
			else if (args[i] == "HOME")
			{
				num |= 0x1000;
			}
			else if (args[i] == "CAPTURE")
			{
				num |= 0x2000;
			}
			else if (args[i] == "CLICK_L")
			{
				num |= 0x400;
			}
			else if (args[i] == "CLICK_R")
			{
				num |= 0x800;
			}
			else if (args[i] == "UP_L")
			{
				text = "270.0";
			}
			else if (args[i].Substring(0, 3) == "LS(")
			{
				string[] array = AsmSplit(args[i]);
				text = array[1];
				text2 = array[2];
			}
			else if (args[i].Substring(0, 3) == "RS(")
			{
				string[] array2 = AsmSplit(args[i]);
				text3 = array2[1];
				text4 = array2[2];
			}
			else if (args[i] == "DOWN_L")
			{
				text = "90.0";
			}
			else if (args[i] == "LEFT_L")
			{
				text = "180.0";
			}
			else if (args[i] == "UPLEFT_L")
			{
				text = "225.0";
			}
			else if (args[i] == "DOWNLEFT_L")
			{
				text = "135.0";
			}
			else if (args[i] == "RIGHT_L")
			{
				text = "0.0";
			}
			else if (args[i] == "UPRIGHT_L")
			{
				text = "315.0";
			}
			else if (args[i] == "DOWNRIGHT_L")
			{
				text = "45.0";
			}
			else if (args[i] == "UP_R")
			{
				text3 = "270.0";
			}
			else if (args[i] == "DOWN_R")
			{
				text3 = "90.0";
			}
			else if (args[i] == "LEFT_R")
			{
				text3 = "180.0";
			}
			else if (args[i] == "UPLEFT_R")
			{
				text3 = "225.0";
			}
			else if (args[i] == "DOWNLEFT_R")
			{
				text3 = "135.0";
			}
			else if (args[i] == "RIGHT_R")
			{
				text3 = "0.0";
			}
			else if (args[i] == "UPRIGHT_R")
			{
				text3 = "315.0";
			}
			else if (args[i] == "DOWNRIGHT_R")
			{
				text3 = "45.0";
			}
		}
		if (isRelease)
		{
			if (num != 0)
			{
				ch552Program.Add($"pressButton({num});");
			}
			if (num2 != 8)
			{
				ch552Program.Add($"pressHatButton({num2});");
			}
			if (text != "")
			{
				ch552Program.Add("moveLeftStick(" + text + ", " + text2 + ");");
			}
			if (text3 != "")
			{
				ch552Program.Add("moveRightStick(" + text3 + ", " + text4 + ");");
			}
			ch552Program.Add("sendReport();");
		}
		else
		{
			if (num != 0)
			{
				ch552Program.Add($"releaseButton({num});");
			}
			if (num2 != 8)
			{
				ch552Program.Add("pressHatButton(8);");
			}
			if (text != "")
			{
				ch552Program.Add("moveLeftStick(0, 0);");
			}
			if (text3 != "")
			{
				ch552Program.Add("moveRightStick(0, 0);");
			}
			ch552Program.Add("sendReport();");
		}
	}

	private void ScrHold(string[] args, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null, bool isRelease = false)
	{
		ulong num = NmcHoldKeyFlag;
		for (int i = 1; i < args.Length; i++)
		{
			if (isRelease)
			{
				if (args[i] == "A")
				{
					num &= 0xFFFFFFFFFFFFFFF7uL;
				}
				else if (args[i] == "B")
				{
					num &= 0xFFFFFFFFFFFFFFFBuL;
				}
				else if (args[i] == "X")
				{
					num &= 0xFFFFFFFFFFFFFFFDuL;
				}
				else if (args[i] == "Y")
				{
					num &= 0xFFFFFFFFFFFFFFFEuL;
				}
				else if (args[i] == "ZL")
				{
					num &= 0xFFFFFFFFFF7FFFFFuL;
				}
				else if (args[i] == "ZR")
				{
					num &= 0xFFFFFFFFFFFFFF7FuL;
				}
				else if (args[i] == "L")
				{
					num &= 0xFFFFFFFFFFBFFFFFuL;
				}
				else if (args[i] == "R")
				{
					num &= 0xFFFFFFFFFFFFFFBFuL;
				}
				else if (args[i] == "DOWN")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "UP")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "RIGHT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "DOWNRIGHT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "UPRIGHT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "LEFT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "DOWNLEFT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "UPLEFT")
				{
					num &= 0xFFFFFFFFFFF0FFFFuL;
				}
				else if (args[i] == "START")
				{
					num &= 0xFFFFFFFFFFFFFDFFuL;
				}
				else if (args[i] == "SELECT")
				{
					num &= 0xFFFFFFFFFFFFFEFFuL;
				}
				else if (args[i] == "HOME")
				{
					num &= 0xFFFFFFFFFFFFEFFFuL;
				}
				else if (args[i] == "CAPTURE")
				{
					num &= 0xFFFFFFFFFFFFDFFFuL;
				}
				else if (args[i] == "CLICK_L")
				{
					num &= 0xFFFFFFFFFFFFF7FFuL;
				}
				else if (args[i] == "CLICK_R")
				{
					num &= 0xFFFFFFFFFFFFFBFFuL;
				}
				else if (args[i] == "UP_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i].Substring(0, 3) == "LS(")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i].Substring(0, 3) == "RS(")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "DOWN_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "LEFT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "UPLEFT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "DOWNLEFT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "RIGHT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "UPRIGHT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "DOWNRIGHT_L")
				{
					num &= 0xFFFF0000FFFFFFFFuL;
					num |= 0x808000000000L;
				}
				else if (args[i] == "UP_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "DOWN_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "LEFT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "UPLEFT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "DOWNLEFT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "RIGHT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "UPRIGHT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
				else if (args[i] == "DOWNRIGHT_R")
				{
					num &= 0xFFFFFFFFFFFFL;
					num |= 0x8080000000000000uL;
				}
			}
			else if (args[i] == "A")
			{
				num |= 8;
			}
			else if (args[i] == "B")
			{
				num |= 4;
			}
			else if (args[i] == "X")
			{
				num |= 2;
			}
			else if (args[i] == "Y")
			{
				num |= 1;
			}
			else if (args[i] == "ZL")
			{
				num |= 0x800000;
			}
			else if (args[i] == "ZR")
			{
				num |= 0x80;
			}
			else if (args[i] == "L")
			{
				num |= 0x400000;
			}
			else if (args[i] == "R")
			{
				num |= 0x40;
			}
			else if (args[i] == "DOWN")
			{
				num |= 0x10000;
			}
			else if (args[i] == "UP")
			{
				num |= 0x20000;
			}
			else if (args[i] == "RIGHT")
			{
				num |= 0x40000;
			}
			else if (args[i] == "DOWNRIGHT")
			{
				num |= 0x50000;
			}
			else if (args[i] == "UPRIGHT")
			{
				num |= 0x60000;
			}
			else if (args[i] == "LEFT")
			{
				num |= 0x80000;
			}
			else if (args[i] == "DOWNLEFT")
			{
				num |= 0x90000;
			}
			else if (args[i] == "UPLEFT")
			{
				num |= 0xA0000;
			}
			else if (args[i] == "START")
			{
				num |= 0x200;
			}
			else if (args[i] == "SELECT")
			{
				num |= 0x100;
			}
			else if (args[i] == "HOME")
			{
				num |= 0x1000;
			}
			else if (args[i] == "CAPTURE")
			{
				num |= 0x2000;
			}
			else if (args[i] == "CLICK_L")
			{
				num |= 0x800;
			}
			else if (args[i] == "CLICK_R")
			{
				num |= 0x400;
			}
			else if (args[i] == "UP_L")
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0;
			}
			else if (args[i].Substring(0, 3) == "LS(")
			{
				string[] array = AsmSplit(args[i]);
				array[1] = array[1].StrCalc(varDatas, varDatas2);
				array[2] = array[2].StrCalc(varDatas, varDatas2);
				double num2 = double.Parse(array[1]) % 360.0;
				double num3 = Math.Min(1.0, double.Parse(array[2]));
				ulong num4 = (ulong)Math.Ceiling(127.5 * Math.Cos(num2 * (Math.PI / 180.0)) * num3 + 127.5);
				ulong num5 = (ulong)Math.Floor(127.5 * Math.Sin(num2 * (Math.PI / 180.0)) * num3 + 127.5);
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= 0 | (num5 << 32) | (num4 << 40);
			}
			else if (args[i].Substring(0, 3) == "RS(")
			{
				string[] array2 = AsmSplit(args[i]);
				array2[1] = array2[1].StrCalc(varDatas, varDatas2);
				array2[2] = array2[2].StrCalc(varDatas, varDatas2);
				double num6 = double.Parse(array2[1]) % 360.0;
				double num7 = Math.Min(1.0, double.Parse(array2[2]));
				ulong num8 = (ulong)Math.Ceiling(127.5 * Math.Cos(num6 * (Math.PI / 180.0)) * num7 + 127.5);
				ulong num9 = (ulong)Math.Floor(127.5 * Math.Sin(num6 * (Math.PI / 180.0)) * num7 + 127.5);
				num &= 0xFFFFFFFFFFFFL;
				num |= 0 | (num9 << 48) | (num8 << 56);
			}
			else if (args[i] == "DOWN_L")
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0xFF00000000L;
			}
			else if (args[i] == "LEFT_L")
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0;
			}
			else if (args[i] == "UPLEFT_L")
			{
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= 0;
			}
			else if (args[i] == "DOWNLEFT_L")
			{
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= 0xFF00000000L;
			}
			else if (args[i] == "RIGHT_L")
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0xFF0000000000L;
			}
			else if (args[i] == "UPRIGHT_L")
			{
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= 0xFF0000000000L;
			}
			else if (args[i] == "DOWNRIGHT_L")
			{
				num &= 0xFFFF0000FFFFFFFFuL;
				num |= 0xFFFF00000000L;
			}
			else if (args[i] == "UP_R")
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0;
			}
			else if (args[i] == "DOWN_R")
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0xFF000000000000L;
			}
			else if (args[i] == "LEFT_R")
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0;
			}
			else if (args[i] == "UPLEFT_R")
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= 0;
			}
			else if (args[i] == "DOWNLEFT_R")
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= 0xFF000000000000L;
			}
			else if (args[i] == "RIGHT_R")
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0xFF00000000000000uL;
			}
			else if (args[i] == "UPRIGHT_R")
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= 0xFF00000000000000uL;
			}
			else if (args[i] == "DOWNRIGHT_R")
			{
				num &= 0xFFFFFFFFFFFFL;
				num |= 0xFFFF000000000000uL;
			}
		}
		if (!ch552Analyze)
		{
			baseNmc.NmcHoldKeyFlag = num;
		}
	}

	private void ScrPress(string[] args, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null)
	{
		if (scrTimer.IsRunning)
		{
			scrTimer.Reset();
		}
		scrTimer.Restart();
		double num = 0.0;
		double num2 = 0.0;
		ulong num3 = 9259542121117908992uL;
		if (decimal.TryParse(args[^2], out var _))
		{
			num = (int)(1000m * decimal.Parse(args[^2]));
			num2 = (int)(1000m * decimal.Parse(args[^1]));
		}
		else
		{
			num = (int)(1000m * decimal.Parse(args[^1]));
		}
		int num4 = 0;
		for (int i = 1; i < args.Length - 1; i++)
		{
			if (args[i] == "A")
			{
				num3 |= 8;
				num4 |= 4;
			}
			else if (args[i] == "B")
			{
				num3 |= 4;
				num4 |= 2;
			}
			else if (args[i] == "X")
			{
				num3 |= 2;
				num4 |= 8;
			}
			else if (args[i] == "Y")
			{
				num3 |= 1;
				num4 |= 1;
			}
			else if (args[i] == "ZL")
			{
				num3 |= 0x800000;
				num4 |= 0x40;
			}
			else if (args[i] == "ZR")
			{
				num3 |= 0x80;
				num4 |= 0x80;
			}
			else if (args[i] == "L")
			{
				num3 |= 0x400000;
				num4 |= 0x10;
			}
			else if (args[i] == "R")
			{
				num3 |= 0x40;
				num4 |= 0x20;
			}
			else if (args[i] == "DOWN")
			{
				num3 |= 0x10000;
			}
			else if (args[i] == "UP")
			{
				num3 |= 0x20000;
			}
			else if (args[i] == "RIGHT")
			{
				num3 |= 0x40000;
			}
			else if (args[i] == "DOWNRIGHT")
			{
				num3 |= 0x50000;
			}
			else if (args[i] == "UPRIGHT")
			{
				num3 |= 0x60000;
			}
			else if (args[i] == "LEFT")
			{
				num3 |= 0x80000;
			}
			else if (args[i] == "DOWNLEFT")
			{
				num3 |= 0x90000;
			}
			else if (args[i] == "UPLEFT")
			{
				num3 |= 0xA0000;
			}
			else if (args[i] == "START")
			{
				num3 |= 0x200;
				num4 |= 0x200;
			}
			else if (args[i] == "SELECT")
			{
				num3 |= 0x100;
				num4 |= 0x100;
			}
			else if (args[i] == "HOME")
			{
				num3 |= 0x1000;
				num4 |= 0x1000;
			}
			else if (args[i] == "CAPTURE")
			{
				num3 |= 0x2000;
				num4 |= 0x2000;
			}
			else if (args[i] == "CLICK_L")
			{
				num3 |= 0x800;
				num4 |= 0x400;
			}
			else if (args[i] == "CLICK_R")
			{
				num3 |= 0x400;
				num4 |= 0x800;
			}
			else if (args[i] == "UP_L")
			{
				num3 &= 0xFFFFFF00FFFFFFFFuL;
				num3 |= 0;
			}
			else if (args[i].Substring(0, 3) == "LS(")
			{
				string[] array = AsmSplit(args[i]);
				array[1] = array[1].StrCalc(varDatas, varDatas2);
				array[2] = array[2].StrCalc(varDatas, varDatas2);
				double num5 = double.Parse(array[1]) % 360.0;
				double num6 = Math.Min(1.0, double.Parse(array[2]));
				ulong num7 = (ulong)Math.Ceiling(127.5 * Math.Cos(num5 * (Math.PI / 180.0)) * num6 + 127.5);
				ulong num8 = (ulong)Math.Floor(127.5 * Math.Sin(num5 * (Math.PI / 180.0)) * num6 + 127.5);
				num3 &= 0xFFFF0000FFFFFFFFuL;
				num3 |= 0 | (num8 << 32) | (num7 << 40);
			}
			else if (args[i].Substring(0, 3) == "RS(")
			{
				string[] array2 = AsmSplit(args[i]);
				array2[1] = array2[1].StrCalc(varDatas, varDatas2);
				array2[2] = array2[2].StrCalc(varDatas, varDatas2);
				double num9 = double.Parse(array2[1]) % 360.0;
				double num10 = Math.Min(1.0, double.Parse(array2[2]));
				ulong num11 = (ulong)Math.Ceiling(127.5 * Math.Cos(num9 * (Math.PI / 180.0)) * num10 + 127.5);
				ulong num12 = (ulong)Math.Floor(127.5 * Math.Sin(num9 * (Math.PI / 180.0)) * num10 + 127.5);
				num3 &= 0xFFFFFFFFFFFFL;
				num3 |= 0 | (num12 << 48) | (num11 << 56);
			}
			else if (args[i] == "DOWN_L")
			{
				num3 &= 0xFFFFFF00FFFFFFFFuL;
				num3 |= 0xFF00000000L;
			}
			else if (args[i] == "LEFT_L")
			{
				num3 &= 0xFFFF00FFFFFFFFFFuL;
				num3 |= 0;
			}
			else if (args[i] == "UPLEFT_L")
			{
				num3 &= 0xFFFF0000FFFFFFFFuL;
				num3 |= 0;
			}
			else if (args[i] == "DOWNLEFT_L")
			{
				num3 &= 0xFFFF0000FFFFFFFFuL;
				num3 |= 0xFF00000000L;
			}
			else if (args[i] == "RIGHT_L")
			{
				num3 &= 0xFFFF00FFFFFFFFFFuL;
				num3 |= 0xFF0000000000L;
			}
			else if (args[i] == "UPRIGHT_L")
			{
				num3 &= 0xFFFF0000FFFFFFFFuL;
				num3 |= 0xFF0000000000L;
			}
			else if (args[i] == "DOWNRIGHT_L")
			{
				num3 &= 0xFFFF0000FFFFFFFFuL;
				num3 |= 0xFFFF00000000L;
			}
			else if (args[i] == "UP_R")
			{
				num3 &= 0xFF00FFFFFFFFFFFFuL;
				num3 |= 0;
			}
			else if (args[i] == "DOWN_R")
			{
				num3 &= 0xFF00FFFFFFFFFFFFuL;
				num3 |= 0xFF000000000000L;
			}
			else if (args[i] == "LEFT_R")
			{
				num3 &= 0xFFFFFFFFFFFFFFL;
				num3 |= 0;
			}
			else if (args[i] == "UPLEFT_R")
			{
				num3 &= 0xFFFFFFFFFFFFL;
				num3 |= 0;
			}
			else if (args[i] == "DOWNLEFT_R")
			{
				num3 &= 0xFFFFFFFFFFFFL;
				num3 |= 0xFF000000000000L;
			}
			else if (args[i] == "RIGHT_R")
			{
				num3 &= 0xFFFFFFFFFFFFFFL;
				num3 |= 0xFF00000000000000uL;
			}
			else if (args[i] == "UPRIGHT_R")
			{
				num3 &= 0xFFFFFFFFFFFFL;
				num3 |= 0xFF00000000000000uL;
			}
			else if (args[i] == "DOWNRIGHT_R")
			{
				num3 &= 0xFFFFFFFFFFFFL;
				num3 |= 0xFFFF000000000000uL;
			}
		}
		if (!ch552Analyze)
		{
			baseNmc.NmcKeyFlag = num3;
		}
		waitFunc((int)num, scrTimer);
		if (num2 != 0.0 && !baseNmc.Cancel)
		{
			if (!ch552Analyze)
			{
				baseNmc.NmcKeyFlag = 9259542121117908992uL;
			}
			waitFunc((int)num2);
		}
	}
}

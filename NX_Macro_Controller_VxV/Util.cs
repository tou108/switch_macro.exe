using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BZComponent;
using NeoSmart.Utils;
using Newtonsoft.Json;
using NX_Macro_Controller_VxV.Properties;
using NxInterface;

namespace NX_Macro_Controller_VxV;

public static class Util
{
	[JsonObject("user")]
	public class UserData
	{
		[JsonProperty("userid")]
		public string UserID { get; set; }

		[JsonProperty("pass")]
		public string Pass { get; set; }

		[JsonProperty("data")]
		public string Data { get; set; }
	}

	[JsonObject("user")]
	public class MacroData
	{
		[JsonProperty("userid")]
		public string UserID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("pass")]
		public string Pass { get; set; }

		[JsonProperty("data")]
		public string Data { get; set; }
	}

	[JsonObject("user")]
	public class MacroUpData
	{
		[JsonProperty("userid")]
		public string UserID { get; set; }

		[JsonProperty("macroid")]
		public int MacroID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("pass")]
		public string Pass { get; set; }

		[JsonProperty("data")]
		public string Data { get; set; }
	}

	[JsonObject("macrolist")]
	public class MacroList
	{
		[JsonProperty("macroname")]
		public string MacroName { get; set; }

		[JsonProperty("macroid")]
		public int MacroID { get; set; }

		[JsonProperty("dlcnt")]
		public int DLCnt { get; set; }

		[JsonProperty("favcnt")]
		public int FavCnt { get; set; }

		[JsonProperty("username")]
		public string UserID { get; set; }

		[JsonProperty("direction")]
		public string Direction { get; set; }

		[JsonProperty("isincludefiles")]
		public bool IsIncludeFiles { get; set; }
	}

	public enum FilePathType
	{
		NotFound,
		File,
		Directory
	}

	private static byte[] bmp = new byte[2] { 66, 77 };

	private static byte[] gif87a = new byte[6] { 71, 73, 70, 56, 55, 97 };

	private static byte[] gif89a = new byte[6] { 71, 73, 70, 56, 57, 97 };

	private static byte[] png = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

	private static byte[] tiffI = new byte[4] { 73, 73, 42, 0 };

	private static byte[] tiffM = new byte[4] { 77, 77, 0, 42 };

	private static byte[] jpeg = new byte[3] { 255, 216, 255 };

	private static byte[] jpegEnd = new byte[2] { 255, 217 };

	private const int LOGPIXELSX = 88;

	private const int LOGPIXELSY = 90;

	private static List<string> numList_StrCalc = new List<string>();

	public static Image GetUserImage(string userID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			string text = ":null";
			HttpClient val = new HttpClient();
			try
			{
				text = val.GetStringAsync(GlobalVar.Server + "/api/usericon?id=" + userID).Result.ToString();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (text == ":null")
			{
				return null;
			}
			return Image.FromStream(new MemoryStream(UrlBase64.Decode(text)));
		}
		catch
		{
			return Resources.usericon;
		}
	}

	public static async Task ChangeUserIcon(string userID, string password, Image icon, bool first = true)
	{
		string text = Path.GetTempPath() + "\\NX";
		Random random = new Random(DateTime.Now.Millisecond);
		int num = random.Next();
		while (Directory.Exists(text + num))
		{
			num = random.Next();
		}
		text += num;
		Directory.CreateDirectory(text);
		icon.Save(text + "\\ic.png", ImageFormat.Png);
		FileStream fileStream = new FileStream(text + "\\ic.png", FileMode.Open, FileAccess.Read);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, array.Length);
		fileStream.Close();
		UserData userData = new UserData();
		userData.UserID = userID;
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		userData.Pass = password;
		userData.Data = UrlBase64.Encode(array);
		if (userData.Data.Length > 59768832)
		{
			throw new Exception();
		}
		Delete(text);
		try
		{
			string text2 = JsonConvert.SerializeObject(userData);
			text2 = text2.Replace("\"", "\\\"");
			StringContent val = new StringContent("\"" + text2 + "\"", Encoding.UTF8, "application/json");
			HttpClient client = new HttpClient();
			try
			{
				await client.PostAsync(GlobalVar.Server + "/api/UserIconChange", (HttpContent)(object)val);
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}

	public static bool ChangeUserName(string userID, string password, string name, bool first = true)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (password.Length < 7)
		{
			return false;
		}
		if (userID.Length < 1)
		{
			return false;
		}
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		bool flag = false;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				return bool.Parse(val.GetStringAsync(GlobalVar.Server + "/api/updateusername?pass=" + password + "&id=" + userID + "&name=" + name).Result.ToString());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return false;
		}
	}

	public static bool ChangeUserPassword(string userID, string password, string newpass, bool first = true)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (password.Length < 7)
		{
			return false;
		}
		if (userID.Length < 1)
		{
			return false;
		}
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		bool flag = false;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				return bool.Parse(val.GetStringAsync(GlobalVar.Server + "/api/updateuserpassword?pass=" + password + "&id=" + userID + "&newpass=" + newpass).Result.ToString());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return false;
		}
	}

	public static string GetUserName(string userID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		string text = ":null";
		HttpClient val = new HttpClient();
		try
		{
			return val.GetStringAsync(GlobalVar.Server + "/api/getusername?id=" + userID).Result.ToString();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool CheckLogin(string userID, string password, bool first = true)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (password.Length < 7)
		{
			return false;
		}
		if (userID.Length < 1)
		{
			return false;
		}
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		bool flag = false;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				return bool.Parse(val.GetStringAsync(GlobalVar.Server + "/api/passwordcheck?pass=" + password + "&id=" + userID).Result.ToString());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return false;
		}
	}

	public static bool AddUser(string userID, string password, string name, bool first = true)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		bool flag = false;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				return bool.Parse(val.GetStringAsync(GlobalVar.Server + "/api/adduser?pass=" + password + "&id=" + userID + "&name=" + name).Result.ToString());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return false;
		}
	}

	public static bool AddFavorite(int macroID)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		bool flag = false;
		HttpClient val = new HttpClient();
		try
		{
			return bool.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/addfavorite?id={macroID}").Result.ToString());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool CheckFavorite(int macroID)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		bool flag = false;
		HttpClient val = new HttpClient();
		try
		{
			return bool.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/checkfavorite?id={macroID}").Result.ToString());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static int MacroDLCounter(int macroID)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		int num = 0;
		HttpClient val = new HttpClient();
		try
		{
			return int.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/downloadcounter?id={macroID}").Result.ToString());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static int MacroFavCounter(int macroID)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		int num = 0;
		HttpClient val = new HttpClient();
		try
		{
			return int.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/favoritecounter?id={macroID}").Result.ToString());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool DeleteMacro(string userID, string password, int macroID, bool first = true)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		bool flag = false;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				return bool.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/deletemacro?pass={password}&userid={userID}&macroid={macroID}").Result.ToString());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			return false;
		}
	}

	public static MacroList GetMacroData(int macroID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		string text = ":null";
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				text = val.GetStringAsync($"{GlobalVar.Server}/api/getmacrodata?id={macroID}").Result.ToString();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			MacroList macroList = JsonConvert.DeserializeObject<MacroList>(text);
			if (text == ":null" || macroList == null)
			{
				return new MacroList();
			}
			return macroList;
		}
		catch
		{
			return new MacroList();
		}
	}

	public static string GetMacroTimeData(int macroID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		string result = ":null";
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				result = val.GetStringAsync($"{GlobalVar.Server}/api/getmacrodate?id={macroID}").Result.ToString();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return result;
		}
		catch
		{
			return result;
		}
	}

	public static int GetMacroSize(int macroID)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		int num = -1;
		HttpClient val = new HttpClient();
		try
		{
			return int.Parse(val.GetStringAsync($"{GlobalVar.Server}/api/macrodatasize?id={macroID}").Result.ToString());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static byte[] MacroDownload(int macroID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		string text = ":null";
		HttpClient val = new HttpClient();
		try
		{
			text = val.GetStringAsync($"{GlobalVar.Server}/api/macrodownload?id={macroID}").Result.ToString();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (text == ":null")
		{
			return null;
		}
		return UrlBase64.Decode(text);
	}

	public static async Task MacroUpload(byte[] data, string macroName, string macroText, string userID, string password, bool first = true)
	{
		MacroData macroData = new MacroData();
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		macroData.Text = macroText;
		macroData.Name = macroName;
		macroData.Pass = password;
		macroData.UserID = userID;
		if (!MacroDataCheck(data))
		{
			MessageBox.Show("アップロードに失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		macroData.Data = UrlBase64.Encode(data);
		try
		{
			string text = JsonConvert.SerializeObject(macroData);
			text = text.Replace("\"", "\\\"");
			StringContent val = new StringContent("\"" + text + "\"", Encoding.UTF8, "application/json");
			HttpClient client = new HttpClient();
			try
			{
				await client.PostAsync(GlobalVar.Server + "/api/macroupload", (HttpContent)(object)val);
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		catch
		{
		}
	}

	public static async Task MacroUpdate(byte[] data, string macroName, string macroText, string userID, string password, int macroID, bool first = true)
	{
		MacroUpData macroUpData = new MacroUpData();
		password = ((!first) ? password.KeyHash3() : password.KeyHash(userID));
		macroUpData.Text = macroText;
		macroUpData.Name = macroName;
		macroUpData.Pass = password;
		macroUpData.UserID = userID;
		if (!MacroDataCheck(data))
		{
			MessageBox.Show("アップロードに失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		macroUpData.Data = UrlBase64.Encode(data);
		macroUpData.MacroID = macroID;
		try
		{
			string text = JsonConvert.SerializeObject(macroUpData);
			text = text.Replace("\"", "\\\"");
			StringContent val = new StringContent("\"" + text + "\"", Encoding.UTF8, "application/json");
			HttpClient client = new HttpClient();
			try
			{
				await client.PostAsync(GlobalVar.Server + "/api/updatemacroinfo", (HttpContent)(object)val);
			}
			finally
			{
				((IDisposable)client)?.Dispose();
			}
		}
		catch
		{
		}
	}

	public static bool MacroDataCheck(byte[] data)
	{
		if (data.Length <= 6)
		{
			return false;
		}
		if (data[0] != 231)
		{
			return false;
		}
		if (data[1] != 72 && data[1] != 136)
		{
			return false;
		}
		if (data[2] != 10)
		{
			return false;
		}
		if (data[3] != 131)
		{
			return false;
		}
		uint num = data[4];
		num |= (uint)(data[5] << 8);
		uint num2 = 160u;
		for (int i = 6; i < data.Length; i++)
		{
			num2 += data[i];
		}
		num2 &= 0xFFFF;
		if (num2 != num)
		{
			return false;
		}
		return true;
	}

	public static bool MacroDataCheckOffline(byte[] data)
	{
		if (data.Length <= 6)
		{
			return false;
		}
		if (MacroDataCheck(data))
		{
			return true;
		}
		if (data[0] == 231 && (data[1] == 72 || data[1] == 136) && data[2] == 3 && data[3] == 131)
		{
			try
			{
				new NMC().NMCRead(data);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		if (data[0] == 78 && data[1] == 88 && data[2] == 77 && data[3] == 65 && data[4] == 67 && data[5] == 82 && data[6] == 79)
		{
			try
			{
				new NMC().NMCRead(data);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		return false;
	}

	public static MacroList[] SearchMacro(string searchStr, int start, int length, int mode)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		string text = ":null";
		HttpClient val = new HttpClient();
		try
		{
			text = val.GetStringAsync($"{GlobalVar.Server}/api/popularmacro?search={searchStr}&start={start}&length={length}&dl={mode}").Result.ToString();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (text == ":null")
		{
			return new MacroList[0];
		}
		return JsonConvert.DeserializeObject<MacroList[]>(text);
	}

	public static bool HashDebug(this string str, string userid)
	{
		string text = str.KeyHash(userid);
		string text2 = str.GenerateHash(userid).KeyHash3();
		return text == text2;
	}

	private static string KeyHash(this string str, string userid)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		List<byte> list = uTF8Encoding.GetBytes(str.ToMd5Hash()).ToList();
		list.AddRange(uTF8Encoding.GetBytes(userid.ToMd5Hash()));
		byte[] array = new byte[1];
		using (SHA256CryptoServiceProvider sHA256CryptoServiceProvider = new SHA256CryptoServiceProvider())
		{
			array = sHA256CryptoServiceProvider.ComputeHash(list.ToArray());
			for (int i = 0; i < 100000; i++)
			{
				array = sHA256CryptoServiceProvider.ComputeHash(array);
			}
		}
		return string.Concat(array.Select((byte b) => $"{b:x2}"));
	}

	public static string GenerateHash(this string str, string userid)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		List<byte> list = uTF8Encoding.GetBytes(str.ToMd5Hash()).ToList();
		list.AddRange(uTF8Encoding.GetBytes(userid.ToMd5Hash()));
		byte[] array = new byte[1];
		using (SHA256CryptoServiceProvider sHA256CryptoServiceProvider = new SHA256CryptoServiceProvider())
		{
			array = sHA256CryptoServiceProvider.ComputeHash(list.ToArray());
			for (int i = 0; i < 71896; i++)
			{
				array = sHA256CryptoServiceProvider.ComputeHash(array);
			}
		}
		return string.Concat(array.Select((byte b) => $"{b:x2}"));
	}

	private static string KeyHash3(this string str)
	{
		byte[] array = new byte[32];
		for (int i = 0; i < 32; i++)
		{
			try
			{
				string s = str.Substring(i * 2, 2);
				array[i] = byte.Parse(s, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				break;
			}
		}
		using (SHA256CryptoServiceProvider sHA256CryptoServiceProvider = new SHA256CryptoServiceProvider())
		{
			for (int j = 0; j < 28104; j++)
			{
				array = sHA256CryptoServiceProvider.ComputeHash(array);
			}
		}
		return string.Concat(array.Select((byte b) => $"{b:x2}"));
	}

	[DllImport("KERNEL32.DLL")]
	private static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

	[DllImport("KERNEL32.DLL")]
	private static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

	[DllImport("shlwapi.dll")]
	public static extern int SHMessageBoxCheck(IntPtr hwnd, string text, string title, MessageBoxCheckFlags uType, int iDefault, string regVal);

	public static void EnableDoubleBuffering(Control control)
	{
		control.GetType().InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, null, control, new object[1] { true });
	}

	public static string GetValueString(string section, string key, string fileName)
	{
		StringBuilder stringBuilder = new StringBuilder(65536);
		GetPrivateProfileString(section, key, "", stringBuilder, Convert.ToUInt32(stringBuilder.Capacity), fileName);
		return stringBuilder.ToString();
	}

	public static int GetValueInt(string section, string key, string fileName)
	{
		new StringBuilder(65536);
		return (int)GetPrivateProfileInt(section, key, 0, fileName);
	}

	public static IEnumerable<FieldInfo> GetAllFields(Type t)
	{
		if (t == null)
		{
			return Enumerable.Empty<FieldInfo>();
		}
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		return t.GetFields(bindingAttr).Concat(GetAllFields(t.BaseType));
	}

	public static void SafelyCreateZipFromDirectory(string sourceDirectoryName, string zipFilePath)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		using FileStream fileStream = new FileStream(zipFilePath, FileMode.Create);
		ZipArchive val = new ZipArchive((Stream)fileStream, (ZipArchiveMode)1);
		try
		{
			string[] files = Directory.GetFiles(sourceDirectoryName, "*", SearchOption.AllDirectories);
			foreach (string text in files)
			{
				if (Path.GetFullPath(text) == Path.GetFullPath(zipFilePath))
				{
					continue;
				}
				string text2 = text.Substring(sourceDirectoryName.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				ZipArchiveEntry val2 = val.CreateEntry(text2);
				val2.LastWriteTime = File.GetLastWriteTime(text);
				using FileStream fileStream2 = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using Stream destination = val2.Open();
				fileStream2.CopyTo(destination, 81920);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static System.Windows.Media.Brush ToBrush(this System.Drawing.Color color)
	{
		return new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
	}

	public static bool UpdateCheck()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		int num = 0;
		try
		{
			HttpClient val = new HttpClient();
			try
			{
				num = int.Parse(val.GetStringAsync(GlobalVar.Server + "/api/updatecheck?app=nx").Result);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
		return GlobalVar.Ver < num;
	}

	public static FilePathType GetFilePathType(string path)
	{
		if (File.Exists(path))
		{
			return FilePathType.File;
		}
		if (Directory.Exists(path))
		{
			return FilePathType.Directory;
		}
		return FilePathType.NotFound;
	}

	[DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
	private static extern bool PathRelativePathTo([Out] StringBuilder pszPath, [In] string pszFrom, [In] FileAttributes dwAttrFrom, [In] string pszTo, [In] FileAttributes dwAttrTo);

	public static string GetRelativePath(string basePath, string absolutePath)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		if (!PathRelativePathTo(stringBuilder, basePath, FileAttributes.Directory, absolutePath, FileAttributes.Normal))
		{
			throw new Exception("相対パスの取得に失敗しました。");
		}
		return stringBuilder.ToString();
	}

	public static void CopyDirectory(string sourceDirName, string destDirName)
	{
		if (!Directory.Exists(destDirName))
		{
			Directory.CreateDirectory(destDirName);
			File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
		}
		if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
		{
			destDirName += Path.DirectorySeparatorChar;
		}
		string[] files = Directory.GetFiles(sourceDirName);
		foreach (string text in files)
		{
			File.Copy(text, destDirName + Path.GetFileName(text), overwrite: true);
		}
		files = Directory.GetDirectories(sourceDirName);
		foreach (string text2 in files)
		{
			CopyDirectory(text2, destDirName + Path.GetFileName(text2));
		}
	}

	public static void ReadConfig()
	{
		string fileName = GlobalVar.BasePath + "config.ini";
		GlobalVar.FavMacro.Clear();
		GlobalVar.MacroList.Clear();
		GlobalVar.BlackList.Clear();
		Enum.TryParse<Keys>(GetValueString("Button", "A", fileName), out KEYCONFIG.Button.A);
		Enum.TryParse<Keys>(GetValueString("Button", "B", fileName), out KEYCONFIG.Button.B);
		Enum.TryParse<Keys>(GetValueString("Button", "X", fileName), out KEYCONFIG.Button.X);
		Enum.TryParse<Keys>(GetValueString("Button", "Y", fileName), out KEYCONFIG.Button.Y);
		Enum.TryParse<Keys>(GetValueString("Button", "L", fileName), out KEYCONFIG.Button.L);
		Enum.TryParse<Keys>(GetValueString("Button", "R", fileName), out KEYCONFIG.Button.R);
		Enum.TryParse<Keys>(GetValueString("Button", "ZL", fileName), out KEYCONFIG.Button.ZL);
		Enum.TryParse<Keys>(GetValueString("Button", "ZR", fileName), out KEYCONFIG.Button.ZR);
		Enum.TryParse<Keys>(GetValueString("Button", "START", fileName), out KEYCONFIG.Button.START);
		Enum.TryParse<Keys>(GetValueString("Button", "SELECT", fileName), out KEYCONFIG.Button.SELECT);
		Enum.TryParse<Keys>(GetValueString("Button", "CLICKL", fileName), out KEYCONFIG.Button.CLICKL);
		Enum.TryParse<Keys>(GetValueString("Button", "CLICKR", fileName), out KEYCONFIG.Button.CLICKR);
		Enum.TryParse<Keys>(GetValueString("Button", "HOME", fileName), out KEYCONFIG.Button.HOME);
		Enum.TryParse<Keys>(GetValueString("Button", "CAPTURE", fileName), out KEYCONFIG.Button.CAPTURE);
		Enum.TryParse<Keys>(GetValueString("DPad", "UP", fileName), out KEYCONFIG.DPad.UP);
		Enum.TryParse<Keys>(GetValueString("DPad", "DOWN", fileName), out KEYCONFIG.DPad.DOWN);
		Enum.TryParse<Keys>(GetValueString("DPad", "RIGHT", fileName), out KEYCONFIG.DPad.RIGHT);
		Enum.TryParse<Keys>(GetValueString("DPad", "LEFT", fileName), out KEYCONFIG.DPad.LEFT);
		Enum.TryParse<Keys>(GetValueString("AnalogL", "UP", fileName), out KEYCONFIG.AnalogL.UP);
		Enum.TryParse<Keys>(GetValueString("AnalogL", "DOWN", fileName), out KEYCONFIG.AnalogL.DOWN);
		Enum.TryParse<Keys>(GetValueString("AnalogL", "RIGHT", fileName), out KEYCONFIG.AnalogL.RIGHT);
		Enum.TryParse<Keys>(GetValueString("AnalogL", "LEFT", fileName), out KEYCONFIG.AnalogL.LEFT);
		Enum.TryParse<Keys>(GetValueString("AnalogR", "UP", fileName), out KEYCONFIG.AnalogR.UP);
		Enum.TryParse<Keys>(GetValueString("AnalogR", "DOWN", fileName), out KEYCONFIG.AnalogR.DOWN);
		Enum.TryParse<Keys>(GetValueString("AnalogR", "RIGHT", fileName), out KEYCONFIG.AnalogR.RIGHT);
		Enum.TryParse<Keys>(GetValueString("AnalogR", "LEFT", fileName), out KEYCONFIG.AnalogR.LEFT);
		KEYCONFIG.DxButton.A = GetValueString("DxButton", "A", fileName);
		KEYCONFIG.DxButton.B = GetValueString("DxButton", "B", fileName);
		KEYCONFIG.DxButton.X = GetValueString("DxButton", "X", fileName);
		KEYCONFIG.DxButton.Y = GetValueString("DxButton", "Y", fileName);
		KEYCONFIG.DxButton.L = GetValueString("DxButton", "L", fileName);
		KEYCONFIG.DxButton.R = GetValueString("DxButton", "R", fileName);
		KEYCONFIG.DxButton.ZL = GetValueString("DxButton", "ZL", fileName);
		KEYCONFIG.DxButton.ZR = GetValueString("DxButton", "ZR", fileName);
		KEYCONFIG.DxButton.START = GetValueString("DxButton", "START", fileName);
		KEYCONFIG.DxButton.SELECT = GetValueString("DxButton", "SELECT", fileName);
		KEYCONFIG.DxButton.CLICKL = GetValueString("DxButton", "CLICKL", fileName);
		KEYCONFIG.DxButton.CLICKR = GetValueString("DxButton", "CLICKR", fileName);
		KEYCONFIG.DxButton.HOME = GetValueString("DxButton", "HOME", fileName);
		KEYCONFIG.DxButton.CAPTURE = GetValueString("DxButton", "CAPTURE", fileName);
		KEYCONFIG.DxDPad.UP = GetValueString("DxDPad", "UP", fileName);
		KEYCONFIG.DxDPad.DOWN = GetValueString("DxDPad", "DOWN", fileName);
		KEYCONFIG.DxDPad.LEFT = GetValueString("DxDPad", "LEFT", fileName);
		KEYCONFIG.DxDPad.RIGHT = GetValueString("DxDPad", "RIGHT", fileName);
		KEYCONFIG.DxAnalogL.UP = GetValueString("DxAnalogL", "UP", fileName);
		KEYCONFIG.DxAnalogL.DOWN = GetValueString("DxAnalogL", "DOWN", fileName);
		KEYCONFIG.DxAnalogL.LEFT = GetValueString("DxAnalogL", "LEFT", fileName);
		KEYCONFIG.DxAnalogL.RIGHT = GetValueString("DxAnalogL", "RIGHT", fileName);
		KEYCONFIG.DxAnalogR.UP = GetValueString("DxAnalogR", "UP", fileName);
		KEYCONFIG.DxAnalogR.DOWN = GetValueString("DxAnalogR", "DOWN", fileName);
		KEYCONFIG.DxAnalogR.LEFT = GetValueString("DxAnalogR", "LEFT", fileName);
		KEYCONFIG.DxAnalogR.RIGHT = GetValueString("DxAnalogR", "RIGHT", fileName);
		KEYCONFIG.XiButton.A = GetValueString("XiButton", "A", fileName);
		KEYCONFIG.XiButton.B = GetValueString("XiButton", "B", fileName);
		KEYCONFIG.XiButton.X = GetValueString("XiButton", "X", fileName);
		KEYCONFIG.XiButton.Y = GetValueString("XiButton", "Y", fileName);
		KEYCONFIG.XiButton.L = GetValueString("XiButton", "L", fileName);
		KEYCONFIG.XiButton.R = GetValueString("XiButton", "R", fileName);
		KEYCONFIG.XiButton.ZL = GetValueString("XiButton", "ZL", fileName);
		KEYCONFIG.XiButton.ZR = GetValueString("XiButton", "ZR", fileName);
		KEYCONFIG.XiButton.START = GetValueString("XiButton", "START", fileName);
		KEYCONFIG.XiButton.SELECT = GetValueString("XiButton", "SELECT", fileName);
		KEYCONFIG.XiButton.CLICKL = GetValueString("XiButton", "CLICKL", fileName);
		KEYCONFIG.XiButton.CLICKR = GetValueString("XiButton", "CLICKR", fileName);
		KEYCONFIG.XiButton.HOME = GetValueString("XiButton", "HOME", fileName);
		KEYCONFIG.XiButton.CAPTURE = GetValueString("XiButton", "CAPTURE", fileName);
		KEYCONFIG.XiDPad.UP = GetValueString("XiDPad", "UP", fileName);
		KEYCONFIG.XiDPad.DOWN = GetValueString("XiDPad", "DOWN", fileName);
		KEYCONFIG.XiDPad.LEFT = GetValueString("XiDPad", "LEFT", fileName);
		KEYCONFIG.XiDPad.RIGHT = GetValueString("XiDPad", "RIGHT", fileName);
		KEYCONFIG.XiAnalogL.UP = GetValueString("XiAnalogL", "UP", fileName);
		KEYCONFIG.XiAnalogL.DOWN = GetValueString("XiAnalogL", "DOWN", fileName);
		KEYCONFIG.XiAnalogL.LEFT = GetValueString("XiAnalogL", "LEFT", fileName);
		KEYCONFIG.XiAnalogL.RIGHT = GetValueString("XiAnalogL", "RIGHT", fileName);
		KEYCONFIG.XiAnalogR.UP = GetValueString("XiAnalogR", "UP", fileName);
		KEYCONFIG.XiAnalogR.DOWN = GetValueString("XiAnalogR", "DOWN", fileName);
		KEYCONFIG.XiAnalogR.LEFT = GetValueString("XiAnalogR", "LEFT", fileName);
		KEYCONFIG.XiAnalogR.RIGHT = GetValueString("XiAnalogR", "RIGHT", fileName);
		bool.TryParse(GetValueString("ControlConfig", "GAMEPADONLY", fileName), out KEYCONFIG.ControlConfig.GAMEPADONLY);
		bool.TryParse(GetValueString("ControlConfig", "NOTUSEDEACTIVATE", fileName), out KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE);
		bool.TryParse(GetValueString("ControlConfig", "NOTUSERUNNINGMACRO", fileName), out KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO);
		bool.TryParse(GetValueString("ControlConfig", "USEKEYBOARD", fileName), out KEYCONFIG.ControlConfig.USEKEYBOARD);
		bool.TryParse(GetValueString("ControlConfig", "USESTICKBINARY", fileName), out KEYCONFIG.ControlConfig.USESTICKBINARY);
		bool.TryParse(GetValueString("ControlConfig", "REC8AXIS", fileName), out KEYCONFIG.ControlConfig.REC8AXIS);
		bool.TryParse(GetValueString("AppConfig", "UPDATECHECK", fileName), out KEYCONFIG.AppConfig.UPDATECHECK);
		Enum.TryParse<Style>(GetValueString("AppConfig", "APPTHEME", fileName), out KEYCONFIG.AppConfig.APPTHEME);
		GetValueString("AppConfig", "SHORTCUTS", fileName);
		GlobalVar.MacroList.AddRange((from _ in GetValueString("AppConfig", "SHORTCUTS", fileName).Trim().Split(',')
			where !string.IsNullOrWhiteSpace(_.Trim())
			select _.Trim()).ToList());
		GlobalVar.CaptureOutput = GetValueString("AppConfig", "CAPTURE", fileName);
		int.TryParse(GetValueString("AppConfig", "LASTVER", fileName), out GlobalVar.LastVer);
		if (string.IsNullOrWhiteSpace(GlobalVar.CaptureOutput))
		{
			GlobalVar.CaptureOutput = Path.GetFullPath(GlobalVar.BasePath + "Captures");
		}
		Enum.TryParse<NXMC_VxV.CaptureStyle>(GetValueString("AppConfig", "CAPTURESTYLE", fileName), out KEYCONFIG.AppConfig.CAPTURESTYLE);
		if (KEYCONFIG.AppConfig.CAPTURESTYLE == NXMC_VxV.CaptureStyle.None)
		{
			KEYCONFIG.AppConfig.CAPTURESTYLE = NXMC_VxV.CaptureStyle.DirectShow;
		}
		bool.TryParse(GetValueString("EditorConfig", "RUNNINGFOCUS", fileName), out KEYCONFIG.EditorConfig.RUNNINGFOCUS);
		GlobalVar.FavMacro.AddRange((from _ in GetValueString("NetworkConfig", "FAVORITE", fileName).Trim().Split(',')
			where int.TryParse(_, out var _)
			select int.Parse(_)).ToList());
		GlobalVar.BlackList.AddRange((from _ in GetValueString("NetworkConfig", "BLACKLIST", fileName).Trim().Split(',')
			where !string.IsNullOrWhiteSpace(_.Trim())
			select _.Trim()).ToList());
		KEYCONFIG.NetworkConfig.ID = GetValueString("NetworkConfig", "ID", fileName);
		KEYCONFIG.NetworkConfig.KEY = GetValueString("NetworkConfig", "KEY", fileName);
		NxCommand.LineNotifyToken = GetValueString("NetworkConfig", "LINETOKEN", fileName);
		GlobalVar.Server = "http://bzl-web.com:8737";
	}

	public static double ColourDistance(System.Drawing.Color e1, System.Drawing.Color e2)
	{
		long num = ((long)e1.R + (long)e2.R) / 2;
		long num2 = (long)e1.R - (long)e2.R;
		long num3 = (long)e1.G - (long)e2.G;
		long num4 = (long)e1.B - (long)e2.B;
		return Math.Sqrt(((512 + num) * num2 * num2 >> 8) + 4 * num3 * num3 + ((767 - num) * num4 * num4 >> 8));
	}

	public static Image ImageResize(this Image src, int width, int height)
	{
		if (src == null)
		{
			return null;
		}
		if (src.Width == width && src.Height == height)
		{
			return src;
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		src.Save(memoryStream, ImageFormat.Bmp);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		BitmapFrame bitmapFrame = BitmapDecoder.Create(memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
		double scaleX = (double)width / bitmapFrame.Width;
		double scaleY = (double)height / bitmapFrame.Height;
		TransformedBitmap source = new TransformedBitmap(bitmapFrame, new ScaleTransform(scaleX, scaleY));
		BmpBitmapEncoder obj = new BmpBitmapEncoder
		{
			Frames = { BitmapFrame.Create(source) }
		};
		memoryStream.Seek(0L, SeekOrigin.Begin);
		obj.Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return (Bitmap)Image.FromStream(memoryStream);
	}

	public static Image CopyImage(this Image src)
	{
		Bitmap bitmap = new Bitmap(src.Width, src.Height);
		Graphics graphics = Graphics.FromImage(bitmap);
		Bitmap bitmap2 = new Bitmap(src);
		graphics.DrawImage(bitmap2, 0, 0, src.Width, src.Height);
		bitmap2.Dispose();
		graphics.Dispose();
		return bitmap;
	}

	public static bool BzPing()
	{
		try
		{
			Ping ping = new Ping();
			List<PingReply> list = new List<PingReply>();
			for (int i = 0; i < 5; i++)
			{
				PingReply item = ping.Send("bzl.server-on.net:8737");
				list.Add(item);
			}
			return list.Any((PingReply reply) => reply.Status == IPStatus.Success);
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void SaveConfig()
	{
		string text = "";
		text += "[Button]\r\n";
		text += $"A={KEYCONFIG.Button.A}\r\n";
		text += $"B={KEYCONFIG.Button.B}\r\n";
		text += $"X={KEYCONFIG.Button.X}\r\n";
		text += $"Y={KEYCONFIG.Button.Y}\r\n";
		text += $"L={KEYCONFIG.Button.L}\r\n";
		text += $"R={KEYCONFIG.Button.R}\r\n";
		text += $"ZL={KEYCONFIG.Button.ZL}\r\n";
		text += $"ZR={KEYCONFIG.Button.ZR}\r\n";
		text += $"START={KEYCONFIG.Button.START}\r\n";
		text += $"SELECT={KEYCONFIG.Button.SELECT}\r\n";
		text += $"CLICKL={KEYCONFIG.Button.CLICKL}\r\n";
		text += $"CLICKR={KEYCONFIG.Button.CLICKR}\r\n";
		text += $"HOME={KEYCONFIG.Button.HOME}\r\n";
		text += $"CAPTURE={KEYCONFIG.Button.CAPTURE}\r\n";
		text += "\r\n";
		text += "[DPad]\r\n";
		text += $"UP={KEYCONFIG.DPad.UP}\r\n";
		text += $"DOWN={KEYCONFIG.DPad.DOWN}\r\n";
		text += $"RIGHT={KEYCONFIG.DPad.RIGHT}\r\n";
		text += $"LEFT={KEYCONFIG.DPad.LEFT}\r\n";
		text += "\r\n";
		text += "[AnalogL]\r\n";
		text += $"UP={KEYCONFIG.AnalogL.UP}\r\n";
		text += $"DOWN={KEYCONFIG.AnalogL.DOWN}\r\n";
		text += $"RIGHT={KEYCONFIG.AnalogL.RIGHT}\r\n";
		text += $"LEFT={KEYCONFIG.AnalogL.LEFT}\r\n";
		text += "\r\n";
		text += "[AnalogR]\r\n";
		text += $"UP={KEYCONFIG.AnalogR.UP}\r\n";
		text += $"DOWN={KEYCONFIG.AnalogR.DOWN}\r\n";
		text += $"RIGHT={KEYCONFIG.AnalogR.RIGHT}\r\n";
		text += $"LEFT={KEYCONFIG.AnalogR.LEFT}\r\n";
		text += "\r\n";
		text += "[DxButton]\r\n";
		text = text + "A=" + KEYCONFIG.DxButton.A + "\r\n";
		text = text + "B=" + KEYCONFIG.DxButton.B + "\r\n";
		text = text + "X=" + KEYCONFIG.DxButton.X + "\r\n";
		text = text + "Y=" + KEYCONFIG.DxButton.Y + "\r\n";
		text = text + "L=" + KEYCONFIG.DxButton.L + "\r\n";
		text = text + "R=" + KEYCONFIG.DxButton.R + "\r\n";
		text = text + "ZL=" + KEYCONFIG.DxButton.ZL + "\r\n";
		text = text + "ZR=" + KEYCONFIG.DxButton.ZR + "\r\n";
		text = text + "START=" + KEYCONFIG.DxButton.START + "\r\n";
		text = text + "SELECT=" + KEYCONFIG.DxButton.SELECT + "\r\n";
		text = text + "CLICKL=" + KEYCONFIG.DxButton.CLICKL + "\r\n";
		text = text + "CLICKR=" + KEYCONFIG.DxButton.CLICKR + "\r\n";
		text = text + "HOME=" + KEYCONFIG.DxButton.HOME + "\r\n";
		text = text + "CAPTURE=" + KEYCONFIG.DxButton.CAPTURE + "\r\n";
		text += "\r\n";
		text += "[DxDPad]\r\n";
		text = text + "UP=" + KEYCONFIG.DxDPad.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.DxDPad.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.DxDPad.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.DxDPad.LEFT + "\r\n";
		text += "\r\n";
		text += "[DxAnalogL]\r\n";
		text = text + "UP=" + KEYCONFIG.DxAnalogL.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.DxAnalogL.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.DxAnalogL.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.DxAnalogL.LEFT + "\r\n";
		text += "\r\n";
		text += "[DxAnalogR]\r\n";
		text = text + "UP=" + KEYCONFIG.DxAnalogR.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.DxAnalogR.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.DxAnalogR.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.DxAnalogR.LEFT + "\r\n";
		text += "\r\n";
		text += "[XiButton]\r\n";
		text = text + "A=" + KEYCONFIG.XiButton.A + "\r\n";
		text = text + "B=" + KEYCONFIG.XiButton.B + "\r\n";
		text = text + "X=" + KEYCONFIG.XiButton.X + "\r\n";
		text = text + "Y=" + KEYCONFIG.XiButton.Y + "\r\n";
		text = text + "L=" + KEYCONFIG.XiButton.L + "\r\n";
		text = text + "R=" + KEYCONFIG.XiButton.R + "\r\n";
		text = text + "ZL=" + KEYCONFIG.XiButton.ZL + "\r\n";
		text = text + "ZR=" + KEYCONFIG.XiButton.ZR + "\r\n";
		text = text + "START=" + KEYCONFIG.XiButton.START + "\r\n";
		text = text + "SELECT=" + KEYCONFIG.XiButton.SELECT + "\r\n";
		text = text + "CLICKL=" + KEYCONFIG.XiButton.CLICKL + "\r\n";
		text = text + "CLICKR=" + KEYCONFIG.XiButton.CLICKR + "\r\n";
		text = text + "HOME=" + KEYCONFIG.XiButton.HOME + "\r\n";
		text = text + "CAPTURE=" + KEYCONFIG.XiButton.CAPTURE + "\r\n";
		text += "\r\n";
		text += "[XiDPad]\r\n";
		text = text + "UP=" + KEYCONFIG.XiDPad.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.XiDPad.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.XiDPad.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.XiDPad.LEFT + "\r\n";
		text += "\r\n";
		text += "[XiAnalogL]\r\n";
		text = text + "UP=" + KEYCONFIG.XiAnalogL.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.XiAnalogL.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.XiAnalogL.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.XiAnalogL.LEFT + "\r\n";
		text += "\r\n";
		text += "[XiAnalogR]\r\n";
		text = text + "UP=" + KEYCONFIG.XiAnalogR.UP + "\r\n";
		text = text + "DOWN=" + KEYCONFIG.XiAnalogR.DOWN + "\r\n";
		text = text + "RIGHT=" + KEYCONFIG.XiAnalogR.RIGHT + "\r\n";
		text = text + "LEFT=" + KEYCONFIG.XiAnalogR.LEFT + "\r\n";
		text += "\r\n";
		text += "[ControlConfig]\r\n";
		text += $"GAMEPADONLY={KEYCONFIG.ControlConfig.GAMEPADONLY}\r\n";
		text += $"NOTUSEDEACTIVATE={KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE}\r\n";
		text += $"NOTUSERUNNINGMACRO={KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO}\r\n";
		text += $"USEKEYBOARD={KEYCONFIG.ControlConfig.USEKEYBOARD}\r\n";
		text += $"USESTICKBINARY={KEYCONFIG.ControlConfig.USESTICKBINARY}\r\n";
		text += $"REC8AXIS={KEYCONFIG.ControlConfig.REC8AXIS}\r\n";
		text += "\r\n";
		text += "[AppConfig]\r\n";
		text += $"UPDATECHECK={KEYCONFIG.AppConfig.UPDATECHECK}\r\n";
		text += $"APPTHEME={KEYCONFIG.AppConfig.APPTHEME}\r\n";
		text = text + "SHORTCUTS=" + string.Join(",", GlobalVar.MacroList) + "\r\n";
		text = text + "CAPTURE=" + GlobalVar.CaptureOutput + "\r\n";
		text += $"CAPTURESTYLE={KEYCONFIG.AppConfig.CAPTURESTYLE}\r\n";
		text += $"LASTVER={GlobalVar.Ver}\r\n";
		text += "\r\n";
		text += "[EditorConfig]\r\n";
		text += $"RUNNINGFOCUS={KEYCONFIG.EditorConfig.RUNNINGFOCUS}\r\n";
		text += "\r\n";
		text += "[NetworkConfig]\r\n";
		text = text + "FAVORITE=" + string.Join(",", GlobalVar.FavMacro) + "\r\n";
		text = text + "BLACKLIST=" + string.Join(",", GlobalVar.BlackList) + "\r\n";
		text = text + "ID=" + KEYCONFIG.NetworkConfig.ID + "\r\n";
		text = text + "KEY=" + KEYCONFIG.NetworkConfig.KEY + "\r\n";
		text = text + "LINETOKEN=" + NxCommand.LineNotifyToken + "\r\n";
		File.WriteAllText(GlobalVar.BasePath + "config.ini", text, Encoding.Default);
	}

	public static void ToDownload(this string url, string fileName)
	{
		Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream();
		using FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
		byte[] array = new byte[1024];
		int count;
		while ((count = responseStream.Read(array, 0, array.Length)) > 0)
		{
			fileStream.Write(array, 0, count);
		}
	}

	public static void PutFilesOnClipboard(this IEnumerable<FileSystemInfo> filesAndFolders, bool moveFilesOnPaste = false)
	{
		try
		{
			if (moveFilesOnPaste)
			{
				DragDropEffects value = ((!moveFilesOnPaste) ? DragDropEffects.Copy : DragDropEffects.Move);
				StringCollection stringCollection = new StringCollection();
				stringCollection.AddRange(filesAndFolders.Select((FileSystemInfo x) => x.FullName).ToArray());
				DataObject dataObject = new DataObject();
				dataObject.SetFileDropList(stringCollection);
				dataObject.SetData("Preferred DropEffect", new MemoryStream(BitConverter.GetBytes((int)value)));
				Clipboard.SetDataObject(dataObject, copy: true);
			}
			else
			{
				StringCollection stringCollection2 = new StringCollection();
				stringCollection2.AddRange(filesAndFolders.Select((FileSystemInfo x) => x.FullName).ToArray());
				Clipboard.SetFileDropList(stringCollection2);
			}
		}
		catch (Exception)
		{
		}
	}

	public static void PasteFiles(string destDir)
	{
		IDataObject dataObject = Clipboard.GetDataObject();
		if (dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop))
		{
			string[] sourceFiles = (string[])dataObject.GetData(DataFormats.FileDrop);
			switch (GetPreferredDropEffect(dataObject))
			{
			case DragDropEffects.None:
			case DragDropEffects.Copy | DragDropEffects.Link:
				CopyFilesToDirectory(sourceFiles, destDir, move: false);
				break;
			case DragDropEffects.Move:
				CopyFilesToDirectory(sourceFiles, destDir, move: true);
				break;
			}
		}
	}

	public static bool CheckClipboardFiles()
	{
		try
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop))
			{
				_ = (string[])dataObject.GetData(DataFormats.FileDrop);
				DragDropEffects preferredDropEffect = GetPreferredDropEffect(dataObject);
				if (preferredDropEffect == (DragDropEffects.Copy | DragDropEffects.Link) || preferredDropEffect == DragDropEffects.None || preferredDropEffect == DragDropEffects.Move)
				{
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static IReadOnlyList<string> GetClipboardImagePathList()
	{
		try
		{
			if (!CheckClipboardImages())
			{
				return null;
			}
			List<string> list = new List<string>();
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop))
			{
				string[] array = (string[])dataObject.GetData(DataFormats.FileDrop);
				DragDropEffects preferredDropEffect = GetPreferredDropEffect(dataObject);
				if (preferredDropEffect == (DragDropEffects.Copy | DragDropEffects.Link) || preferredDropEffect == DragDropEffects.None)
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						if (GetFilePathType(text) == FilePathType.File)
						{
							list.Add(text);
							continue;
						}
						return null;
					}
					return list;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static bool CheckClipboardImages()
	{
		try
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop))
			{
				string[] array = (string[])dataObject.GetData(DataFormats.FileDrop);
				DragDropEffects preferredDropEffect = GetPreferredDropEffect(dataObject);
				if (preferredDropEffect == (DragDropEffects.Copy | DragDropEffects.Link) || preferredDropEffect == DragDropEffects.None)
				{
					string[] array2 = array;
					foreach (string text in array2)
					{
						FilePathType filePathType = GetFilePathType(text);
						Path.GetFileName(text);
						if (filePathType == FilePathType.File)
						{
							try
							{
								if (!IsValidImageFile(text))
								{
									return false;
								}
							}
							catch
							{
								return false;
							}
							continue;
						}
						return false;
					}
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static DragDropEffects GetPreferredDropEffect(IDataObject data)
	{
		DragDropEffects result = DragDropEffects.None;
		if (data != null)
		{
			MemoryStream memoryStream = (MemoryStream)data.GetData("Preferred DropEffect");
			if (memoryStream != null)
			{
				result = (DragDropEffects)memoryStream.ReadByte();
			}
		}
		return result;
	}

	public static void CopyFilesToDirectory(string[] sourceFiles, string destDir, bool move)
	{
		foreach (string text in sourceFiles)
		{
			FilePathType filePathType = GetFilePathType(text);
			string fileName = Path.GetFileName(text);
			string path = Path.Combine(destDir, fileName);
			switch (filePathType)
			{
			case FilePathType.File:
			{
				byte[] bytes2 = File.ReadAllBytes(text);
				File.WriteAllBytes(path, bytes2);
				if (move)
				{
					File.Delete(text);
				}
				break;
			}
			case FilePathType.Directory:
			{
				string[] files = Directory.GetFiles(text, "*", SearchOption.AllDirectories);
				foreach (string text2 in files)
				{
					string path2 = Path.Combine(destDir, GetRelativePath(Path.GetDirectoryName(text), text2).Substring(2));
					byte[] bytes = File.ReadAllBytes(text2);
					Directory.CreateDirectory(Path.GetDirectoryName(path2));
					File.WriteAllBytes(path2, bytes);
				}
				if (move)
				{
					Delete(text);
				}
				break;
			}
			}
		}
	}

	private static bool IsValidImageFile(string file)
	{
		byte[] array = new byte[8];
		byte[] array2 = new byte[2];
		try
		{
			using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				if (fileStream.Length > array.Length)
				{
					fileStream.Read(array, 0, array.Length);
					fileStream.Position = (int)fileStream.Length - array2.Length;
					fileStream.Read(array2, 0, array2.Length);
				}
				fileStream.Close();
			}
			if (ByteArrayStartsWith(array, bmp) || ByteArrayStartsWith(array, gif87a) || ByteArrayStartsWith(array, gif89a) || ByteArrayStartsWith(array, png) || ByteArrayStartsWith(array, tiffI) || ByteArrayStartsWith(array, tiffM))
			{
				return true;
			}
			if (ByteArrayStartsWith(array, jpeg) && ByteArrayStartsWith(array2, jpegEnd))
			{
				return true;
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	private static bool ByteArrayStartsWith(byte[] a, byte[] b)
	{
		if (a.Length < b.Length)
		{
			return false;
		}
		for (int i = 0; i < b.Length; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	public static void Delete(string targetDirectoryPath)
	{
		if (Directory.Exists(targetDirectoryPath))
		{
			string[] files = Directory.GetFiles(targetDirectoryPath);
			foreach (string path in files)
			{
				File.SetAttributes(path, FileAttributes.Normal);
				File.Delete(path);
			}
			files = Directory.GetDirectories(targetDirectoryPath);
			for (int i = 0; i < files.Length; i++)
			{
				Delete(files[i]);
			}
			Directory.Delete(targetDirectoryPath, recursive: false);
		}
	}

	public static Image CreateGammaAdjustedImage(Image img, float gammaValue)
	{
		Bitmap bitmap = new Bitmap(img.Width, img.Height);
		Graphics graphics = Graphics.FromImage(bitmap);
		ImageAttributes imageAttributes = new ImageAttributes();
		imageAttributes.SetGamma(1f / gammaValue);
		graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
		graphics.Dispose();
		return bitmap;
	}

	[DllImport("user32.dll")]
	private static extern bool SetProcessDPIAware();

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr hwnd);

	[DllImport("gdi32.dll")]
	private static extern int GetDeviceCaps(IntPtr hdc, int index);

	[DllImport("user32.dll")]
	private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	public static float GetDisplayScale()
	{
		return new Form().CreateGraphics().DpiX / 96f;
	}

	public static Bitmap AdjustBrightness(Image img, int brightness, Bitmap img2 = null)
	{
		if (img2 == null)
		{
			img2 = new Bitmap(img.Width, img.Height, img.PixelFormat);
		}
		Graphics graphics = Graphics.FromImage(img2);
		float num = (float)brightness / 255f;
		ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
		{
			new float[5] { 1f, 0f, 0f, 0f, 0f },
			new float[5] { 0f, 1f, 0f, 0f, 0f },
			new float[5] { 0f, 0f, 1f, 0f, 0f },
			new float[5] { 0f, 0f, 0f, 1f, 0f },
			new float[5] { num, num, num, 0f, 1f }
		});
		ImageAttributes imageAttributes = new ImageAttributes();
		imageAttributes.SetColorMatrix(colorMatrix);
		graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
		graphics.Dispose();
		return img2;
	}

	public static string StrCmp(this string code, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null, List<(string, string, int)> varDatas3 = null, List<(string, bool, int)> varDatas4 = null)
	{
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("(");
		list._nodeSplit(")");
		list._nodeSplit("!=");
		list._nodeSplit("==");
		list._nodeSplit(">=");
		list._nodeSplit("<=");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeSplit("&&");
		list._nodeSplit("||");
		list._nodeSplit(">");
		list._nodeSplit("<");
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = list[i].Trim();
			if (list[i] == "")
			{
				list.RemoveAt(i);
				i--;
			}
		}
		bool flag = false;
		if (varDatas != null)
		{
			for (int j = 0; j < varDatas.Count; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] == varDatas[j].Item1)
					{
						list[k] = varDatas[j].Item2.ToString();
						flag = true;
					}
				}
			}
		}
		if (varDatas2 != null)
		{
			for (int l = 0; l < varDatas2.Count; l++)
			{
				for (int m = 0; m < list.Count; m++)
				{
					if (list[m] == varDatas2[l].Item1)
					{
						list[m] = varDatas2[l].Item2.ToString();
					}
				}
			}
		}
		if (varDatas3 != null)
		{
			for (int n = 0; n < varDatas3.Count; n++)
			{
				for (int num = 0; num < list.Count; num++)
				{
					if (list[num] == varDatas3[n].Item1)
					{
						list[num] = varDatas3[n].Item2;
					}
				}
			}
		}
		if (varDatas4 != null)
		{
			for (int num2 = 0; num2 < varDatas4.Count; num2++)
			{
				for (int num3 = 0; num3 < list.Count; num3++)
				{
					if (list[num3] == varDatas4[num2].Item1)
					{
						list[num3] = varDatas4[num2].Item2.ToString();
					}
				}
			}
		}
		code = string.Join("", list);
		if (flag)
		{
			return code._strCmpDecimal();
		}
		return code._strCmp();
	}

	private static string _strCmpDecimal(this string code)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num2 != -1 && num3 != -1)
		{
			num = 0;
			num2 = -1;
			num3 = -1;
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i] == '(')
				{
					num++;
					if (num2 == -1)
					{
						num2 = i;
					}
				}
				if (code[i] == ')')
				{
					num--;
					if (num == 0)
					{
						num3 = i;
						break;
					}
				}
			}
			if (num2 != -1 && num3 != -1)
			{
				string code2 = code.Substring(num2 + 1, num3 - num2 - 1);
				string text = code.Substring(0, num2);
				string text2 = code.Substring(num3 + 1);
				string text3 = code2._strCmp();
				code = text + text3 + text2;
			}
		}
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("!=");
		list._nodeSplit("==");
		list._nodeSplit(">=");
		list._nodeSplit("<=");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeSplit("&&");
		list._nodeSplit("||");
		list._nodeSplit(">");
		list._nodeSplit("<");
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		list._nodeClean();
		list._nodeCalcRankDecimal4();
		list._nodeCalcRankDecimal5();
		list._nodeCmpDecimal();
		return list[0];
	}

	private static string _strCmp(this string code)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num2 != -1 && num3 != -1)
		{
			num = 0;
			num2 = -1;
			num3 = -1;
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i] == '(')
				{
					num++;
					if (num2 == -1)
					{
						num2 = i;
					}
				}
				if (code[i] == ')')
				{
					num--;
					if (num == 0)
					{
						num3 = i;
						break;
					}
				}
			}
			if (num2 != -1 && num3 != -1)
			{
				string code2 = code.Substring(num2 + 1, num3 - num2 - 1);
				string text = code.Substring(0, num2);
				string text2 = code.Substring(num3 + 1);
				string text3 = code2._strCmp();
				code = text + text3 + text2;
			}
		}
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("!=");
		list._nodeSplit("==");
		list._nodeSplit(">=");
		list._nodeSplit("<=");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeSplit("&&");
		list._nodeSplit("||");
		list._nodeSplit(">");
		list._nodeSplit("<");
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		list._nodeClean();
		list._nodeCalcRank4();
		list._nodeCalcRank5();
		list._nodeCalcRank6();
		list._nodeCalcRank9();
		list._nodeCalcRank10();
		list._nodeCalcRank11();
		list._nodeCmp();
		return list[0];
	}

	private static void _nodeCmpDecimal(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == ">=")
			{
				bool flag = decimal.Parse(numList[i - 1]) >= decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag.ToString());
				i--;
			}
			else if (numList[i] == "<=")
			{
				bool flag2 = decimal.Parse(numList[i - 1]) <= decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag2.ToString());
				i--;
			}
			else if (numList[i] == ">")
			{
				bool flag3 = decimal.Parse(numList[i - 1]) > decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag3.ToString());
				i--;
			}
			else if (numList[i] == "<")
			{
				bool flag4 = decimal.Parse(numList[i - 1]) < decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag4.ToString());
				i--;
			}
		}
		for (int j = 1; j < numList.Count; j++)
		{
			decimal result;
			if (numList[j] == "!=")
			{
				if (_isString(numList[j - 1]) && _isString(numList[j + 1]))
				{
					bool flag5 = numList[j - 1] != numList[j + 1];
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag5.ToString());
					j--;
				}
				else if (decimal.TryParse(numList[j - 1], out result))
				{
					bool flag6 = decimal.Parse(numList[j - 1]) != decimal.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag6.ToString());
					j--;
				}
				else
				{
					bool flag7 = bool.Parse(numList[j - 1]) != bool.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag7.ToString());
					j--;
				}
			}
			else if (numList[j] == "==")
			{
				if (_isString(numList[j - 1]) && _isString(numList[j + 1]))
				{
					bool flag8 = numList[j - 1] == numList[j + 1];
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag8.ToString());
					j--;
				}
				else if (decimal.TryParse(numList[j - 1], out result))
				{
					bool flag9 = decimal.Parse(numList[j - 1]) == decimal.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag9.ToString());
					j--;
				}
				else
				{
					bool flag10 = bool.Parse(numList[j - 1]) == bool.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag10.ToString());
					j--;
				}
			}
		}
		for (int k = 1; k < numList.Count; k++)
		{
			if (numList[k] == "&&")
			{
				bool flag11 = bool.Parse(numList[k - 1]) && bool.Parse(numList[k + 1]);
				numList.RemoveRange(k - 1, 3);
				numList.Insert(k - 1, flag11.ToString());
				k--;
			}
			else if (numList[k] == "||")
			{
				bool flag12 = bool.Parse(numList[k - 1]) || bool.Parse(numList[k + 1]);
				numList.RemoveRange(k - 1, 3);
				numList.Insert(k - 1, flag12.ToString());
				k--;
			}
		}
	}

	private static void _nodeCmp(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == ">=")
			{
				bool flag = long.Parse(numList[i - 1]) >= long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag.ToString());
				i--;
			}
			else if (numList[i] == "<=")
			{
				bool flag2 = long.Parse(numList[i - 1]) <= long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag2.ToString());
				i--;
			}
			else if (numList[i] == ">")
			{
				bool flag3 = long.Parse(numList[i - 1]) > long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag3.ToString());
				i--;
			}
			else if (numList[i] == "<")
			{
				bool flag4 = long.Parse(numList[i - 1]) < long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, flag4.ToString());
				i--;
			}
		}
		for (int j = 1; j < numList.Count; j++)
		{
			long result;
			if (numList[j] == "!=")
			{
				if (_isString(numList[j - 1]) && _isString(numList[j + 1]))
				{
					bool flag5 = numList[j - 1] != numList[j + 1];
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag5.ToString());
					j--;
				}
				else if (long.TryParse(numList[j - 1], out result))
				{
					bool flag6 = long.Parse(numList[j - 1]) != long.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag6.ToString());
					j--;
				}
				else
				{
					bool flag7 = bool.Parse(numList[j - 1]) != bool.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag7.ToString());
					j--;
				}
			}
			else if (numList[j] == "==")
			{
				if (_isString(numList[j - 1]) && _isString(numList[j + 1]))
				{
					bool flag8 = numList[j - 1] == numList[j + 1];
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag8.ToString());
					j--;
				}
				else if (long.TryParse(numList[j - 1], out result))
				{
					bool flag9 = long.Parse(numList[j - 1]) == long.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag9.ToString());
					j--;
				}
				else
				{
					bool flag10 = bool.Parse(numList[j - 1]) == bool.Parse(numList[j + 1]);
					numList.RemoveRange(j - 1, 3);
					numList.Insert(j - 1, flag10.ToString());
					j--;
				}
			}
		}
		for (int k = 1; k < numList.Count; k++)
		{
			if (numList[k] == "&&")
			{
				bool flag11 = bool.Parse(numList[k - 1]) && bool.Parse(numList[k + 1]);
				numList.RemoveRange(k - 1, 3);
				numList.Insert(k - 1, flag11.ToString());
				k--;
			}
			else if (numList[k] == "||")
			{
				bool flag12 = bool.Parse(numList[k - 1]) || bool.Parse(numList[k + 1]);
				numList.RemoveRange(k - 1, 3);
				numList.Insert(k - 1, flag12.ToString());
				k--;
			}
		}
	}

	public static string StrCalc(this string code, List<(string, decimal, int)> varDatas = null, List<(string, long, int)> varDatas2 = null, List<(string, string, int)> varDatas3 = null, List<(string, bool, int)> varDatas4 = null, bool isDecimal = false)
	{
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("(");
		list._nodeSplit(")");
		list._nodeSplit("!=");
		list._nodeSplit("==");
		list._nodeSplit(">=");
		list._nodeSplit("<=");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeSplit("&&");
		list._nodeSplit("||");
		list._nodeSplit(">");
		list._nodeSplit("<");
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = list[i].Trim();
			if (list[i] == "")
			{
				list.RemoveAt(i);
				i--;
			}
		}
		if (varDatas != null)
		{
			for (int j = 0; j < varDatas.Count; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k] == varDatas[j].Item1)
					{
						list[k] = varDatas[j].Item2.ToString();
						isDecimal = true;
					}
				}
			}
		}
		if (varDatas2 != null)
		{
			for (int l = 0; l < varDatas2.Count; l++)
			{
				for (int m = 0; m < list.Count; m++)
				{
					if (list[m] == varDatas2[l].Item1)
					{
						list[m] = varDatas2[l].Item2.ToString();
					}
				}
			}
		}
		if (varDatas3 != null)
		{
			for (int n = 0; n < varDatas3.Count; n++)
			{
				for (int num = 0; num < list.Count; num++)
				{
					if (list[num] == varDatas3[n].Item1)
					{
						list[num] = varDatas3[n].Item2;
					}
				}
			}
		}
		if (varDatas4 != null)
		{
			for (int num2 = 0; num2 < varDatas4.Count; num2++)
			{
				for (int num3 = 0; num3 < list.Count; num3++)
				{
					if (list[num3] == varDatas4[num2].Item1)
					{
						list[num3] = varDatas4[num2].Item2.ToString();
					}
				}
			}
		}
		code = string.Join("", list);
		if (isDecimal)
		{
			return code._strCalcDecimal();
		}
		return code._strCalc();
	}

	private static string _strCalcDecimal(this string code)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num2 != -1 && num3 != -1)
		{
			num = 0;
			num2 = -1;
			num3 = -1;
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i] == '(')
				{
					num++;
					if (num2 == -1)
					{
						num2 = i;
					}
				}
				if (code[i] == ')')
				{
					num--;
					if (num == 0)
					{
						num3 = i;
						break;
					}
				}
			}
			if (num2 != -1 && num3 != -1)
			{
				string code2 = code.Substring(num2 + 1, num3 - num2 - 1);
				code = code.Substring(0, num2) + code2._strCalc() + code.Substring(num3 + 1);
			}
		}
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeClean();
		list._nodeCalcRankDecimal4();
		list._nodeCalcRankDecimal5();
		return list[0];
	}

	private static string _strCalc(this string code)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		while (num2 != -1 && num3 != -1)
		{
			num = 0;
			num2 = -1;
			num3 = -1;
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i] == '(')
				{
					num++;
					if (num2 == -1)
					{
						num2 = i;
					}
				}
				if (code[i] == ')')
				{
					num--;
					if (num == 0)
					{
						num3 = i;
						break;
					}
				}
			}
			if (num2 != -1 && num3 != -1)
			{
				string code2 = code.Substring(num2 + 1, num3 - num2 - 1);
				code = code.Substring(0, num2) + code2._strCalc() + code.Substring(num3 + 1);
			}
		}
		code = code.Trim();
		List<string> list = numList_StrCalc;
		list.Clear();
		list.Add(code);
		list._strSplit();
		list._nodeSplit("%");
		list._nodeSplit("/");
		list._nodeSplit("*");
		list._nodeSplit("+");
		list._nodeSplit("-");
		list._nodeSplit("|");
		list._nodeSplit("^");
		list._nodeSplit("&");
		list._nodeSplit("<<");
		list._nodeSplit(">>");
		list._nodeClean();
		list._nodeCalcRank4();
		list._nodeCalcRank5();
		list._nodeCalcRank6();
		list._nodeCalcRank9();
		list._nodeCalcRank10();
		list._nodeCalcRank11();
		return list[0];
	}

	private static void _nodeClean(this List<string> numList)
	{
		for (int i = 0; i < numList.Count; i++)
		{
			numList[i] = numList[i].Trim();
			if (numList[i] == "")
			{
				numList.RemoveAt(i);
				i--;
			}
		}
		while (numList[0] == "-")
		{
			if (numList.Count > 1)
			{
				if (numList[1] == "+")
				{
					numList.RemoveAt(1);
				}
				else if (numList[1] == "-")
				{
					numList.RemoveAt(0);
					numList.RemoveAt(0);
				}
				else
				{
					numList[0] = "-" + numList[1];
					numList.RemoveAt(1);
				}
			}
		}
		while (numList[0] == "+")
		{
			numList.RemoveAt(0);
		}
		decimal result2;
		for (int j = 1; j < numList.Count - 1; j++)
		{
			if (numList[j] == "+" && !_isString(numList[j - 1]) && !bool.TryParse(numList[j - 1], out var _) && !decimal.TryParse(numList[j - 1], out result2))
			{
				numList[j] = numList[j + 1];
				numList.RemoveAt(j + 1);
			}
		}
		for (int k = 1; k < numList.Count - 1; k++)
		{
			if (numList[k] == "-" && !decimal.TryParse(numList[k - 1], out result2))
			{
				numList[k] = "-" + numList[k + 1];
				numList.RemoveAt(k + 1);
			}
		}
	}

	private static void _strSplit(this List<string> numList)
	{
		int num;
		for (num = 0; num < numList.Count; num++)
		{
			List<string> list = numList[num].Split(new string[1] { "\"" }, StringSplitOptions.None).ToList();
			int count = list.Count;
			for (int i = 0; i < count - 1; i++)
			{
				list.Insert(i * 2 + 1, "\"");
			}
			numList.RemoveAt(num);
			numList.InsertRange(num, list);
			num += list.Count - 1;
		}
		for (int j = 0; j < numList.Count; j++)
		{
			if (!(numList[j] == "\""))
			{
				continue;
			}
			for (int k = j + 1; k < numList.Count; k++)
			{
				numList[j] += numList[k];
				if (numList[k] == "\"")
				{
					for (int l = j; l < k; l++)
					{
						numList.RemoveAt(j + 1);
					}
					break;
				}
			}
		}
	}

	private static void _nodeSplit(this List<string> numList, string node)
	{
		for (int i = 0; i < numList.Count; i++)
		{
			string text = numList[i].Trim();
			if (text.Length > 1 && _isString(text))
			{
				continue;
			}
			switch (text)
			{
			case "||":
			case "&&":
			case "==":
			case "!=":
			case ">=":
			case "<=":
			case ">>":
			case "<<":
				continue;
			}
			List<string> list = numList[i].Split(new string[1] { node }, StringSplitOptions.None).ToList();
			int count = list.Count;
			for (int j = 0; j < count - 1; j++)
			{
				list.Insert(j * 2 + 1, node);
			}
			numList.RemoveAt(i);
			numList.InsertRange(i, list);
			i += list.Count - 1;
		}
	}

	private static void _nodeCalcRank4(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "%")
			{
				long num = long.Parse(numList[i - 1]) % long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
			else if (numList[i] == "/")
			{
				long num2 = long.Parse(numList[i - 1]) / long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num2.ToString());
				i--;
			}
			else if (numList[i] == "*")
			{
				long num3 = long.Parse(numList[i - 1]) * long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num3.ToString());
				i--;
			}
		}
	}

	private static void _nodeCalcRankDecimal4(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "%")
			{
				decimal num = decimal.Parse(numList[i - 1]) % decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
			else if (numList[i] == "/")
			{
				decimal num2 = decimal.Parse(numList[i - 1]) / decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num2.ToString());
				i--;
			}
			else if (numList[i] == "*")
			{
				decimal num3 = decimal.Parse(numList[i - 1]) * decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num3.ToString());
				i--;
			}
		}
	}

	private static void _nodeCalcRank5(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "+")
			{
				if (!_isString(numList[i - 1]) && !_isString(numList[i + 1]))
				{
					long num = long.Parse(numList[i - 1]) + long.Parse(numList[i + 1]);
					numList.RemoveRange(i - 1, 3);
					numList.Insert(i - 1, num.ToString());
					i--;
				}
			}
			else if (numList[i] == "-")
			{
				long num2 = long.Parse(numList[i - 1]) - long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num2.ToString());
				i--;
			}
		}
		for (int j = 1; j < numList.Count; j++)
		{
			if (numList[j] == "+" && (_isString(numList[j - 1]) || _isString(numList[j + 1])))
			{
				string text = _setdq(numList[j - 1].RemoveDQ() + numList[j + 1].RemoveDQ());
				numList.RemoveRange(j - 1, 3);
				numList.Insert(j - 1, text.ToString());
				j--;
			}
		}
	}

	private static bool _isString(string str)
	{
		if (str[0] == '"' && str[str.Length - 1] == '"')
		{
			return true;
		}
		return false;
	}

	public static string RemoveDQ(this string str)
	{
		if (!_isString(str))
		{
			return str;
		}
		return str.Substring(1, str.Length - 2);
	}

	private static string _setdq(string str)
	{
		return "\"" + str + "\"";
	}

	private static void _nodeCalcRankDecimal5(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "+")
			{
				if (!_isString(numList[i - 1]) && !_isString(numList[i + 1]))
				{
					decimal num = decimal.Parse(numList[i - 1]) + decimal.Parse(numList[i + 1]);
					numList.RemoveRange(i - 1, 3);
					numList.Insert(i - 1, num.ToString());
					i--;
				}
			}
			else if (numList[i] == "-")
			{
				decimal num2 = decimal.Parse(numList[i - 1]) - decimal.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num2.ToString());
				i--;
			}
		}
		for (int j = 1; j < numList.Count; j++)
		{
			if (numList[j] == "+" && (_isString(numList[j - 1]) || _isString(numList[j + 1])))
			{
				string text = _setdq(numList[j - 1].RemoveDQ() + numList[j + 1].RemoveDQ());
				numList.RemoveRange(j - 1, 3);
				numList.Insert(j - 1, text.ToString());
				j--;
			}
		}
	}

	private static void _nodeCalcRank6(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "<<")
			{
				long num = long.Parse(numList[i - 1]) << int.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
			else if (numList[i] == ">>")
			{
				long num2 = long.Parse(numList[i - 1]) >> int.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num2.ToString());
				i--;
			}
		}
	}

	private static void _nodeCalcRank9(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "&")
			{
				long num = long.Parse(numList[i - 1]) & long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
		}
	}

	private static void _nodeCalcRank10(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "^")
			{
				long num = long.Parse(numList[i - 1]) ^ long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
		}
	}

	private static void _nodeCalcRank11(this List<string> numList)
	{
		for (int i = 1; i < numList.Count; i++)
		{
			if (numList[i] == "|")
			{
				long num = long.Parse(numList[i - 1]) | long.Parse(numList[i + 1]);
				numList.RemoveRange(i - 1, 3);
				numList.Insert(i - 1, num.ToString());
				i--;
			}
		}
	}

	public static void WriteLine(int text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(long text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(uint text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(ulong text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(double text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(float text)
	{
		WriteLine(text.ToString());
	}

	public static void WriteLine(string text)
	{
		if (GlobalVar.debug)
		{
			Console.WriteLine(text);
		}
	}

	public static void Write(string text)
	{
		if (GlobalVar.debug)
		{
			Console.Write(text);
		}
	}
}

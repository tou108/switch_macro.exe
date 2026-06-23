using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace NX_Macro_Controller_VxV;

public static class GlobalVar
{
	public static readonly string AppName = "NX Macro Controller ver2.13";

	public static readonly string VerName = "2.13";

	public static readonly string BasePath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);

	public static readonly int Ver = 20130;

	public static readonly string ChLog = "";

	public static readonly List<int> FavMacro = new List<int>();

	public static readonly List<string> BlackList = new List<string>();

	public static readonly List<string> MacroList = new List<string>();

	public static NXMC_VxV MAINFORM;

	public static string[] TaskName = new string[30];

	public static string Server = "http://bzl-web.com:8737";

	public static string CaptureOutput = BasePath + "Captures";

	public static int LastVer = 0;

	public static bool debug = false;

	public static bool debugBuild = false;

	public static bool HighLightLineSet = false;

	public static int HighLightLine = 0;

	public static Thread NmcThread = null;

	public static MemoryMappedViewAccessor ShareController;

	public static MemoryMappedViewAccessor ShareVideoRam;
}

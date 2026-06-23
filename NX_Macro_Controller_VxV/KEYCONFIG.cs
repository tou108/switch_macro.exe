using System.Windows.Forms;
using BZComponent;

namespace NX_Macro_Controller_VxV;

public static class KEYCONFIG
{
	public static class Button
	{
		public static Keys A;

		public static Keys B;

		public static Keys X;

		public static Keys Y;

		public static Keys L;

		public static Keys R;

		public static Keys ZL;

		public static Keys ZR;

		public static Keys CLICKL;

		public static Keys CLICKR;

		public static Keys START;

		public static Keys SELECT;

		public static Keys HOME;

		public static Keys CAPTURE;
	}

	public static class DPad
	{
		public static Keys UP;

		public static Keys DOWN;

		public static Keys LEFT;

		public static Keys RIGHT;
	}

	public static class AnalogL
	{
		public static Keys UP;

		public static Keys DOWN;

		public static Keys LEFT;

		public static Keys RIGHT;
	}

	public static class AnalogR
	{
		public static Keys UP;

		public static Keys DOWN;

		public static Keys LEFT;

		public static Keys RIGHT;
	}

	public static class DxButton
	{
		public static string A = "None";

		public static string B = "None";

		public static string X = "None";

		public static string Y = "None";

		public static string L = "None";

		public static string R = "None";

		public static string ZL = "None";

		public static string ZR = "None";

		public static string CLICKL = "None";

		public static string CLICKR = "None";

		public static string START = "None";

		public static string SELECT = "None";

		public static string HOME = "None";

		public static string CAPTURE = "None";
	}

	public static class DxDPad
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class DxAnalogL
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class DxAnalogR
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class XiButton
	{
		public static string A = "None";

		public static string B = "None";

		public static string X = "None";

		public static string Y = "None";

		public static string L = "None";

		public static string R = "None";

		public static string ZL = "None";

		public static string ZR = "None";

		public static string CLICKL = "None";

		public static string CLICKR = "None";

		public static string START = "None";

		public static string SELECT = "None";

		public static string HOME = "None";

		public static string CAPTURE = "None";
	}

	public static class XiDPad
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class XiAnalogL
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class XiAnalogR
	{
		public static string UP = "None";

		public static string DOWN = "None";

		public static string LEFT = "None";

		public static string RIGHT = "None";
	}

	public static class EditorConfig
	{
		public static bool RUNNINGFOCUS;
	}

	public static class NetworkConfig
	{
		public static string KEY = "";

		public static string ID = "";
	}

	public static class ControlConfig
	{
		public static bool USEKEYBOARD = true;

		public static bool NOTUSERUNNINGMACRO;

		public static bool NOTUSEDEACTIVATE;

		public static bool GAMEPADONLY = true;

		public static bool USESTICKBINARY = false;

		public static bool REC8AXIS = false;
	}

	public static class AppConfig
	{
		public static bool UPDATECHECK = true;

		public static Style APPTHEME = Style.Dark;

		public static NXMC_VxV.CaptureStyle CAPTURESTYLE = NXMC_VxV.CaptureStyle.DirectShow;
	}
}

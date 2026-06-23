using System;
using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

public static class ProController
{
	private static Joycon _joy;

	public static bool GetButtonFlag(Joycon.Button b)
	{
		return _joy.GetButton(b);
	}

	public static float[] GetStickL()
	{
		return _joy.GetStick();
	}

	public static float[] GetStickR()
	{
		return _joy.GetStick2();
	}

	public static bool Connected()
	{
		return _joy.state > Joycon.state_.DROPPED;
	}

	public static bool Open()
	{
		IntPtr intPtr = HIDapi.hid_enumerate(1406, 8201);
		_ = new byte[1];
		if (intPtr != IntPtr.Zero)
		{
			HIDapi.hid_device_info hid_device_info = (HIDapi.hid_device_info)Marshal.PtrToStructure(intPtr, typeof(HIDapi.hid_device_info));
			_joy = new Joycon(HIDapi.hid_open_path(hid_device_info.path), imu: false, localize: false, 0.05f, left: true, hid_device_info.path, hid_device_info.serial_number, 0, isPro: true);
			for (int i = 0; i < 10; i++)
			{
				if (_joy.Attach(0) == 0)
				{
					break;
				}
				if (i == 9)
				{
					return false;
				}
			}
			_joy.Begin();
			return true;
		}
		return false;
	}

	public static void Close()
	{
		_joy.Detach();
	}
}

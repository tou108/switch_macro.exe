using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NX_Macro_Controller_VxV;

public class HIDDevice : IDisposable
{
	private IntPtr hDev = IntPtr.Zero;

	public uint PacketSize { get; private set; }

	public bool DeviceReady
	{
		get
		{
			if (hDev.ToInt32() <= -1)
			{
				return false;
			}
			if (hDev.Equals(IntPtr.Zero))
			{
				return false;
			}
			return true;
		}
	}

	public HIDDevice()
	{
		PacketSize = 64u;
	}

	public bool Open(uint vid, uint pid)
	{
		string value = $"\\\\?\\hid#vid_{vid:x4}&pid_{pid:x4}";
		Guid lpHidGuid = default(Guid);
		Native.HidD_GetHidGuid(ref lpHidGuid);
		IntPtr deviceInfoSet = Native.SetupDiGetClassDevs(ref lpHidGuid, 0, IntPtr.Zero, 18);
		Native.SP_DEVICE_INTERFACE_DATA sP_DEVICE_INTERFACE_DATA = new Native.SP_DEVICE_INTERFACE_DATA();
		sP_DEVICE_INTERFACE_DATA.cbSize = Marshal.SizeOf(sP_DEVICE_INTERFACE_DATA);
		int num = 0;
		while (Native.SetupDiEnumDeviceInterfaces(deviceInfoSet, null, ref lpHidGuid, num, sP_DEVICE_INTERFACE_DATA))
		{
			num++;
			Native.SP_DEVINFO_DATA sP_DEVINFO_DATA = new Native.SP_DEVINFO_DATA();
			sP_DEVINFO_DATA.cbSize = Marshal.SizeOf(sP_DEVINFO_DATA);
			int requiredSize = 0;
			Native.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, sP_DEVICE_INTERFACE_DATA, IntPtr.Zero, 0, ref requiredSize, sP_DEVINFO_DATA);
			IntPtr intPtr = Marshal.AllocHGlobal(requiredSize);
			Marshal.StructureToPtr(new Native.SP_DEVICE_INTERFACE_DETAIL_DATA
			{
				cbSize = Marshal.SizeOf(typeof(Native.SP_DEVICE_INTERFACE_DETAIL_DATA))
			}, intPtr, fDeleteOld: false);
			Native.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, sP_DEVICE_INTERFACE_DATA, intPtr, requiredSize, ref requiredSize, sP_DEVINFO_DATA);
			string text = Marshal.PtrToStringAuto((IntPtr)((int)intPtr + Marshal.SizeOf(typeof(int))));
			Marshal.FreeHGlobal(intPtr);
			if (text.IndexOf(value) == 0)
			{
				hDev = Native.CreateFile(text, 3221225472u, 0u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
				if (hDev.ToInt32() > -1)
				{
					break;
				}
				Native.CloseHandle(hDev);
			}
		}
		Native.SetupDiDestroyDeviceInfoList(deviceInfoSet);
		if (hDev.ToInt32() <= -1)
		{
			hDev = IntPtr.Zero;
			throw new InvalidOperationException(GetErrorMessage());
		}
		if (DeviceReady)
		{
			return true;
		}
		return false;
	}

	public void Dispose()
	{
		Close();
	}

	public void Close()
	{
		if (DeviceReady)
		{
			Native.CloseHandle(hDev);
			hDev = IntPtr.Zero;
		}
	}

	~HIDDevice()
	{
		Close();
	}

	private string GetErrorMessage()
	{
		int lastWin32Error = Marshal.GetLastWin32Error();
		StringBuilder stringBuilder = new StringBuilder(255);
		Native.FormatMessage(4096u, IntPtr.Zero, (uint)lastWin32Error, 0u, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
		return stringBuilder.ToString();
	}

	public void Send(params byte[] data)
	{
		if (!DeviceReady)
		{
			throw new InvalidOperationException("デバイスがオープンされていません");
		}
		if (data.Length > PacketSize)
		{
			throw new ArgumentOutOfRangeException("パケットサイズに対してデータが大きすぎます");
		}
		byte[] array = new byte[PacketSize + 1];
		Array.Clear(array, 0, array.Length);
		Array.Copy(data, 0, array, 1, data.Length);
		uint lpNumberOfBytesWritten = 0u;
		if (!Native.WriteFile(hDev, array, (uint)array.Length, ref lpNumberOfBytesWritten, IntPtr.Zero))
		{
			throw new InvalidOperationException(GetErrorMessage());
		}
	}

	public byte[] Receive()
	{
		if (!DeviceReady)
		{
			throw new InvalidOperationException("デバイスがオープンされていません");
		}
		byte[] array = new byte[PacketSize + 1];
		Array.Clear(array, 0, array.Length);
		uint lpNumberOfBytesRead = 0u;
		if (!Native.ReadFile(hDev, array, (uint)array.Length, ref lpNumberOfBytesRead, IntPtr.Zero))
		{
			throw new InvalidOperationException(GetErrorMessage());
		}
		byte[] array2 = new byte[PacketSize];
		Array.Copy(array, 1, array2, 0, array2.Length);
		return array2;
	}
}

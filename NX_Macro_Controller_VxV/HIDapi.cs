using System;
using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

public class HIDapi
{
	public struct hid_device_info
	{
		[MarshalAs(UnmanagedType.LPStr)]
		public string path;

		public ushort vendor_id;

		public ushort product_id;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string serial_number;

		public ushort release_number;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string manufacturer_string;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string product_string;

		public ushort usage_page;

		public ushort usage;

		public int interface_number;

		public IntPtr next;
	}

	private const string dll = "hidapi.dll";

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_init();

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_exit();

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr hid_enumerate(ushort vendor_id, ushort product_id);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern void hid_free_enumeration(IntPtr phid_device_info);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr hid_open(ushort vendor_id, ushort product_id, [MarshalAs(UnmanagedType.LPWStr)] string serial_number);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr hid_open_path([MarshalAs(UnmanagedType.LPStr)] string path);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_write(IntPtr device, byte[] data, UIntPtr length);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_read_timeout(IntPtr dev, byte[] data, UIntPtr length, int milliseconds);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_read(IntPtr device, byte[] data, UIntPtr length);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_set_nonblocking(IntPtr device, int nonblock);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_send_feature_report(IntPtr device, byte[] data, UIntPtr length);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_get_feature_report(IntPtr device, byte[] data, UIntPtr length);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern void hid_close(IntPtr device);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_get_manufacturer_string(IntPtr device, [MarshalAs(UnmanagedType.LPWStr)] string string_, UIntPtr maxlen);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_get_product_string(IntPtr device, [MarshalAs(UnmanagedType.LPWStr)] string string_, UIntPtr maxlen);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_get_serial_number_string(IntPtr device, [MarshalAs(UnmanagedType.LPWStr)] string string_, UIntPtr maxlen);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int hid_get_indexed_string(IntPtr device, int string_index, [MarshalAs(UnmanagedType.LPWStr)] string string_, UIntPtr maxlen);

	[DllImport("hidapi.dll", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.LPWStr)]
	public static extern string hid_error(IntPtr device);

	private static void PrintEnumeration(IntPtr phid_device_info)
	{
		if (!phid_device_info.Equals(IntPtr.Zero))
		{
			hid_device_info hid_device_info2 = (hid_device_info)Marshal.PtrToStructure(phid_device_info, typeof(hid_device_info));
			Console.WriteLine($"path:       {hid_device_info2.path}");
			Console.WriteLine($"vendor id:  {hid_device_info2.vendor_id:X}");
			Console.WriteLine($"product id: {hid_device_info2.product_id:X}");
			Console.WriteLine($"usage page: {hid_device_info2.usage_page:X}");
			Console.WriteLine($"usage:      {hid_device_info2.usage:X}");
			Console.WriteLine("");
			PrintEnumeration(hid_device_info2.next);
		}
	}

	private static string _getDevicePath(IntPtr phid_device_info, ushort usagePage, ushort usage)
	{
		if (!phid_device_info.Equals(IntPtr.Zero))
		{
			hid_device_info hid_device_info2 = (hid_device_info)Marshal.PtrToStructure(phid_device_info, typeof(hid_device_info));
			if (usagePage == hid_device_info2.usage_page && usage == hid_device_info2.usage)
			{
				return hid_device_info2.path;
			}
			return _getDevicePath(hid_device_info2.next, usagePage, usage);
		}
		return null;
	}

	public static string GetDevicePath(ushort vendorId, ushort productId, ushort usagePage, ushort usage)
	{
		return _getDevicePath(hid_enumerate(vendorId, productId), usagePage, usage);
	}
}

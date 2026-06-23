using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace NX_Macro_Controller_VxV;

[SuppressUnmanagedCodeSecurity]
[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
internal static class Native
{
	[StructLayout(LayoutKind.Sequential)]
	internal class SP_DEVICE_INTERFACE_DATA
	{
		internal int cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));

		internal Guid interfaceClassGuid = Guid.Empty;

		internal int flags;

		internal int reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class SP_DEVINFO_DATA
	{
		internal int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));

		internal Guid classGuid = Guid.Empty;

		internal int devInst;

		internal int reserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		internal int cbSize;

		internal short devicePath;
	}

	internal struct HIDD_ATTRIBUTES
	{
		public int Size;

		public ushort VendorID;

		public ushort ProductID;

		public ushort VersionNumber;
	}

	internal struct HIDP_CAPS
	{
		public ushort Usage;

		public ushort UsagePage;

		public ushort InputReportByteLength;

		public ushort OutputReportByteLength;

		public ushort FeatureReportByteLength;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
		public ushort[] Reserved;

		public ushort NumberLinkCollectionNodes;

		public ushort NumberInputButtonCaps;

		public ushort NumberInputValueCaps;

		public ushort NumberInputDataIndices;

		public ushort NumberOutputButtonCaps;

		public ushort NumberOutputValueCaps;

		public ushort NumberOutputDataIndices;

		public ushort NumberFeatureButtonCaps;

		public ushort NumberFeatureValueCaps;

		public ushort NumberFeatureDataIndices;
	}

	internal const int DIGCF_PRESENT = 2;

	internal const int DIGCF_DEVICEINTERFACE = 16;

	internal const uint GENERIC_READ = 2147483648u;

	internal const uint GENERIC_WRITE = 1073741824u;

	internal const uint FILE_SHARE_READ = 1u;

	internal const uint FILE_SHARE_WRITE = 2u;

	internal const int OPEN_EXISTING = 3;

	internal const int FILE_FLAG_OVERLAPPED = 1073741824;

	internal const uint MAX_USB_DEVICES = 16u;

	[DllImport("kernel32.dll")]
	internal static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr Arguments);

	[DllImport("hid.dll", SetLastError = true)]
	internal static extern void HidD_GetHidGuid(ref Guid lpHidGuid);

	[DllImport("hid.dll", SetLastError = true)]
	internal static extern bool HidD_GetAttributes(IntPtr hDevice, out HIDD_ATTRIBUTES Attributes);

	[DllImport("hid.dll", SetLastError = true)]
	internal static extern bool HidD_GetPreparsedData(IntPtr hDevice, out IntPtr hData);

	[DllImport("hid.dll", SetLastError = true)]
	internal static extern bool HidD_FreePreparsedData(IntPtr hData);

	[DllImport("hid.dll", SetLastError = true)]
	internal static extern bool HidP_GetCaps(IntPtr hData, out HIDP_CAPS capabilities);

	[DllImport("hid.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	internal static extern bool HidD_GetFeature(IntPtr hDevice, IntPtr hReportBuffer, uint ReportBufferLength);

	[DllImport("hid.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	internal static extern bool HidD_SetFeature(IntPtr hDevice, IntPtr ReportBuffer, uint ReportBufferLength);

	[DllImport("hid.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	internal static extern bool HidD_GetProductString(IntPtr hDevice, IntPtr Buffer, uint BufferLength);

	[DllImport("hid.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	internal static extern bool HidD_GetSerialNumberString(IntPtr hDevice, IntPtr Buffer, uint BufferLength);

	[DllImport("setupapi.dll", SetLastError = true)]
	internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, int enumerator, IntPtr hwndParent, int flags);

	[DllImport("setupapi.dll", SetLastError = true)]
	internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, SP_DEVINFO_DATA deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

	[DllImport("setupapi.dll")]
	internal static extern bool SetupDiOpenDeviceInfo(IntPtr deviceInfoSet, string deviceInstanceId, IntPtr hwndParent, int openFlags, SP_DEVINFO_DATA deviceInfoData);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);

	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static extern bool CloseHandle(IntPtr hHandle);

	[DllImport("kernel32.dll", SetLastError = true)]
	internal static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);

	[DllImport("kernel32.dll", SetLastError = true)]
	internal static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
}

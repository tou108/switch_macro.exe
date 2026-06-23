using System;
using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

public class CH375
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void mPCH375_INT_ROUTINE(byte[] iBuffer);

	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void mPCH375_NOTIFY_ROUTINE(uint iEventStatus);

	public const int mCH375_PACKET_LENGTH = 64;

	public const int mCH375_PKT_LEN_SHORT = 8;

	public const int mCH375_MAX_NUMBER = 16;

	public const int mMAX_BUFFER_LENGTH = 4096;

	public const int mDEFAULT_BUFFER_LEN = 1024;

	public const int mCH375_ENDP_INTER_UP = 129;

	public const int mCH375_ENDP_AUX_DOWN = 1;

	public const int mCH375_ENDP_DATA_UP = 130;

	public const int mCH375_ENDP_DATA_DOWN = 2;

	public const int mPipeDeviceCtrl = 4;

	public const int mPipeInterUp = 5;

	public const int mPipeDataUp = 6;

	public const int mPipeDataDown = 7;

	public const int mPipeAuxDown = 8;

	public const int mFuncNoOperation = 0;

	public const int mFuncGetVersion = 1;

	public const int mFuncGetConfig = 2;

	public const int mFuncSetExclusive = 11;

	public const int mFuncResetDevice = 12;

	public const int mFuncResetPipe = 13;

	public const int mFuncAbortPipe = 14;

	public const int mFuncSetTimeout = 15;

	public const int mFuncBufferMode = 16;

	public const int mFuncBufferModeDn = 17;

	public const int mUSB_CLR_FEATURE = 1;

	public const int mUSB_SET_FEATURE = 3;

	public const int mUSB_GET_STATUS = 0;

	public const int mUSB_SET_ADDRESS = 5;

	public const int mUSB_GET_DESCR = 6;

	public const int mUSB_SET_DESCR = 7;

	public const int mUSB_GET_CONFIG = 8;

	public const int mUSB_SET_CONFIG = 9;

	public const int mUSB_GET_INTERF = 10;

	public const int mUSB_SET_INTERF = 11;

	public const int mUSB_SYNC_FRAME = 12;

	public const int mCH375_VENDOR_READ = 192;

	public const int mCH375_VENDOR_WRITE = 64;

	public const int mCH375_SET_CONTROL = 81;

	public const int mCH375_GET_STATUS = 82;

	public const int mBitInputRxd = 2;

	public const int mBitInputReq = 4;

	public const int mStateRXD = 512;

	public const int mStateREQ = 1024;

	public const int MAX_DEVICE_PATH_SIZE = 128;

	public const int MAX_DEVICE_ID_SIZE = 64;

	public const int CH375_DEVICE_ARRIVAL = 3;

	public const int CH375_DEVICE_REMOVE_PEND = 1;

	public const int CH375_DEVICE_REMOVE = 0;

	[DllImport("CH375DLL64.DLL")]
	public static extern IntPtr CH375OpenDevice(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern void CH375CloseDevice(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern uint CH375GetVersion();

	[DllImport("CH375DLL64.DLL")]
	public static extern uint CH375DriverCommand(uint iIndex, ref mWIN32_COMMAND_mBuffer ioCommand);

	[DllImport("CH375DLL64.DLL")]
	public static extern uint CH375DriverCommand(uint iIndex, ref mWIN32_COMMAND_USB_SETUP_PKT ioCommand);

	[DllImport("CH375DLL64.DLL")]
	public static extern uint CH375GetDrvVersion();

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ResetDevice(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375GetDeviceDescr(uint iIndex, byte[] oBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375GetConfigDescr(uint iIndex, byte[] oBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetIntRoutine(uint iIndex, mPCH375_INT_ROUTINE iIntRoutine);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ReadInter(uint iIndex, byte[] oBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375AbortInter(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ReadData(uint iIndex, byte[] oBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375AbortRead(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375WriteData(uint iIndex, byte[] iBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375AbortWrite(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375WriteRead(uint iIndex, byte[] iBuffer, byte[] oBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375GetStatus(uint iIndex, ref uint iStatus);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetTimeout(uint iIndex, uint iWriteTimeout, uint iReadTimeout);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375WriteAuxData(uint iIndex, byte[] iBuffer, ref uint ioLength);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetExclusive(uint iIndex, uint iExclusive);

	[DllImport("CH375DLL64.DLL")]
	public static extern uint CH375GetUsbID(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern string CH375GetDeviceName(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetBufUpload(uint iIndex, uint iEnableOrClear);

	[DllImport("CH375DLL64.DLL")]
	public static extern int CH375QueryBufUpload(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetBufDownload(uint iIndex, uint iEnableOrClear);

	[DllImport("CH375DLL64.DLL")]
	public static extern int CH375QueryBufDownload(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ResetInter(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ResetAux(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ResetRead(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375ResetWrite(uint iIndex);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetDeviceNotify(uint iIndex, string iDeviceID, mPCH375_NOTIFY_ROUTINE iNotifyRoutine);

	[DllImport("CH375DLL64.DLL")]
	public static extern bool CH375SetTimeoutEx(uint iIndex, uint iWriteTimeout, uint iReadTimeout, uint iAuxTimeout, uint iInterTimeout);
}

using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

[StructLayout(LayoutKind.Explicit)]
public struct mWIN32_COMMAND_USB_SETUP_PKT
{
	[FieldOffset(0)]
	public uint mFunction;

	[FieldOffset(0)]
	public int mStatus;

	[FieldOffset(4)]
	public uint mLength;

	[FieldOffset(8)]
	public mUSB_SETUP_PKT mSetupPkt;
}

using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

[StructLayout(LayoutKind.Explicit)]
public struct mWIN32_COMMAND_mBuffer
{
	[FieldOffset(0)]
	public uint mFunction;

	[FieldOffset(0)]
	public int mStatus;

	[FieldOffset(4)]
	public uint mLength;

	[FieldOffset(8)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] mBuffer;
}

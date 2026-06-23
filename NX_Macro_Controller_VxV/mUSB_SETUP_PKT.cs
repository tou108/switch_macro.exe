using System.Runtime.InteropServices;

namespace NX_Macro_Controller_VxV;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct mUSB_SETUP_PKT
{
	[FieldOffset(0)]
	public byte mUspReqType;

	[FieldOffset(1)]
	public byte mUspRequest;

	[FieldOffset(2)]
	public byte mUspValueLow;

	[FieldOffset(3)]
	public byte mUspValueHigh;

	[FieldOffset(2)]
	public ushort mUspValue;

	[FieldOffset(4)]
	public byte mUspIndexLow;

	[FieldOffset(5)]
	public byte mUspIndexHigh;

	[FieldOffset(4)]
	public ushort mUspIndex;

	[FieldOffset(6)]
	public ushort mLength;
}

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace HongliangSoft.Utilities;

[StructLayout(LayoutKind.Explicit)]
internal struct MouseState
{
	[FieldOffset(0)]
	public Point Point;

	[FieldOffset(8)]
	public WheelData WheelData;

	[FieldOffset(8)]
	public XButtonData XButtonData;

	[FieldOffset(12)]
	public MouseStateFlag Flag;

	[FieldOffset(16)]
	public int Time;

	[FieldOffset(20)]
	public IntPtr ExtraInfo;
}

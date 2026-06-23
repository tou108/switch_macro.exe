using System;
using System.Drawing;

namespace HongliangSoft.Utilities;

public class MouseHookedEventArgs : EventArgs
{
	private MouseMessage message;

	private MouseState state;

	public MouseMessage Message => message;

	public Point Point => state.Point;

	public WheelData WheelData => state.WheelData;

	public XButtonData XButtonData => state.XButtonData;

	internal MouseHookedEventArgs(MouseMessage message, ref MouseState state)
	{
		this.message = message;
		this.state = state;
	}
}

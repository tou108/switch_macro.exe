namespace HongliangSoft.Utilities;

public struct XButtonData
{
	public int State;

	public int ControlledButton => State >> 16;

	public bool IsXButton1 => State >> 16 == 1;

	public bool IsXButton2 => State >> 16 == 2;
}

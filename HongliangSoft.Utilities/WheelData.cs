namespace HongliangSoft.Utilities;

public struct WheelData
{
	public int State;

	public static readonly int OneWheel = 120;

	public int WheelDelta
	{
		get
		{
			int num = State >> 16;
			if (num >= 0)
			{
				return num;
			}
			return -num;
		}
	}

	public bool IsOneWheel => State >> 16 == OneWheel;

	public WheelDirection Direction
	{
		get
		{
			int num = State >> 16;
			if (num == 0)
			{
				return WheelDirection.None;
			}
			if (num >= 0)
			{
				return WheelDirection.Forward;
			}
			return WheelDirection.Backward;
		}
	}
}

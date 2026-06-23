namespace HongliangSoft.Utilities;

internal struct MouseStateFlag
{
	public int Flag;

	public bool IsInjected
	{
		get
		{
			return (Flag & 1) != 0;
		}
		set
		{
			Flag = (value ? (Flag | 1) : (Flag & -2));
		}
	}
}

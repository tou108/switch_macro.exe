using NxInterface;

namespace NX_Macro_Controller_VxV;

public static class SerialConnecter
{
	public static void KeyDataSend(ulong keyFlag)
	{
		NxControllerInterface.KeyDataSend(keyFlag);
	}
}

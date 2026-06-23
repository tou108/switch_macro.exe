using System.Collections.Generic;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class KeyMessageFilter : IMessageFilter
{
	private const int WM_KEYDOWN = 256;

	private const int WM_KEYUP = 257;

	private Dictionary<Keys, bool> m_keyTable = new Dictionary<Keys, bool>();

	public Dictionary<Keys, bool> KeyTable
	{
		get
		{
			return m_keyTable;
		}
		private set
		{
			m_keyTable = value;
		}
	}

	public bool IsKeyPressed(Keys k)
	{
		bool value = false;
		if (KeyTable.TryGetValue(k, out value))
		{
			return value;
		}
		return false;
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (m.Msg == 256)
		{
			KeyTable[(Keys)(int)m.WParam] = true;
		}
		if (m.Msg == 257)
		{
			KeyTable[(Keys)(int)m.WParam] = false;
		}
		return false;
	}
}

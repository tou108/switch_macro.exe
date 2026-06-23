using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class ControlWriter : TextWriter
{
	private Control textbox;

	public override Encoding Encoding => Encoding.ASCII;

	public ControlWriter(Control textbox)
	{
		this.textbox = textbox;
	}

	public override void Write(char value)
	{
	}

	public override void Write(string value)
	{
	}
}

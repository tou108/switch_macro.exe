using System.ComponentModel;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class ReceiveKeyTextBox : TextBox
{
	private Keys inputKey;

	private IContainer components;

	public Keys InputKey
	{
		get
		{
			return inputKey;
		}
		set
		{
			inputKey = value;
			Text = inputKey.ToString();
		}
	}

	public ReceiveKeyTextBox()
	{
		InitializeComponent();
		base.ImeMode = ImeMode.Disable;
		ContextMenu = new ContextMenu();
		ShortcutsEnabled = false;
		base.KeyDown += OnKeyDown;
		base.KeyPress += OnKeyPress;
		InputKey = Keys.None;
	}

	private void OnKeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = true;
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		InputKey = e.KeyCode;
		Text = InputKey.ToString();
		e.Handled = true;
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		base.OnPaint(pe);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}

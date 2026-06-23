using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class ReceivePadKeyTextBox : TextBox
{
	private bool _isDirectX = true;

	private bool enter;

	private IContainer components;

	public bool IsDirectX
	{
		get
		{
			return _isDirectX;
		}
		set
		{
			_isDirectX = value;
		}
	}

	public ReceivePadKeyTextBox()
	{
		InitializeComponent();
		base.ImeMode = ImeMode.Disable;
		ContextMenu = new ContextMenu();
		ShortcutsEnabled = false;
		base.KeyDown += OnKeyDown;
		base.KeyPress += OnKeyPress;
		base.Enter += OnEnter;
		base.Leave += OnLeave;
		Text = "None";
	}

	private void OnKeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = true;
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		Text = "None";
		e.Handled = true;
	}

	private void OnEnter(object sender, EventArgs e)
	{
		enter = true;
		Task.Factory.StartNew(delegate
		{
			while (enter)
			{
				if (IsDirectX)
				{
					string gp = GamePadInput.GetKeyD();
					if (gp != "None" && gp != GamePadInput.lastKey_d)
					{
						Invoke((Action)delegate
						{
							Text = "";
							Text = gp;
						});
					}
					Thread.Sleep(16);
				}
				else
				{
					string gp2 = GamePadInput.GetKeyX();
					if (gp2 != "None" && gp2 != GamePadInput.lastKey_x)
					{
						Invoke((Action)delegate
						{
							Text = "";
							Text = gp2;
						});
					}
					Thread.Sleep(16);
				}
			}
		});
	}

	private void OnLeave(object sender, EventArgs e)
	{
		enter = false;
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

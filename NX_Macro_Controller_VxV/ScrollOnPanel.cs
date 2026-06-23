using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class ScrollOnPanel : Panel
{
	public const int WM_HSCROLL = 276;

	public const int WM_VSCROLL = 277;

	public const int SB_ENDSCROLL = 8;

	public EventHandler Scroll2;

	private IContainer components;

	public ScrollOnPanel()
	{
		InitializeComponent();
	}

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 277 && (short)(0xFFFF & (long)m.WParam) == 8)
		{
			Scroll2(this, EventArgs.Empty);
		}
		base.WndProc(ref m);
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

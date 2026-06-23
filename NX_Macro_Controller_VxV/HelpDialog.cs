using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NX_Macro_Controller_VxV.Properties;

namespace NX_Macro_Controller_VxV;

public class HelpDialog : Form
{
	public static bool Opening;

	private IContainer components;

	private WebBrowser webBrowser1;

	public HelpDialog()
	{
		InitializeComponent();
	}

	private void HelpDialog_Load(object sender, EventArgs e)
	{
		webBrowser1.DocumentText = Resources.コマンド説明;
	}

	private void HelpDialog_Shown(object sender, EventArgs e)
	{
		Opening = true;
	}

	private void HelpDialog_FormClosed(object sender, FormClosedEventArgs e)
	{
		Opening = false;
	}

	private void HelpDialog_Activated(object sender, EventArgs e)
	{
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
		this.webBrowser1 = new System.Windows.Forms.WebBrowser();
		base.SuspendLayout();
		this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.webBrowser1.Location = new System.Drawing.Point(0, 0);
		this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
		this.webBrowser1.Name = "webBrowser1";
		this.webBrowser1.Size = new System.Drawing.Size(331, 396);
		this.webBrowser1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(331, 396);
		base.Controls.Add(this.webBrowser1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		base.Name = "HelpDialog";
		this.Text = "ヘルプ";
		base.Activated += new System.EventHandler(HelpDialog_Activated);
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(HelpDialog_FormClosed);
		base.Load += new System.EventHandler(HelpDialog_Load);
		base.Shown += new System.EventHandler(HelpDialog_Shown);
		base.ResumeLayout(false);
	}
}

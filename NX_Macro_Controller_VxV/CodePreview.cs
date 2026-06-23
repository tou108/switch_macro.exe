using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class CodePreview : Form
{
	private IContainer components;

	private TextBox textBox1;

	public string Code
	{
		get
		{
			return textBox1.Text;
		}
		set
		{
			textBox1.Text = value;
			textBox1.Select(0, 0);
		}
	}

	public CodePreview()
	{
		InitializeComponent();
	}

	private void CodePreview_Load(object sender, EventArgs e)
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
		this.textBox1 = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox1.Location = new System.Drawing.Point(0, 0);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.ReadOnly = true;
		this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox1.Size = new System.Drawing.Size(336, 343);
		this.textBox1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(336, 343);
		base.Controls.Add(this.textBox1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "CodePreview";
		this.Text = "コードプレビュー";
		base.Load += new System.EventHandler(CodePreview_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BZComponent;

namespace NX_Macro_Controller_VxV;

public class DropMacro : UserControl
{
	private IContainer components;

	private BindingSource bindingSource1;

	private GroupBoxEx groupBox1;

	private Label label1;

	private Label label2;

	private GhostPanel ghostPanel1;

	public DropMacro()
	{
		InitializeComponent();
		groupBox1.Click += delegate(object sender, EventArgs args)
		{
			OnClick(args);
		};
		groupBox1.MouseUp += delegate(object sender, MouseEventArgs args)
		{
			DropIcon_MouseUp(sender, args);
		};
		groupBox1.AllowDrop = true;
	}

	private void label2_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void label1_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void groupBox1_Enter(object sender, EventArgs e)
	{
	}

	private void label2_DragDrop(object sender, DragEventArgs e)
	{
		OnDragDrop(e);
	}

	private void label1_DragDrop(object sender, DragEventArgs e)
	{
		OnDragDrop(e);
	}

	private void groupBox1_DragDrop(object sender, DragEventArgs e)
	{
		OnDragDrop(e);
	}

	private void DropIcon_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "NX Macro Controller用マクロファイル(*.nxc;*.nmc)|*.nxc;*.nmc|すべてのファイル(*.*)|*.*";
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		try
		{
			if (Util.MacroDataCheckOffline(File.ReadAllBytes(openFileDialog.FileName)) && !GlobalVar.MacroList.Contains(openFileDialog.FileName))
			{
				GlobalVar.MacroList.Add(openFileDialog.FileName);
				GlobalVar.MAINFORM.MacroShortCutReload();
			}
		}
		catch
		{
		}
	}

	private void DropIcon_MouseUp(object sender, MouseEventArgs e)
	{
		_ = e.Button;
		_ = 1048576;
	}

	private void DropMacro_Load(object sender, EventArgs e)
	{
		label2.Parent = label1;
		label2.BackColor = Color.Transparent;
		label2.Top -= label1.Top;
		label2.Left -= label1.Left;
	}

	private void label2_DragEnter(object sender, DragEventArgs e)
	{
		Util.WriteLine("DragEnter");
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			e.Effect = DragDropEffects.Move;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void label1_DragEnter(object sender, DragEventArgs e)
	{
		label2_DragEnter(sender, e);
	}

	private void groupBox1_DragEnter(object sender, DragEventArgs e)
	{
		label2_DragEnter(sender, e);
	}

	private void DropMacro_DragEnter(object sender, DragEventArgs e)
	{
		label2_DragEnter(sender, e);
	}

	private void DropMacro_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		for (int i = 0; i < array.Length; i++)
		{
			try
			{
				if (Util.MacroDataCheckOffline(File.ReadAllBytes(array[i])) && !GlobalVar.MacroList.Contains(array[i]))
				{
					GlobalVar.MacroList.Add(array[i]);
					GlobalVar.MAINFORM.MacroShortCutReload();
				}
			}
			catch
			{
			}
		}
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
		this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
		this.groupBox1 = new BZComponent.GroupBoxEx();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.ghostPanel1 = new BZComponent.GhostPanel();
		((System.ComponentModel.ISupportInitialize)this.bindingSource1).BeginInit();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.BorderColor = System.Drawing.Color.Black;
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.ghostPanel1);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(169, 120);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.DragDrop += new System.Windows.Forms.DragEventHandler(groupBox1_DragDrop);
		this.groupBox1.DragEnter += new System.Windows.Forms.DragEventHandler(groupBox1_DragEnter);
		this.groupBox1.Enter += new System.EventHandler(groupBox1_Enter);
		this.label2.AllowDrop = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("MS UI Gothic", 26.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label2.Location = new System.Drawing.Point(6, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(33, 35);
		this.label2.TabIndex = 2;
		this.label2.Text = "+";
		this.label2.Click += new System.EventHandler(label2_Click);
		this.label2.DragDrop += new System.Windows.Forms.DragEventHandler(label2_DragDrop);
		this.label2.DragEnter += new System.Windows.Forms.DragEventHandler(label2_DragEnter);
		this.label2.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		this.label1.AllowDrop = true;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label1.Location = new System.Drawing.Point(3, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(163, 89);
		this.label1.TabIndex = 1;
		this.label1.Text = "マクロを登録";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(label1_DragDrop);
		this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(label1_DragEnter);
		this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		this.ghostPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ghostPanel1.Location = new System.Drawing.Point(1, 1);
		this.ghostPanel1.Name = "ghostPanel1";
		this.ghostPanel1.Size = new System.Drawing.Size(167, 118);
		this.ghostPanel1.TabIndex = 3;
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.groupBox1);
		base.Name = "DropMacro";
		base.Size = new System.Drawing.Size(175, 126);
		base.Load += new System.EventHandler(DropMacro_Load);
		base.Click += new System.EventHandler(DropIcon_Click);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(DropMacro_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(DropMacro_DragEnter);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		((System.ComponentModel.ISupportInitialize)this.bindingSource1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BZComponent;

namespace NX_Macro_Controller_VxV;

public class DropIcon : UserControl
{
	public bool isFile;

	private IContainer components;

	private BindingSource bindingSource1;

	private GroupBoxEx groupBox1;

	private Label label1;

	private Label label2;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem 貼り付けToolStripMenuItem;

	private ToolStripMenuItem エクスプローラーで開くToolStripMenuItem;

	private GhostPanel ghostPanel1;

	public DropIcon(bool isFile)
	{
		InitializeComponent();
		this.isFile = isFile;
		groupBox1.Click += delegate(object sender, EventArgs args)
		{
			OnClick(args);
		};
		groupBox1.MouseUp += delegate(object sender, MouseEventArgs args)
		{
			DropIcon_MouseUp(sender, args);
		};
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
		openFileDialog.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
		if (isFile)
		{
			openFileDialog.Filter = "すべてのファイル(*.*) | *.*";
		}
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		try
		{
			string text = Path.GetExtension(openFileDialog.FileName).ToLower();
			if (!isFile)
			{
				switch (text)
				{
				case ".bmp":
				case ".jpg":
				case ".png":
				case ".tif":
				{
					Bitmap im = new Bitmap(openFileDialog.FileName);
					List<string> list = GlobalVar.MAINFORM.Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList();
					string text2 = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
					if (list.Contains(text2))
					{
						int num = 0;
						while (list.Contains(text2 + num))
						{
							num++;
						}
						text2 += num;
					}
					ResourcesImage item = new ResourcesImage(im, text2);
					GlobalVar.MAINFORM.Nmc.ResourcesImages.Add(item);
					GlobalVar.MAINFORM.ImageReload();
					break;
				}
				}
			}
			else
			{
				string obj = GlobalVar.MAINFORM.MacroDirectory + GlobalVar.MAINFORM.CurrentDirectory;
				if (!Directory.Exists(GlobalVar.MAINFORM.MacroDirectory))
				{
					Directory.CreateDirectory(GlobalVar.MAINFORM.MacroDirectory);
					GlobalVar.MAINFORM.FSWReload();
				}
				Directory.CreateDirectory(obj);
				File.WriteAllBytes(obj + Path.GetFileName(openFileDialog.FileName), File.ReadAllBytes(openFileDialog.FileName));
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

	private void DropIcon_Load(object sender, EventArgs e)
	{
		label2.Parent = label1;
		label2.BackColor = Color.Transparent;
		label2.Top -= label1.Top;
		label2.Left -= label1.Left;
	}

	private void DropIcon_DragDrop(object sender, DragEventArgs e)
	{
	}

	private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (isFile)
		{
			Util.PasteFiles(GlobalVar.MAINFORM.Nmc.GetDataPath(GlobalVar.MAINFORM.CurrentDirectory));
			return;
		}
		foreach (string clipboardImagePath in Util.GetClipboardImagePathList())
		{
			Bitmap im = new Bitmap(clipboardImagePath);
			List<string> list = GlobalVar.MAINFORM.Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clipboardImagePath);
			bool flag = false;
			for (int num = 0; num < list.Count; num++)
			{
				if (list[num] == fileNameWithoutExtension)
				{
					flag = true;
					ResourcesImage item = new ResourcesImage(im, fileNameWithoutExtension);
					GlobalVar.MAINFORM.Nmc.ResourcesImages.RemoveAt(num);
					GlobalVar.MAINFORM.Nmc.ResourcesImages.Insert(num, item);
				}
			}
			if (!flag)
			{
				ResourcesImage item2 = new ResourcesImage(im, fileNameWithoutExtension);
				GlobalVar.MAINFORM.Nmc.ResourcesImages.Add(item2);
			}
		}
		GlobalVar.MAINFORM.ImageReload();
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		if (isFile)
		{
			貼り付けToolStripMenuItem.Enabled = Util.CheckClipboardFiles();
			エクスプローラーで開くToolStripMenuItem.Visible = true;
		}
		else
		{
			貼り付けToolStripMenuItem.Enabled = Util.CheckClipboardImages();
			エクスプローラーで開くToolStripMenuItem.Visible = false;
		}
	}

	private void エクスプローラーで開くToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = GlobalVar.MAINFORM.MacroDirectory + GlobalVar.MAINFORM.CurrentDirectory;
		Directory.CreateDirectory(text);
		GlobalVar.MAINFORM.FSWReload();
		Process.Start("EXPLORER.EXE", text);
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
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.貼り付けToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.エクスプローラーで開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.ghostPanel1 = new BZComponent.GhostPanel();
		((System.ComponentModel.ISupportInitialize)this.bindingSource1).BeginInit();
		this.groupBox1.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.BorderColor = System.Drawing.Color.Black;
		this.groupBox1.ContextMenuStrip = this.contextMenuStrip1;
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.ghostPanel1);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(169, 120);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.DragDrop += new System.Windows.Forms.DragEventHandler(groupBox1_DragDrop);
		this.groupBox1.Enter += new System.EventHandler(groupBox1_Enter);
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.貼り付けToolStripMenuItem, this.エクスプローラーで開くToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(173, 48);
		this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip1_Opening);
		this.貼り付けToolStripMenuItem.Name = "貼り付けToolStripMenuItem";
		this.貼り付けToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.貼り付けToolStripMenuItem.Text = "貼り付け";
		this.貼り付けToolStripMenuItem.Click += new System.EventHandler(貼り付けToolStripMenuItem_Click);
		this.エクスプローラーで開くToolStripMenuItem.Name = "エクスプローラーで開くToolStripMenuItem";
		this.エクスプローラーで開くToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.エクスプローラーで開くToolStripMenuItem.Text = "エクスプローラーで開く";
		this.エクスプローラーで開くToolStripMenuItem.Click += new System.EventHandler(エクスプローラーで開くToolStripMenuItem_Click);
		this.label2.AutoSize = true;
		this.label2.ContextMenuStrip = this.contextMenuStrip1;
		this.label2.Font = new System.Drawing.Font("MS UI Gothic", 26.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label2.Location = new System.Drawing.Point(6, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(33, 35);
		this.label2.TabIndex = 2;
		this.label2.Text = "+";
		this.label2.Click += new System.EventHandler(label2_Click);
		this.label2.DragDrop += new System.Windows.Forms.DragEventHandler(label2_DragDrop);
		this.label2.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.ContextMenuStrip = this.contextMenuStrip1;
		this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
		this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label1.Location = new System.Drawing.Point(3, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(163, 89);
		this.label1.TabIndex = 1;
		this.label1.Text = "ファイルを追加";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(label1_DragDrop);
		this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		this.ghostPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ghostPanel1.Location = new System.Drawing.Point(1, 1);
		this.ghostPanel1.Name = "ghostPanel1";
		this.ghostPanel1.Size = new System.Drawing.Size(167, 118);
		this.ghostPanel1.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ContextMenuStrip = this.contextMenuStrip1;
		base.Controls.Add(this.groupBox1);
		base.Name = "DropIcon";
		base.Size = new System.Drawing.Size(175, 126);
		base.Load += new System.EventHandler(DropIcon_Load);
		base.Click += new System.EventHandler(DropIcon_Click);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(DropIcon_DragDrop);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(DropIcon_MouseUp);
		((System.ComponentModel.ISupportInitialize)this.bindingSource1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.contextMenuStrip1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

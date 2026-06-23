using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BZComponent;
using HongliangSoft.Utilities;
using NX_Macro_Controller_VxV.Properties;

namespace NX_Macro_Controller_VxV;

public class FileResItem : UserControl
{
	private MouseHook mh;

	private string baseName = "";

	private bool isFolder;

	private string folderPath = "";

	private IContainer components;

	private GroupBoxEx groupBox1;

	private Label label1;

	private TextBox textBox1;

	private MouseHook mouseHook1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem 画像の変更ToolStripMenuItem;

	private ToolStripMenuItem ラベルの変更ToolStripMenuItem;

	private ToolStripMenuItem 削除ToolStripMenuItem;

	private PictureBox pictureBox1;

	private ToolStripMenuItem 切り取りToolStripMenuItem;

	private ToolStripMenuItem コピーToolStripMenuItem;

	private ToolStripMenuItem 貼り付けToolStripMenuItem;

	private GhostPanel ghostPanel1;

	public FileResItem(string name)
	{
		InitializeComponent();
		groupBox1.Click += delegate(object sender, EventArgs args)
		{
			OnClick(args);
		};
		if (name != null)
		{
			SetData(name);
		}
	}

	public void SetFolder(string path)
	{
		pictureBox1.Visible = true;
		isFolder = true;
		folderPath = path;
		if (folderPath == "../" || folderPath == "..\\")
		{
			label1.Text = "前のディレクトリに戻る";
			return;
		}
		int num = path.LastIndexOfAny(new char[2] { '/', '\\' });
		if (num != -1)
		{
			path = path.Substring(num + 1);
		}
		label1.Text = path;
	}

	public void SetTheme(Style theme)
	{
		if (folderPath == "..\\" || folderPath == "../")
		{
			pictureBox1.Image = Resources.back;
		}
		else if (theme == Style.Dark)
		{
			if (isFolder)
			{
				pictureBox1.Image = Resources.folder;
			}
			else
			{
				pictureBox1.Image = Resources.file;
			}
		}
		else if (isFolder)
		{
			pictureBox1.Image = Resources.folder_L;
		}
		else
		{
			pictureBox1.Image = Resources.file_L;
		}
	}

	public void SetName(string name)
	{
		string text = name;
		int num = name.LastIndexOfAny(new char[2] { '/', '\\' });
		if (num != -1)
		{
			name = name.Substring(num + 1);
		}
		label1.Text = name;
		baseName = text;
	}

	public void DeleteTask()
	{
		if (isFolder)
		{
			Util.Delete(GlobalVar.MAINFORM.Nmc.GetDataPath(folderPath));
		}
		else
		{
			File.Delete(GlobalVar.MAINFORM.Nmc.GetDataPath(baseName));
		}
	}

	public void ImageChangeTask()
	{
	}

	public void NameChangeTask()
	{
	}

	public void SetData(string name)
	{
		SetName(name);
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void groupBox1_Enter(object sender, EventArgs e)
	{
	}

	private void pictureBox1_DragDrop(object sender, DragEventArgs e)
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

	private bool CheckLabelName(string name)
	{
		return true;
	}

	private void label1_DoubleClick(object sender, EventArgs e)
	{
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void textBox1_Leave(object sender, EventArgs e)
	{
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r' || e.KeyChar == '\u001b')
		{
			e.Handled = true;
			if (e.KeyChar == '\r')
			{
				base.ActiveControl = null;
			}
		}
	}

	private void textBox1_Enter(object sender, EventArgs e)
	{
	}

	private void ラベルの変更ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		NameChangeTask();
	}

	private void pictureBox1_DoubleClick(object sender, EventArgs e)
	{
		ImageChangeTask();
	}

	private void 画像の変更ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string macroDirectory = GlobalVar.MAINFORM.MacroDirectory;
		if (isFolder)
		{
			Process.Start("EXPLORER.EXE", macroDirectory + folderPath);
		}
		else
		{
			Process.Start("EXPLORER.EXE", macroDirectory + GlobalVar.MAINFORM.CurrentDirectory);
		}
	}

	private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DeleteTask();
	}

	private void ImgResItem_Paint(object sender, PaintEventArgs e)
	{
	}

	private void pictureBox1_DoubleClick_1(object sender, EventArgs e)
	{
		if (isFolder)
		{
			if (folderPath == "../" || folderPath == "..\\")
			{
				int num = GlobalVar.MAINFORM.CurrentDirectory.Substring(0, GlobalVar.MAINFORM.CurrentDirectory.Length - 1).LastIndexOfAny(new char[2] { '/', '\\' });
				if (num == -1)
				{
					GlobalVar.MAINFORM.CurrentDirectory = "";
				}
				else
				{
					GlobalVar.MAINFORM.CurrentDirectory = GlobalVar.MAINFORM.CurrentDirectory.Substring(0, num + 1);
				}
			}
			else
			{
				GlobalVar.MAINFORM.CurrentDirectory = folderPath + "\\";
			}
			GlobalVar.MAINFORM.FSWReload();
			GlobalVar.MAINFORM.DataFileReload();
		}
		else
		{
			string text = GlobalVar.MAINFORM.MacroDirectory + GlobalVar.MAINFORM.CurrentDirectory;
			Process.Start("EXPLORER.EXE", text + label1.Text);
		}
	}

	private void コピーToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = "";
		List<FileSystemInfo> list = new List<FileSystemInfo>();
		if (isFolder)
		{
			if (folderPath == "../" || folderPath == "..\\")
			{
				return;
			}
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(folderPath);
			list.Add(new DirectoryInfo(text));
		}
		else
		{
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(baseName);
			list.Add(new FileInfo(text));
		}
		list.PutFilesOnClipboard();
	}

	private void 切り取りToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = "";
		List<FileSystemInfo> list = new List<FileSystemInfo>();
		if (isFolder)
		{
			if (folderPath == "../" || folderPath == "..\\")
			{
				return;
			}
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(folderPath);
			list.Add(new DirectoryInfo(text));
		}
		else
		{
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(baseName);
			list.Add(new FileInfo(text));
		}
		list.PutFilesOnClipboard(moveFilesOnPaste: true);
	}

	private void 貼り付けToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string text = "";
		if (isFolder)
		{
			if (folderPath == "../" || folderPath == "..\\")
			{
				return;
			}
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(folderPath);
		}
		else
		{
			text = GlobalVar.MAINFORM.Nmc.GetDataPath(baseName);
		}
		text = Path.GetDirectoryName(text);
		Util.PasteFiles(text);
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		貼り付けToolStripMenuItem.Enabled = Util.CheckClipboardFiles();
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
		this.groupBox1 = new BZComponent.GroupBoxEx();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ラベルの変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.切り取りToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.コピーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.貼り付けToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.画像の変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.ghostPanel1 = new BZComponent.GhostPanel();
		this.mouseHook1 = new HongliangSoft.Utilities.MouseHook();
		this.groupBox1.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.BorderColor = System.Drawing.Color.Black;
		this.groupBox1.ContextMenuStrip = this.contextMenuStrip1;
		this.groupBox1.Controls.Add(this.pictureBox1);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.textBox1);
		this.groupBox1.Controls.Add(this.ghostPanel1);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(163, 124);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.DragDrop += new System.Windows.Forms.DragEventHandler(groupBox1_DragDrop);
		this.groupBox1.Enter += new System.EventHandler(groupBox1_Enter);
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.ラベルの変更ToolStripMenuItem, this.切り取りToolStripMenuItem, this.コピーToolStripMenuItem, this.貼り付けToolStripMenuItem, this.削除ToolStripMenuItem, this.画像の変更ToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(173, 136);
		this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip1_Opening);
		this.ラベルの変更ToolStripMenuItem.Name = "ラベルの変更ToolStripMenuItem";
		this.ラベルの変更ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.ラベルの変更ToolStripMenuItem.Text = "ラベルの変更";
		this.ラベルの変更ToolStripMenuItem.Visible = false;
		this.ラベルの変更ToolStripMenuItem.Click += new System.EventHandler(ラベルの変更ToolStripMenuItem_Click);
		this.切り取りToolStripMenuItem.Name = "切り取りToolStripMenuItem";
		this.切り取りToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.切り取りToolStripMenuItem.Text = "切り取り";
		this.切り取りToolStripMenuItem.Click += new System.EventHandler(切り取りToolStripMenuItem_Click);
		this.コピーToolStripMenuItem.Name = "コピーToolStripMenuItem";
		this.コピーToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.コピーToolStripMenuItem.Text = "コピー";
		this.コピーToolStripMenuItem.Click += new System.EventHandler(コピーToolStripMenuItem_Click);
		this.貼り付けToolStripMenuItem.Name = "貼り付けToolStripMenuItem";
		this.貼り付けToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.貼り付けToolStripMenuItem.Text = "貼り付け";
		this.貼り付けToolStripMenuItem.Click += new System.EventHandler(貼り付けToolStripMenuItem_Click);
		this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
		this.削除ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.削除ToolStripMenuItem.Text = "削除";
		this.削除ToolStripMenuItem.Click += new System.EventHandler(削除ToolStripMenuItem_Click);
		this.画像の変更ToolStripMenuItem.Name = "画像の変更ToolStripMenuItem";
		this.画像の変更ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.画像の変更ToolStripMenuItem.Text = "エクスプローラーで開く";
		this.画像の変更ToolStripMenuItem.Click += new System.EventHandler(画像の変更ToolStripMenuItem_Click);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Image = NX_Macro_Controller_VxV.Properties.Resources.folder;
		this.pictureBox1.Location = new System.Drawing.Point(6, 9);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(151, 91);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 3;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		this.pictureBox1.DoubleClick += new System.EventHandler(pictureBox1_DoubleClick_1);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.ContextMenuStrip = this.contextMenuStrip1;
		this.label1.Location = new System.Drawing.Point(6, 103);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(151, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "label1";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(label1_DragDrop);
		this.label1.DoubleClick += new System.EventHandler(pictureBox1_DoubleClick_1);
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.BackColor = System.Drawing.SystemColors.Control;
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox1.Location = new System.Drawing.Point(6, 106);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(151, 12);
		this.textBox1.TabIndex = 2;
		this.textBox1.Visible = false;
		this.textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.textBox1.Enter += new System.EventHandler(textBox1_Enter);
		this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.textBox1.Leave += new System.EventHandler(textBox1_Leave);
		this.ghostPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ghostPanel1.Location = new System.Drawing.Point(1, 1);
		this.ghostPanel1.Name = "ghostPanel1";
		this.ghostPanel1.Size = new System.Drawing.Size(161, 122);
		this.ghostPanel1.TabIndex = 4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ContextMenuStrip = this.contextMenuStrip1;
		base.Controls.Add(this.groupBox1);
		base.Name = "FileResItem";
		base.Size = new System.Drawing.Size(169, 130);
		base.Paint += new System.Windows.Forms.PaintEventHandler(ImgResItem_Paint);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.contextMenuStrip1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

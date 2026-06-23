using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BZComponent;
using HongliangSoft.Utilities;

namespace NX_Macro_Controller_VxV;

public class ImgResItem : UserControl
{
	private MouseHook mh;

	private string baseName = "";

	private IContainer components;

	private GroupBoxEx groupBox1;

	private Label label1;

	private PictureBox pictureBox1;

	private TextBox textBox1;

	private MouseHook mouseHook1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem 画像の変更ToolStripMenuItem;

	private ToolStripMenuItem ラベルの変更ToolStripMenuItem;

	private ToolStripMenuItem 削除ToolStripMenuItem;

	private Panel panel1;

	private GhostPanel ghostPanel1;

	private ToolStripMenuItem 画像を保存ToolStripMenuItem;

	private ToolStripSeparator toolStripMenuItem1;

	public ImgResItem(ResourcesImage resourcesImage = null)
	{
		InitializeComponent();
		groupBox1.Click += delegate(object sender, EventArgs args)
		{
			OnClick(args);
		};
		if (resourcesImage != null)
		{
			SetData(resourcesImage);
		}
	}

	public void SetName(string name)
	{
		label1.Text = name;
		baseName = name;
	}

	public void SetImage(Image image)
	{
		pictureBox1.Image = (Image)image.Clone();
	}

	public void DeleteTask()
	{
		string text = baseName;
		for (int i = 0; i < GlobalVar.MAINFORM.Nmc.ResourcesImages.Count; i++)
		{
			if (GlobalVar.MAINFORM.Nmc.ResourcesImages[i].label == text)
			{
				GlobalVar.MAINFORM.Nmc.ResourcesImages.RemoveAt(i);
				GlobalVar.MAINFORM.ImageReload();
				break;
			}
		}
	}

	public void ImageChangeTask()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			Bitmap image = new Bitmap(openFileDialog.FileName);
			GlobalVar.MAINFORM.Nmc.ResourcesImages.Where((ResourcesImage _) => _.label == baseName).ToList()[0].image = image;
			pictureBox1.Image = image;
		}
	}

	public void NameChangeTask()
	{
		textBox1.Text = label1.Text;
		label1.Visible = false;
		textBox1.BackColor = BZStyle.NormalColor;
		textBox1.ForeColor = BZStyle.TextFont;
		textBox1.Visible = true;
		textBox1.Focus();
		baseName = label1.Text;
		mh = new MouseHook();
		mh.MouseHooked += delegate(object o, MouseHookedEventArgs args)
		{
			if (!base.IsDisposed && args.Message != MouseMessage.Move && args.Message != MouseMessage.LUp && args.Message != MouseMessage.RUp)
			{
				int num = textBox1.PointToClient(args.Point).X;
				int num2 = textBox1.PointToClient(args.Point).Y;
				if ((num < 0 || num > textBox1.Width || num2 < 0 || num2 > textBox1.Height) && textBox1.Visible)
				{
					mh.Dispose();
					ResourcesImage resourcesImage = GlobalVar.MAINFORM.Nmc.ResourcesImages.Where((ResourcesImage _) => _.label == baseName).ToList()[0];
					char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
					resourcesImage.label = textBox1.Text;
					if (!CheckLabelName(textBox1.Text) || textBox1.Text.IndexOfAny(invalidFileNameChars) >= 0)
					{
						resourcesImage.label = baseName;
						MessageBox.Show("無効なラベル名です。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					else
					{
						label1.Text = textBox1.Text;
					}
					textBox1.Visible = false;
					label1.Visible = true;
					groupBox1.Refresh();
				}
			}
		};
		textBox1.SelectAll();
		textBox1.Refresh();
	}

	public void SetData(ResourcesImage resourcesImage)
	{
		SetName(resourcesImage.label);
		SetImage(resourcesImage.image);
	}

	private void pictureBox1_Click(object sender, EventArgs e)
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
		if (name == "")
		{
			return false;
		}
		if (GlobalVar.MAINFORM.Nmc.ResourcesImages.Where((ResourcesImage _) => _.label == name).Count() > 1)
		{
			return false;
		}
		return true;
	}

	private void label1_DoubleClick(object sender, EventArgs e)
	{
		NameChangeTask();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void textBox1_Leave(object sender, EventArgs e)
	{
		if (textBox1.Visible)
		{
			if (mh != null)
			{
				mh.Dispose();
			}
			textBox1.Visible = false;
			label1.Visible = true;
			ResourcesImage resourcesImage = GlobalVar.MAINFORM.Nmc.ResourcesImages.Where((ResourcesImage _) => _.label == baseName).ToList()[0];
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			resourcesImage.label = textBox1.Text;
			if (!CheckLabelName(textBox1.Text) || textBox1.Text.IndexOfAny(invalidFileNameChars) >= 0)
			{
				resourcesImage.label = baseName;
				MessageBox.Show("無効なラベル名です。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				label1.Text = textBox1.Text;
			}
			groupBox1.Refresh();
		}
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
		ImageChangeTask();
	}

	private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		DeleteTask();
	}

	private void ImgResItem_Paint(object sender, PaintEventArgs e)
	{
		panel1.BackColor = BZStyle.NormalColor;
		pictureBox1.BackColor = BZStyle.BackColor;
	}

	private void 画像を保存ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.Filter = "Image File(*.png)|*.png";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			ResourcesImage resourcesImage = GlobalVar.MAINFORM.Nmc.ResourcesImages.Where((ResourcesImage _) => _.label == baseName).ToList()[0];
			Path.GetExtension(saveFileDialog.FileName).ToLower();
			resourcesImage.image.Save(saveFileDialog.FileName, ImageFormat.Png);
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
		this.groupBox1 = new BZComponent.GroupBoxEx();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.ghostPanel1 = new BZComponent.GhostPanel();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.画像の変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ラベルの変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.mouseHook1 = new HongliangSoft.Utilities.MouseHook();
		this.画像を保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.contextMenuStrip1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.BorderColor = System.Drawing.Color.Black;
		this.groupBox1.Controls.Add(this.panel1);
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
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.panel1.Controls.Add(this.pictureBox1);
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(157, 100);
		this.panel1.TabIndex = 3;
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.pictureBox1.Location = new System.Drawing.Point(1, 1);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(155, 98);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(pictureBox1_DragDrop);
		this.pictureBox1.DoubleClick += new System.EventHandler(pictureBox1_DoubleClick);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.Location = new System.Drawing.Point(6, 106);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(151, 12);
		this.label1.TabIndex = 1;
		this.label1.Text = "label1";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(label1_DragDrop);
		this.label1.DoubleClick += new System.EventHandler(label1_DoubleClick);
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
		this.ghostPanel1.TabIndex = 1;
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.画像の変更ToolStripMenuItem, this.ラベルの変更ToolStripMenuItem, this.削除ToolStripMenuItem, this.toolStripMenuItem1, this.画像を保存ToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(181, 120);
		this.画像の変更ToolStripMenuItem.Name = "画像の変更ToolStripMenuItem";
		this.画像の変更ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.画像の変更ToolStripMenuItem.Text = "画像の変更";
		this.画像の変更ToolStripMenuItem.Click += new System.EventHandler(画像の変更ToolStripMenuItem_Click);
		this.ラベルの変更ToolStripMenuItem.Name = "ラベルの変更ToolStripMenuItem";
		this.ラベルの変更ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.ラベルの変更ToolStripMenuItem.Text = "ラベルの変更";
		this.ラベルの変更ToolStripMenuItem.Click += new System.EventHandler(ラベルの変更ToolStripMenuItem_Click);
		this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
		this.削除ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.削除ToolStripMenuItem.Text = "削除";
		this.削除ToolStripMenuItem.Click += new System.EventHandler(削除ToolStripMenuItem_Click);
		this.画像を保存ToolStripMenuItem.Name = "画像を保存ToolStripMenuItem";
		this.画像を保存ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.画像を保存ToolStripMenuItem.Text = "画像を保存";
		this.画像を保存ToolStripMenuItem.Click += new System.EventHandler(画像を保存ToolStripMenuItem_Click);
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ContextMenuStrip = this.contextMenuStrip1;
		base.Controls.Add(this.groupBox1);
		base.Name = "ImgResItem";
		base.Size = new System.Drawing.Size(169, 130);
		base.Paint += new System.Windows.Forms.PaintEventHandler(ImgResItem_Paint);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.contextMenuStrip1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

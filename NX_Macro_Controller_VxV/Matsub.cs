using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HongliangSoft.Utilities;

namespace NX_Macro_Controller_VxV;

public class Matsub : Form
{
	public int mode;

	public NXMC_VxV nxmc;

	public int SelectedIndex;

	public int line;

	public bool Caretflg;

	public bool Ofdflg;

	public int mouseMode;

	private string currentDirectory = "";

	private bool directoryChange;

	private IContainer components;

	public PictureBox pictureBox1;

	private Button button1;

	private Button button2;

	public ListBox listBox1;

	private MouseHook mouseHook1;

	protected override bool ShowWithoutActivation => true;

	public Matsub(int mode)
	{
		this.mode = mode;
		InitializeComponent();
	}

	public void ReplaceLabel()
	{
		if (mode > 0)
		{
			if (listBox1.Text == "..\\")
			{
				int num = currentDirectory.LastIndexOfAny(new char[2] { '/', '\\' });
				if (num == -1)
				{
					currentDirectory = "";
				}
				else
				{
					currentDirectory = currentDirectory.Substring(0, num);
				}
				Matsub_Shown(null, null);
				directoryChange = true;
				Util.WriteLine(currentDirectory);
				return;
			}
			if (listBox1.Text[listBox1.Text.Length - 1] == '\\')
			{
				if (currentDirectory == "")
				{
					currentDirectory = listBox1.Text.Substring(0, listBox1.Text.Length - 1);
				}
				else
				{
					currentDirectory = currentDirectory + "\\" + listBox1.Text.Substring(0, listBox1.Text.Length - 1);
				}
				Matsub_Shown(null, null);
				directoryChange = true;
				Util.WriteLine(currentDirectory);
				return;
			}
		}
		if (mode == 0)
		{
			string text = nxmc.CodeEdit.TextArea.Document.GetText(nxmc.CodeEdit.Document.Lines[line].Offset, nxmc.CodeEdit.Document.Lines[line].Length);
			string text2 = text.Substring(text.IndexOf("ImgCmp"), text.IndexOf("(") - text.IndexOf("ImgCmp"));
			Regex regex = new Regex(text2 + "\\(.*\\)");
			string value = regex.Match(text).Value;
			value = value.Substring(value.IndexOf("(") + 1).Trim();
			if (value[value.Length - 1] == ')')
			{
				value = value.Substring(0, value.Length - 1).Trim();
			}
			List<string> list = value.Split(',').ToList();
			if (list.Count < 2)
			{
				list[0] = ")";
			}
			else
			{
				list[0] = "";
				for (int i = 1; i < list.Count; i++)
				{
					List<string> list2 = list;
					list2[0] = list2[0] + ", " + list[i].Trim();
				}
				list[0] += ")";
			}
			string text3 = regex.Replace(text, text2 + "(" + nxmc.Nmc.ResourcesImages[listBox1.SelectedIndex].label + list[0]);
			nxmc.CodeEdit.Document.Replace(nxmc.CodeEdit.Document.Lines[line].Offset, text.Length, text3);
		}
		else if (mode == 1)
		{
			string text4 = nxmc.CodeEdit.TextArea.Document.GetText(nxmc.CodeEdit.Document.Lines[line].Offset, nxmc.CodeEdit.Document.Lines[line].Length);
			Regex regex2 = new Regex("Call\\(.*\\)");
			string value2 = regex2.Match(text4).Value;
			value2 = value2.Substring(value2.IndexOf("(") + 1).Trim();
			if (value2[value2.Length - 1] == ')')
			{
				value2 = value2.Substring(0, value2.Length - 1).Trim();
			}
			List<string> list3 = value2.Split(',').ToList();
			if (list3.Count < 2)
			{
				list3[0] = ")";
			}
			else
			{
				list3[0] = "";
				for (int j = 1; j < list3.Count; j++)
				{
					List<string> list2 = list3;
					list2[0] = list2[0] + ", " + list3[j].Trim();
				}
				list3[0] += ")";
			}
			string text5 = ((!(currentDirectory == "")) ? regex2.Replace(text4, "Call(R\"" + currentDirectory + "\\" + listBox1.Text + "\"" + list3[0]) : regex2.Replace(text4, "Call(R\"" + listBox1.Text + "\"" + list3[0]));
			nxmc.CodeEdit.Document.Replace(nxmc.CodeEdit.Document.Lines[line].Offset, text4.Length, text5);
		}
		else
		{
			if (mode != 2)
			{
				return;
			}
			string text6 = nxmc.CodeEdit.TextArea.Document.GetText(nxmc.CodeEdit.Document.Lines[line].Offset, nxmc.CodeEdit.Document.Lines[line].Length);
			Regex regex3 = new Regex("CallCsx\\(.*\\)");
			string value3 = regex3.Match(text6).Value;
			value3 = value3.Substring(value3.IndexOf("(") + 1).Trim();
			if (value3[value3.Length - 1] == ')')
			{
				value3 = value3.Substring(0, value3.Length - 1).Trim();
			}
			List<string> list4 = value3.Split(',').ToList();
			if (list4.Count < 2)
			{
				list4[0] = ")";
			}
			else
			{
				list4[0] = "";
				for (int k = 1; k < list4.Count; k++)
				{
					List<string> list2 = list4;
					list2[0] = list2[0] + ", " + list4[k].Trim();
				}
				list4[0] += ")";
			}
			string text7 = ((!(currentDirectory == "")) ? regex3.Replace(text6, "CallCsx(R\"" + currentDirectory + "\\" + listBox1.Text + "\"" + list4[0]) : regex3.Replace(text6, "CallCsx(R\"" + listBox1.Text + "\"" + list4[0]));
			nxmc.CodeEdit.Document.Replace(nxmc.CodeEdit.Document.Lines[line].Offset, text6.Length, text7);
		}
	}

	private void Matsub_Shown(object sender, EventArgs e)
	{
		SetTopMost();
		listBox1.Items.Clear();
		if (mode == 0)
		{
			foreach (ResourcesImage resourcesImage in nxmc.Nmc.ResourcesImages)
			{
				listBox1.Items.Add(resourcesImage.label);
			}
		}
		else
		{
			button1.Enabled = false;
			new List<string>();
			if (nxmc.MacroDirectory != "" && Directory.Exists(nxmc.MacroDirectory))
			{
				if (currentDirectory != "")
				{
					listBox1.Items.Add("..\\");
				}
				string[] directories = Directory.GetDirectories(nxmc.MacroDirectory + currentDirectory, "*", SearchOption.TopDirectoryOnly);
				for (int i = 0; i < directories.Length; i++)
				{
					Util.WriteLine(nxmc.MacroDirectory + currentDirectory);
					Util.WriteLine(directories[i]);
					Util.WriteLine(Util.GetRelativePath(nxmc.MacroDirectory + currentDirectory, directories[i]));
					directories[i] = Util.GetRelativePath(nxmc.MacroDirectory + currentDirectory, directories[i]).Substring(2) + "\\";
					listBox1.Items.Add(directories[i]);
				}
				directories = Directory.GetFiles(nxmc.MacroDirectory + currentDirectory, "*", SearchOption.TopDirectoryOnly);
				for (int j = 0; j < directories.Length; j++)
				{
					directories[j] = Util.GetRelativePath(nxmc.MacroDirectory + currentDirectory, directories[j]).Substring(2);
					listBox1.Items.Add(directories[j]);
				}
			}
		}
		if (listBox1.Items.Count > 0)
		{
			listBox1.SelectedIndex = SelectedIndex;
		}
		else
		{
			button2.Enabled = false;
		}
	}

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

	private void SetTopMost()
	{
		SetWindowPos(base.Handle, -1, 0, 0, 0, 0, 1107u);
	}

	private void listBox1_DoubleClick(object sender, EventArgs e)
	{
		if (listBox1.SelectedIndex >= 0)
		{
			ReplaceLabel();
			if (directoryChange)
			{
				directoryChange = false;
			}
			else
			{
				Close();
			}
		}
	}

	private void Matsub_Load(object sender, EventArgs e)
	{
	}

	private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (mode != 0 || listBox1.SelectedIndex < 0 || listBox1.Items.Count <= 0)
		{
			return;
		}
		Image image = (Image)nxmc.Nmc.ResourcesImages[listBox1.SelectedIndex].image.Clone();
		float num = Math.Min((float)pictureBox1.Width / (float)image.Width, (float)pictureBox1.Height / (float)image.Height);
		using Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
		using Graphics graphics = Graphics.FromImage(bitmap);
		int num2 = (int)((float)image.Width * num);
		int num3 = (int)((float)image.Height * num);
		SolidBrush brush = new SolidBrush(pictureBox1.BackColor);
		graphics.FillRectangle(brush, new RectangleF(0f, 0f, pictureBox1.Width, pictureBox1.Height));
		graphics.DrawImage(image, (pictureBox1.Width - num2) / 2, (pictureBox1.Height - num3) / 2, num2, num3);
		pictureBox1.Image = (Image)bitmap.Clone();
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (listBox1.SelectedIndex >= 0)
		{
			ReplaceLabel();
			if (directoryChange)
			{
				directoryChange = false;
			}
			else
			{
				Close();
			}
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		Ofdflg = true;
		mouseMode = 1;
		if (mode == 0)
		{
			openFileDialog.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Bitmap im = new Bitmap(openFileDialog.FileName);
				List<string> list = nxmc.Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList();
				string text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
				if (list.Contains(text))
				{
					int num = 0;
					while (list.Contains(text + num))
					{
						num++;
					}
					text += num;
				}
				ResourcesImage item = new ResourcesImage(im, text);
				nxmc.Nmc.ResourcesImages.Add(item);
				listBox1.Items.Clear();
				foreach (ResourcesImage resourcesImage in nxmc.Nmc.ResourcesImages)
				{
					listBox1.Items.Add(resourcesImage.label);
				}
				button2.Enabled = true;
				nxmc.ImageReload();
			}
		}
		Ofdflg = false;
	}

	private void Matsub_MouseLeave(object sender, EventArgs e)
	{
		_ = Caretflg;
	}

	private void Matsub_FormClosed(object sender, FormClosedEventArgs e)
	{
		nxmc.Closepopup();
	}

	private void mouseHook1_MouseHooked(object sender, MouseHookedEventArgs e)
	{
		if (base.IsDisposed)
		{
			return;
		}
		int num = e.Point.X;
		int num2 = e.Point.Y;
		int num3 = base.Location.X - 6;
		int num4 = base.Location.Y - 6;
		int num5 = num3 + base.Width + 6;
		int num6 = num4 + base.Height + 6;
		bool flag = false;
		if (num > num3 && num < num5 && num2 > num4 && num2 < num6)
		{
			if (!Ofdflg)
			{
				mouseMode = 0;
			}
			flag = true;
		}
		else if (mouseMode == 1)
		{
			mouseMode = 2;
		}
		if (e.Message == MouseMessage.Move)
		{
			if (!flag && !Caretflg && !Ofdflg && mouseMode == 0)
			{
				Close();
			}
		}
		else if (e.Message == MouseMessage.LDown && !flag && !Ofdflg)
		{
			Close();
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
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
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.mouseHook1 = new HongliangSoft.Utilities.MouseHook();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.pictureBox1.Location = new System.Drawing.Point(140, 3);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(184, 213);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		this.listBox1.FormattingEnabled = true;
		this.listBox1.Items.AddRange(new object[3] { "item1", "item2", "item3" });
		this.listBox1.Location = new System.Drawing.Point(3, 3);
		this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.listBox1.Name = "listBox1";
		this.listBox1.ScrollAlwaysVisible = true;
		this.listBox1.Size = new System.Drawing.Size(133, 186);
		this.listBox1.TabIndex = 0;
		this.listBox1.SelectedIndexChanged += new System.EventHandler(listBox1_SelectedIndexChanged);
		this.listBox1.DoubleClick += new System.EventHandler(listBox1_DoubleClick);
		this.button1.Location = new System.Drawing.Point(72, 193);
		this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(64, 23);
		this.button1.TabIndex = 2;
		this.button1.Text = "追加";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(3, 193);
		this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(64, 23);
		this.button2.TabIndex = 1;
		this.button2.Text = "選択";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.mouseHook1.MouseHooked += new HongliangSoft.Utilities.MouseHookedEventHandler(mouseHook1_MouseHooked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(328, 219);
		base.ControlBox = false;
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.listBox1);
		base.Controls.Add(this.pictureBox1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "Matsub";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Matsub_FormClosed);
		base.Load += new System.EventHandler(Matsub_Load);
		base.Shown += new System.EventHandler(Matsub_Shown);
		base.MouseLeave += new System.EventHandler(Matsub_MouseLeave);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

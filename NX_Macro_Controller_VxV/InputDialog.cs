using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class InputDialog : Form
{
	public static bool Opening;

	private IContainer components;

	private PictureBox pictureBox1;

	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button5;

	private Button button6;

	private Button button7;

	private Button button8;

	private Button button9;

	private Button button10;

	private Button button11;

	private Button button12;

	private Button button13;

	private Button button14;

	private Button button15;

	private Button button16;

	private Button button17;

	private Button button18;

	private Button button19;

	private Button button20;

	private Button button21;

	private Button button22;

	private Button button23;

	private Button button24;

	private Button button25;

	private Button button26;

	private Button button27;

	private CheckBox checkBox1;

	private NumericUpDown numericUpDown1;

	private Label label1;

	private GroupBox groupBox1;

	private Label label2;

	private Button button28;

	private Button button29;

	private Button button30;

	private Button button31;

	private Button button32;

	private Label label3;

	private NumericUpDown numericUpDown2;

	private Label label4;

	public InputDialog()
	{
		InitializeComponent();
	}

	private void InputDialog_Load(object sender, EventArgs e)
	{
	}

	private void InputDialog_FormClosed(object sender, FormClosedEventArgs e)
	{
		Opening = false;
	}

	private void button21_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		base.TopMost = checkBox1.Checked;
	}

	private void button5_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("A", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button8_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("B", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button7_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("Y", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button6_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("X", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button15_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("UP_R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button14_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("LEFT_R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button16_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("RIGHT_R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button13_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("DOWN_R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("HOME", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("CAPTURE", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button11_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("UP", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button10_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("LEFT", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button12_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("RIGHT", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button9_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("DOWN", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button19_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("UP_L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button20_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("RIGHT_L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button18_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("LEFT_L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button17_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("DOWN_L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("START", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("SELECT", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button22_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button23_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("ZR", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button25_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button24_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("ZL", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button26_Click(object sender, EventArgs e)
	{
		GlobalVar.MAINFORM.StartSnippingSetting();
	}

	private void button29_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("CLICK_L", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button28_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown1.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("CLICK_R", numericUpDown1.Value, numericUpDown2.Value);
		}
	}

	private void button30_Click(object sender, EventArgs e)
	{
		if (!(numericUpDown2.Value == 0m))
		{
			GlobalVar.MAINFORM.KeyInputSet("Wait(" + numericUpDown2.Value.ToString("F2") + ")", 0m, 0m, plF: true);
		}
	}

	private void button31_Click(object sender, EventArgs e)
	{
		GlobalVar.MAINFORM.KeyInputSet("Loop()\r\n{\r\n\t\r\n}", 0m, 0m, plF: true);
	}

	private void button27_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		Bitmap im = new Bitmap(openFileDialog.FileName);
		List<string> list = GlobalVar.MAINFORM.Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList();
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
		ResourcesImage resourcesImage = new ResourcesImage(im, text);
		GlobalVar.MAINFORM.Nmc.ResourcesImages.Add(resourcesImage);
		GlobalVar.MAINFORM.ImageReload();
		GlobalVar.MAINFORM.KeyInputSet("ImgCmp(" + resourcesImage.label + ")\r\n{\r\n\t\r\n}", 0m, 0m, plF: true);
	}

	private void button32_Click(object sender, EventArgs e)
	{
		GlobalVar.MAINFORM.KeyInputSet("Stop", 0m, 0m, plF: true);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NX_Macro_Controller_VxV.InputDialog));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.button7 = new System.Windows.Forms.Button();
		this.button8 = new System.Windows.Forms.Button();
		this.button9 = new System.Windows.Forms.Button();
		this.button10 = new System.Windows.Forms.Button();
		this.button11 = new System.Windows.Forms.Button();
		this.button12 = new System.Windows.Forms.Button();
		this.button13 = new System.Windows.Forms.Button();
		this.button14 = new System.Windows.Forms.Button();
		this.button15 = new System.Windows.Forms.Button();
		this.button16 = new System.Windows.Forms.Button();
		this.button17 = new System.Windows.Forms.Button();
		this.button18 = new System.Windows.Forms.Button();
		this.button19 = new System.Windows.Forms.Button();
		this.button20 = new System.Windows.Forms.Button();
		this.button21 = new System.Windows.Forms.Button();
		this.button22 = new System.Windows.Forms.Button();
		this.button23 = new System.Windows.Forms.Button();
		this.button24 = new System.Windows.Forms.Button();
		this.button25 = new System.Windows.Forms.Button();
		this.button26 = new System.Windows.Forms.Button();
		this.button27 = new System.Windows.Forms.Button();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.button28 = new System.Windows.Forms.Button();
		this.button29 = new System.Windows.Forms.Button();
		this.button30 = new System.Windows.Forms.Button();
		this.button31 = new System.Windows.Forms.Button();
		this.button32 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(0, 0);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(389, 382);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.button1.Location = new System.Drawing.Point(92, 12);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(67, 23);
		this.button1.TabIndex = 1;
		this.button1.TabStop = false;
		this.button1.Text = "SELECT";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(234, 12);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(67, 23);
		this.button2.TabIndex = 2;
		this.button2.TabStop = false;
		this.button2.Text = "START";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.button3.Location = new System.Drawing.Point(229, 294);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(72, 23);
		this.button3.TabIndex = 3;
		this.button3.TabStop = false;
		this.button3.Text = "HOME";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button4.Location = new System.Drawing.Point(79, 294);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(72, 23);
		this.button4.TabIndex = 4;
		this.button4.TabStop = false;
		this.button4.Text = "CAPTURE";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.button5.Location = new System.Drawing.Point(302, 99);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(31, 23);
		this.button5.TabIndex = 5;
		this.button5.TabStop = false;
		this.button5.Text = "A";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.button6.Location = new System.Drawing.Point(273, 69);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(31, 23);
		this.button6.TabIndex = 6;
		this.button6.TabStop = false;
		this.button6.Text = "X";
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
		this.button7.Location = new System.Drawing.Point(244, 99);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(31, 23);
		this.button7.TabIndex = 7;
		this.button7.TabStop = false;
		this.button7.Text = "Y";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Click += new System.EventHandler(button7_Click);
		this.button8.Location = new System.Drawing.Point(273, 128);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(31, 23);
		this.button8.TabIndex = 8;
		this.button8.TabStop = false;
		this.button8.Text = "B";
		this.button8.UseVisualStyleBackColor = true;
		this.button8.Click += new System.EventHandler(button8_Click);
		this.button9.Location = new System.Drawing.Point(90, 218);
		this.button9.Name = "button9";
		this.button9.Size = new System.Drawing.Size(31, 23);
		this.button9.TabIndex = 12;
		this.button9.TabStop = false;
		this.button9.Text = "↓";
		this.button9.UseVisualStyleBackColor = true;
		this.button9.Click += new System.EventHandler(button9_Click);
		this.button10.Location = new System.Drawing.Point(61, 189);
		this.button10.Name = "button10";
		this.button10.Size = new System.Drawing.Size(31, 23);
		this.button10.TabIndex = 11;
		this.button10.TabStop = false;
		this.button10.Text = "←";
		this.button10.UseVisualStyleBackColor = true;
		this.button10.Click += new System.EventHandler(button10_Click);
		this.button11.Location = new System.Drawing.Point(90, 159);
		this.button11.Name = "button11";
		this.button11.Size = new System.Drawing.Size(31, 23);
		this.button11.TabIndex = 10;
		this.button11.TabStop = false;
		this.button11.Text = "↑";
		this.button11.UseVisualStyleBackColor = true;
		this.button11.Click += new System.EventHandler(button11_Click);
		this.button12.Location = new System.Drawing.Point(119, 189);
		this.button12.Name = "button12";
		this.button12.Size = new System.Drawing.Size(31, 23);
		this.button12.TabIndex = 9;
		this.button12.TabStop = false;
		this.button12.Text = "→";
		this.button12.UseVisualStyleBackColor = true;
		this.button12.Click += new System.EventHandler(button12_Click);
		this.button13.Location = new System.Drawing.Point(275, 220);
		this.button13.Name = "button13";
		this.button13.Size = new System.Drawing.Size(31, 23);
		this.button13.TabIndex = 16;
		this.button13.TabStop = false;
		this.button13.Text = "↓";
		this.button13.UseVisualStyleBackColor = true;
		this.button13.Click += new System.EventHandler(button13_Click);
		this.button14.Location = new System.Drawing.Point(246, 191);
		this.button14.Name = "button14";
		this.button14.Size = new System.Drawing.Size(31, 23);
		this.button14.TabIndex = 15;
		this.button14.TabStop = false;
		this.button14.Text = "←";
		this.button14.UseVisualStyleBackColor = true;
		this.button14.Click += new System.EventHandler(button14_Click);
		this.button15.Location = new System.Drawing.Point(275, 161);
		this.button15.Name = "button15";
		this.button15.Size = new System.Drawing.Size(31, 23);
		this.button15.TabIndex = 14;
		this.button15.TabStop = false;
		this.button15.Text = "↑";
		this.button15.UseVisualStyleBackColor = true;
		this.button15.Click += new System.EventHandler(button15_Click);
		this.button16.Location = new System.Drawing.Point(304, 191);
		this.button16.Name = "button16";
		this.button16.Size = new System.Drawing.Size(31, 23);
		this.button16.TabIndex = 13;
		this.button16.TabStop = false;
		this.button16.Text = "→";
		this.button16.UseVisualStyleBackColor = true;
		this.button16.Click += new System.EventHandler(button16_Click);
		this.button17.Location = new System.Drawing.Point(92, 124);
		this.button17.Name = "button17";
		this.button17.Size = new System.Drawing.Size(31, 23);
		this.button17.TabIndex = 20;
		this.button17.TabStop = false;
		this.button17.Text = "↓";
		this.button17.UseVisualStyleBackColor = true;
		this.button17.Click += new System.EventHandler(button17_Click);
		this.button18.Location = new System.Drawing.Point(63, 95);
		this.button18.Name = "button18";
		this.button18.Size = new System.Drawing.Size(31, 23);
		this.button18.TabIndex = 19;
		this.button18.TabStop = false;
		this.button18.Text = "←";
		this.button18.UseVisualStyleBackColor = true;
		this.button18.Click += new System.EventHandler(button18_Click);
		this.button19.Location = new System.Drawing.Point(92, 65);
		this.button19.Name = "button19";
		this.button19.Size = new System.Drawing.Size(31, 23);
		this.button19.TabIndex = 18;
		this.button19.TabStop = false;
		this.button19.Text = "↑";
		this.button19.UseVisualStyleBackColor = true;
		this.button19.Click += new System.EventHandler(button19_Click);
		this.button20.Location = new System.Drawing.Point(121, 95);
		this.button20.Name = "button20";
		this.button20.Size = new System.Drawing.Size(31, 23);
		this.button20.TabIndex = 17;
		this.button20.TabStop = false;
		this.button20.Text = "→";
		this.button20.UseVisualStyleBackColor = true;
		this.button20.Click += new System.EventHandler(button20_Click);
		this.button21.Location = new System.Drawing.Point(475, 347);
		this.button21.Name = "button21";
		this.button21.Size = new System.Drawing.Size(75, 23);
		this.button21.TabIndex = 6;
		this.button21.Text = "閉じる";
		this.button21.UseVisualStyleBackColor = true;
		this.button21.Click += new System.EventHandler(button21_Click);
		this.button22.Location = new System.Drawing.Point(319, 12);
		this.button22.Name = "button22";
		this.button22.Size = new System.Drawing.Size(58, 23);
		this.button22.TabIndex = 22;
		this.button22.TabStop = false;
		this.button22.Text = "R";
		this.button22.UseVisualStyleBackColor = true;
		this.button22.Click += new System.EventHandler(button22_Click);
		this.button23.Location = new System.Drawing.Point(319, 41);
		this.button23.Name = "button23";
		this.button23.Size = new System.Drawing.Size(58, 23);
		this.button23.TabIndex = 23;
		this.button23.TabStop = false;
		this.button23.Text = "ZR";
		this.button23.UseVisualStyleBackColor = true;
		this.button23.Click += new System.EventHandler(button23_Click);
		this.button24.Location = new System.Drawing.Point(18, 41);
		this.button24.Name = "button24";
		this.button24.Size = new System.Drawing.Size(58, 23);
		this.button24.TabIndex = 25;
		this.button24.TabStop = false;
		this.button24.Text = "ZL";
		this.button24.UseVisualStyleBackColor = true;
		this.button24.Click += new System.EventHandler(button24_Click);
		this.button25.Location = new System.Drawing.Point(18, 12);
		this.button25.Name = "button25";
		this.button25.Size = new System.Drawing.Size(58, 23);
		this.button25.TabIndex = 24;
		this.button25.TabStop = false;
		this.button25.Text = "L";
		this.button25.UseVisualStyleBackColor = true;
		this.button25.Click += new System.EventHandler(button25_Click);
		this.button26.Location = new System.Drawing.Point(397, 171);
		this.button26.Name = "button26";
		this.button26.Size = new System.Drawing.Size(122, 23);
		this.button26.TabIndex = 1;
		this.button26.TabStop = false;
		this.button26.Text = "PC側画面キャプチャ";
		this.button26.UseVisualStyleBackColor = true;
		this.button26.Click += new System.EventHandler(button26_Click);
		this.button27.Location = new System.Drawing.Point(397, 200);
		this.button27.Name = "button27";
		this.button27.Size = new System.Drawing.Size(122, 23);
		this.button27.TabIndex = 2;
		this.button27.TabStop = false;
		this.button27.Text = "画像認識";
		this.button27.UseVisualStyleBackColor = true;
		this.button27.Click += new System.EventHandler(button27_Click);
		this.checkBox1.AutoSize = true;
		this.checkBox1.Checked = true;
		this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox1.Location = new System.Drawing.Point(8, 112);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(101, 17);
		this.checkBox1.TabIndex = 1;
		this.checkBox1.Text = "常に前面表示";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.numericUpDown1.DecimalPlaces = 2;
		this.numericUpDown1.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown1.Increment = new decimal(new int[4] { 1, 0, 0, 131072 });
		this.numericUpDown1.InterceptArrowKeys = false;
		this.numericUpDown1.Location = new System.Drawing.Point(8, 36);
		this.numericUpDown1.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Size = new System.Drawing.Size(116, 22);
		this.numericUpDown1.TabIndex = 0;
		this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.numericUpDown1.Value = new decimal(new int[4] { 1, 0, 0, 65536 });
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(130, 39);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(20, 13);
		this.label1.TabIndex = 30;
		this.label1.Text = "秒";
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.numericUpDown2);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.numericUpDown1);
		this.groupBox1.Controls.Add(this.checkBox1);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(397, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(153, 153);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "設定";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(6, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 13);
		this.label3.TabIndex = 34;
		this.label3.Text = "待機時間";
		this.numericUpDown2.DecimalPlaces = 2;
		this.numericUpDown2.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.numericUpDown2.Increment = new decimal(new int[4] { 1, 0, 0, 131072 });
		this.numericUpDown2.InterceptArrowKeys = false;
		this.numericUpDown2.Location = new System.Drawing.Point(8, 77);
		this.numericUpDown2.Maximum = new decimal(new int[4] { 10000000, 0, 0, 0 });
		this.numericUpDown2.Name = "numericUpDown2";
		this.numericUpDown2.Size = new System.Drawing.Size(116, 22);
		this.numericUpDown2.TabIndex = 32;
		this.numericUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.numericUpDown2.Value = new decimal(new int[4] { 1, 0, 0, 65536 });
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(130, 80);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(20, 13);
		this.label4.TabIndex = 33;
		this.label4.Text = "秒";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(6, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 13);
		this.label2.TabIndex = 31;
		this.label2.Text = "入力時間";
		this.button28.Location = new System.Drawing.Point(278, 191);
		this.button28.Name = "button28";
		this.button28.Size = new System.Drawing.Size(25, 23);
		this.button28.TabIndex = 32;
		this.button28.TabStop = false;
		this.button28.UseVisualStyleBackColor = true;
		this.button28.Click += new System.EventHandler(button28_Click);
		this.button29.Location = new System.Drawing.Point(95, 95);
		this.button29.Name = "button29";
		this.button29.Size = new System.Drawing.Size(25, 23);
		this.button29.TabIndex = 33;
		this.button29.TabStop = false;
		this.button29.UseVisualStyleBackColor = true;
		this.button29.Click += new System.EventHandler(button29_Click);
		this.button30.Location = new System.Drawing.Point(397, 229);
		this.button30.Name = "button30";
		this.button30.Size = new System.Drawing.Size(122, 23);
		this.button30.TabIndex = 3;
		this.button30.TabStop = false;
		this.button30.Text = "ウェイト";
		this.button30.UseVisualStyleBackColor = true;
		this.button30.Click += new System.EventHandler(button30_Click);
		this.button31.Location = new System.Drawing.Point(397, 258);
		this.button31.Name = "button31";
		this.button31.Size = new System.Drawing.Size(122, 23);
		this.button31.TabIndex = 4;
		this.button31.TabStop = false;
		this.button31.Text = "ループ";
		this.button31.UseVisualStyleBackColor = true;
		this.button31.Click += new System.EventHandler(button31_Click);
		this.button32.Location = new System.Drawing.Point(397, 287);
		this.button32.Name = "button32";
		this.button32.Size = new System.Drawing.Size(122, 23);
		this.button32.TabIndex = 5;
		this.button32.TabStop = false;
		this.button32.Text = "マクロ停止";
		this.button32.UseVisualStyleBackColor = true;
		this.button32.Click += new System.EventHandler(button32_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(558, 382);
		base.Controls.Add(this.button32);
		base.Controls.Add(this.button31);
		base.Controls.Add(this.button30);
		base.Controls.Add(this.button29);
		base.Controls.Add(this.button28);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.button27);
		base.Controls.Add(this.button26);
		base.Controls.Add(this.button24);
		base.Controls.Add(this.button25);
		base.Controls.Add(this.button23);
		base.Controls.Add(this.button22);
		base.Controls.Add(this.button21);
		base.Controls.Add(this.button17);
		base.Controls.Add(this.button18);
		base.Controls.Add(this.button19);
		base.Controls.Add(this.button20);
		base.Controls.Add(this.button13);
		base.Controls.Add(this.button14);
		base.Controls.Add(this.button15);
		base.Controls.Add(this.button16);
		base.Controls.Add(this.button9);
		base.Controls.Add(this.button10);
		base.Controls.Add(this.button11);
		base.Controls.Add(this.button12);
		base.Controls.Add(this.button8);
		base.Controls.Add(this.button7);
		base.Controls.Add(this.button6);
		base.Controls.Add(this.button5);
		base.Controls.Add(this.button4);
		base.Controls.Add(this.button3);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.pictureBox1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Name = "InputDialog";
		this.Text = "InputDialog";
		base.TopMost = true;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(InputDialog_FormClosed);
		base.Load += new System.EventHandler(InputDialog_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown2).EndInit();
		base.ResumeLayout(false);
	}
}

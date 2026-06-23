using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class ShareContent : UserControl
{
	private bool selected;

	private bool viewImage = true;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private PictureBox pictureBox1;

	private TableLayoutPanel tableLayoutPanel2;

	private TableLayoutPanel tableLayoutPanel3;

	private Label label2;

	private Label label1;

	private Label label3;

	private Panel panel1;

	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			selected = value;
			if (selected)
			{
				BackColor = Color.FromArgb(0, 122, 204);
				ForeColor = Color.White;
				Focus();
			}
			else
			{
				BackColor = SystemColors.ControlLight;
				ForeColor = default(Color);
			}
		}
	}

	public bool ShowImage
	{
		get
		{
			return viewImage;
		}
		set
		{
			viewImage = value;
			if (!viewImage)
			{
				tableLayoutPanel1.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
			}
			else
			{
				tableLayoutPanel1.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 70f);
			}
		}
	}

	public Image Image
	{
		get
		{
			return pictureBox1.Image;
		}
		set
		{
			pictureBox1.Image = value;
		}
	}

	public string Direction
	{
		get
		{
			return label1.Text;
		}
		set
		{
			label1.Text = value;
		}
	}

	public string Caption
	{
		get
		{
			return label3.Text;
		}
		set
		{
			label3.Text = value;
		}
	}

	public string Author
	{
		get
		{
			return label2.Text;
		}
		set
		{
			label2.Text = value;
		}
	}

	public ShareContent()
	{
		InitializeComponent();
	}

	private void label2_Click(object sender, EventArgs e)
	{
		Selected = true;
	}

	private void label2_TextChanged(object sender, EventArgs e)
	{
	}

	private void ShareContent_ParentChanged(object sender, EventArgs e)
	{
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void label3_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void label1_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void tableLayoutPanel1_Click(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void label3_MouseDown(object sender, MouseEventArgs e)
	{
		OnMouseDown(e);
	}

	private void label2_MouseDown(object sender, MouseEventArgs e)
	{
		OnMouseDown(e);
	}

	private void label1_MouseDown(object sender, MouseEventArgs e)
	{
		OnMouseDown(e);
	}

	private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
	{
		OnMouseDown(e);
	}

	private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
	{
		OnMouseDown(e);
	}

	private void tableLayoutPanel1_DoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
	}

	private void pictureBox1_DoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
	}

	private void label3_DoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
	}

	private void label2_DoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
	}

	private void label1_DoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
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
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tableLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.tableLayoutPanel2.SuspendLayout();
		this.tableLayoutPanel3.SuspendLayout();
		base.SuspendLayout();
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(166, 70);
		this.tableLayoutPanel1.TabIndex = 0;
		this.tableLayoutPanel1.Click += new System.EventHandler(tableLayoutPanel1_Click);
		this.tableLayoutPanel1.DoubleClick += new System.EventHandler(tableLayoutPanel1_DoubleClick);
		this.tableLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(tableLayoutPanel1_MouseDown);
		this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBox1.Location = new System.Drawing.Point(3, 3);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(64, 64);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
		this.pictureBox1.DoubleClick += new System.EventHandler(pictureBox1_DoubleClick);
		this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
		this.tableLayoutPanel2.ColumnCount = 1;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(70, 0);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 2;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(96, 70);
		this.tableLayoutPanel2.TabIndex = 1;
		this.tableLayoutPanel3.ColumnCount = 2;
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel3.Controls.Add(this.label2, 1, 0);
		this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
		this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel3.Name = "tableLayoutPanel3";
		this.tableLayoutPanel3.RowCount = 1;
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel3.Size = new System.Drawing.Size(96, 26);
		this.tableLayoutPanel3.TabIndex = 0;
		this.label2.AutoEllipsis = true;
		this.label2.AutoSize = true;
		this.label2.Dock = System.Windows.Forms.DockStyle.Left;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(55, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(38, 26);
		this.label2.TabIndex = 0;
		this.label2.Text = "label2";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.TextChanged += new System.EventHandler(label2_TextChanged);
		this.label2.Click += new System.EventHandler(label2_Click);
		this.label2.DoubleClick += new System.EventHandler(label2_DoubleClick);
		this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(label2_MouseDown);
		this.label3.AutoEllipsis = true;
		this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 26);
		this.label3.TabIndex = 1;
		this.label3.Text = "label3";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.Click += new System.EventHandler(label3_Click);
		this.label3.DoubleClick += new System.EventHandler(label3_DoubleClick);
		this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(label3_MouseDown);
		this.label1.AutoEllipsis = true;
		this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(90, 44);
		this.label1.TabIndex = 1;
		this.label1.Text = "説明文";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.label1.DoubleClick += new System.EventHandler(label1_DoubleClick);
		this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(label1_MouseDown);
		this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel1.Location = new System.Drawing.Point(0, 70);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(166, 1);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.ControlLight;
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tableLayoutPanel1);
		this.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "ShareContent";
		base.Size = new System.Drawing.Size(166, 71);
		base.ParentChanged += new System.EventHandler(ShareContent_ParentChanged);
		this.tableLayoutPanel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel3.ResumeLayout(false);
		this.tableLayoutPanel3.PerformLayout();
		base.ResumeLayout(false);
	}
}

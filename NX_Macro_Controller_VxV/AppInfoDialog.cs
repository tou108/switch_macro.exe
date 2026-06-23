using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class AppInfoDialog : Form
{
	private IContainer components;

	private Button button1;

	private Label label1;

	private Label label2;

	private PictureBox pictureBox1;

	private Label label3;

	private Label label4;

	private LinkLabel linkLabel1;

	private LinkLabel linkLabel2;

	private LinkLabel linkLabel3;

	public AppInfoDialog()
	{
		InitializeComponent();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		linkLabel1.LinkVisited = true;
		Process.Start(linkLabel1.Text);
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		linkLabel3.LinkVisited = true;
		Process.Start("mailto:" + linkLabel3.Text);
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		linkLabel2.LinkVisited = true;
		Process.Start(linkLabel2.Text);
	}

	private void AppInfoDialog_Load(object sender, EventArgs e)
	{
		button1.Left = label1.Width / 2 - button1.Width / 2;
		label3.Text = "Version : " + GlobalVar.VerName;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NX_Macro_Controller_VxV.AppInfoDialog));
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.linkLabel3 = new System.Windows.Forms.LinkLabel();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.button1.Location = new System.Drawing.Point(30, 202);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 0;
		this.button1.Text = "OK";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(276, 90);
		this.label1.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.SystemColors.ControlDark;
		this.label2.Font = new System.Drawing.Font("Impact", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(82, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(184, 26);
		this.label2.TabIndex = 2;
		this.label2.Text = "NX Macro Controller";
		this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
		this.pictureBox1.BackgroundImage = (System.Drawing.Image)resources.GetObject("pictureBox1.BackgroundImage");
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Location = new System.Drawing.Point(12, 12);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(64, 64);
		this.pictureBox1.TabIndex = 3;
		this.pictureBox1.TabStop = false;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.SystemColors.ControlDark;
		this.label3.Font = new System.Drawing.Font("Impact", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(83, 48);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(143, 19);
		this.label3.TabIndex = 4;
		this.label3.Text = "Version : 2.00 (BETA 2)";
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Location = new System.Drawing.Point(0, 90);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(276, 23);
		this.label4.TabIndex = 5;
		this.label4.Text = "Copyright (C) 2018-2023 ぼんじり/BZL";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.linkLabel1.Location = new System.Drawing.Point(0, 113);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(276, 23);
		this.linkLabel1.TabIndex = 6;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "http://bzl.hatenablog.com";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.linkLabel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.linkLabel2.Location = new System.Drawing.Point(0, 136);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(276, 23);
		this.linkLabel2.TabIndex = 7;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "https://twitter.com/_3z8";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.linkLabel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.linkLabel3.Location = new System.Drawing.Point(0, 159);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(276, 23);
		this.linkLabel3.TabIndex = 8;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "yu_gen_00@yahoo.co.jp";
		this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(276, 237);
		base.Controls.Add(this.linkLabel3);
		base.Controls.Add(this.linkLabel2);
		base.Controls.Add(this.linkLabel1);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "AppInfoDialog";
		this.Text = "バージョン情報";
		base.Load += new System.EventHandler(AppInfoDialog_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

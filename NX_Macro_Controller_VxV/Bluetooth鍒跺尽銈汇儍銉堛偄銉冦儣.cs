using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NX_Macro_Controller_VxV;

public class Bluetooth制御セットアップ : Form
{
	private class USBDeviceInfo
	{
		public string DeviceID { get; private set; }

		public string PnpDeviceID { get; private set; }

		public string Description { get; private set; }

		public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
		{
			DeviceID = deviceID;
			PnpDeviceID = pnpDeviceID;
			Description = description;
		}
	}

	private enum Win32ShutdownFlags
	{
		Logoff = 0,
		Shutdown = 1,
		Reboot = 2,
		PowerOff = 8,
		Forced = 4
	}

	private List<USBDeviceInfo> usbDevices = new List<USBDeviceInfo>();

	private IContainer components;

	private ListBox listBox1;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Button button1;

	public Bluetooth制御セットアップ()
	{
		InitializeComponent();
	}

	private void Bluetooth制御セットアップ_Load(object sender, EventArgs e)
	{
		usbDevices = GetUSBDevices();
		foreach (USBDeviceInfo usbDevice in usbDevices)
		{
			listBox1.Items.Add(usbDevice.Description);
		}
	}

	private static List<USBDeviceInfo> GetUSBDevices()
	{
		List<USBDeviceInfo> list = new List<USBDeviceInfo>();
		ManagementObjectCollection managementObjectCollection;
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_PnPEntity"))
		{
			managementObjectCollection = managementObjectSearcher.Get();
		}
		foreach (ManagementBaseObject item in managementObjectCollection)
		{
			string text = (string)item.GetPropertyValue("Description");
			if (!string.IsNullOrEmpty(text) && (Regex.IsMatch(text, ".*Bluetooth.*Adapter.*") || Regex.IsMatch(text, ".*Bluetooth.*Radio.*") || Regex.IsMatch(text, ".*Bluetooth.*ラジオ.*") || Regex.IsMatch(text, ".*Bluetooth.*アダプタ.*") || Regex.IsMatch(text, ".*CSR.*Bluetooth.*") || Regex.IsMatch(text, ".*TOSHIBA.*Bluetooth.*") || Regex.IsMatch(text, ".*CSR.*Bluetooth.*") || Regex.IsMatch(text, ".*Intel.*Wireless.*Bluetooth.*") || Regex.IsMatch(text, ".*インテル.*ワイヤレス.*Bluetooth.*")) && item.GetPropertyValue("Name") != null)
			{
				list.Add(new USBDeviceInfo((string)item.GetPropertyValue("DeviceID"), (string)item.GetPropertyValue("ClassGuid"), (string)item.GetPropertyValue("Name")));
			}
		}
		managementObjectCollection.Dispose();
		return list;
	}

	private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (listBox1.SelectedIndex < 0)
		{
			button1.Enabled = false;
			return;
		}
		button1.Enabled = true;
		label4.Text = usbDevices[listBox1.SelectedIndex].Description;
		label5.Text = usbDevices[listBox1.SelectedIndex].PnpDeviceID;
		string deviceID = usbDevices[listBox1.SelectedIndex].DeviceID;
		string text = deviceID.Substring(deviceID.IndexOf("VID_") + 4, 4);
		string text2 = deviceID.Substring(deviceID.IndexOf("PID_") + 4, 4);
		label6.Text = text + "/" + text2;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("注意！\r\n制御用のドライバをインストールすることにより、本来の用途での使用が行えなくなる可能性があります。\r\n本当にインストールを続行してもよろしいですか？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
		{
			return;
		}
		try
		{
			string deviceID = usbDevices[listBox1.SelectedIndex].DeviceID;
			int VID = int.Parse(deviceID.Substring(deviceID.IndexOf("VID_") + 4, 4), NumberStyles.HexNumber);
			int PID = int.Parse(deviceID.Substring(deviceID.IndexOf("PID_") + 4, 4), NumberStyles.HexNumber);
			Task.Factory.StartNew(delegate
			{
				Process process = new Process();
				process.StartInfo.FileName = GlobalVar.BasePath + "DriverReplace.exe";
				process.StartInfo.Arguments = $"{VID} {PID}";
				process.StartInfo.Verb = "RunAs";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.UseShellExecute = false;
				process.Start();
				process.WaitForExit();
				process.Close();
			});
			if (MessageBox.Show("この設定の変更はコンピュータの再起動後に反映されます。\r\nコンピュータを今すぐに再起動しますか？", "コンピュータの再起動", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				RunWin32Shutdown(Win32ShutdownFlags.Reboot);
			}
		}
		catch
		{
			MessageBox.Show("ドライバの置換に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("この設定の変更はコンピュータの再起動後に反映されます。\r\nコンピュータを今すぐに再起動しますか？", "コンピュータの再起動", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			RunWin32Shutdown(Win32ShutdownFlags.Reboot);
		}
	}

	private static void Win32Shutdown(int shutdownFlags)
	{
		Thread thread = new Thread((ThreadStart)delegate
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			managementClass.Get();
			managementClass.Scope.Options.EnablePrivileges = true;
			foreach (ManagementObject instance in managementClass.GetInstances())
			{
				instance.InvokeMethod("Win32Shutdown", new object[2] { shutdownFlags, 0 });
				instance.Dispose();
			}
		});
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		thread.Join();
	}

	private void RunWin32Shutdown(Win32ShutdownFlags shutdownFlags)
	{
		Win32Shutdown((int)shutdownFlags);
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
		this.listBox1 = new System.Windows.Forms.ListBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.listBox1.FormattingEnabled = true;
		this.listBox1.ItemHeight = 12;
		this.listBox1.Location = new System.Drawing.Point(12, 84);
		this.listBox1.Name = "listBox1";
		this.listBox1.Size = new System.Drawing.Size(324, 232);
		this.listBox1.TabIndex = 0;
		this.listBox1.SelectedIndexChanged += new System.EventHandler(listBox1_SelectedIndexChanged);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 12);
		this.label1.TabIndex = 1;
		this.label1.Text = "[デバイス名]";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(12, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(40, 12);
		this.label2.TabIndex = 2;
		this.label2.Text = "[GUID]";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(12, 60);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 12);
		this.label3.TabIndex = 3;
		this.label3.Text = "[VID/HID]";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(81, 15);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(81, 12);
		this.label4.TabIndex = 4;
		this.label4.Text = "                   ";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(81, 38);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(81, 12);
		this.label5.TabIndex = 5;
		this.label5.Text = "                   ";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(81, 60);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(81, 12);
		this.label6.TabIndex = 6;
		this.label6.Text = "                   ";
		this.button1.Enabled = false;
		this.button1.Location = new System.Drawing.Point(218, 322);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(118, 23);
		this.button1.TabIndex = 7;
		this.button1.Text = "インストール";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(348, 357);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.listBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Name = "Bluetooth制御セットアップ";
		this.Text = "Bluetooth制御セットアップ";
		base.Load += new System.EventHandler(Bluetooth制御セットアップ_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

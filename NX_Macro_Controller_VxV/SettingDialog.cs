using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BZComponent;
using NxInterface;

namespace NX_Macro_Controller_VxV;

public class SettingDialog : Form
{
	public bool SettingChanged;

	private IContainer components;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private Label label11;

	private ReceiveKeyTextBox ButtonHOME;

	private Label label10;

	private Label label9;

	private ReceiveKeyTextBox ButtonSELECT;

	private ReceiveKeyTextBox ButtonSTART;

	private Label label8;

	private ReceiveKeyTextBox ButtonZR;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private ReceiveKeyTextBox ButtonZL;

	private ReceiveKeyTextBox ButtonR;

	private ReceiveKeyTextBox ButtonL;

	private ReceiveKeyTextBox ButtonY;

	private ReceiveKeyTextBox ButtonX;

	private ReceiveKeyTextBox ButtonB;

	private ReceiveKeyTextBox ButtonA;

	private Label label2;

	private Label label1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Label label15;

	private ReceiveKeyTextBox DRIGHT;

	private Label label14;

	private ReceiveKeyTextBox DLEFT;

	private Label label13;

	private ReceiveKeyTextBox DDOWN;

	private Label label12;

	private ReceiveKeyTextBox DUP;

	private GroupBox groupBox4;

	private Label label20;

	private ReceiveKeyTextBox ANALOGRRIGHT;

	private Label label21;

	private ReceiveKeyTextBox ANALOGRLEFT;

	private Label label22;

	private ReceiveKeyTextBox ANALOGRDOWN;

	private Label label23;

	private ReceiveKeyTextBox ANALOGRUP;

	private GroupBox groupBox3;

	private Label label16;

	private ReceiveKeyTextBox ANALOGLRIGHT;

	private Label label17;

	private ReceiveKeyTextBox ANALOGLLEFT;

	private Label label18;

	private ReceiveKeyTextBox ANALOGLDOWN;

	private Label label19;

	private ReceiveKeyTextBox ANALOGLUP;

	private CheckBox checkBox1;

	private GroupBox groupBox5;

	private CheckBox checkBox4;

	private CheckBox checkBox3;

	private CheckBox checkBox2;

	private Label label24;

	private ReceiveKeyTextBox ButtonClickR;

	private Label label25;

	private ReceiveKeyTextBox ButtonClickL;

	private Label label26;

	private ReceiveKeyTextBox ButtonCAPTURE;

	private Button button1;

	private Button button2;

	private GroupBox groupBox6;

	private CheckBox checkBox5;

	private ComboBox comboBox1;

	private Label label27;

	private GroupBox groupBox7;

	private CheckBox checkBox6;

	private Button button4;

	private Button button3;

	private TextBox textBox1;

	private Label label28;

	private TextBox textBox2;

	private Label label29;

	private TabPage tabPage3;

	private GroupBox groupBox8;

	private Label label30;

	private ReceivePadKeyTextBox Dx26;

	private Label label31;

	private ReceivePadKeyTextBox Dx25;

	private Label label32;

	private ReceivePadKeyTextBox Dx24;

	private Label label33;

	private ReceivePadKeyTextBox Dx23;

	private GroupBox groupBox9;

	private Label label34;

	private ReceivePadKeyTextBox Dx22;

	private Label label35;

	private ReceivePadKeyTextBox Dx21;

	private Label label36;

	private ReceivePadKeyTextBox Dx20;

	private Label label37;

	private ReceivePadKeyTextBox Dx19;

	private GroupBox groupBox10;

	private Label label38;

	private ReceivePadKeyTextBox Dx14;

	private Label label39;

	private ReceivePadKeyTextBox Dx10;

	private Label label40;

	private ReceivePadKeyTextBox Dx9;

	private Label label41;

	private Label label42;

	private Label label43;

	private ReceivePadKeyTextBox Dx8;

	private ReceivePadKeyTextBox Dx11;

	private Label label44;

	private Label label45;

	private ReceivePadKeyTextBox Dx13;

	private Label label46;

	private ReceivePadKeyTextBox Dx12;

	private Label label47;

	private Label label48;

	private Label label49;

	private ReceivePadKeyTextBox Dx7;

	private ReceivePadKeyTextBox Dx6;

	private ReceivePadKeyTextBox Dx5;

	private ReceivePadKeyTextBox Dx4;

	private ReceivePadKeyTextBox Dx3;

	private ReceivePadKeyTextBox Dx2;

	private ReceivePadKeyTextBox Dx1;

	private Label label50;

	private Label label51;

	private GroupBox groupBox11;

	private Label label52;

	private ReceivePadKeyTextBox Dx18;

	private Label label53;

	private ReceivePadKeyTextBox Dx17;

	private Label label54;

	private ReceivePadKeyTextBox Dx16;

	private Label label55;

	private ReceivePadKeyTextBox Dx15;

	private TabPage tabPage4;

	private GroupBox groupBox12;

	private Label label56;

	private ReceivePadKeyTextBox XI26;

	private Label label57;

	private ReceivePadKeyTextBox XI25;

	private Label label58;

	private ReceivePadKeyTextBox XI24;

	private Label label59;

	private ReceivePadKeyTextBox XI23;

	private GroupBox groupBox13;

	private Label label60;

	private ReceivePadKeyTextBox XI22;

	private Label label61;

	private ReceivePadKeyTextBox XI21;

	private Label label62;

	private ReceivePadKeyTextBox XI20;

	private Label label63;

	private ReceivePadKeyTextBox XI19;

	private GroupBox groupBox14;

	private Label label64;

	private ReceivePadKeyTextBox XI14;

	private Label label65;

	private ReceivePadKeyTextBox XI10;

	private Label label66;

	private ReceivePadKeyTextBox XI9;

	private Label label67;

	private Label label68;

	private Label label69;

	private ReceivePadKeyTextBox XI8;

	private ReceivePadKeyTextBox XI11;

	private Label label70;

	private Label label71;

	private ReceivePadKeyTextBox XI13;

	private Label label72;

	private ReceivePadKeyTextBox XI12;

	private Label label73;

	private Label label74;

	private Label label75;

	private ReceivePadKeyTextBox XI7;

	private ReceivePadKeyTextBox XI6;

	private ReceivePadKeyTextBox XI5;

	private ReceivePadKeyTextBox XI4;

	private ReceivePadKeyTextBox XI3;

	private ReceivePadKeyTextBox XI2;

	private ReceivePadKeyTextBox XI1;

	private Label label76;

	private Label label77;

	private GroupBox groupBox15;

	private Label label78;

	private ReceivePadKeyTextBox XI18;

	private Label label79;

	private ReceivePadKeyTextBox XI17;

	private Label label80;

	private ReceivePadKeyTextBox XI16;

	private Label label81;

	private ReceivePadKeyTextBox XI15;

	private Label label82;

	private ComboBox comboBox2;

	private CheckBox checkBox7;

	private CheckBox checkBox8;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams obj = base.CreateParams;
			obj.ExStyle |= 33554432;
			return obj;
		}
	}

	public SettingDialog()
	{
		InitializeComponent();
	}

	private void ButtonA_TextChanged(object sender, EventArgs e)
	{
		ButtonB.Focus();
	}

	private void ButtonB_TextChanged(object sender, EventArgs e)
	{
		ButtonX.Focus();
	}

	private void ButtonX_TextChanged(object sender, EventArgs e)
	{
		ButtonY.Focus();
	}

	private void ButtonY_TextChanged(object sender, EventArgs e)
	{
		ButtonL.Focus();
	}

	private void ButtonL_TextChanged(object sender, EventArgs e)
	{
		ButtonR.Focus();
	}

	private void ButtonR_TextChanged(object sender, EventArgs e)
	{
		ButtonZL.Focus();
	}

	private void ButtonZL_TextChanged(object sender, EventArgs e)
	{
		ButtonZR.Focus();
	}

	private void ButtonZR_TextChanged(object sender, EventArgs e)
	{
		ButtonClickL.Focus();
	}

	private void ButtonSTART_TextChanged(object sender, EventArgs e)
	{
		ButtonSELECT.Focus();
	}

	private void ButtonSELECT_TextChanged(object sender, EventArgs e)
	{
		ButtonHOME.Focus();
	}

	private void ButtonHOME_TextChanged(object sender, EventArgs e)
	{
		ButtonCAPTURE.Focus();
	}

	private void DUP_TextChanged(object sender, EventArgs e)
	{
		DDOWN.Focus();
	}

	private void DDOWN_TextChanged(object sender, EventArgs e)
	{
		DLEFT.Focus();
	}

	private void DLEFT_TextChanged(object sender, EventArgs e)
	{
		DRIGHT.Focus();
	}

	private void DRIGHT_TextChanged(object sender, EventArgs e)
	{
		ANALOGLUP.Focus();
	}

	private void ANALOGLUP_TextChanged(object sender, EventArgs e)
	{
		ANALOGLDOWN.Focus();
	}

	private void ANALOGLDOWN_TextChanged(object sender, EventArgs e)
	{
		ANALOGLLEFT.Focus();
	}

	private void ANALOGLLEFT_TextChanged(object sender, EventArgs e)
	{
		ANALOGLRIGHT.Focus();
	}

	private void ANALOGLRIGHT_TextChanged(object sender, EventArgs e)
	{
		ANALOGRUP.Focus();
	}

	private void ANALOGRUP_TextChanged(object sender, EventArgs e)
	{
		ANALOGRDOWN.Focus();
	}

	private void ANALOGRDOWN_TextChanged(object sender, EventArgs e)
	{
		ANALOGRLEFT.Focus();
	}

	private void ANALOGRLEFT_TextChanged(object sender, EventArgs e)
	{
		ANALOGRRIGHT.Focus();
	}

	private void ANALOGRRIGHT_TextChanged(object sender, EventArgs e)
	{
	}

	private void SettingDialog_Load(object sender, EventArgs e)
	{
		ButtonA.InputKey = KEYCONFIG.Button.A;
		ButtonB.InputKey = KEYCONFIG.Button.B;
		ButtonX.InputKey = KEYCONFIG.Button.X;
		ButtonY.InputKey = KEYCONFIG.Button.Y;
		ButtonL.InputKey = KEYCONFIG.Button.L;
		ButtonR.InputKey = KEYCONFIG.Button.R;
		ButtonZL.InputKey = KEYCONFIG.Button.ZL;
		ButtonZR.InputKey = KEYCONFIG.Button.ZR;
		ButtonSTART.InputKey = KEYCONFIG.Button.START;
		ButtonSELECT.InputKey = KEYCONFIG.Button.SELECT;
		ButtonHOME.InputKey = KEYCONFIG.Button.HOME;
		DUP.InputKey = KEYCONFIG.DPad.UP;
		DDOWN.InputKey = KEYCONFIG.DPad.DOWN;
		DLEFT.InputKey = KEYCONFIG.DPad.LEFT;
		DRIGHT.InputKey = KEYCONFIG.DPad.RIGHT;
		ANALOGLUP.InputKey = KEYCONFIG.AnalogL.UP;
		ANALOGLDOWN.InputKey = KEYCONFIG.AnalogL.DOWN;
		ANALOGLLEFT.InputKey = KEYCONFIG.AnalogL.LEFT;
		ANALOGLRIGHT.InputKey = KEYCONFIG.AnalogL.RIGHT;
		ANALOGRUP.InputKey = KEYCONFIG.AnalogR.UP;
		ANALOGRDOWN.InputKey = KEYCONFIG.AnalogR.DOWN;
		ANALOGRLEFT.InputKey = KEYCONFIG.AnalogR.LEFT;
		ANALOGRRIGHT.InputKey = KEYCONFIG.AnalogR.RIGHT;
		ButtonClickL.InputKey = KEYCONFIG.Button.CLICKL;
		ButtonClickR.InputKey = KEYCONFIG.Button.CLICKR;
		ButtonCAPTURE.InputKey = KEYCONFIG.Button.CAPTURE;
		Dx1.Text = ((KEYCONFIG.DxButton.A == "") ? "None" : KEYCONFIG.DxButton.A);
		Dx2.Text = ((KEYCONFIG.DxButton.B == "") ? "None" : KEYCONFIG.DxButton.B);
		Dx3.Text = ((KEYCONFIG.DxButton.X == "") ? "None" : KEYCONFIG.DxButton.X);
		Dx4.Text = ((KEYCONFIG.DxButton.Y == "") ? "None" : KEYCONFIG.DxButton.Y);
		Dx5.Text = ((KEYCONFIG.DxButton.L == "") ? "None" : KEYCONFIG.DxButton.L);
		Dx6.Text = ((KEYCONFIG.DxButton.R == "") ? "None" : KEYCONFIG.DxButton.R);
		Dx7.Text = ((KEYCONFIG.DxButton.ZL == "") ? "None" : KEYCONFIG.DxButton.ZL);
		Dx8.Text = ((KEYCONFIG.DxButton.ZR == "") ? "None" : KEYCONFIG.DxButton.ZR);
		Dx9.Text = ((KEYCONFIG.DxButton.CLICKL == "") ? "None" : KEYCONFIG.DxButton.CLICKL);
		Dx10.Text = ((KEYCONFIG.DxButton.CLICKR == "") ? "None" : KEYCONFIG.DxButton.CLICKR);
		Dx11.Text = ((KEYCONFIG.DxButton.START == "") ? "None" : KEYCONFIG.DxButton.START);
		Dx12.Text = ((KEYCONFIG.DxButton.SELECT == "") ? "None" : KEYCONFIG.DxButton.SELECT);
		Dx13.Text = ((KEYCONFIG.DxButton.HOME == "") ? "None" : KEYCONFIG.DxButton.HOME);
		Dx14.Text = ((KEYCONFIG.DxButton.CAPTURE == "") ? "None" : KEYCONFIG.DxButton.CAPTURE);
		Dx15.Text = ((KEYCONFIG.DxDPad.UP == "") ? "None" : KEYCONFIG.DxDPad.UP);
		Dx16.Text = ((KEYCONFIG.DxDPad.DOWN == "") ? "None" : KEYCONFIG.DxDPad.DOWN);
		Dx17.Text = ((KEYCONFIG.DxDPad.LEFT == "") ? "None" : KEYCONFIG.DxDPad.LEFT);
		Dx18.Text = ((KEYCONFIG.DxDPad.RIGHT == "") ? "None" : KEYCONFIG.DxDPad.RIGHT);
		Dx19.Text = ((KEYCONFIG.DxAnalogL.UP == "") ? "None" : KEYCONFIG.DxAnalogL.UP);
		Dx20.Text = ((KEYCONFIG.DxAnalogL.DOWN == "") ? "None" : KEYCONFIG.DxAnalogL.DOWN);
		Dx21.Text = ((KEYCONFIG.DxAnalogL.LEFT == "") ? "None" : KEYCONFIG.DxAnalogL.LEFT);
		Dx22.Text = ((KEYCONFIG.DxAnalogL.RIGHT == "") ? "None" : KEYCONFIG.DxAnalogL.RIGHT);
		Dx23.Text = ((KEYCONFIG.DxAnalogR.UP == "") ? "None" : KEYCONFIG.DxAnalogR.UP);
		Dx24.Text = ((KEYCONFIG.DxAnalogR.DOWN == "") ? "None" : KEYCONFIG.DxAnalogR.DOWN);
		Dx25.Text = ((KEYCONFIG.DxAnalogR.LEFT == "") ? "None" : KEYCONFIG.DxAnalogR.LEFT);
		Dx26.Text = ((KEYCONFIG.DxAnalogR.RIGHT == "") ? "None" : KEYCONFIG.DxAnalogR.RIGHT);
		XI1.Text = ((KEYCONFIG.XiButton.A == "") ? "None" : KEYCONFIG.XiButton.A);
		XI2.Text = ((KEYCONFIG.XiButton.B == "") ? "None" : KEYCONFIG.XiButton.B);
		XI3.Text = ((KEYCONFIG.XiButton.X == "") ? "None" : KEYCONFIG.XiButton.X);
		XI4.Text = ((KEYCONFIG.XiButton.Y == "") ? "None" : KEYCONFIG.XiButton.Y);
		XI5.Text = ((KEYCONFIG.XiButton.L == "") ? "None" : KEYCONFIG.XiButton.L);
		XI6.Text = ((KEYCONFIG.XiButton.R == "") ? "None" : KEYCONFIG.XiButton.R);
		XI7.Text = ((KEYCONFIG.XiButton.ZL == "") ? "None" : KEYCONFIG.XiButton.ZL);
		XI8.Text = ((KEYCONFIG.XiButton.ZR == "") ? "None" : KEYCONFIG.XiButton.ZR);
		XI9.Text = ((KEYCONFIG.XiButton.CLICKL == "") ? "None" : KEYCONFIG.XiButton.CLICKL);
		XI10.Text = ((KEYCONFIG.XiButton.CLICKR == "") ? "None" : KEYCONFIG.XiButton.CLICKR);
		XI11.Text = ((KEYCONFIG.XiButton.START == "") ? "None" : KEYCONFIG.XiButton.START);
		XI12.Text = ((KEYCONFIG.XiButton.SELECT == "") ? "None" : KEYCONFIG.XiButton.SELECT);
		XI13.Text = ((KEYCONFIG.XiButton.HOME == "") ? "None" : KEYCONFIG.XiButton.HOME);
		XI14.Text = ((KEYCONFIG.XiButton.CAPTURE == "") ? "None" : KEYCONFIG.XiButton.CAPTURE);
		XI15.Text = ((KEYCONFIG.XiDPad.UP == "") ? "None" : KEYCONFIG.XiDPad.UP);
		XI16.Text = ((KEYCONFIG.XiDPad.DOWN == "") ? "None" : KEYCONFIG.XiDPad.DOWN);
		XI17.Text = ((KEYCONFIG.XiDPad.LEFT == "") ? "None" : KEYCONFIG.XiDPad.LEFT);
		XI18.Text = ((KEYCONFIG.XiDPad.RIGHT == "") ? "None" : KEYCONFIG.XiDPad.RIGHT);
		XI19.Text = ((KEYCONFIG.XiAnalogL.UP == "") ? "None" : KEYCONFIG.XiAnalogL.UP);
		XI20.Text = ((KEYCONFIG.XiAnalogL.DOWN == "") ? "None" : KEYCONFIG.XiAnalogL.DOWN);
		XI21.Text = ((KEYCONFIG.XiAnalogL.LEFT == "") ? "None" : KEYCONFIG.XiAnalogL.LEFT);
		XI22.Text = ((KEYCONFIG.XiAnalogL.RIGHT == "") ? "None" : KEYCONFIG.XiAnalogL.RIGHT);
		XI23.Text = ((KEYCONFIG.XiAnalogR.UP == "") ? "None" : KEYCONFIG.XiAnalogR.UP);
		XI24.Text = ((KEYCONFIG.XiAnalogR.DOWN == "") ? "None" : KEYCONFIG.XiAnalogR.DOWN);
		XI25.Text = ((KEYCONFIG.XiAnalogR.LEFT == "") ? "None" : KEYCONFIG.XiAnalogR.LEFT);
		XI26.Text = ((KEYCONFIG.XiAnalogR.RIGHT == "") ? "None" : KEYCONFIG.XiAnalogR.RIGHT);
		checkBox4.Checked = KEYCONFIG.ControlConfig.USEKEYBOARD;
		checkBox1.Checked = KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO;
		checkBox2.Checked = KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE;
		checkBox3.Checked = KEYCONFIG.ControlConfig.GAMEPADONLY;
		checkBox5.Checked = KEYCONFIG.AppConfig.UPDATECHECK;
		checkBox6.Checked = KEYCONFIG.EditorConfig.RUNNINGFOCUS;
		checkBox7.Checked = KEYCONFIG.ControlConfig.USESTICKBINARY;
		checkBox8.Checked = KEYCONFIG.ControlConfig.REC8AXIS;
		if (KEYCONFIG.AppConfig.APPTHEME == Style.Dark)
		{
			comboBox1.SelectedIndex = 0;
		}
		else
		{
			comboBox1.SelectedIndex = 1;
		}
		if (KEYCONFIG.AppConfig.CAPTURESTYLE == NXMC_VxV.CaptureStyle.DirectShow)
		{
			comboBox2.SelectedIndex = 0;
		}
		else
		{
			comboBox2.SelectedIndex = 1;
		}
		textBox1.Text = GlobalVar.CaptureOutput;
		textBox2.Text = NxCommand.LineNotifyToken;
		Util.EnableDoubleBuffering(tabControl1);
	}

	private void groupBox1_Enter(object sender, EventArgs e)
	{
	}

	private void groupBox4_Enter(object sender, EventArgs e)
	{
	}

	private void checkBox4_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void checkBox2_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void checkBox3_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void receiveKeyTextBox2_TextChanged(object sender, EventArgs e)
	{
		ButtonClickR.Focus();
	}

	private void ButtonClickR_TextChanged(object sender, EventArgs e)
	{
		ButtonSTART.Focus();
	}

	private void SettingDialog_FormClosed(object sender, FormClosedEventArgs e)
	{
	}

	private void button2_Click(object sender, EventArgs e)
	{
		KEYCONFIG.Button.A = ButtonA.InputKey;
		KEYCONFIG.Button.B = ButtonB.InputKey;
		KEYCONFIG.Button.X = ButtonX.InputKey;
		KEYCONFIG.Button.Y = ButtonY.InputKey;
		KEYCONFIG.Button.L = ButtonL.InputKey;
		KEYCONFIG.Button.R = ButtonR.InputKey;
		KEYCONFIG.Button.ZL = ButtonZL.InputKey;
		KEYCONFIG.Button.ZR = ButtonZR.InputKey;
		KEYCONFIG.Button.START = ButtonSTART.InputKey;
		KEYCONFIG.Button.SELECT = ButtonSELECT.InputKey;
		KEYCONFIG.Button.HOME = ButtonHOME.InputKey;
		KEYCONFIG.DPad.UP = DUP.InputKey;
		KEYCONFIG.DPad.DOWN = DDOWN.InputKey;
		KEYCONFIG.DPad.LEFT = DLEFT.InputKey;
		KEYCONFIG.DPad.RIGHT = DRIGHT.InputKey;
		KEYCONFIG.AnalogL.UP = ANALOGLUP.InputKey;
		KEYCONFIG.AnalogL.DOWN = ANALOGLDOWN.InputKey;
		KEYCONFIG.AnalogL.LEFT = ANALOGLLEFT.InputKey;
		KEYCONFIG.AnalogL.RIGHT = ANALOGLRIGHT.InputKey;
		KEYCONFIG.AnalogR.UP = ANALOGRUP.InputKey;
		KEYCONFIG.AnalogR.DOWN = ANALOGRDOWN.InputKey;
		KEYCONFIG.AnalogR.LEFT = ANALOGRLEFT.InputKey;
		KEYCONFIG.AnalogR.RIGHT = ANALOGRRIGHT.InputKey;
		KEYCONFIG.ControlConfig.USEKEYBOARD = checkBox4.Checked;
		KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO = checkBox1.Checked;
		KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE = checkBox2.Checked;
		KEYCONFIG.ControlConfig.GAMEPADONLY = checkBox3.Checked;
		KEYCONFIG.Button.CLICKL = ButtonClickL.InputKey;
		KEYCONFIG.Button.CLICKR = ButtonClickR.InputKey;
		KEYCONFIG.AppConfig.UPDATECHECK = checkBox5.Checked;
		if (comboBox1.SelectedIndex == 0)
		{
			KEYCONFIG.AppConfig.APPTHEME = Style.Dark;
		}
		else
		{
			KEYCONFIG.AppConfig.APPTHEME = Style.Light;
		}
		KEYCONFIG.Button.CAPTURE = ButtonCAPTURE.InputKey;
		KEYCONFIG.EditorConfig.RUNNINGFOCUS = checkBox6.Checked;
		GlobalVar.CaptureOutput = textBox1.Text;
		NxCommand.LineNotifyToken = textBox2.Text;
		KEYCONFIG.DxButton.A = Dx1.Text;
		KEYCONFIG.DxButton.B = Dx2.Text;
		KEYCONFIG.DxButton.X = Dx3.Text;
		KEYCONFIG.DxButton.Y = Dx4.Text;
		KEYCONFIG.DxButton.L = Dx5.Text;
		KEYCONFIG.DxButton.R = Dx6.Text;
		KEYCONFIG.DxButton.ZL = Dx7.Text;
		KEYCONFIG.DxButton.ZR = Dx8.Text;
		KEYCONFIG.DxButton.CLICKL = Dx9.Text;
		KEYCONFIG.DxButton.CLICKR = Dx10.Text;
		KEYCONFIG.DxButton.START = Dx11.Text;
		KEYCONFIG.DxButton.SELECT = Dx12.Text;
		KEYCONFIG.DxButton.HOME = Dx13.Text;
		KEYCONFIG.DxButton.CAPTURE = Dx14.Text;
		KEYCONFIG.DxDPad.UP = Dx15.Text;
		KEYCONFIG.DxDPad.DOWN = Dx16.Text;
		KEYCONFIG.DxDPad.LEFT = Dx17.Text;
		KEYCONFIG.DxDPad.RIGHT = Dx18.Text;
		KEYCONFIG.DxAnalogL.UP = Dx19.Text;
		KEYCONFIG.DxAnalogL.DOWN = Dx20.Text;
		KEYCONFIG.DxAnalogL.LEFT = Dx21.Text;
		KEYCONFIG.DxAnalogL.RIGHT = Dx22.Text;
		KEYCONFIG.DxAnalogR.UP = Dx23.Text;
		KEYCONFIG.DxAnalogR.DOWN = Dx24.Text;
		KEYCONFIG.DxAnalogR.LEFT = Dx25.Text;
		KEYCONFIG.DxAnalogR.RIGHT = Dx26.Text;
		KEYCONFIG.XiButton.A = XI1.Text;
		KEYCONFIG.XiButton.B = XI2.Text;
		KEYCONFIG.XiButton.X = XI3.Text;
		KEYCONFIG.XiButton.Y = XI4.Text;
		KEYCONFIG.XiButton.L = XI5.Text;
		KEYCONFIG.XiButton.R = XI6.Text;
		KEYCONFIG.XiButton.ZL = XI7.Text;
		KEYCONFIG.XiButton.ZR = XI8.Text;
		KEYCONFIG.XiButton.CLICKL = XI9.Text;
		KEYCONFIG.XiButton.CLICKR = XI10.Text;
		KEYCONFIG.XiButton.START = XI11.Text;
		KEYCONFIG.XiButton.SELECT = XI12.Text;
		KEYCONFIG.XiButton.HOME = XI13.Text;
		KEYCONFIG.XiButton.CAPTURE = XI14.Text;
		KEYCONFIG.XiDPad.UP = XI15.Text;
		KEYCONFIG.XiDPad.DOWN = XI16.Text;
		KEYCONFIG.XiDPad.LEFT = XI17.Text;
		KEYCONFIG.XiDPad.RIGHT = XI18.Text;
		KEYCONFIG.XiAnalogL.UP = XI19.Text;
		KEYCONFIG.XiAnalogL.DOWN = XI20.Text;
		KEYCONFIG.XiAnalogL.LEFT = XI21.Text;
		KEYCONFIG.XiAnalogL.RIGHT = XI22.Text;
		KEYCONFIG.XiAnalogR.UP = XI23.Text;
		KEYCONFIG.XiAnalogR.DOWN = XI24.Text;
		KEYCONFIG.XiAnalogR.LEFT = XI25.Text;
		KEYCONFIG.XiAnalogR.RIGHT = XI26.Text;
		if (comboBox2.SelectedIndex == 0)
		{
			KEYCONFIG.AppConfig.CAPTURESTYLE = NXMC_VxV.CaptureStyle.DirectShow;
		}
		else if (comboBox2.SelectedIndex == 1)
		{
			KEYCONFIG.AppConfig.CAPTURESTYLE = NXMC_VxV.CaptureStyle.OpenCV;
		}
		KEYCONFIG.ControlConfig.USESTICKBINARY = checkBox7.Checked;
		KEYCONFIG.ControlConfig.REC8AXIS = checkBox8.Checked;
		Util.SaveConfig();
		Util.ReadConfig();
		SettingChanged = true;
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void checkBox5_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void ButtonCAPTURE_TextChanged(object sender, EventArgs e)
	{
		DUP.Focus();
	}

	private void checkBox6_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void button3_Click(object sender, EventArgs e)
	{
		FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
		if (folderSelectDialog.ShowDialog() == DialogResult.OK)
		{
			textBox1.Text = folderSelectDialog.Path;
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Process.Start("EXPLORER.EXE", textBox1.Text.Replace("/", "\\"));
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void textBox2_TextChanged(object sender, EventArgs e)
	{
	}

	private void receiveKeyTextBox22_Enter(object sender, EventArgs e)
	{
	}

	private void receiveKeyTextBox22_Leave(object sender, EventArgs e)
	{
	}

	private void receiveKeyTextBox22_TextChanged(object sender, EventArgs e)
	{
		Dx2.Focus();
	}

	private void Dx2_TextChanged(object sender, EventArgs e)
	{
		Dx3.Focus();
	}

	private void Dx3_TextChanged(object sender, EventArgs e)
	{
		Dx4.Focus();
	}

	private void Dx4_TextChanged(object sender, EventArgs e)
	{
		Dx5.Focus();
	}

	private void Dx5_TextChanged(object sender, EventArgs e)
	{
		Dx6.Focus();
	}

	private void Dx6_TextChanged(object sender, EventArgs e)
	{
		Dx7.Focus();
	}

	private void Dx7_TextChanged(object sender, EventArgs e)
	{
		Dx8.Focus();
	}

	private void Dx8_TextChanged(object sender, EventArgs e)
	{
		Dx9.Focus();
	}

	private void Dx9_TextChanged(object sender, EventArgs e)
	{
		Dx10.Focus();
	}

	private void Dx10_TextChanged(object sender, EventArgs e)
	{
		Dx11.Focus();
	}

	private void Dx11_TextChanged(object sender, EventArgs e)
	{
		Dx12.Focus();
	}

	private void Dx12_TextChanged(object sender, EventArgs e)
	{
		Dx13.Focus();
	}

	private void Dx13_TextChanged(object sender, EventArgs e)
	{
		Dx14.Focus();
	}

	private void Dx14_TextChanged(object sender, EventArgs e)
	{
		Dx15.Focus();
	}

	private void Dx15_TextChanged(object sender, EventArgs e)
	{
		Dx16.Focus();
	}

	private void Dx16_TextChanged(object sender, EventArgs e)
	{
		Dx17.Focus();
	}

	private void Dx17_TextChanged(object sender, EventArgs e)
	{
		Dx18.Focus();
	}

	private void Dx18_TextChanged(object sender, EventArgs e)
	{
		Dx19.Focus();
	}

	private void Dx19_TextChanged(object sender, EventArgs e)
	{
		Dx20.Focus();
	}

	private void Dx20_TextChanged(object sender, EventArgs e)
	{
		Dx21.Focus();
	}

	private void Dx21_TextChanged(object sender, EventArgs e)
	{
		Dx22.Focus();
	}

	private void Dx22_TextChanged(object sender, EventArgs e)
	{
		Dx23.Focus();
	}

	private void Dx23_TextChanged(object sender, EventArgs e)
	{
		Dx24.Focus();
	}

	private void Dx24_TextChanged(object sender, EventArgs e)
	{
		Dx25.Focus();
	}

	private void Dx25_TextChanged(object sender, EventArgs e)
	{
		Dx26.Focus();
	}

	private void Dx26_TextChanged(object sender, EventArgs e)
	{
	}

	private void XI1_TextChanged(object sender, EventArgs e)
	{
		XI2.Focus();
	}

	private void XI2_TextChanged(object sender, EventArgs e)
	{
		XI3.Focus();
	}

	private void XI3_TextChanged(object sender, EventArgs e)
	{
		XI4.Focus();
	}

	private void XI4_TextChanged(object sender, EventArgs e)
	{
		XI5.Focus();
	}

	private void XI5_TextChanged(object sender, EventArgs e)
	{
		XI6.Focus();
	}

	private void XI6_TextChanged(object sender, EventArgs e)
	{
		XI7.Focus();
	}

	private void XI7_TextChanged(object sender, EventArgs e)
	{
		XI8.Focus();
	}

	private void XI8_TextChanged(object sender, EventArgs e)
	{
		XI9.Focus();
	}

	private void XI9_TextChanged(object sender, EventArgs e)
	{
		XI10.Focus();
	}

	private void XI10_TextChanged(object sender, EventArgs e)
	{
		XI11.Focus();
	}

	private void XI11_TextChanged(object sender, EventArgs e)
	{
		XI12.Focus();
	}

	private void XI12_TextChanged(object sender, EventArgs e)
	{
		XI13.Focus();
	}

	private void XI13_TextChanged(object sender, EventArgs e)
	{
		XI14.Focus();
	}

	private void XI14_TextChanged(object sender, EventArgs e)
	{
		XI15.Focus();
	}

	private void XI15_TextChanged(object sender, EventArgs e)
	{
		XI16.Focus();
	}

	private void XI16_TextChanged(object sender, EventArgs e)
	{
		XI17.Focus();
	}

	private void XI17_TextChanged(object sender, EventArgs e)
	{
		XI18.Focus();
	}

	private void XI18_TextChanged(object sender, EventArgs e)
	{
		XI19.Focus();
	}

	private void XI19_TextChanged(object sender, EventArgs e)
	{
		XI20.Focus();
	}

	private void XI20_TextChanged(object sender, EventArgs e)
	{
		XI21.Focus();
	}

	private void XI21_TextChanged(object sender, EventArgs e)
	{
		XI22.Focus();
	}

	private void XI22_TextChanged(object sender, EventArgs e)
	{
		XI23.Focus();
	}

	private void XI23_TextChanged(object sender, EventArgs e)
	{
		XI24.Focus();
	}

	private void XI24_TextChanged(object sender, EventArgs e)
	{
		XI25.Focus();
	}

	private void XI25_TextChanged(object sender, EventArgs e)
	{
		XI26.Focus();
	}

	private void XI26_TextChanged(object sender, EventArgs e)
	{
	}

	private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void checkBox7_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void checkBox8_CheckedChanged(object sender, EventArgs e)
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.groupBox9 = new System.Windows.Forms.GroupBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox10 = new System.Windows.Forms.GroupBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.groupBox12 = new System.Windows.Forms.GroupBox();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.groupBox13 = new System.Windows.Forms.GroupBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.groupBox14 = new System.Windows.Forms.GroupBox();
		this.label64 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.groupBox15 = new System.Windows.Forms.GroupBox();
		this.label78 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.label81 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.checkBox6 = new System.Windows.Forms.CheckBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.checkBox8 = new System.Windows.Forms.CheckBox();
		this.checkBox7 = new System.Windows.Forms.CheckBox();
		this.checkBox4 = new System.Windows.Forms.CheckBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.checkBox3 = new System.Windows.Forms.CheckBox();
		this.checkBox2 = new System.Windows.Forms.CheckBox();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label82 = new System.Windows.Forms.Label();
		this.comboBox2 = new System.Windows.Forms.ComboBox();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.button4 = new System.Windows.Forms.Button();
		this.checkBox5 = new System.Windows.Forms.CheckBox();
		this.button3 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label27 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.ANALOGRRIGHT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGRLEFT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGRDOWN = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGRUP = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGLRIGHT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGLLEFT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGLDOWN = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ANALOGLUP = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonCAPTURE = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonClickR = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonClickL = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonZR = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonSTART = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonHOME = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonSELECT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonZL = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonR = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonL = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonY = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonX = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonB = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.ButtonA = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.DRIGHT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.DLEFT = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.DDOWN = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.DUP = new NX_Macro_Controller_VxV.ReceiveKeyTextBox();
		this.Dx26 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx25 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx24 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx23 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx22 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx21 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx20 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx19 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx14 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx10 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx9 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx8 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx11 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx13 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx12 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx7 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx6 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx5 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx4 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx3 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx2 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx1 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx18 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx17 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx16 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.Dx15 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI26 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI25 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI24 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI23 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI22 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI21 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI20 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI19 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI14 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI10 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI9 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI8 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI11 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI13 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI12 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI7 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI6 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI5 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI4 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI3 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI2 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI1 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI18 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI17 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI16 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.XI15 = new NX_Macro_Controller_VxV.ReceivePadKeyTextBox();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.groupBox8.SuspendLayout();
		this.groupBox9.SuspendLayout();
		this.groupBox10.SuspendLayout();
		this.groupBox11.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.groupBox12.SuspendLayout();
		this.groupBox13.SuspendLayout();
		this.groupBox14.SuspendLayout();
		this.groupBox15.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox6.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.ButtonCAPTURE);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.ButtonClickR);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.ButtonClickL);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.ButtonZR);
		this.groupBox1.Controls.Add(this.ButtonSTART);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.ButtonHOME);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.ButtonSELECT);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.ButtonZL);
		this.groupBox1.Controls.Add(this.ButtonR);
		this.groupBox1.Controls.Add(this.ButtonL);
		this.groupBox1.Controls.Add(this.ButtonY);
		this.groupBox1.Controls.Add(this.ButtonX);
		this.groupBox1.Controls.Add(this.ButtonB);
		this.groupBox1.Controls.Add(this.ButtonA);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(8, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(185, 387);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "ボタン";
		this.groupBox1.Enter += new System.EventHandler(groupBox1_Enter);
		this.label26.AutoSize = true;
		this.label26.Location = new System.Drawing.Point(6, 360);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(54, 13);
		this.label26.TabIndex = 27;
		this.label26.Text = "CAPTURE";
		this.label24.AutoSize = true;
		this.label24.Location = new System.Drawing.Point(6, 256);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(42, 13);
		this.label24.TabIndex = 25;
		this.label24.Text = "RCLICK";
		this.label25.AutoSize = true;
		this.label25.Location = new System.Drawing.Point(6, 230);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(40, 13);
		this.label25.TabIndex = 23;
		this.label25.Text = "LCLICK";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(6, 282);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(38, 13);
		this.label9.TabIndex = 18;
		this.label9.Text = "START";
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(6, 204);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(20, 13);
		this.label8.TabIndex = 15;
		this.label8.Text = "ZR";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(6, 334);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(40, 13);
		this.label11.TabIndex = 21;
		this.label11.Text = "HOME";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(6, 308);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(43, 13);
		this.label10.TabIndex = 19;
		this.label10.Text = "SELECT";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(6, 178);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(18, 13);
		this.label7.TabIndex = 13;
		this.label7.Text = "ZL";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(6, 152);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 13);
		this.label6.TabIndex = 12;
		this.label6.Text = "R";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(6, 126);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(12, 13);
		this.label5.TabIndex = 11;
		this.label5.Text = "L";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(6, 100);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(12, 13);
		this.label4.TabIndex = 10;
		this.label4.Text = "Y";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(6, 74);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(13, 13);
		this.label3.TabIndex = 9;
		this.label3.Text = "X";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(6, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(13, 13);
		this.label2.TabIndex = 1;
		this.label2.Text = "B";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(6, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "A";
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.DRIGHT);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.DLEFT);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.DDOWN);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.DUP);
		this.groupBox2.Location = new System.Drawing.Point(199, 5);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Size = new System.Drawing.Size(185, 127);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "十字キー";
		this.label15.AutoSize = true;
		this.label15.Location = new System.Drawing.Point(6, 100);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(39, 13);
		this.label15.TabIndex = 28;
		this.label15.Text = "RIGHT";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(6, 74);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(30, 13);
		this.label14.TabIndex = 26;
		this.label14.Text = "LEFT";
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(6, 48);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(43, 13);
		this.label13.TabIndex = 24;
		this.label13.Text = "DOWN";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(6, 22);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(21, 13);
		this.label12.TabIndex = 22;
		this.label12.Text = "UP";
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(591, 428);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.groupBox4);
		this.tabPage1.Controls.Add(this.groupBox3);
		this.tabPage1.Controls.Add(this.groupBox1);
		this.tabPage1.Controls.Add(this.groupBox2);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.tabPage1.Size = new System.Drawing.Size(583, 402);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "キー設定";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.ANALOGRRIGHT);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add(this.ANALOGRLEFT);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.ANALOGRDOWN);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.ANALOGRUP);
		this.groupBox4.Location = new System.Drawing.Point(390, 136);
		this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Size = new System.Drawing.Size(185, 127);
		this.groupBox4.TabIndex = 3;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "右アナログパッド";
		this.groupBox4.Enter += new System.EventHandler(groupBox4_Enter);
		this.label20.AutoSize = true;
		this.label20.Location = new System.Drawing.Point(6, 100);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(39, 13);
		this.label20.TabIndex = 28;
		this.label20.Text = "RIGHT";
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(6, 74);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 13);
		this.label21.TabIndex = 26;
		this.label21.Text = "LEFT";
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(6, 48);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(43, 13);
		this.label22.TabIndex = 24;
		this.label22.Text = "DOWN";
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(6, 22);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(21, 13);
		this.label23.TabIndex = 22;
		this.label23.Text = "UP";
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.ANALOGLRIGHT);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.ANALOGLLEFT);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.ANALOGLDOWN);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.ANALOGLUP);
		this.groupBox3.Location = new System.Drawing.Point(390, 5);
		this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Size = new System.Drawing.Size(185, 127);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "左アナログパッド";
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(6, 100);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(39, 13);
		this.label16.TabIndex = 28;
		this.label16.Text = "RIGHT";
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(6, 74);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(30, 13);
		this.label17.TabIndex = 26;
		this.label17.Text = "LEFT";
		this.label18.AutoSize = true;
		this.label18.Location = new System.Drawing.Point(6, 48);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(43, 13);
		this.label18.TabIndex = 24;
		this.label18.Text = "DOWN";
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(6, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(21, 13);
		this.label19.TabIndex = 22;
		this.label19.Text = "UP";
		this.tabPage3.Controls.Add(this.groupBox8);
		this.tabPage3.Controls.Add(this.groupBox9);
		this.tabPage3.Controls.Add(this.groupBox10);
		this.tabPage3.Controls.Add(this.groupBox11);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(583, 402);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "ゲームパッド(DirectX)設定";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.groupBox8.Controls.Add(this.label30);
		this.groupBox8.Controls.Add(this.Dx26);
		this.groupBox8.Controls.Add(this.label31);
		this.groupBox8.Controls.Add(this.Dx25);
		this.groupBox8.Controls.Add(this.label32);
		this.groupBox8.Controls.Add(this.Dx24);
		this.groupBox8.Controls.Add(this.label33);
		this.groupBox8.Controls.Add(this.Dx23);
		this.groupBox8.Location = new System.Drawing.Point(390, 136);
		this.groupBox8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox8.Size = new System.Drawing.Size(185, 127);
		this.groupBox8.TabIndex = 7;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "右アナログパッド";
		this.label30.AutoSize = true;
		this.label30.Location = new System.Drawing.Point(6, 100);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(39, 13);
		this.label30.TabIndex = 28;
		this.label30.Text = "RIGHT";
		this.label31.AutoSize = true;
		this.label31.Location = new System.Drawing.Point(6, 74);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(30, 13);
		this.label31.TabIndex = 26;
		this.label31.Text = "LEFT";
		this.label32.AutoSize = true;
		this.label32.Location = new System.Drawing.Point(6, 48);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(43, 13);
		this.label32.TabIndex = 24;
		this.label32.Text = "DOWN";
		this.label33.AutoSize = true;
		this.label33.Location = new System.Drawing.Point(6, 22);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(21, 13);
		this.label33.TabIndex = 22;
		this.label33.Text = "UP";
		this.groupBox9.Controls.Add(this.label34);
		this.groupBox9.Controls.Add(this.Dx22);
		this.groupBox9.Controls.Add(this.label35);
		this.groupBox9.Controls.Add(this.Dx21);
		this.groupBox9.Controls.Add(this.label36);
		this.groupBox9.Controls.Add(this.Dx20);
		this.groupBox9.Controls.Add(this.label37);
		this.groupBox9.Controls.Add(this.Dx19);
		this.groupBox9.Location = new System.Drawing.Point(390, 5);
		this.groupBox9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox9.Name = "groupBox9";
		this.groupBox9.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox9.Size = new System.Drawing.Size(185, 127);
		this.groupBox9.TabIndex = 6;
		this.groupBox9.TabStop = false;
		this.groupBox9.Text = "左アナログパッド";
		this.label34.AutoSize = true;
		this.label34.Location = new System.Drawing.Point(6, 100);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(39, 13);
		this.label34.TabIndex = 28;
		this.label34.Text = "RIGHT";
		this.label35.AutoSize = true;
		this.label35.Location = new System.Drawing.Point(6, 74);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(30, 13);
		this.label35.TabIndex = 26;
		this.label35.Text = "LEFT";
		this.label36.AutoSize = true;
		this.label36.Location = new System.Drawing.Point(6, 48);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(43, 13);
		this.label36.TabIndex = 24;
		this.label36.Text = "DOWN";
		this.label37.AutoSize = true;
		this.label37.Location = new System.Drawing.Point(6, 22);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(21, 13);
		this.label37.TabIndex = 22;
		this.label37.Text = "UP";
		this.groupBox10.Controls.Add(this.label38);
		this.groupBox10.Controls.Add(this.Dx14);
		this.groupBox10.Controls.Add(this.label39);
		this.groupBox10.Controls.Add(this.Dx10);
		this.groupBox10.Controls.Add(this.label40);
		this.groupBox10.Controls.Add(this.Dx9);
		this.groupBox10.Controls.Add(this.label41);
		this.groupBox10.Controls.Add(this.label42);
		this.groupBox10.Controls.Add(this.label43);
		this.groupBox10.Controls.Add(this.Dx8);
		this.groupBox10.Controls.Add(this.Dx11);
		this.groupBox10.Controls.Add(this.label44);
		this.groupBox10.Controls.Add(this.label45);
		this.groupBox10.Controls.Add(this.Dx13);
		this.groupBox10.Controls.Add(this.label46);
		this.groupBox10.Controls.Add(this.Dx12);
		this.groupBox10.Controls.Add(this.label47);
		this.groupBox10.Controls.Add(this.label48);
		this.groupBox10.Controls.Add(this.label49);
		this.groupBox10.Controls.Add(this.Dx7);
		this.groupBox10.Controls.Add(this.Dx6);
		this.groupBox10.Controls.Add(this.Dx5);
		this.groupBox10.Controls.Add(this.Dx4);
		this.groupBox10.Controls.Add(this.Dx3);
		this.groupBox10.Controls.Add(this.Dx2);
		this.groupBox10.Controls.Add(this.Dx1);
		this.groupBox10.Controls.Add(this.label50);
		this.groupBox10.Controls.Add(this.label51);
		this.groupBox10.Location = new System.Drawing.Point(8, 5);
		this.groupBox10.Name = "groupBox10";
		this.groupBox10.Size = new System.Drawing.Size(185, 387);
		this.groupBox10.TabIndex = 4;
		this.groupBox10.TabStop = false;
		this.groupBox10.Text = "ボタン";
		this.label38.AutoSize = true;
		this.label38.Location = new System.Drawing.Point(6, 360);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 13);
		this.label38.TabIndex = 27;
		this.label38.Text = "CAPTURE";
		this.label39.AutoSize = true;
		this.label39.Location = new System.Drawing.Point(6, 256);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(42, 13);
		this.label39.TabIndex = 25;
		this.label39.Text = "RCLICK";
		this.label40.AutoSize = true;
		this.label40.Location = new System.Drawing.Point(6, 230);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(40, 13);
		this.label40.TabIndex = 23;
		this.label40.Text = "LCLICK";
		this.label41.AutoSize = true;
		this.label41.Location = new System.Drawing.Point(6, 282);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(38, 13);
		this.label41.TabIndex = 18;
		this.label41.Text = "START";
		this.label42.AutoSize = true;
		this.label42.Location = new System.Drawing.Point(6, 204);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(20, 13);
		this.label42.TabIndex = 15;
		this.label42.Text = "ZR";
		this.label43.AutoSize = true;
		this.label43.Location = new System.Drawing.Point(6, 334);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(40, 13);
		this.label43.TabIndex = 21;
		this.label43.Text = "HOME";
		this.label44.AutoSize = true;
		this.label44.Location = new System.Drawing.Point(6, 308);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(43, 13);
		this.label44.TabIndex = 19;
		this.label44.Text = "SELECT";
		this.label45.AutoSize = true;
		this.label45.Location = new System.Drawing.Point(6, 178);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(18, 13);
		this.label45.TabIndex = 13;
		this.label45.Text = "ZL";
		this.label46.AutoSize = true;
		this.label46.Location = new System.Drawing.Point(6, 152);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 13);
		this.label46.TabIndex = 12;
		this.label46.Text = "R";
		this.label47.AutoSize = true;
		this.label47.Location = new System.Drawing.Point(6, 126);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(12, 13);
		this.label47.TabIndex = 11;
		this.label47.Text = "L";
		this.label48.AutoSize = true;
		this.label48.Location = new System.Drawing.Point(6, 100);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(12, 13);
		this.label48.TabIndex = 10;
		this.label48.Text = "Y";
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(6, 74);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(13, 13);
		this.label49.TabIndex = 9;
		this.label49.Text = "X";
		this.label50.AutoSize = true;
		this.label50.Location = new System.Drawing.Point(6, 48);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(13, 13);
		this.label50.TabIndex = 1;
		this.label50.Text = "B";
		this.label51.AutoSize = true;
		this.label51.Location = new System.Drawing.Point(6, 22);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 13);
		this.label51.TabIndex = 0;
		this.label51.Text = "A";
		this.groupBox11.Controls.Add(this.label52);
		this.groupBox11.Controls.Add(this.Dx18);
		this.groupBox11.Controls.Add(this.label53);
		this.groupBox11.Controls.Add(this.Dx17);
		this.groupBox11.Controls.Add(this.label54);
		this.groupBox11.Controls.Add(this.Dx16);
		this.groupBox11.Controls.Add(this.label55);
		this.groupBox11.Controls.Add(this.Dx15);
		this.groupBox11.Location = new System.Drawing.Point(199, 5);
		this.groupBox11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox11.Size = new System.Drawing.Size(185, 127);
		this.groupBox11.TabIndex = 5;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "十字キー";
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(6, 100);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(39, 13);
		this.label52.TabIndex = 28;
		this.label52.Text = "RIGHT";
		this.label53.AutoSize = true;
		this.label53.Location = new System.Drawing.Point(6, 74);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(30, 13);
		this.label53.TabIndex = 26;
		this.label53.Text = "LEFT";
		this.label54.AutoSize = true;
		this.label54.Location = new System.Drawing.Point(6, 48);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(43, 13);
		this.label54.TabIndex = 24;
		this.label54.Text = "DOWN";
		this.label55.AutoSize = true;
		this.label55.Location = new System.Drawing.Point(6, 22);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(21, 13);
		this.label55.TabIndex = 22;
		this.label55.Text = "UP";
		this.tabPage4.Controls.Add(this.groupBox12);
		this.tabPage4.Controls.Add(this.groupBox13);
		this.tabPage4.Controls.Add(this.groupBox14);
		this.tabPage4.Controls.Add(this.groupBox15);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(583, 402);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "ゲームパッド(XInput)設定";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.groupBox12.Controls.Add(this.label56);
		this.groupBox12.Controls.Add(this.XI26);
		this.groupBox12.Controls.Add(this.label57);
		this.groupBox12.Controls.Add(this.XI25);
		this.groupBox12.Controls.Add(this.label58);
		this.groupBox12.Controls.Add(this.XI24);
		this.groupBox12.Controls.Add(this.label59);
		this.groupBox12.Controls.Add(this.XI23);
		this.groupBox12.Location = new System.Drawing.Point(390, 136);
		this.groupBox12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox12.Name = "groupBox12";
		this.groupBox12.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox12.Size = new System.Drawing.Size(185, 127);
		this.groupBox12.TabIndex = 11;
		this.groupBox12.TabStop = false;
		this.groupBox12.Text = "右アナログパッド";
		this.label56.AutoSize = true;
		this.label56.Location = new System.Drawing.Point(6, 100);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(39, 13);
		this.label56.TabIndex = 28;
		this.label56.Text = "RIGHT";
		this.label57.AutoSize = true;
		this.label57.Location = new System.Drawing.Point(6, 74);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(30, 13);
		this.label57.TabIndex = 26;
		this.label57.Text = "LEFT";
		this.label58.AutoSize = true;
		this.label58.Location = new System.Drawing.Point(6, 48);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(43, 13);
		this.label58.TabIndex = 24;
		this.label58.Text = "DOWN";
		this.label59.AutoSize = true;
		this.label59.Location = new System.Drawing.Point(6, 22);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(21, 13);
		this.label59.TabIndex = 22;
		this.label59.Text = "UP";
		this.groupBox13.Controls.Add(this.label60);
		this.groupBox13.Controls.Add(this.XI22);
		this.groupBox13.Controls.Add(this.label61);
		this.groupBox13.Controls.Add(this.XI21);
		this.groupBox13.Controls.Add(this.label62);
		this.groupBox13.Controls.Add(this.XI20);
		this.groupBox13.Controls.Add(this.label63);
		this.groupBox13.Controls.Add(this.XI19);
		this.groupBox13.Location = new System.Drawing.Point(390, 5);
		this.groupBox13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox13.Name = "groupBox13";
		this.groupBox13.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox13.Size = new System.Drawing.Size(185, 127);
		this.groupBox13.TabIndex = 10;
		this.groupBox13.TabStop = false;
		this.groupBox13.Text = "左アナログパッド";
		this.label60.AutoSize = true;
		this.label60.Location = new System.Drawing.Point(6, 100);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(39, 13);
		this.label60.TabIndex = 28;
		this.label60.Text = "RIGHT";
		this.label61.AutoSize = true;
		this.label61.Location = new System.Drawing.Point(6, 74);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(30, 13);
		this.label61.TabIndex = 26;
		this.label61.Text = "LEFT";
		this.label62.AutoSize = true;
		this.label62.Location = new System.Drawing.Point(6, 48);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(43, 13);
		this.label62.TabIndex = 24;
		this.label62.Text = "DOWN";
		this.label63.AutoSize = true;
		this.label63.Location = new System.Drawing.Point(6, 22);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(21, 13);
		this.label63.TabIndex = 22;
		this.label63.Text = "UP";
		this.groupBox14.Controls.Add(this.label64);
		this.groupBox14.Controls.Add(this.XI14);
		this.groupBox14.Controls.Add(this.label65);
		this.groupBox14.Controls.Add(this.XI10);
		this.groupBox14.Controls.Add(this.label66);
		this.groupBox14.Controls.Add(this.XI9);
		this.groupBox14.Controls.Add(this.label67);
		this.groupBox14.Controls.Add(this.label68);
		this.groupBox14.Controls.Add(this.label69);
		this.groupBox14.Controls.Add(this.XI8);
		this.groupBox14.Controls.Add(this.XI11);
		this.groupBox14.Controls.Add(this.label70);
		this.groupBox14.Controls.Add(this.label71);
		this.groupBox14.Controls.Add(this.XI13);
		this.groupBox14.Controls.Add(this.label72);
		this.groupBox14.Controls.Add(this.XI12);
		this.groupBox14.Controls.Add(this.label73);
		this.groupBox14.Controls.Add(this.label74);
		this.groupBox14.Controls.Add(this.label75);
		this.groupBox14.Controls.Add(this.XI7);
		this.groupBox14.Controls.Add(this.XI6);
		this.groupBox14.Controls.Add(this.XI5);
		this.groupBox14.Controls.Add(this.XI4);
		this.groupBox14.Controls.Add(this.XI3);
		this.groupBox14.Controls.Add(this.XI2);
		this.groupBox14.Controls.Add(this.XI1);
		this.groupBox14.Controls.Add(this.label76);
		this.groupBox14.Controls.Add(this.label77);
		this.groupBox14.Location = new System.Drawing.Point(8, 5);
		this.groupBox14.Name = "groupBox14";
		this.groupBox14.Size = new System.Drawing.Size(185, 387);
		this.groupBox14.TabIndex = 8;
		this.groupBox14.TabStop = false;
		this.groupBox14.Text = "ボタン";
		this.label64.AutoSize = true;
		this.label64.Location = new System.Drawing.Point(6, 360);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(54, 13);
		this.label64.TabIndex = 27;
		this.label64.Text = "CAPTURE";
		this.label65.AutoSize = true;
		this.label65.Location = new System.Drawing.Point(6, 256);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(42, 13);
		this.label65.TabIndex = 25;
		this.label65.Text = "RCLICK";
		this.label66.AutoSize = true;
		this.label66.Location = new System.Drawing.Point(6, 230);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(40, 13);
		this.label66.TabIndex = 23;
		this.label66.Text = "LCLICK";
		this.label67.AutoSize = true;
		this.label67.Location = new System.Drawing.Point(6, 282);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(38, 13);
		this.label67.TabIndex = 18;
		this.label67.Text = "START";
		this.label68.AutoSize = true;
		this.label68.Location = new System.Drawing.Point(6, 204);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(20, 13);
		this.label68.TabIndex = 15;
		this.label68.Text = "ZR";
		this.label69.AutoSize = true;
		this.label69.Location = new System.Drawing.Point(6, 334);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(40, 13);
		this.label69.TabIndex = 21;
		this.label69.Text = "HOME";
		this.label70.AutoSize = true;
		this.label70.Location = new System.Drawing.Point(6, 308);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(43, 13);
		this.label70.TabIndex = 19;
		this.label70.Text = "SELECT";
		this.label71.AutoSize = true;
		this.label71.Location = new System.Drawing.Point(6, 178);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(18, 13);
		this.label71.TabIndex = 13;
		this.label71.Text = "ZL";
		this.label72.AutoSize = true;
		this.label72.Location = new System.Drawing.Point(6, 152);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(14, 13);
		this.label72.TabIndex = 12;
		this.label72.Text = "R";
		this.label73.AutoSize = true;
		this.label73.Location = new System.Drawing.Point(6, 126);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(12, 13);
		this.label73.TabIndex = 11;
		this.label73.Text = "L";
		this.label74.AutoSize = true;
		this.label74.Location = new System.Drawing.Point(6, 100);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(12, 13);
		this.label74.TabIndex = 10;
		this.label74.Text = "Y";
		this.label75.AutoSize = true;
		this.label75.Location = new System.Drawing.Point(6, 74);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(13, 13);
		this.label75.TabIndex = 9;
		this.label75.Text = "X";
		this.label76.AutoSize = true;
		this.label76.Location = new System.Drawing.Point(6, 48);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(13, 13);
		this.label76.TabIndex = 1;
		this.label76.Text = "B";
		this.label77.AutoSize = true;
		this.label77.Location = new System.Drawing.Point(6, 22);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(14, 13);
		this.label77.TabIndex = 0;
		this.label77.Text = "A";
		this.groupBox15.Controls.Add(this.label78);
		this.groupBox15.Controls.Add(this.XI18);
		this.groupBox15.Controls.Add(this.label79);
		this.groupBox15.Controls.Add(this.XI17);
		this.groupBox15.Controls.Add(this.label80);
		this.groupBox15.Controls.Add(this.XI16);
		this.groupBox15.Controls.Add(this.label81);
		this.groupBox15.Controls.Add(this.XI15);
		this.groupBox15.Location = new System.Drawing.Point(199, 5);
		this.groupBox15.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox15.Name = "groupBox15";
		this.groupBox15.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox15.Size = new System.Drawing.Size(185, 127);
		this.groupBox15.TabIndex = 9;
		this.groupBox15.TabStop = false;
		this.groupBox15.Text = "十字キー";
		this.label78.AutoSize = true;
		this.label78.Location = new System.Drawing.Point(6, 100);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(39, 13);
		this.label78.TabIndex = 28;
		this.label78.Text = "RIGHT";
		this.label79.AutoSize = true;
		this.label79.Location = new System.Drawing.Point(6, 74);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(30, 13);
		this.label79.TabIndex = 26;
		this.label79.Text = "LEFT";
		this.label80.AutoSize = true;
		this.label80.Location = new System.Drawing.Point(6, 48);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(43, 13);
		this.label80.TabIndex = 24;
		this.label80.Text = "DOWN";
		this.label81.AutoSize = true;
		this.label81.Location = new System.Drawing.Point(6, 22);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(21, 13);
		this.label81.TabIndex = 22;
		this.label81.Text = "UP";
		this.tabPage2.Controls.Add(this.groupBox7);
		this.tabPage2.Controls.Add(this.groupBox5);
		this.tabPage2.Controls.Add(this.groupBox6);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.tabPage2.Size = new System.Drawing.Size(583, 402);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "環境設定";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.groupBox7.Controls.Add(this.checkBox6);
		this.groupBox7.Location = new System.Drawing.Point(8, 325);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(567, 42);
		this.groupBox7.TabIndex = 2;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "エディター";
		this.checkBox6.AutoSize = true;
		this.checkBox6.Location = new System.Drawing.Point(6, 18);
		this.checkBox6.Name = "checkBox6";
		this.checkBox6.Size = new System.Drawing.Size(202, 17);
		this.checkBox6.TabIndex = 0;
		this.checkBox6.Text = "実行中の行にフォーカスを合わせる";
		this.checkBox6.UseVisualStyleBackColor = true;
		this.checkBox6.CheckedChanged += new System.EventHandler(checkBox6_CheckedChanged);
		this.groupBox5.Controls.Add(this.checkBox8);
		this.groupBox5.Controls.Add(this.checkBox7);
		this.groupBox5.Controls.Add(this.checkBox4);
		this.groupBox5.Controls.Add(this.checkBox1);
		this.groupBox5.Controls.Add(this.checkBox3);
		this.groupBox5.Controls.Add(this.checkBox2);
		this.groupBox5.Location = new System.Drawing.Point(8, 166);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(567, 153);
		this.groupBox5.TabIndex = 1;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "操作";
		this.checkBox8.AutoSize = true;
		this.checkBox8.Location = new System.Drawing.Point(6, 130);
		this.checkBox8.Name = "checkBox8";
		this.checkBox8.Size = new System.Drawing.Size(217, 17);
		this.checkBox8.TabIndex = 5;
		this.checkBox8.Text = "操作記録中は8方向入力を使用する";
		this.checkBox8.UseVisualStyleBackColor = true;
		this.checkBox8.CheckedChanged += new System.EventHandler(checkBox8_CheckedChanged);
		this.checkBox7.AutoSize = true;
		this.checkBox7.Location = new System.Drawing.Point(6, 107);
		this.checkBox7.Name = "checkBox7";
		this.checkBox7.Size = new System.Drawing.Size(223, 17);
		this.checkBox7.TabIndex = 4;
		this.checkBox7.Text = "アナログパッドの入力を正確に受け取る";
		this.checkBox7.UseVisualStyleBackColor = true;
		this.checkBox7.CheckedChanged += new System.EventHandler(checkBox7_CheckedChanged);
		this.checkBox4.AutoSize = true;
		this.checkBox4.Location = new System.Drawing.Point(6, 18);
		this.checkBox4.Name = "checkBox4";
		this.checkBox4.Size = new System.Drawing.Size(257, 17);
		this.checkBox4.TabIndex = 0;
		this.checkBox4.Text = "キーボード/ゲームパッドによる操作を利用する";
		this.checkBox4.UseVisualStyleBackColor = true;
		this.checkBox4.CheckedChanged += new System.EventHandler(checkBox4_CheckedChanged);
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(6, 40);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(236, 17);
		this.checkBox1.TabIndex = 1;
		this.checkBox1.Text = "マクロ実行中は操作入力を受け付けない\r\n";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.checkBox3.AutoSize = true;
		this.checkBox3.Location = new System.Drawing.Point(6, 84);
		this.checkBox3.Name = "checkBox3";
		this.checkBox3.Size = new System.Drawing.Size(333, 17);
		this.checkBox3.TabIndex = 3;
		this.checkBox3.Text = "ゲームパッド接続時はキーボードの操作入力を受け付けない";
		this.checkBox3.UseVisualStyleBackColor = true;
		this.checkBox3.CheckedChanged += new System.EventHandler(checkBox3_CheckedChanged);
		this.checkBox2.AutoSize = true;
		this.checkBox2.Location = new System.Drawing.Point(6, 62);
		this.checkBox2.Name = "checkBox2";
		this.checkBox2.Size = new System.Drawing.Size(356, 17);
		this.checkBox2.TabIndex = 2;
		this.checkBox2.Text = "ウィンドウが非アクティブ時である場合は操作入力を受け付けない";
		this.checkBox2.UseVisualStyleBackColor = true;
		this.checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);
		this.groupBox6.Controls.Add(this.label82);
		this.groupBox6.Controls.Add(this.comboBox2);
		this.groupBox6.Controls.Add(this.textBox2);
		this.groupBox6.Controls.Add(this.label29);
		this.groupBox6.Controls.Add(this.button4);
		this.groupBox6.Controls.Add(this.checkBox5);
		this.groupBox6.Controls.Add(this.button3);
		this.groupBox6.Controls.Add(this.textBox1);
		this.groupBox6.Controls.Add(this.label28);
		this.groupBox6.Controls.Add(this.comboBox1);
		this.groupBox6.Controls.Add(this.label27);
		this.groupBox6.Location = new System.Drawing.Point(8, 5);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(567, 155);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "アプリケーション";
		this.label82.AutoSize = true;
		this.label82.Location = new System.Drawing.Point(6, 45);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(114, 13);
		this.label82.TabIndex = 8;
		this.label82.Text = "映像キャプチャ方式 :";
		this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox2.FormattingEnabled = true;
		this.comboBox2.Items.AddRange(new object[2] { "DirectShow", "OpenCV" });
		this.comboBox2.Location = new System.Drawing.Point(160, 42);
		this.comboBox2.Name = "comboBox2";
		this.comboBox2.Size = new System.Drawing.Size(121, 21);
		this.comboBox2.TabIndex = 1;
		this.comboBox2.SelectedIndexChanged += new System.EventHandler(comboBox2_SelectedIndexChanged);
		this.textBox2.Location = new System.Drawing.Point(160, 98);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(401, 22);
		this.textBox2.TabIndex = 5;
		this.textBox2.TextChanged += new System.EventHandler(textBox2_TextChanged);
		this.label29.AutoSize = true;
		this.label29.Location = new System.Drawing.Point(6, 101);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(111, 13);
		this.label29.TabIndex = 6;
		this.label29.Text = "LINE Notify トークン :";
		this.button4.Location = new System.Drawing.Point(486, 69);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(75, 23);
		this.button4.TabIndex = 4;
		this.button4.Text = "開く";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.checkBox5.AutoSize = true;
		this.checkBox5.Location = new System.Drawing.Point(9, 126);
		this.checkBox5.Name = "checkBox5";
		this.checkBox5.Size = new System.Drawing.Size(197, 17);
		this.checkBox5.TabIndex = 6;
		this.checkBox5.Text = "起動時にアプリの更新を確認する";
		this.checkBox5.UseVisualStyleBackColor = true;
		this.checkBox5.CheckedChanged += new System.EventHandler(checkBox5_CheckedChanged);
		this.button3.Location = new System.Drawing.Point(441, 69);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(39, 23);
		this.button3.TabIndex = 3;
		this.button3.Text = "...";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.textBox1.Location = new System.Drawing.Point(160, 69);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(275, 22);
		this.textBox1.TabIndex = 2;
		this.textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.label28.AutoSize = true;
		this.label28.Location = new System.Drawing.Point(6, 72);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(146, 13);
		this.label28.TabIndex = 2;
		this.label28.Text = "スクリーンショットの保存先 :";
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Items.AddRange(new object[2] { "濃色", "淡色" });
		this.comboBox1.Location = new System.Drawing.Point(160, 15);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(121, 21);
		this.comboBox1.TabIndex = 0;
		this.comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
		this.label27.AutoSize = true;
		this.label27.Location = new System.Drawing.Point(6, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(70, 13);
		this.label27.TabIndex = 1;
		this.label27.Text = "配色テーマ :";
		this.button1.Location = new System.Drawing.Point(504, 433);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 2;
		this.button1.Text = "キャンセル";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(423, 433);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 1;
		this.button2.Text = "OK";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.ANALOGRRIGHT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGRRIGHT.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGRRIGHT.Location = new System.Drawing.Point(70, 98);
		this.ANALOGRRIGHT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGRRIGHT.Name = "ANALOGRRIGHT";
		this.ANALOGRRIGHT.ShortcutsEnabled = false;
		this.ANALOGRRIGHT.Size = new System.Drawing.Size(104, 22);
		this.ANALOGRRIGHT.TabIndex = 3;
		this.ANALOGRRIGHT.Text = "None";
		this.ANALOGRRIGHT.TextChanged += new System.EventHandler(ANALOGRRIGHT_TextChanged);
		this.ANALOGRLEFT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGRLEFT.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGRLEFT.Location = new System.Drawing.Point(70, 72);
		this.ANALOGRLEFT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGRLEFT.Name = "ANALOGRLEFT";
		this.ANALOGRLEFT.ShortcutsEnabled = false;
		this.ANALOGRLEFT.Size = new System.Drawing.Size(104, 22);
		this.ANALOGRLEFT.TabIndex = 2;
		this.ANALOGRLEFT.Text = "None";
		this.ANALOGRLEFT.TextChanged += new System.EventHandler(ANALOGRLEFT_TextChanged);
		this.ANALOGRDOWN.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGRDOWN.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGRDOWN.Location = new System.Drawing.Point(70, 46);
		this.ANALOGRDOWN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGRDOWN.Name = "ANALOGRDOWN";
		this.ANALOGRDOWN.ShortcutsEnabled = false;
		this.ANALOGRDOWN.Size = new System.Drawing.Size(104, 22);
		this.ANALOGRDOWN.TabIndex = 1;
		this.ANALOGRDOWN.Text = "None";
		this.ANALOGRDOWN.TextChanged += new System.EventHandler(ANALOGRDOWN_TextChanged);
		this.ANALOGRUP.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGRUP.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGRUP.Location = new System.Drawing.Point(70, 20);
		this.ANALOGRUP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGRUP.Name = "ANALOGRUP";
		this.ANALOGRUP.ShortcutsEnabled = false;
		this.ANALOGRUP.Size = new System.Drawing.Size(104, 22);
		this.ANALOGRUP.TabIndex = 0;
		this.ANALOGRUP.Text = "None";
		this.ANALOGRUP.TextChanged += new System.EventHandler(ANALOGRUP_TextChanged);
		this.ANALOGLRIGHT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGLRIGHT.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGLRIGHT.Location = new System.Drawing.Point(70, 98);
		this.ANALOGLRIGHT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGLRIGHT.Name = "ANALOGLRIGHT";
		this.ANALOGLRIGHT.ShortcutsEnabled = false;
		this.ANALOGLRIGHT.Size = new System.Drawing.Size(104, 22);
		this.ANALOGLRIGHT.TabIndex = 3;
		this.ANALOGLRIGHT.Text = "None";
		this.ANALOGLRIGHT.TextChanged += new System.EventHandler(ANALOGLRIGHT_TextChanged);
		this.ANALOGLLEFT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGLLEFT.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGLLEFT.Location = new System.Drawing.Point(70, 72);
		this.ANALOGLLEFT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGLLEFT.Name = "ANALOGLLEFT";
		this.ANALOGLLEFT.ShortcutsEnabled = false;
		this.ANALOGLLEFT.Size = new System.Drawing.Size(104, 22);
		this.ANALOGLLEFT.TabIndex = 2;
		this.ANALOGLLEFT.Text = "None";
		this.ANALOGLLEFT.TextChanged += new System.EventHandler(ANALOGLLEFT_TextChanged);
		this.ANALOGLDOWN.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGLDOWN.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGLDOWN.Location = new System.Drawing.Point(70, 46);
		this.ANALOGLDOWN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGLDOWN.Name = "ANALOGLDOWN";
		this.ANALOGLDOWN.ShortcutsEnabled = false;
		this.ANALOGLDOWN.Size = new System.Drawing.Size(104, 22);
		this.ANALOGLDOWN.TabIndex = 1;
		this.ANALOGLDOWN.Text = "None";
		this.ANALOGLDOWN.TextChanged += new System.EventHandler(ANALOGLDOWN_TextChanged);
		this.ANALOGLUP.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ANALOGLUP.InputKey = System.Windows.Forms.Keys.None;
		this.ANALOGLUP.Location = new System.Drawing.Point(70, 20);
		this.ANALOGLUP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ANALOGLUP.Name = "ANALOGLUP";
		this.ANALOGLUP.ShortcutsEnabled = false;
		this.ANALOGLUP.Size = new System.Drawing.Size(104, 22);
		this.ANALOGLUP.TabIndex = 0;
		this.ANALOGLUP.Text = "None";
		this.ANALOGLUP.TextChanged += new System.EventHandler(ANALOGLUP_TextChanged);
		this.ButtonCAPTURE.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonCAPTURE.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonCAPTURE.Location = new System.Drawing.Point(70, 358);
		this.ButtonCAPTURE.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonCAPTURE.Name = "ButtonCAPTURE";
		this.ButtonCAPTURE.ShortcutsEnabled = false;
		this.ButtonCAPTURE.Size = new System.Drawing.Size(104, 22);
		this.ButtonCAPTURE.TabIndex = 13;
		this.ButtonCAPTURE.Text = "None";
		this.ButtonCAPTURE.TextChanged += new System.EventHandler(ButtonCAPTURE_TextChanged);
		this.ButtonClickR.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonClickR.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonClickR.Location = new System.Drawing.Point(70, 254);
		this.ButtonClickR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonClickR.Name = "ButtonClickR";
		this.ButtonClickR.ShortcutsEnabled = false;
		this.ButtonClickR.Size = new System.Drawing.Size(104, 22);
		this.ButtonClickR.TabIndex = 9;
		this.ButtonClickR.Text = "None";
		this.ButtonClickR.TextChanged += new System.EventHandler(ButtonClickR_TextChanged);
		this.ButtonClickL.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonClickL.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonClickL.Location = new System.Drawing.Point(70, 228);
		this.ButtonClickL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonClickL.Name = "ButtonClickL";
		this.ButtonClickL.ShortcutsEnabled = false;
		this.ButtonClickL.Size = new System.Drawing.Size(104, 22);
		this.ButtonClickL.TabIndex = 8;
		this.ButtonClickL.Text = "None";
		this.ButtonClickL.TextChanged += new System.EventHandler(receiveKeyTextBox2_TextChanged);
		this.ButtonZR.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonZR.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonZR.Location = new System.Drawing.Point(70, 202);
		this.ButtonZR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonZR.Name = "ButtonZR";
		this.ButtonZR.ShortcutsEnabled = false;
		this.ButtonZR.Size = new System.Drawing.Size(104, 22);
		this.ButtonZR.TabIndex = 7;
		this.ButtonZR.Text = "None";
		this.ButtonZR.TextChanged += new System.EventHandler(ButtonZR_TextChanged);
		this.ButtonSTART.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonSTART.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonSTART.Location = new System.Drawing.Point(70, 280);
		this.ButtonSTART.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonSTART.Name = "ButtonSTART";
		this.ButtonSTART.ShortcutsEnabled = false;
		this.ButtonSTART.Size = new System.Drawing.Size(104, 22);
		this.ButtonSTART.TabIndex = 10;
		this.ButtonSTART.Text = "None";
		this.ButtonSTART.TextChanged += new System.EventHandler(ButtonSTART_TextChanged);
		this.ButtonHOME.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonHOME.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonHOME.Location = new System.Drawing.Point(70, 332);
		this.ButtonHOME.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonHOME.Name = "ButtonHOME";
		this.ButtonHOME.ShortcutsEnabled = false;
		this.ButtonHOME.Size = new System.Drawing.Size(104, 22);
		this.ButtonHOME.TabIndex = 12;
		this.ButtonHOME.Text = "None";
		this.ButtonHOME.TextChanged += new System.EventHandler(ButtonHOME_TextChanged);
		this.ButtonSELECT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonSELECT.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonSELECT.Location = new System.Drawing.Point(70, 306);
		this.ButtonSELECT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonSELECT.Name = "ButtonSELECT";
		this.ButtonSELECT.ShortcutsEnabled = false;
		this.ButtonSELECT.Size = new System.Drawing.Size(104, 22);
		this.ButtonSELECT.TabIndex = 11;
		this.ButtonSELECT.Text = "None";
		this.ButtonSELECT.TextChanged += new System.EventHandler(ButtonSELECT_TextChanged);
		this.ButtonZL.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonZL.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonZL.Location = new System.Drawing.Point(70, 176);
		this.ButtonZL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonZL.Name = "ButtonZL";
		this.ButtonZL.ShortcutsEnabled = false;
		this.ButtonZL.Size = new System.Drawing.Size(104, 22);
		this.ButtonZL.TabIndex = 6;
		this.ButtonZL.Text = "None";
		this.ButtonZL.TextChanged += new System.EventHandler(ButtonZL_TextChanged);
		this.ButtonR.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonR.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonR.Location = new System.Drawing.Point(70, 150);
		this.ButtonR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonR.Name = "ButtonR";
		this.ButtonR.ShortcutsEnabled = false;
		this.ButtonR.Size = new System.Drawing.Size(104, 22);
		this.ButtonR.TabIndex = 5;
		this.ButtonR.Text = "None";
		this.ButtonR.TextChanged += new System.EventHandler(ButtonR_TextChanged);
		this.ButtonL.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonL.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonL.Location = new System.Drawing.Point(70, 124);
		this.ButtonL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonL.Name = "ButtonL";
		this.ButtonL.ShortcutsEnabled = false;
		this.ButtonL.Size = new System.Drawing.Size(104, 22);
		this.ButtonL.TabIndex = 4;
		this.ButtonL.Text = "None";
		this.ButtonL.TextChanged += new System.EventHandler(ButtonL_TextChanged);
		this.ButtonY.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonY.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonY.Location = new System.Drawing.Point(70, 98);
		this.ButtonY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonY.Name = "ButtonY";
		this.ButtonY.ShortcutsEnabled = false;
		this.ButtonY.Size = new System.Drawing.Size(104, 22);
		this.ButtonY.TabIndex = 3;
		this.ButtonY.Text = "None";
		this.ButtonY.TextChanged += new System.EventHandler(ButtonY_TextChanged);
		this.ButtonX.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonX.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonX.Location = new System.Drawing.Point(70, 72);
		this.ButtonX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonX.Name = "ButtonX";
		this.ButtonX.ShortcutsEnabled = false;
		this.ButtonX.Size = new System.Drawing.Size(104, 22);
		this.ButtonX.TabIndex = 2;
		this.ButtonX.Text = "None";
		this.ButtonX.TextChanged += new System.EventHandler(ButtonX_TextChanged);
		this.ButtonB.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonB.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonB.Location = new System.Drawing.Point(70, 46);
		this.ButtonB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonB.Name = "ButtonB";
		this.ButtonB.ShortcutsEnabled = false;
		this.ButtonB.Size = new System.Drawing.Size(104, 22);
		this.ButtonB.TabIndex = 1;
		this.ButtonB.Text = "None";
		this.ButtonB.TextChanged += new System.EventHandler(ButtonB_TextChanged);
		this.ButtonA.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.ButtonA.InputKey = System.Windows.Forms.Keys.None;
		this.ButtonA.Location = new System.Drawing.Point(70, 20);
		this.ButtonA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ButtonA.Name = "ButtonA";
		this.ButtonA.ShortcutsEnabled = false;
		this.ButtonA.Size = new System.Drawing.Size(104, 22);
		this.ButtonA.TabIndex = 0;
		this.ButtonA.Text = "None";
		this.ButtonA.TextChanged += new System.EventHandler(ButtonA_TextChanged);
		this.DRIGHT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.DRIGHT.InputKey = System.Windows.Forms.Keys.None;
		this.DRIGHT.Location = new System.Drawing.Point(70, 98);
		this.DRIGHT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.DRIGHT.Name = "DRIGHT";
		this.DRIGHT.ShortcutsEnabled = false;
		this.DRIGHT.Size = new System.Drawing.Size(104, 22);
		this.DRIGHT.TabIndex = 3;
		this.DRIGHT.Text = "None";
		this.DRIGHT.TextChanged += new System.EventHandler(DRIGHT_TextChanged);
		this.DLEFT.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.DLEFT.InputKey = System.Windows.Forms.Keys.None;
		this.DLEFT.Location = new System.Drawing.Point(70, 72);
		this.DLEFT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.DLEFT.Name = "DLEFT";
		this.DLEFT.ShortcutsEnabled = false;
		this.DLEFT.Size = new System.Drawing.Size(104, 22);
		this.DLEFT.TabIndex = 2;
		this.DLEFT.Text = "None";
		this.DLEFT.TextChanged += new System.EventHandler(DLEFT_TextChanged);
		this.DDOWN.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.DDOWN.InputKey = System.Windows.Forms.Keys.None;
		this.DDOWN.Location = new System.Drawing.Point(70, 46);
		this.DDOWN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.DDOWN.Name = "DDOWN";
		this.DDOWN.ShortcutsEnabled = false;
		this.DDOWN.Size = new System.Drawing.Size(104, 22);
		this.DDOWN.TabIndex = 1;
		this.DDOWN.Text = "None";
		this.DDOWN.TextChanged += new System.EventHandler(DDOWN_TextChanged);
		this.DUP.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.DUP.InputKey = System.Windows.Forms.Keys.None;
		this.DUP.Location = new System.Drawing.Point(70, 20);
		this.DUP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.DUP.Name = "DUP";
		this.DUP.ShortcutsEnabled = false;
		this.DUP.Size = new System.Drawing.Size(104, 22);
		this.DUP.TabIndex = 0;
		this.DUP.Text = "None";
		this.DUP.TextChanged += new System.EventHandler(DUP_TextChanged);
		this.Dx26.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx26.IsDirectX = true;
		this.Dx26.Location = new System.Drawing.Point(70, 98);
		this.Dx26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx26.Name = "Dx26";
		this.Dx26.ShortcutsEnabled = false;
		this.Dx26.Size = new System.Drawing.Size(104, 22);
		this.Dx26.TabIndex = 3;
		this.Dx26.Text = "None";
		this.Dx26.TextChanged += new System.EventHandler(Dx26_TextChanged);
		this.Dx25.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx25.IsDirectX = true;
		this.Dx25.Location = new System.Drawing.Point(70, 72);
		this.Dx25.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx25.Name = "Dx25";
		this.Dx25.ShortcutsEnabled = false;
		this.Dx25.Size = new System.Drawing.Size(104, 22);
		this.Dx25.TabIndex = 2;
		this.Dx25.Text = "None";
		this.Dx25.TextChanged += new System.EventHandler(Dx25_TextChanged);
		this.Dx24.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx24.IsDirectX = true;
		this.Dx24.Location = new System.Drawing.Point(70, 46);
		this.Dx24.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx24.Name = "Dx24";
		this.Dx24.ShortcutsEnabled = false;
		this.Dx24.Size = new System.Drawing.Size(104, 22);
		this.Dx24.TabIndex = 1;
		this.Dx24.Text = "None";
		this.Dx24.TextChanged += new System.EventHandler(Dx24_TextChanged);
		this.Dx23.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx23.IsDirectX = true;
		this.Dx23.Location = new System.Drawing.Point(70, 20);
		this.Dx23.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx23.Name = "Dx23";
		this.Dx23.ShortcutsEnabled = false;
		this.Dx23.Size = new System.Drawing.Size(104, 22);
		this.Dx23.TabIndex = 0;
		this.Dx23.Text = "None";
		this.Dx23.TextChanged += new System.EventHandler(Dx23_TextChanged);
		this.Dx22.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx22.IsDirectX = true;
		this.Dx22.Location = new System.Drawing.Point(70, 98);
		this.Dx22.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx22.Name = "Dx22";
		this.Dx22.ShortcutsEnabled = false;
		this.Dx22.Size = new System.Drawing.Size(104, 22);
		this.Dx22.TabIndex = 3;
		this.Dx22.Text = "None";
		this.Dx22.TextChanged += new System.EventHandler(Dx22_TextChanged);
		this.Dx21.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx21.IsDirectX = true;
		this.Dx21.Location = new System.Drawing.Point(70, 72);
		this.Dx21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx21.Name = "Dx21";
		this.Dx21.ShortcutsEnabled = false;
		this.Dx21.Size = new System.Drawing.Size(104, 22);
		this.Dx21.TabIndex = 2;
		this.Dx21.Text = "None";
		this.Dx21.TextChanged += new System.EventHandler(Dx21_TextChanged);
		this.Dx20.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx20.IsDirectX = true;
		this.Dx20.Location = new System.Drawing.Point(70, 46);
		this.Dx20.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx20.Name = "Dx20";
		this.Dx20.ShortcutsEnabled = false;
		this.Dx20.Size = new System.Drawing.Size(104, 22);
		this.Dx20.TabIndex = 1;
		this.Dx20.Text = "None";
		this.Dx20.TextChanged += new System.EventHandler(Dx20_TextChanged);
		this.Dx19.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx19.IsDirectX = true;
		this.Dx19.Location = new System.Drawing.Point(70, 20);
		this.Dx19.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx19.Name = "Dx19";
		this.Dx19.ShortcutsEnabled = false;
		this.Dx19.Size = new System.Drawing.Size(104, 22);
		this.Dx19.TabIndex = 0;
		this.Dx19.Text = "None";
		this.Dx19.TextChanged += new System.EventHandler(Dx19_TextChanged);
		this.Dx14.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx14.IsDirectX = true;
		this.Dx14.Location = new System.Drawing.Point(70, 358);
		this.Dx14.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx14.Name = "Dx14";
		this.Dx14.ShortcutsEnabled = false;
		this.Dx14.Size = new System.Drawing.Size(104, 22);
		this.Dx14.TabIndex = 13;
		this.Dx14.Text = "None";
		this.Dx14.TextChanged += new System.EventHandler(Dx14_TextChanged);
		this.Dx10.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx10.IsDirectX = true;
		this.Dx10.Location = new System.Drawing.Point(70, 254);
		this.Dx10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx10.Name = "Dx10";
		this.Dx10.ShortcutsEnabled = false;
		this.Dx10.Size = new System.Drawing.Size(104, 22);
		this.Dx10.TabIndex = 9;
		this.Dx10.Text = "None";
		this.Dx10.TextChanged += new System.EventHandler(Dx10_TextChanged);
		this.Dx9.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx9.IsDirectX = true;
		this.Dx9.Location = new System.Drawing.Point(70, 228);
		this.Dx9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx9.Name = "Dx9";
		this.Dx9.ShortcutsEnabled = false;
		this.Dx9.Size = new System.Drawing.Size(104, 22);
		this.Dx9.TabIndex = 8;
		this.Dx9.Text = "None";
		this.Dx9.TextChanged += new System.EventHandler(Dx9_TextChanged);
		this.Dx8.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx8.IsDirectX = true;
		this.Dx8.Location = new System.Drawing.Point(70, 202);
		this.Dx8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx8.Name = "Dx8";
		this.Dx8.ShortcutsEnabled = false;
		this.Dx8.Size = new System.Drawing.Size(104, 22);
		this.Dx8.TabIndex = 7;
		this.Dx8.Text = "None";
		this.Dx8.TextChanged += new System.EventHandler(Dx8_TextChanged);
		this.Dx11.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx11.IsDirectX = true;
		this.Dx11.Location = new System.Drawing.Point(70, 280);
		this.Dx11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx11.Name = "Dx11";
		this.Dx11.ShortcutsEnabled = false;
		this.Dx11.Size = new System.Drawing.Size(104, 22);
		this.Dx11.TabIndex = 10;
		this.Dx11.Text = "None";
		this.Dx11.TextChanged += new System.EventHandler(Dx11_TextChanged);
		this.Dx13.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx13.IsDirectX = true;
		this.Dx13.Location = new System.Drawing.Point(70, 332);
		this.Dx13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx13.Name = "Dx13";
		this.Dx13.ShortcutsEnabled = false;
		this.Dx13.Size = new System.Drawing.Size(104, 22);
		this.Dx13.TabIndex = 12;
		this.Dx13.Text = "None";
		this.Dx13.TextChanged += new System.EventHandler(Dx13_TextChanged);
		this.Dx12.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx12.IsDirectX = true;
		this.Dx12.Location = new System.Drawing.Point(70, 306);
		this.Dx12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx12.Name = "Dx12";
		this.Dx12.ShortcutsEnabled = false;
		this.Dx12.Size = new System.Drawing.Size(104, 22);
		this.Dx12.TabIndex = 11;
		this.Dx12.Text = "None";
		this.Dx12.TextChanged += new System.EventHandler(Dx12_TextChanged);
		this.Dx7.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx7.IsDirectX = true;
		this.Dx7.Location = new System.Drawing.Point(70, 176);
		this.Dx7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx7.Name = "Dx7";
		this.Dx7.ShortcutsEnabled = false;
		this.Dx7.Size = new System.Drawing.Size(104, 22);
		this.Dx7.TabIndex = 6;
		this.Dx7.Text = "None";
		this.Dx7.TextChanged += new System.EventHandler(Dx7_TextChanged);
		this.Dx6.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx6.IsDirectX = true;
		this.Dx6.Location = new System.Drawing.Point(70, 150);
		this.Dx6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx6.Name = "Dx6";
		this.Dx6.ShortcutsEnabled = false;
		this.Dx6.Size = new System.Drawing.Size(104, 22);
		this.Dx6.TabIndex = 5;
		this.Dx6.Text = "None";
		this.Dx6.TextChanged += new System.EventHandler(Dx6_TextChanged);
		this.Dx5.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx5.IsDirectX = true;
		this.Dx5.Location = new System.Drawing.Point(70, 124);
		this.Dx5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx5.Name = "Dx5";
		this.Dx5.ShortcutsEnabled = false;
		this.Dx5.Size = new System.Drawing.Size(104, 22);
		this.Dx5.TabIndex = 4;
		this.Dx5.Text = "None";
		this.Dx5.TextChanged += new System.EventHandler(Dx5_TextChanged);
		this.Dx4.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx4.IsDirectX = true;
		this.Dx4.Location = new System.Drawing.Point(70, 98);
		this.Dx4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx4.Name = "Dx4";
		this.Dx4.ShortcutsEnabled = false;
		this.Dx4.Size = new System.Drawing.Size(104, 22);
		this.Dx4.TabIndex = 3;
		this.Dx4.Text = "None";
		this.Dx4.TextChanged += new System.EventHandler(Dx4_TextChanged);
		this.Dx3.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx3.IsDirectX = true;
		this.Dx3.Location = new System.Drawing.Point(70, 72);
		this.Dx3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx3.Name = "Dx3";
		this.Dx3.ShortcutsEnabled = false;
		this.Dx3.Size = new System.Drawing.Size(104, 22);
		this.Dx3.TabIndex = 2;
		this.Dx3.Text = "None";
		this.Dx3.TextChanged += new System.EventHandler(Dx3_TextChanged);
		this.Dx2.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx2.IsDirectX = true;
		this.Dx2.Location = new System.Drawing.Point(70, 46);
		this.Dx2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx2.Name = "Dx2";
		this.Dx2.ShortcutsEnabled = false;
		this.Dx2.Size = new System.Drawing.Size(104, 22);
		this.Dx2.TabIndex = 1;
		this.Dx2.Text = "None";
		this.Dx2.TextChanged += new System.EventHandler(Dx2_TextChanged);
		this.Dx1.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx1.IsDirectX = true;
		this.Dx1.Location = new System.Drawing.Point(70, 20);
		this.Dx1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx1.Name = "Dx1";
		this.Dx1.ShortcutsEnabled = false;
		this.Dx1.Size = new System.Drawing.Size(104, 22);
		this.Dx1.TabIndex = 0;
		this.Dx1.Text = "None";
		this.Dx1.TextChanged += new System.EventHandler(receiveKeyTextBox22_TextChanged);
		this.Dx18.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx18.IsDirectX = true;
		this.Dx18.Location = new System.Drawing.Point(70, 98);
		this.Dx18.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx18.Name = "Dx18";
		this.Dx18.ShortcutsEnabled = false;
		this.Dx18.Size = new System.Drawing.Size(104, 22);
		this.Dx18.TabIndex = 3;
		this.Dx18.Text = "None";
		this.Dx18.TextChanged += new System.EventHandler(Dx18_TextChanged);
		this.Dx17.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx17.IsDirectX = true;
		this.Dx17.Location = new System.Drawing.Point(70, 72);
		this.Dx17.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx17.Name = "Dx17";
		this.Dx17.ShortcutsEnabled = false;
		this.Dx17.Size = new System.Drawing.Size(104, 22);
		this.Dx17.TabIndex = 2;
		this.Dx17.Text = "None";
		this.Dx17.TextChanged += new System.EventHandler(Dx17_TextChanged);
		this.Dx16.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx16.IsDirectX = true;
		this.Dx16.Location = new System.Drawing.Point(70, 46);
		this.Dx16.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx16.Name = "Dx16";
		this.Dx16.ShortcutsEnabled = false;
		this.Dx16.Size = new System.Drawing.Size(104, 22);
		this.Dx16.TabIndex = 1;
		this.Dx16.Text = "None";
		this.Dx16.TextChanged += new System.EventHandler(Dx16_TextChanged);
		this.Dx15.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.Dx15.IsDirectX = true;
		this.Dx15.Location = new System.Drawing.Point(70, 20);
		this.Dx15.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.Dx15.Name = "Dx15";
		this.Dx15.ShortcutsEnabled = false;
		this.Dx15.Size = new System.Drawing.Size(104, 22);
		this.Dx15.TabIndex = 0;
		this.Dx15.Text = "None";
		this.Dx15.TextChanged += new System.EventHandler(Dx15_TextChanged);
		this.XI26.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI26.IsDirectX = false;
		this.XI26.Location = new System.Drawing.Point(70, 98);
		this.XI26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI26.Name = "XI26";
		this.XI26.ShortcutsEnabled = false;
		this.XI26.Size = new System.Drawing.Size(104, 22);
		this.XI26.TabIndex = 3;
		this.XI26.Text = "None";
		this.XI26.TextChanged += new System.EventHandler(XI26_TextChanged);
		this.XI25.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI25.IsDirectX = false;
		this.XI25.Location = new System.Drawing.Point(70, 72);
		this.XI25.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI25.Name = "XI25";
		this.XI25.ShortcutsEnabled = false;
		this.XI25.Size = new System.Drawing.Size(104, 22);
		this.XI25.TabIndex = 2;
		this.XI25.Text = "None";
		this.XI25.TextChanged += new System.EventHandler(XI25_TextChanged);
		this.XI24.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI24.IsDirectX = false;
		this.XI24.Location = new System.Drawing.Point(70, 46);
		this.XI24.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI24.Name = "XI24";
		this.XI24.ShortcutsEnabled = false;
		this.XI24.Size = new System.Drawing.Size(104, 22);
		this.XI24.TabIndex = 1;
		this.XI24.Text = "None";
		this.XI24.TextChanged += new System.EventHandler(XI24_TextChanged);
		this.XI23.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI23.IsDirectX = false;
		this.XI23.Location = new System.Drawing.Point(70, 20);
		this.XI23.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI23.Name = "XI23";
		this.XI23.ShortcutsEnabled = false;
		this.XI23.Size = new System.Drawing.Size(104, 22);
		this.XI23.TabIndex = 0;
		this.XI23.Text = "None";
		this.XI23.TextChanged += new System.EventHandler(XI23_TextChanged);
		this.XI22.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI22.IsDirectX = false;
		this.XI22.Location = new System.Drawing.Point(70, 98);
		this.XI22.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI22.Name = "XI22";
		this.XI22.ShortcutsEnabled = false;
		this.XI22.Size = new System.Drawing.Size(104, 22);
		this.XI22.TabIndex = 3;
		this.XI22.Text = "None";
		this.XI22.TextChanged += new System.EventHandler(XI22_TextChanged);
		this.XI21.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI21.IsDirectX = false;
		this.XI21.Location = new System.Drawing.Point(70, 72);
		this.XI21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI21.Name = "XI21";
		this.XI21.ShortcutsEnabled = false;
		this.XI21.Size = new System.Drawing.Size(104, 22);
		this.XI21.TabIndex = 2;
		this.XI21.Text = "None";
		this.XI21.TextChanged += new System.EventHandler(XI21_TextChanged);
		this.XI20.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI20.IsDirectX = false;
		this.XI20.Location = new System.Drawing.Point(70, 46);
		this.XI20.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI20.Name = "XI20";
		this.XI20.ShortcutsEnabled = false;
		this.XI20.Size = new System.Drawing.Size(104, 22);
		this.XI20.TabIndex = 1;
		this.XI20.Text = "None";
		this.XI20.TextChanged += new System.EventHandler(XI20_TextChanged);
		this.XI19.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI19.IsDirectX = false;
		this.XI19.Location = new System.Drawing.Point(70, 20);
		this.XI19.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI19.Name = "XI19";
		this.XI19.ShortcutsEnabled = false;
		this.XI19.Size = new System.Drawing.Size(104, 22);
		this.XI19.TabIndex = 0;
		this.XI19.Text = "None";
		this.XI19.TextChanged += new System.EventHandler(XI19_TextChanged);
		this.XI14.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI14.IsDirectX = false;
		this.XI14.Location = new System.Drawing.Point(70, 358);
		this.XI14.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI14.Name = "XI14";
		this.XI14.ShortcutsEnabled = false;
		this.XI14.Size = new System.Drawing.Size(104, 22);
		this.XI14.TabIndex = 13;
		this.XI14.Text = "None";
		this.XI14.TextChanged += new System.EventHandler(XI14_TextChanged);
		this.XI10.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI10.IsDirectX = false;
		this.XI10.Location = new System.Drawing.Point(70, 254);
		this.XI10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI10.Name = "XI10";
		this.XI10.ShortcutsEnabled = false;
		this.XI10.Size = new System.Drawing.Size(104, 22);
		this.XI10.TabIndex = 9;
		this.XI10.Text = "None";
		this.XI10.TextChanged += new System.EventHandler(XI10_TextChanged);
		this.XI9.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI9.IsDirectX = false;
		this.XI9.Location = new System.Drawing.Point(70, 228);
		this.XI9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI9.Name = "XI9";
		this.XI9.ShortcutsEnabled = false;
		this.XI9.Size = new System.Drawing.Size(104, 22);
		this.XI9.TabIndex = 8;
		this.XI9.Text = "None";
		this.XI9.TextChanged += new System.EventHandler(XI9_TextChanged);
		this.XI8.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI8.IsDirectX = false;
		this.XI8.Location = new System.Drawing.Point(70, 202);
		this.XI8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI8.Name = "XI8";
		this.XI8.ShortcutsEnabled = false;
		this.XI8.Size = new System.Drawing.Size(104, 22);
		this.XI8.TabIndex = 7;
		this.XI8.Text = "None";
		this.XI8.TextChanged += new System.EventHandler(XI8_TextChanged);
		this.XI11.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI11.IsDirectX = false;
		this.XI11.Location = new System.Drawing.Point(70, 280);
		this.XI11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI11.Name = "XI11";
		this.XI11.ShortcutsEnabled = false;
		this.XI11.Size = new System.Drawing.Size(104, 22);
		this.XI11.TabIndex = 10;
		this.XI11.Text = "None";
		this.XI11.TextChanged += new System.EventHandler(XI11_TextChanged);
		this.XI13.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI13.IsDirectX = false;
		this.XI13.Location = new System.Drawing.Point(70, 332);
		this.XI13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI13.Name = "XI13";
		this.XI13.ShortcutsEnabled = false;
		this.XI13.Size = new System.Drawing.Size(104, 22);
		this.XI13.TabIndex = 12;
		this.XI13.Text = "None";
		this.XI13.TextChanged += new System.EventHandler(XI13_TextChanged);
		this.XI12.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI12.IsDirectX = false;
		this.XI12.Location = new System.Drawing.Point(70, 306);
		this.XI12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI12.Name = "XI12";
		this.XI12.ShortcutsEnabled = false;
		this.XI12.Size = new System.Drawing.Size(104, 22);
		this.XI12.TabIndex = 11;
		this.XI12.Text = "None";
		this.XI12.TextChanged += new System.EventHandler(XI12_TextChanged);
		this.XI7.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI7.IsDirectX = false;
		this.XI7.Location = new System.Drawing.Point(70, 176);
		this.XI7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI7.Name = "XI7";
		this.XI7.ShortcutsEnabled = false;
		this.XI7.Size = new System.Drawing.Size(104, 22);
		this.XI7.TabIndex = 6;
		this.XI7.Text = "None";
		this.XI7.TextChanged += new System.EventHandler(XI7_TextChanged);
		this.XI6.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI6.IsDirectX = false;
		this.XI6.Location = new System.Drawing.Point(70, 150);
		this.XI6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI6.Name = "XI6";
		this.XI6.ShortcutsEnabled = false;
		this.XI6.Size = new System.Drawing.Size(104, 22);
		this.XI6.TabIndex = 5;
		this.XI6.Text = "None";
		this.XI6.TextChanged += new System.EventHandler(XI6_TextChanged);
		this.XI5.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI5.IsDirectX = false;
		this.XI5.Location = new System.Drawing.Point(70, 124);
		this.XI5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI5.Name = "XI5";
		this.XI5.ShortcutsEnabled = false;
		this.XI5.Size = new System.Drawing.Size(104, 22);
		this.XI5.TabIndex = 4;
		this.XI5.Text = "None";
		this.XI5.TextChanged += new System.EventHandler(XI5_TextChanged);
		this.XI4.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI4.IsDirectX = false;
		this.XI4.Location = new System.Drawing.Point(70, 98);
		this.XI4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI4.Name = "XI4";
		this.XI4.ShortcutsEnabled = false;
		this.XI4.Size = new System.Drawing.Size(104, 22);
		this.XI4.TabIndex = 3;
		this.XI4.Text = "None";
		this.XI4.TextChanged += new System.EventHandler(XI4_TextChanged);
		this.XI3.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI3.IsDirectX = false;
		this.XI3.Location = new System.Drawing.Point(70, 72);
		this.XI3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI3.Name = "XI3";
		this.XI3.ShortcutsEnabled = false;
		this.XI3.Size = new System.Drawing.Size(104, 22);
		this.XI3.TabIndex = 2;
		this.XI3.Text = "None";
		this.XI3.TextChanged += new System.EventHandler(XI3_TextChanged);
		this.XI2.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI2.IsDirectX = false;
		this.XI2.Location = new System.Drawing.Point(70, 46);
		this.XI2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI2.Name = "XI2";
		this.XI2.ShortcutsEnabled = false;
		this.XI2.Size = new System.Drawing.Size(104, 22);
		this.XI2.TabIndex = 1;
		this.XI2.Text = "None";
		this.XI2.TextChanged += new System.EventHandler(XI2_TextChanged);
		this.XI1.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI1.IsDirectX = false;
		this.XI1.Location = new System.Drawing.Point(70, 20);
		this.XI1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI1.Name = "XI1";
		this.XI1.ShortcutsEnabled = false;
		this.XI1.Size = new System.Drawing.Size(104, 22);
		this.XI1.TabIndex = 0;
		this.XI1.Text = "None";
		this.XI1.TextChanged += new System.EventHandler(XI1_TextChanged);
		this.XI18.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI18.IsDirectX = false;
		this.XI18.Location = new System.Drawing.Point(70, 98);
		this.XI18.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI18.Name = "XI18";
		this.XI18.ShortcutsEnabled = false;
		this.XI18.Size = new System.Drawing.Size(104, 22);
		this.XI18.TabIndex = 3;
		this.XI18.Text = "None";
		this.XI18.TextChanged += new System.EventHandler(XI18_TextChanged);
		this.XI17.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI17.IsDirectX = false;
		this.XI17.Location = new System.Drawing.Point(70, 72);
		this.XI17.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI17.Name = "XI17";
		this.XI17.ShortcutsEnabled = false;
		this.XI17.Size = new System.Drawing.Size(104, 22);
		this.XI17.TabIndex = 2;
		this.XI17.Text = "None";
		this.XI17.TextChanged += new System.EventHandler(XI17_TextChanged);
		this.XI16.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI16.IsDirectX = false;
		this.XI16.Location = new System.Drawing.Point(70, 46);
		this.XI16.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI16.Name = "XI16";
		this.XI16.ShortcutsEnabled = false;
		this.XI16.Size = new System.Drawing.Size(104, 22);
		this.XI16.TabIndex = 1;
		this.XI16.Text = "None";
		this.XI16.TextChanged += new System.EventHandler(XI16_TextChanged);
		this.XI15.ImeMode = System.Windows.Forms.ImeMode.Disable;
		this.XI15.IsDirectX = false;
		this.XI15.Location = new System.Drawing.Point(70, 20);
		this.XI15.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.XI15.Name = "XI15";
		this.XI15.ShortcutsEnabled = false;
		this.XI15.Size = new System.Drawing.Size(104, 22);
		this.XI15.TabIndex = 0;
		this.XI15.Text = "None";
		this.XI15.TextChanged += new System.EventHandler(XI15_TextChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(591, 463);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.tabControl1);
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Name = "SettingDialog";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		this.Text = "オプション";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SettingDialog_FormClosed);
		base.Load += new System.EventHandler(SettingDialog_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		this.groupBox9.ResumeLayout(false);
		this.groupBox9.PerformLayout();
		this.groupBox10.ResumeLayout(false);
		this.groupBox10.PerformLayout();
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.groupBox12.ResumeLayout(false);
		this.groupBox12.PerformLayout();
		this.groupBox13.ResumeLayout(false);
		this.groupBox13.PerformLayout();
		this.groupBox14.ResumeLayout(false);
		this.groupBox14.PerformLayout();
		this.groupBox15.ResumeLayout(false);
		this.groupBox15.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		base.ResumeLayout(false);
	}
}

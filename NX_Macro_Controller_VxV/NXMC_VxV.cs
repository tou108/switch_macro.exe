using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Xml;
using BZComponent;
using CustomScrollBar;
using DirectShowLib;
using DiscordRPC;
using HongliangSoft.Utilities;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using IronPython.Hosting;
using NX_Macro_Controller_VxV.Properties;
using NxInterface;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PSTaskDialog;
using RJCP.IO.Ports;

namespace NX_Macro_Controller_VxV;

public class NXMC_VxV : FormEx
{
	public struct nxSelection
	{
		public int X1;

		public int X2;

		public int Y1;

		public int Y2;

		public int PicW;

		public int PicH;

		public double PicD;

		public bool Start;
	}

	public enum CaptureStyle
	{
		None,
		DirectShow,
		OpenCV
	}

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

	private System.Drawing.Image _capturedImage = new Bitmap(10, 10);

	public DiscordRpcClient DiscordRpcClient;

	public TextEditor CodeEdit;

	public System.Windows.Controls.ListBox LsBox;

	private Matsub _popUpWindow;

	public nxSelection NxSel;

	public NMC Nmc = new NMC();

	private KeyMessageFilter _mFilter = new KeyMessageFilter();

	private string _selectedPort = "";

	public bool _captureNow;

	private int _captureMode;

	private SerialPortStream _serialPort = new SerialPortStream();

	public bool KeyRecoding;

	private bool _vScrollF;

	private string macroSelCmbText = "";

	private string _amiibo = "";

	private Stopwatch _lastTaskView = new Stopwatch();

	private Stopwatch _lastHighlight = new Stopwatch();

	private string[] _portsBuffer = SerialPort.GetPortNames();

	public DSHDMICapture DsCapture;

	public VideoCapture CvCapture;

	public CaptureStyle CurrentCaptureFormat;

	private KeyboardHook kbh = new KeyboardHook();

	private bool captureRun;

	public string CurrentDirectory = "";

	public string MacroDirectory = "";

	private Bitmap captureScreenBuffer;

	private Process _pokeconProcess;

	private bool _pokeconRnnning;

	private const uint MSGFLT_ALLOW = 1u;

	private const uint WM_DROPFILES = 563u;

	private const uint WM_COPYDATA = 74u;

	private const uint WM_COPYGLOBALDATA = 73u;

	private CompletionWindow completionWindow;

	private string[][] commandlist = (from _ in new string[77][]
		{
			new string[2] { "Press", "Command" },
			new string[2] { "Hold", "Command" },
			new string[2] { "HoldRelease", "Command" },
			new string[2] { "Continue", "NArgsCommand" },
			new string[2] { "Break", "NArgsCommand" },
			new string[2] { "Count", "NArgsCommand" },
			new string[2] { "Call", "Command" },
			new string[2] { "Wait", "Command" },
			new string[2] { "Loop", "Block" },
			new string[2] { "ImgCmp", "Block" },
			new string[2] { "Snipping", "Command" },
			new string[2] { "Stop", "NArgsCommand" },
			new string[2] { "Not", "NArgsBlock" },
			new string[2] { "Notification", "Command" },
			new string[2] { "LineNotifyWithImage", "Command" },
			new string[2] { "LineNotify", "Command" },
			new string[2] { "Amiibo", "Command" },
			new string[2] { "Func", "Block" },
			new string[2] { "Exec", "Command" },
			new string[2] { "Rumble", "Block" },
			new string[2] { "A", "key" },
			new string[2] { "B", "Key" },
			new string[2] { "X", "Key" },
			new string[2] { "Y", "Key" },
			new string[2] { "L", "Key" },
			new string[2] { "R", "Key" },
			new string[2] { "ZL", "Key" },
			new string[2] { "ZR", "Key" },
			new string[2] { "START", "Key" },
			new string[2] { "SELECT", "Key" },
			new string[2] { "HOME", "Key" },
			new string[2] { "CAPTURE", "Key" },
			new string[2] { "UP", "Key" },
			new string[2] { "DOWN", "Key" },
			new string[2] { "RIGHT", "Key" },
			new string[2] { "LEFT", "Key" },
			new string[2] { "UPRIGHT", "Key" },
			new string[2] { "UPLEFT", "Key" },
			new string[2] { "DOWNRIGHT", "Key" },
			new string[2] { "DOWNLEFT", "Key" },
			new string[2] { "UP_L", "Key" },
			new string[2] { "DOWN_L", "Key" },
			new string[2] { "RIGHT_L", "Key" },
			new string[2] { "LEFT_L", "Key" },
			new string[2] { "UPRIGHT_L", "Key" },
			new string[2] { "UPLEFT_L", "Key" },
			new string[2] { "DOWNRIGHT_L", "Key" },
			new string[2] { "DOWNLEFT_L", "Key" },
			new string[2] { "UP_R", "Key" },
			new string[2] { "DOWN_R", "Key" },
			new string[2] { "RIGHT_R", "Key" },
			new string[2] { "LEFT_R", "Key" },
			new string[2] { "UPRIGHT_R", "Key" },
			new string[2] { "UPLEFT_R", "Key" },
			new string[2] { "DOWNRIGHT_R", "Key" },
			new string[2] { "DOWNLEFT_R", "Key" },
			new string[2] { "CLICK_R", "Key" },
			new string[2] { "CLICK_L", "Key" },
			new string[2] { "HIRAGANA", "Key" },
			new string[2] { "KATAKANA", "Key" },
			new string[2] { "ALPHANUMERIC", "Key" },
			new string[2] { "Keyboard", "Command" },
			new string[2] { "KeyboardMode", "Command" },
			new string[2] { "If", "Block" },
			new string[2] { "Else", "NArgsBlock" },
			new string[2] { "ElseIf", "Block" },
			new string[2] { "Var", "NArgsCommand" },
			new string[2] { "CallCsx", "Command" },
			new string[2] { "Print", "Command" },
			new string[2] { "ImgCmp720p", "Block" },
			new string[2] { "ImgCmpRect", "Block" },
			new string[2] { "ImgCmpRect720p", "Block" },
			new string[2] { "ImgCmpGray", "Block" },
			new string[2] { "ImgCmpGray720p", "Block" },
			new string[2] { "ImgCmpRectGray", "Block" },
			new string[2] { "ImgCmpRectGray720p", "Block" },
			new string[2] { "While", "Block" }
		}.ToList()
		orderby _[0].Length
		select _).ToArray();

	private MultiTextWriter mtw;

	private bool isPressCtrl;

	private int[] CRC_TABLE = new int[256]
	{
		0, 7, 14, 9, 28, 27, 18, 21, 56, 63,
		54, 49, 36, 35, 42, 45, 112, 119, 126, 121,
		108, 107, 98, 101, 72, 79, 70, 65, 84, 83,
		90, 93, 224, 231, 238, 233, 252, 251, 242, 245,
		216, 223, 214, 209, 196, 195, 202, 205, 144, 151,
		158, 153, 140, 139, 130, 133, 168, 175, 166, 161,
		180, 179, 186, 189, 199, 192, 201, 206, 219, 220,
		213, 210, 255, 248, 241, 246, 227, 228, 237, 234,
		183, 176, 185, 190, 171, 172, 165, 162, 143, 136,
		129, 134, 147, 148, 157, 154, 39, 32, 41, 46,
		59, 60, 53, 50, 31, 24, 17, 22, 3, 4,
		13, 10, 87, 80, 89, 94, 75, 76, 69, 66,
		111, 104, 97, 102, 115, 116, 125, 122, 137, 142,
		135, 128, 149, 146, 155, 156, 177, 182, 191, 184,
		173, 170, 163, 164, 249, 254, 247, 240, 229, 226,
		235, 236, 193, 198, 207, 200, 221, 218, 211, 212,
		105, 110, 103, 96, 117, 114, 123, 124, 81, 86,
		95, 88, 77, 74, 67, 68, 25, 30, 23, 16,
		5, 2, 11, 12, 33, 38, 47, 40, 61, 58,
		51, 52, 78, 73, 64, 71, 82, 85, 92, 91,
		118, 113, 120, 127, 106, 109, 100, 99, 62, 57,
		48, 55, 34, 37, 44, 43, 6, 1, 8, 15,
		26, 29, 20, 19, 174, 169, 160, 167, 178, 181,
		188, 187, 150, 145, 152, 159, 138, 141, 132, 131,
		222, 217, 208, 215, 194, 197, 204, 203, 230, 225,
		232, 239, 250, 253, 244, 243
	};

	private int _highLightLine = -1;

	private bool macroDataChanged;

	private List<string> pokeconScriptFiles = new List<string>();

	private IContainer components;

	private PictureBox CaptureScreen;

	private BackgroundWorker CaptureBGW;

	private GroupBoxEx groupBox1;

	private ButtonEx CapConnect;

	private ComboBoxEx CapDeviceList;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem ファイルToolStripMenuItem;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private ElementHost elementHost1;

	private System.Windows.Forms.Panel panel1;

	private System.Windows.Forms.ToolTip toolTip1;

	private ButtonEx button3;

	private ButtonEx button4;

	private ButtonEx button6;

	private ToolStripMenuItem マクロの読み込みToolStripMenuItem;

	private ToolStripMenuItem マクロの保存ToolStripMenuItem;

	private ToolStripSeparator toolStripMenuItem1;

	private ToolStripMenuItem 終了ToolStripMenuItem;

	private ToolStripMenuItem 設定ToolStripMenuItem;

	private TabControlEx tabControl1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private ToolStripMenuItem 接続ToolStripMenuItem;

	private FlowLayoutPanel flowLayoutPanel1;

	private ToolStripMenuItem BTSetUpToolStripMenuItem;

	private ToolStripMenuItem 環境設定ToolStripMenuItem1;

	private ContextMenuStrip CaptureContext;

	private ToolStripMenuItem 画面をキャプチャToolStripMenuItem;

	private ToolStripMenuItem バージョン情報ToolStripMenuItem;

	private System.Windows.Forms.Label label1;

	private System.Windows.Forms.Panel panel2;

	private System.Windows.Forms.Panel panel3;

	private ButtonEx buttonEx1;

	private ScrollBarEx vScrollBar1;

	private ScrollBarEx hScrollBar1;

	private System.Windows.Forms.Label label2;

	private KeyboardHook keyboardHook1;

	private ToolStripMenuItem 共有ToolStripMenuItem;

	private ToolStripMenuItem マクロ共有サーバーに接続ToolStripMenuItem;

	private System.Windows.Forms.Timer timer1;

	private TabPage tabPage1;

	private FlowLayoutPanel flowLayoutPanel2;

	private ToolStripMenuItem readmeToolStripMenuItem;

	private ToolStripMenuItem 全画面キャプチャToolStripMenuItem;

	private ToolStripMenuItem ヘルプToolStripMenuItem;

	private ToolStripSeparator toolStripMenuItem3;

	private ToolStripMenuItem amiiboの読み込みToolStripMenuItem;

	private ToolStripSeparator toolStripMenuItem5;

	private System.Windows.Forms.Panel panel4;

	private GhostPanel ghostPanel4;

	private Splitter splitter1;

	private TabPage tabPage4;

	private GhostPanel ghostPanel6;

	private MouseHook mouseHook1;

	private System.Windows.Forms.Button button1;

	private System.Windows.Forms.TextBox textBox1;

	private GroupBoxEx groupBoxEx1;

	private ScrollBarEx scrollBarEx1;

	private System.Windows.Forms.Button button2;

	private ButtonEx ComConnect;

	private ComboBoxEx macroSelCmb;

	private GhostPanel ghostPanel5;

	private ButtonEx buttonEx2;

	private System.Windows.Forms.Label label3;

	private ComboBoxEx ComPortList;

	private ButtonEx buttonEx3;

	private ToolStripMenuItem マクロを上書き保存ToolStripMenuItem;

	private TabControlEx tabControlEx1;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private ScrollBarEx scrollBarEx2;

	private ScrollBarEx scrollBarEx3;

	private ScrollBarEx scrollBarEx4;

	private FlowLayoutPanel flowLayoutPanel3;

	private ComboBoxEx macroSubDirCmb;

	private System.Windows.Forms.Label label4;

	private FileSystemWatcher fileSystemWatcher1;

	private FileSystemWatcher fileSystemWatcher2;

	private FileSystemWatcher fileSystemWatcher3;

	private GhostPanel ghostPanel7;

	private ButtonEx buttonEx4;

	private ToolStripMenuItem cH552SERIALセットアップToolStripMenuItem;

	private ToolStripMenuItem cH552へ書き込みToolStripMenuItem;

	private ToolStripMenuItem マクロの新規作成ToolStripMenuItem;

	private ButtonEx buttonEx5;

	private ToolStripMenuItem discordサーバーToolStripMenuItem;

	private FileSystemWatcher fileSystemWatcher4;

	private FileSystemWatcher fileSystemWatcher5;

	[DllImport("user32", SetLastError = true)]
	private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, uint action, IntPtr unused);

	public NXMC_VxV()
	{
		InitializeComponent();
		ChangeWindowMessageFilterEx(base.Handle, 563u, 1u, (IntPtr)0);
		ChangeWindowMessageFilterEx(base.Handle, 74u, 1u, (IntPtr)0);
		ChangeWindowMessageFilterEx(base.Handle, 73u, 1u, (IntPtr)0);
		if (base.DesignMode)
		{
			return;
		}
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = "regsvr32.exe";
			process.StartInfo.Arguments = "/s \"" + Path.GetFullPath(GlobalVar.BasePath + "NX2VCam.dll") + "\"";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Verb = "RunAs";
			process.Start();
			process.WaitForExit();
			process.Close();
		}
		catch
		{
		}
		GlobalVar.ShareVideoRam = MemoryMappedFile.CreateOrOpen("nx_video_memory", 2765312L).CreateViewAccessor();
		for (int num = 0; num < 2765312; num++)
		{
			GlobalVar.ShareVideoRam.Write(num, 128);
		}
		Task.Factory.StartNew(delegate
		{
			try
			{
				_ = new byte[32];
				NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream("NxConPipe", PipeDirection.InOut);
				namedPipeServerStream.WaitForConnection();
				BinaryReader binaryReader = new BinaryReader(namedPipeServerStream);
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Stop();
				while (true)
				{
					try
					{
						int count = (int)binaryReader.ReadUInt32();
						string str = new string(binaryReader.ReadChars(count));
						Nmc.SendPythonSerial(str);
						stopwatch.Restart();
					}
					catch (Exception)
					{
						namedPipeServerStream.Close();
						Nmc.PythonKeyFlag = 9259542121117908992uL;
						namedPipeServerStream = new NamedPipeServerStream("NxConPipe", PipeDirection.InOut);
						namedPipeServerStream.WaitForConnection();
						binaryReader = new BinaryReader(namedPipeServerStream);
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		});
		DiscordRpcClient = new DiscordRpcClient("785369550442201088");
		DiscordRpcClient.OnReady += delegate
		{
		};
		DiscordRpcClient.OnPresenceUpdate += delegate
		{
		};
		DiscordRpcClient.Initialize();
		DiscordRpcClient.SetPresence(new RichPresence
		{
			Details = "",
			State = "",
			Assets = new Assets
			{
				LargeImageKey = "icon22222_512",
				LargeImageText = "",
				SmallImageKey = "idle",
				SmallImageText = "停止中"
			}
		});
		kbh.KeyboardHooked += keyboardHook1_KeyboardHooked;
		Text = GlobalVar.AppName;
		GlobalVar.MAINFORM = this;
		System.Windows.Forms.Application.AddMessageFilter(_mFilter);
		CodeEdit = new TextEditor();
		LsBox = new System.Windows.Controls.ListBox();
		elementHost1.Child = CodeEdit;
		IHighlightingDefinition syntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(Resources.NX))), HighlightingManager.Instance);
		CodeEdit.SyntaxHighlighting = syntaxHighlighting;
		CodeEdit.FontFamily = new System.Windows.Media.FontFamily("Consola");
		CodeEdit.Text = "//ここにマクロを記述する\r\n";
		CodeEdit.CaretOffset = CodeEdit.Text.Length;
		CodeEdit.ShowLineNumbers = true;
		CodeEdit.Options.ShowEndOfLine = true;
		CodeEdit.Options.AllowScrollBelowDocument = false;
		CodeEdit.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
		CodeEdit.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
		CodeEdit.Drop += delegate(object sender, System.Windows.DragEventArgs args)
		{
			string[] array = (string[])args.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, autoConvert: false);
			try
			{
				string text = Path.GetExtension(array[0]).ToLower();
				if (text == ".nxc" || text == ".nmc")
				{
					Nmc.NMCRead(array[0]);
					Text = GlobalVar.AppName + " - " + Path.GetFileName(array[0]);
					CodeEdit.TextArea.Document.BeginUpdate();
					CodeEdit.TextArea.Document.Text = Nmc.Code;
					CodeEdit.TextArea.Document.EndUpdate();
					マクロを上書き保存ToolStripMenuItem.Enabled = true;
					flowLayoutPanel3.Enabled = true;
					flowLayoutPanel3.Visible = true;
					SetMacroDirectory(array[0]);
					ImageReload();
					DataFileReload();
				}
			}
			catch
			{
			}
		};
		CodeEdit.LayoutUpdated += delegate
		{
			_vScrollF = true;
			vScrollBar1.Maximum = (int)Math.Max(0.0, (CodeEdit.ExtentHeight - CodeEdit.ViewportHeight) * 100.0);
			vScrollBar1.Visible = vScrollBar1.Maximum != 0;
			vScrollBar1.Visible = true;
			vScrollBar1.Value = (int)Math.Max(0.0, CodeEdit.VerticalOffset * 100.0);
			hScrollBar1.Maximum = (int)Math.Max(0.0, (CodeEdit.ExtentWidth - CodeEdit.ViewportWidth) * 100.0);
			hScrollBar1.Visible = hScrollBar1.Maximum != 0;
			hScrollBar1.Visible = true;
			hScrollBar1.Value = (int)Math.Max(0.0, CodeEdit.HorizontalOffset * 100.0);
			vScrollBar1.Top = elementHost1.Top;
			hScrollBar1.Left = elementHost1.Left;
			hScrollBar1.Top = panel1.Height - hScrollBar1.Height;
			vScrollBar1.Left = panel1.Width - vScrollBar1.Width;
			elementHost1.Height = panel1.Height - 1 - ((!hScrollBar1.Visible) ? 1 : hScrollBar1.Height);
			if (!hScrollBar1.Visible)
			{
				vScrollBar1.Height = panel1.Height;
			}
			else
			{
				vScrollBar1.Height = panel1.Height - hScrollBar1.Height;
			}
			if (!vScrollBar1.Visible)
			{
				hScrollBar1.Width = panel1.Width;
				label1.Visible = false;
			}
			else
			{
				hScrollBar1.Width = panel1.Width - vScrollBar1.Width;
				if (hScrollBar1.Visible)
				{
					label1.BackColor = BZStyle.NormalColor;
					label1.Visible = true;
					label1.Left = panel1.Width - vScrollBar1.Width;
					label1.Top = panel1.Height - hScrollBar1.Height;
					label1.Width = vScrollBar1.Width;
					label1.Height = hScrollBar1.Height;
				}
				else
				{
					label1.Visible = false;
				}
			}
			_vScrollF = false;
		};
		vScrollBar1.ValueChanged += delegate
		{
			if (!_vScrollF)
			{
				CodeEdit.ScrollToVerticalOffset((double)vScrollBar1.Value / 100.0);
			}
		};
		hScrollBar1.ValueChanged += delegate
		{
			if (!_vScrollF)
			{
				CodeEdit.ScrollToHorizontalOffset((double)hScrollBar1.Value / 100.0);
			}
		};
		CodeEdit.PreviewKeyUp += delegate(object sender, System.Windows.Input.KeyEventArgs args)
		{
			if (_popUpWindow != null && (args.Key == Key.Up || args.Key == Key.Down || args.Key == Key.Return))
			{
				args.Handled = true;
			}
		};
		CodeEdit.PreviewKeyDown += delegate(object sender, System.Windows.Input.KeyEventArgs args)
		{
			if (args.Key == Key.S && isPressCtrl)
			{
				if (マクロを上書き保存ToolStripMenuItem.Enabled)
				{
					マクロを上書き保存ToolStripMenuItem.PerformClick();
				}
				else
				{
					マクロの保存ToolStripMenuItem.PerformClick();
				}
				args.Handled = true;
			}
			if (_popUpWindow != null && (args.Key == Key.Up || args.Key == Key.Down || args.Key == Key.Return))
			{
				if (_popUpWindow.listBox1.Items.Count > 0)
				{
					if (args.Key == Key.Up && _popUpWindow.listBox1.SelectedIndex > 0)
					{
						_popUpWindow.listBox1.SelectedIndex--;
					}
					if (args.Key == Key.Down && _popUpWindow.listBox1.SelectedIndex < _popUpWindow.listBox1.Items.Count - 1)
					{
						_popUpWindow.listBox1.SelectedIndex++;
					}
					if (args.Key == Key.Return)
					{
						_popUpWindow.ReplaceLabel();
						_popUpWindow.Close();
					}
				}
				args.Handled = true;
			}
			else
			{
				if (_popUpWindow == null && args.Key == Key.Up && CodeEdit.TextArea.Caret.Line >= 2)
				{
					CodeEdit.TextArea.Caret.Line--;
					args.Handled = true;
				}
				if (_popUpWindow == null && args.Key == Key.Down && CodeEdit.TextArea.Caret.Line < CodeEdit.Document.LineCount)
				{
					CodeEdit.TextArea.Caret.Line++;
					args.Handled = true;
				}
			}
		};
		CodeEdit.TextArea.TextEntered += textEditor_TextArea_TextEntered;
		CodeEdit.PreviewKeyUp += TextAreaOnKeyUp;
		CodeEdit.TextArea.TextEntering += TextArea_TextEntering;
		CodeEdit.MouseHover += CodeEditOnMouseHover;
		CodeEdit.Loaded += delegate
		{
			if (PresentationSource.FromVisual(CodeEdit).CompositionTarget is HwndTarget hwndTarget)
			{
				hwndTarget.RenderMode = RenderMode.Default;
			}
		};
		System.Windows.Forms.ContextMenu elctm = new System.Windows.Forms.ContextMenu();
		elctm.MenuItems.Add("実行(&E)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			button6.PerformClick();
		};
		elctm.MenuItems.Add("この行から実行(&S)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			macroStartButtonFunc(CodeEdit.TextArea.Caret.Line - 1);
		};
		elctm.MenuItems.Add("-");
		elctm.MenuItems.Add("元に戻す(&U)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Undo();
		};
		elctm.MenuItems.Add("やり直し(&R)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Redo();
		};
		elctm.MenuItems.Add("-");
		elctm.MenuItems.Add("切り取り(&T)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Cut();
		};
		elctm.MenuItems.Add("コピー(&C)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Copy();
		};
		elctm.MenuItems.Add("貼り付け(&P)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Paste();
		};
		elctm.MenuItems.Add("削除(&D)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.Delete();
		};
		elctm.MenuItems.Add("-");
		elctm.MenuItems.Add("すべて選択(&A)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			CodeEdit.SelectAll();
		};
		elctm.MenuItems.Add("-");
		elctm.MenuItems.Add("ヘルプ(&H)");
		elctm.MenuItems[elctm.MenuItems.Count - 1].Click += delegate
		{
			ヘルプToolStripMenuItem.PerformClick();
		};
		elctm.Popup += delegate
		{
			elctm.MenuItems[3].Enabled = CodeEdit.CanUndo;
			elctm.MenuItems[4].Enabled = CodeEdit.CanRedo;
			elctm.MenuItems[0].Enabled = button4.Enabled;
			elctm.MenuItems[1].Enabled = button4.Enabled;
		};
		elementHost1.ContextMenu = elctm;
		toolTip1.SetToolTip(elementHost1, "Test");
	}

	private void TextAreaOnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
	{
		System.Drawing.Point lpPoint = new System.Drawing.Point(base.Left, base.Top);
		GetCaretPos(out lpPoint);
		int num = IsInMat(CodeEdit.TextArea.Caret.Line, CodeEdit.TextArea.Caret.Column);
		if (num == 1)
		{
			if (_popUpWindow == null)
			{
				if (completionWindow != null)
				{
					completionWindow.Close();
				}
				_popUpWindow = new Matsub(0);
				_popUpWindow.nxmc = this;
				_popUpWindow.line = CodeEdit.TextArea.Caret.Line - 1;
				_popUpWindow.Caretflg = true;
				string matImage = GetMatImage(CodeEdit.TextArea.Caret.Line);
				_popUpWindow.SelectedIndex = Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList().IndexOf(matImage);
				_popUpWindow.Left = lpPoint.X + elementHost1.PointToScreen(new System.Drawing.Point(0, 0)).X;
				_popUpWindow.Top = lpPoint.Y + elementHost1.PointToScreen(new System.Drawing.Point(0, 0)).Y + 15;
				_popUpWindow.StartPosition = FormStartPosition.Manual;
				_popUpWindow.Show();
			}
		}
		else if (num >= 2)
		{
			if (_popUpWindow == null)
			{
				if (completionWindow != null)
				{
					completionWindow.Close();
				}
				_popUpWindow = new Matsub(num - 1);
				_popUpWindow.nxmc = this;
				_popUpWindow.line = CodeEdit.TextArea.Caret.Line - 1;
				_popUpWindow.Caretflg = true;
				string matImage2 = GetMatImage(CodeEdit.TextArea.Caret.Line);
				_popUpWindow.SelectedIndex = Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList().IndexOf(matImage2);
				_popUpWindow.Left = lpPoint.X + elementHost1.PointToScreen(new System.Drawing.Point(0, 0)).X;
				_popUpWindow.Top = lpPoint.Y + elementHost1.PointToScreen(new System.Drawing.Point(0, 0)).Y + 15;
				_popUpWindow.StartPosition = FormStartPosition.Manual;
				_popUpWindow.Show();
			}
		}
		else if (_popUpWindow != null)
		{
			_popUpWindow.Close();
			_popUpWindow = null;
		}
	}

	private void mouseHook1_MouseHooked(object sender, MouseHookedEventArgs e)
	{
		try
		{
			if (_popUpWindow != null && e.Message == MouseMessage.Move)
			{
				_ = _popUpWindow.Caretflg;
			}
		}
		catch (Exception)
		{
		}
	}

	private void CodeEditOnMouseHover(object sender, System.Windows.Input.MouseEventArgs e)
	{
		TextViewPosition? positionFromPoint = CodeEdit.GetPositionFromPoint(e.GetPosition(CodeEdit));
		if (!positionFromPoint.HasValue)
		{
			return;
		}
		int num = IsInMat(positionFromPoint.Value.Line, positionFromPoint.Value.Column);
		if (num == 1)
		{
			if (_popUpWindow == null)
			{
				_popUpWindow = new Matsub(0);
				_popUpWindow.nxmc = this;
				_popUpWindow.line = positionFromPoint.Value.Line - 1;
				string matImage = GetMatImage(positionFromPoint.Value.Line);
				_popUpWindow.SelectedIndex = Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList().IndexOf(matImage);
				_popUpWindow.Left = System.Windows.Forms.Control.MousePosition.X - 14;
				_popUpWindow.Top = System.Windows.Forms.Control.MousePosition.Y;
				_popUpWindow.StartPosition = FormStartPosition.Manual;
				_popUpWindow.Show();
				_popUpWindow.Focus();
				e.Handled = true;
			}
		}
		else if (num >= 2 && _popUpWindow == null)
		{
			_popUpWindow = new Matsub(num - 1);
			_popUpWindow.nxmc = this;
			_popUpWindow.line = positionFromPoint.Value.Line - 1;
			GetMatImage(positionFromPoint.Value.Line);
			_popUpWindow.Left = System.Windows.Forms.Control.MousePosition.X - 14;
			_popUpWindow.Top = System.Windows.Forms.Control.MousePosition.Y;
			_popUpWindow.StartPosition = FormStartPosition.Manual;
			_popUpWindow.Show();
			e.Handled = true;
		}
	}

	private void TextArea_TextEntering(object sender, TextCompositionEventArgs e)
	{
	}

	public void Closepopup()
	{
		if (_popUpWindow != null)
		{
			try
			{
				_popUpWindow.Dispose();
			}
			catch (Exception)
			{
			}
			_popUpWindow = null;
		}
	}

	public void KeyInputSet(string key, decimal time, decimal waitTime = 0m, bool plF = false)
	{
		string[] array = new string[2]
		{
			"Press(" + key + ", " + time.ToString("F2") + ")",
			""
		};
		if (waitTime != 0m)
		{
			array = new string[2]
			{
				"Press(" + key + ", " + time.ToString("F2") + ", " + waitTime.ToString("F2") + ")",
				""
			};
		}
		if (plF)
		{
			array = key.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
			array.Append("");
		}
		TextArea textArea = CodeEdit.TextArea;
		string text = "";
		bool flag = false;
		for (int i = 0; i < textArea.Document.Lines[textArea.Caret.Line - 1].Length; i++)
		{
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == " ")
			{
				text += " ";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == "\t")
			{
				text += "\t";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() == "\u3000")
			{
				text += "\u3000";
			}
			if (textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != "\t" && textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != " " && textArea.Document.Text[textArea.Document.Lines[textArea.Caret.Line - 1].Offset + i].ToString() != "\u3000")
			{
				flag = true;
				break;
			}
		}
		string text2 = "";
		for (int j = 0; j < array.Length; j++)
		{
			if (j != 0)
			{
				text2 += text;
			}
			else if (flag)
			{
				text2 = text2 + "\r\n" + text;
			}
			text2 += array[j];
			if (j < array.Length - 1)
			{
				text2 += "\r\n";
			}
		}
		textArea.Document.Insert(textArea.Caret.Offset, text2);
	}

	[DllImport("user32.dll")]
	private static extern bool GetCaretPos(out System.Drawing.Point lpPoint);

	private void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
	{
		if (e.Text == "{")
		{
			CodeEdit.TextArea.Document.Replace(CodeEdit.TextArea.Caret.Offset - 1, 1, "{}");
			CodeEdit.SelectionStart--;
		}
		if (e.Text == "(")
		{
			CodeEdit.TextArea.Document.Replace(CodeEdit.TextArea.Caret.Offset - 1, 1, "()");
			CodeEdit.SelectionStart--;
		}
		if (CodeEdit.SelectionStart >= 2 && CodeEdit.Text[CodeEdit.SelectionStart - 2] != ' ' && CodeEdit.Text[CodeEdit.SelectionStart - 2] != '\n' && CodeEdit.Text[CodeEdit.SelectionStart - 2] != '\t' && CodeEdit.Text[CodeEdit.SelectionStart - 2] != '(' && CodeEdit.Text[CodeEdit.SelectionStart - 2] != '{')
		{
			return;
		}
		completionWindow = new CompletionWindow(CodeEdit.TextArea);
		IList<ICompletionData> completionData = completionWindow.CompletionList.CompletionData;
		string text = e.Text.ToLower();
		string text2 = "";
		for (int i = 0; i < text.Length; i++)
		{
			text2 += text[i];
			text2 += ".*?";
		}
		string[][] array = commandlist;
		foreach (string[] array2 in array)
		{
			if (text2[0] == array2[0][0].ToString().ToLower()[0] && Regex.Match(array2[0].ToLower(), text2).Success)
			{
				completionData.Add(new CompletionData(array2));
			}
		}
		if (completionData.Count > 0)
		{
			completionWindow.Show();
			completionWindow.Closed += delegate
			{
				completionWindow = null;
			};
		}
	}

	private bool IsInComment(int line, int column)
	{
		if (!(CodeEdit.TextArea.GetService(typeof(IHighlighter)) is IHighlighter highlighter))
		{
			return false;
		}
		int off = CodeEdit.Document.GetOffset(line, column);
		HighlightedLine highlightedLine = highlighter.HighlightLine(line);
		if (highlightedLine.Sections.Count == 0)
		{
			return false;
		}
		return highlightedLine.Sections.Any((HighlightedSection s) => s.Offset <= off && s.Offset + s.Length >= off && s.Color.Foreground.ToString() == "#" + System.Drawing.Color.Green.ToArgb().ToString("X8"));
	}

	private int IsInMat(int line, int column)
	{
		DocumentLine documentLine = CodeEdit.Document.Lines[line - 1];
		try
		{
			string[] array = CodeEdit.Document.GetText(documentLine.Offset, column - 1).Split();
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string text in array2)
			{
				list.AddRange(text.Split(')'));
			}
			list[list.Count - 1] = list[list.Count - 1].TrimStart();
			if (list[list.Count - 1].Substring(0, 5) == "Call(")
			{
				return 2;
			}
			if (list[list.Count - 1].Substring(0, 7) == "ImgCmp(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 8) == "CallCsx(")
			{
				return 3;
			}
			if (list[list.Count - 1].Substring(0, 11) == "ImgCmpRect(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 11) == "ImgCmpGray(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 11) == "ImgCmp720p(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 15) == "ImgCmpRect720p(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 15) == "ImgCmpRectGray(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 15) == "ImgCmpGray720p(")
			{
				return 1;
			}
			if (list[list.Count - 1].Substring(0, 19) == "ImgCmpRectGray720p(")
			{
				return 1;
			}
		}
		catch (Exception)
		{
		}
		return 0;
	}

	private string GetMatImage(int line)
	{
		DocumentLine documentLine = CodeEdit.Document.Lines[line - 1];
		try
		{
			string[] array = CodeEdit.Document.GetText(documentLine.Offset, documentLine.Length).Split('(', ')');
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i].TrimStart() == "ImgCmp")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpRect")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpGray")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmp720p")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpRect720p")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpGray720p")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpRectGray")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "ImgCmpRectGray720p")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "CallCsx")
				{
					return array[i + 1];
				}
				if (array[i].TrimStart() == "Call")
				{
					return array[i + 1];
				}
			}
		}
		catch (Exception)
		{
		}
		return "";
	}

	public void ImageReload(bool isDataOnly = false)
	{
		flowLayoutPanel1.Controls.Clear();
		for (int i = 0; i < Nmc.ResourcesImages.Count; i++)
		{
			ImgResItem imgResItem = new ImgResItem(Nmc.ResourcesImages[i]);
			imgResItem.Width = 155;
			imgResItem.Height = 100;
			flowLayoutPanel1.Controls.Add(imgResItem);
		}
		DropIcon dropIcon = new DropIcon(isFile: false);
		dropIcon.Width = 155;
		dropIcon.Height = 100;
		flowLayoutPanel1.Controls.Add(dropIcon);
		flowLayoutPanel1.ContextMenuStrip = dropIcon.ContextMenuStrip;
		FileItemTheme(KEYCONFIG.AppConfig.APPTHEME);
		flowLayoutPanel1_Resize(null, null);
	}

	public void DataFileReload()
	{
		flowLayoutPanel3.Controls.Clear();
		new List<string>();
		if (MacroDirectory != "" && Directory.Exists(MacroDirectory))
		{
			if (CurrentDirectory != "")
			{
				FileResItem fileResItem = new FileResItem(null);
				fileResItem.SetFolder("..\\");
				fileResItem.Width = 155;
				fileResItem.Height = 100;
				flowLayoutPanel3.Controls.Add(fileResItem);
			}
			string[] directories = Directory.GetDirectories(MacroDirectory + CurrentDirectory, "*", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i] = Util.GetRelativePath(MacroDirectory, directories[i]).Substring(2);
				FileResItem fileResItem2 = new FileResItem(null);
				fileResItem2.SetFolder(directories[i]);
				fileResItem2.Width = 155;
				fileResItem2.Height = 100;
				flowLayoutPanel3.Controls.Add(fileResItem2);
			}
			directories = Directory.GetFiles(MacroDirectory + CurrentDirectory, "*", SearchOption.TopDirectoryOnly);
			for (int j = 0; j < directories.Length; j++)
			{
				directories[j] = Util.GetRelativePath(MacroDirectory, directories[j]).Substring(2);
				FileResItem fileResItem3 = new FileResItem(directories[j]);
				fileResItem3.Width = 155;
				fileResItem3.Height = 100;
				flowLayoutPanel3.Controls.Add(fileResItem3);
			}
		}
		DropIcon dropIcon = new DropIcon(isFile: true);
		dropIcon.Width = 155;
		dropIcon.Height = 100;
		dropIcon.isFile = true;
		flowLayoutPanel3.Controls.Add(dropIcon);
		flowLayoutPanel3.ContextMenuStrip = dropIcon.ContextMenuStrip;
		FileItemTheme(KEYCONFIG.AppConfig.APPTHEME);
	}

	public void MacroShortCutReload()
	{
		flowLayoutPanel2.Controls.Clear();
		for (int i = 0; i < GlobalVar.MacroList.Count; i++)
		{
			if (!string.IsNullOrWhiteSpace(GlobalVar.MacroList[i]))
			{
				MacroItem macroItem = new MacroItem(GlobalVar.MacroList[i]);
				macroItem.Width = 160;
				macroItem.Height = 105;
				Util.EnableDoubleBuffering(macroItem);
				flowLayoutPanel2.Controls.Add(macroItem);
			}
		}
		DropMacro dropMacro = new DropMacro();
		dropMacro.Width = 160;
		dropMacro.Height = 105;
		dropMacro.AllowDrop = true;
		Util.EnableDoubleBuffering(dropMacro);
		flowLayoutPanel2.Controls.Add(dropMacro);
		flowLayoutPanel2_Resize(null, null);
	}

	private void Press(string x = "", int y = 0)
	{
	}

	private async void Form1_Load(object sender, EventArgs e)
	{
		macroSubDirCmb.Tag = null;
		_lastHighlight.Start();
		_lastTaskView.Start();
		if (base.DesignMode)
		{
			return;
		}
		if (!Directory.Exists(GlobalVar.BasePath + "Poke-Controller"))
		{
			File.WriteAllBytes(GlobalVar.BasePath + "Poke-Controller.zip", Resources.Poke_Controller);
			ZipFile.ExtractToDirectory(GlobalVar.BasePath + "Poke-Controller.zip", GlobalVar.BasePath ?? "");
			File.Delete(GlobalVar.BasePath + "Poke-Controller.zip");
		}
		if (!Directory.Exists(GlobalVar.BasePath + "CH552"))
		{
			File.WriteAllBytes(GlobalVar.BasePath + "CH552.zip", Resources.CH552);
			ZipFile.ExtractToDirectory(GlobalVar.BasePath + "CH552.zip", GlobalVar.BasePath ?? "");
			File.Delete(GlobalVar.BasePath + "CH552.zip");
			"https://bzl-web.com/file/sdcc/sdcc.zip".ToDownload(GlobalVar.BasePath + "CH552\\sdcc.zip");
			ZipFile.ExtractToDirectory(GlobalVar.BasePath + "CH552\\sdcc.zip", GlobalVar.BasePath + "CH552");
			File.Delete(GlobalVar.BasePath + "CH552\\sdcc.zip");
		}
		mtw = new MultiTextWriter(new ControlWriter(textBox1), Console.Out);
		Console.SetOut(mtw);
		comboBoxEx2_Enter(null, null);
		CodeEdit.FontFamily = new System.Windows.Media.FontFamily("Consola");
		ForeColor = BZStyle.TextFont;
		BackColor = BZStyle.GrayColor;
		menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
		menuStrip1.ForeColor = BZStyle.TextFont;
		Util.EnableDoubleBuffering(CaptureScreen);
		tabControlEx1.Rank = 1;
		groupBoxEx1.BackColor = BZStyle.BackColor;
		textBox1.BackColor = BZStyle.BackColor;
		textBox1.ForeColor = BZStyle.TextFont;
		scrollBarEx1.Maximum = 0;
		groupBox1.Refresh();
		foreach (ToolStripMenuItem item in menuStrip1.Items)
		{
			item.ForeColor = BZStyle.TextFont;
			foreach (ToolStripItem dropDownItem in item.DropDownItems)
			{
				dropDownItem.ForeColor = BZStyle.TextFont;
			}
		}
		panel2.Location = new System.Drawing.Point(base.ActualLeft, base.ActualTop);
		panel2.Size = new System.Drawing.Size(base.ActualWidth, base.ActualHeight);
		panel3.BackColor = BZStyle.HighlightColor;
		groupBox1.Click += delegate
		{
			Focus();
			base.ActiveControl = null;
		};
		macroDirReload();
		fileSystemWatcher4.EnableRaisingEvents = false;
		fileSystemWatcher4.Dispose();
		fileSystemWatcher4 = new FileSystemWatcher(GlobalVar.BasePath + "Macro");
		fileSystemWatcher4.Filter = "*";
		fileSystemWatcher4.IncludeSubdirectories = false;
		fileSystemWatcher4.EnableRaisingEvents = true;
		fileSystemWatcher4.Created += delegate
		{
			macroDirReload();
		};
		fileSystemWatcher4.Deleted += delegate
		{
			macroDirReload();
		};
		fileSystemWatcher4.Renamed += delegate
		{
			macroDirReload();
		};
		_ = GlobalVar.BasePath + "config.ini";
		if (!File.Exists(GlobalVar.BasePath + "config.ini"))
		{
			Util.SaveConfig();
		}
		ReadConfig();
		GamePadInput.Start();
		NxControllerInterface.SerialPort = _serialPort;
		CapDeviceList.Items.Clear();
		DsDevice[] devicesOfCat = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
		foreach (DsDevice dsDevice in devicesOfCat)
		{
			CapDeviceList.Items.Add(dsDevice.Name);
		}
		if (CapDeviceList.Items.Count > 0)
		{
			CapDeviceList.SelectedIndex = 0;
		}
		ImageReload();
		DataFileReload();
		MacroShortCutReload();
		GlobalVar.TaskName[0] = "準備完了";
	}

	private void ReadConfig()
	{
		Util.ReadConfig();
		SetTheme(KEYCONFIG.AppConfig.APPTHEME);
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
			if ((string)item.GetPropertyValue("Description") == "Generic Bluetooth Adapter" || (string)item.GetPropertyValue("Description") == "Generic Bluetooth Radio")
			{
				list.Add(new USBDeviceInfo((string)item.GetPropertyValue("DeviceID"), (string)item.GetPropertyValue("PNPDeviceID"), (string)item.GetPropertyValue("Description")));
			}
		}
		managementObjectCollection.Dispose();
		return list;
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
	}

	private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		Util.SaveConfig();
		DiscordRpcClient.Dispose();
		CaptureBGW.CancelAsync();
		while (CaptureBGW.IsBusy)
		{
			System.Windows.Forms.Application.DoEvents();
		}
		if (NxControllerInterface.StartedBluetooth)
		{
			NxControllerInterface.ShutdownGamepad();
		}
		if (_serialPort.IsOpen)
		{
			_serialPort.Close();
		}
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = "regsvr32.exe";
			process.StartInfo.Arguments = "/s /u \"" + Path.GetFullPath(GlobalVar.BasePath + "NX2VCam.dll") + "\"";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Verb = "RunAs";
			process.Start();
			process.WaitForExit();
			process.Close();
		}
		catch
		{
		}
	}

	private unsafe void CapConnect_Click(object sender, EventArgs e)
	{
		int capIndex = CapDeviceList.SelectedIndex;
		if (DsCapture != null)
		{
			DsCapture.Stop();
			DsCapture.Dispose();
			DsCapture = null;
			if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
			{
				CapConnect.Image = Resources.B3;
			}
			else
			{
				CapConnect.Image = Resources.B3_L;
			}
			CapDeviceList.Enabled = true;
			CurrentCaptureFormat = CaptureStyle.None;
			GlobalVar.TaskName[1] = "";
			GlobalVar.MAINFORM.TaskView();
			return;
		}
		if (CvCapture != null)
		{
			CvCapture.Dispose();
			CvCapture = null;
			if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
			{
				CapConnect.Image = Resources.B3;
			}
			else
			{
				CapConnect.Image = Resources.B3_L;
			}
			CapDeviceList.Enabled = true;
			CurrentCaptureFormat = CaptureStyle.None;
			GlobalVar.TaskName[1] = "";
			GlobalVar.MAINFORM.TaskView();
			return;
		}
		GlobalVar.TaskName[1] = $"映像デバイス({CapDeviceList.Items[capIndex]}) : 接続試行中";
		GlobalVar.MAINFORM.TaskView();
		CapConnect.Image = Resources.B3_LINK;
		CapDeviceList.Enabled = false;
		Mat _frame = new Mat();
		if (KEYCONFIG.AppConfig.CAPTURESTYLE == CaptureStyle.DirectShow)
		{
			DsCapture = new DSHDMICapture(capIndex, CaptureScreen.Handle);
			DsCapture.renderingSize = CaptureScreen.ClientSize;
			DsCapture.Play();
			CurrentCaptureFormat = CaptureStyle.DirectShow;
		}
		else
		{
			CvCapture = new VideoCapture();
			CvCapture.Open(capIndex);
			CvCapture.Set(CaptureProperty.FrameWidth, 1920.0);
			CvCapture.Set(CaptureProperty.FrameHeight, 1080.0);
			CvCapture.Set(CaptureProperty.Fps, 29.97);
			CurrentCaptureFormat = CaptureStyle.OpenCV;
		}
		Bitmap image = new Bitmap(1920, 1080);
		CaptureScreen.Image = image;
		Task.Factory.StartNew(delegate
		{
			GlobalVar.TaskName[1] = $"映像デバイス({CapDeviceList.Items[capIndex]}) : 接続中";
			GlobalVar.MAINFORM.TaskView();
			if (captureRun)
			{
				captureRun = false;
				return;
			}
			captureRun = true;
			new Stopwatch().Stop();
			byte[] array = new byte[2764800];
			Task.Factory.StartNew(delegate
			{
				Bitmap bitmap3 = new Bitmap(1920, 1080);
				while (true)
				{
					try
					{
						if (CurrentCaptureFormat == CaptureStyle.OpenCV)
						{
							CvCapture.Read(_frame);
							if (_frame.Size().Width > 0)
							{
								bitmap3 = _frame.ToBitmap();
								double num6 = 0.0;
								Bitmap bitmap4;
								lock (NxCommand.lockObject)
								{
									NxCommand.CurrentFrame = bitmap3;
									int num7 = CaptureScreen.Width;
									int num8 = CaptureScreen.Height;
									double val = (double)num7 / (double)bitmap3.Width;
									double val2 = (double)num8 / (double)bitmap3.Height;
									num6 = Math.Min(val, val2);
									int num9 = (int)((double)bitmap3.Width * num6);
									int num10 = (int)((double)bitmap3.Height * num6);
									bitmap4 = (Bitmap)bitmap3.ImageResize(num9, num10);
									if (_captureNow)
									{
										int num11 = (int)((double)Math.Min(NxSel.X1, NxSel.X2) * num6);
										int num12 = (int)((double)Math.Max(NxSel.X1, NxSel.X2) * num6);
										int num13 = (int)((double)Math.Min(NxSel.Y1, NxSel.Y2) * num6);
										int num14 = (int)((double)Math.Max(NxSel.Y1, NxSel.Y2) * num6);
										Bitmap bitmap5 = Util.AdjustBrightness(bitmap4, -30);
										if (Math.Min(NxSel.Y1, NxSel.Y2) != -1)
										{
											Graphics graphics = Graphics.FromImage(bitmap5);
											graphics.DrawImage(bitmap4, num11, num13, new Rectangle(num11, num13, num12 - num11, num14 - num13), GraphicsUnit.Pixel);
											graphics.Dispose();
										}
										bitmap4 = bitmap5;
									}
								}
								captureScreenBuffer = bitmap4;
								Invoke((Action)delegate
								{
									try
									{
										CaptureScreen.Invalidate();
									}
									catch (Exception)
									{
									}
								});
							}
							Thread.Sleep(30);
						}
						else
						{
							Thread.Sleep(1000);
						}
					}
					catch
					{
						Thread.Sleep(1000);
					}
				}
			});
			while (true)
			{
				if (!captureRun)
				{
					captureRun = true;
				}
				try
				{
					_ = CurrentCaptureFormat;
					_ = 2;
					Bitmap bitmap = CaptureImage();
					if (bitmap != null)
					{
						Bitmap bitmap2 = (Bitmap)bitmap.ImageResize(1280, 720);
						BitmapData bitmapData = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
						byte* ptr = (byte*)(void*)bitmapData.Scan0;
						if (ptr != null)
						{
							GlobalVar.ShareVideoRam = MemoryMappedFile.CreateOrOpen("nx_video_memory", 2765312L).CreateViewAccessor();
							fixed (byte* ptr2 = array)
							{
								_ = bitmap2.Height;
								_ = bitmap2.Width;
								for (int num = 719; num >= 0; num--)
								{
									int num2 = 720 - num - 1;
									int num3 = num * 3840;
									int num4 = num2 * 3840;
									for (int num5 = 0; num5 < 3840; num5++)
									{
										ptr2[num3 + num5] = ptr[num4 + num5];
									}
								}
							}
							GlobalVar.ShareVideoRam.WriteArray(0L, array, 0, array.Length);
						}
						bitmap2.UnlockBits(bitmapData);
					}
					Thread.Sleep(8);
					GC.Collect(0);
					GC.WaitForPendingFinalizers();
					GC.Collect(1);
					GC.WaitForPendingFinalizers();
					GC.Collect(2);
					GC.WaitForPendingFinalizers();
				}
				catch
				{
					Thread.Sleep(100);
				}
			}
		});
	}

	private void button1_Click(object sender, EventArgs e)
	{
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (!InputDialog.Opening)
		{
			InputDialog.Opening = true;
			new InputDialog().Show();
		}
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
	}

	private void button1_Click_1(object sender, EventArgs e)
	{
	}

	private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void マクロの読み込みToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "NX Macro Controller用マクロファイル(*.nxc;*.nmc)|*.nxc;*.nmc|すべてのファイル(*.*)|*.*";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			LoadMacro(openFileDialog.FileName);
		}
	}

	private void LoadMacro(string path)
	{
		Nmc.NMCRead(path);
		Text = GlobalVar.AppName + " - " + Path.GetFileName(path);
		CodeEdit.TextArea.Document.BeginUpdate();
		CodeEdit.TextArea.Document.Text = Nmc.Code;
		CodeEdit.TextArea.Document.EndUpdate();
		マクロを上書き保存ToolStripMenuItem.Enabled = true;
		flowLayoutPanel3.Enabled = true;
		flowLayoutPanel3.Visible = true;
		SetMacroDirectory(path);
		ImageReload();
		DataFileReload();
	}

	public void SetMacroDirectory(string path)
	{
		CurrentDirectory = "";
		MacroDirectory = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "\\";
		if (Directory.Exists(MacroDirectory))
		{
			FSWReload();
		}
	}

	public void FSWReload()
	{
		fileSystemWatcher1.EnableRaisingEvents = false;
		fileSystemWatcher1.Dispose();
		fileSystemWatcher1 = new FileSystemWatcher(MacroDirectory + CurrentDirectory);
		fileSystemWatcher1.Filter = "*";
		fileSystemWatcher1.IncludeSubdirectories = false;
		fileSystemWatcher1.EnableRaisingEvents = true;
		fileSystemWatcher1.Created += fileSystemWatcher1_Created;
		fileSystemWatcher1.Deleted += fileSystemWatcher1_Deleted;
		fileSystemWatcher1.Renamed += fileSystemWatcher1_Renamed;
		fileSystemWatcher2.EnableRaisingEvents = false;
		fileSystemWatcher2.Dispose();
		fileSystemWatcher2 = new FileSystemWatcher(Path.GetDirectoryName(Path.GetDirectoryName(MacroDirectory)));
		fileSystemWatcher2.Filter = "*";
		fileSystemWatcher2.IncludeSubdirectories = false;
		fileSystemWatcher2.EnableRaisingEvents = true;
		fileSystemWatcher2.Deleted += fileSystemWatcher2_Deleted;
		fileSystemWatcher2.Renamed += fileSystemWatcher2_Renamed;
		fileSystemWatcher3.EnableRaisingEvents = false;
		fileSystemWatcher3.Dispose();
		fileSystemWatcher3 = new FileSystemWatcher(Path.GetDirectoryName(Path.GetDirectoryName(MacroDirectory + CurrentDirectory)));
		fileSystemWatcher3.Filter = "*";
		fileSystemWatcher3.IncludeSubdirectories = false;
		fileSystemWatcher3.EnableRaisingEvents = true;
		fileSystemWatcher3.Deleted += fileSystemWatcher3_Deleted;
		fileSystemWatcher3.Renamed += fileSystemWatcher3_Renamed;
	}

	private void リソース管理ToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void button4_Click(object sender, EventArgs e)
	{
		if (KeyRecoding)
		{
			KeyRecoding = false;
			return;
		}
		button4.Text = "停止";
		KeyRecoding = true;
		button6.Enabled = false;
		button4.Image = Resources.B4;
		buttonEx2.Enabled = false;
		マクロの読み込みToolStripMenuItem.Enabled = false;
		cH552へ書き込みToolStripMenuItem.Enabled = false;
		cH552SERIALセットアップToolStripMenuItem.Enabled = false;
		Task.Factory.StartNew(delegate
		{
			Stopwatch stopwatch = new Stopwatch();
			ulong num = 9259542121117908992uL;
			stopwatch.Start();
			while (KeyRecoding)
			{
				ulong padAndKeyboardFlag = Nmc.GetPadAndKeyboardFlag();
				if (padAndKeyboardFlag != num)
				{
					long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
					stopwatch.Restart();
					if (elapsedMilliseconds != 0L)
					{
						string text = ((decimal)elapsedMilliseconds / 1000m).ToString("F2");
						string[] keyList = NMC.GetKeyList(num);
						if (keyList.Length == 0)
						{
							string wait = "Wait(" + text + ")";
							Invoke((Action)delegate
							{
								KeyInputSet(wait, 0m, 0m, plF: true);
							});
						}
						else
						{
							string press = "Press(" + string.Join(", ", keyList) + ", " + text + ")";
							Invoke((Action)delegate
							{
								KeyInputSet(press, 0m, 0m, plF: true);
							});
						}
					}
					num = padAndKeyboardFlag;
				}
				Thread.Sleep(1);
			}
			Invoke((Action)delegate
			{
				button4.Text = "記録";
				button6.Enabled = true;
				buttonEx2.Enabled = true;
				マクロの読み込みToolStripMenuItem.Enabled = true;
				cH552へ書き込みToolStripMenuItem.Enabled = true;
				cH552SERIALセットアップToolStripMenuItem.Enabled = true;
				button4.Image = Resources.B2;
			});
		});
	}

	private void 環境設定ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		CaptureStyle cAPTURESTYLE = KEYCONFIG.AppConfig.CAPTURESTYLE;
		SettingDialog settingDialog = new SettingDialog();
		settingDialog.StartPosition = FormStartPosition.CenterParent;
		settingDialog.ShowDialog();
		if (!settingDialog.SettingChanged)
		{
			return;
		}
		ReadConfig();
		GlobalVar.TaskName[0] = "設定が変更されました";
		GlobalVar.MAINFORM.TaskView();
		if (cAPTURESTYLE == KEYCONFIG.AppConfig.CAPTURESTYLE || CapDeviceList.Enabled)
		{
			return;
		}
		if (DsCapture != null)
		{
			DsCapture.Stop();
			DsCapture.Dispose();
			DsCapture = null;
			if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
			{
				CapConnect.Image = Resources.B3;
			}
			else
			{
				CapConnect.Image = Resources.B3_L;
			}
			CurrentCaptureFormat = CaptureStyle.None;
		}
		else if (CvCapture != null)
		{
			CvCapture.Dispose();
			CvCapture = null;
			if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
			{
				CapConnect.Image = Resources.B3;
			}
			else
			{
				CapConnect.Image = Resources.B3_L;
			}
			CurrentCaptureFormat = CaptureStyle.None;
		}
		CapConnect.PerformClick();
	}

	public void MacroActive()
	{
		Invoke((Action)delegate
		{
			DiscordRpcClient.SetPresence(new RichPresence
			{
				Details = "",
				State = "",
				Timestamps = Timestamps.Now,
				Assets = new Assets
				{
					LargeImageKey = "icon22222_512",
					LargeImageText = "",
					SmallImageKey = "online",
					SmallImageText = "動作中"
				}
			});
			panel3.BackColor = System.Drawing.Color.FromArgb(202, 81, 0);
			BorderColor = System.Drawing.Color.FromArgb(202, 81, 0);
			Refresh();
		});
	}

	public void MacroDeactive()
	{
		if (!Nmc.Running)
		{
			Invoke((Action)delegate
			{
				panel3.BackColor = BZStyle.HighlightColor;
				BorderColor = BZStyle.HighlightColor;
				DiscordRpcClient.SetPresence(new RichPresence
				{
					Details = "",
					State = "",
					Assets = new Assets
					{
						LargeImageKey = "icon22222_512",
						LargeImageText = "",
						SmallImageKey = "idle",
						SmallImageText = "停止中"
					}
				});
				Refresh();
			});
		}
	}

	private void button6_Click(object sender, EventArgs e)
	{
		macroStartButtonFunc();
	}

	private void macroStartButtonFunc(int startPos = 0)
	{
		if (Nmc.SubRunningNmc != "")
		{
			Nmc.Cancel = true;
			while (Nmc.Running)
			{
				System.Windows.Forms.Application.DoEvents();
				if (GlobalVar.MAINFORM.Nmc.RunningCsx || NxCommand.ExitFlag)
				{
					if (GlobalVar.NmcThread != null)
					{
						GlobalVar.NmcThread.Abort();
						Nmc.NmcEndSec();
					}
					return;
				}
			}
		}
		if (Nmc.Running)
		{
			Nmc.Cancel = true;
			while (Nmc.Running)
			{
				System.Windows.Forms.Application.DoEvents();
				if (GlobalVar.MAINFORM.Nmc.RunningCsx || NxCommand.ExitFlag)
				{
					if (GlobalVar.NmcThread != null)
					{
						GlobalVar.NmcThread.Abort();
						Nmc.NmcEndSec();
					}
					break;
				}
			}
			return;
		}
		ButtonEx buttonEx = button6;
		string text = (buttonEx5.Text = "停止");
		buttonEx.Text = text;
		ButtonEx buttonEx2 = button6;
		System.Drawing.Image image = (buttonEx5.Image = Resources.B4);
		buttonEx2.Image = image;
		button4.Enabled = false;
		this.buttonEx2.Enabled = false;
		cH552へ書き込みToolStripMenuItem.Enabled = false;
		cH552SERIALセットアップToolStripMenuItem.Enabled = false;
		マクロの読み込みToolStripMenuItem.Enabled = false;
		Nmc.Code = CodeEdit.Text;
		MacroActive();
		CodeEdit.IsReadOnly = true;
		Nmc.IsMain = true;
		Nmc.NmcExecution(startPos);
		Task.Factory.StartNew(delegate
		{
			while (Nmc.Running)
			{
				Thread.Sleep(16);
				if (NxCommand.ExitFlag && GlobalVar.NmcThread != null)
				{
					GlobalVar.NmcThread.Abort();
					Nmc.NmcEndSec();
				}
			}
			Invoke((Action)delegate
			{
				ButtonEx buttonEx3 = button6;
				string text3 = (this.buttonEx5.Text = "実行");
				buttonEx3.Text = text3;
				CodeEdit.IsReadOnly = false;
				button4.Enabled = true;
				this.buttonEx2.Enabled = true;
				cH552へ書き込みToolStripMenuItem.Enabled = true;
				cH552SERIALセットアップToolStripMenuItem.Enabled = true;
				マクロの読み込みToolStripMenuItem.Enabled = true;
				Environment.CurrentDirectory = GlobalVar.BasePath;
				if (_amiibo != "")
				{
					NxControllerInterface.SendAmiibo(_amiibo);
				}
				if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
				{
					ButtonEx buttonEx4 = button6;
					System.Drawing.Image image2 = (this.buttonEx5.Image = Resources.B1);
					buttonEx4.Image = image2;
				}
				else
				{
					ButtonEx buttonEx5 = button6;
					System.Drawing.Image image2 = (this.buttonEx5.Image = Resources.B1_L);
					buttonEx5.Image = image2;
				}
				if (Nmc.SubRunningNmc == "")
				{
					MacroDeactive();
				}
			});
		});
	}

	private void keyboardHook1_KeyboardHooked(object sender, KeyboardHookedEventArgs e)
	{
		if (e.UpDown == KeyboardUpDown.Down)
		{
			if (e.KeyCode == Keys.RControlKey || e.KeyCode == Keys.LControlKey)
			{
				isPressCtrl = true;
			}
		}
		else if (e.UpDown == KeyboardUpDown.Up && (e.KeyCode == Keys.RControlKey || e.KeyCode == Keys.LControlKey))
		{
			isPressCtrl = false;
		}
		if (!KEYCONFIG.ControlConfig.USEKEYBOARD || (KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE && Form.ActiveForm != this) || (KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO && Nmc.Running) || (KEYCONFIG.ControlConfig.GAMEPADONLY && GamePadInput.Connected) || base.ActiveControl == elementHost1)
		{
			Nmc.KeyBoardKeyFlag = 9259542121117908992uL;
			return;
		}
		ulong num = Nmc.KeyBoardKeyFlag;
		if (e.UpDown == KeyboardUpDown.Down)
		{
			if (KEYCONFIG.Button.A == e.KeyCode)
			{
				num |= 8;
			}
			if (KEYCONFIG.Button.B == e.KeyCode)
			{
				num |= 4;
			}
			if (KEYCONFIG.Button.X == e.KeyCode)
			{
				num |= 2;
			}
			if (KEYCONFIG.Button.Y == e.KeyCode)
			{
				num |= 1;
			}
			if (KEYCONFIG.Button.ZL == e.KeyCode)
			{
				num |= 0x800000;
			}
			if (KEYCONFIG.Button.ZR == e.KeyCode)
			{
				num |= 0x80;
			}
			if (KEYCONFIG.Button.L == e.KeyCode)
			{
				num |= 0x400000;
			}
			if (KEYCONFIG.Button.R == e.KeyCode)
			{
				num |= 0x40;
			}
			if (KEYCONFIG.DPad.UP == e.KeyCode)
			{
				num |= 0x20000;
			}
			if (KEYCONFIG.DPad.RIGHT == e.KeyCode)
			{
				num |= 0x40000;
			}
			if (KEYCONFIG.DPad.LEFT == e.KeyCode)
			{
				num |= 0x80000;
			}
			if (KEYCONFIG.DPad.DOWN == e.KeyCode)
			{
				num |= 0x10000;
			}
			if (KEYCONFIG.Button.START == e.KeyCode)
			{
				num |= 0x200;
			}
			if (KEYCONFIG.Button.SELECT == e.KeyCode)
			{
				num |= 0x100;
			}
			if (KEYCONFIG.Button.HOME == e.KeyCode)
			{
				num |= 0x1000;
			}
			if (KEYCONFIG.Button.CAPTURE == e.KeyCode)
			{
				num |= 0x2000;
			}
			if (KEYCONFIG.Button.CLICKL == e.KeyCode)
			{
				num |= 0x800;
			}
			if (KEYCONFIG.Button.CLICKR == e.KeyCode)
			{
				num |= 0x400;
			}
			if (KEYCONFIG.AnalogL.UP == e.KeyCode)
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0;
			}
			if (KEYCONFIG.AnalogL.DOWN == e.KeyCode)
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0xFF00000000L;
			}
			if (KEYCONFIG.AnalogL.LEFT == e.KeyCode)
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0;
			}
			if (KEYCONFIG.AnalogL.RIGHT == e.KeyCode)
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0xFF0000000000L;
			}
			if (KEYCONFIG.AnalogR.UP == e.KeyCode)
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0;
			}
			if (KEYCONFIG.AnalogR.DOWN == e.KeyCode)
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0xFF000000000000L;
			}
			if (KEYCONFIG.AnalogR.LEFT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0;
			}
			if (KEYCONFIG.AnalogR.RIGHT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0xFF00000000000000uL;
			}
		}
		if (e.UpDown == KeyboardUpDown.Up)
		{
			if (KEYCONFIG.Button.A == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFF7uL;
			}
			if (KEYCONFIG.Button.B == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFFBuL;
			}
			if (KEYCONFIG.Button.X == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFFDuL;
			}
			if (KEYCONFIG.Button.Y == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFFEuL;
			}
			if (KEYCONFIG.Button.ZL == e.KeyCode)
			{
				num &= 0xFFFFFFFFFF7FFFFFuL;
			}
			if (KEYCONFIG.Button.ZR == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFF7FuL;
			}
			if (KEYCONFIG.Button.L == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFBFFFFFuL;
			}
			if (KEYCONFIG.Button.R == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFBFuL;
			}
			if (KEYCONFIG.DPad.UP == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFDFFFFuL;
			}
			if (KEYCONFIG.DPad.RIGHT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFBFFFFuL;
			}
			if (KEYCONFIG.DPad.LEFT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFF7FFFFuL;
			}
			if (KEYCONFIG.DPad.DOWN == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFEFFFFuL;
			}
			if (KEYCONFIG.Button.START == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFDFFuL;
			}
			if (KEYCONFIG.Button.SELECT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFEFFuL;
			}
			if (KEYCONFIG.Button.HOME == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFEFFFuL;
			}
			if (KEYCONFIG.Button.CAPTURE == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFDFFFuL;
			}
			if (KEYCONFIG.Button.CLICKL == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFF7FFuL;
			}
			if (KEYCONFIG.Button.CLICKR == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFBFFuL;
			}
			if (KEYCONFIG.AnalogL.UP == e.KeyCode)
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0x8000000000L;
			}
			if (KEYCONFIG.AnalogL.DOWN == e.KeyCode)
			{
				num &= 0xFFFFFF00FFFFFFFFuL;
				num |= 0x8000000000L;
			}
			if (KEYCONFIG.AnalogL.LEFT == e.KeyCode)
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0x800000000000L;
			}
			if (KEYCONFIG.AnalogL.RIGHT == e.KeyCode)
			{
				num &= 0xFFFF00FFFFFFFFFFuL;
				num |= 0x800000000000L;
			}
			if (KEYCONFIG.AnalogR.UP == e.KeyCode)
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0x80000000000000L;
			}
			if (KEYCONFIG.AnalogR.DOWN == e.KeyCode)
			{
				num &= 0xFF00FFFFFFFFFFFFuL;
				num |= 0x80000000000000L;
			}
			if (KEYCONFIG.AnalogR.LEFT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0x8000000000000000uL;
			}
			if (KEYCONFIG.AnalogR.RIGHT == e.KeyCode)
			{
				num &= 0xFFFFFFFFFFFFFFL;
				num |= 0x8000000000000000uL;
			}
		}
		Nmc.KeyBoardKeyFlag = num;
	}

	private void mouseHook1_MouseHooked_1(object sender, MouseHookedEventArgs e)
	{
	}

	private void 接続ToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
	{
	}

	private void 設定ToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
	{
		接続ToolStripMenuItem.DropDownItems.Clear();
		接続ToolStripMenuItem.Text = "接続";
		if (_selectedPort != "")
		{
			接続ToolStripMenuItem.Text = "接続(" + _selectedPort + ")";
		}
		string[] portNames = SerialPort.GetPortNames();
		foreach (string text in portNames)
		{
			接続ToolStripMenuItem.DropDownItems.Add(text);
			接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].ForeColor = BZStyle.TextFont;
			接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].Name = text;
			if (_selectedPort == text)
			{
				((ToolStripMenuItem)接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1]).Checked = true;
			}
			接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].Click += delegate(object o, EventArgs args)
			{
				if (NxControllerInterface.StartedBluetooth)
				{
					NxControllerInterface.StartedBluetooth = false;
					NxControllerInterface.ShutdownGamepad();
				}
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)o;
				if (toolStripMenuItem.Checked)
				{
					if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
					{
						ComConnect.Image = Resources.B5;
					}
					else
					{
						ComConnect.Image = Resources.B5_L;
					}
					_selectedPort = "";
					try
					{
						if (_serialPort.IsOpen)
						{
							_serialPort.Close();
						}
						return;
					}
					catch (Exception)
					{
						_serialPort = new SerialPortStream();
						NxControllerInterface.SerialPort = _serialPort;
						return;
					}
				}
				toolStripMenuItem.Checked = true;
				ComConnect.Image = Resources.B5_LINK;
				_selectedPort = toolStripMenuItem.Name;
				ComPortList.Text = _selectedPort;
				try
				{
					if (_serialPort.IsOpen)
					{
						_serialPort.Close();
					}
				}
				catch (Exception)
				{
					_serialPort = new SerialPortStream();
					NxControllerInterface.SerialPort = _serialPort;
				}
				try
				{
					NxControllerInterface.OpenSerial(toolStripMenuItem.Name);
				}
				catch (Exception)
				{
				}
			};
		}
		接続ToolStripMenuItem.DropDownItems.Add("-");
		接続ToolStripMenuItem.DropDownItems.Add("Bluetooth無線接続");
		接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].ForeColor = BZStyle.TextFont;
		接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].Name = "Bluetooth";
		if (_selectedPort == "Bluetooth")
		{
			((ToolStripMenuItem)接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1]).Checked = true;
		}
		接続ToolStripMenuItem.DropDownItems[接続ToolStripMenuItem.DropDownItems.Count - 1].Click += delegate(object o, EventArgs args)
		{
			if (NxControllerInterface.StartedBluetooth)
			{
				NxControllerInterface.StartedBluetooth = false;
				NxControllerInterface.ShutdownGamepad();
			}
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)o;
			if (toolStripMenuItem.Checked)
			{
				if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
				{
					ComConnect.Image = Resources.B5;
				}
				else
				{
					ComConnect.Image = Resources.B5_L;
				}
				_selectedPort = "";
			}
			else
			{
				try
				{
					if (_serialPort.IsOpen)
					{
						_serialPort.Close();
					}
				}
				catch (Exception)
				{
					_serialPort = new SerialPortStream();
					NxControllerInterface.SerialPort = _serialPort;
				}
				Task.Factory.StartNew(delegate
				{
					NxControllerInterface.StartedBluetooth = true;
					NxControllerInterface.StartGamepad();
				});
				toolStripMenuItem.Checked = true;
				ComConnect.Image = Resources.B5_LINK;
				_selectedPort = toolStripMenuItem.Name;
				ComPortList.Text = "Bluetooth無線接続";
			}
		};
	}

	private void dropIcon1_Click(object sender, EventArgs e)
	{
	}

	private void tabPage3_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
		e.Effect = System.Windows.Forms.DragDropEffects.All;
	}

	private void flowLayoutPanel1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
		e.Effect = System.Windows.Forms.DragDropEffects.All;
	}

	private void dropIcon1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
		e.Effect = System.Windows.Forms.DragDropEffects.All;
	}

	private void flowLayoutPanel1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, autoConvert: false);
		foreach (string text in array)
		{
			try
			{
				switch (Path.GetExtension(text).ToLower())
				{
				case ".bmp":
				case ".jpg":
				case ".png":
				case ".tif":
				{
					Bitmap im = new Bitmap(text);
					List<string> list = GlobalVar.MAINFORM.Nmc.ResourcesImages.Select((ResourcesImage _) => _.label).ToList();
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
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
					break;
				}
				}
			}
			catch
			{
			}
		}
		ImageReload();
	}

	private void 画面をキャプチャToolStripMenuItem_Click(object sender, EventArgs e)
	{
		NxSel.X1 = -1;
		NxSel.X2 = -1;
		NxSel.Y1 = -1;
		NxSel.Y2 = -1;
		NxSel.Start = false;
		_captureMode = 0;
		_captureNow = true;
	}

	public void StartSnippingSetting()
	{
		if (CvCapture != null || DsCapture != null)
		{
			NxSel.X1 = -1;
			NxSel.X2 = -1;
			NxSel.Y1 = -1;
			NxSel.Y2 = -1;
			NxSel.Start = false;
			_captureMode = 1;
			_captureNow = true;
		}
	}

	private void CaptureScreen_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		if (CvCapture == null && DsCapture == null)
		{
			return;
		}
		int num = 1920;
		int num2 = 1080;
		if (DsCapture != null)
		{
			num = DsCapture.Width;
			num2 = DsCapture.Height;
		}
		int num3 = CaptureScreen.Width;
		int num4 = CaptureScreen.Height;
		double num5 = (double)num3 / (double)num;
		double num6 = (double)num4 / (double)num2;
		int num7 = 0;
		int num8 = 0;
		if (num6 > num5)
		{
			num8 = (int)(((double)num4 - (double)num2 * num5) / 2.0);
		}
		else
		{
			num7 = (int)(((double)num3 - (double)num * num6) / 2.0);
			num5 = num6;
		}
		if (DsCapture != null)
		{
			NxSel.X1 = Math.Min(Math.Max((int)((double)e.X / num5), 0), num - 1);
			NxSel.Y1 = Math.Min(Math.Max((int)((double)e.Y / num5), 0), num2 - 1);
			if ((int)((double)e.X / num5) < num && (int)((double)e.Y / num5) < num2)
			{
				Console.WriteLine($"MouseDown : X - {NxSel.X1} / Y - {NxSel.Y1}");
			}
		}
		else
		{
			NxSel.X1 = Math.Min(Math.Max((int)((double)(e.X - num7) / num5), 0), num - 1);
			NxSel.Y1 = Math.Min(Math.Max((int)((double)(e.Y - num8) / num5), 0), num2 - 1);
			if (e.X >= num7 && (int)((double)(e.X - num7) / num5) < num && e.Y >= num8 && (int)((double)(e.Y - num8) / num5) < num2)
			{
				Console.WriteLine($"MouseDown : X - {NxSel.X1} / Y - {NxSel.Y1}");
			}
		}
		NxSel.PicD = num5;
		NxSel.PicW = num7;
		NxSel.PicH = num8;
		NxSel.Start = true;
	}

	public Bitmap CaptureImage()
	{
		while (true)
		{
			try
			{
				if (NxCommand.CurrentFrame == null)
				{
					return null;
				}
				return (Bitmap)NxCommand.CurrentFrame.Clone();
			}
			catch (Exception)
			{
			}
		}
	}

	public async void SetCapImage()
	{
	}

	private byte crc8(byte[] data)
	{
		byte b = 0;
		for (int i = 0; i < data.Length; i++)
		{
			b = (byte)CRC_TABLE[data[i] ^ b];
		}
		return b;
	}

	public void Snipping(int x, int y, int width, int height)
	{
		if (DsCapture != null || CvCapture != null)
		{
			Bitmap bitmap;
			lock (NxCommand.lockObject)
			{
				bitmap = CaptureImage();
			}
			Rectangle rect = new Rectangle(x, y, width, height);
			Bitmap bitmap2 = bitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			if (!Directory.Exists(GlobalVar.CaptureOutput))
			{
				Directory.CreateDirectory(GlobalVar.BasePath + "Captures");
				GlobalVar.CaptureOutput = Path.GetFullPath(GlobalVar.BasePath + "Captures");
				Util.SaveConfig();
			}
			bitmap2.Save(GlobalVar.CaptureOutput + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-ffffff") + ".png", ImageFormat.Png);
		}
	}

	private void CaptureScreen_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		if (NxSel.X1 == -1 || !_captureNow)
		{
			return;
		}
		_captureNow = false;
		try
		{
			if (_captureMode == 0)
			{
				Snipping(Math.Min(NxSel.X1, NxSel.X2), Math.Min(NxSel.Y1, NxSel.Y2), Math.Abs(NxSel.X2 - NxSel.X1) + 1, Math.Abs(NxSel.Y2 - NxSel.Y1) + 1);
				return;
			}
			KeyInputSet("Snipping(" + Math.Min(NxSel.X1, NxSel.X2) + ", " + Math.Min(NxSel.Y1, NxSel.Y2) + ", " + (Math.Abs(NxSel.X2 - NxSel.X1) + 1) + ", " + (Math.Abs(NxSel.Y2 - NxSel.Y1) + 1) + ")", 0m, 0m, plF: true);
		}
		catch
		{
		}
	}

	private void CaptureContext_Opening(object sender, CancelEventArgs e)
	{
		if (_captureNow || (CvCapture == null && DsCapture == null))
		{
			画面をキャプチャToolStripMenuItem.Enabled = false;
		}
		else
		{
			画面をキャプチャToolStripMenuItem.Enabled = true;
		}
	}

	private void CaptureScreen_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		if (_captureNow && (CvCapture != null || DsCapture != null))
		{
			int num = 1920;
			int num2 = 1080;
			if (DsCapture != null)
			{
				num = DsCapture.Width;
				num2 = DsCapture.Height;
			}
			int num3 = CaptureScreen.Width;
			int num4 = CaptureScreen.Height;
			double num5 = (double)num3 / (double)num;
			double num6 = (double)num4 / (double)num2;
			int num7 = 0;
			int num8 = 0;
			if (num6 > num5)
			{
				num8 = (int)(((double)num4 - (double)num2 * num5) / 2.0);
			}
			else
			{
				num7 = (int)(((double)num3 - (double)num * num6) / 2.0);
				num5 = num6;
			}
			NxSel.X2 = Math.Min(Math.Max((int)((double)(e.X - num7) / num5), 0), num - 1);
			NxSel.Y2 = Math.Min(Math.Max((int)((double)(e.Y - num8) / num5), 0), num2 - 1);
		}
	}

	private void BTSetUpToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Bluetooth制御セットアップ bluetooth制御セットアップ = new Bluetooth制御セットアップ();
		bluetooth制御セットアップ.StartPosition = FormStartPosition.CenterParent;
		bluetooth制御セットアップ.ShowDialog();
	}

	private void 接続ToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void NXMC_VxV_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void NXMC_VxV_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		Keys keyCode = e.KeyCode;
		if ((uint)(keyCode - 37) <= 3u)
		{
			e.IsInputKey = true;
		}
	}

	private void CapDeviceList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
	}

	private void CapDeviceList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
	{
	}

	private void tabControl1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
	{
		Keys keyCode = e.KeyCode;
		if ((uint)(keyCode - 37) <= 3u)
		{
			e.Handled = true;
		}
	}

	private void button6_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		Keys keyCode = e.KeyCode;
		if ((uint)(keyCode - 37) <= 3u)
		{
			e.IsInputKey = true;
		}
	}

	private void button1_Click_2(object sender, EventArgs e)
	{
	}

	public void TaskView()
	{
		Task.Factory.StartNew(delegate
		{
			string taskname = "";
			string[] taskName = GlobalVar.TaskName;
			foreach (string text in taskName)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (taskname == "")
					{
						taskname = text;
					}
					else
					{
						taskname = taskname + "   /   " + text;
					}
				}
			}
			Invoke((Action)delegate
			{
				label2.Text = taskname;
			});
		});
	}

	public void TaskViewLite()
	{
		if (_lastTaskView.ElapsedMilliseconds >= 32)
		{
			_lastTaskView.Restart();
			TaskView();
		}
	}

	private void マクロの保存ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.InitialDirectory = Path.GetFullPath(GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text);
		saveFileDialog.Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			Nmc.Code = CodeEdit.Text;
			byte[] fileData = Nmc.GetFileData();
			if (File.Exists(saveFileDialog.FileName))
			{
				File.Delete(saveFileDialog.FileName);
			}
			File.WriteAllBytes(saveFileDialog.FileName, fileData);
			Nmc.FilePath = Path.GetFullPath(Path.GetDirectoryName(saveFileDialog.FileName)) + "\\";
			Nmc.AllPath = saveFileDialog.FileName;
			Text = GlobalVar.AppName + " - " + Path.GetFileName(saveFileDialog.FileName);
			マクロを上書き保存ToolStripMenuItem.Enabled = true;
			flowLayoutPanel3.Enabled = true;
			flowLayoutPanel3.Visible = true;
			SetMacroDirectory(saveFileDialog.FileName);
			GlobalVar.TaskName[0] = "ファイルが保存されました";
			GlobalVar.MAINFORM.TaskView();
			macroDirReload();
		}
	}

	private void バージョン情報ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		AppInfoDialog appInfoDialog = new AppInfoDialog();
		appInfoDialog.StartPosition = FormStartPosition.CenterParent;
		appInfoDialog.ShowDialog();
	}

	private void button1_Click_3(object sender, EventArgs e)
	{
		panel2.Location = new System.Drawing.Point(base.ActualLeft, base.ActualTop);
		panel2.Size = new System.Drawing.Size(base.ActualWidth, base.ActualHeight);
	}

	private void NXMC_VxV_Resize(object sender, EventArgs e)
	{
		panel2.Location = new System.Drawing.Point(base.ActualLeft, base.ActualTop);
		panel2.Size = new System.Drawing.Size(base.ActualWidth, base.ActualHeight);
	}

	private void NXMC_VxV_SizeChanged(object sender, EventArgs e)
	{
		panel2.Location = new System.Drawing.Point(base.ActualLeft, base.ActualTop);
		panel2.Size = new System.Drawing.Size(base.ActualWidth, base.ActualHeight);
	}

	private async void buttonEx1_Click(object sender, EventArgs e)
	{
		Python.CreateEngine().CreateScriptSourceFromString("print(\"Hello World!\")").Execute();
	}

	private void SetTheme(BZComponent.Style style)
	{
		BZStyle.SetStyle(style);
		foreach (ToolStripMenuItem item in menuStrip1.Items)
		{
			item.ForeColor = BZStyle.TextFont;
			foreach (ToolStripItem dropDownItem in item.DropDownItems)
			{
				dropDownItem.ForeColor = BZStyle.TextFont;
			}
		}
		elementHost1.BackColor = BZStyle.NormalColor;
		panel1.BackColor = BZStyle.NormalColor;
		textBox1.BackColor = BZStyle.BackColor;
		textBox1.ForeColor = BZStyle.TextFont;
		groupBoxEx1.BackColor = BZStyle.BackColor;
		ghostPanel7.BackColor = BZStyle.NormalColor;
		CaptureScreen.BackColor = BZStyle.BackColor;
		CapConnect.ForeColor = BZStyle.TextFont;
		ComConnect.ForeColor = BZStyle.TextFont;
		this.buttonEx2.ForeColor = BZStyle.TextFont;
		button3.ForeColor = BZStyle.TextFont;
		button4.ForeColor = BZStyle.TextFont;
		button6.ForeColor = BZStyle.TextFont;
		CodeEdit.Background = BZStyle.BackColor.ToBrush();
		CodeEdit.Foreground = BZStyle.TextFont.ToBrush();
		CodeEdit.BorderBrush = BZStyle.NormalColor.ToBrush();
		CodeEdit.TextArea.TextView.BackgroundRenderers.Add(new HighLightLine(System.Drawing.Color.Khaki, BZStyle.NormalColor));
		CodeEdit.Options.HighlightCurrentLine = true;
		CodeEdit.Options.ShowEndOfLine = false;
		CodeEdit.TextArea.TextView.CurrentLineBackground = BZStyle.DarkColor.ToBrush();
		CodeEdit.TextArea.TextView.CurrentLineBorder = new System.Windows.Media.Pen(BZStyle.ForeColor.ToBrush(), 1.0);
		MacroItemTheme(style);
		if (style == BZComponent.Style.Dark)
		{
			IHighlightingDefinition syntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(Resources.NX_D))), HighlightingManager.Instance);
			CodeEdit.SyntaxHighlighting = syntaxHighlighting;
			CodeEdit.LineNumbersForeground = System.Drawing.Color.SteelBlue.ToBrush();
			HighLightLine.HighLightSet(BZStyle.NormalColor, BZStyle.NormalColor);
			if (!Nmc.Running)
			{
				ButtonEx buttonEx = button6;
				System.Drawing.Image image = (buttonEx5.Image = Resources.B1);
				buttonEx.Image = image;
			}
			if (CapDeviceList.Enabled)
			{
				CapConnect.Image = Resources.B3;
			}
			if (ComPortList.Enabled)
			{
				ComConnect.Image = Resources.B5;
			}
			if (this.buttonEx2.Text != "読込")
			{
				if (this.buttonEx2.Text != "実行")
				{
					this.buttonEx2.Image = Resources.B1;
				}
			}
			else
			{
				this.buttonEx2.Image = Resources.B6;
			}
			buttonEx3.Image = Resources.B7;
			IconSet(Resources.iconD);
			System.Drawing.Color color = System.Drawing.Color.FromArgb(37, 37, 38);
			NxControllerInterface.SendPadcolor(System.Drawing.Color.FromArgb(65, 65, 67), System.Drawing.Color.FromArgb(28, 151, 234), color, color);
		}
		else
		{
			IHighlightingDefinition syntaxHighlighting2 = HighlightingLoader.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(Resources.NX))), HighlightingManager.Instance);
			CodeEdit.SyntaxHighlighting = syntaxHighlighting2;
			CodeEdit.LineNumbersForeground = System.Drawing.Color.SteelBlue.ToBrush();
			HighLightLine.HighLightSet(BZStyle.NormalColor, BZStyle.NormalColor);
			if (!Nmc.Running)
			{
				ButtonEx buttonEx2 = button6;
				System.Drawing.Image image = (buttonEx5.Image = Resources.B1_L);
				buttonEx2.Image = image;
			}
			if (CapDeviceList.Enabled)
			{
				CapConnect.Image = Resources.B3_L;
			}
			if (ComPortList.Enabled)
			{
				ComConnect.Image = Resources.B5_L;
			}
			if (this.buttonEx2.Text != "読込")
			{
				if (this.buttonEx2.Text != "実行")
				{
					this.buttonEx2.Image = Resources.B1_L;
				}
			}
			else
			{
				this.buttonEx2.Image = Resources.B6_L;
			}
			buttonEx3.Image = Resources.B7_L;
			IconSet(Resources.iconL);
			System.Drawing.Color color2 = System.Drawing.Color.FromArgb(167, 167, 179);
			NxControllerInterface.SendPadcolor(System.Drawing.Color.FromArgb(222, 222, 235), System.Drawing.Color.FromArgb(0, 122, 204), color2, color2);
		}
		MacroShortCutReload();
		ImageReload();
		DataFileReload();
		Refresh();
	}

	private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
	{
	}

	private void tabPage2_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void flowLayoutPanel1_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void menuStrip1_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void CaptureScreen_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void label2_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void マクロ共有サーバーに接続ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!MacroShare.Opened)
		{
			MacroShare obj = new MacroShare
			{
				StartPosition = FormStartPosition.CenterParent
			};
			MacroShare.Opened = true;
			obj.Show();
		}
	}

	private void keyboardHook2_KeyboardHooked(object sender, KeyboardHookedEventArgs e)
	{
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (!textBox1.IsDisposed && textBox1 != null && mtw != null)
		{
			string allText = mtw.allText;
			mtw.allText = "";
			if (allText != "")
			{
				textBox1.AppendText(allText);
			}
			if (!scrollBarEx1.IsDisposed && scrollBarEx1 != null)
			{
				int num = (int)((double)scrollBarEx1.Value / 100.0 / (double)TextBoxUTL.GetLineHeight(textBox1));
				int ypos = TextBoxUTL.GetYpos(textBox1);
				if (num != ypos)
				{
					scrollBarEx1.Value = ypos * 100 * TextBoxUTL.GetLineHeight(textBox1);
				}
			}
			scrollBarEx2.Visible = flowLayoutPanel2.VerticalScroll.Visible;
			if (flowLayoutPanel2.VerticalScroll.Maximum >= flowLayoutPanel2.Height)
			{
				scrollBarEx2.Tag = "update";
				scrollBarEx2.Value = flowLayoutPanel2.VerticalScroll.Value * scrollBarEx2.Maximum / (flowLayoutPanel2.VerticalScroll.Maximum - flowLayoutPanel2.Height);
				scrollBarEx2.Tag = "";
			}
			scrollBarEx3.Visible = flowLayoutPanel1.VerticalScroll.Visible;
			if (flowLayoutPanel1.VerticalScroll.Maximum >= flowLayoutPanel1.Height)
			{
				scrollBarEx3.Tag = "update";
				scrollBarEx3.Value = flowLayoutPanel1.VerticalScroll.Value * scrollBarEx3.Maximum / (flowLayoutPanel1.VerticalScroll.Maximum - flowLayoutPanel1.Height);
				scrollBarEx3.Tag = "";
			}
			scrollBarEx4.Visible = flowLayoutPanel3.VerticalScroll.Visible;
			if (flowLayoutPanel3.VerticalScroll.Maximum >= flowLayoutPanel3.Height)
			{
				scrollBarEx4.Tag = "update";
				scrollBarEx4.Value = flowLayoutPanel3.VerticalScroll.Value * scrollBarEx4.Maximum / (flowLayoutPanel3.VerticalScroll.Maximum - flowLayoutPanel3.Height);
				scrollBarEx4.Tag = "";
			}
			tabPage1.PerformLayout();
			tabPage3.PerformLayout();
		}
		string[] portNames = SerialPort.GetPortNames();
		if (portNames.Length != _portsBuffer.Length)
		{
			comboBoxEx2_Enter(null, null);
			_portsBuffer = portNames;
		}
		else
		{
			for (int i = 0; i < portNames.Length; i++)
			{
				if (portNames[i] != _portsBuffer[i])
				{
					comboBoxEx2_Enter(null, null);
					_portsBuffer = portNames;
					break;
				}
			}
		}
		if (macroDataChanged)
		{
			macroDataChanged = false;
			DataFileReload();
		}
	}

	public void UpdateHighLightLite()
	{
		if (_lastHighlight.ElapsedMilliseconds >= 16)
		{
			_lastHighlight.Restart();
			UpdateHighLight();
		}
	}

	public void UpdateHighLight()
	{
		Task.Factory.StartNew(delegate
		{
			if (GlobalVar.HighLightLine != _highLightLine)
			{
				Invoke((Action)delegate
				{
					HighLightLine.HighLightLineSet(GlobalVar.HighLightLine);
					_highLightLine = GlobalVar.HighLightLine;
					if (KEYCONFIG.EditorConfig.RUNNINGFOCUS)
					{
						CodeEdit.ScrollToLine(GlobalVar.HighLightLine);
					}
					CodeEdit.TextArea.TextView.Redraw();
				});
			}
		});
	}

	private void flowLayoutPanel2_MouseEnter(object sender, EventArgs e)
	{
	}

	private void flowLayoutPanel2_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
		Util.WriteLine("DragEnter");
		e.Effect = System.Windows.Forms.DragDropEffects.All;
	}

	private void flowLayoutPanel2_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, autoConvert: false);
		foreach (string text in array)
		{
			try
			{
				if (Util.MacroDataCheckOffline(File.ReadAllBytes(text)) && !GlobalVar.MacroList.Contains(text))
				{
					GlobalVar.MacroList.Add(text);
				}
			}
			catch
			{
			}
		}
		MacroShortCutReload();
	}

	private void MacroItemTheme(BZComponent.Style style)
	{
		foreach (object control in flowLayoutPanel2.Controls)
		{
			try
			{
				((MacroItem)control).ThemeChange(style);
			}
			catch (Exception)
			{
			}
		}
	}

	private void FileItemTheme(BZComponent.Style style)
	{
		foreach (object control in flowLayoutPanel3.Controls)
		{
			try
			{
				((FileResItem)control).SetTheme(style);
			}
			catch (Exception)
			{
			}
		}
	}

	private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Process.Start(GlobalVar.BasePath + "Readme.txt");
	}

	private void 全画面キャプチャToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (CurrentCaptureFormat == CaptureStyle.DirectShow)
		{
			Snipping(0, 0, DsCapture.Width, DsCapture.Height);
		}
		else
		{
			Snipping(0, 0, 1920, 1080);
		}
	}

	private void ヘルプToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (!HelpDialog.Opening)
		{
			new HelpDialog().Show(this);
		}
	}

	private DialogResult UpdateCheckDialog()
	{
		return cTaskDialog.ShowTaskDialogBox(this, "更新の確認", "", "新しいバージョンが公開されています。\r\nアプリケーションを終了して公開元のページを開きますか？", "", "", "今後、このメッセージを表示しない", "", "", eTaskDialogButtons.YesNo, eSysIcons.Information, eSysIcons.Information);
	}

	private void NXMC_VxV_Shown(object sender, EventArgs e)
	{
		if (KEYCONFIG.AppConfig.UPDATECHECK && Util.UpdateCheck())
		{
			Invoke((Action)delegate
			{
				DialogResult num2 = UpdateCheckDialog();
				KEYCONFIG.AppConfig.UPDATECHECK = !cTaskDialog.VerificationChecked;
				if (num2 == DialogResult.Yes)
				{
					Process.Start("https://blog.bzl-web.com/entry/2090/11/11/000000#NX-Macro-Controller");
					Close();
				}
			});
		}
		try
		{
			if (GlobalVar.Ver <= GlobalVar.LastVer)
			{
				return;
			}
			string[] array = File.ReadAllLines(GlobalVar.BasePath + "Readme.txt");
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text == "■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■")
				{
					flag = false;
				}
				if (text == "●更新履歴" || flag)
				{
					stringBuilder.AppendLine(text);
					flag = true;
				}
			}
			ChLog chLog = new ChLog();
			chLog.Log = stringBuilder.ToString().Trim();
			chLog.StartPosition = FormStartPosition.CenterParent;
			chLog.ShowDialog();
		}
		catch
		{
		}
	}

	private void NXMC_VxV_Activated(object sender, EventArgs e)
	{
	}

	private void amiibo読込ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Amiiboデータ(*.bin)|*.bin|すべてのファイル(*.*)|*.*";
		if (openFileDialog.ShowDialog() == DialogResult.OK && File.ReadAllBytes(openFileDialog.FileName).Length <= 540)
		{
			_amiibo = openFileDialog.FileName;
			NxControllerInterface.SendAmiibo(_amiibo);
		}
	}

	private void CaptureScreen_SizeChanged(object sender, EventArgs e)
	{
		captureRun = false;
		if (DsCapture != null)
		{
			DsCapture.renderingSize = CaptureScreen.ClientSize;
		}
	}

	private void pictureBox2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
	{
	}

	private void groupBox1_Enter(object sender, EventArgs e)
	{
	}

	private void ghostPanel5_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void NXMC_VxV_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void panel2_Click(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void mouseHook1_MouseHooked_2(object sender, MouseHookedEventArgs e)
	{
		if (Form.ActiveForm != this || (e.Message != MouseMessage.LDown && e.Message != MouseMessage.RDown && e.Message != MouseMessage.MDown))
		{
			return;
		}
		System.Drawing.Point point = CaptureScreen.PointToClient(e.Point);
		if (point.X >= 0 && point.Y >= 0 && point.X <= CaptureScreen.Width && point.Y <= CaptureScreen.Height)
		{
			if (DsCapture != null)
			{
				double num = (double)DsCapture.Width / (double)CaptureScreen.Width;
				double num2 = (double)DsCapture.Height / (double)CaptureScreen.Height;
				_ = point.X;
				_ = point.Y;
			}
			if (base.ActiveControl != CapDeviceList && base.ActiveControl != ComPortList && base.ActiveControl != macroSelCmb && base.ActiveControl != macroSubDirCmb)
			{
				Focus();
				base.ActiveControl = null;
			}
		}
	}

	private void CapDeviceList_SelectedIndexChanged(object sender, EventArgs e)
	{
		Focus();
		base.ActiveControl = null;
	}

	private void button1_Click_4(object sender, EventArgs e)
	{
	}

	private void ghostPanel6_Paint(object sender, PaintEventArgs e)
	{
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		int textSize = TextBoxUTL.GetTextSize(textBox1);
		if (textBox1.Height >= textSize)
		{
			scrollBarEx1.Maximum = 0;
		}
		else
		{
			scrollBarEx1.Maximum = (textSize - textBox1.Height) * 100;
		}
		int num = (int)((double)scrollBarEx1.Value / 100.0 / (double)TextBoxUTL.GetLineHeight(textBox1));
		int ypos = TextBoxUTL.GetYpos(textBox1);
		if (num != ypos)
		{
			scrollBarEx1.Value = ypos * 100 * TextBoxUTL.GetLineHeight(textBox1);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
	}

	private void scrollBarEx1_Scroll(object sender, ScrollEventArgs e)
	{
		if (scrollBarEx1.Value == scrollBarEx1.Maximum)
		{
			TextBoxUTL.SetYpos(textBox1, scrollBarEx1.Value / 100 / TextBoxUTL.GetLineHeight(textBox1) + 1);
		}
		else
		{
			TextBoxUTL.SetYpos(textBox1, scrollBarEx1.Value / 100 / TextBoxUTL.GetLineHeight(textBox1));
		}
	}

	private void textBox1_Layout(object sender, LayoutEventArgs e)
	{
	}

	private void tabControl1_SizeChanged(object sender, EventArgs e)
	{
	}

	private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void comboBoxEx2_Click(object sender, EventArgs e)
	{
	}

	private void comboBoxEx2_Enter(object sender, EventArgs e)
	{
		string text = ComPortList.Text;
		bool flag = false;
		ComPortList.BeginUpdate();
		ComPortList.Items.Clear();
		ComPortList.Items.Add("接続先ポートを選択");
		string[] portNames = SerialPort.GetPortNames();
		for (int i = 0; i < portNames.Length; i++)
		{
			ComPortList.Items.Add(portNames[i]);
			if (text == portNames[i])
			{
				flag = true;
			}
		}
		ComPortList.Items.Add("Bluetooth無線接続");
		ComPortList.EndUpdate();
		ComPortList.Text = text;
		if (ComPortList.Enabled || flag)
		{
			return;
		}
		if (NxControllerInterface.StartedBluetooth)
		{
			NxControllerInterface.StartedBluetooth = false;
			NxControllerInterface.ShutdownGamepad();
		}
		try
		{
			if (_serialPort.IsOpen)
			{
				_serialPort.Close();
			}
			_serialPort = new SerialPortStream();
			NxControllerInterface.SerialPort = _serialPort;
		}
		catch (Exception)
		{
			_serialPort = new SerialPortStream();
			NxControllerInterface.SerialPort = _serialPort;
		}
		if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
		{
			ComConnect.Image = Resources.B5;
		}
		else
		{
			ComConnect.Image = Resources.B5_L;
		}
		_selectedPort = "";
		ComPortList.Enabled = true;
	}

	private void CapDeviceList_Enter(object sender, EventArgs e)
	{
	}

	private void ComConnect_Click(object sender, EventArgs e)
	{
		string text = ComPortList.Text;
		if (text == "Bluetooth無線接続")
		{
			if (NxControllerInterface.StartedBluetooth)
			{
				NxControllerInterface.StartedBluetooth = false;
				NxControllerInterface.ShutdownGamepad();
				if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
				{
					ComConnect.Image = Resources.B5;
				}
				else
				{
					ComConnect.Image = Resources.B5_L;
				}
				_selectedPort = "";
				ComPortList.Enabled = true;
				return;
			}
			try
			{
				if (_serialPort.IsOpen)
				{
					_serialPort.Close();
				}
			}
			catch (Exception)
			{
				_serialPort = new SerialPortStream();
				NxControllerInterface.SerialPort = _serialPort;
			}
			Task.Factory.StartNew(delegate
			{
				NxControllerInterface.StartedBluetooth = true;
				NxControllerInterface.StartGamepad();
			});
			ComConnect.Image = Resources.B5_LINK;
			_selectedPort = "Bluetooth";
			ComPortList.Enabled = false;
			return;
		}
		if (ComPortList.SelectedIndex > 0)
		{
			if (NxControllerInterface.StartedBluetooth)
			{
				NxControllerInterface.StartedBluetooth = false;
				NxControllerInterface.ShutdownGamepad();
			}
			if (text == _selectedPort)
			{
				if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
				{
					ComConnect.Image = Resources.B5;
				}
				else
				{
					ComConnect.Image = Resources.B5_L;
				}
				_selectedPort = "";
				ComPortList.Enabled = true;
				try
				{
					if (_serialPort.IsOpen)
					{
						_serialPort.Close();
					}
					_serialPort = new SerialPortStream();
					NxControllerInterface.SerialPort = _serialPort;
					return;
				}
				catch (Exception)
				{
					_serialPort = new SerialPortStream();
					NxControllerInterface.SerialPort = _serialPort;
					return;
				}
			}
			ComConnect.Image = Resources.B5_LINK;
			_selectedPort = text;
			ComPortList.Enabled = false;
			try
			{
				if (_serialPort.IsOpen)
				{
					_serialPort.Close();
				}
			}
			catch (Exception)
			{
				_serialPort = new SerialPortStream();
				NxControllerInterface.SerialPort = _serialPort;
			}
			try
			{
				NxControllerInterface.OpenSerial(text);
				return;
			}
			catch (Exception)
			{
				return;
			}
		}
		if (NxControllerInterface.StartedBluetooth)
		{
			NxControllerInterface.StartedBluetooth = false;
			NxControllerInterface.ShutdownGamepad();
		}
		try
		{
			if (_serialPort.IsOpen)
			{
				_serialPort.Close();
			}
		}
		catch (Exception)
		{
			_serialPort = new SerialPortStream();
			NxControllerInterface.SerialPort = _serialPort;
		}
		if (KEYCONFIG.AppConfig.APPTHEME == BZComponent.Style.Dark)
		{
			ComConnect.Image = Resources.B5;
		}
		else
		{
			ComConnect.Image = Resources.B5_L;
		}
		_selectedPort = "";
		ComPortList.Enabled = true;
	}

	private void ComPortList_DropDown(object sender, EventArgs e)
	{
	}

	private void buttonEx3_Click(object sender, EventArgs e)
	{
		Process.Start("EXPLORER.EXE", GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text);
	}

	private void NXMC_VxV_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
		e.Effect = System.Windows.Forms.DragDropEffects.All;
	}

	private void ghostPanel5_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
	}

	private void panel5_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
	{
	}

	private void flowLayoutPanel2_Scroll(object sender, ScrollEventArgs e)
	{
	}

	private void scrollBarEx2_Scroll(object sender, ScrollEventArgs e)
	{
		if ((scrollBarEx2.Tag == null || !(scrollBarEx2.Tag.ToString() == "update")) && flowLayoutPanel2.VerticalScroll.Maximum >= flowLayoutPanel2.Height)
		{
			flowLayoutPanel2.VerticalScroll.Value = (int)((double)scrollBarEx2.Value / (double)scrollBarEx2.Maximum * (double)(flowLayoutPanel2.VerticalScroll.Maximum - flowLayoutPanel2.Height));
		}
	}

	private void flowLayoutPanel2_Resize(object sender, EventArgs e)
	{
		scrollBarEx2.Maximum = Math.Max(0, (flowLayoutPanel2.VerticalScroll.Maximum - flowLayoutPanel2.Height) * 100);
		scrollBarEx2.Visible = flowLayoutPanel2.VerticalScroll.Visible;
	}

	private void flowLayoutPanel1_Resize(object sender, EventArgs e)
	{
		scrollBarEx3.Maximum = Math.Max(0, (flowLayoutPanel1.VerticalScroll.Maximum - flowLayoutPanel1.Height) * 100);
		scrollBarEx3.Visible = flowLayoutPanel1.VerticalScroll.Visible;
	}

	private void scrollBarEx3_Scroll(object sender, ScrollEventArgs e)
	{
		if ((scrollBarEx3.Tag == null || !(scrollBarEx3.Tag.ToString() == "update")) && flowLayoutPanel1.VerticalScroll.Maximum >= flowLayoutPanel1.Height)
		{
			flowLayoutPanel1.VerticalScroll.Value = (int)((double)scrollBarEx3.Value / (double)scrollBarEx3.Maximum * (double)(flowLayoutPanel1.VerticalScroll.Maximum - flowLayoutPanel1.Height));
		}
	}

	private void scrollBarEx4_Scroll(object sender, ScrollEventArgs e)
	{
		if ((scrollBarEx4.Tag == null || !(scrollBarEx4.Tag.ToString() == "update")) && flowLayoutPanel3.VerticalScroll.Maximum >= flowLayoutPanel3.Height)
		{
			flowLayoutPanel3.VerticalScroll.Value = (int)((double)scrollBarEx4.Value / (double)scrollBarEx4.Maximum * (double)(flowLayoutPanel3.VerticalScroll.Maximum - flowLayoutPanel3.Height));
		}
	}

	private void flowLayoutPanel3_Resize(object sender, EventArgs e)
	{
		scrollBarEx4.Maximum = Math.Max(0, (flowLayoutPanel3.VerticalScroll.Maximum - flowLayoutPanel3.Height) * 100);
		scrollBarEx4.Visible = flowLayoutPanel3.VerticalScroll.Visible;
	}

	private void マクロを上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Nmc.Code = CodeEdit.Text;
		byte[] fileData = Nmc.GetFileData();
		File.WriteAllBytes(Nmc.AllPath, fileData);
		GlobalVar.TaskName[0] = "ファイルが保存されました";
		GlobalVar.MAINFORM.TaskView();
	}

	private void NXMC_VxV_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, autoConvert: false);
		try
		{
			string text = Path.GetExtension(array[0]).ToLower();
			if (text == ".nxc" || text == ".nmc")
			{
				Nmc.NMCRead(array[0]);
				Text = GlobalVar.AppName + " - " + Path.GetFileName(array[0]);
				CodeEdit.TextArea.Document.BeginUpdate();
				CodeEdit.TextArea.Document.Text = Nmc.Code;
				CodeEdit.TextArea.Document.EndUpdate();
				マクロを上書き保存ToolStripMenuItem.Enabled = true;
				flowLayoutPanel3.Enabled = true;
				flowLayoutPanel3.Visible = true;
				SetMacroDirectory(array[0]);
				ImageReload();
				DataFileReload();
			}
		}
		catch
		{
		}
	}

	private void flowLayoutPanel3_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, autoConvert: false);
		foreach (string path in array)
		{
			try
			{
				if (Util.GetFilePathType(path) == Util.FilePathType.File)
				{
					string text = Path.GetFileName(path);
					if (CurrentDirectory != "")
					{
						text = CurrentDirectory + text;
					}
					text = Nmc.GetDataPath(text);
					if (!Directory.Exists(GlobalVar.MAINFORM.MacroDirectory))
					{
						Directory.CreateDirectory(GlobalVar.MAINFORM.MacroDirectory);
						GlobalVar.MAINFORM.FSWReload();
					}
					Directory.CreateDirectory(Path.GetDirectoryName(text));
					File.WriteAllBytes(text, File.ReadAllBytes(path));
				}
				else
				{
					if (Util.GetFilePathType(path) != Util.FilePathType.Directory)
					{
						continue;
					}
					string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
					for (int j = 0; j < files.Length; j++)
					{
						string text2 = Util.GetRelativePath(Path.GetDirectoryName(path), files[j]).Substring(2);
						if (CurrentDirectory != "")
						{
							text2 = CurrentDirectory + text2;
						}
						if (!Directory.Exists(GlobalVar.MAINFORM.MacroDirectory))
						{
							Directory.CreateDirectory(GlobalVar.MAINFORM.MacroDirectory);
							GlobalVar.MAINFORM.FSWReload();
						}
						text2 = Nmc.GetDataPath(text2);
						Directory.CreateDirectory(Path.GetDirectoryName(text2));
						File.WriteAllBytes(text2, File.ReadAllBytes(files[j]));
					}
					continue;
				}
			}
			catch
			{
			}
		}
	}

	private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
	{
		macroDataChanged = true;
	}

	private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
	{
		macroDataChanged = true;
	}

	private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
	{
		macroDataChanged = true;
	}

	private void fileSystemWatcher2_Renamed(object sender, RenamedEventArgs e)
	{
		string fullPath = Path.GetFullPath(Path.GetDirectoryName(MacroDirectory));
		if (e.OldFullPath == fullPath)
		{
			macroDataChanged = true;
		}
	}

	private void fileSystemWatcher2_Deleted(object sender, FileSystemEventArgs e)
	{
		Util.WriteLine("fsw2_delete");
		string fullPath = Path.GetFullPath(Path.GetDirectoryName(MacroDirectory));
		if (Path.GetFullPath(e.FullPath) == fullPath)
		{
			macroDataChanged = true;
		}
	}

	private void CaptureScreen_MouseEnter(object sender, EventArgs e)
	{
	}

	private void CaptureScreen_MouseLeave(object sender, EventArgs e)
	{
	}

	private void fileSystemWatcher3_Deleted(object sender, FileSystemEventArgs e)
	{
		string fullPath = Path.GetFullPath(Path.GetDirectoryName(MacroDirectory + CurrentDirectory));
		if (e.FullPath == fullPath)
		{
			CurrentDirectory = "";
			macroDataChanged = true;
		}
	}

	private void fileSystemWatcher3_Renamed(object sender, RenamedEventArgs e)
	{
		string fullPath = Path.GetFullPath(Path.GetDirectoryName(MacroDirectory + CurrentDirectory));
		if (e.OldFullPath == fullPath)
		{
			CurrentDirectory = "";
			macroDataChanged = true;
		}
	}

	private void NXMC_VxV_ResizeBegin(object sender, EventArgs e)
	{
		SuspendLayout();
	}

	private void NXMC_VxV_ResizeEnd(object sender, EventArgs e)
	{
		ResumeLayout();
	}

	private void CaptureScreen_Paint(object sender, PaintEventArgs e)
	{
		if (CurrentCaptureFormat == CaptureStyle.OpenCV)
		{
			Bitmap bitmap = captureScreenBuffer;
			if (bitmap != null)
			{
				int num = (CaptureScreen.Width - captureScreenBuffer.Width) / 2;
				int num2 = (CaptureScreen.Height - captureScreenBuffer.Height) / 2;
				e.Graphics.DrawImage(bitmap, num, num2);
			}
		}
	}

	private void macroSubDirCmb_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (macroSubDirCmb.Tag != null)
		{
			return;
		}
		macroListReload();
		fileSystemWatcher5.EnableRaisingEvents = false;
		fileSystemWatcher5.Dispose();
		fileSystemWatcher5 = new FileSystemWatcher(GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text);
		fileSystemWatcher5.Filter = "*";
		fileSystemWatcher5.IncludeSubdirectories = false;
		fileSystemWatcher5.EnableRaisingEvents = true;
		fileSystemWatcher5.Created += delegate
		{
			Invoke((Action)delegate
			{
				macroListReload();
			});
		};
		fileSystemWatcher5.Deleted += delegate
		{
			Invoke((Action)delegate
			{
				macroListReload();
			});
		};
		fileSystemWatcher5.Renamed += delegate
		{
			Invoke((Action)delegate
			{
				macroListReload();
			});
		};
	}

	private void macroListReload()
	{
		macroSubDirCmb.Tag = "update";
		string text = macroSelCmb.Text;
		macroSelCmb.BeginUpdate();
		macroSelCmb.Items.Clear();
		if (macroSubDirCmb.SelectedIndex == macroSubDirCmb.Items.Count - 1)
		{
			pokeconScriptFiles.Clear();
			string[] files = Directory.GetFiles(Path.GetFullPath(GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text), "*", SearchOption.TopDirectoryOnly);
			buttonEx2.Text = "実行";
			if (base.FormTheme == BZComponent.Style.Dark)
			{
				buttonEx2.Image = Resources.B1;
			}
			else
			{
				buttonEx2.Image = Resources.B1_L;
			}
			string[] array = files;
			foreach (string text2 in array)
			{
				if (!(Path.GetExtension(text2) == ".py"))
				{
					continue;
				}
				string item = "";
				string[] array2 = File.ReadAllLines(text2);
				for (int j = 0; j < array2.Length; j++)
				{
					string text3 = array2[j].Trim();
					if (text3.Length > 6 && text3.Substring(0, 4) == "NAME")
					{
						int num = text3.IndexOf('\'');
						if (num != -1)
						{
							int num2 = text3.LastIndexOf('\'');
							item = text3.Substring(num + 1, num2 - num - 1);
							break;
						}
						int num3 = text3.IndexOf('"');
						if (num3 != -1)
						{
							int num4 = text3.LastIndexOf('"');
							item = text3.Substring(num3 + 1, num4 - num3 - 1);
							break;
						}
					}
				}
				pokeconScriptFiles.Add(text2);
				macroSelCmb.Items.Add(item);
			}
		}
		else
		{
			string[] files2 = Directory.GetFiles(Path.GetFullPath(GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text), "*", SearchOption.TopDirectoryOnly);
			buttonEx2.Text = "読込";
			if (base.FormTheme == BZComponent.Style.Dark)
			{
				buttonEx2.Image = Resources.B6;
			}
			else
			{
				buttonEx2.Image = Resources.B6_L;
			}
			string[] array = files2;
			foreach (string path in array)
			{
				string extension = Path.GetExtension(path);
				if (extension == ".nxc" || extension == ".nmc")
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
					macroSelCmb.Items.Add(fileNameWithoutExtension);
				}
			}
		}
		if (macroSelCmb.Items.Count == 0)
		{
			macroSelCmb.Items.Add("-");
		}
		macroSelCmb.Text = text;
		macroSubDirCmb.Tag = null;
		macroSelCmb.EndUpdate();
	}

	private void macroDirReload()
	{
		macroSubDirCmb.Tag = "update";
		if (!Directory.Exists(GlobalVar.BasePath + "Macro"))
		{
			Directory.CreateDirectory(GlobalVar.BasePath + "Macro");
		}
		if (!Directory.Exists(GlobalVar.BasePath + "Macro\\Default"))
		{
			Directory.CreateDirectory(GlobalVar.BasePath + "Macro\\Default");
		}
		if (!Directory.Exists(GlobalVar.BasePath + "Macro\\Poke-Controller"))
		{
			Directory.CreateDirectory(GlobalVar.BasePath + "Macro\\Poke-Controller");
		}
		if (!Directory.Exists(GlobalVar.BasePath + "Macro\\Poke-Controller\\Template"))
		{
			Directory.CreateDirectory(GlobalVar.BasePath + "Macro\\Poke-Controller\\Template");
		}
		string[] directories = Directory.GetDirectories(GlobalVar.BasePath + "Macro", "*", SearchOption.TopDirectoryOnly);
		string text = macroSubDirCmb.Text;
		macroSubDirCmb.BeginUpdate();
		macroSubDirCmb.Items.Clear();
		macroSubDirCmb.Items.Add("Default");
		for (int i = 0; i < directories.Length; i++)
		{
			string fileName = Path.GetFileName(directories[i]);
			if (!(fileName == "Default") && !(fileName == "Poke-Controller"))
			{
				macroSubDirCmb.Items.Add(Path.GetFileName(directories[i]));
			}
		}
		macroSubDirCmb.Items.Add("Poke-Controller");
		macroSubDirCmb.Text = text;
		macroSubDirCmb.Tag = null;
		macroSubDirCmb.EndUpdate();
	}

	private void buttonEx2_Click(object sender, EventArgs e)
	{
		if (macroSubDirCmb.SelectedIndex == macroSubDirCmb.Items.Count - 1)
		{
			if (_pokeconRnnning)
			{
				if (_pokeconProcess != null)
				{
					try
					{
						_pokeconProcess.Kill();
						return;
					}
					catch (Exception)
					{
						return;
					}
				}
				return;
			}
			if (Nmc.SubRunningNmc != "")
			{
				Nmc.Cancel = true;
				while (Nmc.Running)
				{
					System.Windows.Forms.Application.DoEvents();
				}
			}
			string selectedMacro = macroSelCmb.Text;
			buttonEx2.Image = Resources.B4;
			buttonEx2.Text = "停止";
			_pokeconRnnning = true;
			macroSelCmb.Enabled = false;
			macroSubDirCmb.Enabled = false;
			button6.Enabled = false;
			button4.Enabled = false;
			cH552へ書き込みToolStripMenuItem.Enabled = false;
			cH552SERIALセットアップToolStripMenuItem.Enabled = false;
			Task.Factory.StartNew(delegate
			{
				try
				{
					if (!Directory.Exists(GlobalVar.BasePath + "Macro\\Poke-Controller\\Template"))
					{
						Directory.CreateDirectory(GlobalVar.BasePath + "Macro\\Poke-Controller\\Template");
					}
					Directory.CreateDirectory(GlobalVar.BasePath + "Poke-Controller\\Commands\\PythonCommands");
					Util.CopyDirectory(GlobalVar.BasePath + "Macro\\Poke-Controller\\", GlobalVar.BasePath + "Poke-Controller\\Commands\\PythonCommands");
					string contents = "[LINE]\rtoken = " + NxCommand.LineNotifyToken;
					File.WriteAllText(GlobalVar.BasePath + "Poke-Controller\\line_token.ini", contents);
					Directory.CreateDirectory(GlobalVar.BasePath + "Poke-Controller\\Template");
					Directory.CreateDirectory(GlobalVar.BasePath + "Poke-Controller\\Captures");
					Util.CopyDirectory(GlobalVar.BasePath + "Macro\\Poke-Controller\\Template", GlobalVar.BasePath + "Poke-Controller\\Template");
					DsDevice[] devicesOfCat = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
					int num = -1;
					for (int i = 0; i < devicesOfCat.Length; i++)
					{
						if (devicesOfCat[i].Name == "NX2 Virtual Camera")
						{
							num = i;
							break;
						}
					}
					_pokeconProcess = new Process();
					_pokeconProcess.StartInfo.FileName = GlobalVar.BasePath + "Poke-Controller\\Poke-Controller.exe";
					_pokeconProcess.StartInfo.UseShellExecute = false;
					_pokeconProcess.StartInfo.RedirectStandardOutput = true;
					_pokeconProcess.StartInfo.RedirectStandardError = true;
					_pokeconProcess.StartInfo.CreateNoWindow = true;
					_pokeconProcess.EnableRaisingEvents = true;
					_pokeconProcess.StartInfo.Arguments = num + " " + selectedMacro;
					_pokeconProcess.StartInfo.WorkingDirectory = GlobalVar.BasePath + "Poke-Controller\\";
					_pokeconProcess.Exited += delegate
					{
						_pokeconProcess.WaitForExit();
						Invoke((Action)delegate
						{
							buttonEx2.Text = "実行";
							if (base.FormTheme == BZComponent.Style.Dark)
							{
								buttonEx2.Image = Resources.B1;
							}
							else
							{
								buttonEx2.Image = Resources.B1_L;
							}
							macroSelCmb.Enabled = true;
							macroSubDirCmb.Enabled = true;
							button6.Enabled = true;
							button4.Enabled = true;
							cH552へ書き込みToolStripMenuItem.Enabled = true;
							cH552SERIALセットアップToolStripMenuItem.Enabled = true;
							_pokeconRnnning = false;
							Environment.CurrentDirectory = GlobalVar.BasePath;
							MacroDeactive();
						});
					};
					_pokeconProcess.Start();
					MacroActive();
				}
				catch
				{
					if (_pokeconProcess != null)
					{
						try
						{
							_pokeconProcess.Kill();
						}
						catch
						{
						}
					}
					Invoke((Action)delegate
					{
						buttonEx2.Text = "実行";
						if (base.FormTheme == BZComponent.Style.Dark)
						{
							buttonEx2.Image = Resources.B1;
						}
						else
						{
							buttonEx2.Image = Resources.B1_L;
						}
						macroSelCmb.Enabled = true;
						macroSubDirCmb.Enabled = true;
						button6.Enabled = true;
						button4.Enabled = true;
						cH552へ書き込みToolStripMenuItem.Enabled = true;
						cH552SERIALセットアップToolStripMenuItem.Enabled = true;
						_pokeconRnnning = false;
						MacroDeactive();
					});
				}
				while (true)
				{
					try
					{
						string text2 = _pokeconProcess.StandardOutput.ReadLine();
						if (text2 == null)
						{
							break;
						}
						if (text2 != "")
						{
							Console.WriteLine(text2);
						}
					}
					catch (Exception value)
					{
						Console.WriteLine(value);
						break;
					}
				}
			});
		}
		else
		{
			string text = GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text + "\\" + macroSelCmb.Text;
			if (File.Exists(text + ".nxc"))
			{
				LoadMacro(text + ".nxc");
			}
			else if (File.Exists(text + ".nmc"))
			{
				LoadMacro(text + ".nmc");
			}
		}
	}

	private void buttonEx4_Click(object sender, EventArgs e)
	{
		textBox1.Clear();
	}

	private void cH552SERIALセットアップToolStripMenuItem_Click(object sender, EventArgs e)
	{
		MacroActive();
		button4.Enabled = false;
		button6.Enabled = false;
		buttonEx2.Enabled = false;
		cH552へ書き込みToolStripMenuItem.Enabled = false;
		cH552SERIALセットアップToolStripMenuItem.Enabled = false;
		マクロの読み込みToolStripMenuItem.Enabled = false;
		GlobalVar.TaskName[4] = "CH552への書き込み中";
		TaskView();
		byte[] cH55xSwitchSerialControl_ino = Resources.CH55xSwitchSerialControl_ino;
		if (new CH552Flash().FlashHex(cH55xSwitchSerialControl_ino, 10000L))
		{
			System.Windows.Forms.MessageBox.Show("ファームウェアの書き込みに成功しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			System.Windows.Forms.MessageBox.Show("ファームウェアの書き込みに失敗しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		MacroDeactive();
		button4.Enabled = true;
		button6.Enabled = true;
		buttonEx2.Enabled = true;
		cH552へ書き込みToolStripMenuItem.Enabled = true;
		cH552SERIALセットアップToolStripMenuItem.Enabled = true;
		マクロの読み込みToolStripMenuItem.Enabled = true;
		GlobalVar.TaskName[4] = "";
		TaskView();
	}

	private void cH552へ書き込みToolStripMenuItem_Click(object sender, EventArgs e)
	{
		MacroActive();
		button4.Enabled = false;
		button6.Enabled = false;
		buttonEx2.Enabled = false;
		cH552へ書き込みToolStripMenuItem.Enabled = false;
		cH552SERIALセットアップToolStripMenuItem.Enabled = false;
		マクロの読み込みToolStripMenuItem.Enabled = false;
		GlobalVar.TaskName[4] = "CH552への書き込み中";
		TaskView();
		Nmc.Code = CodeEdit.Text;
		string cH552Program = Nmc.GetCH552Program();
		File.WriteAllText(GlobalVar.BasePath + "CH552\\NXCtoC.c", cH552Program);
		foreach (string item in Directory.EnumerateFiles(GlobalVar.BasePath + "CH552", "NXCtoC.*"))
		{
			if (!(Path.GetExtension(item) == ".c"))
			{
				File.Delete(item);
			}
		}
		Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/c \"" + Path.GetFullPath(GlobalVar.BasePath + "CH552\\compile.bat") + "\"")
		{
			CreateNoWindow = true,
			UseShellExecute = false,
			WorkingDirectory = Path.GetFullPath(GlobalVar.BasePath + "CH552")
		});
		if (process.WaitForExit(30000))
		{
			try
			{
				foreach (string item2 in Directory.EnumerateFiles(GlobalVar.BasePath + "CH552", "NXCtoC.*"))
				{
					if (!(Path.GetExtension(item2) == ".hex") && !(Path.GetExtension(item2) == ".c"))
					{
						File.Delete(item2);
					}
				}
				if (new CH552Flash().FlashHex(File.ReadAllBytes(GlobalVar.BasePath + "CH552\\NXCtoC.hex"), 10000L))
				{
					System.Windows.Forms.MessageBox.Show("プログラムの書き込みに成功しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					System.Windows.Forms.MessageBox.Show("プログラムの書き込みに失敗しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("プログラムの書き込みに失敗しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		else
		{
			System.Windows.Forms.MessageBox.Show("プログラムの書き込みに失敗しました。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		process.Close();
		foreach (string item3 in Directory.EnumerateFiles(GlobalVar.BasePath + "CH552", "NXCtoC.*"))
		{
			File.Delete(item3);
		}
		MacroDeactive();
		button4.Enabled = true;
		button6.Enabled = true;
		buttonEx2.Enabled = true;
		cH552へ書き込みToolStripMenuItem.Enabled = true;
		cH552SERIALセットアップToolStripMenuItem.Enabled = true;
		マクロの読み込みToolStripMenuItem.Enabled = true;
		GlobalVar.TaskName[4] = "";
		TaskView();
	}

	private void マクロの新規作成ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.InitialDirectory = Path.GetFullPath(GlobalVar.BasePath + "Macro\\" + macroSubDirCmb.Text);
		saveFileDialog.Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*";
		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			CodeEdit.Text = "//ここにマクロを記述する";
			Nmc.Code = CodeEdit.Text;
			Nmc.ResourcesImages.Clear();
			Nmc.FilePath = Path.GetFullPath(Path.GetDirectoryName(saveFileDialog.FileName)) + "\\";
			Nmc.AllPath = saveFileDialog.FileName;
			byte[] fileData = Nmc.GetFileData();
			if (File.Exists(saveFileDialog.FileName))
			{
				File.Delete(saveFileDialog.FileName);
			}
			File.WriteAllBytes(saveFileDialog.FileName, fileData);
			Text = GlobalVar.AppName + " - " + Path.GetFileName(saveFileDialog.FileName);
			マクロを上書き保存ToolStripMenuItem.Enabled = true;
			flowLayoutPanel3.Enabled = true;
			flowLayoutPanel3.Visible = true;
			SetMacroDirectory(saveFileDialog.FileName);
		}
	}

	private void discordサーバーToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Process.Start("https://discord.gg/9VwVrsAAAQ");
	}

	private void macroSubDirCmb_TextUpdate(object sender, EventArgs e)
	{
	}

	private void macroSelCmb_TextUpdate(object sender, EventArgs e)
	{
		macroSelCmbText = macroSelCmb.Text;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NX_Macro_Controller_VxV.NXMC_VxV));
		this.CaptureScreen = new System.Windows.Forms.PictureBox();
		this.CaptureContext = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.画面をキャプチャToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.全画面キャプチャToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.CaptureBGW = new System.ComponentModel.BackgroundWorker();
		this.groupBox1 = new BZComponent.GroupBoxEx();
		this.ghostPanel7 = new BZComponent.GhostPanel();
		this.button2 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.buttonEx1 = new BZComponent.ButtonEx();
		this.CapConnect = new BZComponent.ButtonEx();
		this.CapDeviceList = new BZComponent.ComboBoxEx();
		this.vScrollBar1 = new CustomScrollBar.ScrollBarEx();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロの新規作成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロの読み込みToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロを上書き保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロの保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
		this.amiiboの読み込みToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
		this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.BTSetUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.cH552SERIALセットアップToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.cH552へ書き込みToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.接続ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
		this.環境設定ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.共有ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロ共有サーバーに接続ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.バージョン情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.discordサーバーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.readmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.button6 = new BZComponent.ButtonEx();
		this.button4 = new BZComponent.ButtonEx();
		this.button3 = new BZComponent.ButtonEx();
		this.panel1 = new System.Windows.Forms.Panel();
		this.hScrollBar1 = new CustomScrollBar.ScrollBarEx();
		this.label1 = new System.Windows.Forms.Label();
		this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.tabControl1 = new BZComponent.TabControlEx();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.ghostPanel6 = new BZComponent.GhostPanel();
		this.buttonEx5 = new BZComponent.ButtonEx();
		this.buttonEx4 = new BZComponent.ButtonEx();
		this.groupBoxEx1 = new BZComponent.GroupBoxEx();
		this.scrollBarEx1 = new CustomScrollBar.ScrollBarEx();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tabControlEx1 = new BZComponent.TabControlEx();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.scrollBarEx3 = new CustomScrollBar.ScrollBarEx();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.scrollBarEx4 = new CustomScrollBar.ScrollBarEx();
		this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.scrollBarEx2 = new CustomScrollBar.ScrollBarEx();
		this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ghostPanel4 = new BZComponent.GhostPanel();
		this.splitter1 = new System.Windows.Forms.Splitter();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ghostPanel5 = new BZComponent.GhostPanel();
		this.label4 = new System.Windows.Forms.Label();
		this.macroSubDirCmb = new BZComponent.ComboBoxEx();
		this.buttonEx3 = new BZComponent.ButtonEx();
		this.ComPortList = new BZComponent.ComboBoxEx();
		this.buttonEx2 = new BZComponent.ButtonEx();
		this.label3 = new System.Windows.Forms.Label();
		this.ComConnect = new BZComponent.ButtonEx();
		this.macroSelCmb = new BZComponent.ComboBoxEx();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
		this.fileSystemWatcher2 = new System.IO.FileSystemWatcher();
		this.fileSystemWatcher3 = new System.IO.FileSystemWatcher();
		this.fileSystemWatcher4 = new System.IO.FileSystemWatcher();
		this.fileSystemWatcher5 = new System.IO.FileSystemWatcher();
		this.mouseHook1 = new HongliangSoft.Utilities.MouseHook();
		((System.ComponentModel.ISupportInitialize)this.CaptureScreen).BeginInit();
		this.CaptureContext.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.ghostPanel7.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.ghostPanel6.SuspendLayout();
		this.groupBoxEx1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.tabControlEx1.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.tabPage6.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.ghostPanel4.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.ghostPanel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher5).BeginInit();
		base.SuspendLayout();
		this.CaptureScreen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CaptureScreen.BackColor = System.Drawing.SystemColors.ControlDarkDark;
		this.CaptureScreen.ContextMenuStrip = this.CaptureContext;
		this.CaptureScreen.Location = new System.Drawing.Point(1, 1);
		this.CaptureScreen.Margin = new System.Windows.Forms.Padding(0);
		this.CaptureScreen.Name = "CaptureScreen";
		this.CaptureScreen.Size = new System.Drawing.Size(624, 322);
		this.CaptureScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.CaptureScreen.TabIndex = 0;
		this.CaptureScreen.TabStop = false;
		this.CaptureScreen.SizeChanged += new System.EventHandler(CaptureScreen_SizeChanged);
		this.CaptureScreen.Click += new System.EventHandler(CaptureScreen_Click);
		this.CaptureScreen.Paint += new System.Windows.Forms.PaintEventHandler(CaptureScreen_Paint);
		this.CaptureScreen.MouseDown += new System.Windows.Forms.MouseEventHandler(CaptureScreen_MouseDown);
		this.CaptureScreen.MouseEnter += new System.EventHandler(CaptureScreen_MouseEnter);
		this.CaptureScreen.MouseLeave += new System.EventHandler(CaptureScreen_MouseLeave);
		this.CaptureScreen.MouseMove += new System.Windows.Forms.MouseEventHandler(CaptureScreen_MouseMove);
		this.CaptureScreen.MouseUp += new System.Windows.Forms.MouseEventHandler(CaptureScreen_MouseUp);
		this.CaptureContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.画面をキャプチャToolStripMenuItem, this.全画面キャプチャToolStripMenuItem });
		this.CaptureContext.Name = "CaptureContext";
		this.CaptureContext.Size = new System.Drawing.Size(154, 48);
		this.CaptureContext.Opening += new System.ComponentModel.CancelEventHandler(CaptureContext_Opening);
		this.画面をキャプチャToolStripMenuItem.Name = "画面をキャプチャToolStripMenuItem";
		this.画面をキャプチャToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
		this.画面をキャプチャToolStripMenuItem.Text = "画面をキャプチャ";
		this.画面をキャプチャToolStripMenuItem.Click += new System.EventHandler(画面をキャプチャToolStripMenuItem_Click);
		this.全画面キャプチャToolStripMenuItem.Name = "全画面キャプチャToolStripMenuItem";
		this.全画面キャプチャToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
		this.全画面キャプチャToolStripMenuItem.Text = "全画面キャプチャ";
		this.全画面キャプチャToolStripMenuItem.Click += new System.EventHandler(全画面キャプチャToolStripMenuItem_Click);
		this.CaptureBGW.WorkerReportsProgress = true;
		this.CaptureBGW.WorkerSupportsCancellation = true;
		this.CaptureBGW.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.CaptureBGW.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		this.groupBox1.BorderColor = System.Drawing.Color.Black;
		this.groupBox1.Controls.Add(this.ghostPanel7);
		this.groupBox1.Controls.Add(this.button2);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add(this.buttonEx1);
		this.groupBox1.Controls.Add(this.CapConnect);
		this.groupBox1.Controls.Add(this.CapDeviceList);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox1.Location = new System.Drawing.Point(371, 0);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.MinimumSize = new System.Drawing.Size(0, 381);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Size = new System.Drawing.Size(641, 381);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "映像";
		this.groupBox1.Enter += new System.EventHandler(groupBox1_Enter);
		this.ghostPanel7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ghostPanel7.Controls.Add(this.CaptureScreen);
		this.ghostPanel7.Location = new System.Drawing.Point(9, 45);
		this.ghostPanel7.Name = "ghostPanel7";
		this.ghostPanel7.Size = new System.Drawing.Size(626, 324);
		this.ghostPanel7.TabIndex = 5;
		this.button2.Location = new System.Drawing.Point(558, 16);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 4;
		this.button2.Text = "button2";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Visible = false;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.button1.Location = new System.Drawing.Point(476, 17);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 1;
		this.button1.Text = "button1";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Visible = false;
		this.button1.Click += new System.EventHandler(button1_Click_4);
		this.buttonEx1.FlatAppearance.BorderSize = 0;
		this.buttonEx1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.buttonEx1.Image = NX_Macro_Controller_VxV.Properties.Resources.B3_LINK;
		this.buttonEx1.Location = new System.Drawing.Point(333, 17);
		this.buttonEx1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.buttonEx1.Name = "buttonEx1";
		this.buttonEx1.Size = new System.Drawing.Size(75, 23);
		this.buttonEx1.TabIndex = 3;
		this.buttonEx1.Text = "接続";
		this.buttonEx1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.buttonEx1.UseVisualStyleBackColor = true;
		this.buttonEx1.Visible = false;
		this.buttonEx1.Click += new System.EventHandler(buttonEx1_Click);
		this.CapConnect.FlatAppearance.BorderSize = 0;
		this.CapConnect.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.CapConnect.Image = NX_Macro_Controller_VxV.Properties.Resources.B3;
		this.CapConnect.Location = new System.Drawing.Point(252, 17);
		this.CapConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.CapConnect.Name = "CapConnect";
		this.CapConnect.Size = new System.Drawing.Size(75, 23);
		this.CapConnect.TabIndex = 1;
		this.CapConnect.Text = "接続";
		this.CapConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.CapConnect.UseVisualStyleBackColor = true;
		this.CapConnect.Click += new System.EventHandler(CapConnect_Click);
		this.CapConnect.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.CapConnect.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(button6_PreviewKeyDown);
		this.CapDeviceList.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.CapDeviceList.BorderColor = System.Drawing.Color.FromArgb(65, 65, 67);
		this.CapDeviceList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.CapDeviceList.ContentsCheck = true;
		this.CapDeviceList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.CapDeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.CapDeviceList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.CapDeviceList.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.CapDeviceList.FormattingEnabled = true;
		this.CapDeviceList.Location = new System.Drawing.Point(9, 17);
		this.CapDeviceList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.CapDeviceList.Name = "CapDeviceList";
		this.CapDeviceList.Size = new System.Drawing.Size(237, 23);
		this.CapDeviceList.TabIndex = 0;
		this.CapDeviceList.SelectedIndexChanged += new System.EventHandler(CapDeviceList_SelectedIndexChanged);
		this.CapDeviceList.Enter += new System.EventHandler(CapDeviceList_Enter);
		this.CapDeviceList.KeyDown += new System.Windows.Forms.KeyEventHandler(CapDeviceList_KeyDown);
		this.CapDeviceList.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(CapDeviceList_PreviewKeyDown);
		this.vScrollBar1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 128);
		this.vScrollBar1.LargeChange = 8000;
		this.vScrollBar1.Location = new System.Drawing.Point(321, 3);
		this.vScrollBar1.Maximum = 40000;
		this.vScrollBar1.Name = "vScrollBar1";
		this.vScrollBar1.Size = new System.Drawing.Size(19, 290);
		this.vScrollBar1.SmallChange = 4000;
		this.vScrollBar1.TabIndex = 4000;
		this.vScrollBar1.Text = "scrollBarEx1";
		this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(vScrollBar1_Scroll);
		this.menuStrip1.AllowDrop = true;
		this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.ファイルToolStripMenuItem, this.設定ToolStripMenuItem, this.共有ToolStripMenuItem, this.aboutToolStripMenuItem });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 1, 0, 1);
		this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
		this.menuStrip1.Size = new System.Drawing.Size(1012, 24);
		this.menuStrip1.TabIndex = 0;
		this.menuStrip1.Text = "menuStrip1";
		this.menuStrip1.Click += new System.EventHandler(menuStrip1_Click);
		this.menuStrip1.DragDrop += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragDrop);
		this.menuStrip1.DragEnter += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragEnter);
		this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.マクロの新規作成ToolStripMenuItem, this.マクロの読み込みToolStripMenuItem, this.マクロを上書き保存ToolStripMenuItem, this.マクロの保存ToolStripMenuItem, this.toolStripMenuItem1, this.amiiboの読み込みToolStripMenuItem, this.toolStripMenuItem5, this.終了ToolStripMenuItem });
		this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
		this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
		this.ファイルToolStripMenuItem.Text = "ファイル";
		this.マクロの新規作成ToolStripMenuItem.Name = "マクロの新規作成ToolStripMenuItem";
		this.マクロの新規作成ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.マクロの新規作成ToolStripMenuItem.Text = "マクロの新規作成";
		this.マクロの新規作成ToolStripMenuItem.Click += new System.EventHandler(マクロの新規作成ToolStripMenuItem_Click);
		this.マクロの読み込みToolStripMenuItem.Name = "マクロの読み込みToolStripMenuItem";
		this.マクロの読み込みToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.マクロの読み込みToolStripMenuItem.Text = "マクロの読み込み";
		this.マクロの読み込みToolStripMenuItem.Click += new System.EventHandler(マクロの読み込みToolStripMenuItem_Click);
		this.マクロを上書き保存ToolStripMenuItem.Enabled = false;
		this.マクロを上書き保存ToolStripMenuItem.Name = "マクロを上書き保存ToolStripMenuItem";
		this.マクロを上書き保存ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.マクロを上書き保存ToolStripMenuItem.Text = "マクロを上書き保存";
		this.マクロを上書き保存ToolStripMenuItem.Click += new System.EventHandler(マクロを上書き保存ToolStripMenuItem_Click);
		this.マクロの保存ToolStripMenuItem.Name = "マクロの保存ToolStripMenuItem";
		this.マクロの保存ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.マクロの保存ToolStripMenuItem.Text = "マクロに名前を付けて保存";
		this.マクロの保存ToolStripMenuItem.Click += new System.EventHandler(マクロの保存ToolStripMenuItem_Click);
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(205, 6);
		this.amiiboの読み込みToolStripMenuItem.Name = "amiiboの読み込みToolStripMenuItem";
		this.amiiboの読み込みToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.amiiboの読み込みToolStripMenuItem.Text = "Amiiboの読み込み";
		this.amiiboの読み込みToolStripMenuItem.Click += new System.EventHandler(amiibo読込ToolStripMenuItem_Click);
		this.toolStripMenuItem5.Name = "toolStripMenuItem5";
		this.toolStripMenuItem5.Size = new System.Drawing.Size(205, 6);
		this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
		this.終了ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
		this.終了ToolStripMenuItem.Text = "終了";
		this.終了ToolStripMenuItem.Click += new System.EventHandler(終了ToolStripMenuItem_Click);
		this.設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.BTSetUpToolStripMenuItem, this.cH552SERIALセットアップToolStripMenuItem, this.cH552へ書き込みToolStripMenuItem, this.接続ToolStripMenuItem, this.toolStripMenuItem3, this.環境設定ToolStripMenuItem1 });
		this.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem";
		this.設定ToolStripMenuItem.Size = new System.Drawing.Size(51, 22);
		this.設定ToolStripMenuItem.Text = "ツール";
		this.設定ToolStripMenuItem.DropDownOpened += new System.EventHandler(設定ToolStripMenuItem_DropDownOpened);
		this.設定ToolStripMenuItem.Click += new System.EventHandler(設定ToolStripMenuItem_Click);
		this.BTSetUpToolStripMenuItem.Name = "BTSetUpToolStripMenuItem";
		this.BTSetUpToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
		this.BTSetUpToolStripMenuItem.Text = "無線接続セットアップ";
		this.BTSetUpToolStripMenuItem.Click += new System.EventHandler(BTSetUpToolStripMenuItem_Click);
		this.cH552SERIALセットアップToolStripMenuItem.Name = "cH552SERIALセットアップToolStripMenuItem";
		this.cH552SERIALセットアップToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
		this.cH552SERIALセットアップToolStripMenuItem.Text = "CH552-SERIALセットアップ";
		this.cH552SERIALセットアップToolStripMenuItem.Click += new System.EventHandler(cH552SERIALセットアップToolStripMenuItem_Click);
		this.cH552へ書き込みToolStripMenuItem.Name = "cH552へ書き込みToolStripMenuItem";
		this.cH552へ書き込みToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
		this.cH552へ書き込みToolStripMenuItem.Text = "CH552へ書き込み";
		this.cH552へ書き込みToolStripMenuItem.Click += new System.EventHandler(cH552へ書き込みToolStripMenuItem_Click);
		this.接続ToolStripMenuItem.Name = "接続ToolStripMenuItem";
		this.接続ToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
		this.接続ToolStripMenuItem.Text = "接続";
		this.接続ToolStripMenuItem.DropDownOpened += new System.EventHandler(接続ToolStripMenuItem_DropDownOpened);
		this.接続ToolStripMenuItem.Click += new System.EventHandler(接続ToolStripMenuItem_Click);
		this.toolStripMenuItem3.Name = "toolStripMenuItem3";
		this.toolStripMenuItem3.Size = new System.Drawing.Size(203, 6);
		this.環境設定ToolStripMenuItem1.Name = "環境設定ToolStripMenuItem1";
		this.環境設定ToolStripMenuItem1.Size = new System.Drawing.Size(206, 22);
		this.環境設定ToolStripMenuItem1.Text = "オプション";
		this.環境設定ToolStripMenuItem1.Click += new System.EventHandler(環境設定ToolStripMenuItem_Click);
		this.共有ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.マクロ共有サーバーに接続ToolStripMenuItem });
		this.共有ToolStripMenuItem.Name = "共有ToolStripMenuItem";
		this.共有ToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
		this.共有ToolStripMenuItem.Text = "マクロ共有サーバー";
		this.マクロ共有サーバーに接続ToolStripMenuItem.Name = "マクロ共有サーバーに接続ToolStripMenuItem";
		this.マクロ共有サーバーに接続ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
		this.マクロ共有サーバーに接続ToolStripMenuItem.Text = "接続";
		this.マクロ共有サーバーに接続ToolStripMenuItem.Click += new System.EventHandler(マクロ共有サーバーに接続ToolStripMenuItem_Click);
		this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.バージョン情報ToolStripMenuItem, this.discordサーバーToolStripMenuItem, this.readmeToolStripMenuItem, this.ヘルプToolStripMenuItem });
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 22);
		this.aboutToolStripMenuItem.Text = "About";
		this.バージョン情報ToolStripMenuItem.Name = "バージョン情報ToolStripMenuItem";
		this.バージョン情報ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.バージョン情報ToolStripMenuItem.Text = "バージョン情報";
		this.バージョン情報ToolStripMenuItem.Click += new System.EventHandler(バージョン情報ToolStripMenuItem_Click);
		this.discordサーバーToolStripMenuItem.Name = "discordサーバーToolStripMenuItem";
		this.discordサーバーToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.discordサーバーToolStripMenuItem.Text = "Discordサーバー";
		this.discordサーバーToolStripMenuItem.Click += new System.EventHandler(discordサーバーToolStripMenuItem_Click);
		this.readmeToolStripMenuItem.Name = "readmeToolStripMenuItem";
		this.readmeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.readmeToolStripMenuItem.Text = "Readme";
		this.readmeToolStripMenuItem.Click += new System.EventHandler(readmeToolStripMenuItem_Click);
		this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
		this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.ヘルプToolStripMenuItem.Text = "ヘルプ";
		this.ヘルプToolStripMenuItem.Click += new System.EventHandler(ヘルプToolStripMenuItem_Click);
		this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.button6.FlatAppearance.BorderSize = 0;
		this.button6.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.button6.Image = NX_Macro_Controller_VxV.Properties.Resources.B1;
		this.button6.Location = new System.Drawing.Point(7, 327);
		this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(75, 23);
		this.button6.TabIndex = 0;
		this.button6.Text = "実行";
		this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
		this.button6.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.button6.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(button6_PreviewKeyDown);
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button4.FlatAppearance.BorderSize = 0;
		this.button4.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.button4.Image = NX_Macro_Controller_VxV.Properties.Resources.B2;
		this.button4.Location = new System.Drawing.Point(279, 327);
		this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(75, 23);
		this.button4.TabIndex = 1;
		this.button4.Text = "記録";
		this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.button4.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.button4.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(button6_PreviewKeyDown);
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(198, 327);
		this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(75, 23);
		this.button3.TabIndex = 2;
		this.button3.Text = "入力補助";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button3.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.button3.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(button6_PreviewKeyDown);
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BackColor = System.Drawing.Color.Red;
		this.panel1.Controls.Add(this.hScrollBar1);
		this.panel1.Controls.Add(this.vScrollBar1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.elementHost1);
		this.panel1.Location = new System.Drawing.Point(6, 6);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(348, 316);
		this.panel1.TabIndex = 3;
		this.hScrollBar1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 128);
		this.hScrollBar1.LargeChange = 8000;
		this.hScrollBar1.Location = new System.Drawing.Point(5, 298);
		this.hScrollBar1.Maximum = 40000;
		this.hScrollBar1.Name = "hScrollBar1";
		this.hScrollBar1.Orientation = CustomScrollBar.ScrollBarOrientation.Horizontal;
		this.hScrollBar1.Size = new System.Drawing.Size(290, 19);
		this.hScrollBar1.SmallChange = 4000;
		this.hScrollBar1.TabIndex = 4001;
		this.hScrollBar1.Text = "scrollBarEx1";
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label1.BackColor = System.Drawing.SystemColors.ScrollBar;
		this.label1.Location = new System.Drawing.Point(-342, 299);
		this.label1.Margin = new System.Windows.Forms.Padding(0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 23);
		this.label1.TabIndex = 3;
		this.elementHost1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.elementHost1.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.elementHost1.Location = new System.Drawing.Point(1, 1);
		this.elementHost1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.elementHost1.Name = "elementHost1";
		this.elementHost1.Size = new System.Drawing.Size(346, 313);
		this.elementHost1.TabIndex = 0;
		this.elementHost1.Text = "elementHost1";
		this.elementHost1.Child = null;
		this.tabControl1.ActiveColor = System.Drawing.Color.FromArgb(0, 122, 204);
		this.tabControl1.AllowDrop = true;
		this.tabControl1.BackTabColor = System.Drawing.Color.FromArgb(28, 28, 28);
		this.tabControl1.BorderColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.tabControl1.ClosingButtonColor = System.Drawing.Color.WhiteSmoke;
		this.tabControl1.ClosingMessage = null;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.DeActiveColor = System.Drawing.Color.FromArgb(63, 63, 70);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.EnabledTabDrag = false;
		this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.HeaderColor = System.Drawing.Color.FromArgb(45, 45, 48);
		this.tabControl1.HorizontalLineColor = System.Drawing.Color.FromArgb(0, 122, 204);
		this.tabControl1.ItemSize = new System.Drawing.Size(240, 16);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.MinimumSize = new System.Drawing.Size(360, 150);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.SelectedTextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.tabControl1.ShowClosingButton = false;
		this.tabControl1.ShowClosingMessage = false;
		this.tabControl1.Size = new System.Drawing.Size(368, 380);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.TextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.tabControl1.SizeChanged += new System.EventHandler(tabControl1_SizeChanged);
		this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragDrop);
		this.tabControl1.DragEnter += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragEnter);
		this.tabControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.tabPage2.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage2.Controls.Add(this.button6);
		this.tabPage2.Controls.Add(this.button4);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.button3);
		this.tabPage2.Location = new System.Drawing.Point(4, 20);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(360, 356);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "コード";
		this.tabPage2.Click += new System.EventHandler(tabPage2_Click);
		this.tabPage4.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage4.Controls.Add(this.ghostPanel6);
		this.tabPage4.Location = new System.Drawing.Point(4, 20);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(360, 356);
		this.tabPage4.TabIndex = 4;
		this.tabPage4.Text = "ログ";
		this.ghostPanel6.Controls.Add(this.buttonEx5);
		this.ghostPanel6.Controls.Add(this.buttonEx4);
		this.ghostPanel6.Controls.Add(this.groupBoxEx1);
		this.ghostPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ghostPanel6.Location = new System.Drawing.Point(3, 3);
		this.ghostPanel6.Name = "ghostPanel6";
		this.ghostPanel6.Size = new System.Drawing.Size(354, 350);
		this.ghostPanel6.TabIndex = 0;
		this.ghostPanel6.Paint += new System.Windows.Forms.PaintEventHandler(ghostPanel6_Paint);
		this.buttonEx5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.buttonEx5.FlatAppearance.BorderSize = 0;
		this.buttonEx5.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.buttonEx5.Image = NX_Macro_Controller_VxV.Properties.Resources.B1;
		this.buttonEx5.Location = new System.Drawing.Point(4, 2);
		this.buttonEx5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.buttonEx5.Name = "buttonEx5";
		this.buttonEx5.Size = new System.Drawing.Size(75, 23);
		this.buttonEx5.TabIndex = 5;
		this.buttonEx5.Text = "実行";
		this.buttonEx5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.buttonEx5.UseVisualStyleBackColor = true;
		this.buttonEx5.Click += new System.EventHandler(button6_Click);
		this.buttonEx5.KeyDown += new System.Windows.Forms.KeyEventHandler(tabControl1_KeyDown);
		this.buttonEx4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.buttonEx4.FlatAppearance.BorderSize = 0;
		this.buttonEx4.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.buttonEx4.Image = null;
		this.buttonEx4.Location = new System.Drawing.Point(3, 322);
		this.buttonEx4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.buttonEx4.Name = "buttonEx4";
		this.buttonEx4.Size = new System.Drawing.Size(348, 23);
		this.buttonEx4.TabIndex = 4;
		this.buttonEx4.Text = "ログのクリア";
		this.buttonEx4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.buttonEx4.UseVisualStyleBackColor = true;
		this.buttonEx4.Click += new System.EventHandler(buttonEx4_Click);
		this.groupBoxEx1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBoxEx1.BorderColor = System.Drawing.Color.FromArgb(65, 65, 67);
		this.groupBoxEx1.Controls.Add(this.scrollBarEx1);
		this.groupBoxEx1.Controls.Add(this.textBox1);
		this.groupBoxEx1.Location = new System.Drawing.Point(3, 30);
		this.groupBoxEx1.Name = "groupBoxEx1";
		this.groupBoxEx1.Padding = new System.Windows.Forms.Padding(0);
		this.groupBoxEx1.Size = new System.Drawing.Size(348, 287);
		this.groupBoxEx1.TabIndex = 2;
		this.groupBoxEx1.TabStop = false;
		this.scrollBarEx1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.scrollBarEx1.LargeChange = 8000;
		this.scrollBarEx1.Location = new System.Drawing.Point(329, 1);
		this.scrollBarEx1.Maximum = 40000;
		this.scrollBarEx1.Name = "scrollBarEx1";
		this.scrollBarEx1.Size = new System.Drawing.Size(19, 286);
		this.scrollBarEx1.SmallChange = 4000;
		this.scrollBarEx1.TabIndex = 1;
		this.scrollBarEx1.Text = "scrollBarEx1";
		this.scrollBarEx1.Scroll += new System.Windows.Forms.ScrollEventHandler(scrollBarEx1_Scroll);
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox1.Location = new System.Drawing.Point(6, 7);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.ReadOnly = true;
		this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox1.Size = new System.Drawing.Size(342, 274);
		this.textBox1.TabIndex = 0;
		this.textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.textBox1.Layout += new System.Windows.Forms.LayoutEventHandler(textBox1_Layout);
		this.tabPage3.AllowDrop = true;
		this.tabPage3.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage3.Controls.Add(this.tabControlEx1);
		this.tabPage3.Location = new System.Drawing.Point(4, 20);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(360, 356);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "リソース";
		this.tabPage3.DragEnter += new System.Windows.Forms.DragEventHandler(tabPage3_DragEnter);
		this.tabControlEx1.ActiveColor = System.Drawing.Color.FromArgb(0, 122, 204);
		this.tabControlEx1.AllowDrop = true;
		this.tabControlEx1.BackTabColor = System.Drawing.Color.FromArgb(28, 28, 28);
		this.tabControlEx1.BorderColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.tabControlEx1.ClosingButtonColor = System.Drawing.Color.WhiteSmoke;
		this.tabControlEx1.ClosingMessage = null;
		this.tabControlEx1.Controls.Add(this.tabPage6);
		this.tabControlEx1.Controls.Add(this.tabPage5);
		this.tabControlEx1.DeActiveColor = System.Drawing.Color.FromArgb(63, 63, 70);
		this.tabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlEx1.EnabledTabDrag = false;
		this.tabControlEx1.HeaderColor = System.Drawing.Color.FromArgb(45, 45, 48);
		this.tabControlEx1.HorizontalLineColor = System.Drawing.Color.FromArgb(0, 122, 204);
		this.tabControlEx1.ItemSize = new System.Drawing.Size(240, 16);
		this.tabControlEx1.Location = new System.Drawing.Point(0, 0);
		this.tabControlEx1.Name = "tabControlEx1";
		this.tabControlEx1.SelectedIndex = 0;
		this.tabControlEx1.SelectedTextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.tabControlEx1.ShowClosingButton = false;
		this.tabControlEx1.ShowClosingMessage = false;
		this.tabControlEx1.Size = new System.Drawing.Size(360, 356);
		this.tabControlEx1.TabIndex = 1;
		this.tabControlEx1.TextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.tabPage5.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage5.Controls.Add(this.scrollBarEx3);
		this.tabPage5.Controls.Add(this.flowLayoutPanel1);
		this.tabPage5.Location = new System.Drawing.Point(4, 20);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(352, 332);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "画像";
		this.scrollBarEx3.Dock = System.Windows.Forms.DockStyle.Right;
		this.scrollBarEx3.LargeChange = 8000;
		this.scrollBarEx3.Location = new System.Drawing.Point(333, 0);
		this.scrollBarEx3.Maximum = 40000;
		this.scrollBarEx3.Name = "scrollBarEx3";
		this.scrollBarEx3.Size = new System.Drawing.Size(19, 332);
		this.scrollBarEx3.SmallChange = 4000;
		this.scrollBarEx3.TabIndex = 1;
		this.scrollBarEx3.Text = "scrollBarEx3";
		this.scrollBarEx3.Scroll += new System.Windows.Forms.ScrollEventHandler(scrollBarEx3_Scroll);
		this.flowLayoutPanel1.AllowDrop = true;
		this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(352, 332);
		this.flowLayoutPanel1.TabIndex = 0;
		this.flowLayoutPanel1.Click += new System.EventHandler(flowLayoutPanel1_Click);
		this.flowLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(flowLayoutPanel1_DragDrop);
		this.flowLayoutPanel1.DragEnter += new System.Windows.Forms.DragEventHandler(flowLayoutPanel1_DragEnter);
		this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(flowLayoutPanel1_Paint);
		this.flowLayoutPanel1.Resize += new System.EventHandler(flowLayoutPanel1_Resize);
		this.tabPage6.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage6.Controls.Add(this.scrollBarEx4);
		this.tabPage6.Controls.Add(this.flowLayoutPanel3);
		this.tabPage6.Location = new System.Drawing.Point(4, 20);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(352, 332);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "ファイル";
		this.scrollBarEx4.Dock = System.Windows.Forms.DockStyle.Right;
		this.scrollBarEx4.LargeChange = 8000;
		this.scrollBarEx4.Location = new System.Drawing.Point(333, 0);
		this.scrollBarEx4.Maximum = 40000;
		this.scrollBarEx4.Name = "scrollBarEx4";
		this.scrollBarEx4.Size = new System.Drawing.Size(19, 332);
		this.scrollBarEx4.SmallChange = 4000;
		this.scrollBarEx4.TabIndex = 2;
		this.scrollBarEx4.Text = "scrollBarEx4";
		this.scrollBarEx4.Scroll += new System.Windows.Forms.ScrollEventHandler(scrollBarEx4_Scroll);
		this.flowLayoutPanel3.AllowDrop = true;
		this.flowLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.flowLayoutPanel3.AutoScroll = true;
		this.flowLayoutPanel3.Enabled = false;
		this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel3.Name = "flowLayoutPanel3";
		this.flowLayoutPanel3.Size = new System.Drawing.Size(352, 332);
		this.flowLayoutPanel3.TabIndex = 1;
		this.flowLayoutPanel3.Visible = false;
		this.flowLayoutPanel3.DragDrop += new System.Windows.Forms.DragEventHandler(flowLayoutPanel3_DragDrop);
		this.flowLayoutPanel3.DragEnter += new System.Windows.Forms.DragEventHandler(flowLayoutPanel1_DragEnter);
		this.flowLayoutPanel3.Resize += new System.EventHandler(flowLayoutPanel3_Resize);
		this.tabPage1.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.tabPage1.Controls.Add(this.scrollBarEx2);
		this.tabPage1.Controls.Add(this.flowLayoutPanel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 20);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Size = new System.Drawing.Size(360, 356);
		this.tabPage1.TabIndex = 3;
		this.tabPage1.Text = "ショートカット";
		this.scrollBarEx2.Dock = System.Windows.Forms.DockStyle.Right;
		this.scrollBarEx2.LargeChange = 8000;
		this.scrollBarEx2.Location = new System.Drawing.Point(341, 0);
		this.scrollBarEx2.Maximum = 0;
		this.scrollBarEx2.Name = "scrollBarEx2";
		this.scrollBarEx2.Size = new System.Drawing.Size(19, 356);
		this.scrollBarEx2.SmallChange = 4000;
		this.scrollBarEx2.TabIndex = 0;
		this.scrollBarEx2.Text = "scrollBarEx2";
		this.scrollBarEx2.Scroll += new System.Windows.Forms.ScrollEventHandler(scrollBarEx2_Scroll);
		this.flowLayoutPanel2.AllowDrop = true;
		this.flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.flowLayoutPanel2.AutoScroll = true;
		this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel2.Name = "flowLayoutPanel2";
		this.flowLayoutPanel2.Size = new System.Drawing.Size(360, 356);
		this.flowLayoutPanel2.TabIndex = 1;
		this.flowLayoutPanel2.Scroll += new System.Windows.Forms.ScrollEventHandler(flowLayoutPanel2_Scroll);
		this.flowLayoutPanel2.DragDrop += new System.Windows.Forms.DragEventHandler(flowLayoutPanel2_DragDrop);
		this.flowLayoutPanel2.DragEnter += new System.Windows.Forms.DragEventHandler(flowLayoutPanel2_DragEnter);
		this.flowLayoutPanel2.MouseEnter += new System.EventHandler(flowLayoutPanel2_MouseEnter);
		this.flowLayoutPanel2.Resize += new System.EventHandler(flowLayoutPanel2_Resize);
		this.panel2.Controls.Add(this.ghostPanel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.ghostPanel5);
		this.panel2.Controls.Add(this.menuStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(4, 29);
		this.panel2.Margin = new System.Windows.Forms.Padding(0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1012, 450);
		this.panel2.TabIndex = 10;
		this.panel2.Click += new System.EventHandler(panel2_Click);
		this.ghostPanel4.Controls.Add(this.groupBox1);
		this.ghostPanel4.Controls.Add(this.splitter1);
		this.ghostPanel4.Controls.Add(this.panel4);
		this.ghostPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ghostPanel4.Location = new System.Drawing.Point(0, 53);
		this.ghostPanel4.Name = "ghostPanel4";
		this.ghostPanel4.Size = new System.Drawing.Size(1012, 380);
		this.ghostPanel4.TabIndex = 7;
		this.splitter1.Location = new System.Drawing.Point(368, 0);
		this.splitter1.Name = "splitter1";
		this.splitter1.Size = new System.Drawing.Size(3, 380);
		this.splitter1.TabIndex = 7;
		this.splitter1.TabStop = false;
		this.panel4.Controls.Add(this.tabControl1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.MinimumSize = new System.Drawing.Size(368, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(368, 380);
		this.panel4.TabIndex = 6;
		this.panel3.BackColor = System.Drawing.Color.Blue;
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(0, 433);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1012, 17);
		this.panel3.TabIndex = 5;
		this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(0, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1012, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "準備完了";
		this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.label2.Click += new System.EventHandler(label2_Click);
		this.ghostPanel5.AllowDrop = true;
		this.ghostPanel5.Controls.Add(this.label4);
		this.ghostPanel5.Controls.Add(this.macroSubDirCmb);
		this.ghostPanel5.Controls.Add(this.buttonEx3);
		this.ghostPanel5.Controls.Add(this.ComPortList);
		this.ghostPanel5.Controls.Add(this.buttonEx2);
		this.ghostPanel5.Controls.Add(this.label3);
		this.ghostPanel5.Controls.Add(this.ComConnect);
		this.ghostPanel5.Controls.Add(this.macroSelCmb);
		this.ghostPanel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.ghostPanel5.Location = new System.Drawing.Point(0, 24);
		this.ghostPanel5.Name = "ghostPanel5";
		this.ghostPanel5.Size = new System.Drawing.Size(1012, 29);
		this.ghostPanel5.TabIndex = 8;
		this.ghostPanel5.DragDrop += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragDrop);
		this.ghostPanel5.DragEnter += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragEnter);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(521, 5);
		this.label4.Margin = new System.Windows.Forms.Padding(10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(11, 13);
		this.label4.TabIndex = 11;
		this.label4.Text = "/";
		this.macroSubDirCmb.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.macroSubDirCmb.BorderColor = System.Drawing.Color.FromArgb(65, 65, 67);
		this.macroSubDirCmb.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.macroSubDirCmb.ContentsCheck = true;
		this.macroSubDirCmb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.macroSubDirCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.macroSubDirCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.macroSubDirCmb.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.macroSubDirCmb.FormattingEnabled = true;
		this.macroSubDirCmb.Items.AddRange(new object[1] { "Default" });
		this.macroSubDirCmb.Location = new System.Drawing.Point(8, 0);
		this.macroSubDirCmb.Name = "macroSubDirCmb";
		this.macroSubDirCmb.Size = new System.Drawing.Size(133, 23);
		this.macroSubDirCmb.TabIndex = 0;
		this.macroSubDirCmb.SelectedIndexChanged += new System.EventHandler(macroSubDirCmb_SelectedIndexChanged);
		this.macroSubDirCmb.TextUpdate += new System.EventHandler(macroSubDirCmb_TextUpdate);
		this.buttonEx3.FlatAppearance.BorderSize = 0;
		this.buttonEx3.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.buttonEx3.Image = NX_Macro_Controller_VxV.Properties.Resources.B7;
		this.buttonEx3.Location = new System.Drawing.Point(396, 0);
		this.buttonEx3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.buttonEx3.Name = "buttonEx3";
		this.buttonEx3.Size = new System.Drawing.Size(31, 23);
		this.buttonEx3.TabIndex = 2;
		this.buttonEx3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.buttonEx3.UseVisualStyleBackColor = true;
		this.buttonEx3.Click += new System.EventHandler(buttonEx3_Click);
		this.ComPortList.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.ComPortList.BorderColor = System.Drawing.Color.FromArgb(65, 65, 67);
		this.ComPortList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.ComPortList.ContentsCheck = true;
		this.ComPortList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.ComPortList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.ComPortList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.ComPortList.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.ComPortList.FormattingEnabled = true;
		this.ComPortList.Items.AddRange(new object[1] { "接続先を選択" });
		this.ComPortList.Location = new System.Drawing.Point(178, 0);
		this.ComPortList.Name = "ComPortList";
		this.ComPortList.Size = new System.Drawing.Size(156, 23);
		this.ComPortList.TabIndex = 4;
		this.ComPortList.DropDown += new System.EventHandler(ComPortList_DropDown);
		this.ComPortList.Click += new System.EventHandler(comboBoxEx2_Click);
		this.ComPortList.Enter += new System.EventHandler(comboBoxEx2_Enter);
		this.buttonEx2.FlatAppearance.BorderSize = 0;
		this.buttonEx2.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.buttonEx2.Image = NX_Macro_Controller_VxV.Properties.Resources.B6;
		this.buttonEx2.Location = new System.Drawing.Point(433, 0);
		this.buttonEx2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.buttonEx2.Name = "buttonEx2";
		this.buttonEx2.Size = new System.Drawing.Size(75, 23);
		this.buttonEx2.TabIndex = 3;
		this.buttonEx2.Text = "読込";
		this.buttonEx2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.buttonEx2.UseVisualStyleBackColor = true;
		this.buttonEx2.Click += new System.EventHandler(buttonEx2_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(154, 5);
		this.label3.Margin = new System.Windows.Forms.Padding(10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(11, 13);
		this.label3.TabIndex = 6;
		this.label3.Text = "/";
		this.ComConnect.FlatAppearance.BorderSize = 0;
		this.ComConnect.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.ComConnect.Image = NX_Macro_Controller_VxV.Properties.Resources.B5;
		this.ComConnect.Location = new System.Drawing.Point(707, 0);
		this.ComConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ComConnect.Name = "ComConnect";
		this.ComConnect.Size = new System.Drawing.Size(75, 23);
		this.ComConnect.TabIndex = 5;
		this.ComConnect.Text = "接続";
		this.ComConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.ComConnect.UseVisualStyleBackColor = true;
		this.ComConnect.Click += new System.EventHandler(ComConnect_Click);
		this.macroSelCmb.BackColor = System.Drawing.Color.FromArgb(33, 33, 35);
		this.macroSelCmb.BorderColor = System.Drawing.Color.FromArgb(65, 65, 67);
		this.macroSelCmb.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.macroSelCmb.ContentsCheck = true;
		this.macroSelCmb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.macroSelCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.macroSelCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.macroSelCmb.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.macroSelCmb.FormattingEnabled = true;
		this.macroSelCmb.Items.AddRange(new object[1] { "マクロを選択" });
		this.macroSelCmb.Location = new System.Drawing.Point(545, 0);
		this.macroSelCmb.Name = "macroSelCmb";
		this.macroSelCmb.Size = new System.Drawing.Size(212, 23);
		this.macroSelCmb.TabIndex = 1;
		this.macroSelCmb.TextUpdate += new System.EventHandler(macroSelCmb_TextUpdate);
		this.timer1.Enabled = true;
		this.timer1.Interval = 20;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.fileSystemWatcher1.EnableRaisingEvents = true;
		this.fileSystemWatcher1.SynchronizingObject = this;
		this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(fileSystemWatcher1_Created);
		this.fileSystemWatcher1.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher1_Deleted);
		this.fileSystemWatcher1.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher1_Renamed);
		this.fileSystemWatcher2.EnableRaisingEvents = true;
		this.fileSystemWatcher2.SynchronizingObject = this;
		this.fileSystemWatcher2.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher2_Deleted);
		this.fileSystemWatcher2.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher2_Renamed);
		this.fileSystemWatcher3.EnableRaisingEvents = true;
		this.fileSystemWatcher3.SynchronizingObject = this;
		this.fileSystemWatcher3.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher3_Deleted);
		this.fileSystemWatcher3.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher3_Renamed);
		this.fileSystemWatcher4.EnableRaisingEvents = true;
		this.fileSystemWatcher4.SynchronizingObject = this;
		this.fileSystemWatcher5.EnableRaisingEvents = true;
		this.fileSystemWatcher5.SynchronizingObject = this;
		this.mouseHook1.MouseHooked += new HongliangSoft.Utilities.MouseHookedEventHandler(mouseHook1_MouseHooked_2);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1020, 485);
		base.Controls.Add(this.panel2);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.MainMenuStrip = this.menuStrip1;
		this.MinimumSize = new System.Drawing.Size(1036, 524);
		base.Name = "NXMC_VxV";
		base.Padding = new System.Windows.Forms.Padding(1);
		this.Text = "Switch Macro Controller ver2.00";
		base.Activated += new System.EventHandler(NXMC_VxV_Activated);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		base.Shown += new System.EventHandler(NXMC_VxV_Shown);
		base.ResizeBegin += new System.EventHandler(NXMC_VxV_ResizeBegin);
		base.ResizeEnd += new System.EventHandler(NXMC_VxV_ResizeEnd);
		base.SizeChanged += new System.EventHandler(NXMC_VxV_SizeChanged);
		base.Click += new System.EventHandler(NXMC_VxV_Click);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(NXMC_VxV_DragEnter);
		base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(NXMC_VxV_KeyPress);
		base.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(NXMC_VxV_PreviewKeyDown);
		base.Resize += new System.EventHandler(NXMC_VxV_Resize);
		base.Controls.SetChildIndex(this.panel2, 0);
		((System.ComponentModel.ISupportInitialize)this.CaptureScreen).EndInit();
		this.CaptureContext.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.ghostPanel7.ResumeLayout(false);
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.tabPage4.ResumeLayout(false);
		this.ghostPanel6.ResumeLayout(false);
		this.groupBoxEx1.ResumeLayout(false);
		this.groupBoxEx1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabControlEx1.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.tabPage6.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.ghostPanel4.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.ghostPanel5.ResumeLayout(false);
		this.ghostPanel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fileSystemWatcher5).EndInit();
		base.ResumeLayout(false);
	}
}

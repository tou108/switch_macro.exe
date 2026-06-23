using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HongliangSoft.Utilities;

namespace NX_Macro_Controller_VxV;

public class MacroShare : Form
{
	private int sortMode = 2;

	private int selectedpage = -1;

	private int selectedmypage = -1;

	private int selectedaupage = -1;

	private int selectedfavpage = -1;

	private int openedMacroid = -1;

	private string BeforeSearch = "";

	private bool mainSearching;

	public static bool Opened;

	private string uID = "";

	private string uPass = "";

	private List<Util.MacroList> macroList = new List<Util.MacroList>();

	private List<Util.MacroList> mymacroList = new List<Util.MacroList>();

	private List<Util.MacroList> aumacroList = new List<Util.MacroList>();

	private IContainer components;

	private TextBox textBox1;

	private Button button1;

	private TableLayoutPanel tableLayoutPanel1;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel userform;

	private Label label3;

	private Label label2;

	private Label label1;

	private PictureBox pictureBox1;

	private Panel nouserform;

	private Button userLogin;

	private TextBox userPassword;

	private Label label5;

	private TextBox userID;

	private Label label4;

	private Button button2;

	private TextBox textBox3;

	private TableLayoutPanel tableLayoutPanel2;

	private ScrollOnPanel panel2;

	private ScrollOnPanel scrollOnPanel1;

	private Button button3;

	private TableLayoutPanel tableLayoutPanel3;

	private Button button4;

	private Button button5;

	private TextBox textBox4;

	private Label label7;

	private Label label6;

	private TextBox textBox2;

	private Label label8;

	private TextBox textBox5;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem ダウンロードToolStripMenuItem;

	private ToolStripMenuItem お気に入りに登録ToolStripMenuItem;

	private ToolStripMenuItem 作成者のプロファイルToolStripMenuItem;

	private Panel panel3;

	private ScrollOnPanel scrollOnPanel2;

	private TableLayoutPanel tableLayoutPanel4;

	private Label label9;

	private Label label10;

	private PictureBox pictureBox2;

	private Button button6;

	private Button button7;

	private Button button8;

	private ContextMenuStrip contextMenuStrip2;

	private ToolStripMenuItem このユーザーをブラックリストに登録するToolStripMenuItem;

	private ContextMenuStrip contextMenuStrip3;

	private ToolStripMenuItem ユーザー名を変更するToolStripMenuItem;

	private ToolStripMenuItem アイコン画像を変更するToolStripMenuItem;

	private ContextMenuStrip contextMenuStrip4;

	private Button button9;

	private ToolStripMenuItem 編集ToolStripMenuItem;

	private ToolStripMenuItem 削除ToolStripMenuItem;

	private TabPage tabPage3;

	private ScrollOnPanel scrollOnPanel3;

	private TableLayoutPanel tableLayoutPanel5;

	private TableLayoutPanel tableLayoutPanel6;

	private ContextMenuStrip contextMenuStrip5;

	private ToolStripMenuItem ダウンロードToolStripMenuItem1;

	private ToolStripMenuItem 作成者のプロフィールToolStripMenuItem;

	private ToolStripSeparator toolStripMenuItem1;

	private ToolStripMenuItem お気に入りを解除ToolStripMenuItem;

	private Panel panel5;

	private Panel panel4;

	private ToolStripMenuItem マクロデータの詳細ToolStripMenuItem;

	private Panel panel6;

	private Button button11;

	private Button button10;

	private TextBox textBox6;

	private Label label14;

	private Label label12;

	private Label label13;

	private Label label15;

	private Label label11;

	private ToolStripMenuItem マクロデータの詳細ToolStripMenuItem1;

	private TabPage tabPage4;

	private Button button12;

	private Label label16;

	private TextBox textBox7;

	private Button button13;

	private ContextMenuStrip contextMenuStrip6;

	private ToolStripMenuItem 新着順ToolStripMenuItem;

	private ToolStripMenuItem dL数巡ToolStripMenuItem;

	private Label label17;

	private ToolStripMenuItem お気に入り数順ToolStripMenuItem;

	private ContextMenuStrip contextMenuStrip7;

	private ToolStripMenuItem ダウンロードToolStripMenuItem2;

	private ToolStripMenuItem お気に入りに登録ToolStripMenuItem1;

	private Label label18;

	private Button button14;

	private Button button16;

	private Button button15;

	private Label label19;

	public MacroShare()
	{
		InitializeComponent();
	}

	private void MacroShare_Load(object sender, EventArgs e)
	{
		tableLayoutPanel1.ColumnStyles[1].Width = 0f;
		tableLayoutPanel6.ColumnStyles[1].Width = 0f;
		textBox7.Text = string.Join("\r\n", GlobalVar.BlackList);
		ScrollOnPanel scrollOnPanel = panel2;
		scrollOnPanel.Scroll2 = (EventHandler)Delegate.Combine(scrollOnPanel.Scroll2, (EventHandler)delegate
		{
			button1.PerformClick();
		});
		panel2.MouseWheel += delegate
		{
			if (panel2.VerticalScroll.Maximum <= panel2.VerticalScroll.Value + panel2.Height)
			{
				button1.PerformClick();
			}
		};
		ScrollOnPanel scrollOnPanel2 = scrollOnPanel1;
		scrollOnPanel2.Scroll2 = (EventHandler)Delegate.Combine(scrollOnPanel2.Scroll2, (EventHandler)delegate
		{
			button3.PerformClick();
		});
		scrollOnPanel1.MouseWheel += delegate
		{
			if (scrollOnPanel1.VerticalScroll.Maximum <= scrollOnPanel1.VerticalScroll.Value + scrollOnPanel1.Height)
			{
				button3.PerformClick();
			}
		};
		ScrollOnPanel scrollOnPanel3 = this.scrollOnPanel2;
		scrollOnPanel3.Scroll2 = (EventHandler)Delegate.Combine(scrollOnPanel3.Scroll2, (EventHandler)delegate
		{
			ouMacroRead();
		});
		this.scrollOnPanel2.MouseWheel += delegate
		{
			if (this.scrollOnPanel2.VerticalScroll.Maximum <= this.scrollOnPanel2.VerticalScroll.Value + this.scrollOnPanel2.Height)
			{
				ouMacroRead();
			}
		};
		uID = KEYCONFIG.NetworkConfig.ID;
		uPass = KEYCONFIG.NetworkConfig.KEY;
		if (Util.CheckLogin(uID, uPass, first: false))
		{
			userform.Dock = DockStyle.Fill;
			nouserform.Visible = false;
			pictureBox1.Image = Util.GetUserImage(uID).ImageResize(pictureBox1.Width, pictureBox1.Height);
			label1.Text = Util.GetUserName(uID);
			label2.Text = uID;
			button3_Click_1(null, null);
		}
		else
		{
			nouserform.Dock = DockStyle.Fill;
			userform.Visible = false;
		}
		button1_Click(null, EventArgs.Empty);
		FavReload();
	}

	private void FavReload()
	{
		tableLayoutPanel5.Controls.Clear();
		List<Control> list = new List<Control>();
		for (int i = 0; i < GlobalVar.FavMacro.Count; i++)
		{
			try
			{
				Util.MacroList macroData = Util.GetMacroData(GlobalVar.FavMacro[i]);
				Image image = Util.GetUserImage(macroData.UserID).ImageResize(64, 64);
				if (image == null)
				{
					image = new Bitmap(100, 100);
				}
				ShareContent sc = new ShareContent();
				if (image != null)
				{
					sc.Image = image;
				}
				sc.Author = "作成者:" + Util.GetUserName(macroData.UserID) + "\r\n" + $"{macroData.DLCnt}件のダウンロード";
				sc.Direction = macroData.Direction;
				sc.Caption = macroData.MacroName;
				sc.MouseDown += delegate(object o, MouseEventArgs args)
				{
					foreach (ShareContent control in tableLayoutPanel5.Controls)
					{
						control.Selected = false;
					}
					selectedfavpage = tableLayoutPanel5.Controls.IndexOf(sc);
					sc.Selected = true;
					if (args.Button == MouseButtons.Right)
					{
						contextMenuStrip5.Show(Control.MousePosition);
					}
				};
				sc.Dock = DockStyle.Top;
				list.Add(sc);
			}
			catch
			{
				GlobalVar.FavMacro.RemoveAt(i);
			}
		}
		tableLayoutPanel5.SuspendLayout();
		tableLayoutPanel5.Controls.AddRange(list.ToArray());
		tableLayoutPanel5.ResumeLayout();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (mainSearching)
		{
			return;
		}
		string s = textBox1.Text.Trim();
		if (BeforeSearch != s)
		{
			tableLayoutPanel2.Controls.Clear();
			selectedpage = 0;
			BeforeSearch = s;
			macroList.Clear();
		}
		Task.Factory.StartNew(delegate
		{
			while (mainSearching)
			{
				Thread.Sleep(1);
			}
			mainSearching = true;
			Util.MacroList[] array = Util.SearchMacro(s, tableLayoutPanel2.Controls.Count, 10, sortMode);
			new ListViewItem();
			macroList.AddRange(array);
			List<Control> cs = new List<Control>();
			for (int i = 0; i < array.Length; i++)
			{
				Image image = Util.GetUserImage(array[i].UserID).ImageResize(64, 64);
				if (image == null)
				{
					image = new Bitmap(64, 64);
				}
				ShareContent sc = new ShareContent();
				if (image != null)
				{
					sc.Image = image;
				}
				if (!GlobalVar.BlackList.Contains(array[i].UserID))
				{
					sc.Author = "作成者:" + Util.GetUserName(array[i].UserID) + "\r\n" + $"{array[i].DLCnt}件のダウンロード";
					sc.Direction = array[i].Direction;
					sc.Caption = array[i].MacroName;
					sc.MouseDown += delegate(object o, MouseEventArgs args)
					{
						foreach (ShareContent control in tableLayoutPanel2.Controls)
						{
							control.Selected = false;
						}
						selectedpage = tableLayoutPanel2.Controls.IndexOf(sc);
						sc.Selected = true;
						if (args.Button == MouseButtons.Right)
						{
							contextMenuStrip1.Show(Control.MousePosition);
						}
					};
					sc.Dock = DockStyle.Top;
					cs.Add(sc);
				}
			}
			Invoke((Action)delegate
			{
				tableLayoutPanel2.SuspendLayout();
				tableLayoutPanel2.Controls.AddRange(cs.ToArray());
				tableLayoutPanel2.ResumeLayout();
			});
			mainSearching = false;
		});
	}

	private void userLogin_Click(object sender, EventArgs e)
	{
		string text = userID.Text;
		string text2 = userPassword.Text;
		if (Util.CheckLogin(text, text2))
		{
			KEYCONFIG.NetworkConfig.ID = text;
			KEYCONFIG.NetworkConfig.KEY = text2.GenerateHash(text);
			uID = KEYCONFIG.NetworkConfig.ID;
			uPass = KEYCONFIG.NetworkConfig.KEY;
			Util.SaveConfig();
			userform.Dock = DockStyle.Fill;
			nouserform.Visible = false;
			userform.Visible = true;
			pictureBox1.Image = Util.GetUserImage(uID);
			label1.Text = Util.GetUserName(uID);
			label2.Text = uID;
			button3_Click_1(null, null);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		string text = userID.Text;
		string text2 = userPassword.Text;
		if (text.IsNullOrEmpty() || text == "")
		{
			MessageBox.Show("ユーザーIDを入力してください。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (text.Length < 4 || text.Length > 20)
		{
			MessageBox.Show("ユーザーIDは4文字以上20文字以下である必要があります。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (text2.IsNullOrEmpty() || text2 == "")
		{
			MessageBox.Show("パスワードを入力してください。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (text2.Length < 10)
		{
			MessageBox.Show("パスワードは10文字以上である必要があります。。", "NX Macro Controller", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (Util.AddUser(text, text2, text))
		{
			KEYCONFIG.NetworkConfig.ID = text;
			KEYCONFIG.NetworkConfig.KEY = text2.GenerateHash(text);
			uID = KEYCONFIG.NetworkConfig.ID;
			uPass = KEYCONFIG.NetworkConfig.KEY;
			Util.SaveConfig();
			userform.Dock = DockStyle.Fill;
			nouserform.Visible = false;
			userform.Visible = true;
			pictureBox1.Image = Util.GetUserImage(uID);
			label1.Text = Util.GetUserName(uID);
			label2.Text = uID;
			button3_Click_1(null, null);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
	}

	private void textBox2_Leave(object sender, EventArgs e)
	{
	}

	private void textBox2_Enter(object sender, EventArgs e)
	{
	}

	private void textBox3_Leave(object sender, EventArgs e)
	{
		label1.Text = textBox3.Text;
		label1.Visible = true;
		textBox3.Visible = false;
		Util.ChangeUserName(uID, uPass, textBox3.Text, first: false);
	}

	private void label1_DoubleClick(object sender, EventArgs e)
	{
		textBox3.Text = label1.Text;
		label1.Visible = false;
		textBox3.Visible = true;
		textBox3.Focus();
		textBox3.SelectAll();
		textBox3.Refresh();
	}

	private void userform_Click(object sender, EventArgs e)
	{
		base.ActiveControl = null;
	}

	private void MacroShare_Click(object sender, EventArgs e)
	{
		base.ActiveControl = null;
	}

	private void nouserform_Click(object sender, EventArgs e)
	{
		base.ActiveControl = null;
	}

	private void tabPage2_Click(object sender, EventArgs e)
	{
		base.ActiveControl = null;
	}

	private void pictureBox1_DoubleClick(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string userid = uID;
			string password = uPass;
			Image image = Image.FromStream(new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)));
			Util.ChangeUserIcon(userid, password, image, first: false).ContinueWith(delegate
			{
				pictureBox1.Image = Util.GetUserImage(userid).ImageResize(pictureBox1.Width, pictureBox1.Height);
			});
		}
	}

	private void keyboardHook1_KeyboardHooked(object sender, KeyboardHookedEventArgs e)
	{
	}

	private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
	{
		base.ActiveControl = null;
	}

	private void tabControl1_Selected(object sender, TabControlEventArgs e)
	{
		base.ActiveControl = null;
	}

	private void flowLayoutPanel1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
	}

	private void flowLayoutPanel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
	}

	private void button3_Click_1(object sender, EventArgs e)
	{
		Util.MacroList[] array = Util.SearchMacro(uID, tableLayoutPanel3.Controls.Count, 10, 3);
		mymacroList.AddRange(array);
		List<Control> list = new List<Control>();
		string userName = Util.GetUserName(uID);
		for (int i = 0; i < array.Length; i++)
		{
			ShareContent sc = new ShareContent();
			sc.ShowImage = false;
			sc.Author = "作成者:" + userName;
			sc.Direction = array[i].Direction;
			sc.Caption = array[i].MacroName;
			sc.MouseDown += delegate(object o, MouseEventArgs args)
			{
				if (!button9.Visible)
				{
					foreach (ShareContent control in tableLayoutPanel3.Controls)
					{
						control.Selected = false;
					}
					selectedmypage = tableLayoutPanel3.Controls.IndexOf(sc);
					sc.Selected = true;
					if (args.Button == MouseButtons.Right)
					{
						contextMenuStrip4.Show(Control.MousePosition);
					}
				}
			};
			sc.Dock = DockStyle.Top;
			list.Add(sc);
		}
		tableLayoutPanel3.Controls.AddRange(list.ToArray());
	}

	private void myMacroReload()
	{
		mymacroList.Clear();
		tableLayoutPanel3.Controls.Clear();
		button3_Click_1(null, EventArgs.Empty);
	}

	private void button4_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			textBox2.Text = openFileDialog.FileName;
		}
	}

	private void textBox2_Enter_1(object sender, EventArgs e)
	{
		base.ActiveControl = null;
	}

	private void textBox2_KeyDown(object sender, KeyEventArgs e)
	{
		base.ActiveControl = null;
	}

	private void button5_Click(object sender, EventArgs e)
	{
		if (button9.Visible)
		{
			if (selectedmypage < 0)
			{
				return;
			}
			try
			{
				byte[] data = File.ReadAllBytes(textBox2.Text);
				if (textBox5.Text.Trim() == "")
				{
					throw new Exception();
				}
				try
				{
					Util.MacroUpdate(data, textBox5.Text.Trim(), textBox4.Text.Trim(), uID, uPass, mymacroList[selectedmypage].MacroID, first: false).ContinueWith(delegate
					{
						Invoke((Action)delegate
						{
							button5.Text = "アップロード";
							button9.Visible = false;
							myMacroReload();
							tableLayoutPanel2.Controls.Clear();
							selectedpage = 0;
							BeforeSearch = "";
							macroList.Clear();
							textBox2.Text = "";
							textBox5.Text = "";
							textBox4.Text = "";
						});
					});
					return;
				}
				catch
				{
					return;
				}
			}
			catch
			{
				MessageBox.Show("入力内容が不正です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
		}
		try
		{
			byte[] data2 = File.ReadAllBytes(textBox2.Text);
			if (textBox5.Text.Trim() == "")
			{
				throw new Exception();
			}
			try
			{
				Util.MacroUpload(data2, textBox5.Text, textBox4.Text, uID, uPass, first: false).ContinueWith(delegate
				{
					Invoke((Action)delegate
					{
						myMacroReload();
						tableLayoutPanel2.Controls.Clear();
						selectedpage = 0;
						BeforeSearch = "";
						macroList.Clear();
						textBox2.Text = "";
						textBox5.Text = "";
						textBox4.Text = "";
					});
				});
			}
			catch
			{
			}
		}
		catch
		{
			MessageBox.Show("入力内容が不正です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void 作成者のプロファイルToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string ouID = "0";
		if (tabControl1.SelectedIndex == 0)
		{
			panel5.Parent = panel3;
			panel6.Parent = null;
		}
		else
		{
			panel5.Parent = panel4;
			panel6.Parent = null;
		}
		if (tabControl1.SelectedIndex == 0)
		{
			tableLayoutPanel1.ColumnStyles[1].Width = 50f;
			ouID = macroList[selectedpage].UserID;
		}
		else
		{
			tableLayoutPanel6.ColumnStyles[1].Width = 50f;
			ouID = Util.GetMacroData(GlobalVar.FavMacro[selectedfavpage]).UserID;
		}
		Task.Factory.StartNew(delegate
		{
			Image im = Util.GetUserImage(ouID).ImageResize(pictureBox2.Width, pictureBox2.Height);
			string name = Util.GetUserName(ouID);
			Invoke((Action)delegate
			{
				pictureBox2.Image = im;
				label10.Text = name;
				label9.Text = ouID;
				ouMacroRead();
			});
		});
	}

	private void ouMacroRead()
	{
		string searchStr = label9.Text;
		aumacroList.Clear();
		tableLayoutPanel4.Controls.Clear();
		Util.MacroList[] array = Util.SearchMacro(searchStr, tableLayoutPanel4.Controls.Count, 10, 3);
		aumacroList.AddRange(array);
		List<Control> list = new List<Control>();
		string userName = Util.GetUserName(searchStr);
		for (int i = 0; i < array.Length; i++)
		{
			ShareContent sc = new ShareContent();
			sc.ShowImage = false;
			sc.Author = "作成者:" + userName;
			sc.Direction = array[i].Direction;
			sc.Caption = array[i].MacroName;
			sc.MouseDown += delegate(object o, MouseEventArgs args)
			{
				foreach (ShareContent control in tableLayoutPanel4.Controls)
				{
					control.Selected = false;
				}
				selectedaupage = tableLayoutPanel4.Controls.IndexOf(sc);
				sc.Selected = true;
				if (args.Button == MouseButtons.Right)
				{
					contextMenuStrip7.Show(Control.MousePosition);
				}
			};
			sc.Dock = DockStyle.Top;
			list.Add(sc);
		}
		tableLayoutPanel4.SuspendLayout();
		tableLayoutPanel4.Controls.AddRange(list.ToArray());
		tableLayoutPanel4.ResumeLayout();
	}

	private void button6_Click(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			tableLayoutPanel1.ColumnStyles[1].Width = 0f;
		}
		else
		{
			tableLayoutPanel6.ColumnStyles[1].Width = 0f;
		}
	}

	private void ユーザー名を変更するToolStripMenuItem_Click(object sender, EventArgs e)
	{
		label1_DoubleClick(null, EventArgs.Empty);
	}

	private void アイコン画像を変更するToolStripMenuItem_Click(object sender, EventArgs e)
	{
		pictureBox1_DoubleClick(null, EventArgs.Empty);
	}

	private void button8_Click(object sender, EventArgs e)
	{
		contextMenuStrip3.Show(Control.MousePosition);
	}

	private void button7_Click(object sender, EventArgs e)
	{
		contextMenuStrip2.Show(Control.MousePosition);
	}

	private void 編集ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		button9.Visible = true;
		button5.Text = "更新";
		textBox5.Text = mymacroList[selectedmypage].MacroName;
		textBox4.Text = mymacroList[selectedmypage].Direction;
	}

	private void button9_Click(object sender, EventArgs e)
	{
		button9.Visible = false;
		button5.Text = "アップロード";
	}

	private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (selectedmypage < 0)
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			Util.DeleteMacro(uID, uPass, mymacroList[selectedmypage].MacroID, first: false);
			Invoke((Action)delegate
			{
				button9.Visible = false;
				button5.Text = "アップロード";
				myMacroReload();
			});
		});
	}

	private void ダウンロードToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			if (Util.GetMacroData(macroList[selectedpage].MacroID).IsIncludeFiles && MessageBox.Show("このマクロにはリソースファイルが含まれます。\r\n悪意のあるスクリプトなどが埋め込まれている可能性もあるため、信頼できるマクロのみ自己責任で実行してください。\r\nマクロのダウンロードを続行しますか？", "NX Macro Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			Task.Factory.StartNew(delegate
			{
				byte[] bin = Util.MacroDownload(macroList[selectedpage].MacroID);
				Invoke((Action)delegate
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						FileName = macroList[selectedpage].MacroName + ".nxc",
						Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*"
					};
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						File.WriteAllBytes(saveFileDialog.FileName, bin);
					}
				});
			});
		}
		else
		{
			if (Util.GetMacroData(GlobalVar.FavMacro[selectedfavpage]).IsIncludeFiles && MessageBox.Show("このマクロにはリソースファイルが含まれます。\r\n悪意のあるスクリプトなどが埋め込まれている可能性もあるため、信頼できるマクロのみ自己責任で実行してください。\r\nマクロのダウンロードを続行しますか？", "NX Macro Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				return;
			}
			Task.Factory.StartNew(delegate
			{
				byte[] bin = Util.MacroDownload(GlobalVar.FavMacro[selectedfavpage]);
				Invoke((Action)delegate
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						FileName = Util.GetMacroData(GlobalVar.FavMacro[selectedfavpage]).MacroName + ".nxc",
						Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*"
					};
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						File.WriteAllBytes(saveFileDialog.FileName, bin);
					}
				});
			});
		}
	}

	private void お気に入りに登録ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (GlobalVar.FavMacro.Contains(macroList[selectedpage].MacroID))
		{
			return;
		}
		GlobalVar.FavMacro.Add(macroList[selectedpage].MacroID);
		Util.SaveConfig();
		Task.Factory.StartNew(delegate
		{
			Util.AddFavorite(macroList[selectedpage].MacroID);
			Invoke((Action)delegate
			{
				FavReload();
			});
		});
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		tableLayoutPanel1.ColumnStyles[1].Width = 0f;
		tableLayoutPanel6.ColumnStyles[1].Width = 0f;
	}

	private void お気に入りを解除ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		GlobalVar.FavMacro.RemoveAt(selectedfavpage);
		Util.SaveConfig();
		FavReload();
	}

	private void マクロデータの詳細ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		panel6.Dock = DockStyle.Fill;
		if (tabControl1.SelectedIndex == 0)
		{
			panel6.Parent = panel3;
			panel5.Parent = null;
		}
		else
		{
			panel6.Parent = panel4;
			panel5.Parent = null;
		}
		Util.MacroList ouID;
		if (tabControl1.SelectedIndex == 0)
		{
			tableLayoutPanel1.ColumnStyles[1].Width = 50f;
			ouID = macroList[selectedpage];
		}
		else
		{
			tableLayoutPanel6.ColumnStyles[1].Width = 50f;
			ouID = Util.GetMacroData(GlobalVar.FavMacro[selectedfavpage]);
		}
		openedMacroid = ouID.MacroID;
		Task.Factory.StartNew(delegate
		{
			int size = Util.GetMacroSize(ouID.MacroID);
			string aut = "作成者: " + Util.GetUserName(ouID.UserID);
			Invoke((Action)delegate
			{
				label12.Text = ouID.MacroName;
				label13.Text = aut;
				if (ouID.IsIncludeFiles)
				{
					label15.ForeColor = Color.Green;
					label15.Text = "このマクロにはリソースファイルが含まれます";
					label17.Text = "サイズ: " + $"{size:#,0} バイト";
					label19.Text = "DL: " + ouID.DLCnt + " 件   お気に入り: " + ouID.FavCnt + " 件";
					textBox6.Text = ouID.Direction;
					label18.Text = "投稿日時: " + Util.GetMacroTimeData(ouID.MacroID);
				}
				else
				{
					label15.ForeColor = SystemColors.GrayText;
					label15.Text = "サイズ: " + $"{size:#,0} バイト";
					label17.Text = "DL: " + ouID.DLCnt + " 件   お気に入り: " + ouID.FavCnt + " 件";
					label19.Text = "";
					textBox6.Text = ouID.Direction;
					label18.Text = "投稿日時: " + Util.GetMacroTimeData(ouID.MacroID);
				}
			});
		});
	}

	private void このユーザーをブラックリストに登録するToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string item = label9.Text.Trim();
		if (!GlobalVar.BlackList.Contains(item))
		{
			GlobalVar.BlackList.Add(item);
		}
		else
		{
			GlobalVar.BlackList.Remove(item);
		}
		textBox7.Text = string.Join("\r\n", GlobalVar.BlackList);
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		BeforeSearch = "";
		macroList.Clear();
	}

	private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
	{
		string item = label9.Text.Trim();
		if (GlobalVar.BlackList.Contains(item))
		{
			このユーザーをブラックリストに登録するToolStripMenuItem.Text = "このユーザーをブラックリストから解除する";
		}
		else
		{
			このユーザーをブラックリストに登録するToolStripMenuItem.Text = "このユーザーをブラックリストに登録する";
		}
	}

	private void button12_Click(object sender, EventArgs e)
	{
		string[] collection = textBox7.Text.Split(new string[8] { " ", "\u3000", "   ", ",", ".", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
		GlobalVar.BlackList.Clear();
		GlobalVar.BlackList.AddRange(collection);
		Util.SaveConfig();
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		BeforeSearch = "";
		macroList.Clear();
	}

	private void contextMenuStrip6_Opening(object sender, CancelEventArgs e)
	{
		for (int i = 0; i < 3; i++)
		{
			((ToolStripMenuItem)contextMenuStrip6.Items[i]).Checked = false;
		}
		((ToolStripMenuItem)contextMenuStrip6.Items[(sortMode != 2) ? ((sortMode == 0) ? 1 : 2) : 0]).Checked = true;
	}

	private void dL数巡ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		sortMode = 0;
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		BeforeSearch = "";
		macroList.Clear();
		button1.PerformClick();
	}

	private void お気に入り数順ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		sortMode = 1;
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		BeforeSearch = "";
		macroList.Clear();
		button1.PerformClick();
	}

	private void 新着順ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		sortMode = 2;
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		BeforeSearch = "";
		macroList.Clear();
		button1.PerformClick();
	}

	private void button13_Click(object sender, EventArgs e)
	{
		contextMenuStrip6.Show(Control.MousePosition);
	}

	private void ダウンロードToolStripMenuItem2_Click(object sender, EventArgs e)
	{
		if (Util.GetMacroData(aumacroList[selectedaupage].MacroID).IsIncludeFiles && MessageBox.Show("このマクロにはリソースファイルが含まれます。\r\n悪意のあるスクリプトなどが埋め込まれている可能性もあるため、信頼できるマクロのみ自己責任で実行してください。\r\nマクロのダウンロードを続行しますか？", "NX Macro Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			byte[] bin = Util.MacroDownload(aumacroList[selectedaupage].MacroID);
			Invoke((Action)delegate
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					FileName = aumacroList[selectedaupage].MacroName + ".nxc",
					Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*"
				};
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllBytes(saveFileDialog.FileName, bin);
				}
			});
		});
	}

	private void お気に入りに登録ToolStripMenuItem1_Click(object sender, EventArgs e)
	{
		if (GlobalVar.FavMacro.Contains(aumacroList[selectedaupage].MacroID))
		{
			return;
		}
		GlobalVar.FavMacro.Add(aumacroList[selectedaupage].MacroID);
		Util.SaveConfig();
		Task.Factory.StartNew(delegate
		{
			Util.AddFavorite(aumacroList[selectedaupage].MacroID);
			Invoke((Action)delegate
			{
				FavReload();
			});
		});
	}

	private void MacroShare_FormClosed(object sender, FormClosedEventArgs e)
	{
		Opened = false;
	}

	private void button10_Click(object sender, EventArgs e)
	{
		if (Util.GetMacroData(openedMacroid).IsIncludeFiles && MessageBox.Show("このマクロにはリソースファイルが含まれます。\r\n悪意のあるスクリプトなどが埋め込まれている可能性もあるため、信頼できるマクロのみ自己責任で実行してください。\r\nマクロのダウンロードを続行しますか？", "NX Macro Controller", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			byte[] bin = Util.MacroDownload(openedMacroid);
			Invoke((Action)delegate
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					FileName = Util.GetMacroData(openedMacroid).MacroName + ".nxc",
					Filter = "NX Macro Controller用マクロファイル(*.nxc)|*.nxc|すべてのファイル(*.*)|*.*"
				};
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllBytes(saveFileDialog.FileName, bin);
				}
			});
		});
	}

	private void button11_Click(object sender, EventArgs e)
	{
		if (GlobalVar.FavMacro.Contains(openedMacroid))
		{
			return;
		}
		GlobalVar.FavMacro.Add(openedMacroid);
		Util.SaveConfig();
		Task.Factory.StartNew(delegate
		{
			Util.AddFavorite(openedMacroid);
			Invoke((Action)delegate
			{
				FavReload();
			});
		});
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar == '\r')
		{
			button1.PerformClick();
			e.Handled = true;
		}
	}

	private void button14_Click(object sender, EventArgs e)
	{
		tableLayoutPanel2.Controls.Clear();
		selectedpage = 0;
		macroList.Clear();
		button1.PerformClick();
	}

	private void button16_Click(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			tableLayoutPanel1.ColumnStyles[1].Width = 0f;
		}
		else
		{
			tableLayoutPanel6.ColumnStyles[1].Width = 0f;
		}
	}

	private void button15_Click(object sender, EventArgs e)
	{
		byte[] data = Util.MacroDownload(openedMacroid);
		NMC nMC = new NMC();
		nMC.NMCRead(data);
		CodePreview codePreview = new CodePreview();
		codePreview.Code = nMC.Code;
		codePreview.StartPosition = FormStartPosition.CenterParent;
		codePreview.ShowDialog();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NX_Macro_Controller_VxV.MacroShare));
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.button14 = new System.Windows.Forms.Button();
		this.button13 = new System.Windows.Forms.Button();
		this.panel2 = new NX_Macro_Controller_VxV.ScrollOnPanel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.button16 = new System.Windows.Forms.Button();
		this.button15 = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.button11 = new System.Windows.Forms.Button();
		this.button10 = new System.Windows.Forms.Button();
		this.textBox6 = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.button7 = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.button6 = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.scrollOnPanel2 = new NX_Macro_Controller_VxV.ScrollOnPanel();
		this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.nouserform = new System.Windows.Forms.Panel();
		this.button2 = new System.Windows.Forms.Button();
		this.userLogin = new System.Windows.Forms.Button();
		this.userPassword = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.userID = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.userform = new System.Windows.Forms.Panel();
		this.button9 = new System.Windows.Forms.Button();
		this.button8 = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.textBox5 = new System.Windows.Forms.TextBox();
		this.button5 = new System.Windows.Forms.Button();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.button4 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.scrollOnPanel1 = new NX_Macro_Controller_VxV.ScrollOnPanel();
		this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
		this.scrollOnPanel3 = new NX_Macro_Controller_VxV.ScrollOnPanel();
		this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.button12 = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.textBox7 = new System.Windows.Forms.TextBox();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ダウンロードToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.お気に入りに登録ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.作成者のプロファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロデータの詳細ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.このユーザーをブラックリストに登録するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ユーザー名を変更するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.アイコン画像を変更するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip5 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ダウンロードToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.作成者のプロフィールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.マクロデータの詳細ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
		this.お気に入りを解除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip6 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.新着順ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.dL数巡ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.お気に入り数順ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip7 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.ダウンロードToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.お気に入りに登録ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.label19 = new System.Windows.Forms.Label();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		this.scrollOnPanel2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.nouserform.SuspendLayout();
		this.userform.SuspendLayout();
		this.scrollOnPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.tabPage3.SuspendLayout();
		this.tableLayoutPanel6.SuspendLayout();
		this.scrollOnPanel3.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		this.contextMenuStrip2.SuspendLayout();
		this.contextMenuStrip3.SuspendLayout();
		this.contextMenuStrip4.SuspendLayout();
		this.contextMenuStrip5.SuspendLayout();
		this.contextMenuStrip6.SuspendLayout();
		this.contextMenuStrip7.SuspendLayout();
		base.SuspendLayout();
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.Location = new System.Drawing.Point(6, 6);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(210, 22);
		this.textBox1.TabIndex = 0;
		this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(222, 6);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 1;
		this.button1.Text = "検索";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(832, 512);
		this.tableLayoutPanel1.TabIndex = 3;
		this.panel1.Controls.Add(this.button14);
		this.panel1.Controls.Add(this.button13);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.button1);
		this.panel1.Controls.Add(this.textBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Padding = new System.Windows.Forms.Padding(3);
		this.panel1.Size = new System.Drawing.Size(410, 506);
		this.panel1.TabIndex = 0;
		this.button14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button14.Location = new System.Drawing.Point(303, 6);
		this.button14.Name = "button14";
		this.button14.Size = new System.Drawing.Size(75, 23);
		this.button14.TabIndex = 2;
		this.button14.Text = "リロード";
		this.button14.UseVisualStyleBackColor = true;
		this.button14.Click += new System.EventHandler(button14_Click);
		this.button13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button13.Location = new System.Drawing.Point(381, 6);
		this.button13.Name = "button13";
		this.button13.Size = new System.Drawing.Size(23, 23);
		this.button13.TabIndex = 3;
		this.button13.Text = "…";
		this.button13.UseVisualStyleBackColor = true;
		this.button13.Click += new System.EventHandler(button13_Click);
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.AutoScroll = true;
		this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel2.Controls.Add(this.tableLayoutPanel2);
		this.panel2.Location = new System.Drawing.Point(6, 32);
		this.panel2.Margin = new System.Windows.Forms.Padding(0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(398, 471);
		this.panel2.TabIndex = 3;
		this.tableLayoutPanel2.AutoSize = true;
		this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.tableLayoutPanel2.ColumnCount = 1;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 1;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(396, 0);
		this.tableLayoutPanel2.TabIndex = 2;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(419, 3);
		this.panel3.Name = "panel3";
		this.panel3.Padding = new System.Windows.Forms.Padding(3);
		this.panel3.Size = new System.Drawing.Size(410, 506);
		this.panel3.TabIndex = 1;
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.pictureBox2);
		this.panel5.Controls.Add(this.button7);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.button6);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.scrollOnPanel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(404, 500);
		this.panel5.TabIndex = 13;
		this.panel6.Controls.Add(this.label19);
		this.panel6.Controls.Add(this.button16);
		this.panel6.Controls.Add(this.button15);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Controls.Add(this.label17);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.button11);
		this.panel6.Controls.Add(this.button10);
		this.panel6.Controls.Add(this.textBox6);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Controls.Add(this.label12);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Location = new System.Drawing.Point(42, 56);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(338, 392);
		this.panel6.TabIndex = 13;
		this.button16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button16.Location = new System.Drawing.Point(309, 6);
		this.button16.Name = "button16";
		this.button16.Size = new System.Drawing.Size(23, 23);
		this.button16.TabIndex = 18;
		this.button16.Text = "×";
		this.button16.UseVisualStyleBackColor = true;
		this.button16.Click += new System.EventHandler(button16_Click);
		this.button15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button15.Location = new System.Drawing.Point(218, 82);
		this.button15.Name = "button15";
		this.button15.Size = new System.Drawing.Size(114, 23);
		this.button15.TabIndex = 1;
		this.button15.Text = "コードプレビュー";
		this.button15.UseVisualStyleBackColor = true;
		this.button15.Click += new System.EventHandler(button15_Click);
		this.label18.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label18.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label18.Location = new System.Drawing.Point(199, 32);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(133, 13);
		this.label18.TabIndex = 17;
		this.label18.Text = "投稿日 2000-01-01";
		this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label17.AutoSize = true;
		this.label17.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label17.Location = new System.Drawing.Point(4, 65);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(41, 13);
		this.label17.TabIndex = 16;
		this.label17.Text = "サイズ:";
		this.label15.AutoSize = true;
		this.label15.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label15.Location = new System.Drawing.Point(4, 45);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(41, 13);
		this.label15.TabIndex = 15;
		this.label15.Text = "サイズ:";
		this.button11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button11.Location = new System.Drawing.Point(218, 111);
		this.button11.Name = "button11";
		this.button11.Size = new System.Drawing.Size(114, 23);
		this.button11.TabIndex = 2;
		this.button11.Text = "お気に入り";
		this.button11.UseVisualStyleBackColor = true;
		this.button11.Click += new System.EventHandler(button11_Click);
		this.button10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button10.Location = new System.Drawing.Point(218, 53);
		this.button10.Name = "button10";
		this.button10.Size = new System.Drawing.Size(114, 23);
		this.button10.TabIndex = 0;
		this.button10.Text = "ダウンロード";
		this.button10.UseVisualStyleBackColor = true;
		this.button10.Click += new System.EventHandler(button10_Click);
		this.textBox6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox6.Location = new System.Drawing.Point(7, 140);
		this.textBox6.Multiline = true;
		this.textBox6.Name = "textBox6";
		this.textBox6.ReadOnly = true;
		this.textBox6.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox6.Size = new System.Drawing.Size(325, 246);
		this.textBox6.TabIndex = 3;
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(4, 121);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(33, 13);
		this.label14.TabIndex = 11;
		this.label14.Text = "説明";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(0, 20);
		this.label12.TabIndex = 9;
		this.label13.AutoSize = true;
		this.label13.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label13.Location = new System.Drawing.Point(4, 26);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(49, 13);
		this.label13.TabIndex = 10;
		this.label13.Text = "作成者:";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(3, 84);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(217, 13);
		this.label11.TabIndex = 14;
		this.label11.Text = "このユーザーがアップロードしたマクロデータ";
		this.pictureBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox2.Location = new System.Drawing.Point(6, 6);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(75, 75);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 6;
		this.pictureBox2.TabStop = false;
		this.button7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button7.Location = new System.Drawing.Point(346, 6);
		this.button7.Name = "button7";
		this.button7.Size = new System.Drawing.Size(23, 23);
		this.button7.TabIndex = 2;
		this.button7.Text = "…";
		this.button7.UseVisualStyleBackColor = true;
		this.button7.Click += new System.EventHandler(button7_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(87, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(0, 20);
		this.label10.TabIndex = 7;
		this.button6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button6.Location = new System.Drawing.Point(375, 6);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(23, 23);
		this.button6.TabIndex = 3;
		this.button6.Text = "×";
		this.button6.UseVisualStyleBackColor = true;
		this.button6.Click += new System.EventHandler(button6_Click);
		this.label9.AutoSize = true;
		this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label9.Location = new System.Drawing.Point(88, 26);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(0, 13);
		this.label9.TabIndex = 8;
		this.scrollOnPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.scrollOnPanel2.AutoScroll = true;
		this.scrollOnPanel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.scrollOnPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.scrollOnPanel2.Controls.Add(this.tableLayoutPanel4);
		this.scrollOnPanel2.Location = new System.Drawing.Point(6, 100);
		this.scrollOnPanel2.Name = "scrollOnPanel2";
		this.scrollOnPanel2.Size = new System.Drawing.Size(392, 394);
		this.scrollOnPanel2.TabIndex = 10;
		this.tableLayoutPanel4.AutoSize = true;
		this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.tableLayoutPanel4.ColumnCount = 1;
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel4.Name = "tableLayoutPanel4";
		this.tableLayoutPanel4.RowCount = 1;
		this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel4.Size = new System.Drawing.Size(390, 0);
		this.tableLayoutPanel4.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(846, 544);
		this.tabControl1.TabIndex = 4;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(tabControl1_Selecting);
		this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(tabControl1_Selected);
		this.tabPage1.Controls.Add(this.tableLayoutPanel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(838, 518);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "検索";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add(this.nouserform);
		this.tabPage2.Controls.Add(this.userform);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(838, 518);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "マイプロフィール";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.tabPage2.Click += new System.EventHandler(tabPage2_Click);
		this.nouserform.Controls.Add(this.button2);
		this.nouserform.Controls.Add(this.userLogin);
		this.nouserform.Controls.Add(this.userPassword);
		this.nouserform.Controls.Add(this.label5);
		this.nouserform.Controls.Add(this.userID);
		this.nouserform.Controls.Add(this.label4);
		this.nouserform.Location = new System.Drawing.Point(616, 35);
		this.nouserform.Name = "nouserform";
		this.nouserform.Size = new System.Drawing.Size(467, 136);
		this.nouserform.TabIndex = 0;
		this.nouserform.Click += new System.EventHandler(nouserform_Click);
		this.button2.Location = new System.Drawing.Point(10, 95);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 2;
		this.button2.Text = "新規登録";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.userLogin.Location = new System.Drawing.Point(91, 95);
		this.userLogin.Name = "userLogin";
		this.userLogin.Size = new System.Drawing.Size(75, 23);
		this.userLogin.TabIndex = 3;
		this.userLogin.Text = "ログイン";
		this.userLogin.UseVisualStyleBackColor = true;
		this.userLogin.Click += new System.EventHandler(userLogin_Click);
		this.userPassword.Location = new System.Drawing.Point(10, 67);
		this.userPassword.Name = "userPassword";
		this.userPassword.PasswordChar = '*';
		this.userPassword.Size = new System.Drawing.Size(193, 22);
		this.userPassword.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(7, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 13);
		this.label5.TabIndex = 6;
		this.label5.Text = "パスワード";
		this.userID.Location = new System.Drawing.Point(10, 26);
		this.userID.Name = "userID";
		this.userID.Size = new System.Drawing.Size(193, 22);
		this.userID.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(7, 10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(62, 13);
		this.label4.TabIndex = 4;
		this.label4.Text = "ユーザーID";
		this.userform.Controls.Add(this.button9);
		this.userform.Controls.Add(this.button8);
		this.userform.Controls.Add(this.label8);
		this.userform.Controls.Add(this.textBox5);
		this.userform.Controls.Add(this.button5);
		this.userform.Controls.Add(this.textBox4);
		this.userform.Controls.Add(this.label7);
		this.userform.Controls.Add(this.label6);
		this.userform.Controls.Add(this.textBox2);
		this.userform.Controls.Add(this.button4);
		this.userform.Controls.Add(this.button3);
		this.userform.Controls.Add(this.scrollOnPanel1);
		this.userform.Controls.Add(this.label3);
		this.userform.Controls.Add(this.label2);
		this.userform.Controls.Add(this.pictureBox1);
		this.userform.Controls.Add(this.label1);
		this.userform.Controls.Add(this.textBox3);
		this.userform.Location = new System.Drawing.Point(6, 79);
		this.userform.Name = "userform";
		this.userform.Size = new System.Drawing.Size(566, 396);
		this.userform.TabIndex = 0;
		this.userform.Click += new System.EventHandler(userform_Click);
		this.button9.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button9.Location = new System.Drawing.Point(533, 173);
		this.button9.Name = "button9";
		this.button9.Size = new System.Drawing.Size(23, 23);
		this.button9.TabIndex = 18;
		this.button9.Text = "×";
		this.button9.UseVisualStyleBackColor = true;
		this.button9.Visible = false;
		this.button9.Click += new System.EventHandler(button9_Click);
		this.button8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button8.Location = new System.Drawing.Point(533, 10);
		this.button8.Name = "button8";
		this.button8.Size = new System.Drawing.Size(23, 23);
		this.button8.TabIndex = 17;
		this.button8.Text = "…";
		this.button8.UseVisualStyleBackColor = true;
		this.button8.Click += new System.EventHandler(button8_Click);
		this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(7, 224);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(33, 13);
		this.label8.TabIndex = 16;
		this.label8.Text = "名前";
		this.textBox5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox5.Location = new System.Drawing.Point(10, 240);
		this.textBox5.Name = "textBox5";
		this.textBox5.Size = new System.Drawing.Size(546, 22);
		this.textBox5.TabIndex = 4;
		this.button5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button5.Location = new System.Drawing.Point(446, 370);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(110, 23);
		this.button5.TabIndex = 6;
		this.button5.Text = "アップロード";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.textBox4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox4.Location = new System.Drawing.Point(10, 281);
		this.textBox4.Multiline = true;
		this.textBox4.Name = "textBox4";
		this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox4.Size = new System.Drawing.Size(546, 83);
		this.textBox4.TabIndex = 5;
		this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(7, 183);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(185, 13);
		this.label7.TabIndex = 12;
		this.label7.Text = "作成したマクロファイルのアップロード";
		this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(7, 265);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(46, 13);
		this.label6.TabIndex = 11;
		this.label6.Text = "説明文";
		this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox2.Location = new System.Drawing.Point(10, 199);
		this.textBox2.Name = "textBox2";
		this.textBox2.ReadOnly = true;
		this.textBox2.Size = new System.Drawing.Size(430, 22);
		this.textBox2.TabIndex = 2;
		this.textBox2.Enter += new System.EventHandler(textBox2_Enter_1);
		this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox2_KeyDown);
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button4.Location = new System.Drawing.Point(446, 199);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(110, 23);
		this.button4.TabIndex = 3;
		this.button4.Text = "ファイルを選択";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.button3.Location = new System.Drawing.Point(365, 69);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(75, 23);
		this.button3.TabIndex = 1;
		this.button3.Text = "button3";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Visible = false;
		this.button3.Click += new System.EventHandler(button3_Click_1);
		this.scrollOnPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.scrollOnPanel1.AutoScroll = true;
		this.scrollOnPanel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.scrollOnPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.scrollOnPanel1.Controls.Add(this.tableLayoutPanel3);
		this.scrollOnPanel1.Location = new System.Drawing.Point(10, 109);
		this.scrollOnPanel1.Name = "scrollOnPanel1";
		this.scrollOnPanel1.Size = new System.Drawing.Size(546, 58);
		this.scrollOnPanel1.TabIndex = 7;
		this.tableLayoutPanel3.AutoSize = true;
		this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.tableLayoutPanel3.ColumnCount = 1;
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel3.Name = "tableLayoutPanel3";
		this.tableLayoutPanel3.RowCount = 1;
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel3.Size = new System.Drawing.Size(544, 0);
		this.tableLayoutPanel3.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(7, 93);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(217, 13);
		this.label3.TabIndex = 3;
		this.label3.Text = "このユーザーがアップロードしたマクロデータ";
		this.label2.AutoSize = true;
		this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label2.Location = new System.Drawing.Point(88, 30);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(0, 13);
		this.label2.TabIndex = 2;
		this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox1.Location = new System.Drawing.Point(10, 10);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(75, 75);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 5;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.DoubleClick += new System.EventHandler(pictureBox1_DoubleClick);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(87, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(0, 20);
		this.label1.TabIndex = 1;
		this.label1.DoubleClick += new System.EventHandler(label1_DoubleClick);
		this.textBox3.BackColor = System.Drawing.SystemColors.ControlLight;
		this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox3.Font = new System.Drawing.Font("Segoe UI", 11.25f);
		this.textBox3.Location = new System.Drawing.Point(91, 10);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(242, 20);
		this.textBox3.TabIndex = 0;
		this.textBox3.Visible = false;
		this.textBox3.Leave += new System.EventHandler(textBox3_Leave);
		this.tabPage3.Controls.Add(this.tableLayoutPanel6);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(838, 518);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "お気に入り";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tableLayoutPanel6.ColumnCount = 2;
		this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.Controls.Add(this.scrollOnPanel3, 0, 0);
		this.tableLayoutPanel6.Controls.Add(this.panel4, 1, 0);
		this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
		this.tableLayoutPanel6.Name = "tableLayoutPanel6";
		this.tableLayoutPanel6.RowCount = 1;
		this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.Size = new System.Drawing.Size(832, 512);
		this.tableLayoutPanel6.TabIndex = 9;
		this.scrollOnPanel3.AutoScroll = true;
		this.scrollOnPanel3.BackColor = System.Drawing.SystemColors.AppWorkspace;
		this.scrollOnPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.scrollOnPanel3.Controls.Add(this.tableLayoutPanel5);
		this.scrollOnPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.scrollOnPanel3.Location = new System.Drawing.Point(6, 6);
		this.scrollOnPanel3.Margin = new System.Windows.Forms.Padding(6);
		this.scrollOnPanel3.Name = "scrollOnPanel3";
		this.scrollOnPanel3.Size = new System.Drawing.Size(404, 500);
		this.scrollOnPanel3.TabIndex = 8;
		this.tableLayoutPanel5.AutoSize = true;
		this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.tableLayoutPanel5.ColumnCount = 1;
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel5.Name = "tableLayoutPanel5";
		this.tableLayoutPanel5.RowCount = 1;
		this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.Size = new System.Drawing.Size(402, 0);
		this.tableLayoutPanel5.TabIndex = 0;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(416, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(416, 512);
		this.panel4.TabIndex = 9;
		this.tabPage4.Controls.Add(this.button12);
		this.tabPage4.Controls.Add(this.label16);
		this.tabPage4.Controls.Add(this.textBox7);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(838, 518);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "ブラックリスト";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.button12.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button12.Location = new System.Drawing.Point(752, 487);
		this.button12.Name = "button12";
		this.button12.Size = new System.Drawing.Size(75, 23);
		this.button12.TabIndex = 2;
		this.button12.Text = "更新";
		this.button12.UseVisualStyleBackColor = true;
		this.button12.Click += new System.EventHandler(button12_Click);
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(8, 12);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(146, 13);
		this.label16.TabIndex = 1;
		this.label16.Text = "改行又はスペースで区切る";
		this.textBox7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox7.Location = new System.Drawing.Point(11, 28);
		this.textBox7.Multiline = true;
		this.textBox7.Name = "textBox7";
		this.textBox7.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.textBox7.Size = new System.Drawing.Size(816, 453);
		this.textBox7.TabIndex = 0;
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.ダウンロードToolStripMenuItem, this.お気に入りに登録ToolStripMenuItem, this.作成者のプロファイルToolStripMenuItem, this.マクロデータの詳細ToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(172, 92);
		this.ダウンロードToolStripMenuItem.Name = "ダウンロードToolStripMenuItem";
		this.ダウンロードToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.ダウンロードToolStripMenuItem.Text = "ダウンロード";
		this.ダウンロードToolStripMenuItem.Click += new System.EventHandler(ダウンロードToolStripMenuItem_Click);
		this.お気に入りに登録ToolStripMenuItem.Name = "お気に入りに登録ToolStripMenuItem";
		this.お気に入りに登録ToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.お気に入りに登録ToolStripMenuItem.Text = "お気に入りに登録";
		this.お気に入りに登録ToolStripMenuItem.Click += new System.EventHandler(お気に入りに登録ToolStripMenuItem_Click);
		this.作成者のプロファイルToolStripMenuItem.Name = "作成者のプロファイルToolStripMenuItem";
		this.作成者のプロファイルToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.作成者のプロファイルToolStripMenuItem.Text = "作成者のプロフィール";
		this.作成者のプロファイルToolStripMenuItem.Click += new System.EventHandler(作成者のプロファイルToolStripMenuItem_Click);
		this.マクロデータの詳細ToolStripMenuItem.Name = "マクロデータの詳細ToolStripMenuItem";
		this.マクロデータの詳細ToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.マクロデータの詳細ToolStripMenuItem.Text = "マクロデータの詳細";
		this.マクロデータの詳細ToolStripMenuItem.Click += new System.EventHandler(マクロデータの詳細ToolStripMenuItem_Click);
		this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.このユーザーをブラックリストに登録するToolStripMenuItem });
		this.contextMenuStrip2.Name = "contextMenuStrip2";
		this.contextMenuStrip2.Size = new System.Drawing.Size(249, 26);
		this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip2_Opening);
		this.このユーザーをブラックリストに登録するToolStripMenuItem.Name = "このユーザーをブラックリストに登録するToolStripMenuItem";
		this.このユーザーをブラックリストに登録するToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
		this.このユーザーをブラックリストに登録するToolStripMenuItem.Text = "このユーザーをブラックリストに登録する";
		this.このユーザーをブラックリストに登録するToolStripMenuItem.Click += new System.EventHandler(このユーザーをブラックリストに登録するToolStripMenuItem_Click);
		this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.ユーザー名を変更するToolStripMenuItem, this.アイコン画像を変更するToolStripMenuItem });
		this.contextMenuStrip3.Name = "contextMenuStrip3";
		this.contextMenuStrip3.Size = new System.Drawing.Size(186, 48);
		this.ユーザー名を変更するToolStripMenuItem.Name = "ユーザー名を変更するToolStripMenuItem";
		this.ユーザー名を変更するToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
		this.ユーザー名を変更するToolStripMenuItem.Text = "ユーザー名を変更する";
		this.ユーザー名を変更するToolStripMenuItem.Click += new System.EventHandler(ユーザー名を変更するToolStripMenuItem_Click);
		this.アイコン画像を変更するToolStripMenuItem.Name = "アイコン画像を変更するToolStripMenuItem";
		this.アイコン画像を変更するToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
		this.アイコン画像を変更するToolStripMenuItem.Text = "アイコン画像を変更する";
		this.アイコン画像を変更するToolStripMenuItem.Click += new System.EventHandler(アイコン画像を変更するToolStripMenuItem_Click);
		this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.編集ToolStripMenuItem, this.削除ToolStripMenuItem });
		this.contextMenuStrip4.Name = "contextMenuStrip4";
		this.contextMenuStrip4.Size = new System.Drawing.Size(99, 48);
		this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
		this.編集ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
		this.編集ToolStripMenuItem.Text = "編集";
		this.編集ToolStripMenuItem.Click += new System.EventHandler(編集ToolStripMenuItem_Click);
		this.削除ToolStripMenuItem.Name = "削除ToolStripMenuItem";
		this.削除ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
		this.削除ToolStripMenuItem.Text = "削除";
		this.削除ToolStripMenuItem.Click += new System.EventHandler(削除ToolStripMenuItem_Click);
		this.contextMenuStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.ダウンロードToolStripMenuItem1, this.作成者のプロフィールToolStripMenuItem, this.マクロデータの詳細ToolStripMenuItem1, this.toolStripMenuItem1, this.お気に入りを解除ToolStripMenuItem });
		this.contextMenuStrip5.Name = "contextMenuStrip5";
		this.contextMenuStrip5.Size = new System.Drawing.Size(172, 98);
		this.ダウンロードToolStripMenuItem1.Name = "ダウンロードToolStripMenuItem1";
		this.ダウンロードToolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
		this.ダウンロードToolStripMenuItem1.Text = "ダウンロード";
		this.ダウンロードToolStripMenuItem1.Click += new System.EventHandler(ダウンロードToolStripMenuItem_Click);
		this.作成者のプロフィールToolStripMenuItem.Name = "作成者のプロフィールToolStripMenuItem";
		this.作成者のプロフィールToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.作成者のプロフィールToolStripMenuItem.Text = "作成者のプロフィール";
		this.作成者のプロフィールToolStripMenuItem.Click += new System.EventHandler(作成者のプロファイルToolStripMenuItem_Click);
		this.マクロデータの詳細ToolStripMenuItem1.Name = "マクロデータの詳細ToolStripMenuItem1";
		this.マクロデータの詳細ToolStripMenuItem1.Size = new System.Drawing.Size(171, 22);
		this.マクロデータの詳細ToolStripMenuItem1.Text = "マクロデータの詳細";
		this.マクロデータの詳細ToolStripMenuItem1.Click += new System.EventHandler(マクロデータの詳細ToolStripMenuItem_Click);
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
		this.お気に入りを解除ToolStripMenuItem.Name = "お気に入りを解除ToolStripMenuItem";
		this.お気に入りを解除ToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
		this.お気に入りを解除ToolStripMenuItem.Text = "お気に入りを解除";
		this.お気に入りを解除ToolStripMenuItem.Click += new System.EventHandler(お気に入りを解除ToolStripMenuItem_Click);
		this.contextMenuStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.新着順ToolStripMenuItem, this.dL数巡ToolStripMenuItem, this.お気に入り数順ToolStripMenuItem });
		this.contextMenuStrip6.Name = "contextMenuStrip6";
		this.contextMenuStrip6.Size = new System.Drawing.Size(151, 70);
		this.contextMenuStrip6.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip6_Opening);
		this.新着順ToolStripMenuItem.Name = "新着順ToolStripMenuItem";
		this.新着順ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
		this.新着順ToolStripMenuItem.Text = "新着順";
		this.新着順ToolStripMenuItem.Click += new System.EventHandler(新着順ToolStripMenuItem_Click);
		this.dL数巡ToolStripMenuItem.Name = "dL数巡ToolStripMenuItem";
		this.dL数巡ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
		this.dL数巡ToolStripMenuItem.Text = "ダウンロード数順";
		this.dL数巡ToolStripMenuItem.Click += new System.EventHandler(dL数巡ToolStripMenuItem_Click);
		this.お気に入り数順ToolStripMenuItem.Name = "お気に入り数順ToolStripMenuItem";
		this.お気に入り数順ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
		this.お気に入り数順ToolStripMenuItem.Text = "お気に入り数順";
		this.お気に入り数順ToolStripMenuItem.Click += new System.EventHandler(お気に入り数順ToolStripMenuItem_Click);
		this.contextMenuStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.ダウンロードToolStripMenuItem2, this.お気に入りに登録ToolStripMenuItem1 });
		this.contextMenuStrip7.Name = "contextMenuStrip7";
		this.contextMenuStrip7.Size = new System.Drawing.Size(159, 48);
		this.ダウンロードToolStripMenuItem2.Name = "ダウンロードToolStripMenuItem2";
		this.ダウンロードToolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
		this.ダウンロードToolStripMenuItem2.Text = "ダウンロード";
		this.ダウンロードToolStripMenuItem2.Click += new System.EventHandler(ダウンロードToolStripMenuItem2_Click);
		this.お気に入りに登録ToolStripMenuItem1.Name = "お気に入りに登録ToolStripMenuItem1";
		this.お気に入りに登録ToolStripMenuItem1.Size = new System.Drawing.Size(158, 22);
		this.お気に入りに登録ToolStripMenuItem1.Text = "お気に入りに登録";
		this.お気に入りに登録ToolStripMenuItem1.Click += new System.EventHandler(お気に入りに登録ToolStripMenuItem1_Click);
		this.label19.AutoSize = true;
		this.label19.ForeColor = System.Drawing.SystemColors.GrayText;
		this.label19.Location = new System.Drawing.Point(4, 85);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(41, 13);
		this.label19.TabIndex = 19;
		this.label19.Text = "サイズ:";
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(846, 544);
		base.Controls.Add(this.tabControl1);
		this.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		this.MinimumSize = new System.Drawing.Size(570, 430);
		base.Name = "MacroShare";
		base.ShowIcon = false;
		this.Text = "共有";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MacroShare_FormClosed);
		base.Load += new System.EventHandler(MacroShare_Load);
		base.Click += new System.EventHandler(MacroShare_Click);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		this.scrollOnPanel2.ResumeLayout(false);
		this.scrollOnPanel2.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.nouserform.ResumeLayout(false);
		this.nouserform.PerformLayout();
		this.userform.ResumeLayout(false);
		this.userform.PerformLayout();
		this.scrollOnPanel1.ResumeLayout(false);
		this.scrollOnPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.tabPage3.ResumeLayout(false);
		this.tableLayoutPanel6.ResumeLayout(false);
		this.scrollOnPanel3.ResumeLayout(false);
		this.scrollOnPanel3.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.tabPage4.PerformLayout();
		this.contextMenuStrip1.ResumeLayout(false);
		this.contextMenuStrip2.ResumeLayout(false);
		this.contextMenuStrip3.ResumeLayout(false);
		this.contextMenuStrip4.ResumeLayout(false);
		this.contextMenuStrip5.ResumeLayout(false);
		this.contextMenuStrip6.ResumeLayout(false);
		this.contextMenuStrip7.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

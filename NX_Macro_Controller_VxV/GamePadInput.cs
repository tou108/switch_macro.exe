using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;
using Vortice.XInput;

namespace NX_Macro_Controller_VxV;

public static class GamePadInput
{
	public static bool Connected;

	public static bool A;

	public static bool B;

	public static bool X;

	public static bool Y;

	public static bool R;

	public static bool L;

	public static bool ZR;

	public static bool ZL;

	public static bool UP;

	public static bool DOWN;

	public static bool RIGHT;

	public static bool LEFT;

	public static bool START;

	public static bool SELECT;

	public static bool HOME;

	public static bool CAPTURE;

	public static bool CLICK_R;

	public static bool CLICK_L;

	public static bool UP_R;

	public static bool DOWN_R;

	public static bool LEFT_R;

	public static bool RIGHT_R;

	public static bool UP_L;

	public static bool DOWN_L;

	public static bool LEFT_L;

	public static bool RIGHT_L;

	public static ulong LS_X;

	public static ulong LS_Y;

	public static ulong RS_X;

	public static ulong RS_Y;

	public static bool[] buttons_d = new bool[128];

	public static bool[] hatbuttons_d = new bool[4];

	public static bool[] sticks_d = new bool[12];

	public static bool[] buttons_d_ = new bool[128];

	public static bool[] hatbuttons_d_ = new bool[4];

	public static bool[] sticks_d_ = new bool[12];

	public static bool[] buttons_d_new = new bool[128];

	public static bool[] hatbuttons_d_new = new bool[4];

	public static bool[] sticks_d_new = new bool[12];

	public static bool[] buttons_x = new bool[16];

	public static bool[] sticks_x = new bool[8];

	public static bool[] triggers_x = new bool[2];

	public static string lastKey_d = "None";

	private static string _lastKey_d = "None";

	public static string lastKey_x = "None";

	private static string _lastKey_x = "None";

	public static string GetKeyD()
	{
		lastKey_d = _lastKey_d;
		for (int i = 0; i < 128; i++)
		{
			if (buttons_d_new[i])
			{
				_lastKey_d = "B" + i;
				return _lastKey_d;
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (hatbuttons_d_new[j])
			{
				_lastKey_d = "H" + j;
				return _lastKey_d;
			}
		}
		for (int k = 0; k < 12; k++)
		{
			if (sticks_d_new[k])
			{
				_lastKey_d = "S" + k;
				return _lastKey_d;
			}
		}
		_lastKey_d = "None";
		return _lastKey_d;
	}

	public static string GetKeyX()
	{
		lastKey_x = _lastKey_x;
		for (int i = 0; i < 16; i++)
		{
			if (buttons_x[i])
			{
				_lastKey_x = "B" + i;
				return _lastKey_x;
			}
		}
		for (int j = 0; j < 8; j++)
		{
			if (sticks_x[j])
			{
				_lastKey_x = "S" + j;
				return _lastKey_x;
			}
		}
		for (int k = 0; k < 2; k++)
		{
			if (triggers_x[k])
			{
				_lastKey_x = "T" + k;
				return _lastKey_x;
			}
		}
		_lastKey_x = "None";
		return _lastKey_x;
	}

	public static bool GetKeyFlagD(string key)
	{
		if (key == "")
		{
			return false;
		}
		if (key[0] == 'H')
		{
			return hatbuttons_d[int.Parse(key.Substring(1))];
		}
		if (key[0] == 'S')
		{
			return sticks_d[int.Parse(key.Substring(1))];
		}
		if (key[0] == 'B')
		{
			return buttons_d[int.Parse(key.Substring(1))];
		}
		return false;
	}

	public static bool GetKeyFlagX(string key)
	{
		if (key == "")
		{
			return false;
		}
		if (key[0] == 'T')
		{
			return triggers_x[int.Parse(key.Substring(1))];
		}
		if (key[0] == 'S')
		{
			return sticks_x[int.Parse(key.Substring(1))];
		}
		if (key[0] == 'B')
		{
			return buttons_x[int.Parse(key.Substring(1))];
		}
		return false;
	}

	public static void Start()
	{
		Task.Factory.StartNew(delegate
		{
			Connected = false;
			int num = -1;
			while (true)
			{
				try
				{
					if (num == -1)
					{
						for (int i = 0; i < 4; i++)
						{
							if (XInput.GetState(i, out var _))
							{
								num = i;
							}
						}
					}
					if (XInput.GetState(num, out var state2))
					{
						GlobalVar.TaskName[2] = "ゲームパッド(XInput) : 接続中";
						GlobalVar.MAINFORM.TaskView();
						Connected = true;
						while (XInput.GetState(num, out state2))
						{
							for (int j = 0; j < 16; j++)
							{
								int num2 = 1 << j;
								buttons_x[j] = ((uint)state2.Gamepad.Buttons & (uint)num2) != 0;
							}
							sticks_x[0] = state2.Gamepad.LeftThumbX < -16384;
							sticks_x[1] = state2.Gamepad.LeftThumbX > 16384;
							sticks_x[2] = state2.Gamepad.LeftThumbY < -16384;
							sticks_x[3] = state2.Gamepad.LeftThumbY > 16384;
							sticks_x[4] = state2.Gamepad.RightThumbX < -16384;
							sticks_x[5] = state2.Gamepad.RightThumbX > 16384;
							sticks_x[6] = state2.Gamepad.RightThumbY < -16384;
							sticks_x[7] = state2.Gamepad.RightThumbY > 16384;
							triggers_x[0] = state2.Gamepad.LeftTrigger > 128;
							triggers_x[1] = state2.Gamepad.RightTrigger > 128;
							A = GetKeyFlagX(KEYCONFIG.XiButton.A);
							B = GetKeyFlagX(KEYCONFIG.XiButton.B);
							X = GetKeyFlagX(KEYCONFIG.XiButton.X);
							Y = GetKeyFlagX(KEYCONFIG.XiButton.Y);
							START = GetKeyFlagX(KEYCONFIG.XiButton.START);
							HOME = GetKeyFlagX(KEYCONFIG.XiButton.HOME);
							ZR = GetKeyFlagX(KEYCONFIG.XiButton.ZR);
							ZL = GetKeyFlagX(KEYCONFIG.XiButton.ZL);
							R = GetKeyFlagX(KEYCONFIG.XiButton.R);
							L = GetKeyFlagX(KEYCONFIG.XiButton.L);
							CLICK_L = GetKeyFlagX(KEYCONFIG.XiButton.CLICKL);
							CLICK_R = GetKeyFlagX(KEYCONFIG.XiButton.CLICKR);
							SELECT = GetKeyFlagX(KEYCONFIG.XiButton.SELECT);
							CAPTURE = GetKeyFlagX(KEYCONFIG.XiButton.CAPTURE);
							UP = GetKeyFlagX(KEYCONFIG.XiDPad.UP);
							RIGHT = GetKeyFlagX(KEYCONFIG.XiDPad.RIGHT);
							DOWN = GetKeyFlagX(KEYCONFIG.XiDPad.DOWN);
							LEFT = GetKeyFlagX(KEYCONFIG.XiDPad.LEFT);
							LEFT_L = GetKeyFlagX(KEYCONFIG.XiAnalogL.LEFT);
							RIGHT_L = GetKeyFlagX(KEYCONFIG.XiAnalogL.RIGHT);
							UP_L = GetKeyFlagX(KEYCONFIG.XiAnalogL.UP);
							DOWN_L = GetKeyFlagX(KEYCONFIG.XiAnalogL.DOWN);
							LEFT_R = GetKeyFlagX(KEYCONFIG.XiAnalogR.LEFT);
							RIGHT_R = GetKeyFlagX(KEYCONFIG.XiAnalogR.RIGHT);
							UP_R = GetKeyFlagX(KEYCONFIG.XiAnalogR.UP);
							DOWN_R = GetKeyFlagX(KEYCONFIG.XiAnalogR.DOWN);
							if (KEYCONFIG.XiAnalogL.UP != "" && KEYCONFIG.XiAnalogL.UP[0] == 'S')
							{
								switch (int.Parse(KEYCONFIG.XiAnalogL.UP.Substring(1)) / 2)
								{
								case 0:
									LS_Y = (ulong)((ushort)(state2.Gamepad.LeftThumbX + 32768) >> 8);
									break;
								case 1:
									LS_Y = (ulong)((ushort)(state2.Gamepad.LeftThumbY + 32768) >> 8);
									break;
								case 2:
									LS_Y = (ulong)((ushort)(state2.Gamepad.RightThumbX + 32768) >> 8);
									break;
								case 3:
									LS_Y = (ulong)((ushort)(state2.Gamepad.RightThumbY + 32768) >> 8);
									break;
								}
							}
							if (KEYCONFIG.XiAnalogL.LEFT != "" && KEYCONFIG.XiAnalogL.LEFT[0] == 'S')
							{
								switch (int.Parse(KEYCONFIG.XiAnalogL.LEFT.Substring(1)) / 2)
								{
								case 0:
									LS_X = (ulong)((ushort)(state2.Gamepad.LeftThumbX + 32768) >> 8);
									break;
								case 1:
									LS_X = (ulong)((ushort)(state2.Gamepad.LeftThumbY + 32768) >> 8);
									break;
								case 2:
									LS_X = (ulong)((ushort)(state2.Gamepad.RightThumbX + 32768) >> 8);
									break;
								case 3:
									LS_X = (ulong)((ushort)(state2.Gamepad.RightThumbY + 32768) >> 8);
									break;
								}
							}
							if (KEYCONFIG.XiAnalogR.UP != "" && KEYCONFIG.XiAnalogR.UP[0] == 'S')
							{
								switch (int.Parse(KEYCONFIG.XiAnalogR.UP.Substring(1)) / 2)
								{
								case 0:
									RS_Y = (ulong)((ushort)(state2.Gamepad.LeftThumbX + 32768) >> 8);
									break;
								case 1:
									RS_Y = (ulong)((ushort)(state2.Gamepad.LeftThumbY + 32768) >> 8);
									break;
								case 2:
									RS_Y = (ulong)((ushort)(state2.Gamepad.RightThumbX + 32768) >> 8);
									break;
								case 3:
									RS_Y = (ulong)((ushort)(state2.Gamepad.RightThumbY + 32768) >> 8);
									break;
								}
							}
							if (KEYCONFIG.XiAnalogR.LEFT != "" && KEYCONFIG.XiAnalogR.LEFT[0] == 'S')
							{
								switch (int.Parse(KEYCONFIG.XiAnalogR.LEFT.Substring(1)) / 2)
								{
								case 0:
									RS_X = (ulong)((ushort)(state2.Gamepad.LeftThumbX + 32768) >> 8);
									break;
								case 1:
									RS_X = (ulong)((ushort)(state2.Gamepad.LeftThumbY + 32768) >> 8);
									break;
								case 2:
									RS_X = (ulong)((ushort)(state2.Gamepad.RightThumbX + 32768) >> 8);
									break;
								case 3:
									RS_X = (ulong)((ushort)(state2.Gamepad.RightThumbY + 32768) >> 8);
									break;
								}
							}
							LS_Y = (byte)(255 - LS_Y);
							RS_Y = (byte)(255 - RS_Y);
							GlobalVar.MAINFORM.Nmc.PadKeyFlag = getPadFlag();
							GlobalVar.MAINFORM.Nmc.PadKeyFlag = getPadFlag();
							Thread.Sleep(8);
						}
					}
				}
				catch
				{
				}
				num = -1;
				try
				{
					DirectInput directInput = new DirectInput();
					Guid guid = Guid.Empty;
					if (guid == Guid.Empty)
					{
						using IEnumerator<DeviceInstance> enumerator = directInput.GetDevices(SharpDX.DirectInput.DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices).GetEnumerator();
						if (enumerator.MoveNext())
						{
							guid = enumerator.Current.InstanceGuid;
						}
					}
					if (guid == Guid.Empty)
					{
						using IEnumerator<DeviceInstance> enumerator = directInput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AllDevices).GetEnumerator();
						if (enumerator.MoveNext())
						{
							guid = enumerator.Current.InstanceGuid;
						}
					}
					Joystick joystick = new Joystick(directInput, guid);
					joystick.Properties.BufferSize = 128;
					if (joystick.Properties.VendorId != 1406 || joystick.Properties.ProductId != 8201)
					{
						foreach (DeviceObjectInstance @object in joystick.GetObjects())
						{
							DeviceObjectTypeFlags flags = @object.ObjectId.Flags;
							if ((uint)(flags - 1) <= 2u)
							{
								ObjectProperties objectPropertiesById = joystick.GetObjectPropertiesById(@object.ObjectId);
								if (objectPropertiesById != null)
								{
									try
									{
										objectPropertiesById.Range = new InputRange(-1000, 1000);
									}
									catch (Exception)
									{
									}
								}
							}
						}
						GlobalVar.TaskName[2] = "ゲームパッド(DirectX) : 接続中";
						GlobalVar.MAINFORM.TaskView();
						Connected = true;
						int[] array = new int[12];
						while (true)
						{
							joystick.Acquire();
							joystick.Poll();
							JoystickState currentState = joystick.GetCurrentState();
							for (int k = 0; k < Math.Min(128, currentState.Buttons.Length); k++)
							{
								buttons_d[k] = currentState.Buttons[k];
							}
							hatbuttons_d[0] = (currentState.PointOfViewControllers[0] <= 5000 && currentState.PointOfViewControllers[0] > -1) || currentState.PointOfViewControllers[0] > 30000;
							hatbuttons_d[1] = currentState.PointOfViewControllers[0] <= 15000 && currentState.PointOfViewControllers[0] > 3000;
							hatbuttons_d[2] = currentState.PointOfViewControllers[0] <= 24000 && currentState.PointOfViewControllers[0] > 12000;
							hatbuttons_d[3] = currentState.PointOfViewControllers[0] <= 34000 && currentState.PointOfViewControllers[0] > 22000;
							sticks_d[0] = currentState.X < -400;
							sticks_d[1] = currentState.X > 400;
							sticks_d[2] = currentState.Y < -400;
							sticks_d[3] = currentState.Y > 400;
							sticks_d[4] = currentState.Z < -400;
							sticks_d[5] = currentState.Z > 400;
							sticks_d[6] = currentState.RotationX < -400;
							sticks_d[7] = currentState.RotationX > 400;
							sticks_d[8] = currentState.RotationY < -400;
							sticks_d[9] = currentState.RotationY > 400;
							sticks_d[10] = currentState.RotationZ < -400;
							sticks_d[11] = currentState.RotationZ > 400;
							array[0] = currentState.X;
							array[1] = currentState.Y;
							array[2] = currentState.Z;
							array[3] = currentState.RotationX;
							array[4] = currentState.RotationY;
							array[5] = currentState.RotationZ;
							for (int l = 0; l < 128; l++)
							{
								buttons_d_new[l] = !buttons_d_[l] && buttons_d[l];
								buttons_d_[l] = buttons_d[l];
							}
							for (int m = 0; m < 4; m++)
							{
								hatbuttons_d_new[m] = !hatbuttons_d_[m] && hatbuttons_d[m];
								hatbuttons_d_[m] = hatbuttons_d[m];
							}
							for (int n = 0; n < 12; n++)
							{
								sticks_d_new[n] = !sticks_d_[n] && sticks_d[n];
								sticks_d_[n] = sticks_d[n];
							}
							A = GetKeyFlagD(KEYCONFIG.DxButton.A);
							B = GetKeyFlagD(KEYCONFIG.DxButton.B);
							X = GetKeyFlagD(KEYCONFIG.DxButton.X);
							Y = GetKeyFlagD(KEYCONFIG.DxButton.Y);
							START = GetKeyFlagD(KEYCONFIG.DxButton.START);
							HOME = GetKeyFlagD(KEYCONFIG.DxButton.HOME);
							ZR = GetKeyFlagD(KEYCONFIG.DxButton.ZR);
							ZL = GetKeyFlagD(KEYCONFIG.DxButton.ZL);
							R = GetKeyFlagD(KEYCONFIG.DxButton.R);
							L = GetKeyFlagD(KEYCONFIG.DxButton.L);
							CLICK_L = GetKeyFlagD(KEYCONFIG.DxButton.CLICKL);
							CLICK_R = GetKeyFlagD(KEYCONFIG.DxButton.CLICKR);
							SELECT = GetKeyFlagD(KEYCONFIG.DxButton.SELECT);
							CAPTURE = GetKeyFlagD(KEYCONFIG.DxButton.CAPTURE);
							UP = GetKeyFlagD(KEYCONFIG.DxDPad.UP);
							RIGHT = GetKeyFlagD(KEYCONFIG.DxDPad.RIGHT);
							DOWN = GetKeyFlagD(KEYCONFIG.DxDPad.DOWN);
							LEFT = GetKeyFlagD(KEYCONFIG.DxDPad.LEFT);
							LEFT_L = GetKeyFlagD(KEYCONFIG.DxAnalogL.LEFT);
							RIGHT_L = GetKeyFlagD(KEYCONFIG.DxAnalogL.RIGHT);
							UP_L = GetKeyFlagD(KEYCONFIG.DxAnalogL.UP);
							DOWN_L = GetKeyFlagD(KEYCONFIG.DxAnalogL.DOWN);
							LEFT_R = GetKeyFlagD(KEYCONFIG.DxAnalogR.LEFT);
							RIGHT_R = GetKeyFlagD(KEYCONFIG.DxAnalogR.RIGHT);
							UP_R = GetKeyFlagD(KEYCONFIG.DxAnalogR.UP);
							DOWN_R = GetKeyFlagD(KEYCONFIG.DxAnalogR.DOWN);
							if (KEYCONFIG.DxAnalogL.LEFT != "" && KEYCONFIG.DxAnalogL.LEFT[0] == 'S')
							{
								int num3 = int.Parse(KEYCONFIG.DxAnalogL.LEFT.Substring(1)) / 2;
								LS_X = (uint)Math.Min(255, (array[num3] + 1000) * 256 / 2000);
							}
							if (KEYCONFIG.DxAnalogL.UP != "" && KEYCONFIG.DxAnalogL.UP[0] == 'S')
							{
								int num4 = int.Parse(KEYCONFIG.DxAnalogL.UP.Substring(1)) / 2;
								LS_Y = (uint)Math.Min(255, (array[num4] + 1000) * 256 / 2000);
							}
							if (KEYCONFIG.DxAnalogR.LEFT != "" && KEYCONFIG.DxAnalogR.LEFT[0] == 'S')
							{
								int num5 = int.Parse(KEYCONFIG.DxAnalogR.LEFT.Substring(1)) / 2;
								RS_X = (uint)Math.Min(255, (array[num5] + 1000) * 256 / 2000);
							}
							if (KEYCONFIG.DxAnalogR.UP != "" && KEYCONFIG.DxAnalogR.UP[0] == 'S')
							{
								int num6 = int.Parse(KEYCONFIG.DxAnalogR.UP.Substring(1)) / 2;
								RS_Y = (uint)Math.Min(255, (array[num6] + 1000) * 256 / 2000);
							}
							GlobalVar.MAINFORM.Nmc.PadKeyFlag = getPadFlag();
							Thread.Sleep(8);
						}
					}
					if (ProController.Open())
					{
						GlobalVar.TaskName[2] = "Nintendo Switch Proコントローラー : 接続中";
						GlobalVar.MAINFORM.TaskView();
						Connected = true;
						try
						{
							while (ProController.Connected())
							{
								A = ProController.GetButtonFlag(Joycon.Button.A);
								B = ProController.GetButtonFlag(Joycon.Button.B);
								X = ProController.GetButtonFlag(Joycon.Button.X);
								Y = ProController.GetButtonFlag(Joycon.Button.Y);
								START = ProController.GetButtonFlag(Joycon.Button.PLUS);
								HOME = ProController.GetButtonFlag(Joycon.Button.HOME);
								ZR = ProController.GetButtonFlag(Joycon.Button.ZR);
								ZL = ProController.GetButtonFlag(Joycon.Button.ZL);
								R = ProController.GetButtonFlag(Joycon.Button.R);
								L = ProController.GetButtonFlag(Joycon.Button.L);
								CLICK_L = ProController.GetButtonFlag(Joycon.Button.THUMB_L);
								CLICK_R = ProController.GetButtonFlag(Joycon.Button.THUMB_R);
								SELECT = ProController.GetButtonFlag(Joycon.Button.MINUS);
								CAPTURE = ProController.GetButtonFlag(Joycon.Button.CAPTURE);
								UP = ProController.GetButtonFlag(Joycon.Button.DPAD_UP);
								RIGHT = ProController.GetButtonFlag(Joycon.Button.DPAD_RIGHT);
								DOWN = ProController.GetButtonFlag(Joycon.Button.DPAD_DOWN);
								LEFT = ProController.GetButtonFlag(Joycon.Button.DPAD_LEFT);
								float[] stickL = ProController.GetStickL();
								float[] stickR = ProController.GetStickR();
								LEFT_L = (double)stickL[0] < -0.6;
								RIGHT_L = (double)stickL[0] > 0.6;
								UP_L = (double)stickL[1] > 0.6;
								DOWN_L = (double)stickL[1] < -0.6;
								LEFT_R = (double)stickR[0] < -0.6;
								RIGHT_R = (double)stickR[0] > 0.6;
								UP_R = (double)stickR[1] > 0.6;
								DOWN_R = (double)stickR[1] < -0.6;
								LS_X = (uint)(stickL[0] * 127f) + 128;
								LS_Y = (uint)(stickL[1] * 127f) + 128;
								LS_Y = (byte)(255 - LS_Y);
								RS_X = (uint)(stickR[0] * 127f) + 128;
								RS_Y = (uint)(stickR[1] * 127f) + 128;
								RS_Y = (byte)(255 - RS_Y);
								GlobalVar.MAINFORM.Nmc.PadKeyFlag = getPadFlag();
								Thread.Sleep(8);
							}
						}
						catch
						{
						}
						ProController.Close();
					}
				}
				catch (Exception)
				{
				}
				GlobalVar.TaskName[2] = "";
				GlobalVar.MAINFORM.TaskView();
				GlobalVar.MAINFORM.Nmc.PadKeyFlag = 9259542121117908992uL;
				Connected = false;
				Thread.Sleep(4000);
			}
		});
	}

	private static ulong getPadFlag()
	{
		if (!KEYCONFIG.ControlConfig.USEKEYBOARD || (KEYCONFIG.ControlConfig.NOTUSEDEACTIVATE && Form.ActiveForm != GlobalVar.MAINFORM) || (KEYCONFIG.ControlConfig.NOTUSERUNNINGMACRO && GlobalVar.MAINFORM.Nmc.Running))
		{
			return 9259542121117908992uL;
		}
		ulong num = 9259542121117908992uL;
		if (A)
		{
			num |= 8;
		}
		if (B)
		{
			num |= 4;
		}
		if (X)
		{
			num |= 2;
		}
		if (Y)
		{
			num |= 1;
		}
		if (ZL)
		{
			num |= 0x800000;
		}
		if (ZR)
		{
			num |= 0x80;
		}
		if (L)
		{
			num |= 0x400000;
		}
		if (R)
		{
			num |= 0x40;
		}
		if (UP)
		{
			num |= 0x20000;
		}
		if (RIGHT)
		{
			num |= 0x40000;
		}
		if (LEFT)
		{
			num |= 0x80000;
		}
		if (DOWN)
		{
			num |= 0x10000;
		}
		if (START)
		{
			num |= 0x200;
		}
		if (SELECT)
		{
			num |= 0x100;
		}
		if (HOME)
		{
			num |= 0x1000;
		}
		if (CAPTURE)
		{
			num |= 0x2000;
		}
		if (CLICK_L)
		{
			num |= 0x800;
		}
		if (CLICK_R)
		{
			num |= 0x400;
		}
		if (UP_L)
		{
			num &= 0xFFFFFF00FFFFFFFFuL;
			num |= 0;
		}
		if (DOWN_L)
		{
			num &= 0xFFFFFF00FFFFFFFFuL;
			num |= 0xFF00000000L;
		}
		if (LEFT_L)
		{
			num &= 0xFFFF00FFFFFFFFFFuL;
			num |= 0;
		}
		if (RIGHT_L)
		{
			num &= 0xFFFF00FFFFFFFFFFuL;
			num |= 0xFF0000000000L;
		}
		if (UP_R)
		{
			num &= 0xFF00FFFFFFFFFFFFuL;
			num |= 0;
		}
		if (DOWN_R)
		{
			num &= 0xFF00FFFFFFFFFFFFuL;
			num |= 0xFF000000000000L;
		}
		if (LEFT_R)
		{
			num &= 0xFFFFFFFFFFFFFFL;
			num |= 0;
		}
		if (RIGHT_R)
		{
			num &= 0xFFFFFFFFFFFFFFL;
			num |= 0xFF00000000000000uL;
		}
		if (LS_Y == 127)
		{
			LS_Y = 128uL;
		}
		if (RS_Y == 127)
		{
			RS_Y = 128uL;
		}
		if (KEYCONFIG.ControlConfig.USESTICKBINARY && (!GlobalVar.MAINFORM.KeyRecoding || !KEYCONFIG.ControlConfig.REC8AXIS))
		{
			num &= 0xFFFFFFFFu;
			num |= LS_X << 40;
			num |= LS_Y << 32;
			num |= RS_X << 56;
			num |= RS_Y << 48;
		}
		return num;
	}
}

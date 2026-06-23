using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Threading;

namespace NX_Macro_Controller_VxV;

public class Joycon
{
	public enum DebugType
	{
		NONE,
		ALL,
		COMMS,
		THREADING,
		IMU,
		RUMBLE,
		SHAKE
	}

	public enum state_ : uint
	{
		NOT_ATTACHED,
		DROPPED,
		NO_JOYCONS,
		ATTACHED,
		INPUT_MODE_0x30,
		IMU_DATA_OK
	}

	public enum Button
	{
		DPAD_DOWN,
		DPAD_RIGHT,
		DPAD_LEFT,
		DPAD_UP,
		SL,
		SR,
		MINUS,
		HOME,
		PLUS,
		CAPTURE,
		THUMB_L,
		L,
		ZL,
		B,
		A,
		Y,
		X,
		THUMB_R,
		R,
		ZR
	}

	private struct Rumble
	{
		public Queue<float[]> queue = new Queue<float[]>();

		public void set_vals(float low_freq, float high_freq, float amplitude)
		{
			float[] item = new float[3] { low_freq, high_freq, amplitude };
			if (queue.Count > 15)
			{
				queue.Dequeue();
			}
			queue.Enqueue(item);
		}

		public Rumble(float[] rumble_info)
		{
			queue.Enqueue(rumble_info);
		}

		private float clamp(float x, float min, float max)
		{
			if (x < min)
			{
				return min;
			}
			if (x > max)
			{
				return max;
			}
			return x;
		}

		private byte EncodeAmp(float amp)
		{
			if (amp == 0f)
			{
				return 0;
			}
			if ((double)amp < 0.117)
			{
				return (byte)((Math.Log(amp * 1000f, 2.0) * 32.0 - 96.0) / (5.0 - Math.Pow(amp, 2.0)) - 1.0);
			}
			if ((double)amp < 0.23)
			{
				return (byte)(Math.Log(amp * 1000f, 2.0) * 32.0 - 96.0 - 92.0);
			}
			return (byte)((Math.Log(amp * 1000f, 2.0) * 32.0 - 96.0) * 2.0 - 246.0);
		}

		public byte[] GetData()
		{
			byte[] array = new byte[8];
			float[] array2 = queue.Dequeue();
			if (array2[2] == 0f)
			{
				array[0] = 0;
				array[1] = 1;
				array[2] = 64;
				array[3] = 64;
			}
			else
			{
				array2[0] = clamp(array2[0], 40.875885f, 626.28613f);
				array2[1] = clamp(array2[1], 81.75177f, 1252.5723f);
				array2[2] = clamp(array2[2], 0f, 1f);
				ushort num = (ushort)((Math.Round(32.0 * Math.Log(array2[1] * 0.1f, 2.0)) - 96.0) * 4.0);
				byte b = (byte)(Math.Round(32.0 * Math.Log(array2[0] * 0.1f, 2.0)) - 64.0);
				byte b2 = EncodeAmp(array2[2]);
				ushort num2 = (ushort)(Math.Round((double)(int)b2) * 0.5);
				byte num3 = (byte)(num2 % 2);
				if (num3 > 0)
				{
					num2--;
				}
				num2 >>= 1;
				num2 += 64;
				if (num3 > 0)
				{
					num2 |= 0x8000;
				}
				b2 = (byte)(b2 - b2 % 2);
				array[0] = (byte)(num & 0xFF);
				array[1] = (byte)(((num >> 8) & 0xFF) + b2);
				array[2] = (byte)(((num2 >> 8) & 0xFF) + b);
				array[3] = (byte)(num2 & 0xFF);
			}
			for (int i = 0; i < 4; i++)
			{
				array[4 + i] = array[i];
			}
			return array;
		}
	}

	public string path = string.Empty;

	public bool isPro;

	public bool isSnes;

	private bool isUSB;

	private Joycon _other;

	public bool active_gyro;

	private long inactivity = Stopwatch.GetTimestamp();

	public bool send = true;

	public DebugType debug_type = (DebugType)int.Parse(ConfigurationManager.AppSettings["DebugType"]);

	public bool isLeft;

	public state_ state;

	private bool[] buttons_down = new bool[20];

	private bool[] buttons_up = new bool[20];

	private bool[] buttons = new bool[20];

	private bool[] down_ = new bool[20];

	private long[] buttons_down_timestamp = new long[20];

	private float[] stick = new float[2];

	private float[] stick2 = new float[2];

	private IntPtr handle;

	private byte[] default_buf = new byte[8] { 0, 1, 64, 64, 0, 1, 64, 64 };

	private byte[] stick_raw = new byte[3];

	private ushort[] stick_cal = new ushort[6];

	private ushort deadzone;

	private ushort[] stick_precal = new ushort[2];

	private byte[] stick2_raw = new byte[3];

	private ushort[] stick2_cal = new ushort[6];

	private ushort deadzone2;

	private ushort[] stick2_precal = new ushort[2];

	private bool stop_polling = true;

	private bool imu_enabled;

	private short[] acc_r = new short[3];

	private short[] acc_neutral = new short[3];

	private short[] acc_sensiti = new short[3];

	private Vector3 acc_g;

	private short[] gyr_r = new short[3];

	private short[] gyr_neutral = new short[3];

	private short[] gyr_sensiti = new short[3];

	private Vector3 gyr_g;

	private float[] cur_rotation;

	private short[] acc_sen = new short[3] { 16384, 16384, 16384 };

	private short[] gyr_sen = new short[3] { 18642, 18642, 18642 };

	private short[] pro_hor_offset = new short[3] { -710, 0, 0 };

	private short[] left_hor_offset = new short[3];

	private short[] right_hor_offset = new short[3];

	private bool do_localize;

	private float filterweight;

	private const uint report_len = 49u;

	private Rumble rumble_obj;

	private byte global_count;

	private string debug_str;

	public int PadId;

	public int battery = -1;

	public int model = 2;

	public int constate = 2;

	public int connection = 3;

	public PhysicalAddress PadMacAddress = new PhysicalAddress(new byte[6] { 1, 2, 3, 4, 5, 6 });

	public ulong Timestamp;

	public int packetCounter;

	private ushort ds4_ts;

	private ulong lag;

	private int lowFreq = int.Parse(ConfigurationManager.AppSettings["LowFreqRumble"]);

	private int highFreq = int.Parse(ConfigurationManager.AppSettings["HighFreqRumble"]);

	private bool toRumble = bool.Parse(ConfigurationManager.AppSettings["EnableRumble"]);

	private bool showAsXInput = bool.Parse(ConfigurationManager.AppSettings["ShowAsXInput"]);

	private bool showAsDS4 = bool.Parse(ConfigurationManager.AppSettings["ShowAsDS4"]);

	public string serial_number;

	private bool thirdParty;

	private float[] activeData;

	private MadgwickAHRS AHRS = new MadgwickAHRS(0.005f, 0.01f);

	private byte ts_en;

	private readonly Stopwatch shakeTimer = Stopwatch.StartNew();

	private long shakedTime;

	private bool hasShaked;

	private bool dragToggle = bool.Parse(ConfigurationManager.AppSettings["DragToggle"]);

	private Dictionary<int, bool> mouse_toggle_btn = new Dictionary<int, bool>();

	private bool HomeLongPowerOff = bool.Parse(ConfigurationManager.AppSettings["HomeLongPowerOff"]);

	private long PowerOffInactivityMins = int.Parse(ConfigurationManager.AppSettings["PowerOffInactivity"]);

	private string extraGyroFeature = ConfigurationManager.AppSettings["GyroToJoyOrMouse"];

	private int GyroMouseSensitivityX = int.Parse(ConfigurationManager.AppSettings["GyroMouseSensitivityX"]);

	private int GyroMouseSensitivityY = int.Parse(ConfigurationManager.AppSettings["GyroMouseSensitivityY"]);

	private bool GyroHoldToggle = bool.Parse(ConfigurationManager.AppSettings["GyroHoldToggle"]);

	private bool GyroAnalogSliders = bool.Parse(ConfigurationManager.AppSettings["GyroAnalogSliders"]);

	private int GyroAnalogSensitivity = int.Parse(ConfigurationManager.AppSettings["GyroAnalogSensitivity"]);

	private byte[] sliderVal = new byte[2];

	private Thread PollThreadObj;

	public float[] otherStick = new float[2];

	private bool swapAB = bool.Parse(ConfigurationManager.AppSettings["SwapAB"]);

	private bool swapXY = bool.Parse(ConfigurationManager.AppSettings["SwapXY"]);

	public Joycon other
	{
		get
		{
			return _other;
		}
		set
		{
			_other = value;
			if (_other == null || _other == this)
			{
				SetLEDByPlayerNum(PadId);
				return;
			}
			int lEDByPlayerNum = Math.Min(_other.PadId, PadId);
			SetLEDByPlayerNum(lEDByPlayerNum);
		}
	}

	public byte LED { get; private set; }

	public void SetLEDByPlayerNum(int id)
	{
		if (id > 3)
		{
			id = 3;
		}
		if (ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["UseJoyconIncrementalLights"].Value.ToLower() == "true")
		{
			int num = id;
			LED = 0;
			do
			{
				LED |= (byte)(1 << num);
			}
			while (--num >= 0);
		}
		else
		{
			LED = (byte)(1 << id);
		}
		SetPlayerLED(LED);
	}

	public Joycon(IntPtr handle_, bool imu, bool localize, float alpha, bool left, string path, string serialNum, int id = 0, bool isPro = false, bool isSnes = false, bool thirdParty = false)
	{
		serial_number = serialNum;
		activeData = new float[6];
		handle = handle_;
		imu_enabled = imu;
		do_localize = localize;
		rumble_obj = new Rumble(new float[3] { lowFreq, highFreq, 0f });
		for (int i = 0; i < buttons_down_timestamp.Length; i++)
		{
			buttons_down_timestamp[i] = -1L;
		}
		filterweight = alpha;
		isLeft = left;
		PadId = id;
		LED = (byte)(1 << PadId);
		this.isPro = isPro || isSnes;
		this.isSnes = isSnes;
		isUSB = serialNum == "000000000001";
		this.thirdParty = thirdParty;
		this.path = path;
		connection = (isUSB ? 1 : 2);
	}

	public void DebugPrint(string s, DebugType d)
	{
		_ = debug_type;
	}

	public bool GetButtonDown(Button b)
	{
		return buttons_down[(int)b];
	}

	public bool GetButton(Button b)
	{
		return buttons[(int)b];
	}

	public bool GetButton(int b)
	{
		return buttons[b];
	}

	public bool GetButtonUp(Button b)
	{
		return buttons_up[(int)b];
	}

	public float[] GetStick()
	{
		return (float[])stick.Clone();
	}

	public float[] GetStick2()
	{
		return (float[])stick2.Clone();
	}

	public Vector3 GetGyro()
	{
		return gyr_g;
	}

	public Vector3 GetAccel()
	{
		return acc_g;
	}

	public int Attach(byte leds_ = 0)
	{
		state = state_.ATTACHED;
		HIDapi.hid_set_nonblocking(handle, 0);
		byte[] array = new byte[1];
		if (isUSB)
		{
			array = Enumerable.Repeat((byte)0, 64).ToArray();
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 100);
			if (array[0] == 48)
			{
				dump_calibration_data();
				return 0;
			}
			array[0] = 128;
			array[1] = 1;
			HIDapi.hid_write(handle, array, new UIntPtr(2u));
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 100);
			if (array[0] != 129)
			{
				Subcommand(6, new byte[1] { 1 }, 1u);
				return 1;
			}
			if (array[3] == 3)
			{
				PadMacAddress = new PhysicalAddress(new byte[6]
				{
					array[9],
					array[8],
					array[7],
					array[6],
					array[5],
					array[4]
				});
			}
			array = Enumerable.Repeat((byte)0, 64).ToArray();
			array[0] = 128;
			array[1] = 2;
			HIDapi.hid_write(handle, array, new UIntPtr(2u));
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 500);
			array[0] = 128;
			array[1] = 3;
			HIDapi.hid_write(handle, array, new UIntPtr(2u));
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 500);
			array[0] = 128;
			array[1] = 2;
			HIDapi.hid_write(handle, array, new UIntPtr(2u));
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 500);
			array[0] = 128;
			array[1] = 4;
			HIDapi.hid_write(handle, array, new UIntPtr(2u));
			HIDapi.hid_read_timeout(handle, array, new UIntPtr(64u), 500);
		}
		dump_calibration_data();
		_ = new byte[6];
		BlinkHomeLight();
		SetPlayerLED(leds_);
		Subcommand(3, new byte[1] { 48 }, 1u);
		Subcommand(72, new byte[1] { 1 }, 1u);
		Subcommand(64, new byte[1] { (byte)(imu_enabled ? 1 : 0) }, 1u);
		DebugPrint("Done with init.", DebugType.COMMS);
		HIDapi.hid_set_nonblocking(handle, 1);
		return 0;
	}

	public void SetPlayerLED(byte leds_ = 0)
	{
		Subcommand(48, new byte[1] { leds_ }, 1u);
	}

	public void BlinkHomeLight()
	{
		byte[] array = Enumerable.Repeat(byte.MaxValue, 25).ToArray();
		array[0] = 24;
		array[1] = 1;
		Subcommand(56, array, 25u);
	}

	public void SetHomeLight(bool on)
	{
		byte[] array = Enumerable.Repeat(byte.MaxValue, 25).ToArray();
		if (on)
		{
			array[0] = 31;
			array[1] = 240;
		}
		else
		{
			array[0] = 16;
			array[1] = 1;
		}
		Subcommand(56, array, 25u);
	}

	private void SetHCIState(byte state)
	{
		byte[] buf = new byte[1] { state };
		Subcommand(6, buf, 1u);
	}

	public void PowerOff()
	{
		if (state > state_.DROPPED)
		{
			HIDapi.hid_set_nonblocking(handle, 0);
			SetHCIState(0);
			state = state_.DROPPED;
		}
	}

	public void SetFilterCoeff(float a)
	{
		filterweight = a;
	}

	public void Detach(bool close = false)
	{
		stop_polling = true;
		if (state > state_.NO_JOYCONS)
		{
			HIDapi.hid_set_nonblocking(handle, 0);
			if (isUSB)
			{
				byte[] array = Enumerable.Repeat((byte)0, 64).ToArray();
				array[0] = 128;
				array[1] = 5;
				HIDapi.hid_write(handle, array, new UIntPtr(2u));
				array[0] = 128;
				array[1] = 6;
				HIDapi.hid_write(handle, array, new UIntPtr(2u));
			}
		}
		if (close || state > state_.DROPPED)
		{
			HIDapi.hid_close(handle);
		}
		state = state_.NOT_ATTACHED;
	}

	private int ReceiveRaw()
	{
		if (handle == IntPtr.Zero)
		{
			return -2;
		}
		byte[] array = new byte[49];
		int num = HIDapi.hid_read_timeout(handle, array, new UIntPtr(49u), 50);
		if (num > 0)
		{
			for (int i = 0; i < 3; i++)
			{
				ExtractIMUValues(array, i);
				byte b = (byte)Math.Max(0, array[1] - ts_en - 3);
				if (i == 0)
				{
					Timestamp += (ulong)((long)b * 5000L);
					ProcessButtonsAndStick(array);
					DoThingsWithButtons();
					_ = battery;
					battery = (array[2] >> 4) / 2;
				}
				Timestamp += 5000uL;
				packetCounter++;
			}
			if (ts_en == array[1] && !isSnes)
			{
				DebugPrint($"Duplicate timestamp enqueued. TS: {ts_en:X2}", DebugType.THREADING);
			}
			ts_en = array[1];
			DebugPrint($"Enqueue. Bytes read: {num:D}. Timestamp: {array[1]:X2}", DebugType.THREADING);
		}
		return num;
	}

	private void SimulateContinous(int origin, string s)
	{
		if (s.StartsWith("joy_"))
		{
			int num = int.Parse(s.Substring(4));
			ref bool reference = ref buttons[num];
			reference |= buttons[origin];
		}
	}

	private void DoThingsWithButtons()
	{
		int num = ((isPro || !isLeft || other != null) ? 7 : 9);
		long timestamp = Stopwatch.GetTimestamp();
		if (HomeLongPowerOff && buttons[num] && (double)((timestamp - buttons_down_timestamp[num]) / 10000) > 2000.0)
		{
			if (other != null)
			{
				other.PowerOff();
			}
			PowerOff();
			return;
		}
		if (PowerOffInactivityMins > 0 && (timestamp - inactivity) / 10000 > PowerOffInactivityMins * 60 * 1000)
		{
			if (other != null)
			{
				other.PowerOff();
			}
			PowerOff();
			return;
		}
		cur_rotation = AHRS.GetEulerAngles();
		if (GyroAnalogSliders && (other != null || isPro))
		{
			Button button = (isLeft ? Button.ZL : Button.ZR);
			Button button2 = (isLeft ? Button.ZR : Button.ZL);
			Joycon joycon = (isLeft ? this : (isPro ? this : other));
			Joycon joycon2 = ((!isLeft) ? this : (isPro ? this : other));
			int num2 = (int)((float)GyroAnalogSensitivity * (joycon.cur_rotation[0] - joycon.cur_rotation[3]));
			int num3 = (int)((float)GyroAnalogSensitivity * (joycon2.cur_rotation[0] - joycon2.cur_rotation[3]));
			if (buttons[(int)button])
			{
				sliderVal[0] = (byte)Math.Min(255, Math.Max(0, sliderVal[0] + num2));
			}
			else
			{
				sliderVal[0] = 0;
			}
			if (buttons[(int)button2])
			{
				sliderVal[1] = (byte)Math.Min(255, Math.Max(0, sliderVal[1] + num3));
			}
			else
			{
				sliderVal[1] = 0;
			}
		}
		if (!(extraGyroFeature == "joy") && extraGyroFeature == "mouse" && !isPro && other != null && other != null)
		{
			if (bool.Parse(ConfigurationManager.AppSettings["GyroMouseLeftHanded"]))
			{
				_ = isLeft;
			}
			else
				_ = !isLeft;
		}
	}

	private void Poll()
	{
		stop_polling = false;
		int num = 0;
		while (!stop_polling & (state > state_.NO_JOYCONS))
		{
			if (rumble_obj.queue.Count > 0)
			{
				SendRumble(rumble_obj.GetData());
			}
			int num2 = ReceiveRaw();
			if (num2 > 0 && state > state_.DROPPED)
			{
				state = state_.IMU_DATA_OK;
				num = 0;
				continue;
			}
			if (num > 240)
			{
				state = state_.DROPPED;
				DebugPrint("Connection lost. Is the Joy-Con connected?", DebugType.ALL);
				break;
			}
			if (num2 < 0)
			{
				Thread.Sleep(5);
				num++;
			}
		}
	}

	private int ProcessButtonsAndStick(byte[] report_buf)
	{
		if (report_buf[0] == 0)
		{
			throw new ArgumentException("received undefined report. This is probably a bug");
		}
		if (!isSnes)
		{
			stick_raw[0] = report_buf[6];
			stick_raw[1] = report_buf[7];
			stick_raw[2] = report_buf[8];
			if (isPro)
			{
				stick2_raw[0] = report_buf[9];
				stick2_raw[1] = report_buf[10];
				stick2_raw[2] = report_buf[11];
			}
			stick_precal[0] = (ushort)(stick_raw[0] | ((stick_raw[1] & 0xF) << 8));
			stick_precal[1] = (ushort)((stick_raw[1] >> 4) | (stick_raw[2] << 4));
			stick = CenterSticks(stick_precal, stick_cal, deadzone);
			stick2_precal[0] = (ushort)(stick2_raw[0] | ((stick2_raw[1] & 0xF) << 8));
			stick2_precal[1] = (ushort)((stick2_raw[1] >> 4) | (stick2_raw[2] << 4));
			stick2 = CenterSticks(stick2_precal, stick2_cal, deadzone2);
			if (isLeft && other != null && other != this)
			{
				stick2 = otherStick;
				other.otherStick = stick;
			}
			if (!isLeft && other != null && other != this)
			{
				Array.Copy(stick, stick2, 2);
				stick = otherStick;
				other.otherStick = stick2;
			}
		}
		lock (buttons)
		{
			lock (down_)
			{
				for (int i = 0; i < buttons.Length; i++)
				{
					down_[i] = buttons[i];
				}
			}
			buttons = new bool[20];
			buttons[0] = (report_buf[5] & 1) != 0;
			buttons[1] = (report_buf[5] & 4) != 0;
			buttons[3] = (report_buf[5] & 2) != 0;
			buttons[2] = (report_buf[5] & 8) != 0;
			buttons[7] = (report_buf[4] & 0x10) != 0;
			buttons[9] = (report_buf[4] & 0x20) != 0;
			buttons[6] = (report_buf[4] & 1) != 0;
			buttons[8] = (report_buf[4] & 2) != 0;
			buttons[10] = (report_buf[4] & 8) != 0;
			buttons[11] = (report_buf[5] & 0x40) != 0;
			buttons[12] = (report_buf[5] & 0x80) != 0;
			buttons[5] = (report_buf[3] & 0x10) != 0;
			buttons[4] = (report_buf[3] & 0x20) != 0;
			buttons[13] = (report_buf[3] & 4) != 0;
			buttons[14] = (report_buf[3] & 8) != 0;
			buttons[16] = (report_buf[3] & 2) != 0;
			buttons[15] = (report_buf[3] & 1) != 0;
			buttons[17] = (report_buf[4] & 4) != 0;
			buttons[18] = (report_buf[3] & 0x40) != 0;
			buttons[19] = (report_buf[3] & 0x80) != 0;
			long timestamp = Stopwatch.GetTimestamp();
			lock (buttons_up)
			{
				lock (buttons_down)
				{
					bool flag = false;
					for (int j = 0; j < buttons.Length; j++)
					{
						buttons_up[j] = down_[j] & !buttons[j];
						buttons_down[j] = !down_[j] & buttons[j];
						if (down_[j] != buttons[j])
						{
							buttons_down_timestamp[j] = (buttons[j] ? timestamp : (-1));
						}
						if (buttons_up[j] || buttons_down[j])
						{
							flag = true;
						}
					}
					inactivity = (flag ? timestamp : inactivity);
				}
			}
		}
		return 0;
	}

	private void ExtractIMUValues(byte[] report_buf, int n = 0)
	{
		if (isSnes)
		{
			return;
		}
		gyr_r[0] = (short)(report_buf[19 + n * 12] | ((report_buf[20 + n * 12] << 8) & 0xFF00));
		gyr_r[1] = (short)(report_buf[21 + n * 12] | ((report_buf[22 + n * 12] << 8) & 0xFF00));
		gyr_r[2] = (short)(report_buf[23 + n * 12] | ((report_buf[24 + n * 12] << 8) & 0xFF00));
		acc_r[0] = (short)(report_buf[13 + n * 12] | ((report_buf[14 + n * 12] << 8) & 0xFF00));
		acc_r[1] = (short)(report_buf[15 + n * 12] | ((report_buf[16 + n * 12] << 8) & 0xFF00));
		acc_r[2] = (short)(report_buf[17 + n * 12] | ((report_buf[18 + n * 12] << 8) & 0xFF00));
		short[] array = (isPro ? pro_hor_offset : ((!isLeft) ? right_hor_offset : left_hor_offset));
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				acc_g.X = (float)(acc_r[i] - array[i]) * (1f / (float)(acc_sensiti[i] - acc_neutral[i])) * 4f;
				gyr_g.X = (float)(gyr_r[i] - gyr_neutral[i]) * (816f / (float)(gyr_sensiti[i] - gyr_neutral[i]));
				break;
			case 1:
				acc_g.Y = (float)((isLeft ? 1 : (-1)) * (acc_r[i] - array[i])) * (1f / (float)(acc_sensiti[i] - acc_neutral[i])) * 4f;
				gyr_g.Y = (float)(-(isLeft ? 1 : (-1)) * (gyr_r[i] - gyr_neutral[i])) * (816f / (float)(gyr_sensiti[i] - gyr_neutral[i]));
				break;
			case 2:
				acc_g.Z = (float)((isLeft ? 1 : (-1)) * (acc_r[i] - array[i])) * (1f / (float)(acc_sensiti[i] - acc_neutral[i])) * 4f;
				gyr_g.Z = (float)(-(isLeft ? 1 : (-1)) * (gyr_r[i] - gyr_neutral[i])) * (816f / (float)(gyr_sensiti[i] - gyr_neutral[i]));
				break;
			}
		}
		if (other == null && !isPro)
		{
			if (isLeft)
			{
				acc_g.X = 0f - acc_g.X;
				acc_g.Y = 0f - acc_g.Y;
				gyr_g.X = 0f - gyr_g.X;
			}
			else
			{
				gyr_g.Y = 0f - gyr_g.Y;
			}
			float x = acc_g.X;
			acc_g.X = acc_g.Y;
			acc_g.Y = 0f - x;
			x = gyr_g.X;
			gyr_g.X = gyr_g.Y;
			gyr_g.Y = x;
		}
		float num = 0.0174533f;
		AHRS.Update(gyr_g.X * num, gyr_g.Y * num, gyr_g.Z * num, acc_g.X, acc_g.Y, acc_g.Z);
	}

	public void Begin()
	{
		if (PollThreadObj == null)
		{
			PollThreadObj = new Thread(Poll);
			PollThreadObj.IsBackground = true;
			PollThreadObj.Start();
		}
	}

	private float[] CenterSticks(ushort[] vals, ushort[] cal, ushort dz)
	{
		float[] array = new float[2];
		float num = vals[0] - cal[2];
		float num2 = vals[1] - cal[3];
		if (Math.Abs(num * num + num2 * num2) < (float)(dz * dz))
		{
			return array;
		}
		array[0] = Math.Min(1f, Math.Max(-1f, num / (float)(int)((num > 0f) ? cal[0] : cal[4]) / 0.85f));
		array[1] = Math.Min(1f, Math.Max(-1f, num2 / (float)(int)((num2 > 0f) ? cal[1] : cal[5]) / 0.85f));
		return array;
	}

	private static short CastStickValue(float stick_value)
	{
		return (short)Math.Max(-32768f, Math.Min(32767f, stick_value * (float)((stick_value > 0f) ? 32767 : 32768)));
	}

	private static byte CastStickValueByte(float stick_value)
	{
		return (byte)Math.Max(0f, Math.Min(255f, 127f - stick_value * 255f));
	}

	public void SetRumble(float low_freq, float high_freq, float amp)
	{
		if (state > state_.ATTACHED)
		{
			rumble_obj.set_vals(low_freq, high_freq, amp);
		}
	}

	private void SendRumble(byte[] buf)
	{
		byte[] array = new byte[49];
		array[0] = 16;
		array[1] = global_count;
		if (global_count == 15)
		{
			global_count = 0;
		}
		else
		{
			global_count++;
		}
		Array.Copy(buf, 0, array, 2, 8);
		PrintArray(array, DebugType.RUMBLE, 0u, 0u, "Rumble data sent: {0:S}");
		HIDapi.hid_write(handle, array, new UIntPtr(49u));
	}

	private byte[] Subcommand(byte sc, byte[] buf, uint len, bool print = true)
	{
		byte[] array = new byte[49];
		byte[] array2 = new byte[49];
		Array.Copy(default_buf, 0, array, 2, 8);
		Array.Copy(buf, 0L, array, 11L, len);
		array[10] = sc;
		array[1] = global_count;
		array[0] = 1;
		if (global_count == 15)
		{
			global_count = 0;
		}
		else
		{
			global_count++;
		}
		if (print)
		{
			PrintArray(array, DebugType.COMMS, len, 11u, "Subcommand 0x" + $"{sc:X2}" + " sent. Data: 0x{0:S}");
		}
		HIDapi.hid_write(handle, array, new UIntPtr(len + 11));
		int num = 0;
		do
		{
			if (HIDapi.hid_read_timeout(handle, array2, new UIntPtr(49u), 100) < 1)
			{
				DebugPrint("No response.", DebugType.COMMS);
			}
			else if (print)
			{
				PrintArray(array2, DebugType.COMMS, 48u, 1u, "Response ID 0x" + $"{array2[0]:X2}" + ". Data: 0x{0:S}");
			}
			num++;
		}
		while (num < 10 && array2[0] != 33 && array2[14] != sc);
		return array2;
	}

	private void dump_calibration_data()
	{
		if (isSnes || thirdParty)
		{
			return;
		}
		HIDapi.hid_set_nonblocking(handle, 0);
		byte[] array = ReadSPI(128, (byte)(isLeft ? 18 : 29), 9u);
		bool flag = false;
		for (int i = 0; i < 9; i++)
		{
			if (array[i] != byte.MaxValue)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			array = ReadSPI(96, (byte)(isLeft ? 61 : 70), 9u);
		}
		stick_cal[(!isLeft) ? 2 : 0] = (ushort)(((array[1] << 8) & 0xF00) | array[0]);
		stick_cal[isLeft ? 1 : 3] = (ushort)((array[2] << 4) | (array[1] >> 4));
		stick_cal[isLeft ? 2 : 4] = (ushort)(((array[4] << 8) & 0xF00) | array[3]);
		stick_cal[isLeft ? 3 : 5] = (ushort)((array[5] << 4) | (array[4] >> 4));
		stick_cal[isLeft ? 4 : 0] = (ushort)(((array[7] << 8) & 0xF00) | array[6]);
		stick_cal[(!isLeft) ? 1 : 5] = (ushort)((array[8] << 4) | (array[7] >> 4));
		PrintArray(stick_cal, DebugType.NONE, 6u, 0u, "Stick calibration data: {0:S}");
		array = ReadSPI(96, (byte)(isLeft ? 134 : 152), 16u);
		deadzone = (ushort)(((array[4] << 8) & 0xF00) | array[3]);
		if (isPro)
		{
			array = ReadSPI(128, (byte)((!isLeft) ? 18 : 29), 9u);
			flag = false;
			for (int j = 0; j < 9; j++)
			{
				if (array[j] != byte.MaxValue)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				array = ReadSPI(96, (byte)((!isLeft) ? 61 : 70), 9u);
			}
			stick2_cal[isLeft ? 2 : 0] = (ushort)(((array[1] << 8) & 0xF00) | array[0]);
			stick2_cal[(!isLeft) ? 1 : 3] = (ushort)((array[2] << 4) | (array[1] >> 4));
			stick2_cal[(!isLeft) ? 2 : 4] = (ushort)(((array[4] << 8) & 0xF00) | array[3]);
			stick2_cal[(!isLeft) ? 3 : 5] = (ushort)((array[5] << 4) | (array[4] >> 4));
			stick2_cal[(!isLeft) ? 4 : 0] = (ushort)(((array[7] << 8) & 0xF00) | array[6]);
			stick2_cal[isLeft ? 1 : 5] = (ushort)((array[8] << 4) | (array[7] >> 4));
			PrintArray(stick2_cal, DebugType.NONE, 6u, 0u, "Stick calibration data: {0:S}");
			array = ReadSPI(96, (byte)((!isLeft) ? 134 : 152), 16u);
			deadzone2 = (ushort)(((array[4] << 8) & 0xF00) | array[3]);
		}
		array = ReadSPI(128, 40, 10u);
		acc_neutral[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
		acc_neutral[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
		acc_neutral[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
		array = ReadSPI(128, 46, 10u);
		acc_sensiti[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
		acc_sensiti[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
		acc_sensiti[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
		array = ReadSPI(128, 52, 10u);
		gyr_neutral[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
		gyr_neutral[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
		gyr_neutral[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
		array = ReadSPI(128, 58, 10u);
		gyr_sensiti[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
		gyr_sensiti[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
		gyr_sensiti[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
		PrintArray(gyr_neutral, DebugType.IMU, 3u, 0u, "User gyro neutral position: {0:S}");
		if (gyr_neutral[0] + gyr_neutral[1] + gyr_neutral[2] == -3 || Math.Abs(gyr_neutral[0]) > 100 || Math.Abs(gyr_neutral[1]) > 100 || Math.Abs(gyr_neutral[2]) > 100)
		{
			array = ReadSPI(96, 32, 10u);
			acc_neutral[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
			acc_neutral[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
			acc_neutral[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
			array = ReadSPI(96, 38, 10u);
			acc_sensiti[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
			acc_sensiti[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
			acc_sensiti[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
			array = ReadSPI(96, 44, 10u);
			gyr_neutral[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
			gyr_neutral[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
			gyr_neutral[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
			array = ReadSPI(96, 50, 10u);
			gyr_sensiti[0] = (short)(array[0] | ((array[1] << 8) & 0xFF00));
			gyr_sensiti[1] = (short)(array[2] | ((array[3] << 8) & 0xFF00));
			gyr_sensiti[2] = (short)(array[4] | ((array[5] << 8) & 0xFF00));
			PrintArray(gyr_neutral, DebugType.IMU, 3u, 0u, "Factory gyro neutral position: {0:S}");
		}
		HIDapi.hid_set_nonblocking(handle, 1);
	}

	private byte[] ReadSPI(byte addr1, byte addr2, uint len, bool print = false)
	{
		byte[] buf = new byte[5]
		{
			addr2,
			addr1,
			0,
			0,
			(byte)len
		};
		byte[] array = new byte[len];
		byte[] array2 = new byte[len + 20];
		for (int i = 0; i < 100; i++)
		{
			array2 = Subcommand(16, buf, 5u, print: false);
			if (array2[15] == addr2 && array2[16] == addr1)
			{
				break;
			}
		}
		Array.Copy(array2, 20L, array, 0L, len);
		if (print)
		{
			PrintArray(array, DebugType.COMMS, len);
		}
		return array;
	}

	private void PrintArray<T>(T[] arr, DebugType d = DebugType.NONE, uint len = 0u, uint start = 0u, string format = "{0:S}")
	{
		if (d == debug_type || debug_type == DebugType.ALL)
		{
			if (len == 0)
			{
				len = (uint)arr.Length;
			}
			string text = "";
			for (int i = 0; i < len; i++)
			{
				text += string.Format((arr[0] is byte) ? "{0:X2} " : ((arr[0] is float) ? "{0:F} " : "{0:D} "), arr[i + start]);
			}
			DebugPrint(string.Format(format, text), d);
		}
	}
}

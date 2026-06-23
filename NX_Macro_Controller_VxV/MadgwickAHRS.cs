using System;

namespace NX_Macro_Controller_VxV;

public class MadgwickAHRS
{
	public float SamplePeriod { get; set; }

	public float Beta { get; set; }

	public float[] Quaternion { get; set; }

	public float[] old_pitchYawRoll { get; set; }

	public MadgwickAHRS(float samplePeriod)
		: this(samplePeriod, 1f)
	{
	}

	public MadgwickAHRS(float samplePeriod, float beta)
	{
		SamplePeriod = samplePeriod;
		Beta = beta;
		Quaternion = new float[4] { 1f, 0f, 0f, 0f };
		old_pitchYawRoll = new float[3];
	}

	public void Update(float gx, float gy, float gz, float ax, float ay, float az)
	{
		float num = Quaternion[0];
		float num2 = Quaternion[1];
		float num3 = Quaternion[2];
		float num4 = Quaternion[3];
		float num5 = 2f * num;
		float num6 = 2f * num2;
		float num7 = 2f * num3;
		float num8 = 2f * num4;
		float num9 = 4f * num;
		float num10 = 4f * num2;
		float num11 = 4f * num3;
		float num12 = 8f * num2;
		float num13 = 8f * num3;
		float num14 = num * num;
		float num15 = num2 * num2;
		float num16 = num3 * num3;
		float num17 = num4 * num4;
		float num18 = (float)Math.Sqrt(ax * ax + ay * ay + az * az);
		if (num18 != 0f)
		{
			num18 = 1f / num18;
			ax *= num18;
			ay *= num18;
			az *= num18;
			float num19 = num9 * num16 + num7 * ax + num9 * num15 - num6 * ay;
			float num20 = num10 * num17 - num8 * ax + 4f * num14 * num2 - num5 * ay - num10 + num12 * num15 + num12 * num16 + num10 * az;
			float num21 = 4f * num14 * num3 + num5 * ax + num11 * num17 - num8 * ay - num11 + num13 * num15 + num13 * num16 + num11 * az;
			float num22 = 4f * num15 * num4 - num6 * ax + 4f * num16 * num4 - num7 * ay;
			num18 = 1f / (float)Math.Sqrt(num19 * num19 + num20 * num20 + num21 * num21 + num22 * num22);
			num19 *= num18;
			num20 *= num18;
			num21 *= num18;
			num22 *= num18;
			float num23 = 0.5f * ((0f - num2) * gx - num3 * gy - num4 * gz) - Beta * num19;
			float num24 = 0.5f * (num * gx + num3 * gz - num4 * gy) - Beta * num20;
			float num25 = 0.5f * (num * gy - num2 * gz + num4 * gx) - Beta * num21;
			float num26 = 0.5f * (num * gz + num2 * gy - num3 * gx) - Beta * num22;
			num += num23 * SamplePeriod;
			num2 += num24 * SamplePeriod;
			num3 += num25 * SamplePeriod;
			num4 += num26 * SamplePeriod;
			num18 = 1f / (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3 + num4 * num4);
			Quaternion[0] = num * num18;
			Quaternion[1] = num2 * num18;
			Quaternion[2] = num3 * num18;
			Quaternion[3] = num4 * num18;
		}
	}

	public float[] GetEulerAngles()
	{
		float[] array = new float[3];
		float num = Quaternion[0];
		float num2 = Quaternion[1];
		float num3 = Quaternion[2];
		float num4 = Quaternion[3];
		float num5 = num2 * num2;
		float num6 = num3 * num3;
		float num7 = num4 * num4;
		array[0] = (float)Math.Asin(2f * (num * num3 - num4 * num2));
		array[1] = (float)Math.Atan2(2f * (num * num4 + num2 * num3), 1f - 2f * (num6 + num7));
		array[2] = (float)Math.Atan2(2f * (num * num2 + num3 * num4), 1f - 2f * (num5 + num6));
		float[] array2 = new float[6];
		Array.Copy(array, array2, 3);
		Array.Copy(old_pitchYawRoll, 0, array2, 3, 3);
		old_pitchYawRoll = array;
		return array2;
	}
}

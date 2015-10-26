using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class ColorExtensions
	{

		const float epsilon = 0.001F;

		public static Color SetValues(this Color color, Color values, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? values.r : color.r;
			color.g = channels.Contains(Channels.G) ? values.g : color.g;
			color.b = channels.Contains(Channels.B) ? values.b : color.b;
			color.a = channels.Contains(Channels.A) ? values.a : color.a;

			return color;
		}

		public static Color SetValues(this Color color, Color values)
		{
			return color.SetValues(values, Channels.RGBA);
		}

		public static Color SetValues(this Color color, float value, Channels channels)
		{
			return color.SetValues(new Color(value, value, value, value), channels);
		}

		public static Color SetValues(this Color color, float value)
		{
			return color.SetValues(new Color(value, value, value, value), Channels.RGBA);
		}

		public static Color Lerp(this Color color, Color target, float time, Channels channels)
		{
			color.r = channels.Contains(Channels.R) && Mathf.Abs(target.r - color.r) > epsilon ? Mathf.Lerp(color.r, target.r, time) : color.r;
			color.g = channels.Contains(Channels.G) && Mathf.Abs(target.g - color.g) > epsilon ? Mathf.Lerp(color.g, target.g, time) : color.g;
			color.b = channels.Contains(Channels.B) && Mathf.Abs(target.b - color.b) > epsilon ? Mathf.Lerp(color.b, target.b, time) : color.b;
			color.a = channels.Contains(Channels.A) && Mathf.Abs(target.a - color.a) > epsilon ? Mathf.Lerp(color.a, target.a, time) : color.a;

			return color;
		}

		public static Color Lerp(this Color color, Color target, float time)
		{
			return color.Lerp(target, time, Channels.RGBA);
		}

		public static Color LerpLinear(this Color color, Color target, float time, Channels channels)
		{
			Vector4 difference = target - color;
			Vector4 direction = Vector4.zero.SetValues(difference, (Axes)channels);
			float distance = direction.magnitude;

			Vector4 adjustedDirection = direction.normalized * time;

			if (adjustedDirection.magnitude < distance)
			{
				color += Color.clear.SetValues(adjustedDirection, channels);
			}
			else
			{
				color = color.SetValues(target, channels);
			}

			return color;
		}

		public static Color LerpLinear(this Color color, Color target, float time)
		{
			return color.LerpLinear(target, time, Channels.RGBA);
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, float offset, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? center.r + amplitude.r * Mathf.Sin(frequency.r * Time.time + offset) : color.r;
			color.g = channels.Contains(Channels.G) ? center.g + amplitude.g * Mathf.Sin(frequency.g * Time.time + offset) : color.g;
			color.b = channels.Contains(Channels.B) ? center.b + amplitude.b * Mathf.Sin(frequency.b * Time.time + offset) : color.b;
			color.a = channels.Contains(Channels.A) ? center.a + amplitude.a * Mathf.Sin(frequency.a * Time.time + offset) : color.a;

			return color;
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, float offset)
		{
			return color.Oscillate(frequency, amplitude, center, offset, Channels.RGBA);
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, Channels channels)
		{
			return color.Oscillate(frequency, amplitude, center, 0, channels);
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center)
		{
			return color.Oscillate(frequency, amplitude, center, 0, Channels.RGBA);
		}

		public static Color Mult(this Color color, Color otherVector, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? color.r * otherVector.r : color.r;
			color.g = channels.Contains(Channels.G) ? color.g * otherVector.g : color.g;
			color.b = channels.Contains(Channels.B) ? color.b * otherVector.b : color.b;
			color.a = channels.Contains(Channels.A) ? color.a * otherVector.a : color.a;

			return color;
		}

		public static Color Mult(this Color color, Color otherVector)
		{
			return color.Mult(otherVector, Channels.RGBA);
		}

		public static Color Div(this Color color, Color otherVector, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? color.r / otherVector.r : color.r;
			color.g = channels.Contains(Channels.G) ? color.g / otherVector.g : color.g;
			color.b = channels.Contains(Channels.B) ? color.b / otherVector.b : color.b;
			color.a = channels.Contains(Channels.A) ? color.a / otherVector.a : color.a;

			return color;
		}

		public static Color Div(this Color color, Color otherVector)
		{
			return color.Div(otherVector, Channels.RGBA);
		}

		public static Color Pow(this Color color, float power, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? color.r.Pow(power) : color.r;
			color.g = channels.Contains(Channels.G) ? color.g.Pow(power) : color.g;
			color.b = channels.Contains(Channels.B) ? color.b.Pow(power) : color.b;
			color.a = channels.Contains(Channels.A) ? color.a.Pow(power) : color.a;

			return color;
		}

		public static Color Pow(this Color color, float power)
		{
			return color.Pow(power, Channels.RGBA);
		}

		public static Color Round(this Color color, float step, Channels channels)
		{
			color.r = channels.Contains(Channels.R) ? color.r.Round(step) : color.r;
			color.g = channels.Contains(Channels.G) ? color.g.Round(step) : color.g;
			color.b = channels.Contains(Channels.B) ? color.b.Round(step) : color.b;
			color.a = channels.Contains(Channels.A) ? color.a.Round(step) : color.a;

			return color;
		}

		public static Color Round(this Color color, float step)
		{
			return color.Round(step, Channels.RGBA);
		}

		public static Color Round(this Color color)
		{
			return color.Round(1, Channels.RGBA);
		}

		public static float Average(this Color color, Channels channels)
		{
			float average = 0;
			int axisCount = 0;

			if (channels.Contains(Channels.R))
			{
				average += color.r;
				axisCount += 1;
			}

			if (channels.Contains(Channels.G))
			{
				average += color.g;
				axisCount += 1;
			}

			if (channels.Contains(Channels.B))
			{
				average += color.b;
				axisCount += 1;
			}

			if (channels.Contains(Channels.A))
			{
				average += color.a;
				axisCount += 1;
			}

			return average / axisCount;
		}

		public static float Average(this Color color)
		{
			return ((Color)color).Average(Channels.RGBA);
		}

		public static Color ToHSV(this Color RGBColor)
		{
			float R = RGBColor.r;
			float G = RGBColor.g;
			float B = RGBColor.b;
			float H = 0;
			float S = 0;
			float V = 0;
			float d = 0;
			float h = 0;

			float minRGB = Mathf.Min(R, Mathf.Min(G, B));
			float maxRGB = Mathf.Max(R, Mathf.Max(G, B));

			if (minRGB == maxRGB)
			{
				return new Color(0, 0, minRGB, RGBColor.a);
			}

			if (R == minRGB)
			{
				d = G - B;
			}
			else if (B == minRGB)
			{
				d = R - G;
			}
			else
			{
				d = B - R;
			}

			if (R == minRGB)
			{
				h = 3;
			}
			else if (B == minRGB)
			{
				h = 1;
			}
			else
			{
				h = 5;
			}

			H = (60 * (h - d / (maxRGB - minRGB))) / 360;
			S = (maxRGB - minRGB) / maxRGB;
			V = maxRGB;

			return new Color(H, S, V, RGBColor.a);
		}

		public static Color ToRGB(this Color HSVColor)
		{
			float H = HSVColor.r;
			float S = HSVColor.g;
			float V = HSVColor.b;
			float R = 0;
			float G = 0;
			float B = 0;
			float maxHSV = 255 * V;
			float minHSV = maxHSV * (1 - S);
			float h = H * 360;
			float z = (maxHSV - minHSV) * (1 - Mathf.Abs((h / 60) % 2 - 1));

			if (0 <= h && h < 60)
			{
				R = maxHSV;
				G = z + minHSV;
				B = minHSV;
			}
			else if (60 <= h && h < 120)
			{
				R = z + minHSV;
				G = maxHSV;
				B = minHSV;
			}
			else if (120 <= h && h < 180)
			{
				R = minHSV;
				G = maxHSV;
				B = z + minHSV;
			}
			else if (180 <= h && h < 240)
			{
				R = minHSV;
				G = z + minHSV;
				;
				B = maxHSV;
			}
			else if (240 <= h && h < 300)
			{
				R = z + minHSV;
				G = minHSV;
				B = maxHSV;
			}
			else if (300 <= h && h < 360)
			{
				R = maxHSV;
				G = minHSV;
				B = z + minHSV;
			}
			return new Color(R / 255, G / 255, B / 255, HSVColor.a);
		}
	}
}

using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class ColorExtensions
	{
		const float epsilon = 0.001F;

		public static Color SetValues(this Color color, Color values, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r = values.r;

			if ((channels & Channels.G) != 0)
				color.g = values.g;

			if ((channels & Channels.B) != 0)
				color.b = values.b;

			if ((channels & Channels.A) != 0)
				color.a = values.a;

			return color;
		}

		public static Color SetValues(this Color color, float value, Channels channels)
		{
			return color.SetValues(new Color(value, value, value, value), channels);
		}

		public static Color Lerp(this Color color, Color target, float deltaTime, Channels channels)
		{
			if ((channels & Channels.R) != 0 && Mathf.Abs(target.r - color.r) > epsilon)
				color.r = Mathf.Lerp(color.r, target.r, deltaTime);

			if ((channels & Channels.G) != 0 && Mathf.Abs(target.g - color.g) > epsilon)
				color.g = Mathf.Lerp(color.g, target.g, deltaTime);

			if ((channels & Channels.B) != 0 && Mathf.Abs(target.b - color.b) > epsilon)
				color.b = Mathf.Lerp(color.b, target.b, deltaTime);

			if ((channels & Channels.A) != 0 && Mathf.Abs(target.a - color.a) > epsilon)
				color.a = Mathf.Lerp(color.a, target.a, deltaTime);

			return color;
		}

		public static Color LerpLinear(this Color color, Color target, float deltaTime, Channels channels)
		{
			Vector4 difference = target - color;
			Vector4 direction = Vector4.zero.SetValues(difference, (Axes)channels);
			float distance = direction.magnitude;

			Vector4 adjustedDirection = direction.normalized * deltaTime;

			if (adjustedDirection.magnitude < distance)
				color += Color.clear.SetValues(adjustedDirection, channels);
			else
				color = color.SetValues(target, channels);

			return color;
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, float offset, float time, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r = center.r + amplitude.r * Mathf.Sin(frequency.r * time + offset);

			if ((channels & Channels.G) != 0)
				color.g = center.g + amplitude.g * Mathf.Sin(frequency.g * time + offset);

			if ((channels & Channels.B) != 0)
				color.b = center.b + amplitude.b * Mathf.Sin(frequency.b * time + offset);

			if ((channels & Channels.A) != 0)
				color.a = center.a + amplitude.a * Mathf.Sin(frequency.a * time + offset);

			return color;
		}

		public static Color Oscillate(this Color color, float frequency, float amplitude, float center, float offset, float time, Channels channels)
		{
			return color.Oscillate(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), new Color(center, center, center, center), offset, time, channels);
		}

		public static Color Mult(this Color color, Color values, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r *= values.r;

			if ((channels & Channels.G) != 0)
				color.g *= values.g;

			if ((channels & Channels.B) != 0)
				color.b *= values.b;

			if ((channels & Channels.A) != 0)
				color.a *= values.a;

			return color;
		}

		public static Color Mult(this Color color, Color otherVector)
		{
			return color.Mult(otherVector, Channels.RGBA);
		}

		public static Color Div(this Color color, Color values, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r /= values.r;

			if ((channels & Channels.G) != 0)
				color.g /= values.g;

			if ((channels & Channels.B) != 0)
				color.b /= values.b;

			if ((channels & Channels.A) != 0)
				color.a /= values.a;

			return color;
		}

		public static Color Div(this Color color, Color otherVector)
		{
			return color.Div(otherVector, Channels.RGBA);
		}

		public static Color Pow(this Color color, float power, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r = Mathf.Pow(color.r, power);

			if ((channels & Channels.G) != 0)
				color.g = Mathf.Pow(color.g, power);

			if ((channels & Channels.B) != 0)
				color.b = Mathf.Pow(color.b, power);

			if ((channels & Channels.A) != 0)
				color.a = Mathf.Pow(color.a, power);

			return color;
		}

		public static Color Pow(this Color color, float power)
		{
			return color.Pow(power, Channels.RGBA);
		}

		public static float Average(this Color color, Channels channels)
		{
			float sum = 0f;
			int channelCount = 0;

			if ((channels & Channels.R) != 0)
			{
				sum += color.r;
				channelCount++;
			}

			if ((channels & Channels.G) != 0)
			{
				sum += color.g;
				channelCount++;
			}

			if ((channels & Channels.B) != 0)
			{
				sum += color.b;
				channelCount++;
			}

			if ((channels & Channels.A) != 0)
			{
				sum += color.a;
				channelCount++;
			}

			return sum / channelCount;
		}

		public static float Average(this Color color)
		{
			return color.Average(Channels.RGBA);
		}

		public static Color Round(this Color color, float step, Channels channels)
		{
			if ((channels & Channels.R) != 0)
				color.r = color.r.Round(step);

			if ((channels & Channels.G) != 0)
				color.g = color.g.Round(step);

			if ((channels & Channels.B) != 0)
				color.b = color.b.Round(step);

			if ((channels & Channels.A) != 0)
				color.a = color.a.Round(step);

			return color;
		}

		public static Color Round(this Color color, float step)
		{
			return color.Round(step, Channels.RGBA);
		}

		public static Color Round(this Color color)
		{
			return color.Round(1f, Channels.RGBA);
		}

		public static Color ToHsv(this Color RgbColor)
		{
			float hue = 0f;
			float saturation = 0f;
			float value = 0f;
			float d = 0f;
			float h = 0f;

			float minRgb = Mathf.Min(RgbColor.r, Mathf.Min(RgbColor.g, RgbColor.b));
			float maxRgb = Mathf.Max(RgbColor.r, Mathf.Max(RgbColor.g, RgbColor.b));

			if (minRgb == maxRgb)
				return new Color(0f, 0f, minRgb, RgbColor.a);

			if (RgbColor.r == minRgb)
				d = RgbColor.g - RgbColor.b;
			else if (RgbColor.b == minRgb)
				d = RgbColor.r - RgbColor.g;
			else
				d = RgbColor.b - RgbColor.r;

			if (RgbColor.r == minRgb)
				h = 3f;
			else if (RgbColor.b == minRgb)
				h = 1f;
			else
				h = 5f;

			hue = (60f * (h - d / (maxRgb - minRgb))) / 360f;
			saturation = (maxRgb - minRgb) / maxRgb;
			value = maxRgb;

			return new Color(hue, saturation, value, RgbColor.a);
		}

		public static Color ToRgb(this Color HsvColor)
		{
			float red = 0f;
			float green = 0f;
			float blue = 0f;
			float maxHSV = 255f * HsvColor.b;
			float minHSV = maxHSV * (1f - HsvColor.g);
			float h = HsvColor.r * 360f;
			float z = (maxHSV - minHSV) * (1f - Mathf.Abs((h / 60f) % 2f - 1f));

			if (0f <= h && h < 60f)
			{
				red = maxHSV;
				green = z + minHSV;
				blue = minHSV;
			}
			else if (60f <= h && h < 120f)
			{
				red = z + minHSV;
				green = maxHSV;
				blue = minHSV;
			}
			else if (120f <= h && h < 180f)
			{
				red = minHSV;
				green = maxHSV;
				blue = z + minHSV;
			}
			else if (180f <= h && h < 240f)
			{
				red = minHSV;
				green = z + minHSV;
				;
				blue = maxHSV;
			}
			else if (240f <= h && h < 300f)
			{
				red = z + minHSV;
				green = minHSV;
				blue = maxHSV;
			}
			else if (300f <= h && h < 360f)
			{
				red = maxHSV;
				green = minHSV;
				blue = z + minHSV;
			}

			return new Color(red / 255f, green / 255f, blue / 255f, HsvColor.a);
		}
	}
}

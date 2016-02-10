using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Oscillation
{
	public static class OscillationUtility
	{
		static readonly Dictionary<PropertyInfo, IOscillator> propertyToOscillator = new Dictionary<PropertyInfo, IOscillator>();

		public static bool IsValid(PropertyInfo property)
		{
			return property.CanRead && property.CanWrite &&
				(property.PropertyType == typeof(float) ||
				property.PropertyType == typeof(Vector2) ||
				property.PropertyType == typeof(Vector3) ||
				property.PropertyType == typeof(Vector4) ||
				property.PropertyType == typeof(Color));
		}

		public static float Oscillate(OscillationSettings settings, float time)
		{
			switch (settings.WaveShape)
			{
				default:
					return Sine(settings.Frequency, settings.Amplitude, settings.Center, settings.Offset, time);
				case WaveShapes.Sawtooth:
					return Sawtooth(settings.Frequency, settings.Amplitude, settings.Center, settings.Offset, time);
				case WaveShapes.Triangle:
					return Triangle(settings.Frequency, settings.Amplitude, settings.Center, settings.Offset, settings.Ratio, time);
				case WaveShapes.Square:
					return Square(settings.Frequency, settings.Amplitude, settings.Center, settings.Offset, settings.Ratio, time);
				case WaveShapes.WhiteNoise:
					return WhiteNoise(settings.Amplitude, settings.Center);
				case WaveShapes.PerlinNoise:
					return PerlinNoise(settings.Frequency, settings.Amplitude, settings.Center, settings.Offset, time);
			}
		}

		public static IOscillator GetOscillator(PropertyInfo property)
		{
			IOscillator oscillator;

			if (!propertyToOscillator.TryGetValue(property, out oscillator))
			{
				oscillator = CreateOscillator(property);
				propertyToOscillator[property] = oscillator;
			}

			return oscillator;
		}

		static IOscillator CreateOscillator(PropertyInfo property)
		{
			Type oscillatorType = null;

			if (property.PropertyType == typeof(float))
				oscillatorType = typeof(FloatOscillator<>).MakeGenericType(property.DeclaringType);
			else if (property.PropertyType == typeof(Vector2))
				oscillatorType = typeof(Vector2Oscillator<>).MakeGenericType(property.DeclaringType);
			else if (property.PropertyType == typeof(Vector3))
				oscillatorType = typeof(Vector3Oscillator<>).MakeGenericType(property.DeclaringType);
			else if (property.PropertyType == typeof(Vector4))
				oscillatorType = typeof(Vector4Oscillator<>).MakeGenericType(property.DeclaringType);
			else if (property.PropertyType == typeof(Color))
				oscillatorType = typeof(ColorOscillator<>).MakeGenericType(property.DeclaringType);

			if (oscillatorType == null)
				return null;
			else
				return (IOscillator)Activator.CreateInstance(oscillatorType, property);
		}

		public static float Sine(float frequency, float amplitude, float center, float offset, float time)
		{
			return amplitude * Mathf.Sin(frequency * time + offset) + center;
		}

		public static float Sawtooth(float frequency, float amplitude, float center, float offset, float time)
		{
			return amplitude * PMath.Triangle(frequency * time + offset, 1f) + center;
		}

		public static float Triangle(float frequency, float amplitude, float center, float offset, float ratio, float time)
		{
			return amplitude * PMath.Triangle(frequency * time + offset, ratio) + center;
		}

		public static float Square(float frequency, float amplitude, float center, float offset, float ratio, float time)
		{
			return amplitude * PMath.Square(frequency * time + offset, ratio) + center;
		}

		public static float WhiteNoise(float amplitude, float center)
		{
			return amplitude * ((float)PRandom.Generator.NextDouble() * 2f - 1f) + center;
		}

		public static float PerlinNoise(float frequency, float amplitude, float center, float offset, float time)
		{
			return amplitude * (Mathf.Clamp01(Mathf.PerlinNoise(time * frequency, offset)) * 2f - 1f) + center;
		}
	}
}

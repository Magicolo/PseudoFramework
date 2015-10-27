using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class RendererExtensions
	{
		public static Color GetColor(this Renderer renderer, bool shared = false)
		{
			SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
			Color color;

			if (spriteRenderer != null && spriteRenderer.sharedMaterial == null)
				color = ((SpriteRenderer)renderer).color;
			else if (shared)
				color = renderer.sharedMaterial.color;
			else
				color = renderer.material.color;

			return color;
		}

		public static void SetColor(this Renderer renderer, Color color, bool shared = false, Channels channels = Channels.RGBA)
		{
			SpriteRenderer spriteRenderer = renderer as SpriteRenderer;

			if (spriteRenderer != null && spriteRenderer.sharedMaterial == null)
				spriteRenderer.color = spriteRenderer.color.SetValues(color, channels);
			else if (shared)
				renderer.sharedMaterial.SetColor(color, channels);
			else
				renderer.material.SetColor(color, channels);
		}

		public static void SetColor(this Renderer renderer, float color, bool shared = false, Channels channels = Channels.RGBA)
		{
			renderer.SetColor(new Color(color, color, color, color), shared, channels);
		}

		public static void Fade(this Renderer renderer, Color fade, bool shared = false, Channels channels = Channels.RGBA)
		{
			renderer.SetColor(renderer.GetColor(shared) + fade, shared, channels);
		}

		public static void Fade(this Renderer renderer, float fade, bool shared = false, Channels channels = Channels.RGBA)
		{
			renderer.Fade(new Color(fade, fade, fade, fade), shared, channels);
		}

		public static void FadeTowards(this Renderer renderer, Color targetColor, float deltaTIme, bool shared = false, InterpolationModes interpolation = InterpolationModes.Quadratic, Channels channels = Channels.RGBA)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					renderer.SetColor(renderer.GetColor().Lerp(targetColor, deltaTIme, channels), shared, channels);
					break;
				case InterpolationModes.Linear:
					renderer.SetColor(renderer.GetColor().LerpLinear(targetColor, deltaTIme, channels), shared, channels);
					break;
			}
		}

		public static void FadeTowards(this Renderer renderer, float targetColor, float deltaTime, bool shared = false, InterpolationModes interpolation = InterpolationModes.Quadratic, Channels channels = Channels.RGBA)
		{
			renderer.FadeTowards(new Color(targetColor, targetColor, targetColor, targetColor), deltaTime, shared, interpolation, channels);
		}

		public static void OscillateColor(this Renderer renderer, Color frequency, Color amplitude, Color center, float time, bool shared = false, Channels channels = Channels.RGBA)
		{
			renderer.SetColor(renderer.GetColor().Oscillate(frequency, amplitude, center, renderer.GetInstanceID() / 1000f, time, channels), shared, channels);
		}

		public static void OscillateColor(this Renderer renderer, float frequency, float amplitude, float center, float time, bool shared = false, Channels channels = Channels.RGBA)
		{
			renderer.OscillateColor(new Color(frequency, frequency, frequency, frequency), new Color(amplitude, amplitude, amplitude, amplitude), new Color(center, center, center, center), time, shared, channels);
		}
	}
}

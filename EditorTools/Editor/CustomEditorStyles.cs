using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Editor
{
	[System.Serializable]
	public static class CustomEditorStyles
	{
		static Dictionary<ColoredBoxSettings, GUIStyle> boxStyles = new Dictionary<ColoredBoxSettings, GUIStyle>();

		public static GUIStyle BoldFoldout
		{
			get
			{
				GUIStyle style = new GUIStyle("foldout");
				style.fontStyle = FontStyle.Bold;
				return style;
			}
		}

		public static GUIStyle MiniToolbarPopup
		{
			get
			{
				GUIStyle style = new GUIStyle("MiniToolbarPopup");
				style.fontStyle = FontStyle.Bold;
				style.alignment = TextAnchor.MiddleCenter;

				return style;
			}
		}

		static GUIStyle greenBox;
		public static GUIStyle GreenBox
		{
			get
			{
				if (greenBox == null)
				{
					float green = Mathf.Clamp(1.25f - Brightness, 0.5f, 1f);
					greenBox = ColoredBox(new Color(0.5f, green, 0.5f, 1f), 1);
				}

				return greenBox;
			}
		}

		static GUIStyle redBox;
		public static GUIStyle RedBox
		{
			get
			{
				if (redBox == null)
				{
					float red = Mathf.Clamp(1.25F - Brightness, 0.5F, 1);
					redBox = ColoredBox(new Color(red, 0.5F, 0.5F, 1), 1);
				}

				return redBox;
			}
		}

		static GUIStyle greyBox;
		public static GUIStyle GreyBox
		{
			get
			{
				if (greyBox == null)
					greyBox = ColoredBox(new Color(1.3f - Brightness, 1.3f - Brightness, 1.3f - Brightness, 1f), 1);

				return greyBox;
			}
		}

		public static float Brightness { get { return new GUIStyle("label").normal.textColor.Average(); } }

		public static GUIStyle ColoredBox(Color boxColor, int border = 1, float alphaFalloff = 1f)
		{
			var boxSettings = new ColoredBoxSettings(boxColor, border, alphaFalloff);

			GUIStyle style;

			if (!boxStyles.TryGetValue(boxSettings, out style))
			{
				style = new GUIStyle("box");
				style.normal.background = Box(boxColor, border, alphaFalloff);
				boxStyles[boxSettings] = style;
			}

			return style;
		}

		public static Texture2D Box(Color boxColor, int border = 1, float alphaFalloff = 1f)
		{
			var texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
			var pixels = new Color[texture.height * texture.width];
			var alpha = boxColor.a;

			for (int y = 0; y < texture.height; y++)
			{
				for (int x = 0; x < texture.width; x++)
				{
					bool isBorder = (x < border || x > texture.width - border - 1 || y < border || y > texture.height - border - 1);
					var pixel = isBorder ? boxColor : boxColor.SetValues(boxColor / 2, Channels.RGB);

					pixel.a = isBorder ? alpha : alpha * alphaFalloff;
					pixels[y * texture.width + x] = pixel;
				}
			}

			texture.filterMode = FilterMode.Point;
			texture.hideFlags = HideFlags.DontSave;
			texture.SetPixels(pixels);
			texture.Apply();

			return texture;
		}

		struct ColoredBoxSettings
		{
			public Color Color;
			public int Border;
			public float AlphaFalloff;

			public ColoredBoxSettings(Color color, int border, float alphaFalloff)
			{
				Color = color;
				Border = border;
				AlphaFalloff = alphaFalloff;
			}
		}
	}
}

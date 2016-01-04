using UnityEngine;
using System.Collections.Generic;
using System.Xml;

namespace Pseudo
{
	public class ResourcesUtility
	{
		public static Sprite LoadSprite(string path, string spriteFileName, int spriteIndex)
		{
			return LoadSprite(path + "/" + spriteFileName, spriteIndex);
		}

		public static Sprite LoadSprite(string spriteFilePath, int spriteIndex)
		{
			Sprite[] sprites = Resources.LoadAll<Sprite>(spriteFilePath);
			if (sprites != null && sprites.Length > spriteIndex)
			{
				return sprites[spriteIndex];
			}
			else
			{
				Debug.Log("Not found");
				return null;
			}
		}

		public static XmlDocument LoadXmlDocument(string resourceFile)
		{
			TextAsset textAsset = (TextAsset)Resources.Load(resourceFile);
			if (textAsset == null)
			{
				return null;
			}
			else
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.LoadXml(textAsset.text);
				return xmldoc;
			}

		}
	}
}
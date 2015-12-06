using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

namespace Pseudo
{
    public class UIFactory : PScriptableObject
    {

        public GameObject Button;
        public GameObject DropDown;
        public GameObject Image;
        public GameObject ImageButton;
        public GameObject InputField;
        public GameObject Panel;
        public GameObject Slider;
        public GameObject Text;
        public GameObject Toggle;

        public Image CreateImage(Transform parent, Vector3 position, Vector2 dimension, Sprite sourceImage)
        {
            return CreateImage(parent, position, dimension, sourceImage, Color.white);
        }
        public Image CreateImage(Transform parent, Vector3 position, Vector2 dimension, Sprite sourceImage, Color color)
        {
            GameObject newImageGO = (UnityEngine.GameObject)PrefabUtility.InstantiatePrefab(Image);
            Image image = newImageGO.GetComponent<Image>();
            RectTransform trans = newImageGO.GetComponent<RectTransform>();
            trans.SetParent(parent);
            trans.anchorMin = new Vector2(0, 1);
            trans.anchorMax = new Vector2(0, 1);
            trans.anchoredPosition = position;
            trans.sizeDelta = dimension;
            image.sprite = sourceImage;
            image.color = color;
            return image;
        }

        public Button CreateImageButton(Transform parent, Vector3 position, Vector2 dimension, Sprite sourceImage, UnityAction action)
        {
            return CreateImageButton(parent, position, dimension, sourceImage, Color.white, action);
        }
        public Button CreateImageButton(Transform parent, Vector3 position, Vector2 dimension, Sprite sourceImage, Color color, UnityAction action)
        {
            GameObject newImageGO = (UnityEngine.GameObject)PrefabUtility.InstantiatePrefab(ImageButton);
            
            RectTransform trans = newImageGO.GetComponent<RectTransform>();
            trans.SetParent(parent);
            trans.anchorMin = new Vector2(0, 1);
            trans.anchorMax = new Vector2(0, 1);
            trans.anchoredPosition = position;
            trans.sizeDelta = dimension;

            Image image = newImageGO.GetComponent<Image>();
            image.sprite = sourceImage;
            image.color = color;

            Button button = newImageGO.GetComponent<Button>();
            button.onClick.AddListener(action);

            return button;
        }
    }
}
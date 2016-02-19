using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace Pseudo
{
	[System.Serializable]
	public class LayerLineGUI : MonoBehaviour
	{
		public Text LayerNameText;
		public Button EditButton;
		public Image LayerButonImage;
		public Button LayerButton;
		public InputField NameChangeInput;
		public Text NameChangeText;
		public Toggle ShowHideToggle;
		public UIDoubleClickHandler TextDoubleClick;

		public Color selectedColor = Color.cyan;
		public Color baseColor = Color.white;

		[Disable]
		public LayerData LayerData;


		public void Init(LayerData layerData, Transform parent, UnityAction switchLayerAction)
		{
			LayerData = layerData;

			LayerNameText.text = layerData.Name;
			NameChangeText.text = layerData.Name;

			RectTransform trans = GetComponent<RectTransform>();
			trans.SetParent(parent,false);

			LayerButton.onClick.AddListener(switchLayerAction);

			NameChangeInput.gameObject.SetActive(false);
			EditButton.onClick.AddListener(() => EditButtonClicked());

			NameChangeInput.onEndEdit.AddListener((text) => stopWriting(text));

			ShowHideToggle.onValueChanged.AddListener((selected) => toggleView(selected));

			TextDoubleClick.OnDoubleClick.AddListener(() => EditButtonClicked());
		}

		void toggleView(bool selected)
		{
			LayerData.SetVisible(selected);
		}
		void EditButtonClicked()
		{
			NameChangeInput.gameObject.SetActive(true);
			NameChangeInput.text = LayerData.Name;
			NameChangeInput.Select();
			NameChangeInput.ActivateInputField();
			TextDoubleClick.enabled = false;
		}

		void stopWriting(string newText)
		{
			LayerData.Name = newText;
			LayerNameText.text = newText;
			NameChangeInput.gameObject.SetActive(false);
			TextDoubleClick.enabled = true;
		}
		public void SetEnabled(bool v)
		{

		}

		public void SetSelected(bool selected)
		{
			LayerButonImage.color = (selected) ? selectedColor : baseColor;
		}
	}

}

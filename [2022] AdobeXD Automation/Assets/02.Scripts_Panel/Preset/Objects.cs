using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Preset
{
	using UnityEngine.UI;

	public static class Objects
	{
		public static GameObject CreateRootPanel(string _name, Vector2 _scDelta)
		{
			GameObject obj = new GameObject(_name);

			RectTransform rectT = obj.AddComponent<RectTransform>();
			

			rectT.offsetMin = new Vector2(0, 0);
			rectT.offsetMax = new Vector2(_scDelta.x, _scDelta.y);
			rectT.anchorMin = new Vector2(0, 0);
			rectT.anchorMax = new Vector2(1, 1);

			obj.AddComponent<CanvasRenderer>().cullTransparentMesh = true;

			Image image = obj.AddComponent<Image>();
			image.enabled = false;

			return obj;
		}

		public static GameObject CreatePanel(string _name, RectTransform _rect)
		{
			GameObject obj = new GameObject(_name);

			RectTransform rectT = obj.AddComponent<RectTransform>();

			//rectT = rect;
			rectT.position = _rect.position;
			rectT.anchorMin = _rect.anchorMin;
			rectT.anchorMax = _rect.anchorMax;
			rectT.sizeDelta = _rect.sizeDelta;

			obj.AddComponent<CanvasRenderer>().cullTransparentMesh = true;

			Image image = obj.AddComponent<Image>();
			image.enabled = false;

			return obj;
		}

		public static void AddButton(GameObject _obj)
		{
			Button objButton;
			if(!_obj.TryGetComponent<Button>(out objButton))
			{
				objButton = _obj.AddComponent<Button>();
			}
		}

		public static void AddImage(GameObject _obj, Image _image)
		{
			Image objImage;
			if(!_obj.TryGetComponent<Image>(out objImage))
			{
				objImage = _obj.AddComponent<Image>();
			}

			objImage.sprite = _image.sprite;
			objImage.enabled = true;
		}

		public static void AddText(GameObject _obj, Text _text)
		{
			Text objText;
			if(!_obj.TryGetComponent<Text>(out objText))
			{
				Image image;
				if(_obj.TryGetComponent<Image>(out image))
				{
					GameObject.DestroyImmediate(image);
				}
				objText = _obj.AddComponent<Text>();
			}

			objText.font = _text.font;
			objText.text = _text.text;
			objText.fontSize = _text.fontSize;
			objText.alignment = _text.alignment;
		}
	}
}

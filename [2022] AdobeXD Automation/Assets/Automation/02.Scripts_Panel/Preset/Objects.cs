using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presets
{
	using Automation.Definition;
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

		public static void AddButton(GameObject _obj, Styles _style)
		{
			Template.SetButton(_obj, _style);
		}

		public static void AddImage(GameObject _obj, Image _image, Automation.Data.AutomationArguments _arguments)
		{
			Template.SetImage(_obj, _image, _arguments);
		}

		public static void AddText(GameObject _obj, Text _text, Automation.Data.AutomationArguments _arguments)
		{
			Template.SetText(_obj, _text, _arguments);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presets
{
    using Automation;
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

		public static void SetPanelPosition(GameObject _target, GameObject _obj)
        {
			RectTransform tgRectT = _target.GetComponent<RectTransform>();
			RectTransform objRectT = _obj.GetComponent<RectTransform>();

			tgRectT.position = objRectT.position;
			tgRectT.sizeDelta = objRectT.sizeDelta;
        }

		public static GameObject Create_Button(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id,
			Automation.Data.AutomationArguments _arguments)
		{
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = Objects.CreatePanel(name, _obj.GetComponent<RectTransform>());
			Template.SetButton(obj, _obj, _arguments.m_style);

			obj.transform.SetParent(_rootPanel.transform);
			return obj;
		}

		public static GameObject Create_Image(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id,
			Automation.Data.AutomationArguments _arguments)
		{
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = Objects.CreatePanel(name, _obj.GetComponent<RectTransform>());
			Template.SetImage(obj, _obj.GetComponent<Image>(), _arguments);

			obj.transform.SetParent(_rootPanel.transform);
			return obj;
		}

		public static GameObject Create_Text(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id,
			Automation.Data.AutomationArguments _arguments)
		{
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = Objects.CreatePanel(name, _obj.GetComponent<RectTransform>());
			Template.SetText(obj, _obj.GetComponent<Text>(), _arguments);

			obj.transform.SetParent(_rootPanel.transform);
			return obj;
		}

		#region Progressbar

		public static GameObject Create_Progressbar(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id, 
			Automation.Data.AutomationArguments _arguments)
        {
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = GameObject.Instantiate<GameObject>(_arguments.MUI_Templates.progressBar.gameObject, _rootPanel.transform);
			obj.name = name;
			SetPanelPosition(obj, _obj);


			//Debug.Log(_obj.name);
			//Debug.Log(name);

			return obj;
        }

		public static GameObject Create_ProgressbarBackground(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id, 
			Automation.Data.AutomationArguments _arguments)
		{
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = CreatePanel(name, _obj.GetComponent<RectTransform>());
			Template.SetImage(obj, _obj.GetComponent<Image>(), _arguments);
			
			obj.transform.SetParent(_rootPanel.transform);
			return obj;
        }

		public static GameObject Create_ProgressbarHighlight(GameObject _rootPanel, GameObject _obj, LabelCode _lCode, string _id,
			Automation.Data.AutomationArguments _arguments)
		{
			string name = Panels.SetInstanceName(_obj.name, _lCode, _id, _arguments);

			GameObject obj = CreatePanel(name, _obj.GetComponent<RectTransform>());
			Template.SetImage(obj, _obj.GetComponent<Image>(), _arguments);
			
			obj.transform.SetParent(_rootPanel.transform);
			return obj;
		}

		#endregion
	}
}

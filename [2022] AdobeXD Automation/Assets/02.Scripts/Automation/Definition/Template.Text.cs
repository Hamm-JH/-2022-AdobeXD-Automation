using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
	using TMPro;
	using UnityEngine.UI;

	public static partial class Template
	{
		public static void SetText(GameObject _obj, Text _text, Automation.Data.AutomationArguments _arguments /*Styles _style*/)
		{
			switch(_arguments.m_style)
			{
				case Styles.Default_UI:
					SetText_Default(_obj, _text);
					break;

				case Styles.TextmeshPro:
					SetText_TextmeshPro(_obj, _text, _arguments);
					break;
			}
		}

		private static void SetText_Default(GameObject _obj, Text _text)
		{
			Text objText;
			if (!_obj.TryGetComponent<Text>(out objText))
			{
				Image image;
				if (_obj.TryGetComponent<Image>(out image))
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

		private static void SetText_TextmeshPro(GameObject _obj, Text _text, 
			Automation.Data.AutomationArguments _arguments)
		{
			TextMeshProUGUI text;

			Text objText;
			if (!_obj.TryGetComponent<Text>(out objText))
			{
				Image image;
				if (_obj.TryGetComponent<Image>(out image))
				{
					GameObject.DestroyImmediate(image);
				}
				
				
				//objText = _obj.AddComponent<Text>();
			}
			else
			{
				GameObject.DestroyImmediate(objText);

				Image image;
				if (_obj.TryGetComponent<Image>(out image))
				{
					GameObject.DestroyImmediate(image);
				}
			}

			text = _obj.AddComponent<TextMeshProUGUI>();
			text.font = _arguments.m_fontAsset;
			text.text = _text.text;

			text.enableAutoSizing = true;
			text.fontSizeMin = _text.fontSize - 5;
			text.fontSizeMax = _text.fontSize;
			text.alignment = TextAlignmentOptions.TopLeft;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
	using TMPro;
	using UnityEngine.UI;

	public static partial class Template
	{
		public static void SetText(GameObject _obj, Text _text, Automation.Data.AutomationArguments _arguments)
		{
			switch(_arguments.m_style)
			{
				case Style.Default_UI:
					SetText_Default(_obj, _text);
					break;

				case Style.TextmeshPro:
				case Style.ModernUI:
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
			objText.color = _text.color;
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
			text.font = _arguments.Tmp_TMPro.m_fontAsset;
			text.text = _text.text;

			text.enableAutoSizing = true;
			text.fontSizeMin = _text.fontSize - 6;
			text.fontSizeMax = _text.fontSize - 1;
			text.fontSize = _text.fontSize;
			text.alignment = TextAlignmentOptions.TopLeft;
			text.color = _text.color;
		}
	}
}

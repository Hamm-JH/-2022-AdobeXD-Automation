using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
	using UnityEngine.UI;

	public static partial class Template
	{
		public static void SetImage(GameObject _obj, Image _image, Automation.Data.AutomationArguments _arguments)
		{
			switch (_arguments.m_style)
			{
				case Styles.Default_UI:
				case Styles.TextmeshPro:
				case Styles.ModernUI:
					SetSingleImage<Image>(_obj, _image);
					break;
			}
		}

		private static void SetSingleImage<T>(GameObject _obj, Image _image) where T : Image
		{
			T img;
			if(!_obj.TryGetComponent<T>(out img))
			{
				img = _obj.AddComponent<T>();
			}

			img.sprite = _image.sprite;
			img.enabled = true;
		}
	}
}

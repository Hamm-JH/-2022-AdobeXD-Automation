using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
	using UnityEngine.UI;
    using View;

    public static partial class Template
	{
		public static void SetButton(GameObject _obj, Styles _style)
		{
			Button objButton;
			if(!_obj.TryGetComponent<Button>(out objButton))
			{
				switch(_style)
				{
					case Styles.Default_UI:
					case Styles.TextmeshPro:
						_obj.AddComponent<Button>();
						_obj.GetComponent<Image>().enabled = true;
						_obj.AddComponent<UI_Selectable>();
						break;
				}
			}
		}
	}
}
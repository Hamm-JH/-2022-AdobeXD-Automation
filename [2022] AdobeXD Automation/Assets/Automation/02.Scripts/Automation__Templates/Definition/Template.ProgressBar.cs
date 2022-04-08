using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
    using Automation.Templates;
    using Automation.Templates.ModernUI;
    using UnityEngine.UI;

	public static partial class Template
	{
		public static void SetProgressBar(GameObject _obj, 
			Image _bgImg, Image _hlImg, Automation.Data.AutomationArguments _arguments)
		{
			switch(_arguments.m_style)
			{
				case Style.Default_UI:
				case Style.TextmeshPro:
					break;

				case Style.ModernUI:
					SetProgressBar_ModernUI(_obj, _bgImg, _hlImg, _arguments);
					// _obj.rect.width / height
					// 배경 이미지 색
					// 하이라이트 이미지 색
					break;
			}
		}

		private static void SetProgressBar_ModernUI(GameObject _obj,
			Image _bgImg, Image _hlImg, Automation.Data.AutomationArguments _arguments)
        {
			MUI_ProgressBar progressbar = _arguments.MUI_Templates.progressBar;

			GameObject obj = GameObject.Instantiate<GameObject>(progressbar.gameObject, _obj.transform);
			progressbar = obj.GetComponent<MUI_ProgressBar>();


        }
	}
}

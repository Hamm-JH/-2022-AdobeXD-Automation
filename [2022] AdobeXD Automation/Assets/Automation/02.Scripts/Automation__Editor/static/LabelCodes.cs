﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using static Automation.Automation_Adobe;

	public enum LabelCode
	{
		Null = -1,

		Button,
		Boundary,
		Background,
		Text,
		Image,
		
		/// <summary>
		/// 프로그레스바
		/// </summary>
		Progressbar,

		/// <summary>
		/// 프로그레스바 배경
		/// </summary>
		Progressbar_background,
		
		/// <summary>
		/// 프로그레스바 하이라이트
		/// </summary>
		Progressbar_highlight
	}

	public static class LabelCodes
    {
		public static bool IsMainPanelElement(string _name, Automation.Data.AutomationArguments _arguments)
		{
			bool result = false;

			LabelCode lCode = GetCode(_name, _arguments);
			if(lCode == LabelCode.Boundary ||
				lCode == LabelCode.Button ||
				lCode == LabelCode.Progressbar)
			{
				result = true;
			}

			return result;
		}

		public static LabelCode GetCode(string _code, Automation.Data.AutomationArguments _arguments)
		{
			LabelCode result = LabelCode.Null;

			if (_code.Contains(_arguments.m_labelButton))
			{
				result = LabelCode.Button;
			}
			else if (_code.Contains(_arguments.m_labelBoundary))
			{
				result = LabelCode.Boundary;
			}
			else if (_code.Contains(_arguments.m_labelBackground))
			{
				result = LabelCode.Background;
			}
			else if (_code.Contains(_arguments.m_labelText))
			{
				result = LabelCode.Text;
			}
			else if (_code.Contains(_arguments.m_labelImage))
			{
				result = LabelCode.Image;
			}
			else if(_code.Contains(_arguments.MUI_Templates.m_progressbar))
			{
				result = LabelCode.Progressbar;
			}
			else if(_code.Contains(_arguments.MUI_Templates.m_progressbar_background))
			{
				result = LabelCode.Progressbar_background;
			}
			else if(_code.Contains(_arguments.MUI_Templates.m_progressbar_highlight))
			{
				result = LabelCode.Progressbar_highlight;
			}

			return result;
		}

		public static string GetLabelString(LabelCode _code, Automation.Data.AutomationArguments _arguments)
		{
			string result = "";

			if (_code == LabelCode.Button)
			{
				result = _arguments.m_labelButton;
			}
			else if (_code == LabelCode.Boundary)
			{
				result = _arguments.m_labelBoundary;
			}
			else if (_code == LabelCode.Background)
			{
				result = _arguments.m_labelBackground;
			}
			else if (_code == LabelCode.Text)
			{
				result = _arguments.m_labelText;
			}
			else if (_code == LabelCode.Image)
			{
				result = _arguments.m_labelImage;
			}
			else if(_code == LabelCode.Progressbar)
			{
				result = _arguments.MUI_Templates.m_progressbar;
			}
			else if(_code == LabelCode.Progressbar_background)
			{
				result = _arguments.MUI_Templates.m_progressbar_background;
			}
			else if(_code == LabelCode.Progressbar_highlight)
			{
				result = _arguments.MUI_Templates.m_progressbar_highlight;
			}

			return result;
		}

	}
}

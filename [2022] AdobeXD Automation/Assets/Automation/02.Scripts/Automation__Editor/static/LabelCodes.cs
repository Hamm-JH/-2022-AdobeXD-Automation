using System.Collections;
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

			return result;
		}

		public static string GetLabelString(LabelCode _code)
		{
			string result = "";

			if (_code == LabelCode.Button)
			{
				result = "btn";
			}
			else if (_code == LabelCode.Boundary)
			{
				result = "bb";
			}
			else if (_code == LabelCode.Background)
			{
				result = "bg";
			}
			else if (_code == LabelCode.Text)
			{
				result = "tx";
			}
			else if (_code == LabelCode.Image)
			{
				result = "im";
			}

			return result;
		}

	}
}

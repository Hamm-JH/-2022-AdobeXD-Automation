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
		Image
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

	}
}

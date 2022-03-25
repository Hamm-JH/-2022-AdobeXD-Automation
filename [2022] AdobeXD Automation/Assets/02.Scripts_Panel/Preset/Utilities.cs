using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Preset
{
	using static Automation.Automation_Adobe;

	public static class Utilities
	{
		public static void GetSplitDatas(string _name, Automation.Data.AutomationArguments _arguments,
			out LabelCode _lCode, out string _id)
		{
			string[] splits = _name.Split(_arguments.m_split.ToCharArray());

			_lCode = GetCode(splits[0], 
				_arguments.m_labelButton, _arguments.m_labelBoundary, _arguments.m_labelBackground, _arguments.m_labelText, _arguments.m_labelImage);

			_id = GetID(splits[1], _arguments.m_tagID, _arguments.m_splitKeyValue);
		}

		public static LabelCode GetCode(string _code,
			string m_labelButton, string m_labelBoundary, string m_labelBackground, 
			string m_labelText, string m_labelImage)
		{
			LabelCode result = LabelCode.Null;

			if (_code.Contains(m_labelButton))
			{
				result = LabelCode.Button;
			}
			else if (_code.Contains(m_labelBoundary))
			{
				result = LabelCode.Boundary;
			}
			else if (_code.Contains(m_labelBackground))
			{
				result = LabelCode.Background;
			}
			else if (_code.Contains(m_labelText))
			{
				result = LabelCode.Text;
			}
			else if (_code.Contains(m_labelImage))
			{
				result = LabelCode.Image;
			}

			return result;
		}

		public static string GetID(string _code, string _tagID, string _splitKV)
		{
			string result = "";

			result = _code.Replace($"{_tagID}{_splitKV}", "");

			return result;
		}
	}
}

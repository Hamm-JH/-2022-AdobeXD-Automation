using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presets
{
	using Automation;

	using static Automation.Automation_Adobe;

	public static class Utilities
	{
		/// <summary>
		/// 이름 안에 있는 라벨 코드와 아이디 값을 가져온다.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="_arguments"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		public static void GetSplitDatas(string _name, Automation.Data.AutomationArguments _arguments,
			out LabelCode _lCode, out string _id)
		{
			string[] splits = _name.Split(_arguments.m_split.ToCharArray());

			_lCode = LabelCodes.GetCode(splits[0], _arguments);

			_id = GetID(splits[1], _arguments.m_tagID, _arguments.m_splitKeyValue);
		}

		public static string GetID(string _code, string _tagID, string _splitKV)
		{
			string result = "";

			result = _code.Replace($"{_tagID}{_splitKV}", "");

			return result;
		}
	}
}

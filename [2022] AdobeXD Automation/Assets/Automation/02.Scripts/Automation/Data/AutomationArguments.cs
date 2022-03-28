using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Data
{
	using UnityEditor;
	using Automation.Definition;
	using Automation.Data.Templates;

    public class AutomationArguments
    {
		public bool m_isVer2;
		public Styles m_style;
		public T_TMPro Tmp_TMPro;

		public string m_split = "_";
		public string m_splitKeyValue = "::";
		public bool   m_isRemainResourceName = false;
		
		public string m_labelButton = "btn";
		public string m_labelBoundary = "bb";
		public string m_labelBackground = "bg";
		public string m_labelText = "tx";
		public string m_labelImage = "im";
		
		public string m_tagID = "id";
		public string m_tagFunction = "fn";

		public AutomationArguments()
		{
			m_isVer2 = true;
			m_style = Styles.Default_UI;
			Tmp_TMPro = new T_TMPro();

			m_split = "_";
			m_splitKeyValue = "::";
			m_isRemainResourceName = false;

			m_labelButton = "btn";
			m_labelBoundary = "bb";
			m_labelBackground = "bg";
			m_labelText = "tx";
			m_labelImage = "im";

			m_tagID = "id";
			m_tagFunction = "fn";
		}
	}
}

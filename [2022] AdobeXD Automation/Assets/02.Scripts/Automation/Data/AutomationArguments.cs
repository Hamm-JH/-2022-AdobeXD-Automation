using Automation.Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Automation.Data
{
    public class AutomationArguments
    {
		public bool m_isVer2;
		public Styles m_style;
		public TMPro.TMP_FontAsset m_fontAsset;

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
			m_fontAsset = Init_GetTMPro_FontAsset();

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

		private TMPro.TMP_FontAsset Init_GetTMPro_FontAsset()
		{
			TMPro.TMP_FontAsset fontAsset = null;

			//Debug.Log(AssetDatabase.FindAssets(Assets.Definitions.TMP_FONTASSET)[0]);
			string guid = AssetDatabase.FindAssets(Assets.Definitions.TMP_FONTASSET)[0];
			//Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
			string path = AssetDatabase.GUIDToAssetPath(guid);
			fontAsset = (TMPro.TMP_FontAsset)AssetDatabase.LoadAssetAtPath<TMPro.TMP_Asset>(path);

			return fontAsset;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

namespace Automation.Templates
{
	public class T_TMPro
	{
		public TMP_FontAsset m_fontAsset;

		public T_TMPro()
		{
			m_fontAsset = Init_GetTMPro_FontAsset();
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

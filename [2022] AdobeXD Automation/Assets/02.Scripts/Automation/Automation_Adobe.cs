﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using Automation.Definition;
	using UnityEditor;
	using UnityEditor.EditorTools;

	/// <summary>
	/// Version 0.1
	/// </summary>
	public partial class Automation_Adobe : EditorWindow
	{

		Canvas m_rootCanvas;
		GameObject m_target;

		public Data.AutomationArguments arguments;

		string doButton = "Do It";

		public string IDTag
		{
			get => $"{arguments.m_tagID}{arguments.m_splitKeyValue}";
		}

		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Adobe Automation")]
		static void Init()
		{
			Automation_Adobe window = (Automation_Adobe)EditorWindow.GetWindow(typeof(Automation_Adobe));

			window.arguments = new Data.AutomationArguments();

			// 캔버스 검색
			Canvas canvas;
			if(GameObject.Find("Canvas").TryGetComponent<Canvas>(out canvas))
			{
				window.m_rootCanvas = canvas;
			}

			window.Show();
		}

		private void OnGUI()
		{
			GUILayout.Label("Base settings", EditorStyles.boldLabel);
			// 마지막 인자를 true로 두어야 Hierarchy에서 객체를 가져올 수 있다.
			m_rootCanvas = (Canvas)EditorGUILayout.ObjectField("root canvas", m_rootCanvas, typeof(Canvas), true);
			m_target = (GameObject)EditorGUILayout.ObjectField("target object", m_target, typeof(GameObject), true);

			EditorGUILayout.Space();
			GUILayout.Label("TMPro 템플릿");
			arguments.Tmp_TMPro.m_fontAsset = (TMPro.TMP_FontAsset)EditorGUILayout.ObjectField("TmPro FontAsset", arguments.Tmp_TMPro.m_fontAsset, typeof(TMPro.TMP_FontAsset), true);
			//arguments.m_fontAsset = (TMPro.TMP_FontAsset)EditorGUILayout.ObjectField("TmPro FontAsset", arguments.m_fontAsset, typeof(TMPro.TMP_FontAsset), true);

			EditorGUILayout.Space();
			GUILayout.Label("옵션 코드", EditorStyles.boldLabel);
			arguments.m_isRemainResourceName = EditorGUILayout.Toggle("is name remaining", arguments.m_isRemainResourceName);
			arguments.m_isVer2 = EditorGUILayout.Toggle("is ver 2", arguments.m_isVer2);
			arguments.m_style = (Styles)EditorGUILayout.EnumPopup("enums", arguments.m_style);

			EditorGUILayout.Space();
			GUILayout.Label("정의된 문자코드", EditorStyles.boldLabel);
			arguments.m_split = EditorGUILayout.TextField("split string", arguments.m_split);
			arguments.m_splitKeyValue = EditorGUILayout.TextField("split KeyValue", arguments.m_splitKeyValue);
			
			GUILayout.Label("Labels", EditorStyles.boldLabel);
			arguments.m_labelButton = EditorGUILayout.TextField("button", arguments.m_labelButton);
			arguments.m_labelBoundary = EditorGUILayout.TextField("boundary", arguments.m_labelBoundary);
			arguments.m_labelBackground = EditorGUILayout.TextField("background", arguments.m_labelBackground);
			arguments.m_labelText = EditorGUILayout.TextField("text", arguments.m_labelText);
			arguments.m_labelImage = EditorGUILayout.TextField("image", arguments.m_labelImage);

			GUILayout.Label("tags", EditorStyles.boldLabel);
			arguments.m_tagID = EditorGUILayout.TextField("id", arguments.m_tagID);
			arguments.m_tagFunction = EditorGUILayout.TextField("function", arguments.m_tagFunction);

			bool operatable = IsCanOperate(m_rootCanvas, m_target);

			if (operatable)
			{
				EditorGUILayout.HelpBox("변환 준비 완료", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox("root canvas와 root object를 할당하세요", MessageType.Warning);
			}

			EditorGUI.BeginDisabledGroup(!operatable);
			if (GUILayout.Button(doButton))
			{
				Debug.Log("----- do Button -----");

				// 자동화 전 복사 및 아이디 부여
				GameObject readyAutomata = PrepareAutomation(m_rootCanvas, m_target);

				// 자동화 시작
				StartAutomation(m_rootCanvas, readyAutomata);

				
				//if (EditorUtility.DisplayDialog("Title :: 객체 생성", "Message : 객체를 생성하시겠습니까?", "OK", "Cancel"))
				//{
				//	StartAutomation(m_rootCanvas, m_target);
				//}
			}
			EditorGUI.EndDisabledGroup();
		}

		private bool IsCanOperate(Canvas _rCanvas, GameObject _target)
		{
			bool result = false;

			if (_rCanvas != null && _target != null)
			{
				result = true;
			}

			return result;
		}

	}
}

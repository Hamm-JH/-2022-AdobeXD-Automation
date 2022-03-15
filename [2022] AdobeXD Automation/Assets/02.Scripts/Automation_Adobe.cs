using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using UnityEditor;
	using UnityEditor.EditorTools;

	/// <summary>
	/// Version 0.1
	/// </summary>
	public partial class Automation_Adobe : EditorWindow
	{

		Canvas m_rootCanvas;
		GameObject m_target;

		string m_split;
		string m_labelButton;
		string m_labelBoundary;
		string m_labelBackground;
		string m_labelText;
		string m_labelImage;

		string m_tagID;

		string doButton = "Do It";

		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Adobe Automation")]
		static void Init()
		{
			// Get existing open window or if none, make a new one:

			Automation_Adobe window = (Automation_Adobe)EditorWindow.GetWindow(typeof(Automation_Adobe));
			window.Show();
		}

		private void OnGUI()
		{
			GUILayout.Label("Base settings", EditorStyles.boldLabel);
			// ������ ���ڸ� true�� �ξ�� Hierarchy���� ��ü�� ������ �� �ִ�.
			m_rootCanvas = (Canvas)EditorGUILayout.ObjectField("root canvas", m_rootCanvas, typeof(Canvas), true);
			m_target = (GameObject)EditorGUILayout.ObjectField("target object", m_target, typeof(GameObject), true);

			EditorGUILayout.Space();
			GUILayout.Label("���ǵ� �����ڵ�", EditorStyles.boldLabel);
			m_split = EditorGUILayout.TextField("split string", m_split);
			
			GUILayout.Label("Labels", EditorStyles.boldLabel);
			m_labelButton = EditorGUILayout.TextField("button", m_labelButton);
			m_labelBoundary = EditorGUILayout.TextField("boundary", m_labelBoundary);
			m_labelBackground = EditorGUILayout.TextField("background", m_labelBackground);
			m_labelText = EditorGUILayout.TextField("text", m_labelText);
			m_labelImage = EditorGUILayout.TextField("image", m_labelImage);

			GUILayout.Label("tags", EditorStyles.boldLabel);
			m_tagID = EditorGUILayout.TextField("id", m_tagID);

			bool operatable = IsCanOperate(m_rootCanvas, m_target);

			if (operatable)
			{
				EditorGUILayout.HelpBox("��ȯ �غ� �Ϸ�", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox("root canvas�� root object�� �Ҵ��ϼ���", MessageType.Warning);
			}

			EditorGUI.BeginDisabledGroup(!operatable);
			if (GUILayout.Button(doButton))
			{
				Debug.Log("do Button");

				// �����
				StartAutomation(m_rootCanvas, m_target);
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

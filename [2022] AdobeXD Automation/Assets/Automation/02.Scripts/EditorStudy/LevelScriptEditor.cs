using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// 표시되는 대상에 대해 가져옴
		LevelScript myTarget = (LevelScript)target;

		myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
	}
}

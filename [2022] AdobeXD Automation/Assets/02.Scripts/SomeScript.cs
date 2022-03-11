using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SomeScript : MonoBehaviour
{
    public int level;
    public float health;
    public Vector3 target;
}

[CustomEditor(typeof(SomeScript))]
public class SomeScriptEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
	}
}

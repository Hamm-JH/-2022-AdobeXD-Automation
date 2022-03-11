using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBuilderScript : MonoBehaviour
{
    public GameObject obj;
    public Vector3 spawnPoint;

    public void BuildObject()
	{
        Instantiate(obj, spawnPoint, Quaternion.identity);
	}
}

[CustomEditor(typeof(ObjectBuilderScript))]
public class ObjectBuilderEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ObjectBuilderScript myScript = (ObjectBuilderScript)target;

		if(GUILayout.Button("Build Object"))
		{
			myScript.BuildObject();
		}
	}
}

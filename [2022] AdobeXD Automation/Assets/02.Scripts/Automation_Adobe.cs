using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Automation_Adobe : EditorWindow
{
    Canvas m_rootCanvas;
    GameObject m_target;


    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

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


        // 마지막 인자를 true로 두어야 Hierarchy에서 객체를 가져올 수 있다.
        m_target = (GameObject)EditorGUILayout.ObjectField("Hello", m_target, typeof(GameObject), true);
        myString = EditorGUILayout.TextField("text field", myString);

        //EditorGUILayout.BeginFadeGroup(1);
        //EditorGUILayout.EndFadeGroup();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.HelpBox("Hello group", MessageType.Info);
        EditorGUI.EndDisabledGroup();

        //groupEnabled = IsCanOperate(m_rootCanvas, m_target);

        //groupEnabled = EditorGUILayout.BeginToggleGroup("Operatable", IsCanOperate(m_rootCanvas, m_target));
        //if(groupEnabled)
		//{
        //    EditorGUILayout.HelpBox("Hello", MessageType.Info);
		//}
        //else
		//{
        //
		//}
        //EditorGUILayout.EndToggleGroup();

        //EditorGUILayout.BeginToggleGroup("Operatable", groupEnabled);

        //EditorGUILayout.EndToggleGroup();

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        if(myBool)
		{
            EditorGUI.BeginDisabledGroup(myBool);
            EditorGUILayout.HelpBox("true", MessageType.Info);
            EditorGUI.EndDisabledGroup();
		}

        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        if(GUILayout.Button(doButton))
		{
            Debug.Log("do Button");

            if(EditorUtility.DisplayDialog("Title :: 생성 확인창", "Message : 객체를 생성하시겠습니까?", "OK", "Cancel"))
			{
                PrintObjects(m_target);

			}
		}
	}

    private bool IsCanOperate(Canvas _rCanvas, GameObject _target)
	{
        bool result = false;

        if(_rCanvas != null && _target != null)
		{
            result = true;
		}

        return result;
	}

    private void PrintObjects(GameObject _obj)
	{
        if(_obj == null)
		{
            Debug.LogError("Obj is null");
            return;
		}

        

        Debug.Log(_obj.name);

        GameObject obj = new GameObject("Element 1");

        obj.transform.SetParent(_obj.transform);
	}


}

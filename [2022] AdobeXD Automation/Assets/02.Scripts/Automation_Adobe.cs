using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Automation_Adobe : EditorWindow
{
    public enum SplitString
	{
        Underbar,
        Test2
	}

    Canvas m_rootCanvas;
    GameObject m_target;

    string m_split;
    string m_labelButton;
    string m_labelBoundary;
    string m_labelBackground;
    string m_labelText;
    string m_labelImage;
    //SplitString m_split;

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
        m_rootCanvas = (Canvas)EditorGUILayout.ObjectField("root canvas", m_rootCanvas, typeof(Canvas), true);
        m_target = (GameObject)EditorGUILayout.ObjectField("target object", m_target, typeof(GameObject), true);

        EditorGUILayout.Space();
        GUILayout.Label("정의된 문자코드", EditorStyles.boldLabel);
        m_split = EditorGUILayout.TextField("split string", m_split);
        GUILayout.Label("Labels", EditorStyles.boldLabel);
        m_labelButton = EditorGUILayout.TextField("button", m_labelButton);
        m_labelBoundary = EditorGUILayout.TextField("boundary", m_labelBoundary);
        m_labelBackground = EditorGUILayout.TextField("background", m_labelBackground);
        m_labelText = EditorGUILayout.TextField("text", m_labelText);
        m_labelImage = EditorGUILayout.TextField("image", m_labelImage);


        //m_split = (SplitString)EditorGUILayout.EnumFlagsField("split code", m_split);
        //myString = EditorGUILayout.TextField("text field", myString);

        bool operatable = IsCanOperate(m_rootCanvas, m_target);

        if(operatable)
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
            Debug.Log("do Button");

            // 디버깅
            PrintObjects(m_target);

            // 디버깅 끝나면 활성화
            //if (EditorUtility.DisplayDialog("Title :: 생성 확인창", "Message : 객체를 생성하시겠습니까?", "OK", "Cancel"))
            //{
            //    PrintObjects(m_target);
            //}
        }
        EditorGUI.EndDisabledGroup();
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

        List<Transform> trs = _obj.transform.GetComponentsInChildren<Transform>().ToList();

        trs.ForEach(x =>
        {
            if (x.name.Contains(m_labelButton))
            {
                Debug.Log($"find btn label : name {x.name}");
            }
        });

        Debug.Log(_obj.name);



        // 라벨 문자 확인 끝나고 생성단계 진행
        return;
        
        GameObject obj = new GameObject("Element 1");

        obj.transform.SetParent(_obj.transform);
	}


}

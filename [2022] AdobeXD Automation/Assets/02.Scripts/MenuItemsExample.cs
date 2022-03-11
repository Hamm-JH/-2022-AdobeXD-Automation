using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItemsExample
{
	/// <summary>
	/// Menu Item support key rule
	/// % - CTRL on Windows / CMD on OSX
	/// # - SHIFT
	/// & - ALT
	/// LEFT/RIGHT/UP/DOWN - Arrow keys
	/// F1...F12 - F keys
	/// HOME, END, PGUP, PGDN
	/// </summary>

	// Add a new menu item with hotkey CTRL-SHIFT-A
	[MenuItem("Tools/New Option %#a")]
    private static void NewMenuOption()
	{
		Debug.Log("hello new option with CTRL-SHIFT-A");
	}

	// Add a new menu item with hotkey CTRL-G
	[MenuItem("Tools/Item %g")]
	private static void NewNestedOption()
	{
		Debug.Log("hello option with CTRL-G");
	}

	// Add a new menu item with hotkey G
	[MenuItem("Tools/Item2 _g")]
	private static void NewOptionWithHotkey()
	{
		Debug.Log("hello hotkey with g");
	}

	/// <summary>
	/// Special Paths :: MenuItem attribute controls under which top level menu the new item will be placed
	/// 
	/// Assets - items will be available under the "Assets" menu, as well using right-click inside the project view
	/// Assets - Assets메뉴 아래 활성화되는 메뉴. 프로젝트 창에서 우클릭시 확인 가능하다.
	/// 
	/// Asset/Create - items will be listed when clicking on the "Create" button in the project view
	///		(useful when adding new types that can be added to the project)
	///	Asset/Create - 아이템이 프로젝트 뷰 우클릭 - Create창에 생성된다.
	///	
	///	CONTEXT/ComponentName - items will be available by right-clicking inside the inspector of the given component
	///	CONTEXT/ComponentName - inspector 창의 대상을 우클릭시 나타나는 메뉴
	/// </summary>

	// Add a new menu item that is accessed by right-clicking on an asset in the project view
	// 프로젝트 창 내부 우클릭 내부에 메뉴 생성

	[MenuItem("Assets/Load Additive Scene")]
	private static void LoadAdditiveScene()
	{
		Debug.Log("Load Additive Scene");

		var selected = Selection.activeObject;
		EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(selected));
	}

	// Adding a new menu item under Assets/Create
	// 프로젝트 아무 파일 우클릭 - Create 내부에 메뉴 생성
	[MenuItem("Assets/Create/Add configuration")]
	private static void AddConfig()
	{
		Debug.Log("AddConfig");
		// Create and add a new ScriptableObject for storing configuration
	}

	// Add a new menu item that is accessed by right-clicking inside the RigidBody component
	// 인스펙터 창의 Rigidbody 컴포넌트 우클릭시 발생하는 메뉴
	[MenuItem("CONTEXT/Rigidbody/New Option")]
	private static void NewOpenForRigidBody()
	{
		Debug.Log("NewOpenForRigidBody");
	}
}

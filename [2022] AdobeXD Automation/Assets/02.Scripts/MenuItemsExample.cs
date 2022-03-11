using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
}

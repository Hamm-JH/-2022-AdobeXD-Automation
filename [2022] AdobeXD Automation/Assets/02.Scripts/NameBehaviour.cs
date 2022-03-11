using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBehaviour : MonoBehaviour
{
	#region #ContextMenu#

	//public string Name;

	[ContextMenu("Reset Name")]
	private void ResetName()
	{
		Name = string.Empty;
	}

	#endregion

	#region #ContextMenuItem#

	[ContextMenuItem("Randomize Name", "Randomize")]
	public string Name;

	private void Randomize()
	{
		Name = "some Random Name";
	}

	#endregion
}

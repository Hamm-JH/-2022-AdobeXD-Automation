using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using UnityEditor;

	public partial class Automation_Adobe : EditorWindow
	{
		// 템플릿

		public enum LabelCode
		{
			Null = -1,

			Button,
			Boundary,
			Background,
			Text,
			Image
		}
	}
}

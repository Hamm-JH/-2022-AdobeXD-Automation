using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Preset
{
	using UnityEngine.UI;

	public static class Objects
	{
		public static GameObject CreateRootPanel(string _name, Vector2 _scDelta)
		{
			GameObject obj = new GameObject(_name);

			RectTransform rectT = obj.AddComponent<RectTransform>();
			

			rectT.offsetMin = new Vector2(0, 0);
			rectT.offsetMax = new Vector2(_scDelta.x, _scDelta.y);
			rectT.anchorMin = new Vector2(0, 0);
			rectT.anchorMax = new Vector2(1, 1);

			obj.AddComponent<CanvasRenderer>().cullTransparentMesh = true;

			Image image = obj.AddComponent<Image>();
			image.enabled = false;

			return obj;
		}
	}
}

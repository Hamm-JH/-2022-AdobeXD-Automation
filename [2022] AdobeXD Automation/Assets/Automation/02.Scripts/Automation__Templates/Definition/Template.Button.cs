using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Definition
{
	using UnityEngine.UI;
    using View;

    public static partial class Template
	{
		public static void SetButton(GameObject _obj, GameObject _originalSource, Styles _style)
		{
			Image btnImg = null;
			Button objButton;
			if(!_obj.TryGetComponent<Button>(out objButton))
			{
				switch(_style)
				{
					case Styles.Default_UI:
					case Styles.TextmeshPro:
					case Styles.ModernUI:
						_obj.AddComponent<Button>();
						_obj.GetComponent<Image>().enabled = true;
						_obj.AddComponent<UI_Selectable>();

						btnImg = _obj.GetComponent<Image>();
						break;
				}
			}

			// �������� ��ư�� �̹����� ���ִ� ��� ��ư�̹����� �Ҵ�
			Image img;
			if (_originalSource.TryGetComponent<Image>(out img))
			{
				if(img.sprite != null && btnImg != null)
				{
					btnImg.sprite = img.sprite;
				}
			}
		}
	}
}

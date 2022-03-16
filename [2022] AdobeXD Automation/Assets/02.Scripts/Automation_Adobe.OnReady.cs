using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Automation
{
	public partial class Automation_Adobe : EditorWindow
	{
		/// <summary>
		/// 시안 자동화 전에 객체 아이디 부여
		/// </summary>
		/// <param name="_canvas"></param>
		/// <param name="_obj"></param>
		private GameObject PrepareAutomation(Canvas _canvas, GameObject _rootObj)
		{
			GameObject clone = CloneRootUI(_canvas, _rootObj);

			ReadyAutomation(clone.transform, 0, 0);

			return clone;
		}

		/// <summary>
		/// RootUI를 복사한다.
		/// </summary>
		/// <param name="_canvas"></param>
		/// <param name="_rootObj"></param>
		/// <returns></returns>
		private GameObject CloneRootUI(Canvas _canvas, GameObject _rootObj)
		{
			GameObject result = null;

			result = Instantiate(_rootObj, _canvas.transform);
			result.name = $"clone {result.name}";
			//result.transform.SetParent(_canvas.transform);

			return result;
		}

		/// <summary>
		/// 자동화 준비
		/// </summary>
		/// <param name="_tr"></param>
		private void ReadyAutomation(Transform _tr, int parentID, int idCount)
		{
			// 이 Transform이 자동화 대상인가?
			if(IsTargetObject(_tr, m_tagID, m_split))
			{
				// 대상이 자동화 대상인 경우 ID 태그를 찾아서 Replace( -> parentID + idCount )
				// 자식 개체의, 새 요소인
				// bb_ :: 경계
				// btn_ :: 버튼
				// 의 경우에는 부모 아이디에서 변형된 새 아이디를 배치받는다.
				if(_tr.name.Contains(m_labelButton) || _tr.name.Contains(m_labelBoundary))
				{
					_tr.name = _tr.name.Replace(IDTag, $"{IDTag}{parentID*10+idCount}");
				}
				// 부모 개체의 요소로 붙어야 할
				// im_ :: 이미지
				// tx_ :: 텍스트
				// bg_ :: 배경
				// 의 경우에는 부모 아이디 그대로 배치한다.
				else
				{
					_tr.name = _tr.name.Replace(IDTag, $"{IDTag}{parentID}");
				}
			}

			int index = _tr.childCount;
			for (int i = 0; i < index; i++)
			{
				// 자식 개체가 
				ReadyAutomation(_tr.GetChild(i), parentID*10+idCount, i);
			}
		}

	}
}

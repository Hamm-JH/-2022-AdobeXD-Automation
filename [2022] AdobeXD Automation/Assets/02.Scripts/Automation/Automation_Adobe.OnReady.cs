using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

			SetAutomationObjectID(clone.transform, 0, 0, _rootObj);

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
		private void SetAutomationObjectID(Transform _tr, int parentID, int idCount, GameObject _rootObj)
		{
			// 텍스트 요소에 라벨, id 태그 부여
			SetAutomation_TextObject(_tr);

			string ID = "";

			// 이 Transform이 자동화 대상인가?
			if (IsTargetObject(_tr, arguments.m_tagID, arguments.m_split))
			{
				if(arguments.m_isVer2)
				{
					//_tr.name = "hello";
					SetAutomation_AddIDTag(_tr);
				}


				// 대상이 자동화 대상인 경우 ID 태그를 찾아서 Replace( -> parentID + idCount )
				// 자식 개체의, 새 요소인
				// bb_ :: 경계
				// btn_ :: 버튼
				// 의 경우에는 부모 아이디에서 변형된 새 아이디를 배치받는다.
				if(_tr.name.Contains(arguments.m_labelButton) 
				|| _tr.name.Contains(arguments.m_labelBoundary))
				{
					ID = $"{parentID*10+idCount}";
					_tr.name = _tr.name.Replace(IDTag, $"{IDTag}{ID}");
				}
				// 부모 개체의 요소로 붙어야 할
				// im_ :: 이미지
				// tx_ :: 텍스트
				// bg_ :: 배경
				// 의 경우에는 부모 아이디 그대로 배치한다.
				else
				{
					// 부모 개체가 버튼(btn_) 또는 경계(bb_)를 가진 경우
					if(_tr.parent.name.Contains(arguments.m_labelButton) 
					|| _tr.parent.name.Contains(arguments.m_labelBoundary))
					{
						// 자식의 요소(im, tx, bg)를 변환한다.
						ID = $"{parentID}";
						_tr.name = _tr.name.Replace(IDTag, $"{IDTag}{ID}");
					}
					// 부모 개체가 버튼(btn_) 또는 경계(bb_)를 가지지 않은 경우
					else
					{
						// btn, bb를 가진 부모개체를 찾아 id값을 반환한다.
						ID = GetParent_BB_BTN_ID(_tr.parent, _rootObj.transform);
						_tr.name = _tr.name.Replace(IDTag, $"{IDTag}{ID}");
					}
				}
			}

			int index = _tr.childCount;
			for (int i = 0; i < index; i++)
			{
				// 자식 개체가 
				SetAutomationObjectID(_tr.GetChild(i), parentID*10+idCount, i, _rootObj);
			}
		}

		/// <summary>
		/// 텍스트 요소에 tx 라벨, id 태그를 부여한다.
		/// </summary>
		/// <param name="_tr"></param>
		private void SetAutomation_TextObject(Transform _tr)
		{
			Text text;
			if(_tr.TryGetComponent<Text>(out text))
			{
				_tr.name = _tr.name.Replace(_tr.name, $"tx_{_tr.name}");
			}
		}

		/// <summary>
		/// Transform에 ID 태그를 달아둔다.
		/// </summary>
		/// <param name="_tr"></param>
		private void SetAutomation_AddIDTag(Transform _tr)
		{
			LabelCode lCode = GetCode(_tr.name);
			string lString = GetLabelString(lCode);
			if(lCode != LabelCode.Null)
			{
				_tr.name = _tr.name.Replace($"{lString}_", $"{lString}_id::_");
			}
		}

		/// <summary>
		/// 부모중에 Btn, bb 라벨을 가진 개체의 ID를 찾는다.
		/// </summary>
		/// <param name="_tr"></param>
		private string GetParent_BB_BTN_ID(Transform _tr, Transform _root)
		{
			// 루프중에 목표 tr이 변환대상의 root에 닿을경우 종료
			if (_tr == _root) return "";

			string ID = "";

			// 부모개체가 버튼, 경계 라벨을 포함한다면
			if (IsContainsLabel(_tr.parent))
			{
				ID = GetTargetID(_tr.parent);
			}
			else
			{
				ID = GetParent_BB_BTN_ID(_tr.parent, _root);
			}

			return ID;
		}


		private bool IsContainsLabel(Transform _tr)
		{
			if(_tr.name.Contains(arguments.m_labelButton) 
			|| _tr.name.Contains(arguments.m_labelBoundary))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// ID가 부여된 개체의 ID값을 가져온다.
		/// </summary>
		/// <param name="_tr"></param>
		/// <returns></returns>
		private string GetTargetID(Transform _tr)
		{
			string ID = "";

			string name = _tr.name;

			// ID Tag 제거
			name = name.Replace(IDTag, "");

			// ID Tag가 제거된 첫 번째 요소를 가져옴
			ID = name.Split('_')[1];

			return ID;
		}
	}
}

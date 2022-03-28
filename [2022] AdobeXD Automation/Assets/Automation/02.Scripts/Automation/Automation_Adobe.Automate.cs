using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using System;
	using UnityEditor;
	using System.Linq;
	using Presets;
	using UIs;
	using UnityEngine.UI;

	public partial class Automation_Adobe : EditorWindow
	{
		/// <summary>
		/// root of UI
		/// </summary>
		/// <param name="_obj"></param>
		private void StartAutomation(Canvas _canvas, GameObject _obj)
		{
			if(_canvas == null)
			{
				Debug.LogError("canvas is null");
				return;
			}
			if (_obj == null)
			{
				Debug.LogError("Obj is null");
				return;
			}

			List<Transform> trs = _obj.transform.GetComponentsInChildren<Transform>().ToList();

			// 패널화 가능 대상을 검색, 분류한다.
			// 1 아이디 값과, 라벨, 스플릿 수가 존재하는지 확인한다.
			List<Transform> elements = SetTargetList(trs);

			// 아이디, 라벨이 존재한다면 panel화 가능 대상으로 본다.
			// 2 패널화 가능 대상들을 분류해서 Dictionary에 저장한다.
			Dictionary<string, Panel> id_panels = SetPanelInstance(elements);

			// 3 변환하고자 하는 객체의 ID값 기반 사전변수 배치 및 부모객체 ID값 지정이 완료된 상태에서
			// 올바른 재배치를 위해 기존 객체를 재배치한다.
			ReplacePanels(trs, _obj);

			// 루트 패널 객체를 생성한다.
			// - 루트 패널 객체를 캔버스의 자식으로 할당한다.
			GameObject rootPanel = Objects.CreateRootPanel("UI Panel", m_rootCanvas.GetComponent<RectTransform>().sizeDelta);
			rootPanel.transform.SetParent(m_rootCanvas.transform);

			// 4 각 패널 객체들을 생성한다.
			CretaePanels(id_panels, rootPanel);

			// 5 생성된 패널객체들 간에 부모설정을 진행한다.
			SetPanelParents(id_panels);
		}

		#region 1 Set target list

		/// <summary>
		/// 1. ID와 태그, 분할 문자열 수가 최소 2개 이상인 개체의 리스트를 반환한다.
		/// </summary>
		/// <param name="_trs"></param>
		/// <returns></returns>
		private List<Transform> SetTargetList(List<Transform> _trs)
		{
			List<Transform> result = new List<Transform>();

			_trs.ForEach(x =>
			{
				if (IsTargetObject(x, arguments.m_tagID, arguments.m_split))
				{
					result.Add(x);
				}
			});

			return result;
		}

		private bool IsTargetObject(Transform _tr, string _tagID, string _split)
		{
			bool result = false;

			// 이름에 id 태그가 존재하는가?
			bool isTagContains = _tr.name.Contains(_tagID);

			// 이름에 지정된 라벨중에 하나가 존재하는가?
			bool isLabelExist = GetCode(_tr.name) != LabelCode.Null;

			// 이름을 주 스플릿 코드로 나누었을때 수가 2 이상인가?
			bool isMinSplitArgsConfirmed = _tr.name.Split(_split.ToCharArray()).Length >= 2;

			if(!arguments.m_isVer2)
			{
				if (isTagContains
				&& isLabelExist
				&& isMinSplitArgsConfirmed)
				{
					result = true;
				}
			}
			else
			{
				if (isLabelExist
				&& isMinSplitArgsConfirmed)
				{
					result = true;
				}
			}

			return result;
		}

		#endregion

		#region 2 Set panel instance dictionary

		/// <summary>
		/// Transform 리스트에서 Dictionary에 id별로 값을 할당한다.
		/// </summary>
		/// <param name="_elems"></param>
		/// <returns></returns>
		private Dictionary<string, Panel> SetPanelInstance(List<Transform> _elems)
		{
			Dictionary<string, Panel> result = new Dictionary<string, Panel>();

			_elems.ForEach(x =>
			{
				//splits[0]; // 라벨
				LabelCode lCode = LabelCode.Null;
				//splits[1]; // 아이디
				string id = "";

				Utilities.GetSplitDatas(x.name, arguments, out lCode, out id);

				SetPanel(x, id, lCode, result);

				//string res = "";
				
				//string[] splits = x.name.Split(m_split.ToCharArray());
				//splits.ToList().ForEach(y => res += $" [{y}]");
				//Debug.Log($"_elem name : {x.name} // {res}");
			});

			return result;
		}

		/// <summary>
		/// Dictionary의 패널 Value에 id값에 맞는 Transform을 할당합니다.
		/// </summary>
		/// <param name="elem"></param>
		/// <param name="_id"></param>
		/// <param name="_lCode"></param>
		/// <param name="_result"></param>
		private void SetPanel(Transform elem, string _id, LabelCode _lCode, Dictionary<string, Panel> _result)
		{
			// elem Transform의 ID값을 Dictionary가 가지고 있다면
			if (_result.ContainsKey(_id))
			{
				// Dictionary ID값에 대응되는 Value리스트에 elem 추가
				_result[_id].AddElement(elem);
			}
			else
			{
				Panel pnl = new Panel();
				pnl.AddElement(elem);
				_result.Add(_id, pnl);
			}

			if (_lCode == LabelCode.Boundary ||
				_lCode == LabelCode.Button)
			{
				_result[_id].SetParentID(GetParentID(elem));
			}
		}

		/// <summary>
		/// 부모 개체의 ID를 얻는다.
		/// 최상위 개체인 경우 null값을 준다.
		/// </summary>
		/// <param name="_tr"></param>
		private string GetParentID(Transform _tr)
		{
			string pName = _tr.parent.name;

			string[] splits = pName.Split(arguments.m_split.ToCharArray());

			string id = null;

			// 최상위 개체가 아닌 경우
			if(splits.Length >= 2)
			{
				id = Utilities.GetID(splits[1], arguments.m_tagID, arguments.m_splitKeyValue);
			}

			// 최상위 개체인 경우 null값은 없다.
			return id;
		}

		#endregion

		#region 3 Replace Panel 

		private void ReplacePanels(List<Transform> _trs, GameObject _rootPanel)
		{
			Debug.LogWarning("TODO Anchor Pos, Pivot 위치값 초기화는 추후에 진행 // 지금은 수동으로 앵커링, 피봇팅 에디터에서 초기화");
			// TODO Anchor Pos, Pivot 위치값 초기화는 추후에 진행 // 지금은 수동으로 앵커링, 피봇팅 에디터에서 초기화
			//_trs.ForEach(x =>
			//{
			//	if (x.parent != _rootPanel.transform)
			//	{
			//		x.transform.SetParent(_rootPanel.transform);

			//		RectTransform rectT;
			//		if(x.TryGetComponent<RectTransform>(out rectT))
			//		{
			//			Debug.Log($"---------------------------------");

			//			Debug.Log($"{rectT} {rectT.rect}");

			//			//Vector2 pos = new Vector2(rectT.rect.x, rectT.rect.y);
			//			float width = rectT.rect.width;
			//			float height = rectT.rect.height;

			//			Debug.Log($"width : {width}");
			//			Debug.Log($"height : {height}");

			//			rectT.pivot = new Vector2(0.5f, 0.5f);
			//			rectT.anchorMin = new Vector2(0.5f, 0.5f);
			//			rectT.anchorMax = new Vector2(0.5f, 0.5f);
			//			//rectT.position = pos;
			//			rectT.sizeDelta = new Vector2(width, height);

			//			Debug.Log($"{rectT} {rectT.rect}");
			//			width = rectT.rect.width;
			//			height = rectT.rect.height;
			//			Debug.Log($"width : {width}");
			//			Debug.Log($"height : {height}");

			//			Debug.Log($"---------------------------------");
			//		}
			//	}
			//});
		}

		#endregion

		#region 4 Create Panels

		/// <summary>
		/// Dictionary 기반으로 패널을 생성한다.
		/// </summary>
		/// <param name="_panels"></param>
		private void CretaePanels(Dictionary<string, Panel> _panels, GameObject _rootPanel)
		{
			foreach(Panel pan in _panels.Values)
			{
				pan.CreatePanel(_rootPanel, arguments);
			}
		}

		#endregion

		#region 5 Set Panel parents

		private void SetPanelParents(Dictionary<string, Panel> _panels)
		{
			foreach(Panel pan in _panels.Values)
			{
				pan.SetPanelParent(_panels);
			}
		}

		#endregion

		#region Debug Elements
		/// <summary>
		/// label 별 개체를 확인한다.
		/// </summary>
		/// <param name="_trs"></param>
		private void DebugElements(List<Transform> _trs)
		{
			Debug.Log("-------------------------------------");
			Check(_trs, arguments.m_labelButton);
			Debug.Log("-------------------------------------");
			Check(_trs, arguments.m_labelBoundary);
			Debug.Log("-------------------------------------");
			Check(_trs, arguments.m_labelBackground);
			Debug.Log("-------------------------------------");
			Check(_trs, arguments.m_labelText);
			Debug.Log("-------------------------------------");
			Check(_trs, arguments.m_labelImage);
			Debug.Log("-------------------------------------");
		}

		private void Check(List<Transform> _trs, string _code)
		{
			LabelCode lCode = GetCode(_code);

			Debug.Log($"***** codename : {lCode.ToString()}");
			_trs.ForEach(x =>
			{
				if (x.name.Contains(_code))
				{
					Debug.Log($"***** find label : name {x.name}");
				}
			});
		}

		#endregion

		private LabelCode GetCode(string _code)
		{
			LabelCode result = LabelCode.Null;

			if (_code.Contains(arguments.m_labelButton))
			{
				result = LabelCode.Button;
			}
			else if (_code.Contains(arguments.m_labelBoundary))
			{
				result = LabelCode.Boundary;
			}
			else if (_code.Contains(arguments.m_labelBackground))
			{
				result = LabelCode.Background;
			}
			else if (_code.Contains(arguments.m_labelText))
			{
				result = LabelCode.Text;
			}
			else if (_code.Contains(arguments.m_labelImage))
			{
				result = LabelCode.Image;
			}

			return result;
		}

		private string GetLabelString(LabelCode _code)
		{
			string result = "";

			if(_code == LabelCode.Button)
			{
				result = "btn";
			}
			else if(_code == LabelCode.Boundary)
			{
				result = "bb";
			}
			else if(_code == LabelCode.Background)
			{
				result = "bg";
			}
			else if(_code == LabelCode.Text)
			{
				result = "tx";
			}
			else if(_code == LabelCode.Image)
			{
				result = "im";
			}

			return result;
		}
	}
}

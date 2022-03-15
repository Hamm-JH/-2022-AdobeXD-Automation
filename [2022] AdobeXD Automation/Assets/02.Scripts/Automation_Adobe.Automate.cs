using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using System;
	using UnityEditor;
	using System.Linq;
	using Preset;
	using UIs;

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

			// 루트 패널 객체를 생성한다.
			// 루트 패널 객체를 캔버스의 자식으로 할당한다.
			GameObject rootPanel = Objects.CreateRootPanel("UI Panel", m_rootCanvas.GetComponent<RectTransform>().sizeDelta);
			rootPanel.transform.SetParent(m_rootCanvas.transform);

			// 각 패널 객체들을 생성한다.
			CretaePanels(id_panels, rootPanel);

			// 생성한 객체들을 

			//new GameObject("UI Panel");
			//rootPanel.AddComponent<RectTransform>();
			//rootPanel.AddComponent<CanvasRenderer>().cullTransparentMesh = true;

			// 저장된 Dictionary의 각 Value의 클래스에 객체 생성을 지시한다. - 끝

			// 생성된 객체들의 부모 자식 구조를 할당한다.

			return;

			// Debug
			//DebugElements(trs);
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
				// 이름에 id 태그가 존재하는가?
				bool isTagContains = x.name.Contains(m_tagID);

				// 이름에 지정된 라벨중에 하나가 존재하는가?
				bool isLabelExist = GetCode(x.name) != LabelCode.Null;

				// 이름을 주 스플릿 코드로 나누었을때 수가 2 이상인가?
				bool isMinSplitArgsConfirmed = x.name.Split(m_split.ToCharArray()).Length >= 2;

				if(isTagContains
				&& isLabelExist
				&& isMinSplitArgsConfirmed)
				{
					result.Add(x);
				}
			});

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

				Utilities.GetSplitDatas(x.name, m_split, 
					m_labelButton, m_labelBoundary, m_labelBackground, m_labelText, m_labelImage,
					m_tagID, m_splitKeyValue, out lCode, out id);

				SetPanel(x, id, lCode, result);

				string res = "";
				
				//string[] splits = x.name.Split(m_split.ToCharArray());
				//splits.ToList().ForEach(y => res += $" [{y}]");
				//Debug.Log($"_elem name : {x.name} // {res}");
			});

			return result;
		}

		//private void GetSplitDatas(string _name, string _split, out LabelCode _lCode, out string _id)
		//{
		//	string[] splits = _name.Split(_split.ToCharArray());

		//	_lCode = GetCode(splits[0]);

		//	_id = GetID(splits[1]);
		//}

		/// <summary>
		/// Dictionary의 패널 Value에 id값에 맞는 Transform을 할당합니다.
		/// </summary>
		/// <param name="elem"></param>
		/// <param name="_id"></param>
		/// <param name="_lCode"></param>
		/// <param name="_result"></param>
		private void SetPanel(Transform elem, string _id, LabelCode _lCode, Dictionary<string, Panel> _result)
		{
			if (_result.ContainsKey(_id))
			{
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

			string[] splits = pName.Split(m_split.ToCharArray());

			string id = null;

			// 최상위 개체가 아닌 경우
			if(splits.Length >= 2)
			{
				id = GetID(splits[1]);
			}

			// 최상위 개체인 경우 null값은 없다.
			return id;
		}

		#endregion

		#region 3 Create Panels

		/// <summary>
		/// Dictionary 기반으로 패널을 생성한다.
		/// </summary>
		/// <param name="_panels"></param>
		private void CretaePanels(Dictionary<string, Panel> _panels, GameObject _rootPanel)
		{
			foreach(Panel pan in _panels.Values)
			{
				pan.CreatePanel(_rootPanel, m_split,
					m_labelButton, m_labelBoundary, m_labelBackground, m_labelText, m_labelImage,
					m_tagID, m_splitKeyValue);
			}
		}

		#endregion

		#region 4 Set Panel parents

		private void SetPanelParents()
		{

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
			Check(_trs, m_labelButton);
			Debug.Log("-------------------------------------");
			Check(_trs, m_labelBoundary);
			Debug.Log("-------------------------------------");
			Check(_trs, m_labelBackground);
			Debug.Log("-------------------------------------");
			Check(_trs, m_labelText);
			Debug.Log("-------------------------------------");
			Check(_trs, m_labelImage);
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

		private string[] SplitName(string name, string _split)
		{
			return name.Split(_split.ToCharArray());
		}

		private LabelCode GetCode(string _code)
		{
			LabelCode result = LabelCode.Null;

			if (_code.Contains(m_labelButton))
			{
				result = LabelCode.Button;
			}
			else if (_code.Contains(m_labelBoundary))
			{
				result = LabelCode.Boundary;
			}
			else if (_code.Contains(m_labelBackground))
			{
				result = LabelCode.Background;
			}
			else if (_code.Contains(m_labelText))
			{
				result = LabelCode.Text;
			}
			else if (_code.Contains(m_labelImage))
			{
				result = LabelCode.Image;
			}

			return result;
		}

		private string GetID(string _code)
		{
			string result = "";

			result = _code.Replace($"{m_tagID}{m_splitKeyValue}", "");

			return result;
		}
	}
}

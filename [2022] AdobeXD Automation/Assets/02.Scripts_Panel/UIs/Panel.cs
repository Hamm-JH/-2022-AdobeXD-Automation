using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIs
{
	using Preset;
	using System.Linq;
	using UnityEngine.UI;
	using static Automation.Automation_Adobe;

	public class Panel
	{
		public Panel()
		{
			m_inElements = new List<Transform>();
			m_subElements = new List<GameObject>();
			m_instancedElements = new Dictionary<LabelCode, List<GameObject>>();
		}

		/// <summary>
		/// 패널 요소들의 root가 되는 객체
		/// </summary>
		private GameObject m_panel;

		private List<GameObject> m_subElements;

		/// <summary>
		/// 부모 개체의 ID
		/// 패널 간의 위치 배치시에 필요하다.
		/// </summary>
		private string m_parentID;

		/// <summary>
		/// 받아온 시안의 요소들
		/// </summary>
		private List<Transform> m_inElements;

		/// <summary>
		/// TODO 작업 보류 (이 개체의 목적은 subElements로 이전됨)
		/// 생성단계중에 생성된 요소들
		/// </summary>
		private Dictionary<LabelCode, List<GameObject>> m_instancedElements;

		public GameObject IPanel { get => m_panel; set => m_panel=value; }
		public List<GameObject> SubElements { get => m_subElements; set => m_subElements=value; }
		public List<Transform> InElements { get => m_inElements; set => m_inElements=value; }

		/// <summary>
		/// window에서 가져온 데이터
		/// </summary>
		string m_split = "_";
		string m_splitKeyValue = "::";
		string m_labelButton = "btn";
		string m_labelBoundary = "bb";
		string m_labelBackground = "bg";
		string m_labelText = "tx";
		string m_labelImage = "im";
		string m_tagID = "id";
		string m_tagFunction = "fn";

		public void AddElement(Transform _tr)
		{
			if (m_inElements == null) m_inElements = new List<Transform>();

			InElements.Add(_tr);
		}

		/// <summary>
		/// null또는 존재하는 부모 개체의 ID 값을 얻는다.
		/// null값인 경우 Panel 사이의 부모관계가 없는 루트 직전 최상위 개체임
		/// </summary>
		/// <param name="_id"></param>
		public void SetParentID(string _id)
		{
			m_parentID = _id;
		}

		#region 1 Create Panel

		/// <summary>
		/// 패널을 생성한다.
		/// </summary>
		/// <param name="_rootPanel"></param>
		public void CreatePanel(GameObject _rootPanel, string _split, 
			string _labelButton, string _labelBoundary, string _labelBackground, string _labelText, string _labelImage,
			string _tagID, string _splitKeyValue)
		{
			m_split = _split;
			m_labelButton = _labelButton;
			m_labelBoundary = _labelBoundary;
			m_labelBackground = _labelBackground;
			m_labelText = _labelText;
			m_labelImage = _labelImage;
			m_tagID = _tagID;
			m_splitKeyValue = _splitKeyValue;

			LabelCode lCode = LabelCode.Null;
			string id = "";

			// 1 Element 생성단계
			// id 단위로 element 집계된 대상에 대해 새롭게 생성된 element 객체를 생성한다.
			// X 단일 패널 내부에서 생성된 객체들은 m_instancedElements 사전 변수에 할당한다.
			// X 단일 패널 내부에서 생성된 객체들은 라벨에 따라 target또는 subElement변수/리스트에 할당한다.
			// 1차적으로 rootPanel에 수평적으로 할당한다.
			m_inElements.ForEach(x =>
			{
				CreateElement(_rootPanel, x);
			});

			// 2 Element 내부 배치단계
			// 1번 과정에서 m_instancedElements 사전 변수에 할당된 객체들을 대상으로 패널 내부의 배치를 진행한다.
			SetElementPos(IPanel, SubElements);
		}

		#region 1-1 Element 생성단계

		/// <summary>
		/// 각 Element들을 생성한다.
		/// </summary>
		/// <param name="_tr"></param>
		private void CreateElement(GameObject _rootPanel, Transform _tr)
		{
			LabelCode lCode = LabelCode.Null;
			string id = "";

			Utilities.GetSplitDatas(_tr.name, m_split,
					m_labelButton, m_labelBoundary, m_labelBackground, m_labelText, m_labelImage,
					m_tagID, m_splitKeyValue, out lCode, out id);

			//Debug.Log($"code name : {lCode.ToString()}");

			switch(lCode)
			{
				case LabelCode.Boundary:
					Create_Boundary(_rootPanel, _tr, lCode, id);
					break;

				case LabelCode.Button:
					Create_Button(_rootPanel, _tr, lCode, id);
					break;

				case LabelCode.Background:
					Create_Background(_rootPanel, _tr, lCode, id);
					break;

				case LabelCode.Image:
					Create_Image(_rootPanel, _tr, lCode, id);
					break;

				case LabelCode.Text:
					Create_Text(_rootPanel, _tr, lCode, id);
					break;
			}
		}

		#region Create Element

		/// <summary>
		/// Boundary 생성
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Boundary(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			// tr에서 추출할 데이터 : RectTransform
			GameObject obj = Objects.CreatePanel($"ID{_id}_bb", _tr.GetComponent<RectTransform>());

			obj.transform.SetParent(_rootPanel.transform);

			// 요소들의 root로 지정한다.
			IPanel = obj;

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// Button 생성
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Button(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			GameObject obj = Objects.CreatePanel($"ID{_id}_btn", _tr.GetComponent<RectTransform>());
			Objects.AddButton(obj);

			obj.transform.SetParent(_rootPanel.transform);

			// 요소들의 root로 지정한다.
			IPanel = obj;

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// 배경 생성
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Background(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			GameObject obj = Objects.CreatePanel($"ID{_id}_bg", _tr.GetComponent<RectTransform>());
			Objects.AddImage(obj, _tr.GetComponent<Image>());
			obj.transform.SetParent(_rootPanel.transform);

			// 재배치 요소로 할당한다.
			SubElements.Add(obj);

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// Image 생성
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Image(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			// _tr에서 추출할 데이터 : Image sprite
			GameObject obj = Objects.CreatePanel($"ID{_id}_im", _tr.GetComponent<RectTransform>());
			Objects.AddImage(obj, _tr.GetComponent<Image>());

			obj.transform.SetParent(_rootPanel.transform);

			// 재배치 요소로 할당한다.
			SubElements.Add(obj);

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// Text 생성
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Text(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			Debug.Log($"tr name {_tr.name}");
			GameObject obj = Objects.CreatePanel($"ID{_id}_tx", _tr.GetComponent<RectTransform>());
			Objects.AddText(obj, _tr.GetComponent<Text>());

			obj.transform.SetParent(_rootPanel.transform);

			// 재배치 요소로 할당한다.
			SubElements.Add(obj);

			AddNewInstance(obj, _lCode);
		}

		private void AddNewInstance(GameObject _instance, LabelCode _lCode)
		{
			if (!m_instancedElements.ContainsKey(_lCode))
			{
				m_instancedElements.Add(_lCode, new List<GameObject>());
				m_instancedElements[_lCode].Add(_instance);
			}
			else
			{
				m_instancedElements[_lCode].Add(_instance);
			}
		}

		#endregion

		#endregion

		#region 1-2 Element 내부 배치단계

		private void SetElementPos(GameObject _panel, List<GameObject> _subs)
		{
			if(_panel == null)
			{
				//Debug.LogError($"element : {_subs.First().name} have not parent panel");
				throw new System.Exception($"element : {_subs.First().name} have not parent panel");
			}
			_subs.ForEach(x => x.transform.SetParent(_panel.transform));
		}

		#endregion

		#endregion

		#region 2 Set Panel's parent

		/// <summary>
		/// EditorWindow에서 가공되어있는 Dictionary 변수를 갖고와서 이 패널 인스턴스의 부모를 할당한다.
		/// </summary>
		/// <param name="_panels"> 패널 사전 변수 </param>
		public void SetPanelParent(Dictionary<string, Panel> _panels)
		{
			// 부모의 ID를 가지고 옴
			string parentID = m_parentID;

			// 부모 ID와 매칭되는 패널이 존재하는 경우
			if(parentID != null && _panels.ContainsKey(parentID))
			{
				// 부모 ID를 가진 객체로 이 인스턴스의 패널 부모 할당
				IPanel.transform.SetParent(_panels[parentID].IPanel.transform);
			}

		}

		#endregion

	}
}

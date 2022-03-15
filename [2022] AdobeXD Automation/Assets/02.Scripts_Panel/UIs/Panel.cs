using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIs
{
	using Preset;
	using UnityEngine.UI;
	using static Automation.Automation_Adobe;

	public class Panel
	{
		public Panel()
		{
			inElements = new List<Transform>();
			m_instancedElements = new Dictionary<LabelCode, List<GameObject>>();
		}

		private GameObject target;
		private string parentID;

		/// <summary>
		/// �޾ƿ� �þ��� ��ҵ�
		/// </summary>
		private List<Transform> inElements;

		/// <summary>
		/// �����ܰ��߿� ������ ��ҵ�
		/// </summary>
		private Dictionary<LabelCode, List<GameObject>> m_instancedElements;

		public GameObject Target { get => target; set => target=value; }
		public List<Transform> InElements { get => inElements; set => inElements=value; }

		/// <summary>
		/// window���� ������ ������
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
			if (inElements == null) inElements = new List<Transform>();

			InElements.Add(_tr);
		}

		/// <summary>
		/// null�Ǵ� �����ϴ� �θ� ��ü�� ID ���� ��´�.
		/// null���� ��� Panel ������ �θ���谡 ���� ��Ʈ ���� �ֻ��� ��ü��
		/// </summary>
		/// <param name="_id"></param>
		public void SetParentID(string _id)
		{
			parentID = _id;
		}

		/// <summary>
		/// �г��� �����Ѵ�.
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

			// 1 Element �����ܰ�
			inElements.ForEach(x =>
			{
				CreateElement(_rootPanel, x);
			});

			// 2 Element ���� ��ġ�ܰ�
		}

		/// <summary>
		/// �� Element���� �����Ѵ�.
		/// </summary>
		/// <param name="_tr"></param>
		private void CreateElement(GameObject _rootPanel, Transform _tr)
		{
			LabelCode lCode = LabelCode.Null;
			string id = "";

			Utilities.GetSplitDatas(_tr.name, m_split,
					m_labelButton, m_labelBoundary, m_labelBackground, m_labelText, m_labelImage,
					m_tagID, m_splitKeyValue, out lCode, out id);

			Debug.Log($"code name : {lCode.ToString()}");

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



		/// <summary>
		/// Boundary ����
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Boundary(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			// tr���� ������ ������ : RectTransform
			GameObject obj = Objects.CreatePanel($"ID{_id}_bb", _tr.GetComponent<RectTransform>());

			obj.transform.SetParent(_rootPanel.transform);

			AddNewInstance(obj, _lCode);
		}

		private void Create_Button(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			GameObject obj = Objects.CreatePanel($"ID{_id}_btn", _tr.GetComponent<RectTransform>());
			Objects.AddButton(obj);

			obj.transform.SetParent(_rootPanel.transform);

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// ��� ����
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

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// Image ����
		/// </summary>
		/// <param name="_rootPanel"></param>
		/// <param name="_tr"></param>
		/// <param name="_lCode"></param>
		/// <param name="_id"></param>
		private void Create_Image(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
			// _tr���� ������ ������ : Image sprite
			GameObject obj = Objects.CreatePanel($"ID{_id}_im", _tr.GetComponent<RectTransform>());
			Objects.AddImage(obj, _tr.GetComponent<Image>());

			obj.transform.SetParent(_rootPanel.transform);

			AddNewInstance(obj, _lCode);
		}

		/// <summary>
		/// Text ����
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

	}
}

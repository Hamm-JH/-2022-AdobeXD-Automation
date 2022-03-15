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

			// �г�ȭ ���� ����� �˻�, �з��Ѵ�.
			// 1 ���̵� ����, ��, ���ø� ���� �����ϴ��� Ȯ���Ѵ�.
			List<Transform> elements = SetTargetList(trs);

			// ���̵�, ���� �����Ѵٸ� panelȭ ���� ������� ����.
			// 2 �г�ȭ ���� ������ �з��ؼ� Dictionary�� �����Ѵ�.
			Dictionary<string, Panel> id_panels = SetPanelInstance(elements);

			// ��Ʈ �г� ��ü�� �����Ѵ�.
			// ��Ʈ �г� ��ü�� ĵ������ �ڽ����� �Ҵ��Ѵ�.
			GameObject rootPanel = Objects.CreateRootPanel("UI Panel", m_rootCanvas.GetComponent<RectTransform>().sizeDelta);
			rootPanel.transform.SetParent(m_rootCanvas.transform);

			// �� �г� ��ü���� �����Ѵ�.
			CretaePanels(id_panels, rootPanel);

			// ������ ��ü���� 

			//new GameObject("UI Panel");
			//rootPanel.AddComponent<RectTransform>();
			//rootPanel.AddComponent<CanvasRenderer>().cullTransparentMesh = true;

			// ����� Dictionary�� �� Value�� Ŭ������ ��ü ������ �����Ѵ�. - ��

			// ������ ��ü���� �θ� �ڽ� ������ �Ҵ��Ѵ�.

			return;

			// Debug
			//DebugElements(trs);
		}

		#region 1 Set target list

		/// <summary>
		/// 1. ID�� �±�, ���� ���ڿ� ���� �ּ� 2�� �̻��� ��ü�� ����Ʈ�� ��ȯ�Ѵ�.
		/// </summary>
		/// <param name="_trs"></param>
		/// <returns></returns>
		private List<Transform> SetTargetList(List<Transform> _trs)
		{
			List<Transform> result = new List<Transform>();

			_trs.ForEach(x =>
			{
				// �̸��� id �±װ� �����ϴ°�?
				bool isTagContains = x.name.Contains(m_tagID);

				// �̸��� ������ ���߿� �ϳ��� �����ϴ°�?
				bool isLabelExist = GetCode(x.name) != LabelCode.Null;

				// �̸��� �� ���ø� �ڵ�� ���������� ���� 2 �̻��ΰ�?
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
		/// Transform ����Ʈ���� Dictionary�� id���� ���� �Ҵ��Ѵ�.
		/// </summary>
		/// <param name="_elems"></param>
		/// <returns></returns>
		private Dictionary<string, Panel> SetPanelInstance(List<Transform> _elems)
		{
			Dictionary<string, Panel> result = new Dictionary<string, Panel>();

			_elems.ForEach(x =>
			{
				//splits[0]; // ��
				LabelCode lCode = LabelCode.Null;
				//splits[1]; // ���̵�
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
		/// Dictionary�� �г� Value�� id���� �´� Transform�� �Ҵ��մϴ�.
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
		/// �θ� ��ü�� ID�� ��´�.
		/// �ֻ��� ��ü�� ��� null���� �ش�.
		/// </summary>
		/// <param name="_tr"></param>
		private string GetParentID(Transform _tr)
		{
			string pName = _tr.parent.name;

			string[] splits = pName.Split(m_split.ToCharArray());

			string id = null;

			// �ֻ��� ��ü�� �ƴ� ���
			if(splits.Length >= 2)
			{
				id = GetID(splits[1]);
			}

			// �ֻ��� ��ü�� ��� null���� ����.
			return id;
		}

		#endregion

		#region 3 Create Panels

		/// <summary>
		/// Dictionary ������� �г��� �����Ѵ�.
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
		/// label �� ��ü�� Ȯ���Ѵ�.
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

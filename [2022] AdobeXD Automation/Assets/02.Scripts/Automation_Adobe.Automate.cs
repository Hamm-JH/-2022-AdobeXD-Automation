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

			// 3 ��ȯ�ϰ��� �ϴ� ��ü�� ID�� ��� �������� ��ġ �� �θ�ü ID�� ������ �Ϸ�� ���¿���
			// �ùٸ� ���ġ�� ���� ���� ��ü�� ���ġ�Ѵ�.
			ReplacePanels(trs, _obj);

			// ��Ʈ �г� ��ü�� �����Ѵ�.
			// - ��Ʈ �г� ��ü�� ĵ������ �ڽ����� �Ҵ��Ѵ�.
			GameObject rootPanel = Objects.CreateRootPanel("UI Panel", m_rootCanvas.GetComponent<RectTransform>().sizeDelta);
			rootPanel.transform.SetParent(m_rootCanvas.transform);

			// 4 �� �г� ��ü���� �����Ѵ�.
			CretaePanels(id_panels, rootPanel);

			// 5 ������ �гΰ�ü�� ���� �θ����� �����Ѵ�.
			SetPanelParents(id_panels);

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
				if (IsTargetObject(x, m_tagID, m_split))
				{
					result.Add(x);
				}
				// TODO Check
				//// �̸��� id �±װ� �����ϴ°�?
				//bool isTagContains = x.name.Contains(m_tagID);

				//// �̸��� ������ ���߿� �ϳ��� �����ϴ°�?
				//bool isLabelExist = GetCode(x.name) != LabelCode.Null;

				//// �̸��� �� ���ø� �ڵ�� ���������� ���� 2 �̻��ΰ�?
				//bool isMinSplitArgsConfirmed = x.name.Split(m_split.ToCharArray()).Length >= 2;

				//if(isTagContains
				//&& isLabelExist
				//&& isMinSplitArgsConfirmed)
				//{
				//	result.Add(x);
				//}
			});

			return result;
		}

		private bool IsTargetObject(Transform _tr, string _tagID, string _split)
		{
			bool result = false;

			// �̸��� id �±װ� �����ϴ°�?
			bool isTagContains = _tr.name.Contains(_tagID);

			// �̸��� ������ ���߿� �ϳ��� �����ϴ°�?
			bool isLabelExist = GetCode(_tr.name) != LabelCode.Null;

			// �̸��� �� ���ø� �ڵ�� ���������� ���� 2 �̻��ΰ�?
			bool isMinSplitArgsConfirmed = _tr.name.Split(_split.ToCharArray()).Length >= 2;

			if (isTagContains
			&& isLabelExist
			&& isMinSplitArgsConfirmed)
			{
				result = true;
			}

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
				id = Utilities.GetID(splits[1], m_tagID, m_splitKeyValue);
			}

			// �ֻ��� ��ü�� ��� null���� ����.
			return id;
		}

		#endregion

		#region 3 Replace Panel 

		private void ReplacePanels(List<Transform> _trs, GameObject _rootPanel)
		{
			Debug.LogWarning("TODO Anchor Pos, Pivot ��ġ�� �ʱ�ȭ�� ���Ŀ� ���� // ������ �������� ��Ŀ��, �Ǻ��� �����Ϳ��� �ʱ�ȭ");
			// TODO Anchor Pos, Pivot ��ġ�� �ʱ�ȭ�� ���Ŀ� ���� // ������ �������� ��Ŀ��, �Ǻ��� �����Ϳ��� �ʱ�ȭ
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

		//private string[] SplitName(string name, string _split)
		//{
		//	return name.Split(_split.ToCharArray());
		//}

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

		//private string GetID(string _code, string m_tagID, string m_splitKeyValue)
		//{
		//	string result = "";

		//	result = _code.Replace($"{m_tagID}{m_splitKeyValue}", "");

		//	return result;
		//}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using System;
	using UnityEditor;
	using System.Linq;

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

			//Debug.Log("-------------------------------------");
			//Check(trs, m_labelButton);
			//Debug.Log("-------------------------------------");
			//Check(trs, m_labelBoundary);
			//Debug.Log("-------------------------------------");
			//Check(trs, m_labelBackground);
			//Debug.Log("-------------------------------------");
			//Check(trs, m_labelText);
			//Debug.Log("-------------------------------------");
			//Check(trs, m_labelImage);
			//Debug.Log("-------------------------------------");

			trs.ForEach(x =>
			{
				if(x.name.Contains(m_tagID))
				{
					Debug.Log($"find tag : id {x.name.Replace("::", ":").Split(':')[1]}");
				}
			});

			// 라벨 문자 확인 끝나고 생성단계 진행][
			return;

			GameObject obj = new GameObject("Element 1");

			obj.transform.SetParent(_obj.transform);
		}

		private void Check(List<Transform> _trs, string _code)
		{
			LabelCode lCode = GetCode(_code);

			_trs.ForEach(x =>
			{
				if (x.name.Contains(_code))
				{
					Debug.Log($"find label : name {x.name}");
				}
			});

			Debug.Log($"***** codename : {lCode.ToString()}");
		}

		private LabelCode GetCode(string _code)
		{
			LabelCode result = LabelCode.Null;

			if (_code == m_labelButton)
			{
				result = LabelCode.Button;
			}
			else if (_code == m_labelBoundary)
			{
				result = LabelCode.Boundary;
			}
			else if (_code == m_labelBackground)
			{
				result = LabelCode.Background;
			}
			else if (_code == m_labelText)
			{
				result = LabelCode.Text;
			}
			else if (_code == m_labelImage)
			{
				result = LabelCode.Image;
			}

			return result;
		}


		private string GetID(string _name, string _idCode)
		{
			string result = "";



			return result;
		}
	}
}

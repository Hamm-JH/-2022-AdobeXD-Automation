using Preset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIs
{
	using static Automation.Automation_Adobe;

	public class Panel
	{
		private GameObject target;
		private string parentID;

		private List<Transform> inElements;

		public GameObject Target { get => target; set => target=value; }
		public List<Transform> InElements { get => inElements; set => inElements=value; }

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
			if (inElements == null) inElements = new List<Transform>();

			InElements.Add(_tr);
		}

		/// <summary>
		/// null또는 존재하는 부모 개체의 ID 값을 얻는다.
		/// null값인 경우 Panel 사이의 부모관계가 없는 루트 직전 최상위 개체임
		/// </summary>
		/// <param name="_id"></param>
		public void SetParentID(string _id)
		{
			parentID = _id;
		}

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

			inElements.ForEach(x =>
			{
				CreateElement(x);
			});
		}

		private void CreateElement(Transform _tr)
		{
			LabelCode lCode = LabelCode.Null;
			string id = "";

			Utilities.GetSplitDatas(_tr.name, m_split,
					m_labelButton, m_labelBoundary, m_labelBackground, m_labelText, m_labelImage,
					m_tagID, m_splitKeyValue, out lCode, out id);

			switch(lCode)
			{
				case LabelCode.Boundary:

					break;

				case LabelCode.Button:

					break;

				case LabelCode.Background:

					break;

				case LabelCode.Image:

					break;

				case LabelCode.Text:

					break;
			}
		}

	}
}

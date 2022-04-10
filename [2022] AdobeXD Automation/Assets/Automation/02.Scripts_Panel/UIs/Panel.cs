using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIs
{
    using Automation;
    using Automation.Definition;
    using Automation.Templates.ModernUI;
    using Presets;
    using System.Linq;
    using TMPro;
    using UnityEngine.UI;
    using Utilities;
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

        public GameObject IPanel { get => m_panel; set => m_panel = value; }
        public List<GameObject> SubElements { get => m_subElements; set => m_subElements = value; }
        public List<Transform> InElements { get => m_inElements; set => m_inElements = value; }

        /// <summary>
        /// window에서 가져온 데이터
        /// </summary>
        Automation.Data.AutomationArguments m_arguments;

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
        public void CreatePanel(GameObject _rootPanel, Automation.Data.AutomationArguments _arguments)
        {
            m_arguments = _arguments;

            //LabelCode lCode = LabelCode.Null;
            //string id = "";

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
            //Debug.Log(_rootPanel.name);
            SetElementPos(IPanel, SubElements);

            // 3 Element별 상호작용 단계
            AssembleInPanel(_arguments);


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

            Utilities.GetSplitDatas(_tr.name, m_arguments, out lCode, out id);

            //Debug.Log($"code name : {lCode.ToString()}");

            switch (lCode)
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

                case LabelCode.Progressbar:
                    Create_ProgressbarPanel(_rootPanel, _tr, lCode, id);
                    break;

                case LabelCode.Progressbar_background:
                    Create_ProgressbarBackground(_rootPanel, _tr, lCode, id);
                    break;

                case LabelCode.Progressbar_highlight:
                    Create_ProgressbarHighlight(_rootPanel, _tr, lCode, id);
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
            string name = Panels.SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            // tr에서 추출할 데이터 : RectTransform
            GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());

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
            GameObject obj = Objects.Create_Button(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

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
            // 이미지 요소가 존재할 경우에만 신규 인스턴스 할당
            Image image;
            if (_tr.TryGetComponent<Image>(out image))
            {
                GameObject obj = Objects.Create_Image(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

                // 재배치 요소로 할당한다.
                SubElements.Add(obj);
                AddNewInstance(obj, _lCode);
            }
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
            // 이미지 요소가 존재할 경우에만 신규 인스턴스 할당
            Image image;
            if (_tr.TryGetComponent<Image>(out image))
            {
                GameObject obj = Objects.Create_Image(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

                // 재배치 요소로 할당한다.
                SubElements.Add(obj);
                AddNewInstance(obj, _lCode);
            }
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
            GameObject obj = Objects.Create_Text(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

            // 재배치 요소로 할당한다.
            SubElements.Add(obj);
            AddNewInstance(obj, _lCode);
        }

        /// <summary>
        /// 프로그레스 바 패널 생성
        /// </summary>
        /// <param name="_rootPanel"></param>
        /// <param name="_tr"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        private void Create_ProgressbarPanel(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
		{
            GameObject obj = Objects.Create_Progressbar(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

            IPanel = obj;
            AddNewInstance(obj, _lCode);
		}

        /// <summary>
        /// 프로그레스 바 배경 생성
        /// </summary>
        /// <param name="_rootPanel"></param>
        /// <param name="_tr"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        private void Create_ProgressbarBackground(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
        {
            GameObject obj = Objects.Create_ProgressbarBackground(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

            SubElements.Add(obj);
            AddNewInstance(obj, _lCode);
        }

        /// <summary>
        /// 프로그레스 하이라이트 생성
        /// </summary>
        /// <param name="_rootPanel"></param>
        /// <param name="_tr"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        private void Create_ProgressbarHighlight(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
        {
            GameObject obj = Objects.Create_ProgressbarHighlight(_rootPanel, _tr.gameObject, _lCode, _id, m_arguments);

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
            if (_panel == null)
            {
                return;
                //Debug.LogError($"element : {_subs.First().name} have not parent panel");
                Debug.LogError($"element : {_subs.First().name} have not parent panel");
                //throw new System.Exception($"element : {_subs.First().name} have not parent panel");
            }
            else
            {
                _subs.ForEach(x => x.transform.SetParent(_panel.transform));
            }
        }

        #endregion

        #region 1-3 Element 별 상호작용 단계

        private void AssembleInPanel(Automation.Data.AutomationArguments _arguments)
        {
            LabelCode rootLCode = LabelCodes.GetCode(IPanel.name, _arguments);

            if(LabelCodes.IsButton(rootLCode))
            {
                Assemble_Button(IPanel, SubElements, _arguments);
            }
            else if(LabelCodes.IsProgressBar(rootLCode))
            {
                Assemble_Progressbar(IPanel, SubElements, _arguments);
            }
            else if(LabelCodes.IsBoundary(rootLCode))
            {
                Assemble_Boundary(IPanel, SubElements, _arguments);
            }
            
        }

        /// <summary>
        /// 버튼 조립
        /// </summary>
        /// <param name="_iPanel"></param>
        /// <param name="_subElems"></param>
        private void Assemble_Button(GameObject _iPanel, List<GameObject> _subElems,
            Automation.Data.AutomationArguments _arguments)
        {
            Image background = null;
            GameObject target = null;

            foreach(GameObject obj in _subElems)
            {
                if(Panels.TryGetElement(obj, LabelCode.Background, _arguments, out target))
                {
                    background = target.GetComponent<Image>();
                    break;
                }
            }

            // bg 라벨을 가진 개체가 있을 경우 이미지 전달 실행
            if(background != null)
            {
                Image rImg;
                if(_iPanel.TryGetComponent<Image>(out rImg))
                {
                    rImg.sprite = background.sprite;
                }
                GameObject.DestroyImmediate(background.gameObject);
            }
        }

        /// <summary>
        /// 프로그레스바 조립
        /// </summary>
        /// <param name="_iPanel"></param>
        /// <param name="_subElems"></param>
        private void Assemble_Progressbar(GameObject _iPanel, List<GameObject> _subElems,
            Automation.Data.AutomationArguments _arguments)
        {
            MUI_ProgressBar pBar = _iPanel.GetComponent<MUI_ProgressBar>();

            if (pBar == null) return;

            Debug.Log("Hello progressbar");

            Image background = null;
            Image highlight = null;
            GameObject textObj = null;

            GameObject target = null;

            foreach(GameObject obj in _subElems)
            {
                if(Panels.TryGetElement(obj, LabelCode.Progressbar_background, _arguments, out target))
                {
                    background = target.GetComponent<Image>();
                }
                else if(Panels.TryGetElement(obj, LabelCode.Progressbar_highlight, _arguments, out target))
                {
                    highlight = target.GetComponent<Image>();
                }
                else if(Panels.TryGetElement(obj, LabelCode.Text, _arguments, out target))
                {
                    textObj = target;
                }
            }

            if (background)
            {
                Color colr = background.color;
				//pBar.m_progressBar.
				//pBar.m_mgrProcessBar.background.color = new Color(0, 1, 0, 0.2f);       // 배경색
				pBar.m_mgrProcessBar.background.color = new Color(colr.r, colr.g, colr.b, 0.2f);       // 배경색
				GameObject.DestroyImmediate(background.gameObject);
            }

            if(highlight)
            {
                Color colr = highlight.color;
                pBar.m_mgrProcessBar.bar.color = new Color(colr.r, colr.g, colr.b, colr.a);                 // 하이라이트색
                GameObject.DestroyImmediate(highlight.gameObject);
            }

            if(textObj)
            {
                Style style = _arguments.m_style;
                if(Styles.IsDefaultText(style))
                {
                    Text txt = textObj.GetComponent<Text>();

                    TextMeshProUGUI tg = pBar.m_mgrProcessBar.label;

                    tg.font = _arguments.Tmp_TMPro.m_fontAsset;
                    tg.text = txt.text;

                    tg.enableAutoSizing = true;
                    tg.fontSizeMin = txt.fontSize - 5;
                    tg.fontSizeMax = txt.fontSize;
                    tg.alignment = TextAlignmentOptions.Midline;
                    tg.color = txt.color;
                }
                else if(Styles.IsTMProText(style))
                {
                    TextMeshProUGUI txt = textObj.GetComponent<TextMeshProUGUI>();

                    TextMeshProUGUI tg = pBar.m_mgrProcessBar.label;

                    tg.font = _arguments.Tmp_TMPro.m_fontAsset;
                    tg.text = txt.text;

                    tg.enableAutoSizing = true;
                    tg.fontSizeMin = txt.fontSize - 5;
                    tg.fontSizeMax = txt.fontSize;
                    tg.alignment = TextAlignmentOptions.Midline;
                    tg.color = txt.color;
                }

                GameObject.DestroyImmediate(textObj);
            }
        }

        /// <summary>
        /// 경계 조립
        /// </summary>
        /// <param name="_iPanel"></param>
        /// <param name="_subElems"></param>
        /// <param name="_arguements"></param>
        private void Assemble_Boundary(GameObject _iPanel, List<GameObject> _subElems,
            Automation.Data.AutomationArguments _arguements)
        {
            Image background = null;
            GameObject target = null;

            foreach(GameObject obj in _subElems)
            {
                if(Panels.TryGetElement(obj, LabelCode.Background, _arguements, out target))
                {
                    background = target.GetComponent<Image>();
                    break;
                }
                else
				{
                    
				}
            }

            //if(background != null)
            //{
            //    Image rImg;
            //    if(_iPanel.TryGetComponent<Image>(out rImg))
            //    {
            //        rImg.enabled = true;
            //        rImg.sprite = background.sprite;
            //        GameObject.DestroyImmediate(background.gameObject);
            //    }
            //    //GameObject.DestroyImmediate(background.gameObject);
            //}
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
            if (parentID != null && _panels.ContainsKey(parentID))
            {
                // 부모 ID를 가진 객체로 이 인스턴스의 패널 부모 할당
                IPanel.transform.SetParent(_panels[parentID].IPanel.transform);
            }

        }

        #endregion

    }
}

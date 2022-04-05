using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIs
{
    using Automation;
    using Presets;
    using System.Linq;
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
        /// �г� ��ҵ��� root�� �Ǵ� ��ü
        /// </summary>
        private GameObject m_panel;

        private List<GameObject> m_subElements;

        /// <summary>
        /// �θ� ��ü�� ID
        /// �г� ���� ��ġ ��ġ�ÿ� �ʿ��ϴ�.
        /// </summary>
        private string m_parentID;

        /// <summary>
        /// �޾ƿ� �þ��� ��ҵ�
        /// </summary>
        private List<Transform> m_inElements;

        /// <summary>
        /// TODO �۾� ���� (�� ��ü�� ������ subElements�� ������)
        /// �����ܰ��߿� ������ ��ҵ�
        /// </summary>
        private Dictionary<LabelCode, List<GameObject>> m_instancedElements;

        public GameObject IPanel { get => m_panel; set => m_panel = value; }
        public List<GameObject> SubElements { get => m_subElements; set => m_subElements = value; }
        public List<Transform> InElements { get => m_inElements; set => m_inElements = value; }

        /// <summary>
        /// window���� ������ ������
        /// </summary>
        Automation.Data.AutomationArguments m_arguments;

        public void AddElement(Transform _tr)
        {
            if (m_inElements == null) m_inElements = new List<Transform>();

            InElements.Add(_tr);
        }

        /// <summary>
        /// null�Ǵ� �����ϴ� �θ� ��ü�� ID ���� ��´�.
        /// null���� ��� Panel ������ �θ���谡 ���� ��Ʈ ���� �ֻ��� ��ü��
        /// </summary>
        /// <param name="_id"></param>
        public void SetParentID(string _id)
        {
            m_parentID = _id;
        }

        #region 1 Create Panel

        /// <summary>
        /// �г��� �����Ѵ�.
        /// </summary>
        /// <param name="_rootPanel"></param>
        public void CreatePanel(GameObject _rootPanel, Automation.Data.AutomationArguments _arguments)
        {
            m_arguments = _arguments;

            LabelCode lCode = LabelCode.Null;
            string id = "";

            // 1 Element �����ܰ�
            // id ������ element ����� ��� ���� ���Ӱ� ������ element ��ü�� �����Ѵ�.
            // X ���� �г� ���ο��� ������ ��ü���� m_instancedElements ���� ������ �Ҵ��Ѵ�.
            // X ���� �г� ���ο��� ������ ��ü���� �󺧿� ���� target�Ǵ� subElement����/����Ʈ�� �Ҵ��Ѵ�.
            // 1�������� rootPanel�� ���������� �Ҵ��Ѵ�.
            m_inElements.ForEach(x =>
            {
                CreateElement(_rootPanel, x);
            });

            // 2 Element ���� ��ġ�ܰ�
            // 1�� �������� m_instancedElements ���� ������ �Ҵ�� ��ü���� ������� �г� ������ ��ġ�� �����Ѵ�.
            SetElementPos(IPanel, SubElements);

            // 3 Element�� ��ȣ�ۿ� �ܰ�
            AssembleInPanel(_arguments);


        }

        #region 1-1 Element �����ܰ�

        /// <summary>
        /// �� Element���� �����Ѵ�.
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
            }
        }

        #region Create Element

        /// <summary>
        /// Boundary ����
        /// </summary>
        /// <param name="_rootPanel"></param>
        /// <param name="_tr"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        private void Create_Boundary(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
        {
            string name = SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            // tr���� ������ ������ : RectTransform
            GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());

            obj.transform.SetParent(_rootPanel.transform);

            // ��ҵ��� root�� �����Ѵ�.
            IPanel = obj;

            AddNewInstance(obj, _lCode);
        }

        /// <summary>
        /// Button ����
        /// </summary>
        /// <param name="_rootPanel"></param>
        /// <param name="_tr"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        private void Create_Button(GameObject _rootPanel, Transform _tr, LabelCode _lCode, string _id)
        {
            string name = SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());
            Objects.AddButton(obj, m_arguments.m_style);

            obj.transform.SetParent(_rootPanel.transform);

            // ��ҵ��� root�� �����Ѵ�.
            IPanel = obj;

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
            string name = SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            // �̹��� ��Ұ� ������ ��쿡�� �ű� �ν��Ͻ� �Ҵ�
            Image image;
            if (_tr.TryGetComponent<Image>(out image))
            {
                GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());
                Objects.AddImage(obj, _tr.GetComponent<Image>(), m_arguments);
                obj.transform.SetParent(_rootPanel.transform);

                // ���ġ ��ҷ� �Ҵ��Ѵ�.
                SubElements.Add(obj);

                AddNewInstance(obj, _lCode);
            }
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
            string name = SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            // �̹��� ��Ұ� ������ ��쿡�� �ű� �ν��Ͻ� �Ҵ�
            Image image;
            if (_tr.TryGetComponent<Image>(out image))
            {
                // _tr���� ������ ������ : Image sprite
                GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());
                Objects.AddImage(obj, _tr.GetComponent<Image>(), m_arguments);

                obj.transform.SetParent(_rootPanel.transform);

                // ���ġ ��ҷ� �Ҵ��Ѵ�.
                SubElements.Add(obj);

                AddNewInstance(obj, _lCode);
            }
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
            string name = SetInstanceName(_tr.name, _lCode, _id, m_arguments);

            GameObject obj = Objects.CreatePanel(name, _tr.GetComponent<RectTransform>());
            Objects.AddText(obj, _tr.GetComponent<Text>(), m_arguments);

            obj.transform.SetParent(_rootPanel.transform);

            // ���ġ ��ҷ� �Ҵ��Ѵ�.
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

        private string SetInstanceName(string _originalName, LabelCode _lCode, string _id, Automation.Data.AutomationArguments _arguments)
        {
            string result = "";
            string splitCode = _arguments.m_splitKeyValue;

            // lCode�� �����Ǵ� �̸�
            string lName = SetLabelCodeName(_lCode);

            if (_arguments.m_isRemainResourceName)
            {
                //Debug.Log(_originalName);
                string original = "";
                if (Strings.IsSplitIndexOver(_originalName, 2, _arguments.m_split))
                {
                    //_originalName.IndexOf
                    string splitter = _arguments.m_split;
                    string[] splits = _originalName.Split(splitter.ToCharArray());

                    original = _originalName.Replace($"{splits[0]}{splitter}{splits[1]}{splitter}", "");
                }
                else
                {
                    original = "";
                }

                result = string.Format("ID{0}{1}_{2}_{3}", splitCode, _id, lName, original);
            }
            else
            {
                result = string.Format("ID{0}{1}_{2}", splitCode, _id, lName);
            }

            return result;
        }

        private string SetLabelCodeName(LabelCode _lCode)
        {
            string result = "";

            switch (_lCode)
            {
                case LabelCode.Boundary:
                    result = "bb";
                    break;

                case LabelCode.Button:
                    result = "btn";
                    break;

                case LabelCode.Background:
                    result = "bg";
                    break;

                case LabelCode.Image:
                    result = "im";
                    break;

                case LabelCode.Text:
                    result = "tx";
                    break;

                default:
                    throw new System.Exception("Label code not matched");
            }

            return result;
        }

        #endregion

        #endregion

        #region 1-2 Element ���� ��ġ�ܰ�

        private void SetElementPos(GameObject _panel, List<GameObject> _subs)
        {
            if (_panel == null)
            {
                //Debug.LogError($"element : {_subs.First().name} have not parent panel");
                throw new System.Exception($"element : {_subs.First().name} have not parent panel");
            }
            _subs.ForEach(x => x.transform.SetParent(_panel.transform));
        }

        #endregion

        #region 1-3 Element �� ��ȣ�ۿ� �ܰ�

        private void AssembleInPanel(Automation.Data.AutomationArguments _arguments)
        {
            //LabelCode rlCode = LabelCodes.GetCode(IPanel.name, _arguments);

            //// ��ư�� ���
            //if (rlCode == LabelCode.Button)
            //{
            //    Image img = null;
            //    GameObject target = null;
                
            //    // ���� ����߿��� ��� ���� �̹����� �����´�.
            //    SubElements.ForEach(x =>
            //    {
            //        if(Panels.TryGetElement(x, LabelCode.Background, _arguments, out target))
            //        {
            //            img = target.GetComponent<Image>();
            //        }
            //    });

            //    // ��� ���� ���� �̹����� ���� ���
            //    if(img != null)
            //    {
            //        Image rImg;
            //        // ��Ʈ�г��� �̹����� �Ҵ��Ѵ�.
            //        if(IPanel.TryGetComponent<Image>(out rImg))
            //        {
            //            rImg.sprite = img.sprite;
            //        }
            //    }

            //}
            //// ����� ���
            //else if (rlCode == LabelCode.Boundary)
            //{
            //    // ���� ����
            //}
        }

        #endregion

        #endregion

        #region 2 Set Panel's parent

        /// <summary>
        /// EditorWindow���� �����Ǿ��ִ� Dictionary ������ ����ͼ� �� �г� �ν��Ͻ��� �θ� �Ҵ��Ѵ�.
        /// </summary>
        /// <param name="_panels"> �г� ���� ���� </param>
        public void SetPanelParent(Dictionary<string, Panel> _panels)
        {
            // �θ��� ID�� ������ ��
            string parentID = m_parentID;

            // �θ� ID�� ��Ī�Ǵ� �г��� �����ϴ� ���
            if (parentID != null && _panels.ContainsKey(parentID))
            {
                // �θ� ID�� ���� ��ü�� �� �ν��Ͻ��� �г� �θ� �Ҵ�
                IPanel.transform.SetParent(_panels[parentID].IPanel.transform);
            }

        }

        #endregion

    }
}

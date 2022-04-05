using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
	using Automation.Definition;
	using UnityEditor;
	using UnityEditor.EditorTools;

	/// <summary>
	/// Version 0.1
	/// </summary>
	public partial class Automation_Adobe : EditorWindow
	{
		#region field

		/// <summary>
		/// 프로세스 실행에 필수적으로 필요한 canvas
		/// </summary>
		Canvas m_rootCanvas;

		/// <summary>
		/// 프로세스 실행에 필수적으로 필요한 대상 UI 인스턴스
		/// </summary>
		GameObject m_target;

		/// <summary>
		/// 프로세스 동작에 필요한 데이터 정의 인스턴스
		/// </summary>
		public Data.AutomationArguments arguments;

		/// <summary>
		/// Automation 실행 프로세스 분기를 결정한다.
		/// </summary>
		bool toggle_processOption;

		/// <summary>
		/// 템플릿별 변수 정의를 볼수 있게 한다.
		/// </summary>
		bool toggle_styleDefinition;

		/// <summary>
		/// 프로세스중에 정의된 예약 문자열을 본다.
		/// </summary>
		bool toggle_definedTextLayout;

		/// <summary>
		/// 두잇.
		/// </summary>
		string doButton = "Do It";

		public string IDTag
		{
			get => $"{arguments.m_tagID}{arguments.m_splitKeyValue}";
		}

		#endregion

		private static Automation_Adobe instance;

		public static Automation_Adobe Instance
        {
			get
            {
				if(instance == null)
                {
					return null;
                }

				return instance;
            }
        }

		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Adobe Automation")]
		static void Init()
		{
			Automation_Adobe window = (Automation_Adobe)EditorWindow.GetWindow(typeof(Automation_Adobe));

			window.arguments = new Data.AutomationArguments();
			window.toggle_processOption = true;
			window.toggle_styleDefinition = true;
			window.toggle_definedTextLayout = false;

			// 캔버스 검색
			Canvas canvas;
			if(GameObject.Find("Canvas").TryGetComponent<Canvas>(out canvas))
			{
				window.m_rootCanvas = canvas;
			}

			instance = window;

			window.Show();
        }

        private void OnDestroy()
        {
			instance = null;
        }

        private void OnGUI()
		{
			GUILayout.Label("Base settings", EditorStyles.boldLabel);
			// 마지막 인자를 true로 두어야 Hierarchy에서 객체를 가져올 수 있다.
			m_rootCanvas = (Canvas)EditorGUILayout.ObjectField("root canvas", m_rootCanvas, typeof(Canvas), true);
			m_target = (GameObject)EditorGUILayout.ObjectField("target object", m_target, typeof(GameObject), true);

			EditorGUILayout.Space(10);

			toggle_processOption = EditorGUILayout.Foldout(toggle_processOption, "실행 옵션");
			ToggleFields_processOption(toggle_processOption);

			EditorGUILayout.Space(10);

			toggle_styleDefinition = EditorGUILayout.Foldout(toggle_styleDefinition, "템플릿별 변수정의");
			ToggleFields_perStyle(arguments.m_style, toggle_styleDefinition);

			EditorGUILayout.Space(10);

			toggle_definedTextLayout = EditorGUILayout.Foldout(toggle_definedTextLayout, "정의된 예약 문자코드들");
			ToggleFields_definedText(toggle_definedTextLayout);

			bool operatable = IsCanOperate(m_rootCanvas, m_target);
			StanbyFields_RunProcess(operatable);
		}

		#region method

		/// <summary>
		/// Toggle : 프로세스 실행 분기옵션
		/// </summary>
		/// <param name="isOn"></param>
		private void ToggleFields_processOption(bool isOn)
		{
			if(isOn)
			{
				GUILayout.Label("옵션 코드", EditorStyles.boldLabel);
				arguments.m_isRemainResourceName = EditorGUILayout.Toggle("is name remaining", arguments.m_isRemainResourceName);
				arguments.m_isVer2 = EditorGUILayout.Toggle("is ver 2", arguments.m_isVer2);
			}
		}

		/// <summary>
		/// Toggle : 템플릿별 변수할당
		/// </summary>
		/// <param name="_style"></param>
		/// <param name="isOn"></param>
		private void ToggleFields_perStyle(Styles _style, bool isOn)
		{
			if(isOn)
			{
				arguments.m_style = (Styles)EditorGUILayout.EnumPopup("Template", arguments.m_style);

				switch (_style)
				{
					case Styles.Default_UI:

						break;

					case Styles.TextmeshPro:
						{
							EditorGUILayout.Space();
							GUILayout.Label("TMPro 템플릿");
							arguments.Tmp_TMPro.m_fontAsset = (TMPro.TMP_FontAsset)EditorGUILayout.ObjectField("TmPro FontAsset", arguments.Tmp_TMPro.m_fontAsset, typeof(TMPro.TMP_FontAsset), true);
							//arguments.m_fontAsset = (TMPro.TMP_FontAsset)EditorGUILayout.ObjectField("TmPro FontAsset", arguments.m_fontAsset, typeof(TMPro.TMP_FontAsset), true);
						}
						break;

					case Styles.ModernUI:
                        {
							EditorGUILayout.Space();
							GUILayout.Label("ModernUI 템플릿");
							arguments.MUI_Templates.progressBar = (Templates.ModernUI.MUI_ProgressBar)EditorGUILayout.ObjectField("ModernUI ProgressBar 1", arguments.MUI_Templates.progressBar, typeof(Templates.ModernUI.MUI_ProgressBar), true);
                        }
						break;
				}
			}
		}

		/// <summary>
		/// 프로세스 실행 예약문자열
		/// </summary>
		/// <param name="isOn"></param>
		private void ToggleFields_definedText(bool isOn)
		{
			if(isOn)
			{
				GUILayout.Label("split string", EditorStyles.boldLabel);
				arguments.m_split = EditorGUILayout.TextField("split string", arguments.m_split);
				arguments.m_splitKeyValue = EditorGUILayout.TextField("split KeyValue", arguments.m_splitKeyValue);

				GUILayout.Label("Labels", EditorStyles.boldLabel);
				arguments.m_labelButton = EditorGUILayout.TextField("button", arguments.m_labelButton);
				arguments.m_labelBoundary = EditorGUILayout.TextField("boundary", arguments.m_labelBoundary);
				arguments.m_labelBackground = EditorGUILayout.TextField("background", arguments.m_labelBackground);
				arguments.m_labelText = EditorGUILayout.TextField("text", arguments.m_labelText);
				arguments.m_labelImage = EditorGUILayout.TextField("image", arguments.m_labelImage);

				GUILayout.Label("tags", EditorStyles.boldLabel);
				arguments.m_tagID = EditorGUILayout.TextField("id", arguments.m_tagID);
				arguments.m_tagFunction = EditorGUILayout.TextField("function", arguments.m_tagFunction);
			}
		}

		/// <summary>
		/// 조건이 맞는 경우 프로세스 시작
		/// </summary>
		/// <param name="_operatable"></param>
		private void StanbyFields_RunProcess(bool _operatable)
		{
			if (_operatable)
			{
				EditorGUILayout.HelpBox("변환 준비 완료", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox("root canvas와 root object를 할당하세요", MessageType.Warning);
			}

			EditorGUI.BeginDisabledGroup(!_operatable);
			if (GUILayout.Button(doButton))
			{
				Debug.Log("----- do Button -----");

				// 자동화 전 복사 및 아이디 부여
				GameObject readyAutomata = PrepareAutomation(m_rootCanvas, m_target);

				// 자동화 시작
				StartAutomation(m_rootCanvas, readyAutomata);


				//if (EditorUtility.DisplayDialog("Title :: 객체 생성", "Message : 객체를 생성하시겠습니까?", "OK", "Cancel"))
				//{
				//	StartAutomation(m_rootCanvas, m_target);
				//}
			}
			EditorGUI.EndDisabledGroup();
		}

		/// <summary>
		/// 임시 창고
		/// </summary>
		private void TestFunctions()
		{
			//hello = EditorGUILayout.Foldout(hello, doButton);
			//if(hello)
			//{
			//}									// 드디어 찾았다 UI접기 시팍것 ㅋㅋㅋ
			//EditorGUILayout.EnumFlagsField(arguments.m_style);	// 마스크
			//hello = EditorGUILayout.BeginToggleGroup("hello", hello);
			//EditorGUILayout.EndToggleGroup();	// 뭐 어쩌란거
			//EditorGUI.BeginChangeCheck();
			//EditorGUI.EndChangeCheck();		// ????
			//GUIContent[] cons = new GUIContent[3] { GUIContent.none, GUIContent.none, GUIContent.none };
			//GUILayout.Toolbar(1, cons);		// 긴거 세개 나옴 ????
			//EditorGUILayout.EnumPopup("hello", arguments.m_style); // ???
			//GUILayout.FlexibleSpace();		// 늘어나는 공간. 근데 렉있음
			//GUILayout.Box(GUIContent.none);	// 인용 박스처럼 뭔가뜸
			//GUILayout.BeginHorizontal();
			//GUILayout.EndHorizontal();
		}

		/// <summary>
		/// 님 실행 가능??
		/// </summary>
		/// <param name="_rCanvas"></param>
		/// <param name="_target"></param>
		/// <returns></returns>
		private bool IsCanOperate(Canvas _rCanvas, GameObject _target)
		{
			bool result = false;

			if (_rCanvas != null && _target != null)
			{
				result = true;
			}

			return result;
		}

		#endregion
	}
}

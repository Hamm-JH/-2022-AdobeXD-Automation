using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation.Templates
{
    using Automation.Templates.ModernUI;
    using UnityEditor;

    public class MUI_Templates
    {
        public MUI_ProgressBar progressBar;

        public MUI_Templates()
        {
            progressBar = Init_GetMUI_progressBar();
        }

        private MUI_ProgressBar Init_GetMUI_progressBar()
        {
            MUI_ProgressBar pBar = null;

            string guid = AssetDatabase.FindAssets(Assets.Definitions.MUI_PROGRESSBAR)[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            pBar = (MUI_ProgressBar)AssetDatabase.LoadAssetAtPath<MUI_ProgressBar>(path);

            return pBar;
        }
    }
}

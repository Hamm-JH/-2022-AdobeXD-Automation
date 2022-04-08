using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public static class Styles
    {
        public static bool IsDefaultText(Definition.Style _style)
        {
            bool result = false;

            switch(_style)
            {
                case Definition.Style.Default_UI:
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsTMProText(Definition.Style _style)
        {
            bool result = false;

            switch(_style)
            {
                case Definition.Style.ModernUI:
                case Definition.Style.TextmeshPro:
                    result = true;
                    break;
            }

            return result;
        }
    }
}

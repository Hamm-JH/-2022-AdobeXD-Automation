using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    public static class Panels 
    {
        public static bool TryGetElement(GameObject _obj, LabelCode _lCode, Automation.Data.AutomationArguments _arguments, out GameObject _target)
        {
            _target = null;

            LabelCode lCode = LabelCodes.GetCode(_obj.name, _arguments);

            if(lCode == _lCode)
            {
                _target = _obj;
                return true;
            }

            return false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    using Utilities;

    public static class Panels 
    {
        /// <summary>
        /// 객체의 라벨이 입력된 라벨과 같은 인스턴스면 인스턴스를 반환한다.
        /// </summary>
        /// <param name="_obj"> 확인하고자 하는 인스턴스 </param>
        /// <param name="_lInputCode"> 입력 확인할 라벨코드 </param>
        /// <param name="_arguments"> arguments 인스턴스 </param>
        /// <param name="_targetOutput"> _obj 인스턴스가 _lInputCode를 가지고 있을 경우 _targetOutput에 객체를 할당한다. </param>
        /// <returns></returns>
        public static bool TryGetElement(GameObject _obj, LabelCode _lInputCode, Automation.Data.AutomationArguments _arguments, out GameObject _targetOutput)
        {
            _targetOutput = null;

            LabelCode lCode = LabelCodes.GetCode(_obj.name, _arguments);

            if(lCode == _lInputCode)
            {
                _targetOutput = _obj;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 생성될 인스턴스의 이름을 지정한다.
        /// </summary>
        /// <param name="_originalName"></param>
        /// <param name="_lCode"></param>
        /// <param name="_id"></param>
        /// <param name="_arguments"></param>
        /// <returns></returns>
        public static string SetInstanceName(string _originalName, LabelCode _lCode, string _id, Automation.Data.AutomationArguments _arguments)
        {
            string result = "";
            string splitCode = _arguments.m_splitKeyValue;

            // lCode에 대응되는 이름
            string lName = LabelCodes.GetLabelString(_lCode, _arguments);

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

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Automation
{
    using Utilities;

    public static class Panels 
    {
        /// <summary>
        /// ��ü�� ���� �Էµ� �󺧰� ���� �ν��Ͻ��� �ν��Ͻ��� ��ȯ�Ѵ�.
        /// </summary>
        /// <param name="_obj"> Ȯ���ϰ��� �ϴ� �ν��Ͻ� </param>
        /// <param name="_lInputCode"> �Է� Ȯ���� ���ڵ� </param>
        /// <param name="_arguments"> arguments �ν��Ͻ� </param>
        /// <param name="_targetOutput"> _obj �ν��Ͻ��� _lInputCode�� ������ ���� ��� _targetOutput�� ��ü�� �Ҵ��Ѵ�. </param>
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
        /// ������ �ν��Ͻ��� �̸��� �����Ѵ�.
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

            // lCode�� �����Ǵ� �̸�
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

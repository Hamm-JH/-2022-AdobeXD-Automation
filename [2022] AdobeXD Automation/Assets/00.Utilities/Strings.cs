using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Strings
    {
        public static bool IsSplitIndexOver(string _value, int _index, string _splitter)
        {
            bool result = false;

            string[] splits = _value.Split(_splitter.ToCharArray());

            if (splits.Length > _index) result = true;

            return result;
        }
    }
}

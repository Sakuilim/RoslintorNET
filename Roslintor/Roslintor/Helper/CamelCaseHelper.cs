using System;
using System.Collections.Generic;
using System.Text;

namespace Roslintor.Helper
{
    public static class CamelCaseHelper
    {
        public static bool IsCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsUpper(name[0]))
            {
                return false;
            }
            if (char.IsUpper(name[0]))
            {
                return false;
            }
            return true;
        }
    }
}

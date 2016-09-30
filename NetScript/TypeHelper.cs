using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetScript
{
    public class TypeHelper
    {
        public static GlobalVariable GenerateVarible(Type type, string name)
        {
            GlobalVariable gv = new GlobalVariable();
            gv.name = name;
            gv.type = type.DeclaringType.ToString();
            return gv;
        }
        public static GlobalVariable GenerateVarible(string type, string name)
        {
            GlobalVariable gv = new GlobalVariable();
            gv.name = name;
            gv.type = type;
            return gv;
        }
        public static GlobalVariable GenerateVarible(Type type)
        {
            GlobalVariable gv = new GlobalVariable();
            string[] s = type.DeclaringType.ToString().Split('.');
            gv.name = s[s.Length - 1];
            gv.type = type.DeclaringType.ToString();
            return gv;
        }
    }
}

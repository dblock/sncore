using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SnCore.Tools
{
    public class PersistentlyHashable
    {
        public static int GetHashCode(object o)
        {
            StringBuilder hash = new StringBuilder();
            FieldInfo[] fields = o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object fieldvalue = field.GetValue(o);
                hash.AppendLine(fieldvalue == null ? string.Empty : fieldvalue.ToString());
            }

            return hash.ToString().GetHashCode();
        }
    }
}

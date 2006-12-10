using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Tools
{
    public class ExceptionCollection : List<Exception>
    {
        public ExceptionCollection()
        {

        }

        public string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception ex in this)
                {
                    if (sb.Length > 0) sb.AppendLine("<br>");
                    sb.AppendLine(ex.Message);
                }

                return sb.ToString();
            }
        }

        public Exception CombinedException
        {
            get
            {
                return new Exception(Message, this.Count > 0 ? this[0] : null);
            }
        }

        public void Throw()
        {
            if (Count > 0)
            {
                throw CombinedException;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SnCore.Services
{
    public enum TransitSortDirection
    {
        Ascending,
        Descending
    }
    
    [Serializable()]
    public class TransitService
    {
        private int mId;

        public TransitService()
        {
            Id = 0;
        }

        public TransitService(int id)
        {
            Id = id;
        }

        public int Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        public static string Encode(string value)
        {
            if (value == null)
                return string.Empty;
            return HttpUtility.HtmlEncode(value).Replace("\r\n", "<br/>");
        }

        public static string Decode(string value)
        {
            if (value == null)
                return string.Empty;
            return HttpUtility.HtmlDecode(value);
        }
    }
}

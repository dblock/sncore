using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.WebServices
{
    public class ServiceQueryOptions
    {
        public int PageSize = -1;
        public int PageNumber = 0;

        public int FirstResult
        {
            get
            {
                return PageSize * PageNumber;
            }
        }

        public ServiceQueryOptions()
        {
        }

        public ServiceQueryOptions(int pagesize, int pagenumber)
        {
            PageSize = pagesize;
            PageNumber = pagenumber;
        }

        public override int GetHashCode()
        {
            string hash = string.Format("{0}:{1}", PageSize, PageNumber);
            return hash.GetHashCode();
        }

        public string GetSqlQueryTop()
        {
            return string.Format(" TOP {0} ", FirstResult + PageSize);
        }
    };
}

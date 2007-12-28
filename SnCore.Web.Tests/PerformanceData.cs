using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Web.Tests
{
    public class PerformanceData : IComparable<PerformanceData>
    {
        private int mQueries = 0;
        private long mTotalDuration = 0;
        private string mUri = string.Empty;

        public int Queries
        {
            get
            {
                return mQueries;
            }
            set
            {
                mQueries = value;
            }
        }

        public long TotalDuration
        {
            get
            {
                return mTotalDuration;
            }
            set
            {
                mTotalDuration = value;
            }
        }

        public string Uri
        {
            get
            {
                return mUri;
            }
            set
            {
                mUri = value;
            }
        }

        public PerformanceData(string uri)
        {
            mUri = uri;
        }

        public int CompareTo(PerformanceData obj)
        {
            int result = mQueries.CompareTo(obj.mQueries);
            if (result == 0) result = mTotalDuration.CompareTo(obj.mTotalDuration);
            return result;
        }
    }
}

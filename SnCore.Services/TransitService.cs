using System;
using System.Collections;
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
    public class TransitArrayElementService<T> : TransitService
        where T : IDbObject
    {
        protected int mIndex = -1;
        protected int mNextIndex = -1;
        protected int mPrevIndex = -1;
        protected int mNextId = -1;
        protected int mPrevId = -1;
        protected int mCount = 0;

        public int Count
        {
            get
            {
                return mCount;
            }
            set
            {
                mCount = value;
            }
        }

        public int NextIndex
        {
            get
            {
                return mNextIndex;
            }
            set
            {
                mNextIndex = value;
            }
        }

        public int PrevIndex
        {
            get
            {
                return mPrevIndex;
            }
            set
            {
                mPrevIndex = value;
            }
        }

        public int Index
        {
            get
            {
                return mIndex;
            }
            set
            {
                mIndex = value;
            }
        }

        public int NextId
        {
            get
            {
                return mNextId;
            }
            set
            {
                mNextId = value;
            }
        }

        public int PrevId
        {
            get
            {
                return mPrevId;
            }
            set
            {
                mPrevId = value;
            }
        }

        public TransitArrayElementService()
        {

        }

        public TransitArrayElementService(int id, T element, IList<T> collection)
            : base(id)
        {
            mIndex = collection.IndexOf(element);
            mCount = collection.Count;
            
            if (mIndex > 0)
            {
                mPrevIndex = mIndex - 1;
                mPrevId = collection[mPrevIndex].Id;
            }

            if (mIndex + 1 < collection.Count)
            {
                mNextIndex = mIndex + 1;
                mNextId = collection[mNextIndex].Id;
            }
        }
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

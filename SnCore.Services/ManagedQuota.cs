using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SnCore.Services
{
    public class ManagedQuota
    {
        public const int DefaultQuotaUpperLimit = 100;

        private Nullable<int> mUpperLimit = new Nullable<int>();

        public ManagedQuota()
        {

        }

        public ManagedQuota(int upperlimit)
        {
            mUpperLimit = upperlimit;
        }

        public static ManagedQuota GetDefaultEnabledQuota()
        {
            return new ManagedQuota(DefaultQuotaUpperLimit);
        }

        public static ManagedQuota GetDefaultNoQuota()
        {
            return new ManagedQuota();
        }

        public void Check<TransitType, ExceptionType>(IList<TransitType> collection) 
            where ExceptionType : System.Exception, new()
        {
            if (collection == null || collection.Count == 0)
                return;

            if (!mUpperLimit.HasValue)
                return;

            if (collection.Count >= mUpperLimit.Value)
                throw new ExceptionType();
        }

        public void Check<TransitType, ExceptionType>(int count) 
            where ExceptionType : System.Exception, new()
        {
            if (count == 0)
                return;

            if (!mUpperLimit.HasValue)
                return;

            if (count >= mUpperLimit.Value)
                throw new ExceptionType();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Web.Tests
{
    public class PerformanceDataCollection : List<PerformanceData>
    {
        private int mMax = 0;

        public PerformanceDataCollection()
            : this(100)
        {

        }

        public PerformanceDataCollection(int max)
            : base()
        {
            mMax = max;
        }

        public void DumpPigs()
        {
            Console.WriteLine("Pigs:");
            foreach (PerformanceData pd in this)
            {
                Console.WriteLine("{0}: {1} queries in {2} ticks", pd.Uri, pd.Queries, pd.TotalDuration);
            }
        }

        public new void Add(PerformanceData perf)
        {
            // add perf data
            bool fAdded = false;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].CompareTo(perf) < 0)
                {
                    fAdded = true;
                    Insert(i, perf);
                    break;
                }
            }

            if (!fAdded) base.Add(perf);
            if (Count > mMax) RemoveAt(mMax);
        }
    }
}

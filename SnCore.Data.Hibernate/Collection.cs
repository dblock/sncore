using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Data.Hibernate
{
    public abstract class Collection<T>
    {
        public static IList<T> ApplyServiceOptions(int first, int count, IList<T> collection)
        {
            if (count == 0) count = collection.Count;
            List<T> result = new List<T>(count);
 
            for (int i = 0; i < count; i++)
            {
                if (first + i >= collection.Count)
                    break;

                result.Add(collection[first + i]);
            }

            return result;
        }
    }
}

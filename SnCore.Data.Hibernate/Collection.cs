using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Data.Hibernate
{
    public abstract class Collection<T>
    {
        public static List<T> ApplyServiceOptions(int first, int count, IList<T> collection)
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

        public static IList<T> GetSafeCollection(IList<T> collection)
        {
            return (collection == null) ? new List<T>() : collection;
        }

        public static void Remove(IList<T> collection, T el)
        {
            if (collection != null)
            {
                collection.Remove(el);
            }
        }
    }
}

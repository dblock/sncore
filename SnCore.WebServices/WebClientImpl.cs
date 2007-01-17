using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace SnCore.WebServices
{
    public abstract class WebClientImpl<TransitType>
    {
        #region Cache Keys

        private static string GetCacheKey(string method)
        {
            return GetCacheKey(method, null);
        }

        private static string GetCacheKey(string method, object[] args)
        {
            StringBuilder key = new StringBuilder(method);

            if (args != null)
            {
                foreach (object arg in args)
                {
                    key.Append(":");
                    key.Append(arg == null ? string.Empty : arg.GetHashCode().ToString());
                }
            }

            return key.ToString();
        }

        #endregion

        #region Collection

        #region ticket + ServiceQueryOptions

        public delegate IList<TransitType> GetCollectionDelegate(string ticket, ServiceQueryOptions options);

        public static IList<TransitType> GetCollection(
            string ticket, ServiceQueryOptions options, GetCollectionDelegate functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, options);
            object[] args = { options };
            string key = GetCacheKey(functor.Method.Name, args);
            IList<TransitType> collection = (IList<TransitType>) cache.Get(key);
            if (collection == null)
            {
                collection = functor(ticket, options);
                if (collection != null)
                {
                    cache.Insert(key, collection, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + ServiceQueryOptions

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1>(
            string ticket, TypeArg1 arg1, ServiceQueryOptions options);

        public static IList<TransitType> GetCollection<TypeArg1>(
            string ticket, TypeArg1 arg1, ServiceQueryOptions options, 
            GetCollectionDelegate<TypeArg1> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, options);
            object[] args = { arg1, options };
            string key = GetCacheKey(functor.Method.Name, args);
            IList<TransitType> collection = (IList<TransitType>)cache.Get(key);
            if (collection == null)
            {
                collection = functor(ticket, arg1, options);
                if (collection != null)
                {
                    cache.Insert(key, collection, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + arg2 + ServiceQueryOptions

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, ServiceQueryOptions options);

        public static IList<TransitType> GetCollection<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, ServiceQueryOptions options, 
            GetCollectionDelegate<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2, options);
            object[] args = { arg1, arg2, options };
            string key = GetCacheKey(functor.Method.Name, args);
            IList<TransitType> collection = (IList<TransitType>)cache.Get(key);
            if (collection == null)
            {
                collection = functor(ticket, arg1, arg2, options);
                if (collection != null)
                {
                    cache.Insert(key, collection, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3 + ServiceQueryOptions

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3, ServiceQueryOptions options);

        public static IList<TransitType> GetCollection<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3, ServiceQueryOptions options,
            GetCollectionDelegate<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3, options);
            object[] args = { arg1, arg2, arg3, options };
            string key = GetCacheKey(functor.Method.Name, args);
            IList<TransitType> collection = (IList<TransitType>)cache.Get(key);
            if (collection == null)
            {
                collection = functor(ticket, arg1, arg2, arg3, options);
                if (collection != null)
                {
                    cache.Insert(key, collection, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return collection;
        }

        #endregion

        #endregion

        #region Instance

        #region ticket

        public delegate TransitType GetItemDelegate(string ticket);

        public static TransitType GetInstance(
            string ticket, GetItemDelegate functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket);
            string key = GetCacheKey(functor.Method.Name);
            TransitType instance = (TransitType)cache.Get(key);
            if (instance == null)
            {
                instance = functor(ticket);
                if (instance != null)
                {
                    cache.Insert(key, instance, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return instance;
        }

        #endregion

        #region ticket + arg1

        public delegate TransitType GetItemDelegate<TypeArg1>(
            string ticket, TypeArg1 arg1);

        public static TransitType GetInstance<TypeArg1>(
            string ticket, TypeArg1 arg1, GetItemDelegate<TypeArg1> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1);
            object[] args = { arg1 };
            string key = GetCacheKey(functor.Method.Name, args);
            TransitType instance = (TransitType) cache.Get(key);
            if (instance == null)
            {
                instance = functor(ticket, arg1);
                if (instance != null)
                {
                    cache.Insert(key, instance, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return instance;
        }

        #endregion

        #region ticket + arg1 + arg2

        public delegate TransitType GetItemDelegate<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2);

        public static TransitType GetInstance<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, 
            GetItemDelegate<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2);
            object[] args = { arg1, arg2 };
            string key = GetCacheKey(functor.Method.Name, args);
            TransitType instance = (TransitType)cache.Get(key);
            if (instance == null)
            {
                instance = functor(ticket, arg1, arg2);
                if (instance != null)
                {
                    cache.Insert(key, instance, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return instance;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3

        public delegate TransitType GetItemDelegate<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3);

        public static TransitType GetInstance<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3, 
            GetItemDelegate<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3);
            object[] args = { arg1, arg2, arg3 };
            string key = GetCacheKey(functor.Method.Name, args);
            TransitType instance = (TransitType)cache.Get(key);
            if (instance == null)
            {
                instance = functor(ticket, arg1, arg2, arg3);
                if (instance != null)
                {
                    cache.Insert(key, instance, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return instance;
        }

        #endregion

        #endregion

        #region Count

        #region ticket

        public delegate int GetItemDelegateCount(string ticket);

        public static int GetCount(
            string ticket, GetItemDelegateCount functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket);
            string key = GetCacheKey(functor.Method.Name);
            object count = cache.Get(key);
            if (count == null)
            {
                count = functor(ticket);
                if (count != null)
                {
                    cache.Insert(key, count, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return (int) count;
        }

        #endregion

        #region ticket + arg1

        public delegate int GetItemDelegateCount<TypeArg1>(
            string ticket, TypeArg1 arg1);

        public static int GetCount<TypeArg1>(
            string ticket, TypeArg1 arg1, 
            GetItemDelegateCount<TypeArg1> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1);
            object[] args = { arg1 };
            string key = GetCacheKey(functor.Method.Name, args);
            object count = cache.Get(key);
            if (count == null)
            {
                count = functor(ticket, arg1);
                if (count != null)
                {
                    cache.Insert(key, count, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return (int)count;
        }

        #endregion

        #region ticket + arg1 + arg2

        public delegate int GetItemDelegateCount<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2);

        public static int GetCount<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, 
            GetItemDelegateCount<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2);
            object[] args = { arg1, arg2 };
            string key = GetCacheKey(functor.Method.Name, args);
            object count = cache.Get(key);
            if (count == null)
            {
                count = functor(ticket, arg1, arg2);
                if (count != null)
                {
                    cache.Insert(key, count, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return (int)count;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3

        public delegate int GetItemDelegateCount<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3);

        public static int GetCount<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3,
            GetItemDelegateCount<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3);
            object[] args = { arg1, arg2, arg3 };
            string key = GetCacheKey(functor.Method.Name, args);
            object count = cache.Get(key);
            if (count == null)
            {
                count = functor(ticket, arg1, arg2, arg3);
                if (count != null)
                {
                    cache.Insert(key, count, null, Cache.NoAbsoluteExpiration, ts);
                }
            }
            return (int)count;
        }

        #endregion

        #endregion
    }
}

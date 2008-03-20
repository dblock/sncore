using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Diagnostics;

namespace SnCore.WebServices
{
    public class TypeCacheDependency<TransitType> : CacheDependency
    {
        public TypeCacheDependency()
            : base(null, new string[] { GetTypeCacheKey() }, null)
        {

        }

        public static string GetTypeCacheKey()
        {
            return string.Format("type:{0}", typeof(TransitType).Name);
        }
    }

    public abstract class WebClientImpl<TransitType> : WebClientImpl2<TransitType, ServiceQueryOptions>
    {
    }

    public abstract class WebClientImpl2<TransitType, ServiceQueryOptionsType>
    {
        #region Cache Keys

        private static object s_mNullMarker = new object();

        private static void Insert(Cache cache, string key, object instance, TimeSpan ts)
        {
            cache.Insert(key, instance == null ? s_mNullMarker : instance,
                GetTransitTypeCacheDependency(cache), Cache.NoAbsoluteExpiration, ts);
        }

        #region TryGet

        private static bool TryGet<ObjectType>(Cache cache, string key, out ObjectType instance)
        {
            object result = cache.Get(key);

            CacheHitOrMiss(key, result);

            if (result == null)
            {
                instance = default(ObjectType);
                return false;
            }

            if (result == s_mNullMarker)
            {
                instance = default(ObjectType);
                return true;
            }

            instance = (ObjectType)result;
            return true;
        }

        #endregion

        private static void CacheHitOrMiss(string key, object value)
        {
#if DEBUG
            if (value == null)
            {
                Debug.WriteLine(string.Format("Cache miss: {0}", key));
            }
            else
            {
                Debug.WriteLine(string.Format("Cache hit: {0}", key));
            }
#endif
        }

        private static TypeCacheDependency<TransitType> GetTransitTypeCacheDependency(Cache cache)
        {
            string key = TypeCacheDependency<TransitType>.GetTypeCacheKey();
            if (cache[key] == null)
            {
                cache[key] = DateTime.UtcNow;
#if DEBUG
                Debug.WriteLine(string.Format("Added cache dependency key: {0}", key));
#endif
            }
            return new TypeCacheDependency<TransitType>();
        }

        private static string GetCacheKey(string method, string ticket)
        {
            return GetCacheKey(method, null, ticket);
        }

        private static string GetCacheKey(string method, object[] args, string ticket)
        {
            StringBuilder key = new StringBuilder(method);

            if (!string.IsNullOrEmpty(ticket))
            {
                key.Append(":");
                key.Append(ticket.GetHashCode().ToString());
            }

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

        public static void Invalidate(Cache cache)
        {
            string key = TypeCacheDependency<TransitType>.GetTypeCacheKey();
            cache[key] = DateTime.UtcNow;
#if DEBUG
            Debug.WriteLine(string.Format("Invalidated cache dependency: {0}", key));
#endif
        }

        #endregion

        #region Collection

        #region no parameters

        public delegate IList<TransitType> GetCollectionDelegateNoArgs();

        public static IList<TransitType> GetCollection(
            GetCollectionDelegateNoArgs functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor();
            string key = GetCacheKey(functor.Method.Name, cacheticket);
            IList<TransitType> collection = null;
            if (!TryGet(cache, key, out collection))
            {
                collection = functor();
                Insert(cache, key, collection, ts);
            }
            return collection;
        }

        #endregion

        #region ticket + ServiceQueryOptionsType

        public delegate IList<TransitType> GetCollectionDelegate(string ticket, ServiceQueryOptionsType options);

        public static IList<TransitType> GetCollection(
            string ticket, ServiceQueryOptionsType options, GetCollectionDelegate functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, options);
            object[] args = { options };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            IList<TransitType> collection = null;
            if (! TryGet(cache, key, out collection))
            {
                collection = functor(ticket, options);
                Insert(cache, key, collection, ts);
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + ServiceQueryOptionsType

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1>(
            string ticket, TypeArg1 arg1, ServiceQueryOptionsType options);

        public static IList<TransitType> GetCollection<TypeArg1>(
            string ticket, TypeArg1 arg1, ServiceQueryOptionsType options,
            GetCollectionDelegate<TypeArg1> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, options);
            object[] args = { arg1, options };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            IList<TransitType> collection = null;
            if (! TryGet(cache, key, out collection))
            {
                collection = functor(ticket, arg1, options);
                Insert(cache, key, collection, ts);
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + arg2 + ServiceQueryOptionsType

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, ServiceQueryOptionsType options);

        public static IList<TransitType> GetCollection<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, ServiceQueryOptionsType options,
            GetCollectionDelegate<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2, options);
            object[] args = { arg1, arg2, options };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            IList<TransitType> collection = null;
            if (! TryGet(cache, key, out collection))
            {
                collection = functor(ticket, arg1, arg2, options);
                Insert(cache, key, collection, ts);
            }
            return collection;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3 + ServiceQueryOptionsType

        public delegate IList<TransitType> GetCollectionDelegate<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3, ServiceQueryOptionsType options);

        public static IList<TransitType> GetCollection<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3, ServiceQueryOptionsType options,
            GetCollectionDelegate<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3, options);
            object[] args = { arg1, arg2, arg3, options };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            IList<TransitType> collection = null;
            if (!TryGet(cache, key, out collection))
            {
                collection = functor(ticket, arg1, arg2, arg3, options);
                Insert(cache, key, collection, ts);
            }
            return collection;
        }

        #endregion

        #endregion

        #region Instance

        #region ticket

        public delegate TransitType GetItemDelegate(string ticket);

        public static TransitType GetInstance(
            string ticket, GetItemDelegate functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket);
            string key = GetCacheKey(functor.Method.Name, cacheticket);
            TransitType instance = default(TransitType);
            if (!TryGet(cache, key, out instance))
            {
                instance = functor(ticket);
                Insert(cache, key, instance, ts);
            }
            return instance;
        }

        #endregion

        #region ticket + arg1

        public delegate TransitType GetItemDelegate<TypeArg1>(
            string ticket, TypeArg1 arg1);

        public static TransitType GetInstance<TypeArg1>(
            string ticket, TypeArg1 arg1, GetItemDelegate<TypeArg1> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1);
            object[] args = { arg1 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            TransitType instance = default(TransitType);
            if (!TryGet(cache, key, out instance))
            {
                instance = functor(ticket, arg1);
                Insert(cache, key, instance, ts);
            }
            return instance;
        }

        #endregion

        #region ticket + arg1 + arg2

        public delegate TransitType GetItemDelegate<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2);

        public static TransitType GetInstance<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2,
            GetItemDelegate<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2);
            object[] args = { arg1, arg2 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            TransitType instance = default(TransitType);
            if (!TryGet(cache, key, out instance))
            {
                instance = functor(ticket, arg1, arg2);
                Insert(cache, key, instance, ts);
            }
            return instance;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3

        public delegate TransitType GetItemDelegate<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3);

        public static TransitType GetInstance<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3,
            GetItemDelegate<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3);
            object[] args = { arg1, arg2, arg3 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            TransitType instance = default(TransitType);
            if (!TryGet(cache, key, out instance))
            {
                instance = functor(ticket, arg1, arg2, arg3);
                Insert(cache, key, instance, ts);
            }
            return instance;
        }

        #endregion

        #endregion

        #region CreateOrUpdate

        public delegate int CreateOrUpdateItemDelegate(string ticket, TransitType t_instance);

        public static int CreateOrUpdate(
            string ticket, TransitType t_instance, CreateOrUpdateItemDelegate functor, Cache cache)
        {
            int id = functor(ticket, t_instance);
            if (cache != null) Invalidate(cache);
            return id;
        }

        public delegate int CreateOrUpdateItemDelegate<ArgType1>(string ticket, TransitType t_instance, ArgType1 arg1);

        public static int CreateOrUpdate<ArgType1>(
            string ticket, TransitType t_instance, ArgType1 arg1, CreateOrUpdateItemDelegate<ArgType1> functor, Cache cache)
        {
            int id = functor(ticket, t_instance, arg1);
            if (cache != null) Invalidate(cache);
            return id;
        }

        #endregion

        #region Delete

        public delegate void DeleteItemDelegate(string ticket, int id);

        public static void Delete(
            string ticket, int id, DeleteItemDelegate functor, Cache cache)
        {
            functor(ticket, id);
            if (cache != null) Invalidate(cache);
        }

        #endregion

        #region Count

        #region ticket

        public delegate int GetItemDelegateCount(string ticket);

        public static int GetCount(
            string ticket, GetItemDelegateCount functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket);
            string key = GetCacheKey(functor.Method.Name, cacheticket);
            int count = 0;
            if (!TryGet(cache, key, out count))
            {
                count = functor(ticket);
                Insert(cache, key, count, ts);
            }
            return (int)count;
        }

        #endregion

        #region ticket + arg1

        public delegate int GetItemDelegateCount<TypeArg1>(
            string ticket, TypeArg1 arg1);

        public static int GetCount<TypeArg1>(
            string ticket, TypeArg1 arg1,
            GetItemDelegateCount<TypeArg1> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1);
            object[] args = { arg1 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            int count = 0;
            if (!TryGet(cache, key, out count))
            {
                count = functor(ticket, arg1);
                Insert(cache, key, count, ts);
            }
            return (int)count;
        }

        #endregion

        #region ticket + arg1 + arg2

        public delegate int GetItemDelegateCount<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2);

        public static int GetCount<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2,
            GetItemDelegateCount<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2);
            object[] args = { arg1, arg2 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            int count = 0;
            if (!TryGet(cache, key, out count))
            {
                count = functor(ticket, arg1, arg2);
                Insert(cache, key, count, ts);
            }
            return (int)count;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3

        public delegate int GetItemDelegateCount<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3);

        public static int GetCount<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3,
            GetItemDelegateCount<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3);
            object[] args = { arg1, arg2, arg3 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            int count = 0;
            if (!TryGet(cache, key, out count))
            {
                count = functor(ticket, arg1, arg2, arg3);
                Insert(cache, key, count, ts);
            }
            return (int)count;
        }

        #endregion

        #endregion

        #region Bool

        #region ticket

        public delegate bool GetItemDelegateBool(string ticket);

        public static bool GetBool(
            string ticket, GetItemDelegateBool functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket);
            string key = GetCacheKey(functor.Method.Name, cacheticket);
            bool flag = false;
            if (!TryGet(cache, key, out flag))
            {
                flag = functor(ticket);
                Insert(cache, key, flag, ts);
            }
            return (bool)flag;
        }

        #endregion

        #region ticket + arg1

        public delegate bool GetItemDelegateBool<TypeArg1>(
            string ticket, TypeArg1 arg1);

        public static bool GetBool<TypeArg1>(
            string ticket, TypeArg1 arg1,
            GetItemDelegateBool<TypeArg1> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1);
            object[] args = { arg1 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            bool flag = false;
            if (!TryGet(cache, key, out flag))
            {
                flag = functor(ticket, arg1);
                Insert(cache, key, flag, ts);
            }
            return (bool)flag;
        }

        #endregion

        #region ticket + arg1 + arg2

        public delegate bool GetItemDelegateBool<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2);

        public static bool GetBool<TypeArg1, TypeArg2>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2,
            GetItemDelegateBool<TypeArg1, TypeArg2> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2);
            object[] args = { arg1, arg2 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            bool flag = false;
            if (!TryGet(cache, key, out flag))
            {
                flag = functor(ticket, arg1, arg2);
                Insert(cache, key, flag, ts);
            }
            return (bool)flag;
        }

        #endregion

        #region ticket + arg1 + arg2 + arg3

        public delegate bool GetItemDelegateBool<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3);

        public static bool GetBool<TypeArg1, TypeArg2, TypeArg3>(
            string ticket, TypeArg1 arg1, TypeArg2 arg2, TypeArg3 arg3,
            GetItemDelegateBool<TypeArg1, TypeArg2, TypeArg3> functor, Cache cache, TimeSpan ts,
            string cacheticket)
        {
            if (cache == null) return functor(ticket, arg1, arg2, arg3);
            object[] args = { arg1, arg2, arg3 };
            string key = GetCacheKey(functor.Method.Name, args, cacheticket);
            bool flag = false;
            if (!TryGet(cache, key, out flag))
            {
                flag = functor(ticket, arg1, arg2, arg3);
                Insert(cache, key, flag, ts);
            }
            return (bool)flag;
        }

        #endregion

        #endregion
    }
}

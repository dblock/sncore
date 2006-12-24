using System;
using NHibernate;
using System.Collections.Generic;
using System.Web.Hosting;

namespace SnCore.Services
{
    public class ManagedServiceImpl
    {
        private ISession mSession = null;

        public ManagedServiceImpl(ISession session)
        {
            Session = session;
        }

        public ISession Session
        {
            get
            {
                return mSession;
            }
            set
            {
                mSession = value;
            }
        }       
    }

    public class ManagedService<T> : ManagedServiceImpl
        where T : IDbObject
    {
        public ManagedService(ISession session)
            : base(session)
        {

        }

        public static int GetRandomElementId(IList<T> collection)
        {
            if (collection == null || collection.Count == 0)
                return 0;

            return collection[new Random().Next() % collection.Count].Id;
        }
    }
}

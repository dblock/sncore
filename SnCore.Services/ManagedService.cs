using System;
using NHibernate;
using System.Collections;
using System.Web.Hosting;

namespace SnCore.Services
{
    public class ManagedService
    {
        private ISession mSession = null;

        public ManagedService(ISession session)
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

        public static int GetRandomElementId(IList collection)
        {
            if (collection == null)
                return 0;

            if (collection.Count == 0)
                return 0;

            object resultobject = collection[new Random().Next() % collection.Count];
            return (int)resultobject.GetType().GetProperty("Id").GetValue(resultobject, null);
        }

    }
}

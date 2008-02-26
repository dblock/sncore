using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using System.Web.Hosting;
using SnCore.Data.Hibernate;
using NHibernate.Expression;

namespace SnCore.Services
{
    public abstract class ManagedPictureServiceImpl<DatabaseType>
        where DatabaseType : IDbPictureObject, new()
    {
        public static void Delete(ISession session, DatabaseType instance, IList<DatabaseType> collection)
        {
            // renumber the order of Pictures
            IList<DatabaseType> safecollection = Collection<DatabaseType>.GetSafeCollection(collection);
            foreach (DatabaseType p in safecollection)
            {
                if (p.Position >= instance.Position)
                {
                    p.Position--;
                    session.Save(p);
                }
            }
        }

        public static void Renumber(ISession session, IList<DatabaseType> collection)
        {
            List<DatabaseType> safecollection_copy = new List<DatabaseType>(collection);
            safecollection_copy.Sort(delegate(DatabaseType l, DatabaseType r) { return r.Created.CompareTo(l.Created); });
            int current = 0;
            foreach (DatabaseType p in safecollection_copy)
            {
                p.Position = ++current;
                session.Save(p);
            }
        }

        public static void Move(ISession session, DatabaseType instance, IList<DatabaseType> collection, int disp)
        {
            IList<DatabaseType> safecollection = Collection<DatabaseType>.GetSafeCollection(collection);

            // if the collection is not numbered, number by created, then adjust
            if (instance.Position == 0)
            {
                Renumber(session, collection);
            }

            int newPosition = instance.Position + disp;

            if (newPosition < 1) newPosition = 1;
            else if (newPosition > safecollection.Count) newPosition = safecollection.Count;

            foreach (DatabaseType p in safecollection)
            {
                if (p.Position == instance.Position)
                {
                    // this item
                }
                else if (p.Position < instance.Position && p.Position >= newPosition)
                {
                    // item was before me but switched sides
                    p.Position++;
                }
                else if (p.Position > instance.Position && p.Position <= newPosition)
                {
                    // item was after me, but switched sides
                    p.Position--;
                }

                session.Save(p);
            }

            instance.Position = newPosition;
            session.Save(instance);
        }

        public static void Save(ISession session, DatabaseType instance, IList<DatabaseType> collection)
        {
            if (instance.Id == 0)
            {
                IList<DatabaseType> safecollection = Collection<DatabaseType>.GetSafeCollection(collection);
                if (safecollection.Count > 0 && safecollection[0].Position == 0)
                {
                    Renumber(session, safecollection);
                }

                instance.Position = safecollection.Count + 1;
            }

            session.Save(instance);
        }
    }
}

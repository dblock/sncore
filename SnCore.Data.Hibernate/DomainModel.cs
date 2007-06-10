using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Persister.Entity;

namespace SnCore.Data.Hibernate
{
    public class DomainModel
    {
        private Dictionary<string, DomainClass> m_Classes = new Dictionary<string, DomainClass>();

        public DomainModel()
        {

        }

        public DomainModel(ISession session)
        {
            GetClasses(session);
        }

        public Dictionary<string, DomainClass>.ValueCollection Classes
        {
            get
            {
                return m_Classes.Values;
            }
        }

        public DomainClass this[string name]
        {
            get
            {
                return m_Classes[name];
            }
        }

        private void GetClasses(ISession session)
        {
            IDictionary metadata = session.SessionFactory.GetAllClassMetadata();
            foreach (DictionaryEntry entry in metadata)
            {
                SingleTableEntityPersister persister = (SingleTableEntityPersister) entry.Value;
                m_Classes.Add(persister.ClassName, new DomainClass(session, persister.TableName));
            }
        }
    }
}

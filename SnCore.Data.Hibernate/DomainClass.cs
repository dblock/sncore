using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Persister.Entity;

namespace SnCore.Data.Hibernate
{
    public class DomainClass
    {
        private string m_Name;
        Dictionary<string, sp_column> m_Columns = new Dictionary<string, sp_column>();

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public Dictionary<string, sp_column>.ValueCollection Columns
        {
            get
            {
                return m_Columns.Values;
            }
        }

        public sp_column this[string name]
        {
            get
            {
                return m_Columns[name];
            }
        }

        public DomainClass(string name, sp_column[] columns)
        {
            m_Name = name;
            foreach (sp_column col in columns)
            {
                m_Columns.Add(col.COLUMN_NAME, col);
            }
        }

        public DomainClass(ISession session, string name)
        {
            m_Name = GetTableName(name);
            GetColumns(session);
        }

        public DomainClass()
        {

        }

        private void GetColumns(ISession session)
        {
            IQuery query = session.GetNamedQuery("sp_columns");
            query.SetString("table_name", m_Name);

            IList<sp_column> columns = query
                .SetResultTransformer(Transformers.AliasToBean(typeof(sp_column)))
                .List<sp_column>();

            foreach (sp_column column in columns)
            {
                m_Columns.Add(column.COLUMN_NAME, column);
            }
        }

        public static string GetTableName(string fullname)
        {
            string[] table_name_portions = fullname.Split(".".ToCharArray());
            return table_name_portions[table_name_portions.Length - 1];
        }
    }
}
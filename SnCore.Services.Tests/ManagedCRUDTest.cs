using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    public abstract class ManagedCRUDTest<DatabaseType, TransitType, ManagedType> : ManagedServiceTest, IDisposable 
        where ManagedType: IManagedService, new()
        where TransitType: ITransitService, new()
        where DatabaseType: IDbObject, new()
    {
        protected ManagedType _instance = default(ManagedType);

        public virtual ManagedType Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ManagedType();
                    TransitType t_instance = GetTransitInstance();
                    _instance.Session = Session;
                    _instance.CreateOrUpdateDbObject(t_instance, GetSecurityContext());
                    Console.WriteLine("Created {0}: {1}", typeof(DatabaseType).Name, _instance.Id);
                }

                return _instance;
            }
        }

        public ManagedCRUDTest()
        {

        }

        public void Dispose()
        {
            if (_instance != null)
            {
                Console.WriteLine("Deleting {0}: {1}", typeof(DatabaseType).Name, _instance.Id);
                _instance.Delete(GetSecurityContext());
                Session.Flush();
            }
        }

        public override void TearDown()
        {
            if (_instance != null)
            {
                Console.WriteLine("Deleting {0}: {1}", typeof(DatabaseType).Name, _instance.Id);
                _instance.Delete(GetSecurityContext());
                _instance = default(ManagedType);
            }

            Session.Flush();
            base.TearDown();
        }

        public abstract TransitType GetTransitInstance();
        
        protected virtual ManagedSecurityContext GetSecurityContext()
        {
            return AdminSecurityContext;
        }

        [Test]
        public virtual void TestCreateAndDelete()
        {
            ManagedType m_instance = new ManagedType();
            try
            {
                TransitType t_instance = GetTransitInstance();
                m_instance.Session = Session;
                m_instance.CreateOrUpdateDbObject(t_instance, GetSecurityContext());
            }
            finally
            {
                m_instance.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }

        [Test]
        public virtual void TestUpdateAndRetrieve()
        {
            ManagedType m_instance = new ManagedType();
            try
            {
                TransitType t_instance = GetTransitInstance();
                m_instance.Session = Session;
                int id = m_instance.CreateOrUpdateDbObject(t_instance, GetSecurityContext());
                // reload the object
                ManagedType m_instance_copy = new ManagedType();
                m_instance_copy.LoadInstance(Session, id);
                Assert.AreEqual(m_instance_copy.DbObjectInstance, m_instance.DbObjectInstance, "Reloaded instances don't match.");
                // resave the object
                TransitType t_instance_copy = GetTransitInstance();
                t_instance_copy.Id = id;
                ManagedType m_instance_resaved = new ManagedType();
                m_instance_resaved.Session = Session;
                int id_copy = m_instance_resaved.CreateOrUpdateDbObject(t_instance_copy, GetSecurityContext());
                Assert.AreEqual(id, id_copy);                
            }
            finally
            {
                m_instance.Delete(AdminSecurityContext);
                Session.Flush();
            }
        }

    }
}

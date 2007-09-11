using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventTypeTest : ManagedCRUDTest<AccountEventType, TransitAccountEventType, ManagedAccountEventType>
    {
        public ManagedAccountEventTypeTest()
        {

        }

        public override TransitAccountEventType GetTransitInstance()
        {
            TransitAccountEventType t_instance = new TransitAccountEventType();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public void DefaultTypeTest()
        {
            TransitAccountEventType t_type1 = new TransitAccountEventType();
            t_type1.DefaultType = true;
            t_type1.Name = GetNewString();
            ManagedAccountEventType m_type1 = new ManagedAccountEventType(Session);
            t_type1.Id = m_type1.CreateOrUpdate(t_type1, AdminSecurityContext);
            ManagedAccountEventType m_type1_copy1 = new ManagedAccountEventType(Session, t_type1.Id);
            Assert.IsTrue(m_type1_copy1.Instance.DefaultType);

            TransitAccountEventType t_type2 = new TransitAccountEventType();
            t_type2.DefaultType = true;
            t_type2.Name = GetNewString();
            ManagedAccountEventType m_type2 = new ManagedAccountEventType(Session);
            t_type2.Id = m_type2.CreateOrUpdate(t_type2, AdminSecurityContext);

            ManagedAccountEventType m_type1_copy2 = new ManagedAccountEventType(Session, t_type1.Id);
            Assert.IsFalse(m_type1_copy2.Instance.DefaultType);
            
            m_type1.Delete(AdminSecurityContext);
            m_type2.Delete(AdminSecurityContext);
        }
    }
}

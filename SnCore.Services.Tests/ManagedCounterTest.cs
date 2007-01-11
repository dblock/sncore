using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedCounterTest : ManagedCRUDTest<Counter, TransitCounter, ManagedCounter>
    {
        public ManagedCounterTest()
        {

        }

        public override TransitCounter GetTransitInstance()
        {
            TransitCounter t_instance = new TransitCounter();
            t_instance.Total = 1;
            t_instance.Uri = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }

        [Test]
        public void TestFindByUri()
        {
            TransitCounter t_instance = new TransitCounter();
            t_instance.Total = 123;
            t_instance.Uri = string.Format("http://uri/{0}", Guid.NewGuid());
            ManagedCounter m_instance = new ManagedCounter(Session);
            t_instance.Id = m_instance.CreateOrUpdate(t_instance, AdminSecurityContext);
            TransitCounter t_instance_find = ManagedCounter.FindByUri(Session, t_instance.Uri, AdminSecurityContext);
            Assert.AreEqual(t_instance.Id, t_instance_find.Id);
            Assert.AreEqual(t_instance.Uri, t_instance_find.Uri);
            Assert.AreEqual(t_instance.Total, t_instance_find.Total);
            m_instance.Delete(AdminSecurityContext);            
        }
    }
}

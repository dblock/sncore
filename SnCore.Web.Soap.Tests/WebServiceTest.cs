using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Protocols;
using System.Reflection;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests
{
    public abstract class WebServiceTest<TransitType>
    {
        private string mOne;
        private string mMany;
        private SoapHttpClientProtocol mEndPoint;
        private string mAdminTicket = string.Empty;
        private string mUserTicket = string.Empty;

        public WebServiceTest(string one, SoapHttpClientProtocol endpoint)
            : this(one, string.Format("{0}s", one), endpoint)
        {

        }

        public WebServiceTest(string one, string many, SoapHttpClientProtocol endpoint)
        {
            mOne = one;
            mMany = many;
            mEndPoint = endpoint;
        }

        public SoapHttpClientProtocol EndPoint
        {
            get
            {
                return mEndPoint;
            }
        }

        public string GetUserTicket()
        {
            if (string.IsNullOrEmpty(mUserTicket))
            {
                WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
                try
                {
                    mUserTicket = endpoint.Login("user@localhost.com", "password");
                }
                catch (SoapException)
                {
                    WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
                    t_instance.Name = Guid.NewGuid().ToString();
                    t_instance.Password = "password";
                    t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
                    WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
                    int id = account_endpoint.CreateAccount(string.Empty, "user@localhost.com", t_instance);
                    Assert.IsTrue(id > 0);
                    mUserTicket = endpoint.Login("user@localhost.com", "password");
                }
            }

            return mUserTicket;
        }

        public string GetAdminTicket()
        {
            if (string.IsNullOrEmpty(mAdminTicket))
            {
                WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
                mAdminTicket = endpoint.Login("admin@localhost.com", "password");
            }

            return mAdminTicket;
        }

        public abstract TransitType GetTransitInstance();

        public virtual object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket };
            return args;
        }

        public virtual object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, options };
            return args;
        }

        public int GetCount(string ticket)
        {
            try
            {
                object[] args = GetCountArgs(ticket);
                int count = (int)mEndPoint.GetType().InvokeMember(string.Format("Get{0}Count", mMany),
                    BindingFlags.InvokeMethod, null, mEndPoint, args);

                Console.WriteLine("Count: {0}", count);
                Assert.IsTrue(count >= 0);
                return count;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public int Create(string ticket, TransitType t_instance)
        {
            try
            {
                // Console.Write("Creating {0}", mOne);
                object[] args = { ticket, t_instance };
                int id = (int)mEndPoint.GetType().InvokeMember(string.Format("CreateOrUpdate{0}", mOne),
                    BindingFlags.InvokeMethod, null, mEndPoint, args);
                Assert.IsTrue(id > 0);
                Console.WriteLine("Created {0}:{1}", mOne, id);
                return id;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public int Create(string ticket)
        {
            return Create(ticket, GetTransitInstance());
        }

        public object GetInstancePropertyById(string ticket, int id, string name)
        {
            try
            {
                Console.WriteLine("Fetching {0}:{1}", mOne, id);
                object[] args = { ticket, id };
                TransitType t_instance_r = (TransitType)mEndPoint.GetType().InvokeMember(string.Format("Get{0}ById", mOne),
                    BindingFlags.InvokeMethod, null, mEndPoint, args);
                // the retrieved instance must have an id
                return t_instance_r.GetType().InvokeMember(name, BindingFlags.GetProperty, null, t_instance_r, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public int GetMany(string ticket, object options)
        {
            try
            {
                Console.WriteLine("Fetching {0}", mMany);
                object[] args = GetArgs(ticket, options);
                TransitType[] t_instances = (TransitType[])mEndPoint.GetType().InvokeMember(string.Format("Get{0}", mMany),
                    BindingFlags.InvokeMethod, null, mEndPoint, args);
                return t_instances.Length;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public object GetServiceQueryOptions(int page, int size)
        {
            try
            {
                object options = Activator.CreateInstance(mEndPoint.GetType().Assembly.GetType(
                    string.Format("{0}.{1}.ServiceQueryOptions", mEndPoint.GetType().Assembly.GetName().Name, mEndPoint.GetType().Name)));

                object[] pagenumber_args = { page };
                options.GetType().InvokeMember("PageNumber", BindingFlags.SetProperty, null, options, pagenumber_args);

                object[] pagesize_args = { size };
                options.GetType().InvokeMember("PageSize", BindingFlags.SetProperty, null, options, pagesize_args);

                return options;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public void Delete(string ticket, int id)
        {
            try
            {
                Console.WriteLine("Deleting {0}:{1}", mOne, id);
                object[] args = { ticket, id };
                mEndPoint.GetType().InvokeMember(string.Format("Delete{0}", mOne),
                    BindingFlags.InvokeMethod, null, mEndPoint, args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public void TestEndToEnd(string ticket)
        {
            int count1 = GetCount(ticket);
            int id1 = Create(ticket);
            int count2 = GetCount(ticket);
            Assert.IsTrue(count2 >= 0 && count1 + 1 == count2);
            int id2 = (int)GetInstancePropertyById(ticket, id1, "Id");
            Assert.AreEqual(id1, id2);
            int count3 = GetMany(ticket, null);
            Assert.IsTrue(count2 == count3);
            int count4 = GetMany(ticket, GetServiceQueryOptions(0, 0));
            Assert.IsTrue(count2 == count4);
            const int page_size = 10;
            int count5 = GetMany(ticket, GetServiceQueryOptions(0, page_size));
            Assert.IsTrue(count5 >= 1 && count5 <= page_size);
            Delete(ticket, id1);
            int count6 = GetCount(ticket);
            Assert.IsTrue(count1 == count6);
        }

        [Test]
        public virtual void TestEndToEnd()
        {
            TestEndToEnd(GetAdminTicket());
        }

        [Test, ExpectedException(typeof(SoapException))]
        public virtual void TestWithoutLogin()
        {
            Create(string.Empty);
        }
    }
}

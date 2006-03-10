using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;

namespace SnCore.Services
{

    public class ManagedAccountRightBits
    {
        public ManagedAccountRightBits()
        {

        }

        public ManagedAccountRightBits(bool iv)
        {
            AllowCreate = AllowDelete = AllowRetrieve = AllowUpdate = iv;
        }

        public ManagedAccountRightBits(AccountRight r)
        {
            AllowCreate = r.AllowCreate;
            AllowDelete = r.AllowDelete;
            AllowRetrieve = r.AllowRetrieve;
            AllowUpdate = r.AllowUpdate;
        }

        public bool AllowCreate = false;
        public bool AllowRetrieve = false;
        public bool AllowUpdate = false;
        public bool AllowDelete = false;
    }

    /// <summary>
    /// Managed AccountRight right.
    /// </summary>
    public class ManagedAccountRight : ManagedService
    {
        private AccountRight mAccountRight;

        public ManagedAccountRight(ISession session)
            : base(session)
        {

        }

        public ManagedAccountRight(ISession session, int id)
            : base(session)
        {
            mAccountRight = (AccountRight)Session.Load(typeof(AccountRight), id);
        }

        public ManagedAccountRight(ISession session, AccountRight value)
            : base(session)
        {
            mAccountRight = value;
        }

        public int Id
        {
            get
            {
                return mAccountRight.Id;
            }
        }

        public ManagedDataObject DataObject
        {
            get
            {
                return new ManagedDataObject(Session, mAccountRight.DataObject);
            }
        }

        public void Delete()
        {
            mAccountRight.Account.AccountRights.Remove(mAccountRight);
            Session.Delete(mAccountRight);
        }

        public void Create(Account a, DataObject o)
        {
            ManagedAccountRightBits b = new ManagedAccountRightBits();
            b.AllowCreate = b.AllowDelete = b.AllowRetrieve = b.AllowUpdate = true;
            Create(a, o, b);
        }

        public void Create(Account a, DataObject o, ManagedAccountRightBits b)
        {
            mAccountRight = new AccountRight();
            mAccountRight.Account = a;
            mAccountRight.DataObject = o;
            mAccountRight.AllowCreate = b.AllowCreate;
            mAccountRight.AllowDelete = b.AllowDelete;
            mAccountRight.AllowRetrieve = b.AllowRetrieve;
            mAccountRight.AllowUpdate = b.AllowUpdate;

            Session.Save(mAccountRight);
        }

        public bool HasAllRights()
        {
            return
                mAccountRight.AllowCreate
                && mAccountRight.AllowDelete
                && mAccountRight.AllowRetrieve
                && mAccountRight.AllowUpdate;
        }

        public ManagedAccountRightBits Rights
        {
            get
            {
                return new ManagedAccountRightBits(mAccountRight);
            }
        }
    }
}

using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountGroup : TransitService<AccountGroup>
    {
        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {
                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {
                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        private bool mIsPrivate;

        public bool IsPrivate
        {
            get
            {
                return mIsPrivate;
            }
            set
            {
                mIsPrivate = value;
            }
        }

        private int mPictureId = 0;

        public int PictureId
        {
            get
            {

                return mPictureId;
            }
            set
            {
                mPictureId = value;
            }
        }

        public TransitAccountGroup()
        {

        }

        public TransitAccountGroup(AccountGroup value)
            : base(value)
        {

        }

        public override void SetInstance(AccountGroup value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Modified = value.Modified;
            IsPrivate = value.IsPrivate;
            PictureId = ManagedAccountGroup.GetRandomAccountGroupPictureId(value);
            base.SetInstance(value);
        }

        public override AccountGroup GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroup instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.IsPrivate = this.IsPrivate;
            return instance;
        }
    }

    public class ManagedAccountGroup : ManagedService<AccountGroup, TransitAccountGroup>
    {
        public ManagedAccountGroup()
        {

        }

        public ManagedAccountGroup(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroup(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroup(ISession session, AccountGroup value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            bool fNew = (mInstance.Id == 0);
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
            // create an admin account that owns the group
            if (fNew)
            {
                TransitAccountGroupAccount t_admin = new TransitAccountGroupAccount();
                t_admin.AccountGroupId = mInstance.Id;
                t_admin.AccountId = sec.Account.Id;
                t_admin.IsAdministrator = true;
                ManagedAccountGroupAccount m_admin = new ManagedAccountGroupAccount(Session);
                m_admin.CreateOrUpdate(t_admin, ManagedAccount.GetAdminSecurityContext(Session));
            }
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            // everyone can create a group
            acl.Add(new ACLAuthenticatedAllowCreate());
            // everyone is able to see a group (only name/description)
            acl.Add(new ACLEveryoneAllowRetrieve());
            // members can edit or see the group depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Retreive | DataOperation.Update));
            }
            return acl;
        }

        public static int GetRandomAccountGroupPictureId(ISession session, int id)
        {
            try
            {
                AccountGroup AccountGroup = session.Load<AccountGroup>(id);
                return GetRandomAccountGroupPictureId(AccountGroup);
            }
            catch (ObjectNotFoundException)
            {
                return 0;
            }
        }

        public static int GetRandomAccountGroupPictureId(AccountGroup group)
        {
            if (group == null || group.AccountGroupPictures == null || group.AccountGroupPictures.Count == 0)
                return 0;

            return ManagedService<AccountGroupPicture, TransitAccountGroupPicture>.GetRandomElementId(group.AccountGroupPictures);
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Session.Delete(string.Format("FROM AccountGroupAccountRequest r WHERE r.AccountGroup.Id = {0}", Id));
            base.Delete(sec);
        }

        public void Leave(int accountid, ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Update);

            Account adminaccount = null;
            bool fHasAdministrator = false;
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroupAccounts))
            {
                if (account.Account.Id == accountid)
                {
                    Session.Delete(account);
                }
                else if (account.IsAdministrator)
                {
                    // has at least one administrator left
                    fHasAdministrator = true;
                    adminaccount = account.Account;
                }
            }

            if (!fHasAdministrator)
            {
                // deleted the last administrator
                AccountGroupAccount admin = new AccountGroupAccount();
                admin.Account = ManagedAccount.GetAdminAccount(Session);
                if (admin.Account.Id == accountid)
                {
                    // the systme administrator tried to leave the group, he was last and is automatically re-added
                    throw new Exception("System administrator cannot be the last to leave a group.");
                }

                adminaccount = admin.Account;

                admin.AccountGroup = mInstance;
                admin.Created = admin.Modified = DateTime.UtcNow;
                admin.IsAdministrator = true;
                Session.Save(admin);
            }

            // orphan any invitations that this account sent
            foreach (AccountGroupAccountInvitation invitation in Collection<AccountGroupAccountInvitation>.GetSafeCollection(mInstance.AccountGroupAccountInvitations))
            {
                if (invitation.Requester.Id == accountid)
                {
                    invitation.Requester = adminaccount;
                    Session.Save(invitation);
                }
            }
        }

        public bool HasAccount(int accountid)
        {
            return (Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountGroupAccount instance where " +
                    "(instance.AccountGroup.Id = {0} and instance.Account.Id = {1})",
                    Id, accountid)).UniqueResult<int>() > 0);
        }

        public bool HasAccountRequest(int accountid)
        {
            return (Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountGroupAccountRequest instance where " +
                    "(instance.AccountGroup.Id = {0} and instance.Account.Id = {1})",
                    Id, accountid)).UniqueResult<int>() > 0);
        }
    }
}

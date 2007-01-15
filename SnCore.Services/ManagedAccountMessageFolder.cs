using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using SnCore.Tools.Collections;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountMessageFolder : TransitService<AccountMessageFolder>
    {
        private string mFullPath;

        public string FullPath
        {
            get
            {
                return mFullPath;
            }
            set
            {
                mFullPath = value;
            }
        }

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

        private bool mSystem = false;

        public bool System
        {
            get
            {

                return mSystem;
            }
            set
            {
                mSystem = value;
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

        private int mAccountId = 0;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private int mInstanceParentId = 0;

        public int AccountMessageFolderParentId
        {
            get
            {
                return mInstanceParentId;
            }
            set
            {
                mInstanceParentId = value;
            }
        }

        private int mLevel = 0;

        public int Level
        {
            get
            {
                return mLevel;
            }
            set
            {
                mLevel = value;
            }
        }

        private int mMessageCount = 0;

        public int MessageCount
        {
            get
            {
                return mMessageCount;
            }
            set
            {
                mMessageCount = value;
            }
        }

        public TransitAccountMessageFolder()
        {

        }

        public TransitAccountMessageFolder(AccountMessageFolder value)
            : base(value)
        {

        }

        public override void SetInstance(AccountMessageFolder value)
        {
            Name = value.Name;
            System = value.System;
            Created = value.Created;
            Modified = value.Modified;
            AccountId = value.Account.Id;
            AccountMessageFolderParentId = (value.AccountMessageFolderParent != null) ?
                value.AccountMessageFolderParent.Id : 0;

            FullPath = Name;

            AccountMessageFolder parent = value.AccountMessageFolderParent;
            while (parent != null)
            {
                Level++;
                FullPath = FullPath.Insert(0, parent.Name + "/");
                parent = parent.AccountMessageFolderParent;
            }

            base.SetInstance(value);
        }

        public override AccountMessageFolder GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountMessageFolder instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.System = this.System;
            } 
            
            if (Id == 0 || ! instance.System) // system folders cannot be renamed
            {
                instance.Name = this.Name;
            }

            instance.AccountMessageFolderParent =
                (this.AccountMessageFolderParentId != 0) ?
                (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), this.AccountMessageFolderParentId) :
                null;

            return instance;
        }
    }

    public class AccountMessageFolderTree : Tree<AccountMessageFolder>
    {
        public static bool FolderIsEqual(AccountMessageFolder left, AccountMessageFolder right)
        {
            return left.Id == right.Id;
        }

        public static bool FolderIsParent(AccountMessageFolder parent, AccountMessageFolder child)
        {
            if (child.AccountMessageFolderParent == null)
                return false;
            return parent.Id == child.AccountMessageFolderParent.Id;
        }

        public static bool FolderHasParent(AccountMessageFolder folder)
        {
            return folder.AccountMessageFolderParent != null;
        }

        public AccountMessageFolderTree()
        {
        }

        public AccountMessageFolderTree(IEnumerable<AccountMessageFolder> list)
            :
            base(list, FolderHasParent, FolderIsParent, FolderIsEqual)
        {

        }

        public AccountMessageFolderTree(System.Collections.IEnumerable list)
            :
            base(list, FolderHasParent, FolderIsParent, FolderIsEqual)
        {

        }
    }

    public class ManagedAccountMessageFolder : ManagedService<AccountMessageFolder, TransitAccountMessageFolder>
    {
        public ManagedAccountMessageFolder()
        {

        }

        public ManagedAccountMessageFolder(ISession session)
            : base(session)
        {

        }

        public ManagedAccountMessageFolder(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountMessageFolder(ISession session, AccountMessageFolder value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public override TransitAccountMessageFolder GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountMessageFolder t_instance = base.GetTransitInstance(sec);
            t_instance.MessageCount = MessageCount;
            return t_instance;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            foreach (AccountMessageFolder accountmessagefolder in Collection<AccountMessageFolder>.GetSafeCollection(mInstance.AccountMessageFolders))
            {
                new ManagedAccountMessageFolder(Session, accountmessagefolder).Delete(sec);
            }

            DeleteAccountMessages(sec);
            Collection<AccountMessageFolder>.GetSafeCollection(mInstance.Account.AccountMessageFolders).Remove(mInstance);
            base.Delete(sec);
        }

        public void DeleteAccountMessages(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Delete);

            foreach (AccountMessage accountmessage in Collection<AccountMessage>.GetSafeCollection(mInstance.AccountMessages))
            {
                Session.Delete(accountmessage);
            }

            mInstance.AccountMessages = null;
        }

        public int MessageCount
        {
            get
            {
                return GetMessageFolderMessageCount(Session, Id);
            }
        }

        public static int GetMessageFolderMessageCount(ISession session, int folderid)
        {
            return (int)session.CreateQuery(
                string.Format(
                    "select count(*)" +
                    " from AccountMessage m, AccountMessageFolder f" +
                    " where m.AccountMessageFolder.Id = f.Id" +
                    " and f.Id = {0}",
                    folderid.ToString())).UniqueResult();
        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

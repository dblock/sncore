using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using SnCore.Tools.Collections;

namespace SnCore.Services
{
    public class TransitAccountMessageFolder : TransitService
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

        private int mAccountMessageFolderParentId = 0;

        public int AccountMessageFolderParentId
        {
            get
            {

                return mAccountMessageFolderParentId;
            }
            set
            {
                mAccountMessageFolderParentId = value;
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

        public TransitAccountMessageFolder(AccountMessageFolder p)
            : base(p.Id)
        {
            Name = p.Name;
            System = p.System;
            Created = p.Created;
            Modified = p.Modified;
            AccountId = p.Account.Id;
            AccountMessageFolderParentId = (p.AccountMessageFolderParent != null) ?
                p.AccountMessageFolderParent.Id : 0;

            FullPath = Name;
            AccountMessageFolder parent = p.AccountMessageFolderParent;
            while (parent != null)
            {
                Level++;
                FullPath = FullPath.Insert(0, parent.Name + "/");
                parent = parent.AccountMessageFolderParent;
            }
        }

        public AccountMessageFolder GetAccountMessageFolder(ISession session)
        {
            AccountMessageFolder p = (Id != 0) ? (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), Id) : new AccountMessageFolder();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), this.AccountId);
                p.System = this.System;
            }

            p.Name = this.Name;

            p.AccountMessageFolderParent =
                (this.AccountMessageFolderParentId != 0) ?
                (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), this.AccountMessageFolderParentId) :
                null;

            return p;
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

    /// <summary>
    /// Managed account message folder.
    /// </summary>
    public class ManagedAccountMessageFolder : ManagedService
    {
        private AccountMessageFolder mAccountMessageFolder = null;

        public ManagedAccountMessageFolder(ISession session)
            : base(session)
        {

        }

        public ManagedAccountMessageFolder(ISession session, int id)
            : base(session)
        {
            mAccountMessageFolder = (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), id);
        }

        public ManagedAccountMessageFolder(ISession session, AccountMessageFolder value)
            : base(session)
        {
            mAccountMessageFolder = value;
        }

        public int Id
        {
            get
            {
                return mAccountMessageFolder.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountMessageFolder.Account.Id;
            }
        }

        public TransitAccountMessageFolder TransitAccountMessageFolder
        {
            get
            {
                TransitAccountMessageFolder f = new TransitAccountMessageFolder(mAccountMessageFolder);
                f.MessageCount = MessageCount;
                return f;
            }
        }

        public void Delete()
        {
            if (mAccountMessageFolder.AccountMessageFolders != null)
            {
                foreach (AccountMessageFolder accountmessagefolder in mAccountMessageFolder.AccountMessageFolders)
                {
                    new ManagedAccountMessageFolder(Session, accountmessagefolder).Delete();
                }
            }

            DeleteAccountMessages();
            mAccountMessageFolder.Account.AccountMessageFolders.Remove(mAccountMessageFolder);
            Session.Delete(mAccountMessageFolder);
        }

        public void DeleteAccountMessages()
        {
            if (mAccountMessageFolder.AccountMessages != null)
            {
                foreach (AccountMessage accountmessage in mAccountMessageFolder.AccountMessages)
                {
                    Session.Delete(accountmessage);
                }
            }

            mAccountMessageFolder.AccountMessages = null;
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
                return mAccountMessageFolder.Account;
            }
        }
    }
}

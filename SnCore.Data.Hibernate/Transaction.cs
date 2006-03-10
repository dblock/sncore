using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using NHibernate;
using ITransaction = System.EnterpriseServices.ITransaction;

namespace SnCore.Data.Hibernate
{
    /// <summary>
    /// Helper class for working with COM+ transactions.
    /// From Andrew Mayorov, http://wiki.nhibernate.org/display/NH/Using+NHibernate+with+ASP.Net
    /// <seealso cref="Session.BeginTransaction"/>
    /// </summary>
    public class Transaction : IDisposable
    {
        private bool _disposed;

        public Transaction()
        {
            _disposed = false;

            ServiceConfig config = new ServiceConfig();
            config.Transaction = TransactionOption.Required;
            ServiceDomain.Enter(config);

            // SetComplete must be explicitely called.
            ContextUtil.MyTransactionVote = TransactionVote.Abort;

            // Check if connection is already open
            if (Session.Current.IsConnected)
            {
                IDbConnection con = Session.Current.Connection;
                if (con.State == ConnectionState.Open)
                {
                    ((SqlConnection)con).EnlistDistributedTransaction((ITransaction)ContextUtil.Transaction);
                }
            }
        }

        ~Transaction()
        {
            Dispose();
        }

        public void SetComplete()
        {
            try
            {
                ContextUtil.SetComplete();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void SetAbort()
        {
            try
            {
                ContextUtil.SetAbort();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    ServiceDomain.Leave();
                }
                finally
                {
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}

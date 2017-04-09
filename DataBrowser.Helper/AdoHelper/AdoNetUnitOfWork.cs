using RepositoryFoundation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Helper.AdoHelper
{
    public class AdoNetUnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;
        private readonly Action<AdoNetUnitOfWork> _rolledBack;
        private readonly Action<AdoNetUnitOfWork> _committed;
        public AdoNetUnitOfWork(IDbTransaction transaction, Action<AdoNetUnitOfWork> rolledBack, Action<AdoNetUnitOfWork> committed)
        {
            Transaction = transaction;
            _transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
        }
        public AdoNetUnitOfWork(IDbTransaction transaction)
        {
            Transaction = transaction;
            _transaction = transaction;
        }
        public IDbTransaction Transaction { get; private set; }
        public void Dispose()
        {
            if (_transaction == null)
                return;
            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack?.Invoke(this);
            _transaction = null;
        }
        public int Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call save changes twice.");
            _transaction.Commit();
            _committed?.Invoke(this);
            _transaction = null;
            return 1;
        }

        public Task<int> CommitAsync()
        {
            return new Task<int>(() =>
            {
                return Commit();
            });
        }
    }
}

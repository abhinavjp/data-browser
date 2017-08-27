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
        private readonly Action<AdoNetUnitOfWork> _committed;
        private readonly Action<AdoNetUnitOfWork> _rolledBack;
        private IDbTransaction _transaction;
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
        public int Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _committed?.Invoke(this);
                _transaction = null;
            }
            return 1;
        }

        public Task<int> CommitAsync()
        {
            return new Task<int>(() =>
            {
                return Commit();
            });
        }
        public void Dispose()
        {
            if (_transaction == null)
                return;
            _transaction.Rollback();
            _transaction.Dispose();
            _rolledBack?.Invoke(this);
            _transaction = null;
        }

        public void SetLogger(Action<string> logger)
        {
            throw new NotImplementedException();
        }

        public void SetCommandTimeout(int timeOut)
        {
            throw new NotImplementedException();
        }
    }
}

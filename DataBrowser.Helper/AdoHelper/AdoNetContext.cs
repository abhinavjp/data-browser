﻿//using RepositoryFoundation.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace DataBrowser.Helper.AdoHelper
//{
//    public class AdoNetContext: IDisposable
//    {
//        private readonly IDbConnection _connection;
//        private readonly IConnectionFactory _connectionFactory;
//        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
//        private readonly LinkedList<AdoNetUnitOfWork> _uows = new LinkedList<AdoNetUnitOfWork>();
//        public AdoNetContext(IConnectionFactory connectionFactory)
//        {
//            _connectionFactory = connectionFactory;
//            _connection = _connectionFactory.Create();
//        }
//        //public IUnitOfWork<TContext> CreateUnitOfWork<TContext>() where TContext: DbContext
//        //{
//        //    var transaction = _connection.BeginTransaction();
//        //    var uow = new AdoNetUnitOfWork(transaction, RemoveTransaction, RemoveTransaction);
//        //    _rwLock.EnterWriteLock();
//        //    _uows.AddLast(uow);
//        //    _rwLock.ExitWriteLock();
//        //    return uow;
//        //}
//        public IDbCommand CreateCommand()
//        {
//            var command = _connection.CreateCommand();
//            _rwLock.EnterReadLock();
//            if (_uows.Count > 0)
//                command.Transaction = _uows.First.Value.Transaction;
//            _rwLock.ExitReadLock();
//            return command;
//        }
//        private void RemoveTransaction(AdoNetUnitOfWork obj)
//        {
//            _rwLock.EnterWriteLock();
//            _uows.Remove(obj);
//            _rwLock.ExitWriteLock();
//        }
//        public void Dispose()
//        {
//            _connection.Dispose();
//            _rwLock.Dispose();
//            GC.SuppressFinalize(this);
//        }
//    }
//}

using DataBrowser.Data;
using DataBrowser.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrowser.Data.Repository
{
    public class RepositoryPattern<T> : IRepositoryInterface<T> where T : class
    {
        private DataBrowserDbContext databrowserEntities = null;
        private DbSet<T> table = null;
        public RepositoryPattern()
        {
            this.databrowserEntities = new DataBrowserDbContext();
            table = databrowserEntities.Set<T>();
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Dispose()
        {

        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            databrowserEntities.SaveChanges();
        }

        public IEnumerable<T> SelectAll()
        {

            return table.ToList();
        }
        public T SelectByID(object id)
        {
            return table.Find(id);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            databrowserEntities.Entry(obj).State = EntityState.Modified;
        }
        public void BulkInsert(IEnumerable<T> arry)
        {
            databrowserEntities.BulkInsert<T>(arry);
        }
        public void BulkDelete(IEnumerable<T> arry)
        {
            databrowserEntities.BulkDelete<T>(arry);
        }
    }
}

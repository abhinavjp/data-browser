namespace DataBrowser.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataBrowserModel : DbContext
    {
        public DataBrowserModel()
            : base("name=DataBrowserModelEntites")
        {
        }

        public virtual DbSet<DataBaseConnection> DataBaseConnections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

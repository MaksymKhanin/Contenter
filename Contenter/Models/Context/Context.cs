using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Contenter.Models.Context
{
        public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public Context() : base("LocalDb") { }
        static Context()
        {
            Database.SetInitializer<Context>(new DataBaseInitializer());
        }
    }

    public class DataBaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context db)
        {


            base.Seed(db);
        }
    }
}
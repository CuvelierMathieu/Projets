using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCL.Models;

namespace DAL
{
    public class Context : DbContext
    {
        public Context() : base("name=DEV_DistantDB")
        {
            //Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
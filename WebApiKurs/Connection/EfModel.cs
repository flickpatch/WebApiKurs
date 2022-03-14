using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiKurs.Entities;

namespace WebApiKurs.Connection
{
    public class EfModel:DbContext
    {
        public EfModel(DbContextOptions options): base(options)
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
    }
}

using Service_Center.Models;
using Service_Center.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Contexts
{
    class Context :DbContext
    {
        public Context() : base("DBConnection") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Rapair> Rapairs { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Themes> Themes { get; set; }        
    }
}

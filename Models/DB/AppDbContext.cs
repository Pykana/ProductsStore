using Microsoft.EntityFrameworkCore;
using BACKEND_STORE.Models.ENTITIES;

namespace BACKEND_STORE.Models.DB
{
    public class AppDbContext : DbContext
    {
        // <summary>
        /// Contexto de la base de datos que hereda de DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DBSet representa una colección de todas las entidades en la base de datos
        /// Representa la colección de usuarios
        public DbSet<Audit_Logs> AuditLogs { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Customer_User> CustomerUsers { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Roles> Roles { get; set; }

        /// <summary>
        /// Configuración del modelo de la base de datos
        /// 

        protected override void OnModelCreating(ModelBuilder modelBuilder){ 

        
        }

    }
}


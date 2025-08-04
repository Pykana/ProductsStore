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

        // Configuración de las entidades y sus relaciones

        protected override void OnModelCreating(ModelBuilder modelBuilder){

            /********************************************************/
            // Configuración de la entidad Audit_Logs
            /********************************************************/

            modelBuilder.Entity<Audit_Logs>(entity => { 
                entity.ToTable("Audit_Logs");                   // Nombre de la tabla en la base de datos

                entity.HasKey(e => e.Id_AuditLog);              // Primary key
                entity.Property(e => e.Id_AuditLog)             // Primary key property
                        .HasColumnName("Id_AuditLog")           // Nombre de la columna en la base de datos
                        .ValueGeneratedOnAdd();                 // auto-increment

                entity.Property(e => e.TableName)
                      .HasColumnName("Table_Name")              // Nombre de la columna en la base de datos
                      .IsRequired()                             // Es obligatoria
                      .HasMaxLength(200);                       // Maxima longitud de 200 caracteres

                entity.Property(e => e.Operation)
                       .HasColumnName("Operation")              // Nombre de la columna en la base de datos
                       .IsRequired()                            // Es obligatoria
                       .HasMaxLength(50);                       // Maxima longitud de 50 caracteres

                entity.Property(e => e.Timestamp)
                       .HasColumnName("Timestamp")              // Nombre de la columna en la base de datos
                       .HasColumnType("Date")                   // Tipo de dato Date
                       .HasDefaultValueSql("GETDATE()");        // Valor por defecto para la fecha y hora

                entity.Property(e => e.PrimaryKey)
                        .HasColumnName("Primary_Key")           // Nombre de la columna en la base de datos
                       .IsRequired()                            // Es obligatoria
                       .HasMaxLength(200);                      // Maxima longitud de 200 caracteres

                entity.Property(e => e.OldValues)
                        .HasColumnName("Old_Values")            // Nombre de la columna en la base de datos
                        .HasColumnType("nvarchar(max)");        // JSON con datos antes del cambio

                entity.Property(e => e.NewValues)
                        .HasColumnName("New_Values")            // Nombre de la columna en la base de datos
                        .HasColumnType("nvarchar(max)");        // JSON con datos después del cambio

                entity.Property(e => e.ColumnNames)
                        .HasColumnName("Column_Names")          // Nombre de la columna en la base de datos
                        .HasColumnType("nvarchar(max)");        // Columnas afectadas

                entity.Property(e => e.UserId)
                        .HasColumnName("UserId")                // Nombre de la columna en la base de datos
                        .IsRequired();                          // Es obligatoria  

                entity.Property(e => e.ChangedBy)
                        .HasColumnName("Changed_By");           // Nombre de la columna en la base de datos

                entity.HasOne(e => e.ChangedBy)                 // Relación con la entidad Users
                      .WithMany()                               // Muchos a uno
                      .HasForeignKey("UserId")                  // Clave foránea
                      .OnDelete(DeleteBehavior.Restrict);       // Comportamiento al eliminar (no eliminar en cascada)
            });


            /********************************************************/
            // Configuración de la entidad Customers
            /********************************************************/

            modelBuilder.Entity<Customers>(entity => {
                entity.ToTable("Customers");                    // Nombre de la tabla en la base de datos
                entity.HasKey(e => e.Id_Customer);              // Primary key
                entity.Property(e => e.Id_Customer)             // Primary key property
                      .ValueGeneratedOnAdd();                   // auto-increment

                entity.Property(e => e.customer_name)
                    .HasMaxLength(250)                          // Maxima longitud de 100 caracteres
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.customer_email)
                    .HasMaxLength(255)                          // Maxima longitud de 255 caracteres
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.customer_phone)
                    .IsRequired();                              // Es obligatoria
                    

            });
            modelBuilder.Entity<Customer_User>(entity => {

            });
            modelBuilder.Entity<Users>(entity => {

            });
            modelBuilder.Entity<Products>(entity => {

            });
            modelBuilder.Entity<Orders>(entity => {

            });
            modelBuilder.Entity<OrderDetail>(entity => {

            });
            modelBuilder.Entity<Roles>(entity => {

            }); 
        }

    }
}


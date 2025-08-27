using BACKEND_STORE.Models.ENTITIES;
using Microsoft.EntityFrameworkCore;

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

        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditLogs();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditLogs()
        {
           Console.WriteLine("Audit log triggered");
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

                //entity.Property(e => e.ChangedBy)
                //        .HasColumnName("Changed_By");           // Nombre de la columna en la base de datos

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
                      .HasColumnName("Id_Customer")             // Nombre de la columna en la base de datos  .
                      .ValueGeneratedOnAdd();                   // auto-increment

                entity.Property(e => e.customer_name)
                    .HasMaxLength(250)                          // Maxima longitud de 100 caracteres
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.customer_email)
                    .HasMaxLength(255)                          // Maxima longitud de 255 caracteres
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.customer_phone)
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.customer_address)
                    .HasMaxLength(500)                         // Maxima longitud de 500 caracteres
                    .IsRequired();                              // Es obligatoria

                entity.Property(e => e.created_at)
                    .HasColumnType("Date")                      // Tipo de dato Date
                    .HasDefaultValueSql("GETDATE()");          // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                    .HasColumnType("Date")                     // Tipo de dato Date
                    .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_at)
                    .HasColumnType("Date")                      // Tipo de dato Date
                    .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.is_active)
                    .HasColumnType("bit") // Tipo de dato bit
                    .HasDefaultValue(true);                     // Valor por defecto para el estado activo

                entity.Property(e => e.created_by)
                    .HasMaxLength(100)                          // Maxima longitud de 100 caracteres
                    .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.updated_by)
                    .HasMaxLength(100)                          // Maxima longitud de 100 caracteres
                    .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_by)
                    .HasMaxLength(100)                          // Maxima longitud de 100 caracteres
                    .IsRequired(false);                          // Es no obligatoria

                // Indice Unico
                entity.HasIndex(e => e.customer_email)
                    .IsUnique();

                entity.HasIndex(e => e.customer_phone)
                    .IsUnique();


            });
            modelBuilder.Entity<Customer_User>(entity => {
                entity.ToTable("Customer_User"); // Nombre de la tabla en la base de datos
                
                entity.HasKey(e => new { e.customer_id, e.user_id }); // Primary key compuesta
                
                entity.Property(e => e.customer_id)
                        .HasColumnName("customer_id") // Nombre de la columna en la base de datos
                        .IsRequired();                // Es obligatoria

                entity.Property(e => e.user_id)
                       .HasColumnName("user_id") // Nombre de la columna en la base de datos
                       .IsRequired();                // Es obligatoria

                entity.Property(e => e.created_at)
                          .HasColumnType("Date") // Tipo de dato Date
                          .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                            .HasColumnType("Date") // Tipo de dato Date
                            .IsRequired(false);                          // Es no obligatoria


                entity.Property(e => e.deleted_at)
                            .HasColumnType("Date") // Tipo de dato Date
                            .IsRequired(false);                          // Es no obligatoria


                entity.Property(e => e.created_by)
                            .HasMaxLength(100) // Maxima longitud de 100 caracteres
                            .IsRequired(false);                          // Es no obligatoria


                entity.Property(e => e.updated_by)
                            .HasMaxLength(100) // Maxima longitud de 100 caracteres
                            .IsRequired(false);                          // Es no obligatoria


                entity.Property(e => e.deleted_by)
                            .HasMaxLength(100) // Maxima longitud de 100 caracteres
                            .IsRequired(false);                          // Es no obligatoria


                //llaves foráneas

                entity.HasOne(e => e.Customer) // Relación con la entidad Customers
                      .WithMany(c => c.CustomerUsers) // Muchos a uno
                      .HasForeignKey(e => e.customer_id); // Clave foránea

                entity.HasOne(e => e.Customer) // Relación con la entidad Customers
                      .WithMany(c => c.CustomerUsers) // Muchos a uno
                      .HasForeignKey(e => e.user_id); // Clave foránea

            });
            modelBuilder.Entity<Users>(entity => {
                entity.ToTable("Users"); // Nombre de la tabla en la base de datos

                entity.HasKey(e => e.Id_User); // Primary key
                entity.Property(e => e.Id_User) // Primary key property
                      .HasColumnName("Id_User") // Nombre de la columna en la base de datos
                      .ValueGeneratedOnAdd(); // auto-increment

                entity.Property(e => e.username)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.password)
                        .HasMaxLength(255) // Maxima longitud de 255 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.email)
                        .HasMaxLength(255) // Maxima longitud de 255 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.name)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.lastname)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.created_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.is_active)
                        .HasColumnType("bit") // Tipo de dato bit
                        .HasDefaultValue(true); // Valor por defecto para el estado activo

                entity.Property(e => e.created_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.updated_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.RoleId)
                        .HasColumnName("RoleId") // Nombre de la columna en la base de datos
                        .IsRequired(); // Es obligatoria

                // Indice Unico
                entity.HasIndex(e => e.username)
                    .IsUnique();

                entity.HasIndex(e => e.email)
                    .IsUnique();

                // Relaciones
                entity.HasOne(e => e.Role) // Relación con la entidad Roles
                      .WithMany() // Muchos a uno
                      .HasForeignKey("RoleId") // Clave foránea
                      .OnDelete(DeleteBehavior.Restrict); // Comportamiento al eliminar (no eliminar en cascada)

            });
            modelBuilder.Entity<Products>(entity => {
                entity.ToTable("Products"); // Nombre de la tabla en la base de datos

                entity.HasKey(e => e.Id_Product); // Primary key
                entity.Property(e => e.Id_Product) // Primary key property
                      .HasColumnName("Id_Product") // Nombre de la columna en la base de datos
                      .ValueGeneratedOnAdd(); // auto-increment

                entity.Property(e => e.product_name)
                        .HasMaxLength(250) // Maxima longitud de 250 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.product_description)
                        .HasMaxLength(500) // Maxima longitud de 500 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.price)
                        .HasColumnType("decimal(18,2)") // Tipo de dato decimal con 18 dígitos y 2 decimales
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.stock_quantity)
                        .HasColumnType("int") // Tipo de dato int
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.created_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.is_active)
                        .HasColumnType("bit") // Tipo de dato bit
                        .HasDefaultValue(true); // Valor por defecto para el estado activo

                entity.Property(e => e.created_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.updated_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria


            });
            modelBuilder.Entity<Orders>(entity => {
                entity.ToTable("Orders"); // Nombre de la tabla en la base de datos

                entity.HasKey(e => e.Id_Order); // Primary key

                entity.Property(e => e.Id_Order) // Primary key property
                      .HasColumnName("Id_Order") // Nombre de la columna en la base de datos
                      .ValueGeneratedOnAdd(); // auto-increment

                entity.Property(e => e.order_date)
                        .HasColumnType("Date") // Tipo de dato Date
                        .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.total_amount)
                        .HasColumnType("decimal(18,2)") // Tipo de dato decimal con 18 dígitos y 2 decimales
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.status)
                        .HasMaxLength(50) // Maxima longitud de 50 caracteres
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.created_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_at)
                        .HasColumnType("Date") // Tipo de dato Date
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.is_active)
                        .HasColumnType("bit") // Tipo de dato bit
                        .HasDefaultValue(true); // Valor por defecto para el estado activo

                entity.Property(e => e.created_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.updated_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_by)
                        .HasMaxLength(100) // Maxima longitud de 100 caracteres
                        .IsRequired(false);                          // Es no obligatoria

                //Llaves foráneas
                entity.Property(e => e.customer_id)
                        .HasColumnName("customer_id") // Nombre de la columna en la base de datos
                        .IsRequired(); // Es obligatoria

                entity.Property(e => e.user_id)
                        .HasColumnName("user_id") // Nombre de la columna en la base de datos
                        .IsRequired(); // Es obligatoria

                // Relaciones
                entity.HasOne(e => e.Customer)
                      .WithMany()
                      .HasForeignKey(e => e.customer_id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.user_id)
                      .OnDelete(DeleteBehavior.Restrict);


            });
            modelBuilder.Entity<OrderDetail>(entity => {
                    entity.ToTable("OrderDetails"); // Nombre de la tabla en la base de datos
                    entity.HasKey(e => e.Id_OrderDetail); // Primary key

                    entity.Property(e => e.Id_OrderDetail) // Primary key property
                          .HasColumnName("Id_OrderDetail") // Nombre de la columna en la base de datos
                          .ValueGeneratedOnAdd(); // auto-increment

                    entity.Property(e => e.quantity)
                            .HasColumnType("int") // Tipo de dato int
                            .IsRequired(); // Es obligatoria

                    entity.Property(e => e.unit_price)
                            .HasColumnType("decimal(18,2)") // Tipo de dato decimal con 18 dígitos y 2 decimales
                            .IsRequired(); // Es obligatoria

                    //Llaves foráneas

                    entity.Property(e => e.order_id)
                            .HasColumnName("order_id") // Nombre de la columna en la base de datos
                            .IsRequired(); // Es obligatoria

                    entity.Property(e => e.product_id)
                            .HasColumnName("product_id") // Nombre de la columna en la base de datos
                            .IsRequired(); // Es obligatoria

                    // Relaciones

                    entity.HasOne(e => e.Order) // Relación con la entidad Orders
                          .WithMany(o => o.OrderDetails) // Muchos a uno
                          .HasForeignKey(e => e.order_id) // Clave foránea
                          .OnDelete(DeleteBehavior.Restrict); // Comportamiento al eliminar (no eliminar en cascada)

                    entity.HasOne(e => e.Product) // Relación con la entidad Products
                            .WithMany() // Muchos a uno
                            .HasForeignKey(e => e.product_id) // Clave foránea
                            .OnDelete(DeleteBehavior.Restrict); // Comportamiento al eliminar (no eliminar en cascada)
            });
            modelBuilder.Entity<Roles>(entity => {
                   entity.ToTable("Roles"); // Nombre de la tabla en la base de datos

                   entity.HasKey(e => e.Id_Role); // Primary key

                   entity.Property(e => e.Id_Role) // Primary key property
                             .HasColumnName("Id_Role") // Nombre de la columna en la base de datos
                             .ValueGeneratedOnAdd(); // auto-increment

                   entity.Property(e => e.role_name)
                             .HasMaxLength(100) // Maxima longitud de 100 caracteres
                             .IsRequired(); // Es obligatoria

                entity.Property(e => e.created_at)
                            .HasColumnType("Date") // Tipo de dato Date
                            .HasDefaultValueSql("GETDATE()"); // Valor por defecto para la fecha y hora

                entity.Property(e => e.updated_at)
                                .HasColumnType("Date") // Tipo de dato Date
                                .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_at)
                                .HasColumnType("Date") // Tipo de dato Date
                                .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.created_by)
                                .HasMaxLength(100) // Maxima longitud de 100 caracteres
                                .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.updated_by)
                                .HasMaxLength(100) // Maxima longitud de 100 caracteres
                                .IsRequired(false);                          // Es no obligatoria

                entity.Property(e => e.deleted_by)
                                .HasMaxLength(100) // Maxima longitud de 100 caracteres
                                .IsRequired(false);                          // Es no obligatoria

                //relacion
                entity.HasMany(e => e.Users) // Relación con la entidad Users
                          .WithOne(u => u.Role) // Muchos a uno
                          .HasForeignKey(u => u.RoleId) // Clave foránea
                          .OnDelete(DeleteBehavior.Restrict); // Comportamiento al eliminar (no eliminar en cascada)

            }); 
        }

    }
}


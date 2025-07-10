using Microsoft.EntityFrameworkCore;
using static BACKEND_STORE.Models.ENTITIES.Entities;

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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<PedidoProducto> PedidoProductos { get; set; }


        /// <summary>
        /// Configuración del modelo de la base de datos
        /// 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///////////////////////////////////////////
            //     LLAVES FORÁNEAS Y RELACIONES      //
            ///////////////////////////////////////////

            //////////////////////////////
            //      PEDIDO PRODUCTO     //
            //////////////////////////////
            modelBuilder.Entity<PedidoProducto>(entity =>
            {
                // Id es la clave primaria compuesta por PedidoId y ProductoId
                entity.HasKey(pp => new { pp.PedidoId, pp.ProductoId });

                // PedidoId es obligatorio y debe existir en la tabla Pedidos
                entity.Property(pp => pp.Cantidad)
                    .IsRequired()
                    .HasDefaultValue(1);

                // ProductoId es obligatorio y debe existir en la tabla Productos
                entity.HasOne(pp => pp.Pedido)
                    .WithMany(p => p.PedidoProductos)
                    .HasForeignKey(pp => pp.PedidoId);

                // Relación: PedidoProducto pertenece a un Pedido y a un Producto
                entity.HasOne(pp => pp.Producto)
                    .WithMany(p => p.PedidoProductos)
                    .HasForeignKey(pp => pp.ProductoId);
            });



            ///////////////////////////////////////////
            //          TABLAS Y RELACIONES          //
            ///////////////////////////////////////////

            //////////////////////////////
            //          USUARIO         //
            //////////////////////////////
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Id es la clave primaria y se genera automáticamente
                entity.HasKey(u => u.IdUsuario);
                entity.Property(u => u.IdUsuario)
                    .ValueGeneratedOnAdd();

                // Nombre es obligatorio y tiene una longitud máxima de 100 caracteres
                entity.Property(u => u.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                // Apellido es obligatorio y tiene una longitud máxima de 150 caracteres
                entity.Property(u => u.Apellido)
                    .IsRequired()
                    .HasMaxLength(150);

                // Email es obligatorio, debe ser único y tiene un formato de email válido
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                // FechaCreacion tiene un valor por defecto de la fecha actual
                entity.Property(u => u.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");
            });

            //////////////////////////////
            //          PEDIDO          //
            //////////////////////////////
            modelBuilder.Entity<Pedido>(entity =>
            {
                // Id es la clave primaria y se genera automáticamente
                entity.HasKey(p => p.IdPedido);
                entity.Property(p => p.IdPedido)
                    .ValueGeneratedOnAdd();

                // FechaCreacion tiene un valor por defecto de la fecha actual
                entity.Property(p => p.FechaPedido)
                    .HasDefaultValueSql("GETDATE()");

                // FechaCreacion tiene un valor por defecto de la fecha actual
                entity.Property(p => p.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");

                // UsuarioId es obligatorio y debe existir en la tabla Usuarios
                entity.Property(p => p.UsuarioId)
                    .IsRequired();

                // Relación: Pedido pertenece a un Usuario
                entity.HasOne(p => p.Usuario)
                    .WithMany(u => u.Pedidos)
                    .HasForeignKey(p => p.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //////////////////////////////
            //          PRODUCTO        //
            //////////////////////////////
            modelBuilder.Entity<Producto>(entity =>
            {
                // Id es la clave primaria y se genera automáticamente
                entity.HasKey(p => p.IdProducto);
                entity.Property(p => p.IdProducto)
                    .ValueGeneratedOnAdd();

                // Nombre es obligatorio y tiene una longitud máxima de 100 caracteres
                entity.Property(p => p.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                // Descripción es obligatoria y tiene una longitud máxima de 500 caracteres
                entity.Property(p => p.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                // Precio es obligatorio y debe ser mayor que cero
                entity.Property(p => p.Precio)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0.01m);

                //Stock es obligatorio y debe ser mayor o igual a cero
                entity.Property(p => p.Stock)
                    .IsRequired()
                    .HasDefaultValue(0);

                // FechaCreacion tiene un valor por defecto de la fecha actual
                entity.Property(p => p.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");
            });


            // Este método se llama cuando el modelo de la base de datos se está creando

            base.OnModelCreating(modelBuilder);
        }

    }
}


using System.ComponentModel.DataAnnotations;

namespace BACKEND_STORE.Models.ENTITIES
{
    public class Entities  //MODELO DE SE DE DATOS PARA ENTIDADES
    {
        // ==========================================
        // ============ TABLA USUARIO ============
        // ==========================================
        public class Usuario
        {
            [Key]
            public int IdUsuario { get; set; }

            [Required(ErrorMessage = "El campo Nombre es obligatorio")]
            [StringLength(100)]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio")]
            [StringLength(150)]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo Email es obligatorio")]
            [EmailAddress(ErrorMessage = "Formato de email inválido")]
            public string Email { get; set; }

            public DateTime FechaCreacion { get; set; }

            // Relación: Usuario tiene muchos Pedidos
            // Un usuario puede tener muchos pedidos
            public ICollection<Pedido> Pedidos { get; set; }
        }

        // ==========================================
        // ============ TABLA PEDIDOS ============
        // ==========================================
        public class Pedido { 
            [Key]
            public int IdPedido { get; set; }

            [Required(ErrorMessage = "El campo FechaPedido es obligatorio")]
            public DateTime FechaPedido { get; set; }

            [Required(ErrorMessage = "El campo UsuarioId es obligatorio")]
            public int UsuarioId { get; set; }

            public DateTime FechaCreacion { get; set; }

            // Relación: Pedido pertenece a un Usuario
            public Usuario Usuario { get; set; }
            // Relación: Pedido puede tener muchos Productos
            public ICollection<PedidoProducto> PedidoProductos { get; set; }
        }

        // ==========================================
        // ============ TABLA PRODUCTO ============
        // ==========================================
        public class Producto
        {
            [Key]
            public int IdProducto { get; set; }

            [Required(ErrorMessage = "El campo Nombre es obligatorio")]
            [StringLength(100)]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Descripción es obligatorio")]
            [StringLength(500)]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "El campo Precio es obligatorio")]
            [Range(0.01, double.MaxValue, ErrorMessage = "El Precio debe ser mayor que cero")]
            public decimal Precio { get; set; }

            [Required(ErrorMessage = "El campo Stock es obligatorio")] 
            [Range(0, int.MaxValue, ErrorMessage = "El Stock no puede ser negativo")]
            public int Stock { get; set; }

            public DateTime FechaCreacion { get; set; }


            // Relación: muchos a muchos con cantidad
            public ICollection<PedidoProducto> PedidoProductos { get; set; }
        }

        // ==========================================================
        // ============ TABLA INTERMEDIA PEDIDO PRODUCTO ============
        // ==========================================================
        public class PedidoProducto
        {

            [Required(ErrorMessage = "El campo PedidoId es obligatorio")]
            public int PedidoId { get; set; }

            [Required(ErrorMessage = "El campo ProductoId es obligatorio")]
            public int ProductoId { get; set; }

            [Required(ErrorMessage = "El campo Cantidad es obligatorio")]
            [Range(1, int.MaxValue, ErrorMessage = "La Cantidad debe ser mayor que cero")]
            public int Cantidad { get; set; }

            // Relación: PedidoProducto pertenece a un Pedido
            public Pedido Pedido { get; set; }
            // Relación: PedidoProducto pertenece a un Producto
            public Producto Producto { get; set; }
        }

    }
}

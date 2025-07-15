namespace BACKEND_STORE.Models.ENTITIES
{
    // ==========================================
    // ============ TABLA ORDENES ============
    // ==========================================

    public class Orders
    {
        public int Id_Order { get; set; } // ID de la orden
        public int customer_id { get; set; } // ID del cliente asociado a la orden
        public Customers Customer { get; set; } // Navegación
        public int user_id { get; set; } // ID del usuario que creó la orden
        public Users User { get; set; } // Navegación
        public DateTime order_date { get; set; } // Fecha de la orden
        public decimal total_amount { get; set; } // Monto total de la orden
        public string status { get; set; } // Estado de la orden (pendiente, completada, cancelada)
        public DateTime created_at { get; set; } // Fecha de creación de la orden
        public DateTime? updated_at { get; set; } // Fecha de actualización de la orden
        public DateTime? deleted_at { get; set; } // Fecha de eliminación de la orden
        public bool is_active { get; set; } // Estado de la orden (activa/inactiva)
        public string created_by { get; set; } // Usuario que creó la orden
        public string updated_by { get; set; } // Usuario que actualizó la orden
        public string deleted_by { get; set; } // Usuario que eliminó la orden

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Relación con los detalles de la orden
    }
}

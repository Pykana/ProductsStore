namespace BACKEND_STORE.Models.ENTITIES
{

    // ==========================================
    // ============ TABLA CLIENTES ============
    // ==========================================

    public class Customers
    {
        public int Id_Customer { get; set; } // ID del cliente
        public string customer_name { get; set; } // Nombre del cliente
        public string customer_email { get; set; } // Email del cliente
        public int customer_phone { get; set; } // Teléfono del cliente
        public string customer_address { get; set; } // Dirección del cliente
        public DateTime created_at { get; set; } // Fecha de creación del cliente
        public DateTime? updated_at { get; set; } // Fecha de actualización del cliente
        public DateTime? deleted_at { get; set; } // Fecha de eliminación del cliente
        public bool is_active { get; set; } // Estado del cliente (activo/inactivo)
        public string created_by { get; set; } // Usuario que creó el cliente
        public string updated_by { get; set; } // Usuario que actualizó el cliente
        public string deleted_by { get; set; } // Usuario que eliminó el cliente
        public ICollection<Customer_User> CustomerUsers { get; set; } = new List<Customer_User>(); // Relación
    }
}

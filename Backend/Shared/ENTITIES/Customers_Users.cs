namespace BACKEND_STORE.Shared.ENTITIES
{
    // ==================================================================
    // ============ TABLA INTERMEDIA CLIENTE - USUARIO ============
    // ==================================================================

    public class Customer_User
    {
        public int customer_id { get; set; } // ID del cliente
        public int user_id { get; set; } // ID del usuario
        public DateTime created_at { get; set; } // Fecha de creación de la relación
        public DateTime? updated_at { get; set; } // Fecha de actualización de la relación
        public DateTime? deleted_at { get; set; } // Fecha de eliminación de la relación
        public bool is_active { get; set; } // Estado de la relación (activa/inactiva)
        public string created_by { get; set; } // Usuario que creó la relación
        public string updated_by { get; set; } // Usuario que actualizó la relación
        public string deleted_by { get; set; } // Usuario que eliminó la relación

        public Customers Customer { get; set; }  // Navegación
        public Users User { get; set; }          // Navegación
    }
}

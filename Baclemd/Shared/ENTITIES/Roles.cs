namespace BACKEND_STORE.Shared.ENTITIES
{
    // ==========================================
    // ============ TABLA ROLES ============
    // ==========================================

    public class Roles
    {
        public int Id_Role { get; set; } // ID del rol
        public string role_name { get; set; } // Nombre del rol
        public string role_description { get; set; } // Descripción del rol
        public DateTime created_at { get; set; } // Fecha de creación del rol
        public DateTime? updated_at { get; set; } // Fecha de actualización del rol
        public DateTime? deleted_at { get; set; } // Fecha de eliminación del rol
        public bool is_active { get; set; } // Estado del rol (activo/inactivo)
        public string created_by { get; set; } // Usuario que creó el rol
        public string updated_by { get; set; } // Usuario que actualizó el rol
        public string deleted_by { get; set; } // Usuario que eliminó el rol

        public ICollection<Users> Users { get; set; } = new List<Users>(); // Relación con usuarios
    }
}

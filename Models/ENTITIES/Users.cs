namespace BACKEND_STORE.Models.ENTITIES
{
    // ==========================================
    // ============ TABLA USUARIOS ============
    // ==========================================

    public class Users
    {
        public int Id_User { get; set; } // ID del usuario
        public string username { get; set; } // Nombre de usuario
        public string password { get; set; } // Contraseña del usuario
        public string email { get; set; } // Email del usuario
        public string name { get; set; } // Nombre del usuario
        public string lastname { get; set; } // Apellido del usuario
        public DateTime created_at { get; set; } // Fecha de creación del usuario
        public DateTime? updated_at { get; set; } // Fecha de actualización del usuario
        public DateTime? deleted_at { get; set; } // Fecha de eliminación del usuario
        public bool is_active { get; set; } // Fecha de eliminación del usuario
        public string created_by { get; set; } // Usuario que creó el usuario
        public string updated_by { get; set; } // Usuario que actualizó el usuario
        public string deleted_by { get; set; } // Usuario que eliminó el usuario
        public ICollection<Customer_User> CustomerUsers { get; set; } = new List<Customer_User>();  // Relación
        public int RoleId { get; set; } // clave foránea visible
        public Roles Role { get; set; } // Relación con roles   
    }
}

namespace BACKEND_STORE.Models
{
    public class Role
    {
        public class RolePost
        {
            public int Id_Role { get; set; } // ID del rol
            public string role_name { get; set; } // Nombre del rol
            public string role_description { get; set; } // Descripción del rol
        }

        public class RoleRequestPost
        {
            public string role_name { get; set; }
            public string role_description { get; set; }
            public bool IsActive { get; set; }
            public string create_by { get; set; }
        }

        public class RoleRequestPut
        {
            public int Id_Role { get; set; } // ID del rol
            public string role_name { get; set; } // Nombre del rol
            public string role_description { get; set; } // Descripción del rol
            public bool IsActive { get; set; }
            public string update_by { get; set; }
        }
    }
}

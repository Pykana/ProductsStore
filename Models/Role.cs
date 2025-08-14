using System.ComponentModel.DataAnnotations;

namespace BACKEND_STORE.Models
{
    public class Role
    {
        public class RolePost
        {
            [Required(ErrorMessage = "El ID del rol es obligatorio")]
            [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo")]
            public int Id_Role { get; set; } // ID del rol

            [Required(ErrorMessage = "El nombre del rol es obligatorio")]
            public string role_name { get; set; } // Nombre del rol

            [Required(ErrorMessage = "La descripción del rol es obligatoria")]
            public string role_description { get; set; } // Descripción del rol
        }

        public class RoleRequestPost
        {
            [Required(ErrorMessage = "El nombre del rol es obligatorio")]
            public string role_name { get; set; }

            [Required(ErrorMessage = "La descripción del rol es obligatoria")]
            public string role_description { get; set; }

            [Required(ErrorMessage = "El estado del rol es obligatorio")]
            public bool IsActive { get; set; }

            [Required(ErrorMessage = "El creador del rol es obligatorio")]
            public string create_by { get; set; }
        }

        public class RoleRequestPut
        {
            [Required(ErrorMessage = "El ID del rol es obligatorio")]
            public int Id_Role { get; set; } // ID del rol
            [Required(ErrorMessage = "El nombre del rol es obligatorio")]
            public string role_name { get; set; } // Nombre del rol

            [Required(ErrorMessage = "La descripción del rol es obligatoria")]
            public string role_description { get; set; } // Descripción del rol

            [Required(ErrorMessage = "El estado del rol es obligatorio")]
            public bool IsActive { get; set; }

            [Required(ErrorMessage = "El actualizador del rol es obligatorio")]
            public string update_by { get; set; }
        }
    }
}

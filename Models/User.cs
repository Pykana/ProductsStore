using System.ComponentModel.DataAnnotations;

namespace BACKEND_STORE.Models
{
    public class User
    {
        public class userDTO
        {
            public string user_name { get; set; }
            public string user_email { get; set; }
            public string role { get; set; }
            public string pass { get; set; }
        }

        public class UserRequestPost
        {
            [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
            public string user_name { get; set; }

            [Required(ErrorMessage = "El correo electrónico es obligatorio")]
            public string name { get; set; }

            [Required(ErrorMessage = "El apellido es obligatorio")]
            public string lastname { get; set; }

            [Required(ErrorMessage = "El correo electrónico es obligatorio")]
            public string user_email { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            public string user_password { get; set; }

            [Required(ErrorMessage = "El creador del usuario es obligatorio")]
            public string userEdit { get; set; }

            [Required(ErrorMessage = "El ID del rol es obligatorio")]
            [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo")]
            public int Id_Role { get; set; }
        }

        public class UserRequestPut
        {
            [Required(ErrorMessage = "El ID del usuario es obligatorio")]
            public int Id_User { get; set; }

            [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
            public string user_name { get; set; }

            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string user_email { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            public string user_password { get; set; }

            [Required(ErrorMessage = "El ID del rol es obligatorio")]
            public int Id_Role { get; set; }

            [Required(ErrorMessage = "El estado del usuario es obligatorio")]
            public bool IsActive { get; set; }

            [Required(ErrorMessage = "El actualizador del usuario es obligatorio")]
            public string update_by { get; set; }

            [Required(ErrorMessage = "El campo userEdit es necesario")]
            public string userEdit { get; set; }
        }

        public class UserLogin
        {
            [Required(ErrorMessage = "El correo electrónico es obligatorio")]
            public string user_email { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            public string user_password { get; set; }
        }


    }
}

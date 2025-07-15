using System.ComponentModel.DataAnnotations;

namespace BACKEND_STORE.Models.GET
{
    public class Login
    {
        public class login
        {
            [Required(ErrorMessage = "El campo Nombre de Usuario es obligatorio")]
            public string username { get; set; }
            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            public string password { get; set; }
        }

        public class restore
        {
            [Required(ErrorMessage = "El campo Email es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato del email es incorrecto")]
            public string email { get; set; }
        }

    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace BACKEND_STORE.Modules.Login.Models
{
    public class Login
    {
        public class registerPOST
        {
            [Required(ErrorMessage ="El campo Nombre es obligatorio")]
            public string name { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio")]
            public string lastname { get; set; }

            [Required(ErrorMessage = "El campo Nombre de Usuario es obligatorio")]
            public string username { get; set; }

            [Required(ErrorMessage = "El campo Email es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato del email es incorrecto")]
            public string email { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            public string password { get; set; }
        }

        public class changePasswordPOST
        {
            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            [MinLength(10,ErrorMessage ="El campo contraseña necesita al menos 10 caracteres")]
            public string password { get; set; }
            [Required(ErrorMessage = "El campo Confirmar Contraseña es obligatorio")]
            [Compare("password", ErrorMessage = "Las contraseñas no coinciden")]
            public string confirmPassword { get; set; }
        }


        public class login
        {
            [Required(ErrorMessage = "El campo Nombre de Usuario es obligatorio")]
            public string username { get; set; }
            [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
            public string password { get; set; }
        }

        public class restoreGET
        {
            [Required(ErrorMessage = "El campo Email es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato del email es incorrecto")]
            public string email { get; set; }
        }

        public class LoginResponse
        {
            public string idUser { get; set; }
            public string user { get; set; }
            public string roleid { get; set; }
            public string token { get; set; }
            public bool success { get; set; }
        }
    }
}

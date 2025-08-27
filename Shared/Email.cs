using System.Net;
using System.Net.Mail;
using System.Text;

namespace BACKEND_STORE.Shared
{
    public class Email
    {
        private readonly string _HostBase;
        private readonly string _UsuarioBase;
        private readonly string _EmailBase;
        private readonly string _PasswordBase;
        private readonly string _NameBase;
        private readonly string _PuertoBase;
        private readonly string _SslBase;

        public Email(IConfiguration config) {
            _HostBase = config["Secrets:STORE_KEY_EMAIL_HOST"] ?? string.Empty;
            _EmailBase = config["Secrets:STORE_KEY_EMAIL_BASE"] ?? string.Empty;
            _UsuarioBase = config["Secrets:STORE_KEY_EMAIL_USER"] ?? string.Empty;
            _PasswordBase = config["Secrets:STORE_KEY_EMAIL_PASSW"] ?? string.Empty;
            _NameBase = config["Secrets:STORE_KEY_EMAIL_EMAIL_NAME"] ?? string.Empty;
            _PuertoBase = config["Secrets:STORE_KEY_EMAIL_PORT"] ?? string.Empty;
            _SslBase = config["Secrets:STORE_KEY_EMAIL_SSL"] ?? "true";
        }

        public bool SendMail(string correo, string data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_HostBase) || string.IsNullOrWhiteSpace(_EmailBase) ||
                    string.IsNullOrWhiteSpace(_UsuarioBase) || string.IsNullOrWhiteSpace(_PasswordBase))
                {
                    return false;
                }

                int puerto = 587;
                bool usarSsl = true;

                // Validación de puerto
                if (!string.IsNullOrEmpty(_PuertoBase))
                {
                    int.TryParse(_PuertoBase, out puerto);
                }

                // Validación de SSL
                if (!string.IsNullOrEmpty(_SslBase))
                {
                    bool.TryParse(_SslBase, out usarSsl);
                }

                // Configurar cliente SMTP
                using (SmtpClient smtp = new SmtpClient(_HostBase, puerto))
                {
                    smtp.EnableSsl = usarSsl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_UsuarioBase, _PasswordBase);

                    using (MailMessage mensaje = new MailMessage())
                    {
                        // Asignar emisor
                        mensaje.From = new MailAddress(_EmailBase, $"Recuperacion Contraseña {_NameBase}", Encoding.UTF8);
                        mensaje.To.Add(new MailAddress(correo));

                        // Contenido del mensaje
                        mensaje.Subject = "Recuperacion Contraseña";
                        mensaje.Body = $"Codigo: {data}";
                        mensaje.IsBodyHtml = true;
                        mensaje.Priority = MailPriority.Normal;

                        // Enviar correo
                        smtp.Send(mensaje);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                return false;
            }
        }






    }
}

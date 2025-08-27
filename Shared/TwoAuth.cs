using Net.Codecrete.QrCodeGenerator;
using OtpNet;

namespace BACKEND_STORE.Shared
{
    public class TwoAuth
    {
        public bool ValidateCode(string secretBase32, string CodigoUsuario)
        {
            var secretbytes = Base32Encoding.ToBytes(secretBase32);

            var topt = new Totp(secretbytes);

            string CodigoEsperado = topt.ComputeTotp();
            return CodigoEsperado == CodigoUsuario;
        }

        public string GenerarQrBase64(string uri)
        {
            var qr = QrCode.EncodeText(uri, QrCode.Ecc.Medium);
            byte[] png = qr.ToPng(20, 4);
            return Convert.ToBase64String(png);
        }
    }
}

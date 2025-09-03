using Net.Codecrete.QrCodeGenerator;
using SkiaSharp;

namespace BACKEND_STORE.Shared
{
    public static class QrCodeBitmapExtensions
    {
        /// <summary>
        /// Returns a bitmap image of this QR Code, with the specified module scale and border modules.
        /// </summary>
        /// <param name="qr">QR Code a convertir en imagen</param>
        /// <param name="scale">Tamaño de cada cuadrado (px)</param>
        /// <param name="border">Módulos de borde (margen)</param>
        /// <returns>Un objeto <see cref="SKBitmap"/></returns>
        public static SKBitmap ToBitmap(this QrCode qr, int scale, int border)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            if (border < 0)
                throw new ArgumentOutOfRangeException(nameof(border));

            int size = qr.Size;
            int dim = (size + border * 2) * scale;
            var bitmap = new SKBitmap(dim, dim);

            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                using (var paint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Fill })
                {
                    for (int y = 0; y < size; y++)
                    {
                        for (int x = 0; x < size; x++)
                        {
                            if (qr.GetModule(x, y))
                            {
                                int rx = (x + border) * scale;
                                int ry = (y + border) * scale;
                                canvas.DrawRect(rx, ry, scale, scale, paint);
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Devuelve el QR en formato PNG como <c>byte[]</c>.
        /// </summary>
        /// <param name="qr">QR Code</param>
        /// <param name="scale">Tamaño de cada cuadrado (px)</param>
        /// <param name="border">Módulos de borde (margen)</param>
        /// <returns>Bytes de un archivo PNG</returns>
        public static byte[] ToPng(this QrCode qr, int scale, int border)
        {
            using (var bitmap = qr.ToBitmap(scale, border))
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }
    }
}

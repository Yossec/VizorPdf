using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.Content;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VizorPdf.helper
{
    internal class PdfToImageConverter
    {
        public async Task<ImageSource> ConvertWithPdfium(string pdfPath, int pageNumber = 0)
        {
            try
            {
                using var stream = new FileStream(pdfPath, FileMode.Open);
                using var document = PdfiumViewer.PdfDocument.Load(stream);

                var width = (int)document.PageSizes[pageNumber].Width * 2;
                var height = (int)document.PageSizes[pageNumber].Height * 2;

                using var bitmap = new SKBitmap(width, height);
                using var canvas = new SKCanvas(bitmap);

                canvas.Clear(SKColors.White);

                // Renderizar página
                using var image = document.Render(pageNumber, width, height, 300, 300, true);
                using var skImage = SKImage.FromBitmap(bitmap);
                using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
                using var memStream = new MemoryStream();
                data.SaveTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);

                return ImageSource.FromStream(() => memStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}

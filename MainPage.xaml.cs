using VizorPdf.helper;

namespace VizorPdf
{
    public partial class MainPage : ContentPage
    {
        private readonly PdfToImageConverter _pdfConverter = new PdfToImageConverter();
        public MainPage()
        {
            InitializeComponent();
            LoadPdfAsImage();
        }
        private async void LoadPdfAsImage()
        {
            try
            {
                // Cambia esta ruta por la ubicación de tu PDF
                string pdfPath = "C:/Users/HP/Desktop/firma/prueba.pdf";

                // Convertir la primera página (página 0) a imagen
                var imageSource = await _pdfConverter.ConvertWithPdfium(pdfPath, 0);

                if (imageSource != null)
                {
                    pdfImageView.Source = imageSource;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo convertir el PDF a imagen", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }

    }
}

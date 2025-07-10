#if MACCATALYST
using CoreGraphics;
using Foundation;
using PdfKit;
using UIKit;
#endif

namespace VizorPdf
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

#if MACCATALYST
            pdfImageView.Source = RenderPdfPageToImage("C:\\Users\\HP\\Desktop\\firma\\prueba.pdf", 0);
#endif
        }

#if MACCATALYST
        public ImageSource RenderPdfPageToImage(string pdfPath, int pageIndex)
        {
            using var url = new NSUrl(pdfPath, false);
            using var doc = CGPDFDocument.FromFile(url.Path);

            if (doc == null || doc.Pages < pageIndex + 1)
                return null;

            var page = doc.GetPage(pageIndex + 1); // 1-based index
            var pageRect = page.GetBoxRect(CGPDFBox.Media);
            var width = (int)pageRect.Width;
            var height = (int)pageRect.Height;

            UIGraphics.BeginImageContext(new CGSize(width, height));
            var context = UIGraphics.GetCurrentContext();

            context.SetFillColor(UIColor.White.CGColor);
            context.FillRect(new CGRect(0, 0, width, height));
            context.TranslateCTM(0, height);
            context.ScaleCTM(1, -1);

            context.DrawPDFPage(page);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return ImageSource.FromStream(() => image.AsPNG().AsStream());
        }
#endif
    }
}

using Application.Contracts;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Utilities.Services
{
    public class PDFOperations: IPDFOperations
    {
        private readonly IConverter _converter;
        public PDFOperations(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] ConvertHTMLToPDF(string htmlTemplate, string styleSheetPath = null)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Margins = new MarginSettings { Top = 10 }
                },

                Objects = {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = htmlTemplate,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet =  styleSheetPath },
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },         
                    }
                }
            }; 
            
            return _converter.Convert(doc);
        }
    }
}


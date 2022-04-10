using Application.Contracts;
using CoreHtmlToImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class ImageOperations : IImageOperations
    {
        public byte[] ConvertHTMLToImage(string htmlTemplate, string format)
        {
            var converter = new HtmlConverter();

            var imageFormat = format == "png" ? ImageFormat.Png : ImageFormat.Jpg;

            return converter.FromHtmlString(htmlTemplate, format: imageFormat);
        }
    }
}

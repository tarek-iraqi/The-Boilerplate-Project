using Helpers.Enums;
using Helpers.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileValidator
    {
        Result IsValidFile(byte[] file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        Result IsValidFile(MemoryStream file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        Result IsValidFile(IFormFile file, int fileSizeInMega, FileExtensions[] allowedExtensions);
    }
}

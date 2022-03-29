using Helpers.Enums;
using Helpers.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Application.Interfaces
{
    public interface IFileValidator
    {
        Result IsValidFile(byte[] file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        Result IsValidFile(MemoryStream file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        Result IsValidFile(IFormFile file, int fileSizeInMega, FileExtensions[] allowedExtensions);
    }
}

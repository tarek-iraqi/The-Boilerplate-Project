using Helpers.BaseModels;
using Helpers.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Application.Contracts
{
    public interface IFileValidator
    {
        OperationResult IsValidFile(byte[] file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        OperationResult IsValidFile(MemoryStream file, int fileSizeInMega, FileExtensions[] allowedExtensions);
        OperationResult IsValidFile(IFormFile file, int fileSizeInMega, FileExtensions[] allowedExtensions);
    }
}

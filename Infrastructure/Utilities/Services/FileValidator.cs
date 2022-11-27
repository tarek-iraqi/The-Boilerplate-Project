using Application.Contracts;
using FileSignatures;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Enums;
using Helpers.Localization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Utilities.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly IFileFormatInspector _fileFormatInspector;

        public FileValidator(IFileFormatInspector fileFormatInspector)
        {
            _fileFormatInspector = fileFormatInspector;
        }
        public OperationResult IsValidFile(byte[] file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            using (var stream = new MemoryStream(file))
                return IsValidFile(stream, fileSizeInMega, allowedExtensions);
        }

        public OperationResult IsValidFile(MemoryStream file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            var isValidFileLength = file.Length <= fileSizeInMega * 1024 * 1024;

            if (!isValidFileLength) return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                OperationError.Add("File Size", LocalizationKeys.NotValidFileSize));

            var format = _fileFormatInspector.DetermineFileFormat(file);
            var isValidFileType = format != null && allowedExtensions.Any(ext => ext.ToString() == format.Extension.ToLower());

            if (!isValidFileType) return OperationResult.Fail(ErrorStatusCodes.InvalidAttribute,
                OperationError.Add("File Type", LocalizationKeys.NotValidFileType));

            return OperationResult.Success();
        }

        public OperationResult IsValidFile(IFormFile file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                return IsValidFile(stream, fileSizeInMega, allowedExtensions);
            }
        }
    }
}

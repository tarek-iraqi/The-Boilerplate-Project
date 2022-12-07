using Application.Contracts;
using FileSignatures;
using Helpers.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Utilities.Services;

public class LocalServerUpload : IUpload
{
    private readonly string _directoryPath;
    private readonly IFileFormatInspector _fileFormatInspector;

    public string BaseUrl { get; }

    public LocalServerUpload(IWebHostEnvironment env,
        IApplicationConfiguration configuration,
        IFileFormatInspector fileFormatInspector)
    {
        _directoryPath = Path.Combine(env.WebRootPath, KeyValueConstants.UploadFolder);
        _fileFormatInspector = fileFormatInspector;
        BaseUrl = $"{configuration.GetAppSettings().Api_URL}/{KeyValueConstants.UploadFolder}";
    }

    public async Task<string> UploadFile(MemoryStream file, string fileName)
    {
        var format = _fileFormatInspector.DetermineFileFormat(file);

        Directory.CreateDirectory(_directoryPath);

        fileName = $"{fileName}.{format.Extension.ToLower()}";
        string filePath = Path.Combine(_directoryPath, fileName);

        await File.WriteAllBytesAsync(filePath, file.ToArray());

        return fileName;
    }

    public async Task<string> UploadFile(byte[] file, string fileName)
    {
        using (var stream = new MemoryStream(file))
            return await UploadFile(stream, fileName);
    }

    public async Task<string> UploadFile(IFormFile file, string fileName)
    {
        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            return await UploadFile(stream, fileName);
        }
    }

    public async Task<bool> DeleteFile(string fileName)
    {
        var path = Path.Combine(_directoryPath, fileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        else return false;
    }
}
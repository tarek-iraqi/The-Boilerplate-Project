using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IUpload
{
    Task<string> UploadFile(byte[] file, string fileName);
    Task<string> UploadFile(MemoryStream file, string fileName);
    Task<string> UploadFile(IFormFile file, string fileName);
    Task<bool> DeleteFile(string fileName);
    string BaseUrl { get; }
}
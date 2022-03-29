using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces;
using FileSignatures;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class AmazonUpload : IUpload
    {
        public string BaseUrl { get; }
        private readonly IAmazonS3 _s3Client;
        private readonly IApplicationConfiguration _configuration;
        private readonly IFileFormatInspector _fileFormatInspector;

        public AmazonUpload(IAmazonS3 s3Client,
            IApplicationConfiguration configuration,
            IFileFormatInspector fileFormatInspector)
        {
            _s3Client = s3Client;
            _configuration = configuration;
            _fileFormatInspector = fileFormatInspector;
            BaseUrl = _configuration.GetAmazonSettings().AWS_FILE_PATH;
        }

        public async Task<string> UploadFile(MemoryStream file, string fileName)
        {
            var format = _fileFormatInspector.DetermineFileFormat(file);

            fileName = $"{fileName}.{format.Extension.ToLower()}";

            var request = new PutObjectRequest
            {
                BucketName = _configuration.GetAmazonSettings().AWS_BUCKET,
                CannedACL = S3CannedACL.PublicRead,
                Key = fileName
            };

            request.InputStream = file;
            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK) return fileName;
            else return null;
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
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _configuration.GetAmazonSettings().AWS_BUCKET,
                Key = fileName
            };

            var result = await _s3Client.DeleteObjectAsync(request);

            return result.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}

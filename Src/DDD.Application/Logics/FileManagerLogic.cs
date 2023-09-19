using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DDD.Application.ILogics;
using Microsoft.AspNetCore.Http;

namespace DDD.Application.Logics
{
    public class FileManagerLogic : IFileManagerLogic
    {
        private readonly BlobServiceClient _blobServiceClient;
        public FileManagerLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<byte[]> Get(string FileName, string containerName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(FileName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task Upload(IFormFile file, string containerName, string fileName=null)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
            var createResponse = await blobContainer.CreateIfNotExistsAsync();
            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                await blobContainer.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            string blobName = fileName ??  file.FileName;
            var blobClient = blobContainer.GetBlobClient(blobName);
            using (var fileStream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

        }
    }
}

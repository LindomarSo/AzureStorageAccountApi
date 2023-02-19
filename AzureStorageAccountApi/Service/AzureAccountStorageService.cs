using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureStorageAccountApi.Model;
using AzureStorageAccountApi.Service.Interface;
using AzureStorageAccountApi.Settings;

namespace AzureStorageAccountApi.Service
{
    public class AzureAccountStorageService : IAzureAccountStorageService
    {
        private StorageAccountConnection _connection;
        private BlobServiceClient _blobService;


        public AzureAccountStorageService(StorageAccountConnection connection)
        {
            _connection = connection;
            // Permite manipular os recursos do Azure Storage Accounts e os containers de blob.
            _blobService = new BlobServiceClient(_connection.StorageConnectionString);
        }

        public async Task<object> AddBlobAsync(FileModel file)
        {
            if (file.File.Length <= 0)
            {
                return null;
            }

            var fileName = Guid.NewGuid().ToString();

            Stream image = file.File.OpenReadStream();

            // Permite manipular os containers do armazenamento do Azure e seus blobs
            BlobContainerClient containerClient = _blobService.GetBlobContainerClient(file.ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            // Permite manipular os blobs do Storage Account
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(image);

            return new { message = "Nome do arguivo: " + fileName };
        }

        public async Task<string> CreateContainerAsync(string containerName)
        {
            var containerClient = await _blobService.CreateBlobContainerAsync(containerName, PublicAccessType.Blob);

            return containerClient.Value.Name;
        }

        public async Task<IReadOnlyCollection<string>> GetAllBlobsAsync(string containerName)
        {
            var results = new List<string>();
            BlobContainerClient blobContainerClient = _blobService.GetBlobContainerClient(containerName);

            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                results.Add(
                    Flurl.Url.Combine(
                        blobContainerClient.Uri.AbsoluteUri,
                        blobItem.Name
                   ));
            }

            return results;
        }

        public AsyncPageable<BlobContainerItem> GetAllContainersAsync()
        {
            var response = _blobService.GetBlobContainersAsync();

            return response;
        }

        public async Task<bool> DeleteContainerAsync(string containerName)
        {
            BlobContainerClient blobContainer = _blobService.GetBlobContainerClient(containerName);
            var response = await blobContainer.DeleteIfExistsAsync();

            return response.Value;
        }

        public bool DeleteBlob(string containerName, string blobName)
        {
            BlobContainerClient blobContainer = _blobService.GetBlobContainerClient(containerName);

            BlobClient blobClient = blobContainer.GetBlobClient(blobName);

            var response = blobClient.DeleteIfExists();

            return response.Value;

        }
    }
}

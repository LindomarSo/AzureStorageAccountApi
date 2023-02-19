using Azure.Storage.Blobs.Models;
using Azure;
using AzureStorageAccountApi.Model;

namespace AzureStorageAccountApi.Service.Interface
{
    public interface IAzureAccountStorageService
    {
        Task<IReadOnlyCollection<string>> GetAllBlobsAsync(string containerName);
        AsyncPageable<BlobContainerItem> GetAllContainersAsync();
        Task<string> CreateContainerAsync(string containerName);
        Task<object> AddBlobAsync(FileModel file);
        Task<bool> DeleteContainerAsync(string containerName);
        bool DeleteBlob(string containerName, string blobName);
    }
}

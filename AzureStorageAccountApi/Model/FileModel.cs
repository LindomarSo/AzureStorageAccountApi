namespace AzureStorageAccountApi.Model
{
    public class FileModel
    {
        public string ContainerName { get; set; }
        public IFormFile File { get; set; }
    }
}

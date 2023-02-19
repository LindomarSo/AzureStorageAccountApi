using AzureStorageAccountApi.Model;
using AzureStorageAccountApi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageAccountApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureStorageAccountController : ControllerBase
    {
        private readonly IAzureAccountStorageService _azureAccountStorageService;

        public AzureStorageAccountController(IAzureAccountStorageService azureAccountStorageService)
        {
            _azureAccountStorageService = azureAccountStorageService;
        }

        /// <summary>
        /// Pegar lista de blobs
        /// </summary>
        /// <returns></returns>
        [HttpGet("blobItem/{containerName}")]
        public async Task<ActionResult<IReadOnlyCollection<string>>> GetBlobsAsync(string containerName)
        {
            return Ok(await _azureAccountStorageService.GetAllBlobsAsync(containerName));
        }

        /// <summary>
        /// Método responsável pela criação de containers
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateContainerAsync(string containerName)
        {
            return Ok(await _azureAccountStorageService.CreateContainerAsync(containerName));
        }

        /// <summary>
        /// Método responsável por adicionar um novo arquivo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("addblob")]
        public async Task<ActionResult<object>> AddBlobAsync([FromForm]FileModel file)
        {
            return Ok(await _azureAccountStorageService.AddBlobAsync(file));
        }

        /// <summary>
        /// Pegar containers
        /// </summary>
        /// <returns></returns>
        [HttpGet("containers")]
        public IActionResult GetContainers()
        {
            return Ok(_azureAccountStorageService.GetAllContainersAsync());
        }

        /// <summary>
        /// Método responsável por deletar um container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        [HttpDelete("containerName/{containerName}")]
        public async Task<ActionResult<bool>> DeleteContainerAsync(string containerName)
        {
            return Ok(await _azureAccountStorageService.DeleteContainerAsync(containerName));
        }

        /// <summary>
        /// Método responsável por deletar um container
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        [HttpDelete("containerName/{containerName}/blobName/{blobName}")]
        public ActionResult<bool> DeleteBlob(string containerName, string blobName)
        {
            return Ok(_azureAccountStorageService.DeleteBlob(containerName, blobName));
        }
    }
}

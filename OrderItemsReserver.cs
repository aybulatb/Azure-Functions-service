using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

namespace Aibulat.Function
{
    public static class OrderItemsReserver
    {
        private const string ConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=aibulatstorage;AccountKey=t6g2PWKfwmxq72PjkH0o/QZAVpj7CJJd2Y6/79LxTYgAdfB6UplJ5EzZ6TYzD75Q5t+3AyDeAHTr+ASt61Hk+g==;EndpointSuffix=core.windows.net";
        private const string ContainerName = "aibulatcontainer";

        [FunctionName("OrderItemsReserver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var blobServiceClient = new BlobServiceClient(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            var fileName = "blob_" + DateTime.Now.ToString("HH:mm:ss") + ".json";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            req.Body.Position = 0;

            await blobClient.UploadAsync(req.Body);

            return new OkObjectResult(blobClient);
        }
    }
}

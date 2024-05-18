using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Starly.CrossCutting.Azure;

public class AzureBlobClient
{
    private readonly string _connectionString;

    public AzureBlobClient(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Upload(Stream content, string blobName, string containerName)
    {
        var blobClient = await CreateBlobClient(containerName, blobName);
        await blobClient.UploadAsync(content, true);
    }

    public async Task Delete(string blobName, string containerName)
    {
        var blobClient = await CreateBlobClient(containerName, blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    private async Task<BlobClient> CreateBlobClient(string containerName, string blobName)
    {
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);        
        var blobClient = containerClient.GetBlobClient(blobName);
        return blobClient;
    }
}
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BandLabAssesment.Services;

public class ImageUploader
{
    private readonly BlobContainerClient _containerClient;

    private readonly StorageTransferOptions _storageTransferOptions = new()
    {
        InitialTransferSize = Constants.BlobStorage.InitialTransferSize,
        MaximumTransferSize = Constants.MaxFileSize,
    };

    public ImageUploader(BlobContainerClient containerClient) => _containerClient = containerClient;

    public async Task<string> UploadImage(Stream stream, bool IsOriginalImage, string filename, CancellationToken token)
    {
        var folder = IsOriginalImage ? Constants.BlobStorage.OriginalImageFolder : Constants.BlobStorage.ResizedImageFolder;
        var blobClient = _containerClient.GetBlobClient($"{folder}/{filename}");
        await blobClient.UploadAsync(stream, new BlobUploadOptions
        {
            TransferOptions = _storageTransferOptions
        }, token);
        return blobClient.Uri.ToString();
    }
}
using Azure.Storage.Blobs;
using EventBookingApi.Interface;
using EventBookingApi.Misc;
using EventBookingApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class EventImageController : ControllerBase
{
    private readonly IRepository<Guid, EventImage> _imageRepository;
    private readonly IRepository<Guid, Event> _eventRepository;
    private readonly IOtherFunctionalities _other;
    // private readonly BlobContainerClient _containerClinet;

    public EventImageController(IRepository<Guid, EventImage> imageRepository, IRepository<Guid, Event> eventRepository, IOtherFunctionalities other)
    {
        _imageRepository = imageRepository;
        _eventRepository = eventRepository;
        _other = other;
        // var sasUrl = configuration["AzureBlob:ContainerSasUrl"];
        // _containerClinet = new BlobContainerClient(new Uri(sasUrl!));
    }

    [HttpGet("getall")]
    public async Task<IActionResult> getall()
    {
        try
        {
            var images = await _imageRepository.GetAll();
            images = images.Where(img => img?.Event?.EventStatus != EventStatus.Cancelled);
            var result = images.Select(i => new { i.Id, i.FileName, i.EventId, i.FileType });
            return Ok(result);
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }
    [HttpGet("myevent/getall")]
    public async Task<IActionResult> getMyEventImages()
    {
        try
        {
            var userId = _other.GetLoggedInUserId(User);
            var images = await _imageRepository.GetAll();
            images = images.Where(img => img?.Event?.EventStatus != EventStatus.Cancelled && img?.Event?.ManagerId == userId);
            var result = images.Select(i => new { i.Id, i.FileName, i.EventId, i.FileType });
            return Ok(result);
        }
        catch (Exception err)
        {
            return BadRequest(err.Message);
        }
    }
    [HttpDelete("delete/{imageid}")]
    public async Task<IActionResult> deleteImage(Guid imageid)
    {
        try
        {
            var image = await _imageRepository.Delete(imageid);
            return Ok(image);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("upload/{eventId}")]
    public async Task<IActionResult> UploadImage(Guid eventId, IFormFile image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("No image provided.");

        var evt = await _eventRepository.GetById(eventId);
        if (evt == null) return NotFound("Event not found.");

        using var ms = new MemoryStream();
        await image.CopyToAsync(ms);
        var imageModel = new EventImage
        {
            FileName = image.FileName,
            FileType = Path.GetExtension(image.FileName)?.TrimStart('.').ToLower() ?? "webp",
            FileContent = ms.ToArray(),
            UploadedAt = DateTime.UtcNow,
            EventId = eventId
        };

        await _imageRepository.Add(imageModel);

        //azure blob
        //using var stream = image.OpenReadStream();
        //var blobClient = _containerClinet.GetBlobClient(imageModel.Id.ToString());
        //await blobClient.UploadAsync(stream,overwrite:true);

        // return Ok(new
        // {
        //     imageModel.Id,
        //     imageModel.FileName,
        //     Url = $"https://chowdristorageacc022.blob.core.windows.net/images/{imageModel.Id}.{image.FileType}"
        // });
        //end azure blob

        return Ok(new
        {
            imageModel.Id,
            imageModel.FileName,
            Url = $"http://localhost:5279/api/eventimage/download/{imageModel.Id}"
        });
    }
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadImage(Guid id)
    {
        var image = await _imageRepository.GetById(id);
        if (image == null) return NotFound();

        // azure blob
        // var blobClient = _containerClinet?.GetBlobClient(id.ToString());
        // if(await blobClient!.ExistsAsync())
        // {
        //     var downloadInfor = await blobClient.DownloadStreamingAsync();
        //     var contentType = GetMimeType(image.FileType);
        //     return File(downloadInfor.Value.Content, contentType, image.FileName);
        // }
        // return NotFound();
        //end azure

        var contentType = GetMimeType(image.FileType);
        return File(image.FileContent, contentType, image.FileName);
    }

    private string GetMimeType(string ext)
    {
        return ext.ToLower() switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "webp" => "image/webp",
            "gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}
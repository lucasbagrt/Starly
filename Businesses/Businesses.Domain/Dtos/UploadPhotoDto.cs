using Microsoft.AspNetCore.Http;

namespace Businesses.Domain.Dtos;

public class UploadPhotoDto
{
    public int BusinessId { get; set; }
    public List<IFormFile> Photos { get; set; }
}

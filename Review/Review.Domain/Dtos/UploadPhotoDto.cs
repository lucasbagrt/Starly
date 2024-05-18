using Microsoft.AspNetCore.Http;

namespace Review.Domain.Dtos;

public class UploadPhotoDto
{
    public int ReviewId { get; set; }
    public List<IFormFile> Photos { get; set; }
}

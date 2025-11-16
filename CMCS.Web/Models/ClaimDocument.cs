//Referencing list//
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0//

using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models;

public sealed class ClaimDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] public string OriginalFileName { get; set; } = default!;
    [Required] public string StoredFileName { get; set; } = default!;
    public long SizeBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

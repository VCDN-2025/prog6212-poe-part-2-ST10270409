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

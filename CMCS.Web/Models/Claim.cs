using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.Models;

public sealed class Claim
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required, Range(0.5, 24)]
    public double HoursWorked { get; set; }

    [Required, Range(50, 5000)]
    public decimal HourlyRate { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public decimal Total => Math.Round((decimal)HoursWorked * HourlyRate, 2);

    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

    public List<ClaimDocument> Documents { get; set; } = new();
}

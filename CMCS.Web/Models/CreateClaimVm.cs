//Referencing list//
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0//

using System.ComponentModel.DataAnnotations;

namespace CMCS.Web.ViewModels;

public sealed class CreateClaimVm
{
    [Required, DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required, Range(0.5, 24)]
    public double HoursWorked { get; set; }

    [Required, Range(50, 5000)]
    public decimal HourlyRate { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }
}

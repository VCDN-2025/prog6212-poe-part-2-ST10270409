using System;

namespace CMCS.Web.Models;
public class ClaimListVm
{
    public Guid ClaimId { get; set; }
    public string MonthLabel { get; set; } = "";
    public decimal TotalHours { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
}

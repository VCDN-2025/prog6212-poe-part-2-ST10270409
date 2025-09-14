using System;

namespace CMCS.Web.Models;
public class ClaimItemVm
{
    public DateTime WorkDate { get; set; }
    public decimal Hours { get; set; }
    public string Activity { get; set; } = "";
    public decimal HourlyRate { get; set; }
    public decimal LineTotal => Math.Round(Hours * HourlyRate, 2);
}

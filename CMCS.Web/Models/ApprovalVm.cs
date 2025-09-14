using System;

namespace CMCS.Web.Models;
public class ApprovalVm
{
    public Guid ClaimId { get; set; }
    public string Lecturer { get; set; } = "";
    public string MonthLabel { get; set; } = "";
    public string Stage { get; set; } = "ProgrammeCoordinator";
    public string Status { get; set; } = "UnderReview";
}

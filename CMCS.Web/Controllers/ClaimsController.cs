using Microsoft.AspNetCore.Mvc;
using CMCS.Web.Models;
using System;
using System.Collections.Generic;

namespace CMCS.Web.Controllers;
public class ClaimsController : Controller
{
    // In-memory demo data for prototype (no DB)
    private static readonly List<ClaimListVm> _claims = new()
    {
        new ClaimListVm { ClaimId = Guid.NewGuid(), MonthLabel = "August 2025",    TotalHours = 42, TotalAmount = 12600, Status = "Approved" },
        new ClaimListVm { ClaimId = Guid.NewGuid(), MonthLabel = "September 2025", TotalHours = 18, TotalAmount =  5400, Status = "Draft" },
        new ClaimListVm { ClaimId = Guid.NewGuid(), MonthLabel = "October 2025",   TotalHours =  0, TotalAmount =     0, Status = "Not Started" }
    };

    public IActionResult New()
    {
        var vm = new NewClaimVm
        {
            Items = new()
            {
                new ClaimItemVm { WorkDate = DateTime.Today.AddDays(-3), Hours = 2.0m,  Activity = "Lecture: PROG6212", HourlyRate = 300 },
                new ClaimItemVm { WorkDate = DateTime.Today.AddDays(-2), Hours = 1.5m,  Activity = "Consultation",      HourlyRate = 300 }
            }
        };
        return View(vm);
    }

    public IActionResult My() => View(_claims);

    public IActionResult Details(Guid id)
    {
        var claim = _claims.Find(c => c.ClaimId == id);
        if (claim is null) return NotFound();

        var vm = new NewClaimVm
        {
            LecturerName = "Lonwabo Wabo (demo)",
            MonthLabel = claim.MonthLabel,
            Status = claim.Status,
            Items = new()
            {
                new ClaimItemVm { WorkDate = DateTime.Today.AddDays(-7), Hours = 3, Activity = "Lecture",  HourlyRate = 300 },
                new ClaimItemVm { WorkDate = DateTime.Today.AddDays(-6), Hours = 2, Activity = "Marking",  HourlyRate = 250 }
            }
        };
        return View(vm);
    }
}

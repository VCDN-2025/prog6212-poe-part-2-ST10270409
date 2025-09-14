using Microsoft.AspNetCore.Mvc;
using CMCS.Web.Models;
using System;
using System.Collections.Generic;

namespace CMCS.Web.Controllers;
public class ApprovalsController : Controller
{
    public IActionResult Index()
    {
        var items = new List<ApprovalVm>
        {
            new ApprovalVm { ClaimId = Guid.NewGuid(), Lecturer = "A. Chetty", MonthLabel = "September 2025", Stage = "ProgrammeCoordinator", Status = "UnderReview" },
            new ApprovalVm { ClaimId = Guid.NewGuid(), Lecturer = "L. Wabo",   MonthLabel = "August 2025",    Stage = "AcademicManager",     Status = "UnderReview" }
        };
        return View(items);
    }
}

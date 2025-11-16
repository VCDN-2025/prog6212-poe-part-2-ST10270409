//Referencing list//
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0//

using CMCS.Web.Models;
using CMCS.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Web.Controllers;

public sealed class CoordinatorController : Controller
{
    private readonly IClaimRepository _repo;
    private readonly ILogger<CoordinatorController> _log;
    public CoordinatorController(IClaimRepository repo, ILogger<CoordinatorController> log) { _repo = repo; _log = log; }

    public async Task<IActionResult> Index()
    {
        var pending = (await _repo.GetAllAsync()).Where(c => c.Status == ClaimStatus.Pending).ToList();
        return View(pending);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Verify(Guid id)
    {
        try
        {
            var claim = await _repo.GetAsync(id);
            if (claim is null) return NotFound();
            claim.Status = ClaimStatus.VerifiedByCoordinator;
            await _repo.UpdateAsync(claim);
            TempData["ok"] = "Claim verified.";
        }
        catch (Exception ex) { _log.LogError(ex, "Verify failed"); TempData["err"] = "Could not verify claim."; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id)
    {
        try
        {
            var claim = await _repo.GetAsync(id);
            if (claim is null) return NotFound();
            claim.Status = ClaimStatus.Rejected;
            await _repo.UpdateAsync(claim);
            TempData["ok"] = "Claim rejected.";
        }
        catch (Exception ex) { _log.LogError(ex, "Reject failed"); TempData["err"] = "Could not reject claim."; }
        return RedirectToAction(nameof(Index));
    }
}

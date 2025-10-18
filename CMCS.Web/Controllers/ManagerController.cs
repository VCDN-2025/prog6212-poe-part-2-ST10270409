using CMCS.Web.Models;
using CMCS.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Web.Controllers;

public sealed class ManagerController : Controller
{
    private readonly IClaimRepository _repo;
    private readonly ILogger<ManagerController> _log;
    public ManagerController(IClaimRepository repo, ILogger<ManagerController> log) { _repo = repo; _log = log; }

    public async Task<IActionResult> Index()
    {
        var verified = (await _repo.GetAllAsync()).Where(c => c.Status == ClaimStatus.VerifiedByCoordinator).ToList();
        return View(verified);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            var claim = await _repo.GetAsync(id);
            if (claim is null) return NotFound();
            claim.Status = ClaimStatus.ApprovedByManager;
            await _repo.UpdateAsync(claim);
            TempData["ok"] = "Claim approved.";
        }
        catch (Exception ex) { _log.LogError(ex, "Approve failed"); TempData["err"] = "Could not approve claim."; }
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

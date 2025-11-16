//Referencing list//
//https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0//

using CMCS.Web.Models;
using CMCS.Web.Services;
using CMCS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Web.Controllers;

public sealed class ClaimsController : Controller
{
    private readonly IClaimRepository _repo;
    private readonly IFileCrypto _crypto;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ClaimsController> _log;

    public ClaimsController(IClaimRepository repo, IFileCrypto crypto, IWebHostEnvironment env, ILogger<ClaimsController> log)
    { _repo = repo; _crypto = crypto; _env = env; _log = log; }

    [HttpGet]
    public IActionResult Create() => View(new CreateClaimVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateClaimVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        try
        {
            var claim = new Claim
            {
                Date = vm.Date,
                HoursWorked = vm.HoursWorked,
                HourlyRate = vm.HourlyRate,
                Notes = vm.Notes
            };
            await _repo.AddAsync(claim);
            TempData["ok"] = "Claim submitted successfully.";
            return RedirectToAction(nameof(My));
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Create claim failed");
            TempData["err"] = "Sorry, something went wrong while saving your claim.";
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> My()
    {
        var all = await _repo.GetAllAsync();
        return View(all);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(Guid id, IFormFile file)
    {
        if (file is null || file.Length == 0) { TempData["err"] = "Please select a file."; return RedirectToAction(nameof(My)); }
        if (!_crypto.IsAllowedExtension(file.FileName)) { TempData["err"] = "Only .pdf, .docx, .xlsx allowed."; return RedirectToAction(nameof(My)); }
        if (file.Length > 2 * 1024 * 1024) { TempData["err"] = "File exceeds 2MB limit."; return RedirectToAction(nameof(My)); }

        try
        {
            var claim = await _repo.GetAsync(id);
            if (claim is null) { TempData["err"] = "Claim not found."; return RedirectToAction(nameof(My)); }

            var targetDir = Path.Combine(_env.WebRootPath, "uploads");
            await using var stream = file.OpenReadStream();
            var stored = await _crypto.EncryptAndSaveAsync(stream, targetDir, file.FileName);

            claim.Documents.Add(new ClaimDocument
            {
                OriginalFileName = file.FileName,
                StoredFileName = stored,
                SizeBytes = file.Length
            });
            await _repo.UpdateAsync(claim);

            TempData["ok"] = $"Uploaded {file.FileName}.";
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Upload failed");
            TempData["err"] = "Upload failed. Ensure file type/size is valid.";
        }
        return RedirectToAction(nameof(My));
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid claimId, Guid docId)
    {
        var claim = await _repo.GetAsync(claimId);
        var doc = claim?.Documents.FirstOrDefault(d => d.Id == docId);
        if (doc is null) return NotFound();

        var temp = Path.GetTempFileName();
        await _crypto.DecryptToAsync(doc.StoredFileName, temp);
        var bytes = await System.IO.File.ReadAllBytesAsync(temp);
        System.IO.File.Delete(temp);

        return File(bytes, "application/octet-stream", doc.OriginalFileName);
    }
}

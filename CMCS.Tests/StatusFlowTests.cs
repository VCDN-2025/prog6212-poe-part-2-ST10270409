using CMCS.Web.Models;
using CMCS.Web.Services;
using FluentAssertions;

namespace CMCS.Tests;

public class StatusFlowTests
{
    [Fact]
    public async Task Pending_To_Verified_To_Approved()
    {
        var env = TestHelpers.TempWebHostEnv(out var root);
        try
        {
            var repo = new JsonClaimRepository(env);
            var c = new Claim { Date = DateTime.Today, HoursWorked = 1, HourlyRate = 100m };
            await repo.AddAsync(c);

            var loaded = await repo.GetAsync(c.Id);
            loaded!.Status.Should().Be(ClaimStatus.Pending);

            loaded.Status = ClaimStatus.VerifiedByCoordinator;
            await repo.UpdateAsync(loaded);

            (await repo.GetAsync(c.Id))!.Status.Should().Be(ClaimStatus.VerifiedByCoordinator);

            loaded.Status = ClaimStatus.ApprovedByManager;
            await repo.UpdateAsync(loaded);

            (await repo.GetAsync(c.Id))!.Status.Should().Be(ClaimStatus.ApprovedByManager);
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }

    [Fact]
    public async Task Pending_To_Rejected()
    {
        var env = TestHelpers.TempWebHostEnv(out var root);
        try
        {
            var repo = new JsonClaimRepository(env);
            var c = new Claim { Date = DateTime.Today, HoursWorked = 2, HourlyRate = 150m };
            await repo.AddAsync(c);

            var loaded = await repo.GetAsync(c.Id);
            loaded!.Status.Should().Be(ClaimStatus.Pending);

            loaded.Status = ClaimStatus.Rejected;
            await repo.UpdateAsync(loaded);

            (await repo.GetAsync(c.Id))!.Status.Should().Be(ClaimStatus.Rejected);
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }
}

using CMCS.Web.Models;
using CMCS.Web.Services;
using FluentAssertions;

namespace CMCS.Tests;

public class RepositoryTests
{
    [Fact]
    public async Task JsonRepo_Add_Then_Get_Works()
    {
        var env = TestHelpers.TempWebHostEnv(out var root);
        try
        {
            var repo = new JsonClaimRepository(env);
            var claim = new Claim { Date = DateTime.Today, HoursWorked = 3, HourlyRate = 200m, Notes = "demo" };

            await repo.AddAsync(claim);
            var loaded = await repo.GetAsync(claim.Id);

            loaded.Should().NotBeNull();
            loaded!.Total.Should().Be(600m);
            (await repo.GetAllAsync()).Should().ContainSingle(c => c.Id == claim.Id);
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }
}

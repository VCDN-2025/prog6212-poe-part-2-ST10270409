using System.Text.Json;
using CMCS.Web.Models;

namespace CMCS.Web.Services;

public sealed class JsonClaimRepository : IClaimRepository
{
    private readonly string _jsonPath;
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };

    public JsonClaimRepository(IWebHostEnvironment env)
    {
        var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
        Directory.CreateDirectory(dataDir);
        _jsonPath = Path.Combine(dataDir, "claims.json");
        if (!File.Exists(_jsonPath)) File.WriteAllText(_jsonPath, "[]");
    }

    public async Task<List<Claim>> GetAllAsync()
    {
        using var fs = File.OpenRead(_jsonPath);
        var claims = await JsonSerializer.DeserializeAsync<List<Claim>>(fs) ?? new();
        return claims.OrderByDescending(c => c.Date).ToList();
    }

    public async Task<Claim?> GetAsync(Guid id) =>
        (await GetAllAsync()).FirstOrDefault(c => c.Id == id);

    public async Task AddAsync(Claim claim)
    {
        var items = await GetAllAsync();
        items.Add(claim);
        await SaveAsync(items);
    }

    public async Task UpdateAsync(Claim claim)
    {
        var items = await GetAllAsync();
        var ix = items.FindIndex(c => c.Id == claim.Id);
        if (ix >= 0) { items[ix] = claim; await SaveAsync(items); }
    }

    private async Task SaveAsync(List<Claim> items)
    {
        await using var fs = File.Create(_jsonPath);
        await JsonSerializer.SerializeAsync(fs, items, _opts);
    }
}

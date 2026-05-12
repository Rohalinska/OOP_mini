using System.Text.Json;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class FilePropertyRepository : IPropertyRepository
{
    private readonly string _fileName = "database.json";

    public async Task<List<Property>> GetAllAsync()
    {
        if (!File.Exists(_fileName)) return new List<Property>();
        var json = await File.ReadAllTextAsync(_fileName);
        // Спрощена десеріалізація для лаби
        return JsonSerializer.Deserialize<List<Property>>(json) ?? new List<Property>();
    }

    public async Task AddAsync(Property property)
    {
        var list = await GetAllAsync();
        list.Add(property);
        await SaveInternal(list);
    }

    public async Task SaveAsync() { /* Для оновлення існуючих */ }

    private async Task SaveInternal(List<Property> list)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(list, options);
        await File.WriteAllTextAsync(_fileName, json);
    }
}
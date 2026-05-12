using System.Linq;
using RealEstate.Domain;

namespace RealEstate.Application;

public class PropertyService
{
    private readonly IPropertyRepository _repo;

    public PropertyService(IPropertyRepository repo)
    {
        _repo = repo;
    }

    public async Task AddPropertyAsync(Property property) => await _repo.AddAsync(property);

    public async Task<List<Property>> GetAllPropertiesAsync() => await _repo.GetAllAsync();

    // --- ТУТ 4 LINQ-ЗАПИТИ ДЛЯ ЛАБИ 35 ---

    // 1. Фільтрація: Тільки доступні об'єкти
    public async Task<List<Property>> GetAvailablePropertiesAsync() =>
        (await _repo.GetAllAsync()).Where(p => p.Status == PropertyStatus.Available).ToList();

    // 2. Агрегація: Загальна вартість усієї нерухомості
    public async Task<decimal> GetTotalPortfolioValueAsync() =>
        (await _repo.GetAllAsync()).Sum(p => p.Price);

    // 3. Сортування та фільтр: Топ найдешевших об'єктів до вказаної ціни
    public async Task<List<Property>> GetCheapestAsync(decimal maxPrice) =>
        (await _repo.GetAllAsync())
            .Where(p => p.Price <= maxPrice)
            .OrderBy(p => p.Price)
            .ToList();

    // 4. Аналітика: Кількість об'єктів за типом (Квартири/Будинки)
    public async Task<Dictionary<string, int>> GetCountByTypeAsync() =>
        (await _repo.GetAllAsync())
            .GroupBy(p => p.GetType().Name)
            .ToDictionary(g => g.Key, g => g.Count());
}
using System.Collections.Generic;
using RealEstate.Domain;

namespace RealEstate.Infrastructure;

public class InMemoryRepository : IPropertyRepository
{
    private readonly List<Property> _storage = new();
    public void Add(Property property) => _storage.Add(property);
    public IEnumerable<Property> GetAll() => _storage;
}
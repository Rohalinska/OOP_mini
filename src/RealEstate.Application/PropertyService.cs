using System.Collections.Generic;
using RealEstate.Domain;

namespace RealEstate.Application;

public class PropertyService
{
    private readonly IPropertyRepository _repository;
    public PropertyService(IPropertyRepository repository) => _repository = repository;
    public void RegisterProperty(Property p) => _repository.Add(p);
    public IEnumerable<Property> GetCatalog() => _repository.GetAll();
}
using System;
using System.Collections.Generic;

namespace RealEstate.Domain;

// Власні винятки для контролю якості (Fault Handling)
public class DomainException : Exception { public DomainException(string m) : base(m) {} }
public class InvalidPriceException : DomainException { public InvalidPriceException() : base("Ціна має бути > 0") {} }
public class InvalidAddressException : DomainException { public InvalidAddressException() : base("Адреса не може бути порожньою") {} }
public class AlreadySoldException : DomainException { public AlreadySoldException() : base("Операція неможлива: об'єкт уже продано") {} }

public enum PropertyStatus { Available, Sold }

public abstract class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Address { get; set; }
    public decimal Price { get; set; }
    public PropertyStatus Status { get; set; } = PropertyStatus.Available;

    protected Property() { }

    protected Property(string address, decimal price)
    {
        if (string.IsNullOrWhiteSpace(address)) throw new InvalidAddressException();
        if (price <= 0) throw new InvalidPriceException();
        Address = address;
        Price = price;
    }

    public void MarkAsSold()
    {
        if (Status == PropertyStatus.Sold) throw new AlreadySoldException();
        Status = PropertyStatus.Sold;
    }
}

public class Apartment : Property
{
    public int Floor { get; set; }
    public Apartment() { }
    public Apartment(string a, decimal p, int f) : base(a, p) => Floor = f;
}

public interface IPropertyRepository
{
    Task<List<Property>> GetAllAsync();
    Task AddAsync(Property property);
    Task UpdateAsync(Property property);
}
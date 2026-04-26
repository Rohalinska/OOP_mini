using System;
using System.Collections.Generic;

namespace RealEstate.Domain;

public abstract class Property
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Address { get; }
    public decimal Price { get; }

    protected Property(string address, decimal price)
    {
        // Цей блок виправляє помилку в тестах (FAIL)
        if (string.IsNullOrWhiteSpace(address)) 
            throw new ArgumentException("Адреса не може бути порожньою");
            
        if (price <= 0) 
            throw new ArgumentException("Ціна має бути більшою за нуль");

        Address = address;
        Price = price;
    }

    public abstract decimal CalculateCommission();
}

public class Apartment : Property
{
    public int Floor { get; }
    public Apartment(string addr, decimal p, int floor) : base(addr, p) 
    {
        Floor = floor;
    }
    
    public override decimal CalculateCommission() => Price * 0.05m;
}

public class House : Property
{
    public double YardSize { get; }
    public House(string addr, decimal p, double yard) : base(addr, p) 
    {
        YardSize = yard;
    }
    
    public override decimal CalculateCommission() => Price * 0.08m;
}

public interface IPropertyRepository
{
    void Add(Property property);
    IEnumerable<Property> GetAll();
}
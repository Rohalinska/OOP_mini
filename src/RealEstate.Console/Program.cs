using System;
using System.Text;
using RealEstate.Domain;
using RealEstate.Application;
using RealEstate.Infrastructure;

Console.OutputEncoding = Encoding.UTF8;

var repo = new InMemoryRepository();
var service = new PropertyService(repo);

Console.WriteLine("=== Агенція нерухомості (Ітерація 1) ===");

// Просто додаємо дані в код, як було спочатку
service.RegisterProperty(new Apartment("Київ, вул. Хрещатик 1", 1500000, 4));
service.RegisterProperty(new House("Київська обл., с. Щасливе", 3000000, 10.5));

Console.WriteLine("\nСписок доступних об'єктів:");
foreach (var item in service.GetCatalog())
{
    string type = item is Apartment ? "Квартира" : "Будинок";
    Console.WriteLine($"{type}: {item.Address} | Ціна: {item.Price:N0} грн | Комісія: {item.CalculateCommission():N0} грн");
}
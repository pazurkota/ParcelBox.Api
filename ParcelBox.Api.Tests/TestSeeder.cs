using Microsoft.Extensions.DependencyInjection;
using ParcelBox.Api.Abstraction;
using ParcelBox.Api.Model;

namespace ParcelBox.Api.Tests;

public static class TestSeeder
{
    public static void SeedDefaultLockers(IServiceProvider services)
    {
        var repository = services.GetRequiredService<IRepository<Locker>>();
        
        try
        {
            repository.Create(TestData.LockerEntity(1, "WAS-001"));
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
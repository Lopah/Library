namespace EntityFrameworkCore.Tests.Base;

public static class ApplicationFake
{
    static ApplicationFake()
    {
        DbContext = new();

        DbContext.Database.EnsureDeleted();

        DbContext.Database.EnsureCreated();
        
        // Seed Data

        DbContext.Cars.AddRange(
            new Car
        {
            Bought = true,
            Color = Color.Blue,
            Name = "Test1"
        }, new Car
        {
            Bought = false,
            Color = Color.Red,
            Name = "Test2"
        }, new Car
        {
            Bought = true,
            Color = Color.Red,
            Name = "Test3"
        }, new Car
        {
            Bought = false,
            Color = Color.Blue,
            Name = "Test4"
        }, new Car
        {
            Bought = true,
            Color = Color.Blue,
            Name = "Test5"
        });

        DbContext.SaveChanges();
    }

    public static readonly ApplicationDbContext DbContext;
}

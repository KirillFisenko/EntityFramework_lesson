using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

public class Phone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Guarantee Guarantee { get; set; } // Навигационное свойство
}

public class Guarantee
{
    [ForeignKey(nameof(Phone))] // Ключ гарантии = внешний ключ к телефону
    public int Id { get; set; }
    public int Months { get; set; }

    public Phone Phone { get; set; } // Навигационное свойство
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public ApplicationDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                      .LogTo(Console.WriteLine, LogLevel.Information);
    }
}

public class Program
{
    public static void Main()
    {
        using var dbContext = new ApplicationDbContext();

        var phone1 = new Phone
        {
            Name = "Phone A",
            Price = 299.99m,
            Guarantee = new Guarantee
            {
                Months = 12
            }
        };

        var phone2 = new Phone
        {
            Name = "Phone B",
            Price = 499.99m,
            Guarantee = new Guarantee
            {
                Months = 24
            }
        };

        dbContext.AddRange(phone1, phone2);
        dbContext.SaveChanges();
    }
}
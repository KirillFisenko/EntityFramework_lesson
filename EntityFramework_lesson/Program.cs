using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Phone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    // One-to-One
    public Guarantee Guarantee { get; set; } // Навигационное свойство

    // One-to-Many
    public Storage Storage { get; set; } // Навигационное свойство   

    // Many-to-Many
    public List<App> Apps { get; set; } // Навигационное свойство
}

public class Guarantee
{
    [Key, ForeignKey(nameof(Phone))]
    public int PhoneId { get; set; }
    public int Months { get; set; }
}

public class Storage
{
    public int Id { get; set; }
    public int SizeGB { get; set; }

    // One-to-Many
    public List<Phone> Phones { get; set; } // Навигационное свойство
}

public class App
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Many-to-Many
    public List<Phone> Phones { get; set; } // Навигационное свойство
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Guarantee> Guarantees { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<App> Apps { get; set; }

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
        dbContext.Database.Migrate();
    }
}
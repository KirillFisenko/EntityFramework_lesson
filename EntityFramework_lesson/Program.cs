using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class Phone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Storage Storage { get; set; } // Навигационное свойство
}

public class Storage
{
    public int Id { get; set; }
    public int SizeGB { get; set; }

    public List<Phone> Phones { get; set; } // Навигационное свойство 
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

        // Создадим два вида памяти
        var storage64 = new Storage { SizeGB = 64 };
        var storage128 = new Storage { SizeGB = 128 };

        // Создадим три телефона с разной памятью
        var phone1 = new Phone { Name = "Phone A", Price = 100, Storage = storage64 };
        var phone2 = new Phone { Name = "Phone B", Price = 150, Storage = storage64 };
        var phone3 = new Phone { Name = "Phone C", Price = 200, Storage = storage128 };

        dbContext.AddRange(phone1, phone2, phone3);
        dbContext.SaveChanges();
    }
}
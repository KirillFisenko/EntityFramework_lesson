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

    public Guarantee Guarantee { get; set; }
    public List<SoftwareUpdate> SoftwareUpdates { get; set; }
    public List<App> Apps { get; set; }
}

public class Guarantee
{
    [Key, ForeignKey(nameof(Phone))]
    public int PhoneId { get; set; }
    public int Months { get; set; }

    public Phone Phone { get; set; }
}

public class App
{
    public int Id { get; set; }
    public string Title { get; set; }

    public List<Phone> Phones { get; set; }
}

public class SoftwareUpdate
{
    public int Id { get; set; }
    public string Version { get; set; }
    public DateTime ReleaseDate { get; set; }

    public Phone Phone { get; set; }
}


public class ApplicationDbContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Guarantee> Guarantees { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<SoftwareUpdate> SoftwareUpdates { get; set; }

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

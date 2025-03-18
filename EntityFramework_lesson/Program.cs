using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Phone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ApplicationDbContext1 : DbContext
{
    public DbSet<Phone> Phones { get; set; }

    public ApplicationDbContext1()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=devices;Uid=root;Pwd=m48kHz16bit%;";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
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
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}

public class Program
{
    public static void Main()
    {
        var phone = new Phone()
        {
            Name = "Xiaomi Poco F4",
            Price = 25000
        };

        using var dbContext = new ApplicationDbContext();
        dbContext.Phones.Add(phone);
        dbContext.SaveChanges();
    }

    public static void Main1()
    {
        using var dbContext = new ApplicationDbContext1();

        foreach (var phone in dbContext.Phones)
        {
            Console.WriteLine(phone.Name);
        }
    }
}

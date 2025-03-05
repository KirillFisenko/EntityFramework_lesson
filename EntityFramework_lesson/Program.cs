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

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}

public class Program
{
    public static void Main()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;

        var phone = new Phone()
        {
            Name = "Xiaomi Poco F4",
            Price = 25000
        };

        using var dbContext = new ApplicationDbContext(options);
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

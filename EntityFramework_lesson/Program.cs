using Microsoft.EntityFrameworkCore;

public class Phone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost;Database=devices;Uid=root;Pwd=m48kHz16bit%;";
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
}

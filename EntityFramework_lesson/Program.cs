using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        using var dbContext = new ApplicationDbContext();

        var newPhone = new Phone
        {
            Name = "iPhone 14",
            Price = 999.99m
        };

        dbContext.Phones.Add(newPhone);
        dbContext.SaveChanges();

        var phones = new List<Phone>
        {
            new Phone { Name = "Samsung Galaxy S21", Price = 799.99m },
            new Phone { Name = "Google Pixel 6", Price = 599.99m }
        };

        dbContext.Phones.AddRange(phones);
        dbContext.SaveChanges();

        foreach (var phone in dbContext.Phones)
        {
            Console.WriteLine(phone.Name);
        }

        var phones1 = dbContext.Phones.ToList();
        foreach (var phone in phones1)
        {
            Console.WriteLine(phone.Name);
        }

        var cheapPhones = dbContext.Phones.Where(p => p.Price < 500).ToList();
        foreach (var phone in cheapPhones)
        {
            Console.WriteLine($"{phone.Name} - {phone.Price}");
        }

        var phone1 = dbContext.Phones.FirstOrDefault(p => p.Name == "iPhone 13");
        if (phone1 != null)
        {
            Console.WriteLine($"{phone1.Name} - {phone1.Price}");
        }

        var phone2 = dbContext.Phones.Find(1);
        if (phone2 != null)
        {
            Console.WriteLine($"{phone2.Name} - {phone2.Price}");
        }

        //var phone3 = new Phone { Id = 1 };
        //dbContext.Phones.Remove(phone3);
        //dbContext.SaveChanges();

        var phone4 = dbContext.Phones.Find(1);
        if (phone4 != null)
        {
            dbContext.Phones.Remove(phone4);
            dbContext.SaveChanges();
        }

        var phonesToDelete = dbContext.Phones.Where(p => p.Price < 500).ToList();
        dbContext.Phones.RemoveRange(phonesToDelete);
        dbContext.SaveChanges();

        var phone5 = dbContext.Phones.FirstOrDefault(p => p.Id == 1);
        if (phone5 != null)
        {
            phone5.Name = "iPhone 13 Pro Max";
            phone5.Price = 1199.99m;
            dbContext.SaveChanges();
        }
    }
}

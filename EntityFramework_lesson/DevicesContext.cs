using Microsoft.EntityFrameworkCore;

namespace EntityFramework_lesson;

public partial class DevicesContext : DbContext
{
    public DevicesContext()
    {
    }

    public DevicesContext(DbContextOptions<DevicesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Phone> Phones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=devices;uid=root;pwd=m48kHz16bit%", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.21-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("phones");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

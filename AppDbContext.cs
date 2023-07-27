using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Tag> Tags { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
		builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
		builder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();

		builder.Entity<Category>().Property(c => c.Name).HasMaxLength(30).IsRequired();

		builder.Entity<Tag>().Property(t => t.Name).HasMaxLength(30).IsRequired();
	}

}
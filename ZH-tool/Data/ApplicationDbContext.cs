using Microsoft.EntityFrameworkCore;
using ZH_tool.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ZH_tool.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Zh> Zhk { get; set; }
        public DbSet<GeneraltZh> GeneraltZhk { get; set; }
        public DbSet<Hallgato> Hallgatok { get; set; }
        public DbSet<Megoldas> Megoldasok { get; set; }
        public DbSet<Ertekeles> Ertekelesek { get; set; }
        public DbSet<Feladat> Feladatok { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PostgreSQL specifikus konfigurációk, ha szükségesek
            modelBuilder.Entity<Zh>().ToTable("zh_tabla");
            modelBuilder.Entity<GeneraltZh>().ToTable("generalt_zh_tabla");

            modelBuilder.Entity<Hallgato>().ToTable("hallgatok_tabla");
            modelBuilder.Entity<Megoldas>().ToTable("megoldasok_tabla");
            modelBuilder.Entity<Ertekeles>().ToTable("ertekelesek_tabla");
            modelBuilder.Entity<Feladat>().ToTable("feladatok_tabla");

        }
    }
}

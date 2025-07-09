using Microsoft.EntityFrameworkCore;
using CompanyService.Model;

namespace CompanyService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

              public DbSet<AraciKurum> ARACI_KURUM { get; set; }
              public DbSet<FirmaLogoLinkleri> FirmaLogoLinkleri {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AraciKurum>().ToTable("ARACI_KURUM");
            modelBuilder.Entity<AraciKurum>().HasKey(a => a.ARACI_KURUM_ID);
            modelBuilder.Entity<AraciKurum>()
                .Property(a => a.ARACI_KURUM_ADI) 
                .HasColumnName("ARACI_KURUM_ADI");

            // Configure FirmaLogoLinkleri entity
            modelBuilder.Entity<FirmaLogoLinkleri>().ToTable("Firam_Logo_Linkleri");
            modelBuilder.Entity<FirmaLogoLinkleri>().HasKey(f => f.Firma_Id);
            modelBuilder.Entity<FirmaLogoLinkleri>().Property(f => f.Firma_Id).HasColumnName("Firma_Id");
            modelBuilder.Entity<FirmaLogoLinkleri>().Property(f => f.Hisse_Adi).HasColumnName("Hisse_Adi");
            modelBuilder.Entity<FirmaLogoLinkleri>().Property(f => f.LogoLink).HasColumnName("LogoLink");
        }
    }
} 
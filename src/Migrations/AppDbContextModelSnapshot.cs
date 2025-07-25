﻿// <auto-generated />
using CompanyService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompanyService.Model.AraciKurum", b =>
                {
                    b.Property<int>("ARACI_KURUM_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ARACI_KURUM_ID"));

                    b.Property<string>("ARACI_KURUM_ADI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ARACI_KURUM_ADI");

                    b.HasKey("ARACI_KURUM_ID");

                    b.ToTable("ARACI_KURUM", (string)null);
                });

            modelBuilder.Entity("CompanyService.Model.FirmaLogoLinkleri", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FirmaId")
                        .HasColumnType("int");

                    b.Property<int>("HisseAdi")
                        .HasColumnType("int");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Firam_Logo_Linkleri", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

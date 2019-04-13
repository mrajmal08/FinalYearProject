using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinalYearProject.Models
{
    public partial class OPPCContext : DbContext
    {
        public OPPCContext()
        {
        }

        public OPPCContext(DbContextOptions<OPPCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<UserSystem> UserSystem { get; set; }
        public virtual DbSet<Website> Website { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=MRAJMAL\\SQLEXPRESS;Database=OPPC;Trusted_Connection=True;User ID=sa;Password=ajmal");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.CategoryCreatedDate)
                    .HasColumnName("Category_CreatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CategoryDisc)
                    .HasColumnName("Category_Disc")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryImage)
                    .HasColumnName("Category_Image")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryName)
                    .HasColumnName("Category_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryStatus)
                    .HasColumnName("Category_Status")
                    .HasMaxLength(50);

                entity.Property(e => e.CategoryType)
                    .HasColumnName("Category_Type")
                    .HasMaxLength(50);

                entity.Property(e => e.ParentCategory)
                   .HasColumnName("Parent_Category")
                   .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.WebsiteId).HasColumnName("Website_ID");


                entity.Property(e => e.DiscountedPrice)
                    .HasColumnName("Discounted_Price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductBrand)
                    .HasColumnName("Product_Brand")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductCreatedDate)
                    .HasColumnName("Product_CreatedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProductDeals).HasColumnName("Product_Deals");

                entity.Property(e => e.ProductImage)
                    .HasColumnName("Product_Image")
                    .IsUnicode(false);

                entity.Property(e => e.ProductMetaDisc)
                    .HasColumnName("Product_MetaDisc")
                    .HasMaxLength(2000);

                entity.Property(e => e.ProductMetaTags)
                    .HasColumnName("Product_MetaTags")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductName)
                    .HasColumnName("Product_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductPrice)
                    .HasColumnName("Product_Price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductRating)
                    .HasColumnName("Product_Rating")
                    .HasMaxLength(50);

                entity.Property(e => e.ProductStatus)
                    .HasColumnName("Product_Status")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductUrl)
                    .HasColumnName("Product_Url")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductVariety)
                    .HasColumnName("Product_Variety")
                    .HasMaxLength(500);

                entity.Property(e => e.ProductViews)
                    .HasColumnName("Product_Views")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserSystem>(entity =>
            {
                entity.ToTable("User System");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("Created_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserAddress)
                    .HasColumnName("User_Address")
                    .HasMaxLength(50);

                entity.Property(e => e.UserContact)
                    .HasColumnName("User_Contact")
                    .HasMaxLength(50);

                entity.Property(e => e.UserCountry)
                    .HasColumnName("User_Country")
                    .HasMaxLength(50);

                entity.Property(e => e.UserDob)
                    .HasColumnName("User_DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("User_Email")
                    .HasMaxLength(50);

                entity.Property(e => e.UserFname)
                    .HasColumnName("User_FName")
                    .HasMaxLength(50);

                entity.Property(e => e.UserGender)
                    .HasColumnName("User_Gender")
                    .HasMaxLength(50);

                entity.Property(e => e.UserImage)
                    .HasColumnName("User_Image")
                    .HasMaxLength(500);

                entity.Property(e => e.UserPassword).HasColumnName("User_Password");

                entity.Property(e => e.UserSname)
                    .HasColumnName("User_SName")
                    .HasMaxLength(50);

                entity.Property(e => e.UserStatus)
                    .HasColumnName("User_Status")
                    .HasMaxLength(50);

                entity.Property(e => e.UserType)
                    .HasColumnName("User_Type")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Website>(entity =>
            {
                entity.Property(e => e.WebsiteId).HasColumnName("Website_ID");

                entity.Property(e => e.WebsiteLogo)
                    .HasColumnName("Website_Logo")
                    .HasMaxLength(500);

                entity.Property(e => e.WebsiteName)
                    .HasColumnName("Website_Name")
                    .HasMaxLength(500);

                entity.Property(e => e.WebsiteUrl)
                    .HasColumnName("Website_Url")
                    .HasMaxLength(500);
            });
        }
    }
}

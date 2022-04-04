using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Helperland_1.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Helperland_1.Data
{
    public partial class HelperlandContext : DbContext
    {
        public HelperlandContext()
        {
        }

        public HelperlandContext(DbContextOptions<HelperlandContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<FavoriteAndBlocked> FavoriteAndBlocked { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<ServiceRequest> ServiceRequest { get; set; }
        public virtual DbSet<ServiceRequestAddress> ServiceRequestAddress { get; set; }
        public virtual DbSet<ServiceRequestExtra> ServiceRequestExtra { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<Zipcode> Zipcode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=DESKTOP-HASA225; database=Helperland; trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_City_State");
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Subject).HasMaxLength(500);

                entity.Property(e => e.UploadFileName).HasMaxLength(100);
            });

            modelBuilder.Entity<FavoriteAndBlocked>(entity =>
            {
                entity.HasOne(d => d.TargetUser)
                    .WithMany(p => p.FavoriteAndBlockedTargetUser)
                    .HasForeignKey(d => d.TargetUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteAndBlocked_User");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteAndBlockedUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteAndBlocked_FavoriteAndBlocked");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.Comments).HasMaxLength(2000);

                entity.Property(e => e.Friendly).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.OnTimeArrival).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.QualityOfService).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.RatingDate).HasColumnType("datetime");

                entity.Property(e => e.Ratings).HasColumnType("decimal(2, 1)");

                entity.HasOne(d => d.RatingFromNavigation)
                    .WithMany(p => p.RatingRatingFromNavigation)
                    .HasForeignKey(d => d.RatingFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_User");

                entity.HasOne(d => d.RatingToNavigation)
                    .WithMany(p => p.RatingRatingToNavigation)
                    .HasForeignKey(d => d.RatingTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_User1");

                entity.HasOne(d => d.ServiceRequest)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.ServiceRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_ServiceRequest");
            });

            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Discount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Distance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaymentTransactionRefNo).HasMaxLength(50);

                entity.Property(e => e.RefundedAmount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ServiceHourlyRate).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ServiceStartDate).HasColumnType("datetime");

                entity.Property(e => e.SpacceptedDate)
                    .HasColumnName("SPAcceptedDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubTotal).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.ServiceProvider)
                    .WithMany(p => p.ServiceRequestServiceProvider)
                    .HasForeignKey(d => d.ServiceProviderId)
                    .HasConstraintName("FK_ServiceRequest_User1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ServiceRequestUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceRequest_User");
            });

            modelBuilder.Entity<ServiceRequestAddress>(entity =>
            {
                entity.Property(e => e.AddressLine1).HasMaxLength(200);

                entity.Property(e => e.AddressLine2).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.HasOne(d => d.ServiceRequest)
                    .WithMany(p => p.ServiceRequestAddress)
                    .HasForeignKey(d => d.ServiceRequestId)
                    .HasConstraintName("FK_ServiceRequestAddress_ServiceRequest");
            });

            modelBuilder.Entity<ServiceRequestExtra>(entity =>
            {
                entity.HasOne(d => d.ServiceRequest)
                    .WithMany(p => p.ServiceRequestExtra)
                    .HasForeignKey(d => d.ServiceRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceRequestExtra_ServiceRequest");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TestName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.BankTokenId).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.PaymentGatewayUserRef).HasMaxLength(200);

                entity.Property(e => e.TaxNo).HasMaxLength(50);

                entity.Property(e => e.UserProfilePicture).HasMaxLength(200);

                entity.Property(e => e.ZipCode).HasMaxLength(20);
            });

            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PK_UserAddresses");

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.AddressLine2).HasMaxLength(200);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAddress)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserAddresses_User");
            });

            modelBuilder.Entity<Zipcode>(entity =>
            {
                entity.Property(e => e.ZipcodeValue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Zipcode)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zipcode_City");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

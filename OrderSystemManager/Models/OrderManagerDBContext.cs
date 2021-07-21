using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OrderSystemManager.Models
{
    public partial class OrderManagerDBContext : DbContext
    {
        public OrderManagerDBContext()
        {
        }

        public OrderManagerDBContext(DbContextOptions<OrderManagerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CartRecord> CartRecords { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<FoodType> FoodTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=. ;Initial Catalog=OrderingDB; Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<CartRecord>(entity =>
            {
                entity.HasKey(e => e.CartId);

                entity.ToTable("CartRecord");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.CartRecords)
                    .HasForeignKey(d => d.FoodId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CartRecord_Food");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CartRecords)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CartRecord_User");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.AddDate).HasColumnType("datetime");

                entity.Property(e => e.Desc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FoodName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FoodTypeId).HasColumnName("FoodTypeID");

                entity.Property(e => e.PicUrl).IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.FoodType)
                    .WithMany(p => p.Foods)
                    .HasForeignKey(d => d.FoodTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Food_FoodType");
            });

            modelBuilder.Entity<FoodType>(entity =>
            {
                entity.ToTable("FoodType");

                entity.Property(e => e.FoodTypeId).HasColumnName("FoodTypeID");

                entity.Property(e => e.FoodTypeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RealName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Totle).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.FoodName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PicUrl).IsUnicode(false);

                entity.Property(e => e.UnitPrice).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RealName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_User_UserType");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.Property(e => e.UserTypeName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

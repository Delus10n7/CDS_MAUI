using System;
using System.Collections.Generic;
using CDS_DAL;
using Microsoft.EntityFrameworkCore;
using CDS_DomainModel.Entities;
using DriveType = CDS_DomainModel.Entities.DriveType;

namespace CDS_DAL.Context;

public partial class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalService> AdditionalService { get; set; }

    public virtual DbSet<AvailabilityStatus> AvailabilityStatus { get; set; }

    public virtual DbSet<BodyType> BodyType { get; set; }

    public virtual DbSet<Booking> Booking { get; set; }

    public virtual DbSet<BookingStatus> BookingStatus { get; set; }

    public virtual DbSet<Brand> Brand { get; set; }

    public virtual DbSet<CarCondition> CarCondition { get; set; }

    public virtual DbSet<CarConfiguration> CarConfiguration { get; set; }

    public virtual DbSet<Cars> Cars { get; set; }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<Discount> Discount { get; set; }

    public virtual DbSet<DiscountType> DiscountType { get; set; }

    public virtual DbSet<DriveType> DriveType { get; set; }

    public virtual DbSet<EngineType> EngineType { get; set; }

    public virtual DbSet<Model> Model { get; set; }

    public virtual DbSet<OrderStatus> OrderStatus { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<RoleType> RoleType { get; set; }

    public virtual DbSet<SelectedService> SelectedService { get; set; }

    public virtual DbSet<ServiceContracts> ServiceContracts { get; set; }

    public virtual DbSet<TransmissionType> TransmissionType { get; set; }

    public virtual DbSet<UserBase> Users { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Manager> Managers { get; set; }
    public virtual DbSet<Administrator> Administrators { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserBase>()
            .ToTable("Users")
            .HasDiscriminator<int>("RoleId")
            .HasValue<Customer>(1)
            .HasValue<Manager>(2)
            .HasValue<Administrator>(3);

        modelBuilder.Entity<UserBase>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey("RoleId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AdditionalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC0787D51D76");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<AvailabilityStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabi__3214EC073CE0FD3B");

            entity.HasIndex(e => e.AvailabilityName, "UQ__Availabi__86FED0215DDD6599").IsUnique();

            entity.Property(e => e.AvailabilityName).HasMaxLength(50);
        });

        modelBuilder.Entity<BodyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BodyType__3214EC07F7F9F4E9");

            entity.HasIndex(e => e.BodyName, "UQ__BodyType__CF6A10AC86CDB320").IsUnique();

            entity.Property(e => e.BodyName).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC078129580C");

            entity.HasIndex(e => e.CarId, "IX_Booking_CarId");

            entity.HasIndex(e => e.UserId, "IX_Booking_ClientId");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "IX_Booking_Dates");

            entity.HasIndex(e => e.BookingStatusId, "IX_Booking_Status").HasFilter("([BookingStatusId]=(1))");

            entity.HasIndex(e => e.UserId, "IX_Booking_UserId");

            entity.Property(e => e.StartDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.BookingStatus).WithMany(p => p.Booking)
                .HasForeignKey(d => d.BookingStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__Booking__08B54D69");

            entity.HasOne(d => d.Car).WithMany(p => p.Booking)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__CarId__06CD04F7");

            entity.HasOne(d => d.User).WithMany(p => p.Booking)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__UserId__05D8E0BE");
        });

        modelBuilder.Entity<BookingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookingS__3214EC078829FEC5");

            entity.HasIndex(e => e.StatusName, "UQ__BookingS__05E7698AF2CD1FE7").IsUnique();

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC0757A0CF52");

            entity.HasIndex(e => e.CountryId, "IX_Brand_CountryId");

            entity.Property(e => e.BrandName).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.Brand)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__Brand__CountryId__5BE2A6F2");
        });

        modelBuilder.Entity<CarCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarCondi__3214EC07F3F2A8A5");

            entity.HasIndex(e => e.ConditionName, "UQ__CarCondi__CE7E6066F2148F55").IsUnique();

            entity.Property(e => e.ConditionName).HasMaxLength(50);
        });

        modelBuilder.Entity<CarConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarConfi__3214EC073F6A3B1A");

            entity.HasIndex(e => new { e.BodyTypeId, e.EngineTypeId, e.TransmissionTypeId, e.DriveTypeId }, "IX_CarConfiguration_Full");

            entity.HasIndex(e => e.ModelId, "IX_CarConfiguration_ModelId");

            entity.Property(e => e.EngineVolume).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.BodyType).WithMany(p => p.CarConfiguration)
                .HasForeignKey(d => d.BodyTypeId)
                .HasConstraintName("FK__CarConfig__BodyT__628FA481");

            entity.HasOne(d => d.DriveType).WithMany(p => p.CarConfiguration)
                .HasForeignKey(d => d.DriveTypeId)
                .HasConstraintName("FK__CarConfig__Drive__656C112C");

            entity.HasOne(d => d.EngineType).WithMany(p => p.CarConfiguration)
                .HasForeignKey(d => d.EngineTypeId)
                .HasConstraintName("FK__CarConfig__Engin__6383C8BA");

            entity.HasOne(d => d.Model).WithMany(p => p.CarConfiguration)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK__CarConfig__Model__619B8048");

            entity.HasOne(d => d.TransmissionType).WithMany(p => p.CarConfiguration)
                .HasForeignKey(d => d.TransmissionTypeId)
                .HasConstraintName("FK__CarConfig__Trans__6477ECF3");
        });

        modelBuilder.Entity<Cars>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC074808A61D");

            entity.HasIndex(e => e.AvailabilityId, "IX_Cars_AvailabilityId");

            entity.HasIndex(e => e.ConditionId, "IX_Cars_ConditionId");

            entity.HasIndex(e => e.ConfigurationId, "IX_Cars_ConfigurationId");

            entity.HasIndex(e => e.Price, "IX_Cars_Price");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VIN).HasMaxLength(17);

            entity.HasOne(d => d.Availability).WithMany(p => p.Cars)
                .HasForeignKey(d => d.AvailabilityId)
                .HasConstraintName("FK__Cars__Availabili__693CA210");

            entity.HasOne(d => d.Condition).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ConditionId)
                .HasConstraintName("FK__Cars__ConditionI__68487DD7");

            entity.HasOne(d => d.Configuration).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ConfigurationId)
                .HasConstraintName("FK__Cars__Configurat__6A30C649");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC07BC720195");

            entity.Property(e => e.CountryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC0721272C56");

            entity.HasIndex(e => new { e.IsActive, e.EndDate, e.StartDate }, "IX_Discount_Active");

            entity.HasIndex(e => e.BrandId, "IX_Discount_Brand").HasFilter("([BrandId] IS NOT NULL)");

            entity.HasIndex(e => e.ClientId, "IX_Discount_Client").HasFilter("([ClientId] IS NOT NULL)");

            entity.HasIndex(e => e.ModelId, "IX_Discount_Model").HasFilter("([ModelId] IS NOT NULL)");

            entity.HasIndex(e => e.DiscountTypeId, "IX_Discount_Type");

            entity.Property(e => e.DiscountName).HasMaxLength(100);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Brand).WithMany(p => p.Discount)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Discount__BrandI__6E01572D");

            entity.HasOne(d => d.Client).WithMany(p => p.Discount)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__Discount__Client__6EF57B66");

            entity.HasOne(d => d.DiscountType).WithMany(p => p.Discount)
                .HasForeignKey(d => d.DiscountTypeId)
                .HasConstraintName("FK__Discount__Discou__6FE99F9F");

            entity.HasOne(d => d.Model).WithMany(p => p.Discount)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK__Discount__ModelI__6D0D32F4");
        });

        modelBuilder.Entity<DiscountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC071C42F3DD");

            entity.HasIndex(e => e.TypeName, "UQ__Discount__D4E7DFA871F7055F").IsUnique();

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<DriveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DriveTyp__3214EC0732B0C48A");

            entity.HasIndex(e => e.DriveName, "UQ__DriveTyp__60E80A4BB57CE4F0").IsUnique();

            entity.Property(e => e.DriveName).HasMaxLength(50);
        });

        modelBuilder.Entity<EngineType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EngineTy__3214EC07CCDF5746");

            entity.HasIndex(e => e.EngineName, "UQ__EngineTy__FF59FEFE17AA2A43").IsUnique();

            entity.Property(e => e.EngineName).HasMaxLength(50);
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Model__3214EC0748E93BD0");

            entity.HasIndex(e => e.BrandId, "IX_Model_BrandId");

            entity.Property(e => e.ModelName).HasMaxLength(100);

            entity.HasOne(d => d.Brand).WithMany(p => p.Model)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Model__BrandId__5EBF139D");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC0757CA12AA");

            entity.HasIndex(e => e.StatusName, "UQ__OrderSta__05E7698A7DCC2F11").IsUnique();

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07838906EE");

            entity.HasIndex(e => e.CarId, "IX_Orders_CarId");

            entity.HasIndex(e => e.ClientId, "IX_Orders_ClientId");

            entity.HasIndex(e => e.ManagerId, "IX_Orders_ManagerId");

            entity.HasIndex(e => e.OrderDate, "IX_Orders_OrderDate");

            entity.HasIndex(e => e.StatusId, "IX_Orders_StatusId");

            entity.HasIndex(e => e.IsTradeIn, "IX_Orders_TradeIn").HasFilter("([IsTradeIn]=(1))");

            entity.Property(e => e.IsTradeIn).HasDefaultValue(false);
            entity.Property(e => e.TradeInValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Car).WithMany(p => p.OrdersCar)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK__Orders__CarId__787EE5A0");

            entity.HasOne(d => d.Client).WithMany(p => p.OrdersClient)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ClientId__76969D2E");

            entity.HasOne(d => d.Manager).WithMany(p => p.OrdersManager)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ManagerI__778AC167");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Orders__StatusId__797309D9");

            entity.Property(d => d.SalePrice)
                .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<RoleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleType__3214EC07D534CF58");

            entity.HasIndex(e => e.RoleName, "UQ__RoleType__8A2B6160448DF9CE").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SelectedService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Selected__3214EC07312EDE84");

            entity.HasIndex(e => e.ServiceContractId, "IX_SelectedService_ContractId");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.AdditionalService).WithMany(p => p.SelectedService)
                .HasForeignKey(d => d.AdditionalServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SelectedS__Addit__02FC7413");

            entity.HasOne(d => d.ServiceContract).WithMany(p => p.SelectedService)
                .HasForeignKey(d => d.ServiceContractId)
                .HasConstraintName("FK__SelectedS__Servi__02084FDA");
        });

        modelBuilder.Entity<ServiceContracts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceC__3214EC0711ED28C5");

            entity.HasIndex(e => e.ClientId, "IX_ServiceContracts_ClientId");

            entity.HasIndex(e => e.SaleDate, "IX_ServiceContracts_SaleDate");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.Property(e => e.ContractStatus).HasMaxLength(100);

            entity.HasOne(d => d.Client).WithMany(p => p.ServiceContractsClient)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceCo__Clien__7E37BEF6");

            entity.HasOne(d => d.Manager).WithMany(p => p.ServiceContractsManager)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceCo__Manag__7F2BE32F");
        });

        modelBuilder.Entity<TransmissionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transmis__3214EC0717D8804A");

            entity.HasIndex(e => e.TransmissionName, "UQ__Transmis__1BD99A90CDA96F57").IsUnique();

            entity.Property(e => e.TransmissionName).HasMaxLength(50);
        });

        modelBuilder.Ignore<Users>();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

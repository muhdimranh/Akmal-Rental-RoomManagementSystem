using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Viho.web.DataDB;

public partial class DbRentalContext : DbContext
{
    public DbRentalContext()
    {
    }

    public DbRentalContext(DbContextOptions<DbRentalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAttendanceCleaner> TbAttendanceCleaners { get; set; }

    public virtual DbSet<TbAttendancedate> TbAttendancedates { get; set; }

    public virtual DbSet<TbInventory> TbInventories { get; set; }

    public virtual DbSet<TbInvestor> TbInvestors { get; set; }

    public virtual DbSet<TbLocation> TbLocations { get; set; }

    public virtual DbSet<TbLot> TbLots { get; set; }

    public virtual DbSet<TbPayment> TbPayments { get; set; }

    public virtual DbSet<TbProfit> TbProfits { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<TbRoom> TbRooms { get; set; }

    public virtual DbSet<TbSale> TbSales { get; set; }

    public virtual DbSet<TbShare> TbShares { get; set; }

    public virtual DbSet<TbTenant> TbTenants { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=POPIZTUF\\SQLEXPRESS;Database=db_rental;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAttendanceCleaner>(entity =>
        {
            entity.HasKey(e => e.AttId).HasName("PK_tb_attendance");

            entity.ToTable("tb_attendanceCleaner");

            entity.Property(e => e.AttId).HasColumnName("att_id");
            entity.Property(e => e.AttCount).HasColumnName("att_count");
            entity.Property(e => e.AttLid).HasColumnName("att_lid");

            entity.HasOne(d => d.AttL).WithMany(p => p.TbAttendanceCleaners)
                .HasForeignKey(d => d.AttLid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_attendanceCleaner_tb_location");
        });

        modelBuilder.Entity<TbAttendancedate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tb_dateAttendance");

            entity.ToTable("tb_attendancedate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attdate)
                .HasColumnType("date")
                .HasColumnName("attdate");
            entity.Property(e => e.Attid).HasColumnName("attid");

            entity.HasOne(d => d.Att).WithMany(p => p.TbAttendancedates)
                .HasForeignKey(d => d.Attid)
                .HasConstraintName("FK_tb_attendancedate_tb_attendanceCleaner");
        });

        modelBuilder.Entity<TbInventory>(entity =>
        {
            entity.HasKey(e => e.IId);

            entity.ToTable("tb_inventory");

            entity.Property(e => e.IId).HasColumnName("i_id");
            entity.Property(e => e.IItem)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("i_item");
            entity.Property(e => e.ILocationid).HasColumnName("i_locationid");
            entity.Property(e => e.IQuantity).HasColumnName("i_quantity");

            entity.HasOne(d => d.ILocation).WithMany(p => p.TbInventories)
                .HasForeignKey(d => d.ILocationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_inventory_tb_location");
        });

        modelBuilder.Entity<TbInvestor>(entity =>
        {
            entity.HasKey(e => e.IId);

            entity.ToTable("tb_investor");

            entity.Property(e => e.IId)
                .ValueGeneratedNever()
                .HasColumnName("i_id");
            entity.Property(e => e.IInvestment).HasColumnName("i_investment");
            entity.Property(e => e.ILot).HasColumnName("i_lot");
            entity.Property(e => e.IPercent).HasColumnName("i_percent");

            entity.HasOne(d => d.IIdNavigation).WithOne(p => p.TbInvestor)
                .HasForeignKey<TbInvestor>(d => d.IId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_investor_tb_user");

            entity.HasOne(d => d.ILotNavigation).WithMany(p => p.TbInvestors)
                .HasForeignKey(d => d.ILot)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_investor_tb_lot");
        });

        modelBuilder.Entity<TbLocation>(entity =>
        {
            entity.HasKey(e => e.LId);

            entity.ToTable("tb_location");

            entity.Property(e => e.LId).HasColumnName("l_id");
            entity.Property(e => e.IsPaymentMade).HasColumnName("isPaymentMade");
            entity.Property(e => e.LAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("l_address");
            entity.Property(e => e.LCctv)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("l_cctv");
            entity.Property(e => e.LCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("l_code");
            entity.Property(e => e.LImglayout1)
                .IsUnicode(false)
                .HasColumnName("l_imglayout1");
            entity.Property(e => e.LModemIp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("l_modemIP");
            entity.Property(e => e.LReminderDate).HasColumnName("l_reminderDate");
            entity.Property(e => e.LWifi)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("l_wifi");
        });

        modelBuilder.Entity<TbLot>(entity =>
        {
            entity.HasKey(e => e.LotId);

            entity.ToTable("tb_lot");

            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.LotName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lot_name");
            entity.Property(e => e.LotStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lot_status");
        });

        modelBuilder.Entity<TbPayment>(entity =>
        {
            entity.HasKey(e => e.PId);

            entity.ToTable("tb_payment");

            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.PAmount).HasColumnName("p_amount");
            entity.Property(e => e.PDate)
                .HasColumnType("date")
                .HasColumnName("p_date");
            entity.Property(e => e.PLocationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("p_locationCode");
            entity.Property(e => e.PReceipt)
                .IsUnicode(false)
                .HasColumnName("p_receipt");
            entity.Property(e => e.PRoomNo).HasColumnName("p_roomNo");
            entity.Property(e => e.PTenantid).HasColumnName("p_tenantid");
            entity.Property(e => e.PType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("p_type");

            entity.HasOne(d => d.PTenant).WithMany(p => p.TbPayments)
                .HasForeignKey(d => d.PTenantid)
                .HasConstraintName("FK_tb_payment_tb_tenants");
        });

        modelBuilder.Entity<TbProfit>(entity =>
        {
            entity.ToTable("tb_profit");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RlId);

            entity.ToTable("tb_role");

            entity.Property(e => e.RlId).HasColumnName("rl_id");
            entity.Property(e => e.RlDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rl_desc");
        });

        modelBuilder.Entity<TbRoom>(entity =>
        {
            entity.HasKey(e => e.RId);

            entity.ToTable("tb_room");

            entity.Property(e => e.RId).HasColumnName("r_id");
            entity.Property(e => e.RDepositAmount).HasColumnName("r_depositAmount");
            entity.Property(e => e.RDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("r_desc");
            entity.Property(e => e.RImg1)
                .IsUnicode(false)
                .HasColumnName("r_img1");
            entity.Property(e => e.RImg2)
                .IsUnicode(false)
                .HasColumnName("r_img2");
            entity.Property(e => e.RLocationid).HasColumnName("r_locationid");
            entity.Property(e => e.RNo).HasColumnName("r_no");
            entity.Property(e => e.RPrice).HasColumnName("r_price");
            entity.Property(e => e.RStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("r_status");
            entity.Property(e => e.RTid).HasColumnName("r_tid");
            entity.Property(e => e.RType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("r_type");

            entity.HasOne(d => d.RLocation).WithMany(p => p.TbRooms)
                .HasForeignKey(d => d.RLocationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_room_tb_location");

            entity.HasOne(d => d.RT).WithMany(p => p.TbRooms)
                .HasForeignKey(d => d.RTid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tb_room_tb_tenants");
        });

        modelBuilder.Entity<TbSale>(entity =>
        {
            entity.HasKey(e => e.SId);

            entity.ToTable("tb_sales");

            entity.Property(e => e.SId).HasColumnName("s_id");
            entity.Property(e => e.SAmountIn).HasColumnName("s_amountIN");
            entity.Property(e => e.SAmountOut).HasColumnName("s_amountOUT");
            entity.Property(e => e.SCategory)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_category");
            entity.Property(e => e.SDate)
                .HasColumnType("date")
                .HasColumnName("s_date");
            entity.Property(e => e.SDetail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_detail");
            entity.Property(e => e.SPaymentid).HasColumnName("s_paymentid");
            entity.Property(e => e.SReceipt)
                .IsUnicode(false)
                .HasColumnName("s_receipt");
            entity.Property(e => e.STransactionType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("s_transactionType");

            entity.HasOne(d => d.SPayment).WithMany(p => p.TbSales)
                .HasForeignKey(d => d.SPaymentid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tb_sales_tb_payment");
        });

        modelBuilder.Entity<TbShare>(entity =>
        {
            entity.HasKey(e => e.ShareId);

            entity.ToTable("tb_share");

            entity.Property(e => e.ShareId).HasColumnName("share_id");
            entity.Property(e => e.ShareAmount).HasColumnName("share_amount");
            entity.Property(e => e.ShareDate)
                .HasColumnType("date")
                .HasColumnName("share_date");
            entity.Property(e => e.ShareInvestor).HasColumnName("share_investor");

            entity.HasOne(d => d.ShareInvestorNavigation).WithMany(p => p.TbShares)
                .HasForeignKey(d => d.ShareInvestor)
                .HasConstraintName("FK_tb_share_tb_investor");
        });

        modelBuilder.Entity<TbTenant>(entity =>
        {
            entity.HasKey(e => e.TId);

            entity.ToTable("tb_tenants");

            entity.Property(e => e.TId).HasColumnName("t_id");
            entity.Property(e => e.LastReminderDate).HasColumnType("datetime");
            entity.Property(e => e.TAddOnDetail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_addOnDetail");
            entity.Property(e => e.TAddress)
                .HasColumnType("text")
                .HasColumnName("t_address");
            entity.Property(e => e.TAgreement)
                .IsUnicode(false)
                .HasColumnName("t_agreement");
            entity.Property(e => e.TCardCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_cardCode");
            entity.Property(e => e.TEmerContact)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_emerContact");
            entity.Property(e => e.TEntrydate)
                .HasColumnType("datetime")
                .HasColumnName("t_entrydate");
            entity.Property(e => e.TExitdate)
                .HasColumnType("datetime")
                .HasColumnName("t_exitdate");
            entity.Property(e => e.TIc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_ic");
            entity.Property(e => e.TName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("t_name");
            entity.Property(e => e.TPaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_paymentStatus");
            entity.Property(e => e.TPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("t_phone");
            entity.Property(e => e.TRoomid).HasColumnName("t_roomid");
            entity.Property(e => e.TUploadIc)
                .IsUnicode(false)
                .HasColumnName("t_uploadIC");

            entity.HasOne(d => d.TRoom).WithMany(p => p.TbTenants)
                .HasForeignKey(d => d.TRoomid)
                .HasConstraintName("FK_tb_tenants_tb_room");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UId);

            entity.ToTable("tb_user");

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("u_email");
            entity.Property(e => e.UPass)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("u_pass");
            entity.Property(e => e.UPhone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("u_phone");
            entity.Property(e => e.URoleid).HasColumnName("u_roleid");
            entity.Property(e => e.UUsername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("u_username");

            entity.HasOne(d => d.URole).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.URoleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_user_tb_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

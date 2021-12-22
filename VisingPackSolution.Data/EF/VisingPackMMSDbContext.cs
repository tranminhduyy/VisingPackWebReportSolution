using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VisingPackSolution.Data.EF
{
    public partial class VisingPackMMSDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        //public VisingPackMMSDbContext()
        //{
        //}

        //public VisingPackMMSDbContext(DbContextOptions<VisingPackMMSDbContext> options)
        //    : base(options)
        //{
        //}

        public VisingPackMMSDbContext(DbContextOptions<VisingPackMMSDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppRoleClaim> AppRoleClaims { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppUserClaim> AppUserClaims { get; set; }
        public virtual DbSet<AppUserLogin> AppUserLogins { get; set; }
        public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
        public virtual DbSet<AppUserToken> AppUserTokens { get; set; }
        public virtual DbSet<Btd2PoEvent> Btd2PoEvents { get; set; }
        public virtual DbSet<Btd2PoInfo> Btd2PoInfos { get; set; }
        public virtual DbSet<Btd2PoKpi> Btd2PoKpis { get; set; }
        public virtual DbSet<Btd3PoEvent> Btd3PoEvents { get; set; }
        public virtual DbSet<Btd3PoInfo> Btd3PoInfos { get; set; }
        public virtual DbSet<Btd3PoKpi> Btd3PoKpis { get; set; }
        public virtual DbSet<Btd4PoEvent> Btd4PoEvents { get; set; }
        public virtual DbSet<Btd4PoInfo> Btd4PoInfos { get; set; }
        public virtual DbSet<Btd4PoKpi> Btd4PoKpis { get; set; }
        public virtual DbSet<Btd5PoEvent> Btd5PoEvents { get; set; }
        public virtual DbSet<Btd5PoInfo> Btd5PoInfos { get; set; }
        public virtual DbSet<Btd5PoKpi> Btd5PoKpis { get; set; }
        public virtual DbSet<D1000PoEvent> D1000PoEvents { get; set; }
        public virtual DbSet<D1000PoInfo> D1000PoInfos { get; set; }
        public virtual DbSet<D1000PoKpi> D1000PoKpis { get; set; }
        public virtual DbSet<D1100PoEvent> D1100PoEvents { get; set; }
        public virtual DbSet<D1100PoInfo> D1100PoInfos { get; set; }
        public virtual DbSet<D1100PoKpi> D1100PoKpis { get; set; }
        public virtual DbSet<D650PoEvent> D650PoEvents { get; set; }
        public virtual DbSet<D650PoInfo> D650PoInfos { get; set; }
        public virtual DbSet<D650PoKpi> D650PoKpis { get; set; }
        public virtual DbSet<D750PoEvent> D750PoEvents { get; set; }
        public virtual DbSet<D750PoInfo> D750PoInfos { get; set; }
        public virtual DbSet<D750PoKpi> D750PoKpis { get; set; }
        public virtual DbSet<Gmc1Alarm> Gmc1Alarms { get; set; }
        public virtual DbSet<Gmc1PoEvent> Gmc1PoEvents { get; set; }
        public virtual DbSet<Gmc1PoInfo> Gmc1PoInfos { get; set; }
        public virtual DbSet<Gmc1PoKpi> Gmc1PoKpis { get; set; }
        public virtual DbSet<Gmc2Alarm> Gmc2Alarms { get; set; }
        public virtual DbSet<Gmc2PoEvent> Gmc2PoEvents { get; set; }
        public virtual DbSet<Gmc2PoInfo> Gmc2PoInfos { get; set; }
        public virtual DbSet<Gmc2PoKpi> Gmc2PoKpis { get; set; }
        public virtual DbSet<MachineEvent> MachineEvents { get; set; }
        public virtual DbSet<MmsMaintain> MmsMaintains { get; set; }
        public virtual DbSet<P5mPoEvent> P5mPoEvents { get; set; }
        public virtual DbSet<P5mPoInfo> P5mPoInfos { get; set; }
        public virtual DbSet<P5mPoKpi> P5mPoKpis { get; set; }
        public virtual DbSet<P601PoEvent> P601PoEvents { get; set; }
        public virtual DbSet<P601PoInfo> P601PoInfos { get; set; }
        public virtual DbSet<P601PoKpi> P601PoKpis { get; set; }
        public virtual DbSet<P604PoEvent> P604PoEvents { get; set; }
        public virtual DbSet<P604PoInfo> P604PoInfos { get; set; }
        public virtual DbSet<P604PoKpi> P604PoKpis { get; set; }
        public virtual DbSet<P605PoEvent> P605PoEvents { get; set; }
        public virtual DbSet<P605PoInfo> P605PoInfos { get; set; }
        public virtual DbSet<P605PoKpi> P605PoKpis { get; set; }
        public virtual DbSet<SclPoEvent> SclPoEvents { get; set; }
        public virtual DbSet<SclPoInfo> SclPoInfos { get; set; }
        public virtual DbSet<SclPoKpi> SclPoKpis { get; set; }
        public virtual DbSet<TableDay> TableDays { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-V1I9QML\\SQLEXPRESS;Database=VisingPackMMS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Account");

                entity.Property(e => e.Datetime).HasPrecision(2);
            });

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<AppUserLogin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();
            });

            modelBuilder.Entity<AppUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AppUserToken>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Btd2PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD2_PO_Event");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd2PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD2_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd2PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD2_PO_KPI");

                entity.Property(e => e.Endtime).HasColumnType("datetime");

                entity.Property(e => e.Starttime).HasColumnType("datetime");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd3PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD3_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd3PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD3_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd3PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD3_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd4PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD4_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd4PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD4_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd4PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD4_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd5PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD5_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd5PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD5_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Btd5PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BTD5_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1000PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1000_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1000PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1000_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1000PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1000_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1100PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1100_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1100PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1100_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D1100PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D1100_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D650PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D650_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D650PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D650_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D650PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D650_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D750PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D750_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D750PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D750_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<D750PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("D750_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc1Alarm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC1_Alarm");

                entity.Property(e => e.Datetime).HasPrecision(2);
            });

            modelBuilder.Entity<Gmc1PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC1_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc1PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC1_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Ra).HasColumnName("RA");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc1PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC1_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc2Alarm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC2_Alarm");

                entity.Property(e => e.Datetime).HasPrecision(2);
            });

            modelBuilder.Entity<Gmc2PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC2_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc2PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC2_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Ra).HasColumnName("RA");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Upe).HasColumnName("UPE");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<Gmc2PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GMC2_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<MachineEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Machine_Event");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<MmsMaintain>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MMS_Maintain");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);
            });

            modelBuilder.Entity<P5mPoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P5M_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P5mPoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P5M_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P5mPoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P5M_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P601PoEvent>(entity =>
            {
                entity.HasKey(x => x.Uid);

                entity.ToTable("P601_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P601PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P601_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P601PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P601_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P604PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P604_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P604PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P604_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P604PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P604_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P605PoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P605_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P605PoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P605_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<P605PoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("P605_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<SclPoEvent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SCL_PO_Event");

                entity.Property(e => e.Datetime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<SclPoInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SCL_PO_Info");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            modelBuilder.Entity<SclPoKpi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SCL_PO_KPI");

                entity.Property(e => e.Endtime).HasPrecision(2);

                entity.Property(e => e.Starttime).HasPrecision(2);

                entity.Property(e => e.Uid).HasColumnName("UID");

                entity.Property(e => e.Ws).HasColumnName("WS");
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TableDay>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Table_Day");

                entity.Property(e => e.Date).HasColumnType("date");

                //entity.Property(e => e.Uid).HasColumnName("UID");

                //entity.Property(e => e.Ws).HasColumnName("WS");

                //entity.Property(e => e.Datetime).HasPrecision(2);
            });

            base.OnModelCreating(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

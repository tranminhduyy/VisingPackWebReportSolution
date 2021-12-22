using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using VisingPackSolution.Data.Speed.Entities;

#nullable disable

namespace VisingPackSolution.Data.Speed.EF
{
    public partial class GroupHistoryDBContext : DbContext
    {
        public GroupHistoryDBContext()
        {
        }

        public GroupHistoryDBContext(DbContextOptions<GroupHistoryDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DivBtd2SpeedHistrecord> DivBtd2SpeedHistrecords { get; set; }
        public virtual DbSet<DivBtd3SpeedHistrecord> DivBtd3SpeedHistrecords { get; set; }
        public virtual DbSet<DivBtd4SpeedHistrecord> DivBtd4SpeedHistrecords { get; set; }
        public virtual DbSet<DivBtd5SpeedHistrecord> DivBtd5SpeedHistrecords { get; set; }
        public virtual DbSet<DivD1000SpeedHistrecord> DivD1000SpeedHistrecords { get; set; }
        public virtual DbSet<DivD1100SpeedHistrecord> DivD1100SpeedHistrecords { get; set; }
        public virtual DbSet<DivD650SpeedHistrecord> DivD650SpeedHistrecords { get; set; }
        public virtual DbSet<DivD750SpeedHistrecord> DivD750SpeedHistrecords { get; set; }
        public virtual DbSet<DivGmc1LengthHistrecord> DivGmc1LengthHistrecords { get; set; }
        public virtual DbSet<DivGmc1SpeedHistrecord> DivGmc1SpeedHistrecords { get; set; }
        public virtual DbSet<DivGmc2LengthHistrecord> DivGmc2LengthHistrecords { get; set; }
        public virtual DbSet<DivGmc2SpeedHistrecord> DivGmc2SpeedHistrecords { get; set; }
        public virtual DbSet<DivP5mSpeedHistrecord> DivP5mSpeedHistrecords { get; set; }
        public virtual DbSet<DivP601SpeedHistrecord> DivP601SpeedHistrecords { get; set; }
        public virtual DbSet<DivP604SpeedHistrecord> DivP604SpeedHistrecords { get; set; }
        public virtual DbSet<DivP605SpeedHistrecord> DivP605SpeedHistrecords { get; set; }
        public virtual DbSet<DivSclSpeedHistrecord> DivSclSpeedHistrecords { get; set; }
        public virtual DbSet<DivSpeedHistrecord> DivSpeedHistrecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-V1I9QML\\SQLEXPRESS;Database=GroupHistoryDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DivBtd2SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_BTD2_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivBtd3SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_BTD3_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivBtd4SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_BTD4_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivBtd5SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_BTD5_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivD1000SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_D1000_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivD1100SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_D1100_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivD650SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_D650_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivD750SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_D750_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivGmc1LengthHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_GMC1_LENGTH_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaRealLength).HasColumnName("cola_RealLength");

                entity.Property(e => e.ColaSetupLength).HasColumnName("cola_SetupLength");
            });

            modelBuilder.Entity<DivGmc1SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_GMC1_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");

                entity.Property(e => e.ColaMaxSpeed).HasColumnName("cola_MaxSpeed");
            });

            modelBuilder.Entity<DivGmc2LengthHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_GMC2_LENGTH_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaRealLength).HasColumnName("cola_RealLength");

                entity.Property(e => e.ColaSetupLength).HasColumnName("cola_SetupLength");
            });

            modelBuilder.Entity<DivGmc2SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_GMC2_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivP5mSpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_P5M_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivP601SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_P601_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivP604SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_P604_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivP605SpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_P605_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivSclSpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_SCL_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaHistVariable).HasColumnName("cola_HistVariable");
            });

            modelBuilder.Entity<DivSpeedHistrecord>(entity =>
            {
                entity.HasKey(e => e.TriggerTime);

                entity.ToTable("DIV_SPEED_HISTRECORD");

                entity.Property(e => e.TriggerTime).HasColumnType("datetime");

                entity.Property(e => e.ColaBtd2).HasColumnName("cola_BTD2");

                entity.Property(e => e.ColaBtd3).HasColumnName("cola_BTD3");

                entity.Property(e => e.ColaBtd4).HasColumnName("cola_BTD4");

                entity.Property(e => e.ColaBtd5).HasColumnName("cola_BTD5");

                entity.Property(e => e.ColaD1000).HasColumnName("cola_D1000");

                entity.Property(e => e.ColaD1100).HasColumnName("cola_D1100");

                entity.Property(e => e.ColaD650).HasColumnName("cola_D650");

                entity.Property(e => e.ColaD750).HasColumnName("cola_D750");

                entity.Property(e => e.ColaGmc1).HasColumnName("cola_GMC1");

                entity.Property(e => e.ColaGmc2).HasColumnName("cola_GMC2");

                entity.Property(e => e.ColaP5m).HasColumnName("cola_P5M");

                entity.Property(e => e.ColaP601).HasColumnName("cola_P601");

                entity.Property(e => e.ColaP604).HasColumnName("cola_P604");

                entity.Property(e => e.ColaP605).HasColumnName("cola_P605");

                entity.Property(e => e.ColaScl).HasColumnName("cola_SCL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace mvc.Models;

//public partial class Mvcdbfirst2Context : DbContext
//{
//    public Mvcdbfirst2Context()
//    {
//    }

//    public Mvcdbfirst2Context(DbContextOptions<Mvcdbfirst2Context> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Converter> Converters { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-IN8B58B;Initial Catalog=mvcdbfirst2;Integrated Security=True;TrustServerCertificate=true;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Converter>(entity =>
//        {
//            entity.HasNoKey();

//            entity.Property(e => e.C1ConversionEfficiency).HasColumnName("C1_ConversionEfficiency");
//            entity.Property(e => e.C1Converteractivationstatus)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("C1_Converteractivationstatus");
//            entity.Property(e => e.C1Frequency).HasColumnName("C1_Frequency");
//            entity.Property(e => e.C1GenerationHour).HasColumnName("C1_GenerationHour");
//            entity.Property(e => e.C1InverterTemperature).HasColumnName("C1_InverterTemperature");
//            entity.Property(e => e.C1TotalAcpower).HasColumnName("C1_TotalACPower");
//            entity.Property(e => e.C1TotalDcpower).HasColumnName("C1_TotalDCPower");
//            entity.Property(e => e.C1TotalPowerGenerated).HasColumnName("C1_TotalPowerGenerated");
//            entity.Property(e => e.C2ConversionEfficiency).HasColumnName("C2_ConversionEfficiency");
//            entity.Property(e => e.C2Converteractivationstatus)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("C2_Converteractivationstatus");
//            entity.Property(e => e.C2Frequency).HasColumnName("C2_Frequency");
//            entity.Property(e => e.C2GenerationHour).HasColumnName("C2_GenerationHour");
//            entity.Property(e => e.C2InverterTemperature).HasColumnName("C2_InverterTemperature");
//            entity.Property(e => e.C2TotalAcpower).HasColumnName("C2_TotalACPower");
//            entity.Property(e => e.C2TotalDcpower).HasColumnName("C2_TotalDCPower");
//            entity.Property(e => e.C2TotalPowerGenerated).HasColumnName("C2_TotalPowerGenerated");
//            entity.Property(e => e.C3ConversionEfficiency).HasColumnName("C3_ConversionEfficiency");
//            entity.Property(e => e.C3Frequency).HasColumnName("C3_Frequency");
//            entity.Property(e => e.C3GenerationHour).HasColumnName("C3_GenerationHour");
//            entity.Property(e => e.C3InverterTemperature).HasColumnName("C3_InverterTemperature");
//            entity.Property(e => e.C3TotalAcpower).HasColumnName("C3_TotalACPower");
//            entity.Property(e => e.C3TotalDcpower).HasColumnName("C3_TotalDCPower");
//            entity.Property(e => e.C3TotalPowerGenerated).HasColumnName("C3_TotalPowerGenerated");
//            entity.Property(e => e.C4ConversionEfficiency).HasColumnName("C4_ConversionEfficiency");
//            entity.Property(e => e.C4Frequency).HasColumnName("C4_Frequency");
//            entity.Property(e => e.C4GenerationHour).HasColumnName("C4_GenerationHour");
//            entity.Property(e => e.C4InverterTemperature).HasColumnName("C4_InverterTemperature");
//            entity.Property(e => e.C4TotalAcpower).HasColumnName("C4_TotalACPower");
//            entity.Property(e => e.C4TotalDcpower).HasColumnName("C4_TotalDCPower");
//            entity.Property(e => e.C4TotalPowerGenerated).HasColumnName("C4_TotalPowerGenerated");
//            entity.Property(e => e.DateTime).HasColumnType("datetime");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}

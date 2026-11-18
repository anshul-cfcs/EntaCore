using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class TenantDbContext : DbContext
{
    public TenantDbContext()
    {
    }

    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BranchMaster> BranchMasters { get; set; }

    public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BranchMaster>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__BranchMa__A1682FA5B89DA969");

            entity.ToTable("BranchMaster");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchManagerId).HasColumnName("BranchManagerID");
            entity.Property(e => e.BranchManagerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeMaster>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.ToTable("EmployeeMaster");

            entity.Property(e => e.EmployeeId)
                .HasDefaultValueSql("(newid())", "DF__EmployeeM__Emplo__76969D2E")
                .HasColumnName("EmployeeID");
            entity.Property(e => e.AadharNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CareOfPersonName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CareOfTypeText)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__EmployeeM__Creat__7A672E12")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DesignationId).HasColumnName("DesignationID");
            entity.Property(e => e.Dob).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EmailID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(302)
                .IsUnicode(false)
                .HasComputedColumnSql("((Trim(isnull([FirstName]+' ',''))+isnull([MiddleName]+' ',''))+isnull([LastName],''))", true);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PanNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pfno)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PFNO");
            entity.Property(e => e.ProfilePhoto)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SignatureImage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SubManagerId).HasColumnName("SubManagerID");
            entity.Property(e => e.SuperManagerId).HasColumnName("SuperManagerID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.TitleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.EmployeeMasters)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_EmployeeMaster_BranchMaster");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_EmployeeMaster_Manager");

            entity.HasOne(d => d.SuperManager).WithMany(p => p.InverseSuperManager)
                .HasForeignKey(d => d.SuperManagerId)
                .HasConstraintName("FK_EmployeeMaster_SuperManager");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

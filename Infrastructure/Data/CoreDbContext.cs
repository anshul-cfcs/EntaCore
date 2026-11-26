using System;
using System.Collections.Generic;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class CoreDbContext : DbContext
{
    public CoreDbContext()
    {
    }

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CareOfTypeMaster> CareOfTypeMasters { get; set; }

    public virtual DbSet<CompanyMaster> CompanyMasters { get; set; }

    public virtual DbSet<ConfigValue> ConfigValues { get; set; }

    public virtual DbSet<CountryMaster> CountryMasters { get; set; }

    public virtual DbSet<DbconfigurationMaster> DbconfigurationMasters { get; set; }

    public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }

    public virtual DbSet<ErrorLogMaster> ErrorLogMasters { get; set; }

    public virtual DbSet<LoginLogMaster> LoginLogMasters { get; set; }

    public virtual DbSet<MailConfigurationMaster> MailConfigurationMasters { get; set; }

    public virtual DbSet<PermissionMaster> PermissionMasters { get; set; }

    public virtual DbSet<RoleMaster> RoleMasters { get; set; }

    public virtual DbSet<StateMaster> StateMasters { get; set; }

    public virtual DbSet<TimeZoneMaster> TimeZoneMasters { get; set; }

    public virtual DbSet<TitleMaster> TitleMasters { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserPasswordUpdateLog> UserPasswordUpdateLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CareOfTypeMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CareOfTy__3214EC2719A19520");

            entity.ToTable("CareOfTypeMaster");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyMaster>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__CompanyM__2D971C4C5E99A980");

            entity.ToTable("CompanyMaster");

            entity.HasIndex(e => e.CompanyCode, "UQ__CompanyM__11A0134B8EA740D8").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompanyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CompanyLogo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CompanyStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active", "DF__CompanyMa__Compa__4CA06362");
            entity.Property(e => e.CompanyValidFrom).HasColumnType("datetime");
            entity.Property(e => e.CompanyValidUpto).HasColumnType("datetime");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__CompanyMa__Creat__5070F446")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyId).HasColumnName("CurrencyID");
            entity.Property(e => e.FinancialYearStartMonth).HasDefaultValue((byte)4, "DF_CompanyMaster_FinancialYearStartMonth");
            entity.Property(e => e.Gstno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTNo");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF__CompanyMa__IsAct__4D94879B");
            entity.Property(e => e.IsDms).HasColumnName("IsDMS");
            entity.Property(e => e.LegalName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MailId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MailID");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PIN");
            entity.Property(e => e.PrimaryContactEmail)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryContactName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryContactPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.SubTitle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionEndDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionPlanId).HasColumnName("SubscriptionPlanID");
            entity.Property(e => e.SubscriptionStartDate).HasColumnType("datetime");
            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("WebsiteURL");
        });

        modelBuilder.Entity<ConfigValue>(entity =>
        {
            entity.HasKey(e => e.ConfigValueId).HasName("PK__ConfigVa__FAD9F86757DA6479");

            entity.Property(e => e.ConfigValueId).HasColumnName("ConfigValueID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ConfigKey)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ConfigName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.KeyType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.KeyValue)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CountryMaster>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("CountryMaster");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CountryName).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__CountryMa__Creat__69C6B1F5")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF__CountryMa__IsAct__67DE6983");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<DbconfigurationMaster>(entity =>
        {
            entity.HasKey(e => e.DbconfigurationId).HasName("PK__DBConfig__E7C2640CC1ECC1B8");

            entity.ToTable("DBConfigurationMaster");

            entity.Property(e => e.DbconfigurationId)
                .ValueGeneratedNever()
                .HasColumnName("DBConfigurationID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dbname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DBName");
            entity.Property(e => e.PwdHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UserID");
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

            entity.HasOne(d => d.Company).WithMany(p => p.EmployeeMasters)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_EmployeeMaster_CompanyMaster");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_EmployeeMaster_Manager");

            entity.HasOne(d => d.SuperManager).WithMany(p => p.InverseSuperManager)
                .HasForeignKey(d => d.SuperManagerId)
                .HasConstraintName("FK_EmployeeMaster_SuperManager");
        });

        modelBuilder.Entity<ErrorLogMaster>(entity =>
        {
            entity.HasKey(e => e.ErrorLogId);

            entity.ToTable("ErrorLogMaster");

            entity.Property(e => e.ErrorLogId).HasColumnName("ErrorLogID");
            entity.Property(e => e.ActionName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ControllerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())", "DF_ErrorLogMaster_CreatedAt")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.ExceptionMessage)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.ExceptionSource)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExceptionType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InnerExceptionMessage)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF_ErrorLogMaster_IsActive");
            entity.Property(e => e.MethodName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.QueryString)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RequestMethod)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RequestPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.StackTrace).IsUnicode(false);
            entity.Property(e => e.StatusCode).HasDefaultValue(500, "DF_ErrorLogMaster_StatusCode");
            entity.Property(e => e.TraceId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoginLogMaster>(entity =>
        {
            entity.HasKey(e => e.LoginLogId).HasName("PK__LoginLog__D42E7ACC21A36A41");

            entity.ToTable("LoginLogMaster");

            entity.Property(e => e.LoginLogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("LoginLogID");
            entity.Property(e => e.ClientIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ClientIP");
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientVersion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FailureReason)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LogOutTime).HasColumnType("datetime");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LoginTypeId).HasColumnName("LoginTypeID");
            entity.Property(e => e.Platform)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SessionId)
                .HasMaxLength(500)
                .HasColumnName("SessionID");
            entity.Property(e => e.UserAgent)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.LoginLogMasters)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_LoginLogMaster_CompanyMaster");

            entity.HasOne(d => d.Employee).WithMany(p => p.LoginLogMasters)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_LoginLogMaster_EmployeeMaster");

            entity.HasOne(d => d.UserLogin).WithMany(p => p.LoginLogMasters)
                .HasForeignKey(d => d.UserLoginId)
                .HasConstraintName("FK_LoginLogMaster_UserLogin");
        });

        modelBuilder.Entity<MailConfigurationMaster>(entity =>
        {
            entity.HasKey(e => e.MailConfigurationId).HasName("PK__MailConf__9B1218A680F2538A");

            entity.ToTable("MailConfigurationMaster");

            entity.Property(e => e.MailConfigurationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MailConfigurationID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayCompanyAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DisplayCompanyName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FromMailId)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FromMailID");
            entity.Property(e => e.FromName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SenderMailId)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SenderMailID");
            entity.Property(e => e.SenderPasswordHash)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SiteUrl)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Smtpport).HasColumnName("SMTPPort");
            entity.Property(e => e.Smtpserver)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SMTPServer");
            entity.Property(e => e.SslMode).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.MailConfigurationMasters)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Mail_Company");
        });

        modelBuilder.Entity<PermissionMaster>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB0FD9590942");

            entity.ToTable("PermissionMaster");

            entity.HasIndex(e => e.PermissionCode, "UQ__Permissi__91FE575044005719").IsUnique();

            entity.HasIndex(e => e.PermissionCode, "UQ__Permissi__91FE5750F99648AD").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModuleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PermissionCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoleMaster>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__RoleMast__8AFACE3AA059CFD0");

            entity.ToTable("RoleMaster");

            entity.HasIndex(e => e.RoleCode, "UQ__RoleMast__D62CB59C41728A53").IsUnique();

            entity.HasIndex(e => e.RoleCode, "UQ__RoleMast__D62CB59CEAAA0B6E").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsSystemRole).HasDefaultValue(true);
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<StateMaster>(entity =>
        {
            entity.HasKey(e => e.StateId);

            entity.ToTable("StateMaster");

            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__StateMast__Creat__6E8B6712")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF__StateMast__IsAct__6CA31EA0");
            entity.Property(e => e.StateCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateName).HasMaxLength(150);
            entity.Property(e => e.StateShortCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TimeZoneMaster>(entity =>
        {
            entity.HasKey(e => e.TimeZoneId);

            entity.ToTable("TimeZoneMaster");

            entity.Property(e => e.TimeZoneId).HasColumnName("TimeZoneID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TimeZoneName).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UtcOffset).HasMaxLength(20);
        });

        modelBuilder.Entity<TitleMaster>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__TitleMas__FBDF78C9BEF2F969");

            entity.ToTable("TitleMaster");

            entity.Property(e => e.RecordId)
                .ValueGeneratedNever()
                .HasColumnName("RecordID");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.EmployeeId, "UQ_UserLogin_EmployeeID").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ_UserLogin_UserName").IsUnique();

            entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");
            entity.Property(e => e.AdminResetDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())", "DF__UserLogin__Creat__08B54D69")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentToken).HasMaxLength(500);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true, "DF__UserLogin__IsAct__06CD04F7");
            entity.Property(e => e.IsAllowSms).HasColumnName("IsAllowSMS");
            entity.Property(e => e.IsDms).HasColumnName("IsDMS");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LoginActive).HasDefaultValue(true, "DF__UserLogin__Login__05D8E0BE");
            entity.Property(e => e.Otp)
                .HasMaxLength(500)
                .HasColumnName("OTP");
            entity.Property(e => e.Otpkey).HasColumnName("OTPKey");
            entity.Property(e => e.OtprequestCount).HasColumnName("OTPRequestCount");
            entity.Property(e => e.OtpvalidUpto)
                .HasColumnType("datetime")
                .HasColumnName("OTPValidUpto");
            entity.Property(e => e.OtpwrongCount).HasColumnName("OTPWrongCount");
            entity.Property(e => e.PasswordChangedAt).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithOne(p => p.UserLogin)
                .HasForeignKey<UserLogin>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogin_EmployeeMaster");

            entity.HasOne(d => d.Role).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserLogin_RoleMaster");
        });

        modelBuilder.Entity<UserPasswordUpdateLog>(entity =>
        {
            entity.HasKey(e => e.PassId);

            entity.ToTable("UserPasswordUpdateLog");

            entity.Property(e => e.PassId).HasColumnName("PassID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(1000);
            entity.Property(e => e.UserLoginId).HasColumnName("UserLoginID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

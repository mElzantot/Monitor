﻿// <auto-generated />
using System;
using ITI.CEI40.Monitor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ITI.CEI40.Monitor.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200521183429_updateDataBase")]
    partial class updateDataBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<DateTime>("EstDuration")
                        .HasColumnName("Estimated Duration");

                    b.Property<int>("FK_ProjectId");

                    b.Property<int>("FK_TeamId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Priority");

                    b.Property<float>("Progress");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("Start Date");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("FK_ProjectId");

                    b.HasIndex("FK_TeamId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("AvailableTime");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("FK_TeamID");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<float>("SalaryRate");

                    b.Property<string>("SecurityStamp");

                    b.Property<float>("TotalEvaluation");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("Workload");

                    b.HasKey("Id");

                    b.HasIndex("FK_TeamID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Claim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<int>("FK_SubTaskId");

                    b.HasKey("Id");

                    b.HasIndex("FK_SubTaskId");

                    b.ToTable("Claims");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FK_ManagerID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("FK_ManagerID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.DepartmentProjects", b =>
                {
                    b.Property<int>("DepartmentID");

                    b.Property<int>("ProjectID");

                    b.HasKey("DepartmentID", "ProjectID");

                    b.HasIndex("ProjectID");

                    b.ToTable("DepartmentProjects");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<float>("EstimatedDuration");

                    b.Property<float>("Income");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<float>("Outcome");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("Priority");

                    b.Property<float>("Progress");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<float>("TotalBudget");

                    b.HasKey("ID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.SubTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("End Date");

                    b.Property<int?>("Evaluation");

                    b.Property<string>("FK_EngineerID")
                        .IsRequired();

                    b.Property<int>("FK_TaskId");

                    b.Property<bool>("IsUnderWork");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Priority");

                    b.Property<int>("Progress");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("Start Date");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("FK_EngineerID");

                    b.HasIndex("FK_TaskId");

                    b.ToTable("SubTask");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.SubTaskSession", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_SubTaskID");

                    b.Property<int?>("SessDuration");

                    b.Property<DateTime?>("SessEndtDate")
                        .HasColumnName("End Date");

                    b.Property<DateTime>("SessStartDate")
                        .HasColumnName("Start Date");

                    b.HasKey("ID");

                    b.HasIndex("FK_SubTaskID");

                    b.ToTable("SubTaskSession");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FK_DepartmentId");

                    b.Property<string>("FK_TeamLeaderId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("FK_DepartmentId");

                    b.HasIndex("FK_TeamLeaderId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Activity", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("FK_ProjectId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ITI.CEI40.Monitor.Entities.Team", "Team")
                        .WithMany("Tasks")
                        .HasForeignKey("FK_TeamId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.ApplicationUser", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.Team", "Team")
                        .WithMany("Engineers")
                        .HasForeignKey("FK_TeamID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Claim", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.SubTask", "SubTask")
                        .WithMany("Claims")
                        .HasForeignKey("FK_SubTaskId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Department", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser", "Manager")
                        .WithMany()
                        .HasForeignKey("FK_ManagerID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.DepartmentProjects", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.Department", "Department")
                        .WithMany("DepartmentProjects")
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ITI.CEI40.Monitor.Entities.Project", "Project")
                        .WithMany("DepartmentProjects")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.SubTask", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser", "Engineer")
                        .WithMany("subTasks")
                        .HasForeignKey("FK_EngineerID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ITI.CEI40.Monitor.Entities.Activity", "Task")
                        .WithMany("SubTasks")
                        .HasForeignKey("FK_TaskId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.SubTaskSession", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.SubTask", "SubTask")
                        .WithMany("SubTaskSession")
                        .HasForeignKey("FK_SubTaskID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ITI.CEI40.Monitor.Entities.Team", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.Department", "Department")
                        .WithMany("Teams")
                        .HasForeignKey("FK_DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser", "TeamLeader")
                        .WithMany()
                        .HasForeignKey("FK_TeamLeaderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ITI.CEI40.Monitor.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

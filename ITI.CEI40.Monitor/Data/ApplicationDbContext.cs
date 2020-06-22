using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITI.CEI40.Monitor.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Activity> Tasks { get; set; }
        public virtual DbSet<SubTask> SubTasks { get; set; }
        public virtual DbSet<SubTaskSession> SubTaskSessions { get; set; }
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }  


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //-----One To one Optional Between Files and Comments
            modelBuilder.Entity<Comment>()
            .HasOne(a => a.File)
            .WithOne(b => b.Comment)
             .HasForeignKey<Files>(b => b.CommentID);

            //------One To Many RelationShip Between User and Team
            modelBuilder.Entity<ApplicationUser>()
           .HasOne<Team>(s => s.Team)
           .WithMany(t => t.Engineers)
           .HasForeignKey(s => s.FK_TeamID);


            modelBuilder.Entity<Department>()
                        .HasOne(x => x.Manager)
                        .WithMany()
                        .HasForeignKey(x => x.FK_ManagerID);

            modelBuilder.Entity<Team>()
                   .HasOne(x => x.TeamLeader)
                    .WithMany()
                   .HasForeignKey(x => x.FK_TeamLeaderId);

            //Team Tasks many to many Relship
            /*modelBuilder.Entity<TeamTasks>()
                .HasKey(TT => new { TT.TeamID, TT.TaskID });

            modelBuilder.Entity<TeamTasks>()
                .HasOne(t => t.Team)
                .WithMany(tt => tt.TeamTasks)
                .HasForeignKey(pt => pt.TeamID);

            modelBuilder.Entity<TeamTasks>()
             .HasOne(pt => pt.Task)
             .WithMany(t => t.TeamTasks)
             .HasForeignKey(pt => pt.TaskID); */

            //----------User/ Notification Many to Many

            modelBuilder.Entity<NotificationUsers>()
                .HasKey(TT => new { TT.userID, TT.NotificationId });

            modelBuilder.Entity<NotificationUsers>()
                .HasOne(t => t.User)
                .WithMany(tt => tt.NotificationUsers)
                .HasForeignKey(pt => pt.userID);

            modelBuilder.Entity<NotificationUsers>()
             .HasOne(pt => pt.Notification)
             .WithMany(t => t.NotificationUsers)
             .HasForeignKey(pt => pt.NotificationId);

            //-------Department Projects many to many Relship
            modelBuilder.Entity<DepartmentProjects>()
                .HasKey(TT => new { TT.DepartmentID, TT.ProjectID });

            modelBuilder.Entity<DepartmentProjects>()
                .HasOne(t => t.Department)
                .WithMany(tt => tt.DepartmentProjects)
                .HasForeignKey(pt => pt.DepartmentID);

            modelBuilder.Entity<DepartmentProjects>()
             .HasOne(pt => pt.Project)
             .WithMany(t => t.DepartmentProjects)
             .HasForeignKey(pt => pt.ProjectID);



            //-------Tasks Dependecy many to many Relship
            modelBuilder.Entity<Dependencies>()
                .HasKey(TT => new { TT.ActivityToFollowId, TT.FollowingActivityId });

            modelBuilder.Entity<Dependencies>()
            .Property(d => d.Lag);

            modelBuilder.Entity<Dependencies>()
                .HasOne(t => t.ActivityToFollow)
                .WithMany(tt => tt.FollowingActivities)
                .HasForeignKey(pt => pt.ActivityToFollowId);

            modelBuilder.Entity<Dependencies>()
             .HasOne(pt => pt.FollowingActivity)
             .WithMany(t => t.ActivitiesToFollow)
             .HasForeignKey(pt => pt.FollowingActivityId);

            //Engineer SubTasks many to many Relship

            //modelBuilder.Entity<EngineerSubTasks>()
            //    .HasKey(TT => new { TT.EngineerID, TT.SubTaskID });

            //modelBuilder.Entity<EngineerSubTasks>()
            //    .Property(es => es.Status)
            //    .IsRequired();

            //modelBuilder.Entity<EngineerSubTasks>()
            //    .Property(es => es.Evaluation)
            //    .IsRequired();

            //modelBuilder.Entity<EngineerSubTasks>()
            //    .HasOne(t => t.Engineer)
            //    .WithMany(tt => tt.EngineerSubTasks)
            //    .HasForeignKey(pt => pt.EngineerID);

            //modelBuilder.Entity<EngineerSubTasks>()
            // .HasOne(pt => pt.SubTask)
            // .WithMany(t => t.EngineerSubTasks)
            // .HasForeignKey(pt => pt.SubTaskID);

            //--------On Delete  options 

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(modelBuilder);
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("Task")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        public int ViewOrder { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //[Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        public Team Team { get; set; }
        [ForeignKey(nameof(Team))]
        public int? FK_TeamId { get; set; }


        public ICollection<SubTask> SubTasks { get; set; }

        public Department Department { get; set; }
        [ForeignKey(nameof(Department))]
        public int? FK_DepartmentId { get; set; }

        public Project Project { get; set; }
        [ForeignKey(nameof(Project))]
        public int FK_ProjectId { get; set; }


        [DataType(DataType.DateTime)]
        [Column("Start Date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Column("End Date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        //[DataType(DataType.DateTime)]
        [Column("Estimated Duration")]
        [Display(Name = "Estimated Duration")]
        public int EstDuration { get; set; }

        public ICollection<Dependencies> ActivitiesToFollow { set; get; }
        public ICollection<Dependencies> FollowingActivities { set; get; }

        public float Progress { get; set; }

        public Status Status { get; set; }
        public Priority Priority { get; set; }

        //public ICollection<TeamTasks> TeamTasks { get; set; }


    }
}

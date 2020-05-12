using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("Claims")]
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public SubTask SubTask { get; set; }

        [ForeignKey(nameof(SubTask))]
        public int FK_SubTaskId { get; set; }
    }
}

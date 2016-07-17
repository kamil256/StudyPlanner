namespace StudyPlanner.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Training")]
    public partial class Training
    {
        public int TrainingId { get; set; }

        public int SectionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime AddedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CompletedDate { get; set; }

        public int? UserId { get; set; }

        public virtual Section Section { get; set; }

        public virtual User User { get; set; }
    }
}

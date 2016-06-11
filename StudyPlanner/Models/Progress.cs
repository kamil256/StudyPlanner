namespace StudyPlanner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Progress")]
    public partial class Progress
    {
        public int ProgressId { get; set; }

        public int? SectionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreationDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstStepReachedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SecondStepReachedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ThirdStepReachedDate { get; set; }

        public virtual Section Section { get; set; }
    }
}

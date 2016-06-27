namespace StudyPlanner.EF
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
        public DateTime CompletionDate { get; set; }

        public int LessonsLeft { get; set; }

        public virtual Section Section { get; set; }
    }
}

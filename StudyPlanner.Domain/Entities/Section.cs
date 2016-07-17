namespace StudyPlanner.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Section")]
    public partial class Section
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Section()
        {
            Trainings = new HashSet<Training>();
        }

        public int SectionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int BookId { get; set; }

        public int StartPageNumber { get; set; }

        public int EndPageNumber { get; set; }

        public int? UserId { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Training> Trainings { get; set; }
    }
}

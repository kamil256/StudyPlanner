namespace StudyPlanner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Author")]
    public partial class Author
    {
        public int AuthorId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual Book Book { get; set; }
    }
}

namespace StudyPlanner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        public int BookId { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public int? AuthorId { get; set; }

        public int? PublicationDate { get; set; }

        public int? PublisherId { get; set; }

        public virtual Author Author { get; set; }

        public virtual Publisher Publisher { get; set; }

        public virtual Section Section { get; set; }
    }
}

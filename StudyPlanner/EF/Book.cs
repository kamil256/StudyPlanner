namespace StudyPlanner.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            AuthorOfBooks = new HashSet<AuthorOfBook>();
            Sections = new HashSet<Section>();
        }

        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public int PublisherId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Released { get; set; }

        public int Pages { get; set; }

        [StringLength(32)]
        public string Cover { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthorOfBook> AuthorOfBooks { get; set; }

        public virtual Publisher Publisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Section> Sections { get; set; }
    }
}

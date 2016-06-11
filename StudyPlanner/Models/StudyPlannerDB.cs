namespace StudyPlanner.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudyPlannerDB : DbContext
    {
        public StudyPlannerDB()
            : base("name=StudyPlannerDB")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasOptional(e => e.Book)
                .WithRequired(e => e.Author);

            modelBuilder.Entity<Book>()
                .HasOptional(e => e.Section)
                .WithRequired(e => e.Book);
        }
    }
}

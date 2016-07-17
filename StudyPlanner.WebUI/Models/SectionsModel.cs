using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class SectionsModel
    {
        public Book[] Books { get; set; }
        public Section[] Sections { get; set; }

        public StudyPlanner.Domain.Entities.Book SelectedBook { get; set; }
        public string[] SelectedBookAuthors { get; set; }

        public int? NewSectionStartPageNumber { get; set; }
        public int? NewSectionEndPageNumber { get; set; }
        public string NewSectionName { get; set; }

        public class Book
        {
            public int BookId { get; set; }
            public string Title { get; set; }
        }

        public class Section
        {
            public int SectionId { get; set; }
            public string Name { get; set; }
            public int StartPageNumber { get; set; }
            public int EndPageNumber { get; set; }
            public int NumberOfPages { get; set; }
            public int NumberOfTrainingsCompleted { get; set; }
            public bool IsTrainingInProgress { get; set; }
        }
    }
}
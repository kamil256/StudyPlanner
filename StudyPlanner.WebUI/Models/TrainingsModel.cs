using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class TrainingsModel
    {
        public int FilterBookId { get; set; }
        public int FilterSectionId { get; set; }
        public bool Pending { get; set; } = true;
        public bool Complete { get; set; } = true;
        
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public IEnumerable<Training> Trainings { get; set; }

        public enum Sorting { Lesson_age, Start_page, Total_pages };
        public Sorting SelectedSorting { get; set; } = Sorting.Lesson_age;
        public enum SortingOrder { Ascending, Descending };
        public SortingOrder SelectedSortingOrder { get; set; } = SortingOrder.Descending;

        public int Page { get; set; } = 1;
        public Pagination Pagination { get; set; }
    }
}
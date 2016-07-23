using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }

        public string SearchString { get; set; }
        public DateTime? ReleasedFrom { get; set; }
        public DateTime? ReleasedTo { get; set; }
        public int? PagesFrom { get; set; }
        public int? PagesTo { get; set; }
        public Dictionary<string, bool> SelectedAuthors { get; set; }
        public Dictionary<string, bool> SelectedPublishers { get; set; }

        public enum Sorting { Title, Released, Pages };
        public Sorting SelectedSorting { get; set; } = Sorting.Title;
        public enum SortingOrder { Ascending, Descending };
        public SortingOrder SelectedSortingOrder { get; set; } = SortingOrder.Ascending;

        public int Page { get; set; } = 1;
        public Pagination Pagination { get; set; }
    }
}
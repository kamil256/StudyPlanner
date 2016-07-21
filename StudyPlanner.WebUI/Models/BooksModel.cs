using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class BooksModel
    {
        public string FilterSearchString { get; set; }
        public DateTime? FilterReleasedFrom { get; set; }
        public DateTime? FilterReleasedTo { get; set; }
        public int? FilterPagesFrom { get; set; }
        public int? FilterPagesTo { get; set; }
        public List<string> FilterAuthors { get; set; }
        public List<string> FilterPublishers { get; set; }

        public int TotalBooksInDatabase { get; set; }
        public List<Author> Authors { get; set; }
        public List<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }

        public string SortBy { get; set; } = "Title";
        public string[] SortByItems { get; } = new[] { "Title", "Released", "Pages" };
        public string SortingOrder { get; set; } = "Ascending";
        public string[] SortingOrderItems { get; } = new[] { "Ascending", "Descending" };

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
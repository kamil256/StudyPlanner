using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyPlanner.EF;

namespace StudyPlanner.Models
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
        public List<Book> Books { get; set; }

        public string SortBy { get; set; } = "Title";
        public string[] SortByItems { get; } = new[] { "Title", "Released", "Pages" };
        public string SortingOrder { get; set; } = "Ascending";
        public string[] SortingOrderItems { get; } = new[] { "Ascending", "Descending" };

        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }

        public class Book
        {
            public int BookId { get; set; }
            public string Title { get; set; }
            public List<Author> Authors { get; set; }
            public string Publisher { get; set; }
            public int PublisherId { get; set; }
            public DateTime Released { get; set; }
            public int Pages { get; set; }
            public string Cover { get; set; }
            
            public static explicit operator BooksModel.Book(EF.Book b)
            {
                return new BooksModel.Book
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Authors = (from x in b.AuthorOfBooks orderby x.Priority select x.Author).ToList(),
                    Publisher = b.Publisher.Name,
                    PublisherId = b.PublisherId,
                    Released = b.Released,
                    Pages = b.Pages,
                    Cover = b.Cover
                };
            }
        }
    }
}
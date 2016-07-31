using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyPlanner.Domain.Entities;

namespace StudyPlanner.Domain.Abstract
{
    public interface IRepository
    {
        IEnumerable<Book> Books { get; }
        IEnumerable<Author> Authors { get; }
        IEnumerable<Publisher> Publishers { get; }
        IEnumerable<Section> Sections { get; }
        IEnumerable<Training> Trainings { get; }
        IEnumerable<AuthorOfBook> AuthorsOfBooks { get; }
        IEnumerable<User> Users { get; }

        void AddBook(string title, string[] authorsNames, string publisherName, DateTime released, int pages, byte[] coverFile, string coverFileType, string userEmail);
        void RemoveBook(int bookId);
        void UpdateBook(int bookId, string title, string[] authorsNames, string publisherName, DateTime released, int pages, byte[] coverFile, string coverFileType);
        IEnumerable<Author> GetAuthorsOfBook(Book book);
        //void AddAuthor(Author author);
        //void AddPublisher(Publisher publisher);
        void AddUser(User user);
        void AddSection(int bookId, string name, int startPageNumber, int endPageNumber, string userEmail);
        void AddTraining(int sectionId, string userEmail);
    }
}

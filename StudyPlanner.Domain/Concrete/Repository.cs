using StudyPlanner.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyPlanner.Domain.Entities;

namespace StudyPlanner.Domain.Concrete
{
    public class Repository : IRepository
    {
        private EFDbContext dbContext = new EFDbContext();

        public IEnumerable<Author> Authors
        {
            get { return dbContext.Authors; } 
        }

        public IEnumerable<AuthorOfBook> AuthorsOfBooks
        {
            get { return dbContext.AuthorsOfBooks; } 
        }

        public IEnumerable<Book> Books
        {
            get { return dbContext.Books; }
        }

        public IEnumerable<Publisher> Publishers
        {
            get { return dbContext.Publishers; } 
        }

        public IEnumerable<Section> Sections
        {
            get { return dbContext.Sections; } 
        }

        public IEnumerable<Training> Trainings
        {
            get { return dbContext.Trainings; } 
        }

        public IEnumerable<User> Users
        {
            get { return dbContext.Users; }
        }

        public void AddBook(string title, string[] authorsNames, string publisherName, DateTime released, int pages, byte[] coverFile, string coverFileType, string userEmail)
        {
            User user = dbContext.Users.First(u => u.Email == userEmail);

            List<Author> authors = new List<Author>();
            foreach (string authorName in authorsNames)
            {
                Author author = dbContext.Authors.FirstOrDefault(a => a.Name == authorName);
                if (author == null)
                {
                    author = new Author() { Name = authorName, User = user };
                    dbContext.Authors.Add(author);
                }
                authors.Add(author);
            }

            Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.Name == publisherName);
            if (publisher == null)
            {
                publisher = new Publisher() { Name = publisherName, User = user };
                dbContext.Publishers.Add(publisher);
            }

            Book book = new Book()
            {
                Title = title,
                Publisher = publisher,
                Released = released,
                Pages = pages,
                CoverImageData = coverFile,
                CoverImageMimeType = coverFileType,
                User = user
            };

            for (int priority = 0; priority < authors.Count; priority++)
            {
                dbContext.AuthorsOfBooks.Add(new AuthorOfBook()
                {
                    Book = book,
                    Author = authors[priority],
                    Priority = priority,
                    User = user
                });
            }

            dbContext.SaveChanges();
        }

        public void RemoveBook(int bookId)
        {
            Book book = dbContext.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                foreach (Section section in book.Sections)
                    dbContext.Trainings.RemoveRange(section.Trainings);
                dbContext.Sections.RemoveRange(book.Sections);
                dbContext.AuthorsOfBooks.RemoveRange(book.AuthorOfBooks);
                dbContext.Books.Remove(book);
                dbContext.SaveChanges();
                dbContext.Authors.RemoveRange(dbContext.Authors.Where(a => a.AuthorOfBooks.Count == 0));
                dbContext.Publishers.RemoveRange(dbContext.Publishers.Where(p => p.Books.Count == 0));
                dbContext.SaveChanges();
            }
        }

        public void UpdateBook(int bookId, string title, string[] authorsNames, string publisherName, DateTime released, int pages, byte[] coverFile, string coverFileType)
        {
            Book book = dbContext.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                book.Title = title;

                dbContext.AuthorsOfBooks.RemoveRange(dbContext.AuthorsOfBooks.Where(aob => aob.BookId == bookId));
                dbContext.Authors.RemoveRange(dbContext.Authors.Where(a => a.AuthorOfBooks.Count == 0));
                List<Author> authors = new List<Author>();
                foreach (string authorName in authorsNames)
                {
                    Author author = dbContext.Authors.FirstOrDefault(a => a.Name == authorName);
                    if (author == null)
                    {
                        author = new Author() { Name = authorName, User = book.User };
                        dbContext.Authors.Add(author);
                    }
                    authors.Add(author);
                }

                if (book.Publisher.Name != publisherName)
                {
                    Publisher publisher = dbContext.Publishers.FirstOrDefault(p => p.Name == publisherName);
                    if (publisher == null)
                    {
                        publisher = new Publisher() { Name = publisherName, User = book.User };
                        dbContext.Publishers.Add(publisher);
                    }
                    Publisher prevPublisher = book.Publisher;
                    book.Publisher = publisher;
                    dbContext.Publishers.Remove(prevPublisher);
                }

                book.Released = released;
                book.Pages = pages;
                if (coverFile != null)
                    book.CoverImageData = coverFile;
                if (coverFileType != null)
                    book.CoverImageMimeType = coverFileType;

                for (int priority = 0; priority < authors.Count; priority++)
                {
                    dbContext.AuthorsOfBooks.Add(new AuthorOfBook()
                    {
                        Book = book,
                        Author = authors[priority],
                        Priority = priority,
                        User = book.User
                    });
                }


                dbContext.SaveChanges();
            }

            
        }
        // parameter should be bookId
        public IEnumerable<Author> GetAuthorsOfBook(Book book)
        {
            return from aob in dbContext.AuthorsOfBooks where aob.BookId == book.BookId select aob.Author;
        }

        //public void AddAuthor(Author author)
        //{
        //    dbContext.Authors.Add(author);
        //    dbContext.SaveChanges();
        //}

        //public void AddPublisher(Publisher publisher)
        //{
        //    dbContext.Publishers.Add(publisher);
        //    dbContext.SaveChanges();
        //}

        public void AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void AddSection(int bookId, string name, int startPageNumber, int endPageNumber, string userEmail)
        {
            User user = dbContext.Users.First(u => u.Email == userEmail);
            dbContext.Sections.Add(new Section()
            {
                BookId = bookId,
                Name = name,
                StartPageNumber = startPageNumber,
                EndPageNumber = endPageNumber,
                UserId = user.UserId
            });
            dbContext.SaveChanges();
        }

        public void AddTraining(int sectionId, string userEmail)
        {
            User user = dbContext.Users.First(u => u.Email == userEmail);
            dbContext.Trainings.Add(new Training() { SectionId = sectionId, AddedDate = DateTime.Now, UserId = user.UserId });
            dbContext.SaveChanges();
        }
    }
}

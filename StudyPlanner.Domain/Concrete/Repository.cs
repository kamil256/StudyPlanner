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
            get { return dbContext.AuthorOfBooks; } 
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

        public void AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public IEnumerable<Author> GetAuthorsOfBook(Book book)
        {
            return from aob in dbContext.AuthorOfBooks where aob.BookId == book.BookId select aob.Author;
        }
    }
}

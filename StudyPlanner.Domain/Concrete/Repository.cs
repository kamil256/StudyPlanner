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

        public void AddUser(string username, string email, string hashedPassword, string salt)
        {
            dbContext.Users.Add(new User() { UserName = username, Password = hashedPassword, Salt = salt });
            dbContext.SaveChanges();
        }
    }
}

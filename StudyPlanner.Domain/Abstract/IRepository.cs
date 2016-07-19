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
        void AddUser(User user);
    }
}

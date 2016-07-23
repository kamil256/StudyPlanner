using StudyPlanner.Domain.Abstract;
using StudyPlanner.Domain.Entities;
using StudyPlanner.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.WebUI.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private IRepository repository;

        public BooksController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult List(int? selectedAuthor, int? selectedPublisher)
        {
            BooksListViewModel model = new BooksListViewModel();

            if (selectedAuthor == null)
                model.SelectedAuthors = (from a in repository.Authors
                                         orderby a.Name
                                         select new { Id = a.AuthorId.ToString(), Value = true })
                                       .ToDictionary(a => a.Id, a => a.Value);
            else
                model.SelectedAuthors = (from a in repository.Authors
                                         orderby a.Name
                                         select new { Id = a.AuthorId.ToString(), Value = selectedAuthor == a.AuthorId ? true : false })
                                       .ToDictionary(a => a.Id, a => a.Value);

            if (selectedPublisher == null)
                model.SelectedPublishers = (from p in repository.Publishers
                                            orderby p.Name
                                            select new { Id = p.PublisherId.ToString(), Value = true })
                                          .ToDictionary(p => p.Id, p => p.Value);
            else
                model.SelectedPublishers = (from p in repository.Publishers
                                            orderby p.PublisherId
                                            select new { Id = p.PublisherId.ToString(), Value = selectedPublisher == p.PublisherId ? true : false })
                                          .ToDictionary(p => p.Id, p => p.Value);

            return List(model);
        }

        [HttpPost]
        public ActionResult List(BooksListViewModel model)
        {
            model.Authors = repository.Authors.OrderBy(a => a.Name);
            model.Publishers = repository.Publishers.OrderBy(p => p.Name);
            model.Books = repository.Books;

            List<Author> selectedAuthorsList = (from sa in model.SelectedAuthors
                                                where sa.Value
                                                select repository.Authors.First(a => a.AuthorId.ToString() == sa.Key))
                                                .ToList();

            List<Publisher> selectedPublishersList = (from sp in model.SelectedPublishers
                                                      where sp.Value
                                                      select repository.Publishers.First(p => p.PublisherId.ToString() == sp.Key))
                                                      .ToList();

            if (!String.IsNullOrEmpty(model.SearchString))
                model.Books = from b in model.Books
                              where b.Title.ToLower().Contains(model.SearchString.ToLower()) ||
                                    (from a in repository.GetAuthorsOfBook(b)
                                     where a.Name.ToLower().Contains(model.SearchString.ToLower())
                                     select a)
                                     .Count() != 0
                              select b;

            if (model.ReleasedFrom != null)
                model.Books = from b in model.Books where b.Released >= model.ReleasedFrom select b;

            if (model.ReleasedTo != null)
                model.Books = from b in model.Books where b.Released <= model.ReleasedTo select b;

            if (model.PagesFrom != null)
                model.Books = from b in model.Books where b.Pages >= model.PagesFrom select b;

            if (model.PagesTo != null)
                model.Books = from b in model.Books where b.Pages <= model.PagesTo select b;

            model.Books = from b in model.Books
                          where (from a in repository.GetAuthorsOfBook(b)
                                 join sa in selectedAuthorsList on a.AuthorId equals sa.AuthorId
                                 select a)
                                 .Count() != 0 ||
                                 repository.GetAuthorsOfBook(b).Count() == 0
                          select b;

            model.Books = from b in model.Books
                          where (from p in selectedPublishersList
                                 where p.PublisherId == b.PublisherId
                                 select p)
                                 .Count() != 0
                          select b;

            switch (model.SelectedSorting)
            {
                case BooksListViewModel.Sorting.Title:
                    if (model.SelectedSortingOrder == BooksListViewModel.SortingOrder.Ascending)
                        model.Books = from b in model.Books orderby b.Title ascending select b;
                    else
                        model.Books = from b in model.Books orderby b.Title descending select b;
                    break;
                case BooksListViewModel.Sorting.Released:
                    if (model.SelectedSortingOrder == BooksListViewModel.SortingOrder.Ascending)
                        model.Books = from b in model.Books orderby b.Released ascending select b;
                    else
                        model.Books = from b in model.Books orderby b.Released descending select b;
                    break;
                case BooksListViewModel.Sorting.Pages:
                    if (model.SelectedSortingOrder == BooksListViewModel.SortingOrder.Ascending)
                        model.Books = from b in model.Books orderby b.Pages ascending select b;
                    else
                        model.Books = from b in model.Books orderby b.Pages descending select b;
                    break;
            }

            model.Pagination = new Pagination()
            {
                ItemsPerPage = 1,
                CurrentPage = model.Page,
                TotalItems = model.Books.Count()
            };
            ModelState.Remove("Page");
            model.Page = 1;

            model.Books = model.Books
                          .Skip((model.Pagination.CurrentPage - 1) * model.Pagination.ItemsPerPage)
                          .Take(model.Pagination.ItemsPerPage)
                          .ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            BooksAddBookViewModel model = new BooksAddBookViewModel();
            model.Authors = repository.Authors.OrderBy(a => a.Name);
            model.Publishers = repository.Publishers.OrderBy(p => p.Name);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddBook(BooksAddBookViewModel model)
        {
            var v = Request.Form.ToString();
            model.Authors = repository.Authors.OrderBy(a => a.Name);
            model.Publishers = repository.Publishers.OrderBy(p => p.Name);

            if (ModelState.IsValid && false)
            {
                byte[] coverFile = new byte[model.Cover.ContentLength];
                model.Cover.InputStream.Read(coverFile, 0, coverFile.Length);
                repository.AddBook(model.Title, model.Author, model.Publisher, model.Released ?? default(DateTime), model.Pages ?? 0, coverFile, model.Cover.ContentType, User.Identity.Name);
                return RedirectToAction("List", "Books");
            }
            else
            {
                return View(model);
            }
        }

        //public ActionResult RemoveBook(int BookId)
        //{
        //    var book = (from b in repository.Books where b.BookId == BookId select b).First();
        //    var sections = from s in repository.Sections where s.BookId == BookId select s;
        //    var trainings = from t in repository.Trainings join s in sections on t.Section.SectionId equals s.SectionId select t;
        //    var authorsOfBook = from aob in repository.AuthorsOfBooks where aob.BookId == BookId select aob;
        //    var publisher = (from p in repository.Publishers where p.PublisherId == book.PublisherId select p).First();

        //    if ((from b in repository.Books where b.PublisherId == publisher.PublisherId select b).Count() == 1)
        //        repository.Publishers.Remove(publisher);
        //    repository.AuthorsOfBooks.RemoveRange(authorsOfBook);
        //    repository.Trainings.RemoveRange(trainings);
        //    repository.Sections.RemoveRange(sections);
        //    repository.Books.Remove(book);
        //    repository.SaveChanges();

        //    var authorsWithoutBooks = from a in repository.Authors join aob in repository.AuthorsOfBooks on a.AuthorId equals aob.AuthorId into x from y in x.DefaultIfEmpty() where x.Count() == 0 select a;
        //    repository.Authors.RemoveRange(authorsWithoutBooks);
        //    repository.SaveChanges();
        //    return RedirectToAction("Books");
        //}
    }
}
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
            ViewBag.Title = "Add new book";
            AddBookModel model = new AddBookModel();
            model.AuthorsList = (from a in repository.Authors orderby a.Name select a.Name).ToArray();
            model.PublishersList = (from p in repository.Publishers orderby p.Name select p.Name).ToArray();
            //model.Authors = new List<string>();
            //model.Authors.Add("empty");
            return View(model);
        }

        //[HttpPost]
        //public ActionResult AddBook(AddBookModel model)
        //{
        //    if (model.AddCover)
        //    {
        //        string extension = Path.GetExtension(model.cover.FileName);
        //        if (extension == "")
        //            extension = ".jpg";
        //        string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

        //        model.cover.SaveAs(Path.Combine(Server.MapPath("~"), @"Covers\Temp", filename));
        //        model.CoverName = filename;
        //    }
        //    //ViewBag.cover = Path.GetExtension(model.cover.FileName) == "" ? "pusty" : ":(";
        //    //model.cover.SaveAs(@"D:\pliczunio.rtf");

        //    //using (FileStream fs = new FileStream(@"D:\pliczek.rtf", FileMode.Create))
        //    //{
        //    //    model.cover.InputStream.CopyTo(fs);
        //    //}
        //    model.AuthorsList = (from a in repository.Authors orderby a.Name select a.Name).ToArray();
        //    model.PublishersList = (from p in repository.Publishers orderby p.Name select p.Name).ToArray();

        //    if (model.AddAuthor && model.Author.Trim().Length != 0)
        //    {
        //        if (model.Authors == null)
        //            model.Authors = new List<string>();
        //        if (!model.Authors.Contains(model.Author))
        //            model.Authors.Add(model.Author);
        //    }
        //    if (!String.IsNullOrEmpty(model.RemoveAuthor))
        //    {
        //        if (model.Authors != null)
        //        {

        //            model.Authors.Remove(model.RemoveAuthor);                    
        //        }
        //    }

        //    if (!model.AddAuthor && !model.AddCover && String.IsNullOrEmpty(model.RemoveAuthor) && model.Authors != null && model.Authors.Count > 0 && !String.IsNullOrEmpty(model.CoverName) && ModelState.IsValid)
        //    {
        //        //string extension = Path.GetExtension(model.cover.FileName);
        //        //if (extension == "")
        //        //    extension = ".jpg";
        //        //string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

        //        //model.cover.SaveAs(Path.Combine(Server.MapPath("~"), "Covers", filename));
        //        System.IO.File.Move(Path.Combine(Server.MapPath("~"), @"Covers\Temp", model.CoverName), Path.Combine(Server.MapPath("~"), "Covers", model.CoverName));

        //        Publisher publisher = new Publisher { Name = model.Publisher };
        //        if ((from p in repository.Publishers where publisher.Name == p.Name select p).Count() == 0)
        //            publisher = repository.Publishers.Add(publisher);
        //        else
        //            publisher = (from p in repository.Publishers where publisher.Name == p.Name select p).First();

        //        Book book = new Book
        //        {
        //            Title = model.Title,
        //            Publisher = publisher,
        //            Released = model.Released ?? new DateTime(1, 1, 1),
        //            Pages = model.Pages ?? 0,
        //            Cover = model.CoverName
        //        };
        //        if ((from b in repository.Books where b.Title == book.Title && b.Publisher.PublisherId == book.Publisher.PublisherId && b.Released == book.Released && b.Pages == book.Pages select b).Count() != 0)
        //            throw new Exception("Taka książka już istnieje w bazie danych");
        //        else
        //            book = repository.Books.Add(book);

        //        for (int i = 0; i < model.Authors.Count; i++)
        //        {
        //            Author author = new Author { Name = model.Authors[i] };
        //            if ((from a in repository.Authors where a.Name == author.Name select a).Count() == 0)
        //                author = repository.Authors.Add(author);
        //            else
        //                author = (from a in repository.Authors where a.Name == author.Name select a).First();

        //            repository.AuthorOfBooks.Add(new AuthorOfBook { Author = author, Book = book, Priority = i });
        //        }

        //        repository.SaveChanges();
        //        return RedirectToAction("Books");
        //    }
        //    else
        //    {
        //        ViewBag.Title = "Add new book";
        //        return View(model);
        //    }
        //}

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
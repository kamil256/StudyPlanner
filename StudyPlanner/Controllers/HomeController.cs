using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyPlanner.Models;
using StudyPlanner.EF;
using System.IO;

namespace StudyPlanner.Controllers
{
    public class HomeController : Controller
    {
        private StudyPlannerDB db = new StudyPlannerDB();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Books(int selectAuthor = 0, int selectPublisher = 0)
        {
            BooksModel model = new BooksModel();
            
            model.FilterAuthors = new List<string>();
            foreach (var x in db.Authors.OrderBy(x => x.Name))
            {
                if (selectAuthor != 0)
                {
                    if (selectAuthor == x.AuthorId)
                        model.FilterAuthors.Add("true");
                    else
                        model.FilterAuthors.Add("false");
                }
                else
                    model.FilterAuthors.Add("true");
            }

            model.FilterPublishers = new List<string>();
            foreach (var x in db.Publishers.OrderBy(x => x.Name))
            {
                if (selectPublisher != 0)
                {
                    if (selectPublisher == x.PublisherId)
                        model.FilterPublishers.Add("true");
                    else
                        model.FilterPublishers.Add("false");
                }
                else
                    model.FilterPublishers.Add("true");
            }
            return Books(model);
        }

        [HttpPost]
        public ActionResult Books(BooksModel model)
        {
            ViewBag.Title = "Books";
            model.TotalBooksInDatabase = db.Books.Count();
            model.Authors = (from a in db.Authors orderby a.Name select a).ToList();
            // Create list of authors depending selected checkboxes
            List<Author> filteredAuthors = new List<Author>();
            for (int i = 0; i < model.Authors.Count; i++)
                if (model.FilterAuthors[i].ToLower() == bool.TrueString.ToLower())
                    filteredAuthors.Add(model.Authors[i]);

            model.Publishers = (from p in db.Publishers orderby p.Name select p).ToList();
            // Create list of publishers depending selected checkboxes
            List<Publisher> filteredPublishers = new List<Publisher>();
            for (int i = 0; i < model.Publishers.Count; i++)
                if (model.FilterPublishers[i].ToLower() == bool.TrueString.ToLower())
                    filteredPublishers.Add(model.Publishers[i]);

            // Create list of books depending on filter values
            model.Books = new List<BooksModel.Book>();
            foreach (var x in db.Books.ToList())
            {
                var book = (BooksModel.Book)x;
                if ((from a in book.Authors join fa in filteredAuthors on a.AuthorId equals fa.AuthorId select a).Count() == 0)
                    if ((from ab in db.AuthorOfBooks where ab.Book.BookId == book.BookId select ab).Count() != 0) // Jeśli książka nie ma autorów, to jej nie odrzucaj w tym kroku
                    continue;

                if ((from fb in filteredPublishers where book.PublisherId == fb.PublisherId select fb).Count() == 0)
                    continue;

                if (!String.IsNullOrEmpty(model.FilterSearchString))
                    if (!book.Title.ToLower().Contains(model.FilterSearchString.ToLower()) && (from a in book.Authors where a.Name.ToLower().Contains(model.FilterSearchString.ToLower()) select a).Count() == 0)
                        continue;

                if (book.Released < model.FilterReleasedFrom || book.Released > model.FilterReleasedTo)
                    continue;

                if (book.Pages < model.FilterPagesFrom || book.Pages > model.FilterPagesTo)
                    continue;

                model.Books.Add(book);
            }

            switch (model.SortBy)
            {
                case "Title":
                    if (model.SortingOrder == "Ascending")
                        model.Books = (from b in model.Books orderby b.Title select b).ToList();
                    if (model.SortingOrder == "Descending")
                        model.Books = (from b in model.Books orderby b.Title descending select b).ToList();
                    break;
                case "Released":
                    if (model.SortingOrder == "Ascending")
                        model.Books = (from b in model.Books orderby b.Released select b).ToList();
                    if (model.SortingOrder == "Descending")
                        model.Books = (from b in model.Books orderby b.Released descending select b).ToList();
                    break;
                case "Pages":
                    if (model.SortingOrder == "Ascending")
                        model.Books = (from b in model.Books orderby b.Pages select b).ToList();
                    if (model.SortingOrder == "Descending")
                        model.Books = (from b in model.Books orderby b.Pages descending select b).ToList();
                    break;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            ViewBag.Title = "Add new book";
            AddBookModel model = new AddBookModel();
            model.AuthorsList = (from a in db.Authors orderby a.Name select a.Name).ToArray();
            model.PublishersList = (from p in db.Publishers orderby p.Name select p.Name).ToArray();
            //model.Authors = new List<string>();
            //model.Authors.Add("empty");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddBook(AddBookModel model)
        {
            if (model.AddCover)
            {
                string extension = Path.GetExtension(model.cover.FileName);
                if (extension == "")
                    extension = ".jpg";
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                model.cover.SaveAs(Path.Combine(Server.MapPath("~"), @"Covers\Temp", filename));
                model.CoverName = filename;
            }
            //ViewBag.cover = Path.GetExtension(model.cover.FileName) == "" ? "pusty" : ":(";
            //model.cover.SaveAs(@"D:\pliczunio.rtf");

            //using (FileStream fs = new FileStream(@"D:\pliczek.rtf", FileMode.Create))
            //{
            //    model.cover.InputStream.CopyTo(fs);
            //}
            model.AuthorsList = (from a in db.Authors orderby a.Name select a.Name).ToArray();
            model.PublishersList = (from p in db.Publishers orderby p.Name select p.Name).ToArray();

            if (model.AddAuthor && model.Author.Trim().Length != 0)
            {
                if (model.Authors == null)
                    model.Authors = new List<string>();
                if (!model.Authors.Contains(model.Author))
                    model.Authors.Add(model.Author);
            }
            if (!String.IsNullOrEmpty(model.RemoveAuthor))
            {
                if (model.Authors != null)
                {
                    
                    model.Authors.Remove(model.RemoveAuthor);                    
                }
            }

            if (!model.AddAuthor && !model.AddCover && String.IsNullOrEmpty(model.RemoveAuthor) && model.Authors != null && model.Authors.Count > 0 && !String.IsNullOrEmpty(model.CoverName) && ModelState.IsValid)
            {
                //string extension = Path.GetExtension(model.cover.FileName);
                //if (extension == "")
                //    extension = ".jpg";
                //string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;

                //model.cover.SaveAs(Path.Combine(Server.MapPath("~"), "Covers", filename));
                System.IO.File.Move(Path.Combine(Server.MapPath("~"), @"Covers\Temp", model.CoverName), Path.Combine(Server.MapPath("~"), "Covers", model.CoverName));

                Publisher publisher = new Publisher { Name = model.Publisher };
                if ((from p in db.Publishers where publisher.Name == p.Name select p).Count() == 0)
                    publisher = db.Publishers.Add(publisher);
                else
                    publisher = (from p in db.Publishers where publisher.Name == p.Name select p).First();

                Book book = new Book
                {
                    Title = model.Title,
                    Publisher = publisher,
                    Released = model.Released ?? new DateTime(1, 1, 1),
                    Pages = model.Pages ?? 0,
                    Cover = model.CoverName
                };
                if ((from b in db.Books where b.Title == book.Title && b.Publisher.PublisherId == book.Publisher.PublisherId && b.Released == book.Released && b.Pages == book.Pages select b).Count() != 0)
                    throw new Exception("Taka książka już istnieje w bazie danych");
                else
                    book = db.Books.Add(book);

                for (int i = 0; i < model.Authors.Count; i++)
                {
                    Author author = new Author { Name = model.Authors[i] };
                    if ((from a in db.Authors where a.Name == author.Name select a).Count() == 0)
                        author = db.Authors.Add(author);
                    else
                        author = (from a in db.Authors where a.Name == author.Name select a).First();

                    db.AuthorOfBooks.Add(new AuthorOfBook { Author = author, Book = book, Priority = i });
                }

                db.SaveChanges();
                return RedirectToAction("Books");
            }
            else
            {
                ViewBag.Title = "Add new book";
                return View(model);
            }
        }

        public ActionResult RemoveBook(int BookId)
        {
            var book = (from b in db.Books where b.BookId == BookId select b).First();
            var sections = from s in db.Sections where s.BookId == BookId select s;
            var trainings = from t in db.Trainings join s in sections on t.Section.SectionId equals s.SectionId select t;
            var authorsOfBook = from aob in db.AuthorOfBooks where aob.BookId == BookId select aob;
            var publisher = (from p in db.Publishers where p.PublisherId == book.PublisherId select p).First();

            if ((from b in db.Books where b.PublisherId == publisher.PublisherId select b).Count() == 1)
                db.Publishers.Remove(publisher);
            db.AuthorOfBooks.RemoveRange(authorsOfBook);
            db.Trainings.RemoveRange(trainings);
            db.Sections.RemoveRange(sections);
            db.Books.Remove(book);
            db.SaveChanges();

            var authorsWithoutBooks = from a in db.Authors join aob in db.AuthorOfBooks on a.AuthorId equals aob.AuthorId into x from y in x.DefaultIfEmpty() where x.Count() == 0 select a;
            db.Authors.RemoveRange(authorsWithoutBooks);
            db.SaveChanges();
            return RedirectToAction("Books");
        }

        [HttpGet]
        public ActionResult Sections(int BookId = 0)
        {
            ViewBag.Title = "Sections";
            SectionsModel model = new SectionsModel();
            model.Books = (from b in db.Books
                           orderby b.Title
                           select new SectionsModel.Book
                           {
                               BookId = b.BookId,
                               Title = b.Title
                           }).ToArray();
            if (BookId == 0)
                model.SelectedBook = (from b in db.Books orderby b.Title select b).FirstOrDefault();
            else
                model.SelectedBook = (from b in db.Books where b.BookId == BookId orderby b.Title select b).FirstOrDefault();
            if (model.SelectedBook != null)
            {
                model.SelectedBookAuthors = (from ab in db.AuthorOfBooks
                                             where ab.BookId == model.SelectedBook.BookId
                                             orderby ab.Priority
                                             select ab.Author.Name).ToArray();
                model.Sections = (from s in db.Sections
                                  where s.BookId == model.SelectedBook.BookId
                                  orderby s.StartPageNumber
                                  select new Models.SectionsModel.Section
                                  {
                                      SectionId = s.SectionId,
                                      Name = s.Name,
                                      StartPageNumber = s.StartPageNumber,
                                      EndPageNumber = s.EndPageNumber,
                                      NumberOfPages = s.EndPageNumber - s.StartPageNumber + 1,
                                      NumberOfTrainingsCompleted = (from t in db.Trainings
                                                                    where s.SectionId == t.SectionId && t.LessonsLeft == 0
                                                                    select t).Count(),
                                      IsTrainingInProgress = (from t in db.Trainings
                                                              where s.SectionId == t.SectionId && t.LessonsLeft > 0
                                                              select t).Count() == 0 ? false : true
                                  }).ToArray();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Sections(int BookId, SectionsModel model)
        {
            ViewBag.Title = "Sections";
            Book book = (from b in db.Books
                         where b.BookId == BookId
                         select b).FirstOrDefault();
            if (book != null &&
                model.NewSectionStartPageNumber >= 1 &&
                model.NewSectionEndPageNumber <= book.Pages &&
                model.NewSectionStartPageNumber <= model.NewSectionEndPageNumber &&
                !String.IsNullOrWhiteSpace(model.NewSectionName))
            {
                db.Sections.Add(new Section
                {
                    BookId = BookId,
                    StartPageNumber = model.NewSectionStartPageNumber ?? 0,
                    EndPageNumber = model.NewSectionEndPageNumber ?? 0,
                    Name = model.NewSectionName
                });
                db.SaveChanges();
            }
            return RedirectToAction("Sections", new { BookId = BookId });
        }

        public ActionResult Trainings(TrainingsModel model)
        {
            ViewBag.Title = "Trainings";
            
            model.Books = new List<Book>();
            model.Books.Add(new Book { BookId = 0, Title = "All" });
            model.Books.AddRange(db.Books.ToList());

            model.Sections = new List<Section>();
            model.Sections.Add(new Section { SectionId = 0, Name = "All" });
            if (model.FilterBookId != 0)
                model.Sections.AddRange((from x in db.Sections where x.BookId == model.FilterBookId select x).ToList());
            
            List<EF.Training> trainings = db.Trainings.ToList();
            if (model.FilterBookId != 0)
                trainings = (from x in trainings where x.Section.BookId == model.FilterBookId select x).ToList();
            if (model.FilterSectionId != 0)
                trainings = (from x in trainings where x.SectionId == model.FilterSectionId select x).ToList();

            model.Trainings = new List<TrainingsModel.Training>();
            foreach (EF.Training x in trainings)
                model.Trainings.Add((TrainingsModel.Training)x);
            if (model.FilterDaysSinceLastActivityFrom != null)
                model.Trainings = (from x in model.Trainings where x.DaysSinceLastActivity >= model.FilterDaysSinceLastActivityFrom select x).ToList();
            if (model.FilterDaysSinceLastActivityTo != null)
                model.Trainings = (from x in model.Trainings where x.DaysSinceLastActivity <= model.FilterDaysSinceLastActivityTo select x).ToList();
            if (!model.FilterTrainingCompleted)
                model.Trainings = (from x in model.Trainings where x.LessonsLeft != 0 select x).ToList();
            if (!model.Filter1LessonLeft)
                model.Trainings = (from x in model.Trainings where x.LessonsLeft != 1 select x).ToList();
            if (!model.Filter2LessonsLeft)
                model.Trainings = (from x in model.Trainings where x.LessonsLeft != 2 select x).ToList();
            if (!model.Filter3LessonsLeft)
                model.Trainings = (from x in model.Trainings where x.LessonsLeft != 3 select x).ToList();
            
            return View(model);
        }

        public ActionResult AddTraining(int BookId, int SectionId)
        {
            if ((from x in db.Trainings where x.SectionId == SectionId && x.LessonsLeft > 0 select x).Count() == 0)
            {
                db.Trainings.Add(new Training() { SectionId = SectionId, CompletionDate = DateTime.Now, LessonsLeft = 3 });
                db.SaveChanges();
            }
            return RedirectToAction("Sections", new { BookId = BookId });
        }
    }
}
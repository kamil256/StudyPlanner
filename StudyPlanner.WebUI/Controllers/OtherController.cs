using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.Controllers
{
    public class OtherController : Controller
    {
        //[HttpGet]
        //public ActionResult Sections(int BookId = 0)
        //{
        //    ViewBag.Title = "Sections";
        //    SectionsModel model = new SectionsModel();
        //    model.Books = (from b in repository.Books
        //                   orderby b.Title
        //                   select new SectionsModel.Book
        //                   {
        //                       BookId = b.BookId,
        //                       Title = b.Title
        //                   }).ToArray();
        //    if (BookId == 0)
        //        model.SelectedBook = (from b in repository.Books orderby b.Title select b).FirstOrDefault();
        //    else
        //        model.SelectedBook = (from b in repository.Books where b.BookId == BookId orderby b.Title select b).FirstOrDefault();
        //    if (model.SelectedBook != null)
        //    {
        //        model.SelectedBookAuthors = (from ab in repository.AuthorsOfBooks
        //                                     where ab.BookId == model.SelectedBook.BookId
        //                                     orderby ab.Priority
        //                                     select ab.Author.Name).ToArray();
        //        model.Sections = (from s in repository.Sections
        //                          where s.BookId == model.SelectedBook.BookId
        //                          orderby s.StartPageNumber
        //                          select new Models.SectionsModel.Section
        //                          {
        //                              SectionId = s.SectionId,
        //                              Name = s.Name,
        //                              StartPageNumber = s.StartPageNumber,
        //                              EndPageNumber = s.EndPageNumber,
        //                              NumberOfPages = s.EndPageNumber - s.StartPageNumber + 1,
        //                              NumberOfTrainingsCompleted = (from t in repository.Trainings
        //                                                            where s.SectionId == t.SectionId && t.LessonsLeft == 0
        //                                                            select t).Count(),
        //                              IsTrainingInProgress = (from t in repository.Trainings
        //                                                      where s.SectionId == t.SectionId && t.LessonsLeft > 0
        //                                                      select t).Count() == 0 ? false : true
        //                          }).ToArray();
        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Sections(int BookId, SectionsModel model)
        //{
        //    ViewBag.Title = "Sections";
        //    Book book = (from b in repository.Books
        //                 where b.BookId == BookId
        //                 select b).FirstOrDefault();
        //    if (book != null &&
        //        model.NewSectionStartPageNumber >= 1 &&
        //        model.NewSectionEndPageNumber <= book.Pages &&
        //        model.NewSectionStartPageNumber <= model.NewSectionEndPageNumber &&
        //        !String.IsNullOrWhiteSpace(model.NewSectionName))
        //    {
        //        repository.Sections.Add(new Section
        //        {
        //            BookId = BookId,
        //            StartPageNumber = model.NewSectionStartPageNumber ?? 0,
        //            EndPageNumber = model.NewSectionEndPageNumber ?? 0,
        //            Name = model.NewSectionName
        //        });
        //        repository.SaveChanges();
        //    }
        //    return RedirectToAction("Sections", new { BookId = BookId });
        //}

        //public ActionResult Trainings(TrainingsModel model)
        //{
        //    ViewBag.Title = "Trainings";

        //    model.Books = new List<Book>();
        //    model.Books.Add(new Book { BookId = 0, Title = "All" });
        //    model.Books.AddRange(repository.Books.ToList());

        //    model.Sections = new List<Section>();
        //    model.Sections.Add(new Section { SectionId = 0, Name = "All" });
        //    if (model.FilterBookId != 0)
        //        model.Sections.AddRange((from x in repository.Sections where x.BookId == model.FilterBookId select x).ToList());

        //    List<EF.Training> trainings = repository.Trainings.ToList();
        //    if (model.FilterBookId != 0)
        //        trainings = (from x in trainings where x.Section.BookId == model.FilterBookId select x).ToList();
        //    if (model.FilterSectionId != 0)
        //        trainings = (from x in trainings where x.SectionId == model.FilterSectionId select x).ToList();

        //    model.Trainings = new List<TrainingsModel.Training>();
        //    foreach (EF.Training x in trainings)
        //        model.Trainings.Add((TrainingsModel.Training)x);
        //    //if (model.FilterDaysSinceLastActivityFrom != null)
        //    //    model.Trainings = (from x in model.Trainings where x.TimeSinceLastActivity >= model.FilterDaysSinceLastActivityFrom select x).ToList();
        //    //if (model.FilterDaysSinceLastActivityTo != null)
        //    //    model.Trainings = (from x in model.Trainings where x.TimeSinceLastActivity <= model.FilterDaysSinceLastActivityTo select x).ToList();
        //    if (!model.FilterTrainingCompleted)
        //        model.Trainings = (from x in model.Trainings where x.LessonsLeft != 0 select x).ToList();
        //    if (!model.Filter1LessonLeft)
        //        model.Trainings = (from x in model.Trainings where x.LessonsLeft != 1 select x).ToList();
        //    if (!model.Filter2LessonsLeft)
        //        model.Trainings = (from x in model.Trainings where x.LessonsLeft != 2 select x).ToList();
        //    if (!model.Filter3LessonsLeft)
        //        model.Trainings = (from x in model.Trainings where x.LessonsLeft != 3 select x).ToList();

        //    return View(model);
        //}

        //public ActionResult AddTraining(int BookId, int SectionId)
        //{
        //    if ((from x in repository.Trainings where x.SectionId == SectionId && x.LessonsLeft > 0 select x).Count() == 0)
        //    {
        //        repository.Trainings.Add(new Training() { SectionId = SectionId, CompletionDate = DateTime.Now, LessonsLeft = 3 });
        //        repository.SaveChanges();
        //    }
        //    return RedirectToAction("Sections", new { BookId = BookId });
        //}

        //public ActionResult AddProgress(int TrainingId)
        //{
        //    var training = (from t in repository.Trainings where t.TrainingId == TrainingId select t).FirstOrDefault();
        //    if (training.LessonsLeft > 0)
        //        training.LessonsLeft--;
        //    training.CompletionDate = DateTime.Now;
        //    repository.SaveChanges();
        //    return RedirectToAction("Trainings");
        //}
    }
}
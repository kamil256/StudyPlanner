using StudyPlanner.Domain.Abstract;
using StudyPlanner.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.WebUI.Controllers
{
    public class SectionsController : Controller
    {
        private IRepository repository;

        public SectionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult List(int BookId = 0)
        {
            ViewBag.Title = "Sections";
            SectionsModel model = new SectionsModel();
            model.Books = (from b in repository.Books
                           orderby b.Title
                           select new SectionsModel.Book
                           {
                               BookId = b.BookId,
                               Title = b.Title
                           }).ToArray();
            if (BookId == 0)
                model.SelectedBook = (from b in repository.Books orderby b.Title select b).FirstOrDefault();
            else
                model.SelectedBook = (from b in repository.Books where b.BookId == BookId orderby b.Title select b).FirstOrDefault();
            if (model.SelectedBook != null)
            {
                model.SelectedBookAuthors = (from ab in repository.AuthorsOfBooks
                                             where ab.BookId == model.SelectedBook.BookId
                                             orderby ab.Priority
                                             select ab.Author.Name).ToArray();
                model.Sections = (from s in repository.Sections
                                  where s.BookId == model.SelectedBook.BookId
                                  orderby s.StartPageNumber
                                  select new Models.SectionsModel.Section
                                  {
                                      SectionId = s.SectionId,
                                      Name = s.Name,
                                      StartPageNumber = s.StartPageNumber,
                                      EndPageNumber = s.EndPageNumber,
                                      NumberOfPages = s.EndPageNumber - s.StartPageNumber + 1,
                                      NumberOfTrainingsCompleted = (from t in repository.Trainings
                                                                    where s.SectionId == t.SectionId// && t.LessonsLeft == 0
                                                                    select t).Count(),
                                      IsTrainingInProgress = (from t in repository.Trainings
                                                              where s.SectionId == t.SectionId// && t.LessonsLeft > 0
                                                              select t).Count() == 0 ? false : true
                                  }).ToArray();
            }
            return View(model);
        }

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
    }
}
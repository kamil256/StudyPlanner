using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyPlanner.WebUI.Models;
using StudyPlanner.Domain.Entities;
using StudyPlanner.Domain.Abstract;

namespace StudyPlanner.WebUI.Controllers
{
    public class TrainingsController : Controller
    {
        private IRepository repository;

        public TrainingsController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult List(TrainingsModel model)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == model.FilterBookId);
            model.Books = new[] { new Book() { BookId = 0, Title = "All" } }.Concat(repository.Books.OrderBy(b => b.Title));
            model.Sections = new[] { new Section() { SectionId = 0, Name = "All" } };
            if (book != null)
                model.Sections = model.Sections.Concat(book.Sections.OrderBy(s => s.StartPageNumber));

            model.Trainings = repository.Trainings;
            if (model.FilterBookId != 0)
            {
                model.Trainings = model.Trainings.Where(t => t.Section.BookId == model.FilterBookId);
                if (model.FilterSectionId != 0)
                    model.Trainings = model.Trainings.Where(t => t.SectionId == model.FilterSectionId);
            }

            if (!model.Complete)
                model.Trainings = model.Trainings.Where(t => t.CompletedDate == null);
            if (!model.Pending)
                model.Trainings = model.Trainings.Where(t => t.CompletedDate != null);

            switch (model.SelectedSorting)
            {
                case TrainingsModel.Sorting.Lesson_age:
                    if (model.SelectedSortingOrder == TrainingsModel.SortingOrder.Ascending)
                        model.Trainings =  from t in model.Trainings
                                           orderby (t.CompletedDate != null ? t.CompletedDate : t.AddedDate) descending
                                           select t;
                    else
                        model.Trainings = from t in model.Trainings
                                          orderby (t.CompletedDate != null ? t.CompletedDate : t.AddedDate) ascending
                                          select t;
                    break;
                case TrainingsModel.Sorting.Start_page:
                    if (model.SelectedSortingOrder == TrainingsModel.SortingOrder.Ascending)
                        model.Trainings = from t in model.Trainings
                                          orderby t.Section.StartPageNumber ascending
                                          select t;
                    else
                        model.Trainings = from t in model.Trainings
                                          orderby t.Section.StartPageNumber descending
                                          select t;
                    break;
                case TrainingsModel.Sorting.Total_pages:
                    if (model.SelectedSortingOrder == TrainingsModel.SortingOrder.Ascending)
                        model.Trainings = from t in model.Trainings
                                          orderby t.Section.EndPageNumber - t.Section.StartPageNumber ascending
                                          select t;
                    else
                        model.Trainings = from t in model.Trainings
                                          orderby t.Section.EndPageNumber - t.Section.StartPageNumber descending
                                          select t;
                    break;
            }

            return View(model);
        }

        public ActionResult AddTraining(int BookId, int SectionId)
        {
            repository.AddTraining(SectionId, User.Identity.Name);
            return RedirectToAction("List", "Sections", new { BookId = BookId });
        }

        [HttpPost]
        public JsonResult CompleteLesson(int trainingId)
        {
            repository.CompleteLesson(trainingId, User.Identity.Name);
            // Add completionDate to training
            // Should return to returnUrl argument
            return Json(new { success = "success" });
        }
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
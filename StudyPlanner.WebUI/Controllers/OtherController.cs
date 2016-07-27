using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.Controllers
{
    public class OtherController : Controller
    {
        

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
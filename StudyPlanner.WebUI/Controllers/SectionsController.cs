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
    public class SectionsController : Controller
    {
        private IRepository repository;

        public SectionsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult List(int? bookId)
        {
            SectionsListViewModel model = new SectionsListViewModel();
            model.Books = repository.GetBooks(User.Identity.Name).OrderBy(b => b.Title);
            if (bookId == null)
                model.BookId = model.Books.FirstOrDefault().BookId;
            return View(model);
        }

        [HttpPost]
        public ActionResult List(SectionsListViewModel model)
        {
            Book book = repository.GetBooks(User.Identity.Name).FirstOrDefault(b => b.BookId == model.BookId);
            if (book != null &&
                model.NewSectionStartPageNumber >= 1 &&
                model.NewSectionEndPageNumber <= book.Pages &&
                model.NewSectionStartPageNumber <= model.NewSectionEndPageNumber &&
                !String.IsNullOrWhiteSpace(model.NewSectionName))
            {
                repository.AddSection(model.BookId ?? 0, model.NewSectionName, model.NewSectionStartPageNumber ?? 0, model.NewSectionEndPageNumber ?? 0, User.Identity.Name);
            }
            return RedirectToAction("List", new { BookId = model.BookId });
        }
    }
}
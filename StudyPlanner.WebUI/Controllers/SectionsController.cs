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
            model.Books = repository.Books.OrderBy(b => b.Title);
            if (bookId == null)
                model.SelectedBookId = repository.Books.FirstOrDefault().BookId;
            else
                model.SelectedBookId = repository.Books.FirstOrDefault(b => b.BookId == bookId).BookId;
            if (model.SelectedBookId != null)
            {
                model.SelectedBookAuthors = repository.GetAuthorsOfBook(repository.Books.FirstOrDefault(b => b.BookId == model.SelectedBookId)).Select(a => a.Name);
                model.Sections = repository.Books.FirstOrDefault(b => b.BookId == model.SelectedBookId).Sections;
                if (model.Sections != null)
                    model.Sections = model.Sections.OrderBy(s => s.StartPageNumber);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult List(SectionsListViewModel model)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == model.SelectedBookId);
            if (book != null &&
                model.NewSectionStartPageNumber >= 1 &&
                model.NewSectionEndPageNumber <= book.Pages &&
                model.NewSectionStartPageNumber <= model.NewSectionEndPageNumber &&
                !String.IsNullOrWhiteSpace(model.NewSectionName))
            {
                repository.AddSection(model.SelectedBookId ?? 0, model.NewSectionName, model.NewSectionStartPageNumber ?? 0, model.NewSectionEndPageNumber ?? 0, User.Identity.Name);
            }
            return RedirectToAction("List", new { BookId = model.SelectedBookId });
        }
    }
}
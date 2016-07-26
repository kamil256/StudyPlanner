using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.WebUI.Models
{
    public class BooksAddBookViewModel
    {
        public IEnumerable<Author> AuthorsList { get; set; }
        public IEnumerable<Publisher> PublishersList { get; set; }
    }
}
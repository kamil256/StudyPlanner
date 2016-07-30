using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyPlanner.Domain.Entities;

namespace StudyPlanner.WebUI.Models
{
    public class BooksBookInfoViewModel
    {
        public Book Book { get; set; }
        public Author[] Authors { get; set; }
        public string SearchString { get; set; }
    }
}
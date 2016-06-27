using StudyPlanner.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyPlanner.Models
{
    public class AddBookModel
    {
        [Required]
        public string Title { get; set; }

        public string Author { get; set; }
        public List<string> Authors { get; set; }
        public bool AddAuthor { get; set; }
        public string RemoveAuthor { get; set; }
        public bool AddCover { get; set; }
        public string CoverName { get; set; }
        [Required]
        public string Publisher { get; set; }

        [Required]
        public DateTime? Released { get; set; }

        [Required]
        public int? Pages { get; set; }

        public string[] AuthorsList { get; set; }
        public string[] PublishersList { get; set; }

        public HttpPostedFileBase cover { get; set; }
        //public static implicit operator Book(AddBookModel model)
        //{

        //}
    }
}
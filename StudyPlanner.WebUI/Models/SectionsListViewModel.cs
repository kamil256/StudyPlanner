using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class SectionsListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Section> Sections { get; set; }

        public int? SelectedBookId { get; set; }
        public IEnumerable<string> SelectedBookAuthors { get; set; }

        [Required]
        public int? NewSectionStartPageNumber { get; set; }

        [Required]
        public int? NewSectionEndPageNumber { get; set; }

        [Required]
        public string NewSectionName { get; set; }
    }
}
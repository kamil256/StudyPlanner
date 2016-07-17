using StudyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyPlanner.WebUI.Models
{
    public class TrainingsModel
    {
        public int FilterBookId { get; set; }
        public int FilterSectionId { get; set; }
        public int? FilterDaysSinceLastActivityFrom { get; set; }
        public int? FilterDaysSinceLastActivityTo { get; set; }
        public bool FilterTrainingCompleted { get; set; } = true;
        public bool Filter1LessonLeft { get; set; } = true;
        public bool Filter2LessonsLeft { get; set; } = true;
        public bool Filter3LessonsLeft { get; set; } = true;

        public List<Book> Books { get; set; }
        public List<Section> Sections { get; set; }
        public List<TrainingsModel.Training> Trainings { get; set; }

        public string SortBy { get; set; }
        public string[] SortByItems { get; } = new[] { "Last activity", "Lessons left", "Authors", "Books" };
        public string SortingOrder { get; set; }
        public string[] SortingOrderItems { get; } = new[] { "Ascending", "Descending" };

        public class Training
        {
            public int TrainingId { get; set; }
            public int SectionId { get; set; }
            public string BookTitle { get; set; }
            public string SectionName { get; set; }
            public List<Author> Authors { get; set; }
            public DateTime Deadline { get; set; }
            public int LessonsLeft { get; set; }
            
            public static explicit operator TrainingsModel.Training(StudyPlanner.Domain.Entities.Training t)
            {
                return new TrainingsModel.Training
                {
                    TrainingId = t.TrainingId,
                    SectionId = t.SectionId,
                    BookTitle = t.Section.Book.Title,
                    SectionName = t.Section.Name,
                    Authors = (from x in t.Section.Book.AuthorOfBooks orderby x.Priority select x.Author).ToList()//,
                    //Deadline = t.CompletionDate.AddDays(14),
                    //LessonsLeft = t.LessonsLeft
                };
            }
        }
    }
}
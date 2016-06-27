using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyPlanner.EF;

namespace StudyPlanner.Models
{
    public class SectionsModel
    {
        public int BookId { get; set; }
        public string[] Authors { get; set; }
        public string Publisher { get; set; }
        public DateTime Released { get; set; }
        public int Pages { get; set; }
        public string Cover { get; set; }

        public List<Book> Books { get; set; }
        public List<Section> Sections { get; set; }


        public int AddStartPageNumber { get; set; }
        public int AddEndPageNumber { get; set; }
        public string AddName { get; set; }

        public class Section
        {
            public int SectionId { get; set; }
            public string Name { get; set; }
            public int StartPageNumber { get; set; }
            public int EndPageNumber { get; set; }
            public int NumberOfPages { get; set; }
            public int TrainingsCompleted { get; set; }
            public bool TrainingInProgress { get; set; }

            

            public static explicit operator Section(EF.Section section)
            {
                return new Section
                {
                    SectionId = section.SectionId,
                    Name = section.Name,
                    StartPageNumber = section.StartPageNumber,
                    EndPageNumber = section.EndPageNumber,
                    NumberOfPages = section.EndPageNumber - section.StartPageNumber + 1
                };
            }
        }
    }
}
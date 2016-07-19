using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlanner.Domain.Entities.MetaData
{
    public partial class UserMetaData
    {
        [Display(Name = "Your name")]
        public string Name { get; set; }

        [Display(Name = "E-mail address")]
        public string Email { get; set; }
    }
}

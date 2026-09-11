using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupReportGenerator.Models
{
    public class RecipeHeader
    {
        public int RecipeId { get; set; }

        public string RecipeName { get; set; }

        public string LineName { get; set; }

        public string Model { get; set; }

        public string BoardSide { get; set; }

        public DateTime ImportedDate { get; set; }
    }
}

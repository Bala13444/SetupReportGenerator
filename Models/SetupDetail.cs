using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupReportGenerator.Models
{
    public class SetupDetail
    {
        public int SetupDetailId { get; set; }

        public int RecipeId { get; set; }

        public string MachineName { get; set; }

        public string Table { get; set; }

        public string Track { get; set; }

        public string PartNumber { get; set; }

        public string ReferenceDesignator { get; set; }

        public string FeederType { get; set; }
    }
}

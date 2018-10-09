using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class DataTableRequestModel
    {
        public string searchPhase { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string orderCol { get; set; }
        public string orderDir { get; set; }
    }
}
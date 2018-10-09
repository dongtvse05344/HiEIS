using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class DataTableResponseModel<T>
        where T : class
    {
        public List<T> data { get; set; }
        public int total { get; set; }
        public int display { get; set; }
    }


}
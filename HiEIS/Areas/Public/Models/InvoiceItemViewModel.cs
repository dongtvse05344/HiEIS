using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiEIS.Areas.Public.Models
{
    public class InvoiceItemViewModel
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATRate { get; set; }
    }
}
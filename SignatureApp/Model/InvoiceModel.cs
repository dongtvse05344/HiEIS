using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureApp.Model
{
    public partial class InvoiceModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEnterprise { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Enterprise { get; set; }
        public string LookupCode { get; set; }
        public string TemplateForm { get; set; }
        public string TemplateSerial { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string FileUrl { get; set; }
        public bool PaymentStatus { get; set; }
        public int Status { get; set; }
        public string CodeGuid { get; set; }
    }
}

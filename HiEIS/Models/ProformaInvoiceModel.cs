using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class GetFileUrlProformaInvoiceModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã tìm kiếm")]
        public string LookupCode { get; set; }
    }
    public class DeleteProformaInvoiceModel
    {
        public int Id { get; set; }  

    }
    public class ProformaInvoiceModel
    {
    }
    public class TableProformaInvoiceModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string FileUrl { get; set; }
        public string PaymentDeadline { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public string StaffId { get; set; }
        public string CustomerId { get; set; }
        public string StaffCompanyName { get; set; }
        public string StaffCompanyTaxNo { get; set; }
        public string StaffCompanyAddress { get; set; }
        public string LookupCode { get; set; }
        public string Total { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HiEIS.Areas.Public.Models
{
    public class ProformaInvoiceViewModel
    {

        [Required]
        public System.DateTime Date { get; set; }
        public string FileUrl { get; set; }
        [Required]
        public System.DateTime PaymentDeadline { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public decimal SubTotal { get; set; }
        [Required]
        public decimal VATAmount { get; set; }
        [Required]
        public decimal Liabilities { get; set; }
        [Required]
        public decimal TotalNoLiabilities { get; set; }
        [Required]
        public decimal Total { get; set; }
        public string CustomerId { get; set; }
        public string StaffId { get; set; }
        public List<ProformaInvoiceItemViewModel> ProformaInvoiceItems { get; set; }
    }

    public class ProformaInvoiceListViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEnterprise { get; set; }
        public System.DateTime Date { get; set; }
        public string FileUrl { get; set; }
        public System.DateTime PaymentDeadline { get; set; }
        public int Status { get; set; }
        public string LookupCode { get; set; }
        public decimal Total { get; set; }
    }

    public class CreateProformaViewModel : ProformaInvoiceViewModel
    {
        public string CustomerEnterprise { get; set; }
        public string LookupCode { get; set; }
        public string Deadline { get; set; }
    }

    public class UpdateProformaViewModel : ProformaInvoiceViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEnterprise { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTel { get; set; }
        public string Deadline { get; set; }
    }

    public class ProformaViewPDF : ProformaInvoiceViewModel
    {
        public string StaffCompanyName { get; set; }
        public string StaffCompanyTel { get; set; }
        public string StaffCompanyBank { get; set; }
        public string StaffCompanyBankAccountNumber { get; set; }
        public string StaffName { get; set; }
        public string LookupCode { get; set; }
        public string CustomerEnterprise { get; set; }
        public string CustomerEmail { get; set; }
    }
}
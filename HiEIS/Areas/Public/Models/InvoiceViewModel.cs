using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HiEIS.Areas.Public.Models
{
    public class InvoiceViewModel
    {
        [Required]
        public System.DateTime Date { get; set; }

        public string DueDateString { get; set; }

        public Nullable<System.DateTime> DueDate { get; set; }

        [Required]
        public int PaymentMethod { get; set; }

        //public string FileUrl { get; set; }

        [Required]
        public decimal SubTotal { get; set; }

        [Required]
        public decimal VATRate { get; set; }

        [Required]
        public decimal VATAmount { get; set; }

        public string Note { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string AmountInWords { get; set; }

        public string Name { get; set; }

        public string Enterprise { get; set; }

        public string Address { get; set; }

        public string TaxNo { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string BankAccountNumber { get; set; }

        public string Bank { get; set; }

        [Required]
        public int TemplateId { get; set; }

        public string TemplateForm { get; set; }

        public string TemplateSerial { get; set; }

        public string CustomerId { get; set; }

        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
    }
    
    public class InvoiceListViewModel
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
    
    public class CreateInvoiceViewModel : InvoiceViewModel
    {
        public string LookupCode { get; set; }
        public int Type { get; set; }
        public string StaffId { get; set; }
        public bool PaymentStatus { get; set; }
        public int Status { get; set; }
    }
    
    public class UpdateInvoiceViewModel : InvoiceViewModel
    {
        public int Id { get; set; }
    }

    public class PdfViewModel : InvoiceViewModel
    {
        public string Number { get; set; }
        public string LookupCode { get; set; }
        public string StaffCompanyName { get; set; }
        public string StaffCompanyTaxNo { get; set; }
    }
    
}
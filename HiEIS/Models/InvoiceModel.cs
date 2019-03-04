using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public string LookupCode { get; set; }
        public string Number { get; set; }
        public int Type { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public int PaymentMethod { get; set; }
        public string FileUrl { get; set; }
        public string FileUrl2 { get; set; }
        public string  FilUrl3 { get; set; }
        public decimal SubTotal { get; set; }
        public decimal VATRate { get; set; }
        public decimal VATAmount { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
        public string AmountInWords { get; set; }
        public bool PaymentStatus { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string Address { get; set; }
        public string TaxNo { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string BankAccountNumber { get; set; }
        public string Bank { get; set; }
        public int TemplateId { get; set; }
        public string StaffId { get; set; }
        public string CustomerId { get; set; }
    }

    public class InvoiceSignVM
    {
        public int Id { get; set; }

        public String FileUrl { get; set; }
    }
    public class TableInvoiceModel
    {
        public string StaffCompanyName { get; set; }
        public string StaffCompanyTaxNo { get; set; }
        public string StaffCompanyAddress { get; set; }
        public string LookupCode { get; set; }
        public string TemplateForm { get; set; }
        public string TemplateSerial { get; set; }
        public string Number { get; set; }
        public string FileUrl { get; set; }
        public string Total { get; set; }
        public bool PaymentStatus { get; set; }
        public int Status { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    }
    public class LookupModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã tìm kiếm")]
        public string InvoiceLookupCode { get; set; }
    }
}
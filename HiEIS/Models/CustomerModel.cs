using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HiEIS.Models
{
    public class CreateCustomerModel : CustomerModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class CustomerModel
    {
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public Nullable<decimal> TotalLiabilities { get; set; }
    }
    public class TableCustomerModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
        public string AspNetUserEmail { get; set; }
        public Nullable<decimal> TotalLiabilities { get; set; }
    }

    public class CustomerProductModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public string ProductUnitPrice
        {
            get { return string.Format("{0:#,##0}", this.DUnitPrice); }
            set
            {
                decimal dValue;
                decimal.TryParse(value, out dValue);
                this.DUnitPrice = dValue;
            }
        }

        public decimal? DUnitPrice { get; set; }
        public decimal? ProductVATRate { get; set; }
       
        public decimal ProductAmount { get; set; }

        public bool? HasIndex { get; set; }
    }

    public class LiabilitiesModel
    {

        public string Liabilities
        {
            get; set;
        }
    }
}
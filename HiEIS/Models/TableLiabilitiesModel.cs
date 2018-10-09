using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class TableCompanyLiabilitiesModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public decimal? Current { get; set; }
        public decimal Remain
        {
            get { return (this.Current ?? 0) - (this.Payment ?? 0); }
        }
        public decimal? Payment { get; set; }
        public string Address { get; set; }
    }
    public class TableCustomerLiabilitiesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public decimal? Current { get; set; }
        public decimal Remain
        {
            get { return (this.Current ?? 0) - (this.Payment ?? 0); }
        }
        public decimal? Payment { get; set; }
        public string Address { get; set; }
    }
    public class DetailCustomerTransactionModel
    {
        public decimal? Current { get; set; }
        public decimal? Last { get; set; }
        public decimal? TotalCurrent
        {
            get { return (Current + LastRemain); }
        }
        public decimal? Remain
        {
            get { return ((this.TotalCurrent ?? 0) - (this.Payment ?? 0)); }
        }
        public decimal? LastRemain
        {
            get { return ((this.Last ?? 0) - (this.LastPayment ?? 0)); }
        }
        public decimal? Payment { get; set; }
        public decimal? LastPayment { get; set; }
    }
    public class DetailCustomerLiabilitiesModel
    {
        public string Name { get; set; }
        public string Enterprise { get; set; }
        public string TaxNo { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
    }
    public class TableLiabilitiesDetailModel
    {
        public string Date { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}
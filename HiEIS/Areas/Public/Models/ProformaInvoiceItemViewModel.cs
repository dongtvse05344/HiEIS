using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HiEIS.Areas.Public.Models
{
    public class ProformaInvoiceItemViewModel
    {
        public int ProformaInvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public decimal VATRate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int OldNumber { get; set; }
        public int NewNumber { get; set; }
        public string DateFromS { get; set; }
        public string DateToS { get; set; }
        public DateTime? DateFrom
        {
            get
            {
                DateTime date;
                var rs = DateTime.TryParseExact(this.DateFromS, "dd/MM/yyyy", null, DateTimeStyles.None, out date);
                if (rs)
                {
                    return date;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    this.DateFromS = value.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    this.DateFromS = "";
                }
            }
        }

        public DateTime? DateTo
        {
            get
            {
                DateTime date;
                var rs = DateTime.TryParseExact(this.DateToS, "dd/MM/yyyy", null, DateTimeStyles.None, out date);
                if (rs)
                {
                    return date;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    this.DateToS = value.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    this.DateToS = "";
                }
            }
        }

        public bool? HasIndex { get; set; }
    }
}
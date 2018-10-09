//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HiEIS.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProformaInvoiceItem
    {
        public int ProformaInvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal VATRate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<int> OldNumber { get; set; }
        public Nullable<int> NewNumber { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ProformaInvoice ProformaInvoice { get; set; }
    }
}

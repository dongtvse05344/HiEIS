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
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime Date { get; set; }
        public string CustomerId { get; set; }
        public int CompanyId { get; set; }
        public string Note { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

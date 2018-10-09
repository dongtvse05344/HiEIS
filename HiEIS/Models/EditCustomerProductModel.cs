using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class EditCustomerProductModel
    {
        public string CustomerId { get; set; }
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Range(1, Double.PositiveInfinity)]
        public int Amount { get; set; }
    }
}
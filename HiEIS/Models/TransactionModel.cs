using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HiEIS.Utils;

namespace HiEIS.Models
{
    public class CreateTransactionModel
    {
        [Required(ErrorMessage = "Chỉ được nhập 1(nợ) hoặc 2(trả)")]
        [Range((int)HiEISUtil.TransactionType.Liability, (int)HiEISUtil.TransactionType.Payment)]
        public int Type { get; set; }

        [Required(ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Range(1, Double.PositiveInfinity)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Chưa nhập mã khách hàng")]
        public string CustomerId { get; set; }
        public string Note { get; set; }
    }
    public class TransactionModel
    {
    }
    public class TableTransactionModel
    {
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }
}
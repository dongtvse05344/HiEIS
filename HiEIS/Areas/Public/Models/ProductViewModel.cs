using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HiEIS.Utils;

namespace HiEIS.Areas.Public.Models
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string Name { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = false, NullDisplayText = "")]
        public string Code { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, NullDisplayText = "")]
        public string Unit { get; set; }


        [Range(0, Double.PositiveInfinity, ErrorMessage = "Vui lòng nhập đơn giá > 0")]
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Regex("^(\\d,?)+(\\.\\d+)?$", ErrorMessage = "Vui lòng nhập đơn giá > 0")]
        public string UnitPrice
        {
            get { return string.Format("{0}", this.DUnitPrice); }
            set
            {
                decimal dValue;
                decimal.TryParse(value, out dValue);
                this.DUnitPrice = dValue;
            }
        }

        public string SUnitPrice
        {
            get { return string.Format("{0:#,##}", this.DUnitPrice); }
        }
        public decimal? DUnitPrice { get; set; }

        public decimal VATRate { get; set; }

        public bool HasIndex { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập thuế suất")]
        //[Regex("^\\d+(\\.\\d+)?$", ErrorMessage = "Vui lòng nhập chữ số")]
        //[RangeString(0.0, 40.0, typeof(decimal), ErrorMessage = "Vui lòng nhập số từ 0 đến 40")]
        //public string VATRate
        //{
        //    get { return string.Format("{0:0.00}", this.DVATRate); }
        //    set
        //    {
        //        decimal dValue;
        //        decimal.TryParse(value, out dValue);
        //        this.DVATRate = dValue;
        //    }
        //}

        //public decimal? DVATRate { get; set; }


    }

    public class CreateProductModel : ProductViewModel
    {
        public int CompanyId { get; set; }

        public bool IsActive { get; set; }

    }

    public class UpdateProductModel : ProductViewModel
    {
        public int Id { get; set; }
    }

    public class DeleteProductModel
    {
        public bool IsActive { get; set; }
    }
}

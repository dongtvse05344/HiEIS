using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HiEIS.Utils;

namespace HiEIS.Models
{
    public class CreateCompanyModel: CompanyModel
    {
        public bool IsActive { get; set; }
    }

    public class UpdateCompanyModel : CompanyModel
    {
        public int Id { get; set; }
    }

    public class CompanyModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã số thuế")]
        [RegularExpression("^.{10,13}$", ErrorMessage = "Mã số thuế phải từ 10-13 chữ số")]
        [UniqueTaxNo(ErrorMessage = "Mã số thuế đã tồn tại")]
        public string TaxNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string Email { get; set; }

        public string Website { get; set; }
        public string Fax { get; set; }
        public string Bank { get; set; }
        public string BankAccountNumber { get; set; }
    }

    public class TableCompanyModel : CompanyModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }
}
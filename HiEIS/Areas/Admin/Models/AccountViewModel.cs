using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiEIS.Annotations;
using System.ComponentModel.DataAnnotations;
using HiEIS.Utils;

namespace HiEIS.Areas.Admin.Models
{
    public class AccountViewModel
    {
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [UniqueEmail(ErrorMessage = "Email đã tồn tại. Vui lòng nhập email khác")]
        public string Email { get; set; }

        [Username(ErrorMessage = "Tài khoản đã tồn tại. Vui lòng nhập tài khoản khác")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class ListAccountViewModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }

    public class AccountCustomerViewModel : AccountViewModel
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEnterprise { get; set; }
        public string CustomerTaxNo { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerFax { get; set; }
        public string CustomerBank { get; set; }
        public string CustomerBankAccountNumber { get; set; }
    }

    public class CreateCustomerAccount : AccountViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải ít nhất {2} ký tự.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }

        public string CustomerEnterprise { get; set; }

        public string CustomerTaxNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ khách hàng")]
        public string CustomerAddress { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Phone Number")]
        public string CustomerTel { get; set; }

        public string CustomerFax { get; set; }

        public string CustomerBank { get; set; }

        [Regex("^[0-9]+$", ErrorMessage = "Vui lòng nhập chữ số")]
        public string CustomerBankAccountNumber { get; set; }
    }

    public class UpdateCustomerAccount
    {
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        public string CustomerName { get; set; }

        public string CustomerEnterprise { get; set; }

        public string CustomerTaxNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ khách hàng")]
        public string CustomerAddress { get; set; }

        [Phone]
        public string CustomerTel { get; set; }
        
        public string CustomerFax { get; set; }

        public string CustomerBank { get; set; }

        public string CustomerBankAccountNumber { get; set; }

        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        public string UserName { get; set; }
    }

    public class CreateAdminAccount : AccountViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải ít nhất {2} ký tự.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class AdminViewModel : AccountViewModel
    {
        public string Id { get; set; }
    }

    public class UpdateAdminAccount
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
    }
}

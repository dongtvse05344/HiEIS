using HiEIS.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiEIS.Models
{
    public class CreateStaffModel : StaffModel
    {
        [Username(ErrorMessage ="Tài khoản đã tồn tại. Vui lòng nhập tài khoản khác")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải ít nhất {2} ký tự.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không trùng khớp.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress]
        [UniqueEmail(ErrorMessage = "Email đã tồn tại. Vui lòng nhập email khác")]
        public string Email { get; set; }

    }
    public class UpdateStaffModel: StaffModel
    {
        public string AspNetUserUserName { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress]
        public string AspNetUserEmail { get; set; }
    }

    public class StaffModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên")]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vị trí")]
        public List<string> Roles { get; set; }
    }

    public class TableStaffModel : StaffModel
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class StaffAccountModel : StaffModel
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTaxNo { get; set; }
        public bool AspNetUserIsActive { get; set; }
    }

    public class CreateStaffAccountModel : CreateStaffModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp hợp lệ")]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "Please enter a number between 0 and 250.")]
        public int CompanyId { get; set; }
    }

    public class StaffAccModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhân viên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress]
        public string AspNetUserEmail { get; set; }
        
        public string Address { get; set; }
        public string Tel { get; set; }
    }
}
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HiEIS.Areas.Public.Models
{
    public class TemplateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mẫu số hóa đơn")]
        public string Form { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập kí hiệu hóa đơn")]
        public string Serial { get; set; }

        public string FileUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng hóa đơn")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số bắt đầu")]
        public int BeginNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số hiện tại")]
        public int CurrentNo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu sử dụng hóa đơn")]
        [DataType(DataType.DateTime)]
        public System.DateTime ReleaseDate { get; set; }

        public string ReleaseAnnouncementUrl { get; set; }

        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
    }

    public class CreateTemplateModel : TemplateViewModel
    {
        public int CompanyId { get; set; }

        public string Date { get; set; }

        [Required(ErrorMessage = "Vui lòng đính kèm mẫu hóa đơn")]
        public HttpPostedFileBase InvoiceTemplateFile { get; set; }

        [Required(ErrorMessage = "Vui lòng đính kèm thông báo phát hành hóa đơn")]
        public HttpPostedFileBase ReleaseAnnounmentFile { get; set; }
    }

    public class UpdateTemplateModel : TemplateViewModel
    {
        public string Date { get; set; }

        [Required(ErrorMessage = "Vui lòng đính kèm thông báo phát hành hóa đơn")]
        public HttpPostedFileBase ReleaseAnnounmentFile { get; set; }
    }
}
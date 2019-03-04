using HiEIS.Businesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Areas.Public.Models;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using HiEIS.Utils;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleStaffs)]
    public class TemplatesController : HiEISCompanyController
    {
        //GET: Public/Templates
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Index()
        {
            var company = this.Company;
            var templateBusiness = new TemplateBusiness();
            var templates = templateBusiness.GetTemplateIsActive(this.Company.Id);
            ViewBag.Templates = templates;
            return View();
        }

        [HttpGet]
        public JsonResult GetAllTemplates()
        {
            var company = this.Company;
            var templateBusiness = new TemplateBusiness();
            var templates = templateBusiness.GetTemplateIsActive(this.Company.Id);
            return Json(templates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get pdf template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet]
        public FileContentResult GetTemplatePdf(int templateId)
        {
            try
            {
                var templateBusiness = new TemplateBusiness();
                var templates = templateBusiness.GetTemplateById(templateId);

                //virtual path
                string pdfFileUrl = templates.FileUrl;
                //real path
                string physicalPath = Path.Combine(Server.MapPath(@"\"), pdfFileUrl.Substring(1));
                //get stream
                byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);

                return File(pdfBytes, "application/pdf");
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        //GET: Public/Templates/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Public/Templates/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        [HttpPost]
        public ActionResult Create(CreateTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var company = this.Company;
                var templateBusiness = new TemplateBusiness();
                var template = templateBusiness.GetTemplateByFormSerialActive(this.Company.Id, model.Form, model.Serial);
                if (template != null)
                {
                    if (model.Form == template.Form && model.Serial == template.Serial)
                    {
                        TempData["message"] = new object[]
                        { "fail", "Mẫu "+ model.Form + " , ký hiệu " + model.Serial + " đã có, bạn có thể thêm số lượng", "Thất bại" };
                        return Redirect("Index");
                    }
                }
                else
                {
                    
                    model.FileUrl = GetPathFile(model.InvoiceTemplateFile, "TemplateInvoice");
                    model.FileUrl2 = GetPathFile(model.InvoiceTemplateFile2, "TemplateInvoice");
                    model.FileUrl3 = GetPathFile(model.InvoiceTemplateFile3, "TemplateInvoice");

                    model.ReleaseAnnouncementUrl = GetPathFile(model.ReleaseAnnounmentFile, "ReleaseAnnouncement");
                    model.IsActive = true;

                    bool success = templateBusiness.AddNewTemplate(model, this.Company.Id);
                    if (success)
                    {
                        TempData["message"] = new object[] { "success", "Đã tạo mẫu hóa đơn.", "Thành công" };
                    }
                    else
                    {
                        TempData["message"] = new object[] { "fail", "Có lỗi xảy ra, vui lòng thử lại sau.", "Thất bại" };
                    }

                    return Redirect("Index");
                }
            }
            return View(model);
        }
        
        //GET: Public/Templates/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Update()
        {
            return View();
        }

        [Authorize(Roles = HiEISUtil.RoleManager)]
        [HttpPost]
        public ActionResult AddMoreBlockAjax(UpdateTemplateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var templateBusiness = new TemplateBusiness();
                
                model.ReleaseAnnouncementUrl = GetPathFile(model.ReleaseAnnounmentFile, "ReleaseAnnouncement");

                bool success = templateBusiness.AddMoreBlock(model, this.Company.Id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã thêm số lượng cho mẫu."
                    });
                }
                else
                {
                    
                    return Json(new
                    {
                        success = false,
                        message = "Có lỗi xảy ra, vui lòng thử lại sau."
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    data = new HiEIS.Models.ValidationResultModel(ModelState),
                    message = "Dữ liệu không hợp lệ."
                });
            }
        }

        [HttpGet]
        public JsonResult TemplateDetail(HiEIS.Models.DataTableRequestModel req, int id)
        {
            var templateBusiness = new TemplateBusiness();
            var templates = templateBusiness.GetTemplatesByFormSerial(id, req);
           // ViewBag.TemplatesDetail = templates;
            return Json(templates, JsonRequestBehavior.AllowGet);
        }

        public string GetPathFile(HttpPostedFileBase file, string location)
        {
            var s = Guid.NewGuid();
            var ext = Path.GetExtension(file.FileName);
            var name = $"{s}{ext}";
            var localPath = "/Files/" + location + "/" + name;
            string path = Server.MapPath("~" + localPath);
            file.SaveAs(path);
            return localPath;
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var company = this.Company;
            var business = new TemplateBusiness();
            bool success = business.DeleteTemplate(id, company.Id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa mẫu hóa đơn.",
                    title = "Thành công"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    title = "Không thành công"
                });
            }
        }
    }
}
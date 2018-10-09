using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Businesses;
using HiEIS.Areas.Public.Models;
using HiEIS.Utils;
using Microsoft.AspNet.Identity;
using System.IO;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleNotAdmin)]
    public class ProformaInvoiceController : Utils.HiEISCompanyController
    {
        //int companyId = 1;

        // GET: Public/ProformaInvoice
        public ActionResult ListByCustomerId(DataTableRequestModel model
                                            , string compName
                                            , string lookupCode)
        {
            string customerId = User.Identity.GetUserId();
            model.searchPhase = customerId;
            var business = new Businesses.ProformaInvoiceBusiness();
            var response = business.ListByCustomerId(model, compName, lookupCode);
            return Json(response);
        }

        //Delete Proforma
        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        public ActionResult DeleteProformaInvoice(int id)
        {
            var company = this.Company;
            var business = new ProformaInvoiceBusiness();
            bool success = business.DeleteProformaInvoice(id, company.Id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa thông báo phí.",
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

        //GET: Public/Proforma
        //[Authorize(Roles = HiEISUtil.RoleLiabilityAccountant + "," + HiEISUtil.RoleAccountingManager + "," + HiEISUtil.RoleCustomer)]
        public ActionResult Index()
        {
            if (User.IsInRole(HiEISUtil.RoleCustomer))  
            {
                return View("CustomerView");
            }
            return View("CompanyView");
        }
        
        //[Authorize(Roles = HiEISUtil.RoleAccountants)]
        //[HttpGet]
        //public JsonResult GetProformaInvoices()
        //{
        //    var comp = this.Company;
        //    var business = new ProformaInvoiceBusiness();
        //    var invoices = business.GetAllProformaInvoices(comp.Id);
        //    return Json(invoices, JsonRequestBehavior.AllowGet);
        //}

        [Authorize(Roles = HiEISUtil.RoleAccountants)]
        public JsonResult GetProformaInvoices(DataTableRequestModel model, string customerName, string lookupCode, string minDate, string maxDate, string statusString)
        {
            var comp = this.Company;
            var business = new ProformaInvoiceBusiness();
            int? status = null;
            var min = minDate.ParseDate("dd/MM/yyyy");
            var max = maxDate.ParseDate("dd/MM/yyyy");
            
            if (!string.IsNullOrWhiteSpace(statusString))
            {
                status = int.Parse(statusString);
            }

            var proformaInvoices = business.GetAllProformaInvoices(model, comp.Id, customerName, lookupCode, min, max, status);
            return Json(proformaInvoices);
        }

        //Get: Public/Proforma/Create
        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        public ActionResult Create()
        {
            return View();
        }

        //Post: Public/Proforma/Create
        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        [HttpPost]
        public JsonResult Create(CreateProformaViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //get current staff id
                string staffId = User.Identity.GetUserId();

                var business = new ProformaInvoiceBusiness();
                bool success = business.CreateProformaInvoice(model, staffId, this.ControllerContext);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã tạo thông báo phí.",
                        url = "/Public/ProformaInvoice"
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

        //Get: Public/Proforma/Edit
        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        public ActionResult Edit(int id, DataTableRequestModel model)
        {
            var company = this.Company;
            var proformaBusiness = new ProformaInvoiceBusiness();
            var proforma = proformaBusiness.GetProformaById(id, company.Id);
            if (proforma == null)
            {
                return RedirectToAction("Index");
            }
            return View(proforma);
        }

        //Post: Public/Proforma/Edit
        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        [HttpPost]
        public JsonResult Edit(int id, UpdateProformaViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var company = this.Company;
                string staffId = User.Identity.GetUserId();
                var proformaBusiness = new ProformaInvoiceBusiness();
                model.Id = id;
                model.StaffId = staffId;
                bool success = proformaBusiness.UpdateProforma(model, id, company.Id, this.ControllerContext);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật thông báo phí.",
                        url = "/Public/ProformaInvoice"
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

        //Post: Public/Proforma/ConfirmProforma
        [Authorize(Roles = HiEISUtil.RoleAccountingManager)]
        [HttpPost]
        public JsonResult ConfirmProforma(int id)
        {
            object resultObj = null;
            string staffId = User.Identity.GetUserId();
            var company = this.Company;
            var business = new ProformaInvoiceBusiness();
            var emailBusiness = new EmailBusiness();
            var confirmProforma = business.ConfirmProforma(id, company.Id, staffId, this.ControllerContext);
            if (confirmProforma != null)
            {
                resultObj = new
                {
                    success = true,
                    message = "Đã duyệt thông báo phí.",
                    title = "Thành công"
                };

                if (confirmProforma.CustomerEmail != null)
                {
                    var viewData = new ViewDataDictionary();
                    viewData.Model = confirmProforma;
                    string subject = "[HiEIS] Phát hành thông báo phí";
                    string content = emailBusiness.ToHtml("ProformaEmailTemplate", viewData, this.ControllerContext);
                    emailBusiness.SendEmail(confirmProforma.CustomerEmail, subject, content);
                }
            }
            else
            {
                resultObj = new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại sau.",
                    title = "Không thành công"
                };
            }
            return Json(resultObj);
        }

        [AllowAnonymous]
        public ActionResult GetFileUrl(GetFileUrlProformaInvoiceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new ProformaInvoiceBusiness();
                    string result = business.GetFileUrl(model);
                    return Json(new
                    {
                        success = true,
                        fileUrl = result
                    });
                }
                else
                {
                    var valid = new ValidationResultModel(ModelState);
                    return Json(new
                    {
                        success = false,
                        data = valid,
                        message = "Dữ liệu không phù hợp"
                    });
                }
            }
            catch (Exceptions.InvoiceNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Mã tìm kiếm không tồn tại"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }

        }

        public ActionResult ConvertToInvoice(int id)
        {
            var business = new ProformaInvoiceBusiness();
            var invoice = business.ConvertToInvoice(id, this.Company.Id);
            if (invoice != null)
            {
                TempData["Invoice"] = invoice;
                return RedirectToAction("CreateFromProforma", "Invoices");
            }
            else
            {
                TempData["message"] = new object[] { "error", "Có lỗi xảy ra, vui lòng thử lại sau!", "Không thành công" };
                return View("CompanyView");
            }
            
        }

        public JsonResult GetProformaEmail(int id)
        {
            var busiess = new ProformaInvoiceBusiness();
            string result = busiess.GetProformaEmail(id);
            return Json(new
            {
                email = result
            });
        }

        public JsonResult SendEmail(int id, string email)
        {
            var business = new ProformaInvoiceBusiness();
            var emailBusiness = new EmailBusiness();
            var viewData = new ViewDataDictionary();
            var proforma = business.GetPDFModelById(id);
            if (proforma != null)
            {
                viewData.Model = proforma;
                string subject = "[HiEIS] Phát hành thông báo phí";
                string content = emailBusiness.ToHtml("ProformaEmailTemplate", viewData, this.ControllerContext);
                emailBusiness.SendEmail(email, subject, content);
                return Json(new
                {
                    success = true,
                    message = "Đã gửi thông báo phí.",
                    title = "Thành công"
                });
            }
            return Json(new
            {
                success = false,
                message = "Không tìm thấy thông báo phí!",
                title = "Không thành công"
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HiEIS.Businesses;
using Newtonsoft.Json;
using HiEIS.Models;
using System.Text;
using HiEIS.Areas.Public.Models;
using HiEIS.Utils;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleNotAdmin)]
    public class InvoicesController : HiEISCompanyController
    {
        //private int companyId = 1;

        //GET: Public/Invoices
        [Authorize(Roles = HiEISUtil.RoleAccountants + "," + HiEISUtil.RoleCustomer)]
        public ActionResult Index()
        {
            if (User.IsInRole(HiEISUtil.RoleCustomer))
            {
                return View("CustomerView");
            }
            return View("CompanyView");
        }

        //[Authorize(Roles = HiEISUtil.RoleStaffs)]
        //[HttpGet]
        //public JsonResult GetInvoices()
        //{
        //    var company = this.Company;
        //    var business = new InvoiceBusiness();
        //    var invoices = business.GetAllInvoices(company.Id);
        //    return Json(invoices, JsonRequestBehavior.AllowGet);
        //}

        //GET: Public/Products
        [Authorize(Roles = HiEISUtil.RoleAccountants)]
        public JsonResult GetInvoices(DataTableRequestModel model, string customerName, string lookupCode, string minString, string maxString, string paymentString, string statusString)
        {
            var company = this.Company;
            var business = new InvoiceBusiness();
            bool? paymentStatus = null;
            int? status = null;
            var minDate = minString.ParseDate("dd/MM/yyyy");
            var maxDate = maxString.ParseDate("dd/MM/yyyy");

            if (paymentString == "0")
            {
                paymentStatus = false;
            }
            else if (paymentString == "1")
            {
                paymentStatus = true;
            }
            if (!string.IsNullOrWhiteSpace(statusString))
            {
                status = int.Parse(statusString);
            }

            var invoices = business.GetAllInvoices(model, company.Id, customerName, lookupCode, minDate, maxDate, paymentStatus, status);
            return Json(invoices);
        }

        [HttpGet]
        public FileContentResult GetInvoicePdf(int invoiceId)
        {
            try
            {
                var invoiceBusiness = new InvoiceBusiness();
                var invoices = invoiceBusiness.GetInvoiceById(invoiceId);

                //virtual path
                string pdfFileUrl = invoices.FileUrl;
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

        //GET: Public/Invoices/Create
        [Authorize(Roles = HiEISUtil.RoleNormalAccountants)]
        public ActionResult Create()
        {
            var productBusiness = new ProductBusiness();
            var templateBusiness = new TemplateBusiness();
            var company = this.Company;

            var products = productBusiness.GetAllProducts(company.Id);
            var templates = templateBusiness.GetTemplateActiveByCompanyId(company.Id);
            var paymentMethods = HiEISUtil.GetPaymentMethods();
            var rates = HiEISUtil.GetVATRates();

            ViewBag.Products = products;
            ViewBag.Templates = templates;
            ViewBag.PaymentMethods = paymentMethods;
            ViewBag.VATRates = rates;

            return View();
        }

        //POST: Public/Invoices/Create
        [Authorize(Roles = HiEISUtil.RoleNormalAccountants)]
        [HttpPost]
        public JsonResult Create(CreateInvoiceViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //get current staff id
                string staffId = User.Identity.GetUserId();
                model.DueDate = model.DueDateString.ParseDate("dd/MM/yyyy");

                var business = new InvoiceBusiness();
                bool success = business.CreateInvoice(model, staffId);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã tạo hóa đơn.",
                        url = "/Public/Invoices"
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

        //GET: Public/Invoices/Edit/{id}
        [Authorize(Roles = HiEISUtil.RoleNormalAccountants)]
        public ActionResult Edit(int id)
        {
            var productBusiness = new ProductBusiness();
            var templateBusiness = new TemplateBusiness();
            var invoiceBusiness = new InvoiceBusiness();
            var company = this.Company;

            var invoice = invoiceBusiness.GetNewInvoiceById(id, company.Id);
            if (invoice == null)
            {
                return RedirectToAction("Index");
            }

            var products = productBusiness.GetAllProducts(company.Id);
            var templates = templateBusiness.GetTemplateActiveByCompanyId(company.Id);
            var paymentMethods = HiEISUtil.GetPaymentMethods();
            var rates = HiEISUtil.GetVATRates();

            ViewBag.Products = products;
            ViewBag.Templates = templates;
            ViewBag.PaymentMethods = paymentMethods;
            ViewBag.VATRates = rates;

            return View(invoice);
        }

        //POST: Public/Invoices/Edit
        [Authorize(Roles = HiEISUtil.RoleNormalAccountants)]
        [HttpPost]
        public JsonResult Edit(int id, InvoiceViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                model.DueDate = model.DueDateString.ParseDate("dd/MM/yyyy");

                var company = this.Company;
                var business = new InvoiceBusiness();
                bool success = business.UpdateInvoice(model, id, company.Id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật hóa đơn.",
                        url = "/Public/Invoices"
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
                var valid = new ValidationResultModel(ModelState);
                return Json(new
                {
                    success = false,
                    data = valid,
                    message = "Dữ liệu không hợp lệ."
                });
            }
        }

        //POST: Public/Invoices/Delete/{id}
        [Authorize(Roles = HiEISUtil.RoleNormalAccountants)]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var company = this.Company;
            var business = new InvoiceBusiness();
            bool success = business.DeleteInvoice(id, company.Id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa hóa đơn.",
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

        //POST: Public/Invoices/ConfirmPayment/{id}
        [Authorize(Roles = HiEISUtil.RolePayableAccountant)]
        [HttpPost]
        public JsonResult ConfirmPayment(int id)
        {
            var company = this.Company;
            var business = new InvoiceBusiness();
            bool success = business.ConfirmPayment(id, company.Id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xác nhận thanh toán.",
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

        //POST: Public/Invoices/Sign/{id}
        [Authorize(Roles = HiEISUtil.RoleAccountingManager)]
        [HttpPost]
        public JsonResult Sign(int id)
        {
            object resultObj = null;
            var company = this.Company;
            var business = new InvoiceBusiness();
            var emailBusiness = new EmailBusiness();
            var signedInvoice = business.SignInvoice(id, company.Id);

            if (signedInvoice != null)
            {
                resultObj = new
                {
                    success = true,
                    message = "Đã phát hành hóa đơn.",
                    title = "Thành công"
                };

                if (signedInvoice.Email != null)
                {
                    var viewData = new ViewDataDictionary();
                    viewData.Model = signedInvoice;

                    string subject = "[HiEIS] Thông báo phát hành hóa đơn điện tử";
                    string content = emailBusiness.ToHtml("InvoiceEmailTemplate", viewData, this.ControllerContext);
                    emailBusiness.SendEmail(signedInvoice.Email, subject, content);
                }
            }
            else
            {
                resultObj = new
                {
                    success = false,
                    message = "Không tìm thấy hóa đơn, vui lòng thử lại sau.",
                    title = "Không thành công"
                };
            }

            return Json(resultObj);
        }

        public async Task<ActionResult> GetEnterpriseInfoByTaxNo(string taxNo)
        {
            using (HttpClient client = new HttpClient())
            {
                StringBuilder apiUrl = new StringBuilder("/api/company/");
                apiUrl.Append(taxNo);

                client.BaseAddress = new Uri("https://thongtindoanhnghiep.co");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var info = JsonConvert.DeserializeObject<EnterpriseTaxModel>(data);
                    return Json(info, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { });
        }

        [Authorize(Roles = HiEISUtil.RoleCustomer)]
        public ActionResult ListByCustomerId(DataTableRequestModel model
                                            , string compName
                                            , string lookupCode
                                            , string paymentStatus
                                            , string minDate
                                            , string maxDate)
        {
            string customerId = User.Identity.GetUserId();

            model.searchPhase = customerId;
            var business = new Businesses.InvoiceBusiness();
            var min = minDate.ParseDate("dd/MM/yyyy");
            var max = maxDate.ParseDate("dd/MM/yyyy");
            var response = business.ListByCustomerId(model, compName, lookupCode, paymentStatus, min, max);
            return Json(response);
        }

        [AllowAnonymous]
        public ActionResult FindFileUrl(LookupModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new InvoiceBusiness();

                    //virtual path
                    string pdfFileUrl = business.GetPdfFileUrl(model);
                    //real path
                    string physicalPath = Path.Combine(Server.MapPath(@"\"), pdfFileUrl.Substring(1));
                    //get stream
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);

                    return File(pdfBytes, "application/pdf");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new
                    {
                        message = "Dữ liệu không phù hợp"
                    });
                }
            }
            catch (Exceptions.InvoiceNotFoundException)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    message = "Mã tìm kiếm không tồn tại"
                });
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new
                {
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }

        }

        [HttpPost]
        public JsonResult NumberAllInvoices()
        {
            object resultObj = null;
            var company = this.Company;
            var business = new InvoiceBusiness();
            bool success = business.NumberOutOfDateInvoices();

            if (success)
            {
                resultObj = new
                {
                    success = true,
                    message = "Đã phát hành hóa đơn.",
                    title = "Thành công"
                };
            }
            else
            {
                resultObj = new
                {
                    success = false,
                    message = "Không tìm thấy hóa đơn, vui lòng thử lại sau.",
                    title = "Không thành công"
                };
            }

            return Json(resultObj);
        }

        public JsonResult GetInvoiceEmail(int id)
        {
            var business = new InvoiceBusiness();
            string result = business.GetInvoiceEmail(id);
            return Json(new
            {
                email = result
            });
        }

        public JsonResult SendEmail(int id, string email)
        {
            var business = new InvoiceBusiness();
            var emailBusiness = new EmailBusiness();
            var viewData = new ViewDataDictionary();
            var invoice = business.GetPdfModelById(id);

            if (invoice != null)
            {
                viewData.Model = invoice;
                string subject = "[HiEIS] Thông báo hóa đơn điện tử";
                string content = emailBusiness.ToHtml("InvoiceEmailTemplate", viewData, this.ControllerContext);
                emailBusiness.SendEmail(email, subject, content);

                return Json(new
                {
                    success = true,
                    message = "Đã gửi hóa đơn.",
                    title = "Thành công"
                });
            }

            return Json(new
            {
                success = false,
                message = "Không tìm thấy hóa đơn!",
                title = "Không thành công"
            });
        }

        public ActionResult CreateFromProforma()
        {
            var productBusiness = new ProductBusiness();
            var templateBusiness = new TemplateBusiness();

            var products = productBusiness.GetAllProducts(this.Company.Id);
            var templates = templateBusiness.GetTemplateActiveByCompanyId(this.Company.Id);
            var paymentMethods = HiEISUtil.GetPaymentMethods();
            var rates = HiEISUtil.GetVATRates();
            var model = TempData["Invoice"] as UpdateInvoiceViewModel;

            ViewBag.Products = products;
            ViewBag.Templates = templates;
            ViewBag.PaymentMethods = paymentMethods;
            ViewBag.VATRates = rates;

            return View(model);
        }

        public JsonResult Test()
        {
            var business = new InvoiceBusiness();
            bool result = business.NumberOutOfDateInvoices();
            if (result)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã đánh số hàng loạt.",
                    title = "Thành công"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra!!!!!!",
                    title = "Thành công"
                });
            }
        }

        [AllowAnonymous]
        public JsonResult ReceiveInvoice(string InvoiceGuid, string CompGuid)
        {
            var files = Request.Files;
            var name = files[0].FileName;
            var localPath = "/Files/Signed/" + name;
            string path = Server.MapPath("~" + localPath);
            files[0].SaveAs(path);

            var business = new InvoiceBusiness();
            var emailBusiness = new EmailBusiness();
            var signedInvoice = business.Sign(InvoiceGuid, localPath, CompGuid);
            if (signedInvoice != null)
            {
                if (signedInvoice.Email != null)
                {
                    var viewData = new ViewDataDictionary();
                    viewData.Model = signedInvoice;

                    string subject = "[HiEIS] Thông báo phát hành hóa đơn điện tử";
                    string content = emailBusiness.ToHtml("~/Areas/Public/Views/Invoices/InvoiceEmailTemplate.cshtml", viewData, this.ControllerContext);

                    emailBusiness.SendEmail(signedInvoice.Email, subject, content);
                  
                }
                return Json(new
                {
                    result = true
                });


            }

            return Json(new { result = false });
        }

        [AllowAnonymous]
        public JsonResult GetInvoiceToSign(string CompGuid)
        {
            var business = new InvoiceBusiness();
            var invoices = business.GetInvoiceToSignByCompanyGuid(CompGuid);
            if (invoices != null)
            {
                return Json(new
                {
                    result = true,
                    Invoices = invoices
                });
            }
            return Json(new { result = false });
        }
    }
}
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HiEIS.Utils;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using HiEIS.Businesses;

namespace HiEIS.Areas.Admin.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleAdmin)]
    public class CompanyController : Controller
    {
        //GET: Admin/Company
        public ActionResult Index()
        {
            return View();
        }

        // GET: Public/Company
        private List<ValidationModel> ValidateUpdate(UpdateCompanyModel model)
        {
            return Validate(model);
        }

        private List<ValidationModel> Validate(CompanyModel model)
        {
            List<ValidationModel> list = new List<ValidationModel>();
            ValidationModel validate;
            if (string.IsNullOrWhiteSpace(model.TaxNo))
            {
                validate = new ValidationModel();
                validate.name = "TaxNo";
                validate.error = "Mã số thuế chưa nhập dữ liệu";
                list.Add(validate);
            }
            else if (!Regex.IsMatch(model.TaxNo, "^.{10,13}$"))
            {
                validate = new ValidationModel();
                validate.name = "TaxNo";
                validate.error = "Mã số thuế phải từ 10-13 ký tự";
                list.Add(validate);
            }
            return list;
        }

        private List<ValidationModel> ValidateCreate(CreateCompanyModel model)
        {
            List<ValidationModel> list = Validate(model);
            ValidationModel validate;
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                validate = new ValidationModel();
                validate.name = "Name";
                validate.error = "Vui lòng nhập tên doanh nghiệp";
                list.Add(validate);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                validate = new ValidationModel();
                validate.name = "Address";
                validate.error = "Vui lòng nhập địa chỉ";
                list.Add(validate);
            }
            if (string.IsNullOrWhiteSpace(model.TaxNo))
            {
                validate = new ValidationModel();
                validate.name = "TaxNo";
                validate.error = "Vui lòng nhập mã số thuế";
                list.Add(validate);
            }
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                validate = new ValidationModel();
                validate.name = "TaxNo";
                validate.error = "Vui lòng nhập email";
                list.Add(validate);
            }
            if (string.IsNullOrWhiteSpace(model.Tel))
            {
                validate = new ValidationModel();
                validate.name = "Tel";
                validate.error = "Vui lòng nhập số điện thoại";
                list.Add(validate);
            }
            return list;
        }

        public ActionResult List(DataTableRequestModel model, string compName, string compTaxNo, string compAddress, string compTel)
        {
            var business = new Businesses.CompanyBusiness();
            var response = business.List(model, compName, compTaxNo, compAddress, compTel);
            return Json(response);
        }

        //GET: Admin/Company/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Admin/Company/CreateAsync
        [HttpPost]
        public JsonResult CreateAsync(CreateCompanyModel model)
        {
            if (this.ModelState.IsValid)
            {
                var business = new CompanyBusiness();
                var success = business.CreateCompany(model);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã tạo doanh nghiệp.",
                        url = "/Admin/Company"
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
                    data = new ValidationResultModel(ModelState),
                    message = "Dữ liệu không phù hợp"
                });
            }
        }

        //GET: Admin/Company/Edit/{id}
        public ActionResult Edit(int id)
        {
            var business = new CompanyBusiness();
            var company = business.GetCompanyById(id);

            if (company == null)
            {
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //POST: Admin/Company/Edit
        [HttpPost]
        public JsonResult Edit(UpdateCompanyModel model)
        {
            if (this.ModelState.IsValid)
            {
                var business = new CompanyBusiness();
                bool success = business.UpdateCompany(model.Id, model);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật hóa đơn.",
                        url = "/Admin/Company"
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
                    data = new ValidationResultModel(ModelState),
                    message = "Dữ liệu không hợp lệ."
                });
            }
        }

        //POST: Admin/Company/Delete/{id}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var business = new CompanyBusiness();
            bool success = business.DeleteCompany(id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa doanh nghiệp.",
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

        //POST: Admin/Company/Activate/{id}
        [HttpPost]
        public JsonResult Activate(int id)
        {
            var business = new CompanyBusiness();
            bool success = business.ActivateCompany(id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã khôi phục doanh nghiệp.",
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

        //POST: Admin/Company/Deactivate/{id}
        [HttpPost]
        public JsonResult Deactivate(int id)
        {
            var business = new CompanyBusiness();
            bool success = business.DeactivateCompany(id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã vô hiệu hóa doanh nghiệp.",
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

    }
}
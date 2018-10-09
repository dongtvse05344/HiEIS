using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using HiEIS.Businesses;
using HiEIS.Models;
using HiEIS.Utils;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleStaffs)]
    public class CustomerController : HiEISCompanyController
    {
        //GET: Public/Customer
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Index()
        {
            var comp = this.Company;
            var productBusiness = new ProductBusiness();
            var products = productBusiness.GetAllProducts(comp.Id);
            ViewBag.Products = products;
            return View();
        }

        private List<ValidationModel> Validate(CustomerModel model)
        {
            List<ValidationModel> list = new List<ValidationModel>();
            ValidationModel validate;
            if (!Regex.IsMatch(model.TaxNo, "^.{10,13}$"))
            {
                validate = new ValidationModel();
                validate.name = "TaxNo";
                validate.error = "Mã số thuế phải từ 10-13 ký tự";
                list.Add(validate);
            }
            return list;
        }

        private List<ValidationModel> ValidateCreate(CreateCustomerModel model)
        {
            List<ValidationModel> list = Validate(model);
            ValidationModel validate;
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                validate = new ValidationModel();
                validate.name = "Name";
                validate.error = "Tên doanh nghiệp chưa nhập dữ liệu";
                list.Add(validate);
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                validate = new ValidationModel();
                validate.name = "Address";
                validate.error = "Địa chỉ chưa nhập dữ liệu";
                list.Add(validate);
            }
            return list;
        }

        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult DeleteCustomerProduct(DeleteCustomerProductModel model)
        {
            try
            {
                var business = new Businesses.CustomerBusiness();
                bool result = business.DeleteCustomerProduct(model, this.Company.Id);
                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Xóa sản phẩm thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Có lỗi xảy ra, vui lòng thử lại"
                    });
                }
            }
            catch (Exceptions.NotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Sản phẩm không tồn tại"
                });
            }
            catch (Exceptions.ProductNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Sản phẩm không tồn tại"
                });
            }
            catch (Exceptions.CustomerNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Khách hàng không tồn tại"
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

        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult EditCustomerProduct(EditCustomerProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new Businesses.CustomerBusiness();
                    business.EditCustomerProduct(model, this.Company.Id);
                    return Json(new
                    {
                        success = true,
                        message = "Thêm sản phẩm thành công"
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
            catch (Exceptions.ProductNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Sản phẩm không tồn tại"
                });
            }
            catch (Exceptions.CustomerNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Khách hàng không tồn tại"
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

        //GET: Public/Customer/List
        public ActionResult List(DataTableRequestModel model,string customerName, string enterprise, string taxNo)
        {
            var comp = this.Company;
            var business = new Businesses.CustomerBusiness();
            var response = business.List(model, comp.Id, customerName, enterprise, taxNo);
            return Json(response);
        }

        //GET: Public/Customer/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Create()
        {
            return View();
        }

        //POST: Public/Customer/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        [HttpPost]
        public ActionResult Create(string id)
        {
            try
            {
                var business = new Businesses.CustomerBusiness();
                bool result = business.CreateCustomer(id, this.Company.Id);
                //add customer to this company
                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Khởi tạo khách hàng thành công"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Có lỗi xảy ra, vui lòng thử lại"
                    });
                }
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
        
        //POST: Public/Customer/Delete/{id}
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult DeleteCustomer(string id)
        {

            var business = new Businesses.CustomerBusiness();
            bool result = business.DeleteCustomer(id, this.Company.Id);
            if (result)
            {
                return Json(new
                {
                    success = true,
                    message = "Xóa khách hàng thành công"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }


        }

        public ActionResult GetNonCustomerCompany(DataTableRequestModel model)
        {
            var comp = this.Company;
            var business = new Businesses.CustomerBusiness();
            var response = business.GetNonCustomerCompany(model, comp.Id);
            return Json(response);
        }

        public ActionResult GetAllCustomers(DataTableRequestModel model)
        {
            var comp = this.Company;
            var business = new Businesses.CustomerBusiness();
            var response = business.GetAllCustomers(model, comp.Id);
            return Json(response);
        }

        public ActionResult CheckIfCustomerBelongsToCompany(string customerId)
        {
            var comp = this.Company;
            var business = new Businesses.CustomerBusiness();
            bool result = business.CheckIfCustomerBelongsToCompany(customerId, comp.Id);
            //add customer to this company
            if (result)
            {
                return Json(new
                {
                    success = true,
                    message = "Khách hàng đã có trong danh sách của công ty"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Khách hàng không có trong danh sách của công ty"
                });
            }
        }
        
        public ActionResult GetLiabilitiesByCustomerId(string id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var comp = this.Company;
                    var business = new CustomerBusiness();
                    string result = business.GetLiabilities(id, comp.Id);
                    return Json(new
                    {
                        success = true,
                        liabilities = result
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
    }
}
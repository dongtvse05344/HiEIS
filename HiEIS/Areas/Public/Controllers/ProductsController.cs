using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Entities;
using HiEIS.Businesses;
using HiEIS.Models;
using HiEIS.Areas.Public.Models;
using HiEIS.Utils;
using System.Web.Helpers;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleStaffs)]
    public class ProductsController : HiEISCompanyController
    {

        //GET: Public/Products
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Index()
        {
            return View();
        }

        //GET: Public/Products
        public JsonResult GetProducts(DataTableRequestModel model, string productName, string productCode, string minPrice, string maxPrice)
        {
            var company = this.Company;
            var business = new ProductBusiness();
            decimal? min = null, max = null;
            if (minPrice != "")
            {
                min = Decimal.Parse(minPrice);
            }
            if (maxPrice != "")
            {
                max = Decimal.Parse(maxPrice);
            }

            var products = business.GetProducts(model, company.Id, productName, productCode, min, max);
            return Json(products);
        }

        //GET: Public/Products/Create
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Create()
        {
            var rates = HiEISUtil.GetVATRates();

            ViewBag.VATRates = rates;

            return View();
        }

        public JsonResult CreateProduct(CreateProductModel model)
        {
            if (this.ModelState.IsValid)
            {
                var company = this.Company;
                var business = new ProductBusiness();

                bool success = business.AddNewProduct(model, company.Id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã tạo sản phẩm.",
                        url = "/Public/Products"
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

        //GET: Public/Products/Edit/{id}
        [Authorize(Roles = HiEISUtil.RoleManager)]
        public ActionResult Edit(int id)
        {
            var company = this.Company;
            var business = new ProductBusiness();

            if (business.GetCompanyId(id) != company.Id)
            {
                return RedirectToAction("Index");
            }

            var product = business.GetProductById(id);
            var rates = Utils.HiEISUtil.GetVATRates();

            //ViewBag.Product = product;
            ViewBag.VATRates = rates;

            return View(product);
        }

        //POST: Public/Products/Edit
        [Authorize(Roles = HiEISUtil.RoleManager)]
        [HttpPost]
        public ActionResult EditProduct(UpdateProductModel model)
        {
            if (this.ModelState.IsValid)
            {
                var company = this.Company;
                var business = new ProductBusiness();

                bool success = business.UpdateProduct(model, company.Id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật sản phẩm.",
                        url = "/Public/Products"
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

        //POST: Public/Products/Delete/{id}
        [Authorize(Roles = HiEISUtil.RoleManager)]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var company = this.Company;
            var business = new ProductBusiness();

            if (business.GetCompanyId(id) == company.Id)
            {
                business.DeleteProduct(id);
            }

            return Json(new { });
            //return RedirectToAction("Index");
        }
        
        public JsonResult GetProductByCustomerId(DataTableRequestModel model, string id)
        {
            var comp = this.Company;
            var business = new Businesses.ProductBusiness();
            var response = business.GetPoductByCustomerId(model, comp.Id, id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOldNumber(int productId, string customerId)
        {
            var business = new ProductBusiness();
            string result = business.GetOldNumber(productId, customerId);
            return Json(new
            {
                success = true,
                number = result
            });
        }
    }
}
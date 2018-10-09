using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Models;
using HiEIS.Utils;
using static HiEIS.Utils.HiEISUtil;
using HiEIS.Businesses;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleNotAdmin)]
    public class LiabilitiesController : HiEISCompanyController
    {
        // GET: Public/Liabilities
        public ActionResult Index()
        {
            if (User.IsInRole(HiEISUtil.RoleCustomer))
            {
                return View("CustomerView");
            }
            return View("CompanyView");
        }

        [Authorize(Roles = HiEISUtil.RoleLiabilityAccountant)]
        public ActionResult CreateTransaction(CreateTransactionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new TransactionBusiness();
                    var success = business.CreateTransaction(model, this.Company.Id);

                    if (success)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Đã thêm thanh toán"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Có lỗi xảy ra, vui lòng thử lại sau!"
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
                        message = "Dữ liệu không phù hợp"
                    });
                }
            }
            catch (Exceptions.CustomerNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Khách hàng không tồn tại"
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        //Index: CompanyView
        public ActionResult GetCompanyLiabilities(DataTableRequestModel model, string customerName, string customerAddress, string customerTel)
        {
            var comp = this.Company;
            var business = new Businesses.TransactionBusiness();
            var response = business.GetCompanyLiabilities(model, comp.Id, customerName, customerAddress, customerTel);
            return Json(response);
        }

        //Index: CustomerView
        public ActionResult GetCustomerLiabilities(DataTableRequestModel model, string companyName, string companyAddress, string companyTel)
        {
            var business = new Businesses.TransactionBusiness();
            var response = business.GetCustomerLiabilities(model, this.UserId, companyName, companyAddress, companyTel);
            return Json(response);
        }

        /*
            Company View: hiện chi tiết của Customer
        */
        public ActionResult CustomerDetails(HiEIS.Models.DataTableRequestModel model, string Id)
        {
            var business = new Businesses.TransactionBusiness();
            var comp = this.Company;

            var customer = business.GetCustomerLiabilitiesDetail(Id, comp.Id);
            ViewBag.CustomerInfo = customer;
            ViewBag.customerId = Id;

            return View("CustomerDetails");

            //var transaction = business.GetCustomerTransactionDetail(id, company.Id, year);
            //ViewBag.TotalCurrent = transaction.TotalCurrent;//tong no
            //ViewBag.Current = transaction.Current; // no nam nay
            //ViewBag.Last = transaction.Last; //no nam trc
            //ViewBag.Payment = transaction.Payment; //tra
            //ViewBag.LastPayment = transaction.LastPayment;
            //ViewBag.Remain = transaction.Remain;// con lai nam nay
            //ViewBag.LastRemain = transaction.LastRemain; //con lai nam trc
        }
        
        public ActionResult GetLiabilitiesDetail(DataTableRequestModel model, string customerId, int year)
        {
            var comp = this.Company;
            var business = new TransactionBusiness();
            var response = business.GetLiabilitiesDetail(
                model
                , comp.Id
                , customerId
                , year
                , out DetailCustomerTransactionModel trans);

            return Json(new
            {
                data = response,
                transaction = trans
            });
        }

        public ActionResult GetYear(string id)
        {

            if (ModelState.IsValid)
            {
                var comp = this.Company;
                var business = new TransactionBusiness();
                var result = business.GetYearTransaction(id, comp.Id);
                return Json(new
                {
                    success = true,
                    listYear = result
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

        /*
            Customer View: hiện chi tiết của Company 
        */
        public ActionResult CompanyDetails(HiEIS.Models.DataTableRequestModel model, int Id)
        {
            var business = new Businesses.TransactionBusiness();
            var comp = this.Company;

            var company = business.GetCompanyLiabilitiesDetail(this.UserId, Id);
            ViewBag.CompanyInfo = company;
            ViewBag.companyId = Id;
            return View("CompanyDetails");
        }

        public ActionResult GetLiabilitiesDetailByCompanyId(DataTableRequestModel model, int companyId, int year)
        {
            var business = new TransactionBusiness();
            var response = business.GetLiabilitiesDetail(
                model
                , companyId
                , this.UserId
                , year
                , out DetailCustomerTransactionModel trans);

            return Json(new
            {
                data = response,
                transaction = trans
            });
        }

        public ActionResult GetYearByCompanyId(int companyId)
        {

            if (ModelState.IsValid)
            {
                var business = new TransactionBusiness();
                var result = business.GetYearTransactionByCompanyId(this.UserId, companyId);
                return Json(new
                {
                    success = true,
                    listYear = result
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
    }
}
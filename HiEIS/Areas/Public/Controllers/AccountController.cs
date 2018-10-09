using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HiEIS.Businesses;
using HiEIS.Models;
using HiEIS.Utils;
using HiEIS.Areas.Admin.Models;

namespace HiEIS.Areas.Public.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager = null;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Public/Account
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var business = new AccountBusiness();
            
            if (User.IsInRole(HiEISUtil.RoleCustomer))
            {
                var acc = business.GetCusAccById(userId);
                return View("CustomerView", acc);

            }
            else
            {
                var acc = business.GetStaffAccById(userId);
                return View("CompanyView", acc);
            }
        }

        //POST: Public/Account/EditStaff
        [HttpPost]
        public JsonResult EditStaff(UpdateStaffModel model)
        {
            if (ModelState.IsValid)
            {
                var business = new AccountBusiness();
                bool success = business.UpdateStaffAccount(model);

                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật tài khoản"
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
                return Json(new
                {
                    success = false,
                    data = new ValidationResultModel(ModelState),
                    message = "Dữ liệu không phù hợp"
                });
            }

        }

        //POST: Public/Account/EditCustomer
        [HttpPost]
        public JsonResult EditCustomer(UpdateCustomerAccount model)
        {
            if (ModelState.IsValid)
            {
                var business = new AccountBusiness();
                bool success = business.UpdateCustomerAccount(model);

                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật tài khoản"
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
                return Json(new
                {
                    success = false,
                    data = new ValidationResultModel(ModelState),
                    message = "Dữ liệu không phù hợp"
                });
            }

        }

        public ActionResult ChangePassword(string url)
        {
            ViewBag.PreviousUrl = url;

            return View();
        }

        [HttpPost]
        public JsonResult ChangePasswordAsync(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = User.Identity.GetUserId();
                var token = UserManager.GeneratePasswordResetToken(model.Id);
                var result = UserManager.ResetPassword(model.Id, token, model.Password);

                if (result.Succeeded)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã thay đổi mật khẩu"
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
    }
}
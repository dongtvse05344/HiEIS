using HiEIS.Businesses;
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HiEIS.Areas.Admin.Models;
using HiEIS.Utils;

namespace HiEIS.Areas.Admin.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleAdmin)]
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

        /*
            CUSTOMER 
        */
        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult GetAllCustomerAccount(DataTableRequestModel model, string username, string email, string enterprise, string taxno)
        {
            var business = new AccountBusiness();
            var response = business.ListAllCustomers(model, username, email, enterprise, taxno);
            return Json(response);
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync(CreateCustomerAccount model)
        {
            ApplicationUser user = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.CustomerTel, IsActive = true };
                    var createUserResult = await this.UserManager.CreateAsync(user, model.Password);
                    if (createUserResult.Succeeded)
                    {
                        await this.UserManager.AddToRoleAsync(user.Id, "Customer");
                        var business = new AccountBusiness();
                        business.CreateCustomer(model, user.Id);
                        return Json(new
                        {
                            success = true,
                            message = "Đã tạo khách hàng.",
                            url = "/Admin/Account/Customer"
                        });
                    }
                    else
                    {
                        var valid = new ValidationResultModel(createUserResult);
                        return Json(new
                        {
                            success = false,
                            data = valid,
                            message = "Dữ liệu không phù hợp"
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
            catch (Exception)
            {
                if (user != null)
                {
                    this.UserManager.Delete(user);
                }
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        public ActionResult EditCustomer(string id)
        {
            var business = new AccountBusiness();
            var customer = business.GetCusAccById(id);
            if (customer == null)
            {
                return RedirectToAction("Customer");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditCustomer(UpdateCustomerAccount model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new AccountBusiness();
                    business.UpdateCustomerAccount(model);

                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật khách hàng"
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

            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        [HttpPost]
        public JsonResult Activate(string id)
        {
            var business = new AccountBusiness();
            bool success = business.ActivateAccount(id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã khôi phục tài khoản.",
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

        [HttpPost]
        public JsonResult Deactivate(string id)
        {
            string userId = User.Identity.GetUserId();
            if (userId == id)
            {
                return Json(new
                {
                    success = false,
                    message = "Tại khoản hiện tại đang đăng nhập, không thể vô hiệu hóa.",
                    title = "Không thành công"
                });
            }
            else
            {
                var business = new AccountBusiness();
                bool success = business.DeactivateAccount(id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã vô hiệu hóa tài khoản.",
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

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var business = new AccountBusiness();
            bool success = business.DeleteCusAcc(id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa tài khoản.",
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

        /*
            ADMIN 
        */
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult GetAllAdminAccount(DataTableRequestModel model, string username, string email)
        {
            var business = new AccountBusiness();
            var response = business.ListAllAdmin(model, username, email);
            return Json(response);
        }

        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateAdminAsync(CreateAdminAccount model)
        {
            ApplicationUser user = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber, IsActive = true };
                    var createUserResult = await this.UserManager.CreateAsync(user, model.Password);
                    if (createUserResult.Succeeded)
                    {
                        await this.UserManager.AddToRoleAsync(user.Id, "Admin");

                        return Json(new
                        {
                            success = true,
                            message = "Đã tạo Admin.",
                            url = "/Admin/Account/Admin"
                        });
                    }
                    else
                    {
                        var valid = new ValidationResultModel(createUserResult);
                        return Json(new
                        {
                            success = false,
                            data = valid,
                            message = "Dữ liệu không phù hợp"
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
            catch (Exception)
            {
                if (user != null)
                {
                    this.UserManager.Delete(user);
                }
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteAccount(string id)
        {
            string userId = User.Identity.GetUserId();
            if (userId == id)
            {
                return Json(new
                {
                    success = false,
                    message = "Tại khoản hiện tại đang đăng nhập, không thể xóa.",
                    title = "Không thành công"
                });
            }
            else
            {
                var business = new AccountBusiness();
                bool success = business.DeleteAccount(id);
                if (success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Đã xóa tài khoản.",
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

        public ActionResult EditAdmin(string id)
        {
            var business = new AccountBusiness();
            var customer = business.GetAdminAccById(id);
            if (customer == null)
            {
                return RedirectToAction("Admin");
            }
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditAdmin(UpdateAdminAccount model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new AccountBusiness();
                    business.UpdateAdminAccount(model);

                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật admin"
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

            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        /*
            STAFF 
         */
        public ActionResult Staff()
        {
            return View();
        }

        public JsonResult GetAllStaffAccounts(DataTableRequestModel model, string name, string userName, string email, string companyName)
        {
            var business = new AccountBusiness();
            var response = business.ListAllStaffs(model, name, userName, email, companyName);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GET: Admin/Account/CreateStaff
        public ActionResult CreateStaff()
        {
            var roles = HiEISUtil.GetAllStaffRoles();
            ViewBag.StaffRoles = roles;

            return View();
        }

        //POST: Admin/Account/CreateStaffAsync
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateStaffAsync(CreateStaffAccountModel model)
        {
            ApplicationUser user = null;
            try
            {
                if (ModelState.IsValid)
                {
                    user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Tel, IsActive = true };
                    var createUserResult = await this.UserManager.CreateAsync(user, model.Password);
                    if (createUserResult.Succeeded)
                    {
                        foreach (var role in model.Roles)
                        {
                            await this.UserManager.AddToRoleAsync(user.Id, role);
                        }
                        var business = new StaffBusiness();
                        business.CreateStaff(model, model.CompanyId, user.Id);
                        return Json(new
                        {
                            success = true,
                            message = "Đã tạo tài khoản doanh nghiệp.",
                            url = "/Admin/Account/Staff"
                        });
                    }
                    else
                    {
                        var valid = new ValidationResultModel(createUserResult);
                        return Json(new
                        {
                            success = false,
                            data = valid,
                            message = "Dữ liệu không phù hợp"
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
            catch (Exception)
            {
                if (user != null)
                {
                    this.UserManager.Delete(user);
                }
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        //GET: Admin/Account/Edit/{id}
        public ActionResult EditStaff(string id)
        {
            var business = new AccountBusiness();
            var roles = HiEISUtil.GetAllStaffRoles();

            var acc = business.GetStaffAccById(id);
            if (acc == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.StaffRoles = roles;
            return View(acc);
        }

        //POST: Admin/Account/Edit
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditStaffAsync(UpdateStaffModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new AccountBusiness();
                    business.UpdateStaffAccount(model);

                    if (model.Roles.Count > 0)
                    {
                        var roles = await UserManager.GetRolesAsync(model.Id);
                        await UserManager.RemoveFromRolesAsync(model.Id, roles.ToArray());
                        foreach (var role in model.Roles)
                        {
                            await this.UserManager.AddToRoleAsync(model.Id, role);
                        }
                    }

                    return Json(new
                    {
                        success = true,
                        message = "Đã cập nhật tài khoản doanh nghiệp",
                        url = "/Admin/Account/Staff"
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
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Có lỗi xảy ra, vui lòng thử lại"
                });
            }
        }

        //POST: Admin/Account/DeleteStaff/{id}
        [HttpPost]
        public JsonResult DeleteStaff(string id)
        {
            var business = new AccountBusiness();
            bool success = business.DeleteStaff(id);

            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa tài khoản doanh nghiệp.",
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

        public ActionResult ChangePassword(string id, string url)
        {
            ViewBag.AccId = id;
            ViewBag.PreviousUrl = url;

            return View();
        }

        [HttpPost]
        public JsonResult ChangePasswordAsync(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không tìm thấy tài khoản"
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
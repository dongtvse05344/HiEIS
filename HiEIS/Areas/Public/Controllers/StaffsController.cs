using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HiEIS.Utils;
using HiEIS.Businesses;

namespace HiEIS.Areas.Public.Controllers
{
    [Authorize(Roles = HiEISUtil.RoleManager)]
    public class StaffsController : HiEISCompanyController
    {
        private ApplicationUserManager _userManager = null;
        //private int companyId = 1;

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

        //GET: Public/Staffs
        public ActionResult Index()
        {
            return View();
        }

        //GET: Public/Staffs
        public ActionResult List(DataTableRequestModel model, string staffName, string staffCode, string staffTel, string staffUsername)
        {
            var business = new StaffBusiness();
            var response = business.List(model, this.Company.Id, staffName, staffCode, staffTel, staffUsername);
            return Json(response);
        }

        //GET: Public/Staffs/Create
        public ActionResult Create()
        {
            var roles = HiEISUtil.GetStaffRoles();
            ViewBag.StaffRoles = roles;

            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreateAsync(CreateStaffModel model)
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
                        var business = new Businesses.StaffBusiness();
                        business.CreateStaff(model, this.Company.Id, user.Id);
                        return Json(new
                        {
                            success = true,
                            message = "Đã tạo nhân viên.",
                            url = "/Public/Staffs"
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

        //GET: Public/Staffs/Edit/{id}
        public ActionResult Edit(string id)
        {
            var business = new StaffBusiness();
            var roles = HiEISUtil.GetStaffRoles();
            var company = this.Company;

            var staff = business.GetStaffById(id, company.Id);
            if (staff == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.StaffRoles = roles;
            return View(staff);
        }

        //POST: Public/Staffs/Edit
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(UpdateStaffModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var business = new Businesses.StaffBusiness();
                    business.UpdateStaff(model, this.Company.Id);

                    if (model.Roles[0] != "") //if not Manager??
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
                        message = "Đã cập nhật nhân viên"
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
            catch (Exceptions.StaffNotFoundException)
            {
                return Json(new
                {
                    success = false,
                    message = "Nhân viên không tồn tại"
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

        //POST: Public/Staffs/Delete/{id}
        [HttpPost]
        public JsonResult Delete(string id)
        {
            var business = new StaffBusiness();
            var company = this.Company;

            bool success = business.DeleteStaff(id, company.Id);
            if (success)
            {
                return Json(new
                {
                    success = true,
                    message = "Đã xóa nhân viên.",
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
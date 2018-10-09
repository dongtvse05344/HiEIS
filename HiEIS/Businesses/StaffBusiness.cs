using AutoMapper;
using HiEIS.Entities;
using HiEIS.Models;
using HiEIS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiEIS.Businesses
{
    public class StaffBusiness
    {
        public DataTableResponseModel<TableStaffModel> List(
                                DataTableRequestModel model
                                , int companyId
                                , string staffName
                                , string staffCode
                                , string staffTel
                                , string staffUsername)
        {
            staffName = (staffName ?? "").ToLower();
            staffCode = (staffCode ?? "").ToLower();
            staffTel = (staffTel ?? "").ToLower();
            staffUsername = (staffUsername ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullName = string.IsNullOrWhiteSpace(staffName);
            var isNullCode = string.IsNullOrWhiteSpace(staffCode);
            var isNullTel = string.IsNullOrWhiteSpace(staffTel);
            var isNullUsername = string.IsNullOrWhiteSpace(staffUsername);

            using (var dbContext = new HiEISEntities())
            {
                var companyStaffs = dbContext.Staffs
                      .Where(a => a.CompanyId == companyId && a.AspNetUser.IsActive == true);
                var queriedList = companyStaffs
                        .Where(a => 
                                   (isNull || (a.Name.Contains(model.searchPhase)))
                                && (isNullName || a.Name.ToLower().Contains(staffName))
                                && (isNullCode || a.Code.ToLower().Contains(staffCode))
                                && (isNullTel || a.Tel.ToLower().Contains(staffTel))
                                && (isNullUsername || a.AspNetUser.UserName.ToLower().Contains(staffUsername))    
                              );
                IOrderedQueryable<Staff> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(TableStaffModel.Name):
                        sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                        break;
                    case nameof(TableStaffModel.Code):
                        sortedList = queriedList.OrderBy(a => a.Code, isAsc);
                        break;
                    case nameof(TableStaffModel.UserName):
                        sortedList = queriedList.OrderBy(a => a.AspNetUser.UserName, isAsc);
                        break;
                    case nameof(TableStaffModel.Address):
                        sortedList = queriedList.OrderBy(a => a.Address, isAsc);
                        break;
                    case nameof(TableStaffModel.Email):
                        sortedList = queriedList.OrderBy(a => a.AspNetUser.Email, isAsc);
                        break;
                    case nameof(TableStaffModel.Tel):
                        sortedList = queriedList.OrderBy(a => a.Tel, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<TableStaffModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Staff, TableStaffModel>)
                    .ToList();
                response.total = companyStaffs.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public UpdateStaffModel GetStaffById(string id, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var staffInDb = db.Staffs.Find(id);
                var staff = new UpdateStaffModel();
                if (staffInDb != null && staffInDb.CompanyId == companyId)
                {
                    staff = Mapper.Map<Staff, UpdateStaffModel>(staffInDb);
                }
                else
                {
                    staff = null;
                }
                return staff;
            }
        }

        public bool CreateStaff(CreateStaffModel model, int companyId, string staffId)
        {
            bool result;
            using (var dbcontext = new HiEISEntities())
            {
                try
                {
                    var company = dbcontext.Companies.Find(companyId);
                    var staff = new Staff();
                    staff.Id = staffId;
                    staff.Name = model.Name;
                    staff.Code = model.Code;
                    staff.Address = model.Address;
                    staff.Tel = model.Tel;
                    staff.CompanyId = companyId;

                    dbcontext.Staffs.Add(staff);
                    dbcontext.SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                }
            }
            return result;
        }
        
        public bool UpdateStaff(UpdateStaffModel model, int companyId)
        {
            bool result = false;
            using (var dbcontext = new HiEISEntities())
            {
                try
                {
                    if (dbcontext.Staffs
                                .Any(a => a.Id == model.Id
                                && a.CompanyId == companyId))
                    {
                        var staff = dbcontext.Staffs
                            .FirstOrDefault(a => a.Id == model.Id
                                            && a.CompanyId == companyId);
                        staff.Name = model.Name;
                        staff.AspNetUser.Email = model.AspNetUserEmail;
                        staff.Code = model.Code;
                        staff.Address = model.Address;
                        staff.Tel = model.Tel;
                        dbcontext.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                }
            }
            return result;
        }

        public bool DeleteStaff(string id, int companyId)
        {
            bool result = false;

            using (var db = new HiEISEntities())
            {
                try
                {
                    var staffInDb = db.Staffs.Find(id);
                    if (staffInDb != null && staffInDb.CompanyId == companyId)
                    {
                        staffInDb.AspNetUser.IsActive = false;
                        db.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                }
            }

            return result;
        }
    }
}
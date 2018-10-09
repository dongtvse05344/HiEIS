using AutoMapper;
using HiEIS.Areas.Admin.Models;
using HiEIS.Entities;
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiEIS.Utils;

namespace HiEIS.Businesses
{
    public class AccountBusiness
    {
        public void Login(string username, string password)
        {
            using (var dbcontext = new Entities.HiEISEntities())
            {
                var user = dbcontext.AspNetUsers.FirstOrDefault(u => u.UserName == username);
                var users = dbcontext.AspNetUsers.Where(u => u.EmailConfirmed).ToList();
            }
        }

        public DataTableResponseModel<ListAccountViewModel> List(
                                    DataTableRequestModel model
                                    , string username
                                    , string email)
        {
            username = (username ?? "").ToLower();
            email = (email ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullUsername = string.IsNullOrWhiteSpace(username);
            var isNullEmail = string.IsNullOrWhiteSpace(email);
            using (var db = new HiEISEntities())
            {
                var queriedList = db.AspNetUsers
                                 .Where(a =>
                                   (isNull || (a.UserName.Contains(model.searchPhase)))
                                && (isNullUsername || a.UserName.ToLower().Contains(username))
                                && (isNullEmail || a.Email.ToLower().Contains(email))
                              );
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<ListAccountViewModel>();
                response.data = pagingList
                    .Select(Mapper.Map<AspNetUser, ListAccountViewModel>)
                    .ToList();
                response.total = db.AspNetUsers.Count();
                response.display = queriedList.Count();
                return response;
            }

        }

        public DataTableResponseModel<AccountCustomerViewModel> ListAllCustomers(
                                        DataTableRequestModel model
                                        , string username
                                        , string email
                                        , string enterprise
                                        , string taxno)
        {
            username = (username ?? "").ToLower();
            email = (email ?? "").ToLower();
            enterprise = (enterprise ?? "").ToLower();
            taxno = (taxno ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullUsername = string.IsNullOrWhiteSpace(username);
            var isNullEmail = string.IsNullOrWhiteSpace(email);
            var isNullEnterprise = string.IsNullOrWhiteSpace(enterprise);
            var isNullTaxNo = string.IsNullOrWhiteSpace(taxno);
            using (var db = new HiEISEntities())
            {
                var temCus = new AspNetUser();
                var customerList = db.AspNetUsers.Where(a => a.AspNetRoles.Select(r => r.Name).Contains(HiEISUtil.RoleCustomer));
                var queriedList = customerList
                                 .Where(a =>
                                 (isNull || (a.UserName.Contains(model.searchPhase)))
                                && (isNullUsername || a.UserName.ToLower().Contains(username))
                                && (isNullEmail || a.Email.ToLower().Contains(email))
                                && (isNullEnterprise || a.Customer.Enterprise.ToLower().Contains(enterprise))
                                && (isNullTaxNo || a.Customer.TaxNo.ToLower().Contains(taxno))
                              );
                //var sortedList = queriedList.OrderBy(a => a.Id);
                IOrderedQueryable<AspNetUser> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(temCus.Customer.Name):
                        sortedList = queriedList.OrderBy(a => a.Customer.Name, isAsc);
                        break;
                    case nameof(temCus.UserName):
                        sortedList = queriedList.OrderBy(a => a.UserName, isAsc);
                        break;
                    case nameof(temCus.Customer.Enterprise):
                        sortedList = queriedList.OrderBy(a => a.Customer.Enterprise, isAsc);
                        break;
                    case nameof(temCus.Email):
                        sortedList = queriedList.OrderBy(a => a.Email, isAsc);
                        break;
                    case nameof(temCus.Customer.TaxNo):
                        sortedList = queriedList.OrderBy(a => a.Customer.TaxNo, isAsc);
                        break;
                    case nameof(temCus.Customer.Tel):
                        sortedList = queriedList.OrderBy(a => a.Customer.Tel, isAsc);
                        break;
                    case nameof(temCus.Customer.Bank):
                        sortedList = queriedList.OrderBy(a => a.Customer.Bank, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<AccountCustomerViewModel>();
                response.data = pagingList
                    .Select(Mapper.Map<AspNetUser, AccountCustomerViewModel>)
                    .ToList();
                
                response.total = customerList.Count();
                response.display = queriedList.Count();
                return response;
            }

        }


        public bool CreateCustomer(CreateCustomerAccount model, string customerId)
        {
            bool result = false;
            using (var dbcontext = new HiEISEntities())
            {
                try
                {
                    var customer = new Customer();
                    customer.Id = customerId;
                    customer.Name = model.CustomerName;
                    customer.Enterprise = model.CustomerEnterprise;
                    customer.TaxNo = model.CustomerTaxNo;
                    customer.Address = model.CustomerAddress;
                    customer.Tel = model.CustomerTel;
                    customer.Fax = model.CustomerFax;
                    customer.Bank = model.CustomerBank;
                    customer.BankAccountNumber = model.CustomerBankAccountNumber;
                    //customer.TotalLiabilities = 0;
                    dbcontext.Customers.Add(customer);
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

        public UpdateCustomerAccount GetCusAccById(string id)
        {
            using (var db = new HiEISEntities())
            {
                var accInDb = db.AspNetUsers.Find(id);
                var acc = new UpdateCustomerAccount();
                if (accInDb != null)
                {
                    acc = Mapper.Map<AspNetUser, UpdateCustomerAccount>(accInDb);
                }
                else
                {
                    acc = null;
                }
                return acc;
            }
        }

        public bool UpdateCustomerAccount(UpdateCustomerAccount model)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var customer = db.Customers.Find(model.Id);
                    customer.Name = model.CustomerName;
                    customer.Enterprise = model.CustomerEnterprise;
                    customer.TaxNo = model.CustomerTaxNo;
                    customer.Address = model.CustomerAddress;
                    customer.Tel = model.CustomerTel;
                    customer.Fax = model.CustomerFax;
                    customer.Bank = model.CustomerBank;
                    customer.BankAccountNumber = model.CustomerBankAccountNumber;
                    customer.AspNetUser.Email = model.Email;
                    db.SaveChanges();
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

        public bool ActivateAccount(string id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var accInDb = db.AspNetUsers.Find(id);
                    if (accInDb != null)
                    {
                        accInDb.IsActive = true;

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

        public bool DeactivateAccount(string id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {

                try
                {
                    var accInDb = db.AspNetUsers.Find(id);
                    if (accInDb != null)
                    {
                        accInDb.IsActive = false;

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

        public bool DeleteCusAcc(string id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var accInDb = db.AspNetUsers.Find(id);
                    var accInCus = db.Customers.Find(id);
                    if (accInDb != null && accInCus != null)
                    {
                        db.Customers.Remove(accInCus);
                        db.AspNetUsers.Remove(accInDb);

                        db.SaveChanges();

                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    //throw;
                }
            }

            return result;
        }

        public DataTableResponseModel<AdminViewModel> ListAllAdmin(
                                        DataTableRequestModel model
                                        , string username
                                        , string email)
        {
            username = (username ?? "").ToLower();
            email = (email ?? "").ToLower();

            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullUsername = string.IsNullOrWhiteSpace(username);
            var isNullEmail = string.IsNullOrWhiteSpace(email);

            using (var db = new HiEISEntities())
            {
                var temAdmin = new AspNetUser();
                var adminList = db.AspNetUsers.Where(a => a.AspNetRoles.Select(r => r.Name).Contains(HiEISUtil.RoleAdmin));
                var queriedList = adminList
                                 .Where(a =>
                                 (isNull || (a.UserName.Contains(model.searchPhase)))
                                && (isNullUsername || a.UserName.ToLower().Contains(username))
                                && (isNullEmail || a.Email.ToLower().Contains(email))
                              );
                //var sortedList = queriedList.OrderBy(a => a.Id);
                IOrderedQueryable<AspNetUser> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(temAdmin.PhoneNumber):
                        sortedList = queriedList.OrderBy(a => a.PhoneNumber, isAsc);
                        break;
                    case nameof(temAdmin.UserName):
                        sortedList = queriedList.OrderBy(a => a.UserName, isAsc);
                        break;
                    case nameof(temAdmin.Email):
                        sortedList = queriedList.OrderBy(a => a.Email, isAsc);
                        break;
                   
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<AdminViewModel>();
                response.data = pagingList
                    .Select(Mapper.Map<AspNetUser, AdminViewModel>)
                    .ToList();
                response.total = adminList.Count();
                response.display = queriedList.Count();
                return response;
            }

        }
        public bool DeleteAccount(string id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var accInDb = db.AspNetUsers.Find(id);
                    if (accInDb != null)
                    {
                        db.AspNetUsers.Remove(accInDb);

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

        public UpdateAdminAccount GetAdminAccById(string id)
        {
            using (var db = new HiEISEntities())
            {
                var accInDb = db.AspNetUsers.Find(id);
                var acc = new UpdateAdminAccount();
                if (accInDb != null)
                {
                    acc = Mapper.Map<AspNetUser, UpdateAdminAccount>(accInDb);
                }
                else
                {
                    acc = null;
                }
                return acc;
            }
        }

        public bool UpdateAdminAccount(UpdateAdminAccount model)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var acc = db.AspNetUsers.Find(model.Id);
                    acc.Email = model.Email;
                    acc.PhoneNumber = model.PhoneNumber;
                    db.SaveChanges();
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

        public DataTableResponseModel<StaffAccountModel> ListAllStaffs(
                                DataTableRequestModel model
                                , string name
                                , string userName
                                , string email
                                , string companyName)
        {
            name = (name ?? "").ToLower();
            userName = (userName ?? "").ToLower();
            email = (email ?? "").ToLower();
            companyName = (companyName ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullName = string.IsNullOrWhiteSpace(name);
            var isNullUserName = string.IsNullOrWhiteSpace(userName);
            var isNullEmail = string.IsNullOrWhiteSpace(email);
            var isNullCompany = string.IsNullOrWhiteSpace(companyName);

            using (var dbContext = new HiEISEntities())
            {
                var companyStaffs = dbContext.Staffs;
                var queriedList = companyStaffs
                        .Where(a =>
                                   (isNull || (a.Name.Contains(model.searchPhase)))
                                && (isNullName || a.Name.ToLower().Contains(name))
                                && (isNullUserName || a.AspNetUser.UserName.ToLower().Contains(userName))
                                && (isNullEmail || a.AspNetUser.Email.ToLower().Contains(email))
                                && (isNullCompany || a.Company.Name.ToLower().Contains(companyName))
                              );
                IOrderedQueryable<Staff> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(StaffAccountModel.CompanyName):
                        sortedList = queriedList.OrderBy(a => a.Company.Name, isAsc);
                        break;
                    case nameof(StaffAccountModel.Name):
                        sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                        break;
                    case nameof(StaffAccountModel.UserName):
                        sortedList = queriedList.OrderBy(a => a.AspNetUser.UserName, isAsc);
                        break;
                    case nameof(StaffAccountModel.Address):
                        sortedList = queriedList.OrderBy(a => a.Address, isAsc);
                        break;
                    case nameof(StaffAccountModel.Email):
                        sortedList = queriedList.OrderBy(a => a.AspNetUser.Email, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<StaffAccountModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Staff, StaffAccountModel>)
                    .ToList();
                response.total = companyStaffs.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public UpdateStaffModel GetStaffAccById(string id)
        {
            using (var db = new HiEISEntities())
            {
                var staffInDb = db.Staffs.Find(id);
                var staff = new UpdateStaffModel();

                if (staffInDb != null)
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
        
        public bool UpdateStaffAccount(UpdateStaffModel model)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var accInDb = db.Staffs.Find(model.Id);

                    if (accInDb != null)
                    {
                        accInDb.Name = model.Name;
                        accInDb.AspNetUser.Email = model.AspNetUserEmail;
                        accInDb.Code = model.Code;
                        accInDb.Address = model.Address;
                        accInDb.Tel = model.Tel;

                        db.SaveChanges();

                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    //throw;
                }
            }
            return result;
        }

        public bool DeleteStaff(string id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var accInDb = db.AspNetUsers.Find(id);
                    var staff = db.Staffs.Find(id);

                    if (accInDb != null && staff != null)
                    {
                        db.Staffs.Remove(staff);
                        db.AspNetUsers.Remove(accInDb);

                        db.SaveChanges();

                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    //throw;
                }
            }
            return result;
        }
    }
}
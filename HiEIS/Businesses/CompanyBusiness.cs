using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiEIS.Entities;
using AutoMapper;
using HiEIS.Utils;

namespace HiEIS.Businesses
{
    public class CompanyBusiness
    {
        public DataTableResponseModel<TableCompanyModel> List(DataTableRequestModel model
                                                                , string compName
                                                                , string compTaxNo
                                                                , string compAddress
                                                                , string compTel)
        {
            compName = (compName ?? "").ToLower();
            compTaxNo = (compTaxNo ?? "").ToLower();
            compAddress = (compAddress ?? "").ToLower();
            compTel = (compTel ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullCompName = string.IsNullOrWhiteSpace(compName);
            var isNullCompTaxNo = string.IsNullOrWhiteSpace(compTaxNo);
            var isNullCompAddress = string.IsNullOrWhiteSpace(compAddress);
            var isNullCompTel = string.IsNullOrWhiteSpace(compTel);

            using (var dbContext = new HiEISEntities())
            {
                var tempComp = new Company();
                var queriedList = dbContext.Companies
                      .Where(a =>
                                   (isNull || (a.Name.Contains(model.searchPhase)))
                                && (isNullCompName || a.Name.Contains(compName))
                                && (isNullCompTaxNo || a.TaxNo.Contains(compTaxNo))
                                && (isNullCompAddress || a.Address.Contains(compAddress))
                                && (isNullCompTel || a.Tel.Contains(compTel))
                            );
                IOrderedQueryable<Company> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(tempComp.Tel):
                        sortedList = queriedList.OrderBy(a => a.Tel, isAsc);
                        break;
                    case nameof(tempComp.Name):
                        sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                        break;
                    case nameof(tempComp.Address):
                        sortedList = queriedList.OrderBy(a => a.Address, isAsc);
                        break;
                    case nameof(tempComp.Email):
                        sortedList = queriedList.OrderBy(a => a.Email, isAsc);
                        break;
                    case nameof(tempComp.Website):
                        sortedList = queriedList.OrderBy(a => a.Website, isAsc);
                        break;
                    case nameof(tempComp.Fax):
                        sortedList = queriedList.OrderBy(a => a.Fax, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCompanyModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Company, TableCompanyModel>)
                    .ToList();
                response.total = dbContext.Companies.Count();
                response.display = queriedList.Count();
                return response;
            }
        }
        public bool UpdateCompany(int id, CompanyModel model)
        {
            bool result = false;
            using (var dbcontext = new HiEISEntities())
            {
                try
                {
                    var companyInDb = dbcontext.Companies.Find(id);
                    if (companyInDb != null)
                    {
                        Mapper.Map(model, companyInDb);
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

        public bool CreateCompany(CreateCompanyModel model)
        {
            bool result;
            using (var dbcontext = new HiEISEntities())
            {
                try
                {
                    model.IsActive = true;
                    var company = Mapper.Map<CreateCompanyModel, Company>(model);
                    dbcontext.Companies.Add(company);
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

        public bool ActivateCompany(int id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var companyInDb = db.Companies.Find(id);
                    if (companyInDb != null)
                    {
                        companyInDb.IsActive = true;

                        var staffs = companyInDb.Staffs.ToList();
                        if (staffs.Count > 0)
                        {
                            foreach (var staff in staffs)
                            {
                                staff.AspNetUser.IsActive = true;
                            }
                        }

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

        public bool DeactivateCompany(int id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var companyInDb = db.Companies.Find(id);
                    if (companyInDb != null)
                    {
                        companyInDb.IsActive = false;

                        var staffs = companyInDb.Staffs.ToList();
                        if (staffs.Count > 0)
                        {
                            foreach (var staff in staffs)
                            {
                                staff.AspNetUser.IsActive = false;
                            }
                        }

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

        public bool DeleteCompany(int id)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var companyInDb = db.Companies.Find(id);
                    if (companyInDb != null)
                    {
                        db.Companies.Remove(companyInDb);

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

        public Company GetCompanyByUsername(string userId)
        {
            using (var dbContext = new HiEISEntities())
            {
                return dbContext.Companies
                    .FirstOrDefault(a => a.Staffs.Any(s => s.Id == userId));
            }
        }

        public UpdateCompanyModel GetCompanyById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var companyInDb = db.Companies.Find(id);
                var company = new UpdateCompanyModel();

                if (companyInDb != null)
                {
                    company = Mapper.Map<Company, UpdateCompanyModel>(companyInDb);
                }
                else
                {
                    company = null;
                }

                return company;
            }
        }

        public string GetCompanyGuid(string username)
        {
            //var company;
            using (var db = new HiEISEntities())
            {
                var user = db.AspNetUsers.FirstOrDefault(a => a.UserName == username);
                var company = GetCompanyByUsername(user.Id);
                if (company != null)
                {
                    company.CodeGuid = Guid.NewGuid().ToString();
                    var companyInDb = db.Companies.Find(company.Id);
                    if (companyInDb != null)
                    {
                        companyInDb.CodeGuid = company.CodeGuid;
                    }
                    db.SaveChanges();
                    return company.CodeGuid;
                }

            }
            return null;
        }
    }
}
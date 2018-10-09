using AutoMapper;
using HiEIS.Entities;
using HiEIS.Models;
using HiEIS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiEIS.Utils;
namespace HiEIS.Businesses
{
    public class CustomerBusiness
    {
        public bool DeleteCustomerProduct(DeleteCustomerProductModel model, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {
                var company = dbcontext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == model.CustomerId))
                {
                    throw new Exceptions.CustomerNotFoundException();
                }
                if (!company.Products.Any(a => a.Id == model.ProductId))
                {
                    throw new Exceptions.ProductNotFoundException();
                }
                if (dbcontext.CustomerProducts.Any(a => a.CustomerId == model.CustomerId
                                                && a.ProductId == model.ProductId))
                {
                    var customerProduct = dbcontext.CustomerProducts
                        .FirstOrDefault(a => a.CustomerId == model.CustomerId
                                        && a.ProductId == model.ProductId);
                    dbcontext.CustomerProducts.Remove(customerProduct);
                    dbcontext.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exceptions.NotFoundException();
                }
            }
        }

        public bool EditCustomerProduct(EditCustomerProductModel model, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {
                var company = dbcontext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == model.CustomerId))
                {
                    throw new Exceptions.CustomerNotFoundException();
                }
                if (!company.Products.Any(a => a.Id == model.ProductId))
                {
                    throw new Exceptions.ProductNotFoundException();
                }
                if (dbcontext.CustomerProducts
                                .Any(a => a.CustomerId == model.CustomerId
                                        && a.ProductId == model.ProductId))
                {
                    var customerProduct = dbcontext.CustomerProducts
                        .FirstOrDefault(a => a.CustomerId == model.CustomerId
                                        && a.ProductId == model.ProductId);
                    customerProduct.Amount = model.Amount;
                    dbcontext.SaveChanges();
                    return true;
                }
                else
                {
                    var customerProduct = Mapper.Map<EditCustomerProductModel, CustomerProduct>(model);
                    dbcontext.CustomerProducts.Add(customerProduct);
                    dbcontext.SaveChanges();
                    return true;
                }
            }
        }

        public DataTableResponseModel<TableCustomerModel> List(
                            DataTableRequestModel model
                            , int companyId
                            , string customerName
                            , string enterprise
                            , string taxNo)
        {
            customerName = (customerName ?? "").ToLower();
            enterprise = (enterprise ?? "").ToLower();
            taxNo = (taxNo ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullCName = string.IsNullOrWhiteSpace(customerName);
            var isNullEnterprise = string.IsNullOrWhiteSpace(enterprise);
            var isNullTaxNo = string.IsNullOrWhiteSpace(taxNo);
            using (var dbContext = new HiEISEntities())
            {
                var temCus = new Customer();
                var company = dbContext.Companies.Find(companyId);
                IQueryable<Customer> queriedList = company.CompanyCustomers.AsQueryable()
                      .Select(a => a.Customer)
                      .Where(a => (isNull
                         || ((a.TaxNo == null
                                || a.TaxNo.Like(model.searchPhase))
                            || a.Name.Like(model.searchPhase)
                            || a.Enterprise.Like(model.searchPhase)))
                        && ((isNullCName
                            || a.Name.Like(customerName))
                        && (isNullEnterprise
                            || a.Enterprise.Like(enterprise))
                        && (isNullTaxNo
                            || a.TaxNo.Like(taxNo)))
                         );
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCustomerModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Customer, TableCustomerModel>)
                    .ToList();
                response.total = company.CompanyCustomers.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public bool Create(string username, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {
                var user = dbcontext.AspNetUsers.FirstOrDefault(a => a.UserName == username);
                if (user != null && user.AspNetRoles.Any(a => a.Name == "Customer"))
                {
                    var customer = user.Customer;
                    //gan company cho customer
                    var company = dbcontext.Companies.Find(companyId);
                    if (!customer.CompanyCustomers.Any(a => a.CompanyId == companyId))
                    {
                        customer.CompanyCustomers.Add(new CompanyCustomer
                        {
                            CompanyId = company.Id,
                            CustomerId = customer.Id,
                            Liabilities = 0
                        });
                        dbcontext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CreateCustomer(string id, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {

                var customer = dbcontext.Customers.Find(id);
                //gan company cho customer
                var company = dbcontext.Companies.Find(companyId);
                if (!customer.CompanyCustomers.Any(a => a.CompanyId == companyId))
                {
                    customer.CompanyCustomers.Add(new CompanyCustomer
                    {
                        CompanyId = company.Id,
                        CustomerId = customer.Id,
                        Liabilities = 0
                    });
                    dbcontext.SaveChanges();
                    return true;
                }
            }
            return false;
        }


        public DataTableResponseModel<TableCustomerModel> GetNonCustomerCompany(
                               DataTableRequestModel model, int companyId)
        {

            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            using (var dbContext = new HiEISEntities())
            {
                //var company = dbContext.Companies.Find(companyId);
                var queriedList = dbContext.Customers
                      .Where(a => (isNull
                                || ((a.TaxNo == null
                                || a.TaxNo.Contains(model.searchPhase))
                                || a.Name.Contains(model.searchPhase)
                                || a.Enterprise.Contains(model.searchPhase)
                                || a.Tel.Contains(model.searchPhase))
                                )
                                && !a.CompanyCustomers.Any(b => b.CompanyId == companyId)
                                && a.AspNetUser.IsActive == true
                            );
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCustomerModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Customer, TableCustomerModel>)
                    .ToList();
                response.total = dbContext.Customers.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public DataTableResponseModel<TableCustomerModel> GetAllCustomers(
                       DataTableRequestModel model, int companyId)
        {

            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            using (var dbContext = new HiEISEntities())
            {
                //var company = dbContext.Companies.Find(companyId);
                var queriedList = dbContext.Customers
                      .Where(a => (isNull
                                || ((a.TaxNo == null
                                || a.TaxNo.Contains(model.searchPhase))
                                || a.Name.Contains(model.searchPhase)
                                || a.Enterprise.Contains(model.searchPhase)
                                || a.Tel.Contains(model.searchPhase))
                                )
                                && a.AspNetUser.IsActive == true
                                )
                            ;
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCustomerModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Customer, TableCustomerModel>)
                    .ToList();
                response.total = dbContext.Customers.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public bool CheckIfCustomerBelongsToCompany(string customerId, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {
                var company = dbcontext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == customerId))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DeleteCustomer(string customerId, int companyId)
        {
            bool result = false;
            bool saveDB = false;
            using (var dbcontext = new HiEISEntities())
            {

                var company = dbcontext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == customerId))
                {
                    result = false;
                    throw new Exceptions.CustomerNotFoundException();
                }
                else
                {
                    var customer = company.CompanyCustomers
                        .FirstOrDefault(a => a.CustomerId == customerId);
                    company.CompanyCustomers.Remove(customer);
                    dbcontext.SaveChanges();
                    saveDB = true;
                    bool deleteProduct = DeleteAllProductofCustomer(customerId, companyId);
                    if (saveDB == true && deleteProduct == true)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public string GetLiabilities(string customerId, int companyId)
        {
            using (var dbcontext = new HiEISEntities())
            {

                var company = dbcontext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == customerId))
                {
                    throw new Exceptions.CustomerNotFoundException();

                }
                else
                {
                    var customer = company.CompanyCustomers
                        .FirstOrDefault(a => a.CustomerId == customerId);
                    return string.Format("{0:#,##0}", customer.Liabilities);

                }
            }

        }

        public bool DeleteAllProductofCustomer(string customerId, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var deleteProducts = from p in db.Products
                                         join cp in db.CustomerProducts on p.Id equals cp.ProductId
                                         where cp.CustomerId == customerId && p.CompanyId == companyId
                                         select cp;
                    if (deleteProducts != null)
                    {
                        foreach (var product in deleteProducts)
                        {
                            db.CustomerProducts.Remove(product);
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
    }
}
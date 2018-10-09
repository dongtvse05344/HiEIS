using AutoMapper;
using HiEIS.Entities;
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HiEIS.Utils.HiEISUtil;

namespace HiEIS.Businesses
{
    public class TransactionBusiness
    {
        public bool UpdateCompanyCustomerLiability(string customerId, int companyId, int type, decimal amount)
        {
            bool result;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var customer = db.CompanyCustomers.FirstOrDefault(a => a.CompanyId == companyId && a.CustomerId == customerId);
                    if (type == (int)TransactionType.Liability)
                    {
                        customer.Liabilities += amount;
                    }
                    else
                    {
                        customer.Liabilities -= amount;
                    }
                    db.SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        //public void UpdateCustomerLiabilityTransaction(string customerId, int type, decimal amount)
        //{
        //    using (var dbcontext = new HiEISEntities())
        //    {
        //        if (type == (int)Utils.HiEISUtil.TransactionType.Liability)
        //        {
        //            var CustomerLiabilityTransaction = dbcontext.Customers.FirstOrDefault(a => a.Id == customerId);
        //            CustomerLiabilityTransaction.TotalLiabilities += amount;
        //            dbcontext.SaveChanges();
        //        }
        //        else
        //        {
        //            var CustomerLiabilityTransaction = dbcontext.Customers.FirstOrDefault(a => a.Id == customerId);
        //            CustomerLiabilityTransaction.TotalLiabilities -= amount;
        //            dbcontext.SaveChanges();
        //        }
        //    }
        //}

        public bool CreateTransaction(CreateTransactionModel model, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                var company = db.Companies.Find(companyId);
                if (company.CompanyCustomers.Any(a => a.CustomerId == model.CustomerId))
                {
                    try
                    {
                        var transaction = Mapper.Map<CreateTransactionModel, Transaction>(model);
                        transaction.CompanyId = companyId;
                        transaction.Date = DateTime.Now;

                        db.Transactions.Add(transaction);
                        db.SaveChanges();

                        bool success = UpdateCompanyCustomerLiability(model.CustomerId, companyId, model.Type, model.Amount);

                        if (success)
                        {
                            result = true;
                        }

                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public DataTableResponseModel<TableTransactionModel> ListByCustomerId(DataTableRequestModel model, string companyName)
        {
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullcompanyName = string.IsNullOrWhiteSpace(companyName);
            using (var dbContext = new HiEISEntities())
            {
                var customerTransactions = dbContext.Transactions
                      .Where(a => a.CustomerId == model.searchPhase);
                var queriedList = customerTransactions
                      .Where(a => isNull
                        || (isNullcompanyName && a.CustomerId == model.searchPhase)
                        || (a.CustomerId == model.searchPhase && a.Company.Name == companyName));
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableTransactionModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Transaction, TableTransactionModel>)
                    .ToList();
                response.total = customerTransactions.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public DataTableResponseModel<TableTransactionModel> ListByCompanyId(DataTableRequestModel model, int companyId)
        {
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            using (var dbContext = new HiEISEntities())
            {
                var companyTransactions = dbContext.Transactions
                      .Where(a => a.CompanyId == companyId);
                var queriedList = companyTransactions
                      .Where(a => isNull
                         || a.CustomerId == model.searchPhase);
                var sortedList = queriedList.OrderBy(a => a.Id);
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableTransactionModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Transaction, TableTransactionModel>)
                    .ToList();
                response.total = companyTransactions.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public DataTableResponseModel<TableCompanyLiabilitiesModel> GetCompanyLiabilities(
            DataTableRequestModel model
            , int companyId
            , string customerName
            , string customerAddress
            , string customerTel)
        {
            using (var dbContext = new HiEISEntities())
            {
                var temCus = new Customer();
                var customers = dbContext.Customers
                    .Where(a => a.CompanyCustomers.Any(b => b.CompanyId == companyId)
                            && a.Transactions.Any(b => b.CompanyId == companyId));
                var nullCustomerName = string.IsNullOrWhiteSpace(customerName);
                var nullCustomerAddress = string.IsNullOrWhiteSpace(customerAddress);
                var nullCustomerTel = string.IsNullOrWhiteSpace(customerTel);
                var queriedList = customers.Where(a => (nullCustomerName || a.Name.Contains(customerName))
                                                    && (nullCustomerAddress || a.Address.Contains(customerAddress))
                                                    && (nullCustomerTel || a.Tel.Contains(customerTel)));
                //var sortedlist = queriedList.OrderBy(a => a.Name);
                IOrderedQueryable<Customer> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(temCus.Tel):
                        sortedList = queriedList.OrderBy(a => a.Tel, isAsc);
                        break;
                    case nameof(temCus.Address):
                        sortedList = queriedList.OrderBy(a => a.Address, isAsc);
                        break;
                   
                    default:
                        sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCompanyLiabilitiesModel>();
                response.data = pagingList
                    .Select(a => new TableCompanyLiabilitiesModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Address = a.Address,
                        Tel = a.Tel,
                        Current = a.Transactions
                                    .Where(b => b.Type == (int)TransactionType.Liability
                                        && b.CompanyId == companyId)
                                    .Sum(b => b.Amount),
                        Payment = a.Transactions
                                    .Where(b => b.Type == (int)TransactionType.Payment
                                        && b.CompanyId == companyId)
                                    .Sum(b => b.Amount)
                    })
                    .ToList();
                response.total = customers.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public DataTableResponseModel<TableCustomerLiabilitiesModel> GetCustomerLiabilities(
            DataTableRequestModel model
            , string customerId
            , string companyName
            , string companyAddress
            , string companyTel)
        {
            using (var dbContext = new HiEISEntities())
            {
                var companies = dbContext.Companies
                    .Where(a => a.CompanyCustomers.Any(b => b.CustomerId == customerId)
                            && a.Transactions.Any(b => b.CustomerId == customerId));
                var nullCompanyName = string.IsNullOrWhiteSpace(companyName);
                var nullCompanyAddress = string.IsNullOrWhiteSpace(companyAddress);
                var nullCompanyTel = string.IsNullOrWhiteSpace(companyTel);
                var queriedList = companies.Where(a => (nullCompanyName || a.Name.Contains(companyName))
                                                    && (nullCompanyAddress || a.Address.Contains(companyAddress))
                                                    && (nullCompanyTel || a.Tel.Contains(companyTel)));
                var sortedlist = queriedList.OrderBy(a => a.Name);
                var pagingList = sortedlist.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableCustomerLiabilitiesModel>();
                response.data = pagingList
                    .Select(a => new TableCustomerLiabilitiesModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Address = a.Address,
                        Tel = a.Tel,
                        Current = a.Transactions
                                    .Where(b => b.Type == (int)TransactionType.Liability
                                        && b.CustomerId == customerId)
                                    .Sum(b => b.Amount),
                        Payment = a.Transactions
                                    .Where(b => b.Type == (int)TransactionType.Payment
                                        && b.CustomerId == customerId)
                                    .Sum(b => b.Amount)
                    })
                    .ToList();
                response.total = companies.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public DataTableResponseModel<TableLiabilitiesDetailModel> GetLiabilitiesDetail(
            DataTableRequestModel model
            , int companyId
            , string customerId
            , int year
            , out DetailCustomerTransactionModel trans)
        {
            using (var dbContext = new HiEISEntities())
            {
                var temTrans = new Transaction();
                var company = dbContext.Companies.Find(companyId);
                IQueryable<Transaction> queriedList = company.Transactions.AsQueryable().Where(a => a.CustomerId == customerId && a.CompanyId == companyId && a.Date.Year == year);
                //var sortedlist = queriedList.OrderBy(a => a.Id);
                IOrderedQueryable<Transaction> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(temTrans.Date):
                        sortedList = queriedList.OrderBy(a => a.Date, isAsc);
                        break;
                    case nameof(temTrans.Type):
                        sortedList = queriedList.OrderBy(a => a.Type, isAsc);
                        break;
                    case nameof(temTrans.Amount):
                        sortedList = queriedList.OrderBy(a => a.Amount, isAsc);
                        break;
                    case nameof(temTrans.Note):
                        sortedList = queriedList.OrderBy(a => a.Note, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                var response = new DataTableResponseModel<TableLiabilitiesDetailModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Transaction, TableLiabilitiesDetailModel>)
                    .ToList();
                response.total = queriedList.Count();
                response.display = queriedList.Count();
                trans = GetCustomerTransactionDetail(customerId, companyId, year);
                return response;
            }
        }

        public DetailCustomerLiabilitiesModel GetCustomerLiabilitiesDetail(
            string customerId
            , int companyId)
        {
            using (var dbContext = new HiEISEntities())
            {
                var company = dbContext.Companies.Find(companyId);
                if (!company.CompanyCustomers.Any(a => a.CustomerId == customerId))
                {
                    throw new Exceptions.CustomerNotFoundException();
                }
                var customerInDb = dbContext.Customers.Find(customerId);
                var customer = new DetailCustomerLiabilitiesModel();
                customer = Mapper.Map<Customer, DetailCustomerLiabilitiesModel>(customerInDb);
                return customer;
            }
        }

        public DetailCompanyLiabilitiesModel GetCompanyLiabilitiesDetail(
            string customerId
            , int companyId)
        {
            using (var dbContext = new HiEISEntities())
            {
                var companyInDb = dbContext.Companies.Find(companyId);
                if (!companyInDb.CompanyCustomers.Any(a => a.CustomerId == customerId))
                {
                    throw new Exceptions.CustomerNotFoundException();
                }
                var company = new DetailCompanyLiabilitiesModel();
                company = Mapper.Map<Company, DetailCompanyLiabilitiesModel>(companyInDb);
                return company;
            }
        }

        public DetailCustomerTransactionModel GetCustomerTransactionDetail(string customerId, int companyId, int year)
        {
            using (var dbContext = new HiEISEntities())
            {
                var lastyear = year - 1;
                var transaction = dbContext.Transactions.Where(a => a.CustomerId == customerId && a.CompanyId == companyId);
                var response = new DetailCustomerTransactionModel();
                response.Current = transaction.Where(b => b.Type == (int)TransactionType.Liability
                                                && b.CompanyId == companyId && b.Date.Year == year)
                                                .Sum(b => (decimal?)b.Amount) ?? 0;
                response.Last = transaction.Where(b => b.Type == (int)TransactionType.Liability
                                                && b.CompanyId == companyId && b.Date.Year == lastyear)
                                                .Sum(b => (decimal?)b.Amount) ?? 0;
                response.Payment = transaction.Where(b => b.Type == (int)TransactionType.Payment
                                                && b.CompanyId == companyId && b.Date.Year == year)
                                                .Sum(b => (decimal?)b.Amount) ?? 0;
                response.LastPayment = transaction.Where(b => b.Type == (int)TransactionType.Payment
                                                && b.CompanyId == companyId && b.Date.Year == lastyear)
                                                .Sum(b => (decimal?)b.Amount) ?? 0;
                return response;
            }
        }

        public List<int> GetYearTransaction(string customerId, int companyId)
        {
            using (var dbContext = new HiEISEntities())
            {
                var year = dbContext.Transactions.Where(a => a.CustomerId == customerId && a.CompanyId == companyId)
                                                     .Select(a => a.Date.Year).Distinct().OrderByDescending(a => a)
                                                     .ToList();

                return year;


            }
        }

        public List<int> GetYearTransactionByCompanyId(string customerId, int companyId)
        {
            using (var dbContext = new HiEISEntities())
            {
                var year = dbContext.Transactions.Where(a => a.CustomerId == customerId && a.CompanyId == companyId)
                                                     .Select(a => a.Date.Year).Distinct().OrderByDescending(a => a)
                                                     .ToList();

                return year;


            }
        }
    }
}
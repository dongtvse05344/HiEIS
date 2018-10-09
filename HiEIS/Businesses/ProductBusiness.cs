using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiEIS.Areas.Public.Models;
using HiEIS.Entities;
using AutoMapper;
using System.Web.Http;
using System.Net;
using HiEIS.Models;
using HiEIS.Utils;

namespace HiEIS.Businesses
{
    public class ProductBusiness
    {
        public List<UpdateProductModel> GetAllProducts(int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var products = db.Products
                    .Where(p => p.CompanyId == companyId && p.IsActive == true)
                    .Select(Mapper.Map<Product, UpdateProductModel>)
                    .OrderBy(p => p.Name)
                    .ToList();
                return products;
            }
        }

        public DataTableResponseModel<UpdateProductModel> GetProducts(
                                                DataTableRequestModel model
                                                , int companyId
                                                , string productName
                                                , string productCode
                                                , decimal? minPrice
                                                , decimal? maxPrice)
        {
            using (var db = new HiEISEntities())
            {
                productName = (productName ?? "").ToLower();
                productCode = (productCode ?? "").ToLower();
                var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
                var isNullName = string.IsNullOrWhiteSpace(productName);
                var isNullCode = string.IsNullOrWhiteSpace(productCode);
                var hasValueMin = minPrice.HasValue;
                var hasValueMax = maxPrice.HasValue;
                using (var dbContext = new HiEISEntities())
                {
                    var products = dbContext.Products.Where(a => a.CompanyId == companyId);
                    var queriedList = products
                            .Where(a =>
                                       (isNull || (a.Name.Contains(model.searchPhase)))
                                    && (isNullName || a.Name.ToLower().Contains(productName))
                                    && (isNullCode || a.Code.ToLower().Contains(productCode))
                                    && (!hasValueMin || (a.UnitPrice >= minPrice))
                                    && (!hasValueMax || a.UnitPrice <= maxPrice)
                                  );
                    IOrderedQueryable<Product> sortedList;
                    var isAsc = model.orderDir == "asc";
                    switch (model.orderCol)
                    {
                        case nameof(UpdateProductModel.Name):
                            sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                            break;
                        case nameof(UpdateProductModel.Code):
                            sortedList = queriedList.OrderBy(a => a.Code, isAsc);
                            break;
                        case nameof(UpdateProductModel.Unit):
                            sortedList = queriedList.OrderBy(a => a.Unit, isAsc);
                            break;
                        case nameof(UpdateProductModel.SUnitPrice):
                            sortedList = queriedList.OrderBy(a => a.UnitPrice, isAsc);
                            break;
                        case nameof(UpdateProductModel.VATRate):
                            sortedList = queriedList.OrderBy(a => a.VATRate, isAsc);
                            break;
                        default:
                            sortedList = queriedList.OrderBy(a => a.Id, isAsc);
                            break;
                    }
                    var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                    var response = new DataTableResponseModel<UpdateProductModel>();
                    response.data = pagingList
                        .Select(Mapper.Map<Product, UpdateProductModel>)
                        .ToList();
                    response.total = products.Count();
                    response.display = queriedList.Count();
                    return response;
                }
            }
        }

        public UpdateProductModel GetProductById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var productInDb = db.Products.Find(id);
                var product = Mapper.Map<Product, UpdateProductModel>(productInDb);
                return product;
            }

        }

        public int GetCompanyId(int productId)
        {
            using (var db = new HiEISEntities())
            {
                var productInDb = db.Products.Find(productId);
                return productInDb.CompanyId;
            }
        }

        public bool AddNewProduct(CreateProductModel model, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                try
                {
                    model.CompanyId = companyId;
                    model.IsActive = true;

                    var product = Mapper.Map<CreateProductModel, Product>(model);
                    db.Products.Add(product);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateProduct(UpdateProductModel model, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var productInDb = db.Products.Find(model.Id);
                    if (productInDb != null && productInDb.CompanyId == companyId && productInDb.IsActive)
                    {
                        Mapper.Map(model, productInDb);
                        db.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }

        public void DeleteProduct(int id)
        {
            using (var db = new HiEISEntities())
            {
                var productInDb = db.Products.Find(id);
                if (productInDb == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                productInDb.IsActive = false;
                db.SaveChanges();
            }
        }

        public DataTableResponseModel<CustomerProductModel> GetPoductByCustomerId(
              DataTableRequestModel model, int companyId, string customerId)
        {
            using (var dbContext = new HiEISEntities())
            {
                var temProduct = new CustomerProductModel();
                var queriedList = (from p in dbContext.Products
                                   join cp in dbContext.CustomerProducts on p.Id equals cp.ProductId
                                   join c in dbContext.Customers on cp.CustomerId equals c.Id
                                   where p.CompanyId == companyId && c.Id == customerId
                                   select new CustomerProductModel
                                   {
                                       CustomerId = c.Id,
                                       CustomerName = c.Name,
                                       ProductId = p.Id,
                                       ProductName = p.Name,
                                       ProductUnit = p.Unit,
                                       DUnitPrice = p.UnitPrice,
                                       ProductAmount = cp.Amount,
                                       ProductVATRate = p.VATRate,
                                       HasIndex = p.HasIndex
                                   });

                //var sortedList = queriedList.OrderBy(a => a.ProductName);
                IOrderedQueryable<CustomerProductModel> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(temProduct.ProductUnit):
                        sortedList = queriedList.OrderBy(a => a.ProductUnit, isAsc);
                        break;
                    case nameof(temProduct.ProductAmount):
                        sortedList = queriedList.OrderBy(a => a.ProductAmount, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.ProductName, isAsc);
                        break;
                }
                var response = new DataTableResponseModel<CustomerProductModel>();
                if (model.pageSize == -1)
                {
                    response.data = sortedList.ToList();
                }
                else
                {
                    var pagingList = sortedList.Skip(model.page * model.pageSize).Take(model.pageSize);
                    response.data = pagingList.ToList();
                }
                response.total = queriedList.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        public string GetOldNumber(int productId, string customerId)
        {
            string number = "";
            using (var db = new HiEISEntities())
            {
                //var proformaList = from z in db.ProformaInvoices where z.CustomerId == customerId select ;

                //var item = from i in db.ProformaInvoiceItems
                //           join p in proformaList on p.Id equals i.
                int status = (int)Utils.HiEISUtil.ProformaInvoiceStatus.Approved;
                var item = (from i in db.ProformaInvoiceItems
                            join p in (from z in db.ProformaInvoices where z.CustomerId == customerId && z.Status == status select z)
                            on i.ProformaInvoiceId equals p.Id
                            where i.ProductId == productId
                            orderby i.ProformaInvoiceId descending
                            select i).FirstOrDefault();
                if (item != null)
                {
                    number = item.NewNumber.ToString();
                }

            }
            return number;
        }
    }
}
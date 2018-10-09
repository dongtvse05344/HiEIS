using HiEIS.Entities;
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HiEIS.Utils;
using AutoMapper;
using HiEIS.Areas.Public.Models;
using System.Web.Mvc;
using System.IO;
using IronPdf;

namespace HiEIS.Businesses
{
    public class ProformaInvoiceBusiness
    {
        public object ViewData { get; private set; }

        public DataTableResponseModel<TableProformaInvoiceModel> ListByCustomerId(DataTableRequestModel model
                                                                                    , string compName
                                                                                    , string lookupCode)
        {
            compName = (compName ?? "").ToLower();
            lookupCode = (lookupCode ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullCompName = string.IsNullOrWhiteSpace(compName);
            var isNullLookupCode = string.IsNullOrWhiteSpace(lookupCode);
            using (var dbContext = new HiEISEntities())
            {
                var customerProformaInvoice = dbContext.ProformaInvoices
                      .Where(a => a.CustomerId == model.searchPhase && a.Status == (int)HiEISUtil.ProformaInvoiceStatus.Approved);
                var queriedList = customerProformaInvoice
                      .Where(a => (isNull
                        || (a.CustomerId == model.searchPhase))
                         && ((isNullCompName || a.Staff.Company.Name.ToLower().Contains(compName))
                         && (isNullLookupCode || a.LookupCode.ToLower().Contains(lookupCode)))
                       );
                IOrderedQueryable<ProformaInvoice> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(TableProformaInvoiceModel.StaffCompanyName):
                        sortedList = queriedList.OrderBy(a => a.Staff.Company.Name, isAsc);
                        break;
                    case nameof(TableProformaInvoiceModel.LookupCode):
                        sortedList = queriedList.OrderBy(a => a.LookupCode, isAsc);
                        break;
                    case nameof(TableProformaInvoiceModel.PaymentDeadline):
                        sortedList = queriedList.OrderBy(a => a.PaymentDeadline, isAsc);
                        break;
                    case nameof(TableProformaInvoiceModel.Total):
                        sortedList = queriedList.OrderBy(a => a.Total, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Date, !isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<TableProformaInvoiceModel>();
                response.data = pagingList
                    .Select(Mapper.Map<ProformaInvoice, TableProformaInvoiceModel>)
                    .ToList();
                response.total = customerProformaInvoice.Count();
                response.display = queriedList.Count();
                return response;
            }
        }
        public bool DeleteProformaInvoice(int id, int companyId)
        {
            bool result = false;
            using (var dbcontext = new HiEISEntities())
            {


                try
                {
                    var proformaInDB = dbcontext.ProformaInvoices.Find(id);
                    if (proformaInDB != null
                        && proformaInDB.Staff.CompanyId == companyId
                        && proformaInDB.Status == (int)HiEISUtil.ProformaInvoiceStatus.New)
                    {
                        string fileUrl = HttpContext.Current.Server.MapPath("~" + proformaInDB.FileUrl);
                        var itemsInDB = dbcontext.ProformaInvoiceItems.Where(i => i.ProformaInvoiceId == id);
                        dbcontext.ProformaInvoiceItems.RemoveRange(itemsInDB);
                        dbcontext.ProformaInvoices.Remove(proformaInDB);
                        if (System.IO.File.Exists(fileUrl))
                        {
                            System.IO.File.Delete(fileUrl);
                        }

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

        //public List<ProformaInvoiceListViewModel> GetAllProformaInvoices(int companyId)
        //{
        //    using (var db = new HiEISEntities())
        //    {
        //        List<ProformaInvoiceListViewModel> proformaInvoice = new List<ProformaInvoiceListViewModel>();
        //        var company = db.Companies.Find(companyId);
        //        if (company != null)
        //        {
        //            proformaInvoice = company.Staffs.SelectMany(a => a.ProformaInvoices)
        //                .OrderByDescending(a => a.Date)
        //                .Select(Mapper.Map<ProformaInvoice, ProformaInvoiceListViewModel>)
        //                .ToList();
        //        }

        //        return proformaInvoice;
        //    }
        //}

        public DataTableResponseModel<ProformaInvoiceListViewModel> GetAllProformaInvoices(
                                                DataTableRequestModel model
                                                , int companyId
                                                , string customerName
                                                , string lookupCode
                                                , DateTime? minDate
                                                , DateTime? maxDate
                                                , int? status)
        {
            using (var db = new HiEISEntities())
            {
                customerName = (customerName ?? "").ToLower();
                lookupCode = (lookupCode ?? "").ToLower();
                var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
                var isNullName = string.IsNullOrWhiteSpace(customerName);
                var isNullCode = string.IsNullOrWhiteSpace(lookupCode);
                var hasValueStatus = status.HasValue;

                using (var dbContext = new HiEISEntities())
                {
                    var proformaInvoices = dbContext.ProformaInvoices.Where(a => a.Staff.CompanyId == companyId);
                    var queriedList = proformaInvoices
                            .Where(a =>
                                       (isNull || (a.Customer.Name.Contains(model.searchPhase)))
                                    && (isNullName || a.Customer.Name.ToLower().Contains(customerName))
                                    && (isNullCode || a.LookupCode.ToLower().Contains(lookupCode))
                                    && (!hasValueStatus || (a.Status == status))
                                    && (!minDate.HasValue || a.Date >= minDate.Value)
                                    && (!maxDate.HasValue || a.Date <= maxDate.Value)
                                  );
                    IOrderedQueryable<ProformaInvoice> sortedList;
                    var isAsc = model.orderDir == "asc";
                    switch (model.orderCol)
                    {
                        case nameof(ProformaInvoiceListViewModel.CustomerName):
                            sortedList = queriedList.OrderBy(a => a.Customer.Name, isAsc);
                            break;
                        case nameof(ProformaInvoiceListViewModel.LookupCode):
                            sortedList = queriedList.OrderBy(a => a.LookupCode, isAsc);
                            break;
                        case nameof(ProformaInvoiceListViewModel.Date):
                            sortedList = queriedList.OrderBy(a => a.Date, isAsc);
                            break;
                        case nameof(ProformaInvoiceListViewModel.PaymentDeadline):
                            sortedList = queriedList.OrderBy(a => a.PaymentDeadline, isAsc);
                            break;
                        case nameof(ProformaInvoiceListViewModel.Status):
                            sortedList = queriedList.OrderBy(a => a.Status, isAsc);
                            break;
                        case nameof(ProformaInvoiceListViewModel.Total):
                            sortedList = queriedList.OrderBy(a => a.Total, isAsc);
                            break;
                        default:
                            sortedList = queriedList.OrderBy(a => a.Date, !isAsc);
                            break;
                    }
                    var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                    var response = new DataTableResponseModel<ProformaInvoiceListViewModel>();
                    response.data = pagingList
                        .Select(Mapper.Map<ProformaInvoice, ProformaInvoiceListViewModel>)
                        .ToList();
                    response.total = proformaInvoices.Count();
                    response.display = queriedList.Count();
                    return response;
                }
            }
        }

        public bool CreateProformaInvoice(CreateProformaViewModel model, string staffId, ControllerContext controllerContext)
        {
            bool result = false;
            var invoiceBusiness = new InvoiceBusiness();
            var staffBusiness = new StaffBusiness();
            using (var db = new HiEISEntities())
            {
                try
                {
                    string lookupCode = invoiceBusiness.GenerateLookupCode();
                    string fileName = "/Files/ProformaInvoices/"
                        + DateTime.Now.ToString("yyyyMMdd") + "_"
                        + lookupCode
                        + ".pdf";
                    model.LookupCode = lookupCode;
                    model.StaffId = staffId;
                    model.Date = DateTime.Now;
                    model.Status = (int)HiEISUtil.ProformaInvoiceStatus.New;
                    model.PaymentDeadline = DateTime.ParseExact(model.Deadline, "dd/MM/yyyy", null);
                    var proforma = Mapper.Map<CreateProformaViewModel, ProformaInvoice>(model);
                    var pdfModel = Mapper.Map<CreateProformaViewModel, ProformaViewPDF>(model);
                    var staff = db.Staffs.Find(staffId);
                    if (staff != null)
                    {
                        pdfModel.StaffName = staff.Name;
                        pdfModel.StaffCompanyName = staff.Company.Name;
                        pdfModel.StaffCompanyTel = staff.Company.Tel;
                        pdfModel.StaffCompanyBank = staff.Company.Bank;
                        pdfModel.StaffCompanyBankAccountNumber = staff.Company.BankAccountNumber;
                    }
                    proforma.FileUrl = GenerateProformaPDF(fileName, pdfModel, controllerContext);

                    db.ProformaInvoices.Add(proforma);
                    db.SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public UpdateProformaViewModel GetProformaById(int id, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var proformaInDB = db.ProformaInvoices.Find(id);
                var proforma = new UpdateProformaViewModel();
                if (proformaInDB != null
                    && proformaInDB.Staff.CompanyId == companyId
                    && proformaInDB.Status == (int)HiEISUtil.ProformaInvoiceStatus.New)
                {
                    proforma = Mapper.Map<ProformaInvoice, UpdateProformaViewModel>(proformaInDB);
                    if (proformaInDB.CustomerId != null)
                    {
                        proforma.CustomerName = proformaInDB.Customer.Name;
                        proforma.CustomerEnterprise = proformaInDB.Customer.Enterprise;
                        proforma.CustomerAddress = proformaInDB.Customer.Address;
                        proforma.CustomerTel = proformaInDB.Customer.Tel;
                    }

                    for (int i = 0; i < proformaInDB.ProformaInvoiceItems.Count(); i++)
                    {
                        proforma.ProformaInvoiceItems.ElementAt(i).HasIndex = proformaInDB.ProformaInvoiceItems.ElementAt(i).Product.HasIndex;
                    }


                }
                else
                {
                    proforma = null;
                }
                return proforma;
            }
        }
        public string GetFileUrl(GetFileUrlProformaInvoiceModel model)
        {
            using (var dbContext = new HiEISEntities())
            {
                if (dbContext.ProformaInvoices.Any(a => a.LookupCode == model.LookupCode))
                {
                    var proformaInvoice = dbContext.ProformaInvoices
                      .FirstOrDefault(a => a.LookupCode == model.LookupCode);
                    return proformaInvoice.FileUrl;
                }
                throw new Exceptions.InvoiceNotFoundException();
            }
        }

        public bool UpdateProforma(UpdateProformaViewModel model, int id, int companyId, ControllerContext controllerContext)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var proformaInDb = db.ProformaInvoices.Find(id);
                    if (proformaInDb != null
                       && proformaInDb.Staff.CompanyId == companyId
                       && proformaInDb.Status == (int)HiEISUtil.ProformaInvoiceStatus.New)
                    {
                        model.Date = DateTime.Now;
                        model.PaymentDeadline = DateTime.ParseExact(model.Deadline, "dd/MM/yyyy", null);
                        model.Status = (int)HiEISUtil.ProformaInvoiceStatus.New;

                        var pdfModel = Mapper.Map<UpdateProformaViewModel, ProformaViewPDF>(model);
                        pdfModel.StaffCompanyName = proformaInDb.Staff.Company.Name;
                        pdfModel.StaffCompanyTel = proformaInDb.Staff.Company.Tel;
                        pdfModel.StaffCompanyBank = proformaInDb.Staff.Company.Bank;
                        pdfModel.StaffCompanyBankAccountNumber = proformaInDb.Staff.Company.BankAccountNumber;
                        pdfModel.CustomerEnterprise = proformaInDb.Customer.Enterprise;

                        model.FileUrl = GenerateProformaPDF(proformaInDb.FileUrl, pdfModel, controllerContext);

                        Mapper.Map(model, proformaInDb);
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

        public ProformaViewPDF ConfirmProforma(int id, int companyId, string staffId, ControllerContext controllerContext)
        {
            using (var db = new HiEISEntities())
            {
                try
                {
                    var proformaInDb = db.ProformaInvoices.Find(id);
                    if (proformaInDb != null
                        && proformaInDb.Staff.CompanyId == companyId
                        && proformaInDb.Status == (int)HiEISUtil.ProformaInvoiceStatus.New)
                    {
                        proformaInDb.Status = (int)HiEISUtil.ProformaInvoiceStatus.Approved;
                        proformaInDb.Date = DateTime.Now;
                        proformaInDb.StaffId = staffId;
                        db.SaveChanges();

                        var pdfModel = Mapper.Map<ProformaInvoice, ProformaViewPDF>(proformaInDb);
                        pdfModel.CustomerEmail = proformaInDb.Customer.AspNetUser.Email;
                        proformaInDb.FileUrl = GenerateProformaPDF(proformaInDb.FileUrl, pdfModel, controllerContext);

                        return pdfModel;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }

            }

        }

        public string GenerateProformaPDF(string fileName, ProformaViewPDF model, ControllerContext controllerContext)
        {
            var emailBusiness = new EmailBusiness();
            var viewData = new ViewDataDictionary();
            viewData.Model = model;
            string html = emailBusiness.ToHtml("ProformaInvoicePDF", viewData, controllerContext);
            var Renderer = new IronPdf.HtmlToPdf();
            Renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            Renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
            Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            Renderer.PrintOptions.PrintHtmlBackgrounds = true;
            Renderer.PrintOptions.MarginTop = 10;
            Renderer.PrintOptions.MarginBottom = 10;
            Renderer.PrintOptions.MarginLeft = 10;
            Renderer.PrintOptions.MarginRight = 10;
            var PDF = Renderer.RenderHtmlAsPdf(html);
            string fileUrl = HttpContext.Current.Server.MapPath("~" + fileName);
            PDF.SaveAs(fileUrl);
            return fileName;
        }

        public UpdateInvoiceViewModel ConvertToInvoice(int proformaId, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var proformaInDb = db.ProformaInvoices.Find(proformaId);
                var invoice = new UpdateInvoiceViewModel();

                if (proformaInDb != null && proformaInDb.Status == (int)HiEISUtil.ProformaInvoiceStatus.Approved)
                {
                    invoice.Address = proformaInDb.Customer.Address;
                    invoice.Bank = proformaInDb.Customer.Bank;
                    invoice.BankAccountNumber = proformaInDb.Customer.BankAccountNumber;
                    invoice.CustomerId = proformaInDb.CustomerId;
                    invoice.DueDate = proformaInDb.PaymentDeadline;
                    invoice.Email = proformaInDb.Customer.AspNetUser.Email;
                    invoice.Enterprise = proformaInDb.Customer.Enterprise;
                    invoice.Fax = proformaInDb.Customer.Fax;
                    invoice.Name = proformaInDb.Customer.Name;
                    invoice.Note = "";
                    invoice.PaymentMethod = (int)HiEISUtil.PaymentMethod.Cash;
                    invoice.SubTotal = proformaInDb.SubTotal;
                    invoice.TaxNo = proformaInDb.Customer.TaxNo;
                    invoice.Tel = proformaInDb.Customer.Tel;
                    invoice.Total = proformaInDb.TotalNoLiabilities;
                    invoice.VATAmount = proformaInDb.VATAmount; // = 0
                    invoice.VATRate = 0;

                    invoice.InvoiceItems = new List<InvoiceItemViewModel>();
                    foreach (var item in proformaInDb.ProformaInvoiceItems)
                    {
                        var invoiceItem = new InvoiceItemViewModel();
                        invoiceItem.ProductId = item.ProductId;
                        invoiceItem.ProductName = item.Product.Name;
                        invoiceItem.ProductUnit = item.Product.Unit;
                        invoiceItem.Quantity = item.Quantity;
                        invoiceItem.UnitPrice = item.UnitPrice;
                        invoiceItem.VATRate = item.VATRate;

                        invoice.InvoiceItems.Add(invoiceItem);
                    }
                }
                else
                {
                    invoice = null;
                }

                return invoice;
            }
        }

        public string GetProformaEmail(int id)
        {
            string email = "";
            using (var db = new HiEISEntities())
            {
                try
                {
                    var proforma = db.ProformaInvoices.Find(id);
                    if (proforma != null)
                    {
                        email = proforma.Customer.AspNetUser.Email;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return email;
        }

        public ProformaViewPDF GetPDFModelById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var proformaInDb = db.ProformaInvoices.Find(id);
                var pdfModel = new ProformaViewPDF();
                if (proformaInDb != null)
                {
                    pdfModel = Mapper.Map<ProformaInvoice, ProformaViewPDF>(proformaInDb);
                } else
                {
                    pdfModel = null;
                }
                return pdfModel;
            }
        }
    }
}
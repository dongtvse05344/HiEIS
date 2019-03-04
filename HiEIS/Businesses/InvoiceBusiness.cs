using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HiEIS.Areas.Public.Models;
using HiEIS.Entities;
using AutoMapper;
using HiEIS.Utils;
using System.Text;
using System.IO;
using System.Xml;
using iTextSharp.text.pdf;
using HiEIS.Models;
using System.Data.Entity;
using iTextSharp.text;

namespace HiEIS.Businesses
{
    public class InvoiceBusiness
    {
        public string GetPdfFileUrl(LookupModel lookupModel)
        {
            using (var dbContext = new HiEISEntities())
            {
                if (dbContext.Invoices.Any(a => a.LookupCode == lookupModel.InvoiceLookupCode))
                {
                    var invoice = dbContext.Invoices
                      .FirstOrDefault(a => a.LookupCode == lookupModel.InvoiceLookupCode);
                    return invoice.FileUrl;
                }
                throw new Exceptions.InvoiceNotFoundException();
            }
        }

        public DataTableResponseModel<TableInvoiceModel> ListByCustomerId(
                                                                DataTableRequestModel model
                                                                , string compName
                                                                , string lookupCode
                                                                , string paymentStatus
                                                                , DateTime? min
                                                                , DateTime? max)
        {
            compName = (compName ?? "").ToLower();
            lookupCode = (lookupCode ?? "").ToLower();
            var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
            var isNullCompName = string.IsNullOrWhiteSpace(compName);
            var isNullLookupCode = string.IsNullOrWhiteSpace(lookupCode);
            var isNullPayment = string.IsNullOrWhiteSpace(paymentStatus);
            bool payment = false;
            if (!isNullPayment && paymentStatus == "1")
            {
                payment = true;
            }

            using (var dbContext = new HiEISEntities())
            {
                var customerInvoice = dbContext.Invoices
                      .Where(a => a.CustomerId == model.searchPhase);
                var queriedList = customerInvoice
                      .Where(a => (isNull
                        || (a.CustomerId == model.searchPhase))
                       && ((isNullCompName || a.Staff.Company.Name.ToLower().Contains(compName))
                       && (isNullLookupCode || a.LookupCode.ToLower().Contains(lookupCode))
                       && (isNullPayment || a.PaymentStatus == payment))
                       && (!min.HasValue || a.Date >= min.Value)
                       && (!max.HasValue || a.Date <= max.Value)
                        );
                IOrderedQueryable<Invoice> sortedList;
                var isAsc = model.orderDir == "asc";
                switch (model.orderCol)
                {
                    case nameof(TableInvoiceModel.StaffCompanyName):
                        sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                        break;
                    case nameof(TableInvoiceModel.LookupCode):
                        sortedList = queriedList.OrderBy(a => a.LookupCode, isAsc);
                        break;
                    case nameof(TableInvoiceModel.TemplateForm):
                        sortedList = queriedList.OrderBy(a => a.Template.Form, isAsc);
                        break;
                    case nameof(TableInvoiceModel.Number):
                        sortedList = queriedList.OrderBy(a => a.Number, isAsc);
                        break;
                    case nameof(TableInvoiceModel.PaymentStatus):
                        sortedList = queriedList.OrderBy(a => a.PaymentStatus, isAsc);
                        break;
                    case nameof(TableInvoiceModel.Total):
                        sortedList = queriedList.OrderBy(a => a.Total, isAsc);
                        break;
                    case nameof(TableInvoiceModel.Date):
                        sortedList = queriedList.OrderBy(a => a.Date, isAsc);
                        break;
                    case nameof(TableInvoiceModel.DueDate):
                        sortedList = queriedList.OrderBy(a => a.DueDate, isAsc);
                        break;
                    case nameof(TableInvoiceModel.Status):
                        sortedList = queriedList.OrderBy(a => a.Status, isAsc);
                        break;
                    default:
                        sortedList = queriedList.OrderBy(a => a.Date, !isAsc);
                        break;
                }
                var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                var response = new DataTableResponseModel<TableInvoiceModel>();
                response.data = pagingList
                    .Select(Mapper.Map<Invoice, TableInvoiceModel>)
                    .ToList();
                response.total = customerInvoice.Count();
                response.display = queriedList.Count();
                return response;
            }
        }

        //public List<InvoiceListViewModel> GetAllInvoices(int companyId)
        //{
        //    using (var db = new HiEISEntities())
        //    {
        //        List<InvoiceListViewModel> invoices = new List<InvoiceListViewModel>();
        //        var company = db.Companies.Find(companyId);

        //        if (company != null)
        //        {
        //            invoices = company.Templates.SelectMany(a => a.Invoices)
        //                .Select(Mapper.Map<Invoice, InvoiceListViewModel>)
        //                .ToList();
        //        }

        //        return invoices;
        //    }
        //}


        public DataTableResponseModel<InvoiceListViewModel> GetAllInvoices(
                                                DataTableRequestModel model
                                                , int companyId
                                                , string customerName
                                                , string lookupCode
                                                , DateTime? minDate
                                                , DateTime? maxDate
                                                , bool? paymentStatus
                                                , int? status)
        {
            using (var db = new HiEISEntities())
            {
                customerName = (customerName ?? "").ToLower();
                lookupCode = (lookupCode ?? "").ToLower();
                var isNull = string.IsNullOrWhiteSpace(model.searchPhase);
                var isNullName = string.IsNullOrWhiteSpace(customerName);
                var isNullCode = string.IsNullOrWhiteSpace(lookupCode);
                //var hasValueMin = minPrice.HasValue;
                //var hasValueMax = maxPrice.HasValue;
                var hasValuePayment = paymentStatus.HasValue;
                var hasValueStatus = status.HasValue;

                using (var dbContext = new HiEISEntities())
                {
                    var invoices = dbContext.Invoices.Where(a => a.Staff.CompanyId == companyId);
                    var queriedList = invoices
                            .Where(a =>
                                       (isNull || (a.Name.Contains(model.searchPhase)))
                                    && (isNullName || a.Name.ToLower().Contains(customerName))
                                    && (isNullCode || a.LookupCode.ToLower().Contains(lookupCode))
                                    && (!hasValuePayment || (a.PaymentStatus == paymentStatus))
                                    && (!hasValueStatus || (a.Status == status))
                                    && (!minDate.HasValue || a.Date >= minDate.Value)
                                    && (!maxDate.HasValue || a.Date <= maxDate.Value)
                                  );
                    IOrderedQueryable<Invoice> sortedList;
                    var isAsc = model.orderDir == "asc";
                    switch (model.orderCol)
                    {
                        case nameof(InvoiceListViewModel.Date):
                            sortedList = queriedList.OrderBy(a => a.Date, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.Name):
                            sortedList = queriedList.OrderBy(a => a.Name, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.LookupCode):
                            sortedList = queriedList.OrderBy(a => a.LookupCode, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.TemplateForm):
                            sortedList = queriedList.OrderBy(a => a.Template.Form, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.Number):
                            sortedList = queriedList.OrderBy(a => a.Number, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.PaymentStatus):
                            sortedList = queriedList.OrderBy(a => a.PaymentStatus, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.DueDate):
                            sortedList = queriedList.OrderBy(a => a.DueDate, isAsc);
                            break;
                        case nameof(InvoiceListViewModel.Status):
                            sortedList = queriedList.OrderBy(a => a.Status, isAsc);
                            break;
                        default:
                            sortedList = queriedList.OrderBy(a => a.Date, !isAsc);
                            break;
                    }
                    var pagingList = sortedList.Skip(model.page).Take(model.pageSize);
                    var response = new DataTableResponseModel<InvoiceListViewModel>();
                    response.data = pagingList
                        .Select(Mapper.Map<Invoice, InvoiceListViewModel>)
                        .ToList();
                    response.total = invoices.Count();
                    response.display = queriedList.Count();
                    return response;
                }
            }
        }

        public int GetCompanyId(int invoiceId)
        {
            using (var db = new HiEISEntities())
            {
                var invoiceInDb = db.Invoices.Find(invoiceId);
                //int companyId = db.Templates.FirstOrDefault(t => t.Id == invoiceInDb.TemplateId).CompanyId;
                int companyId = invoiceInDb.Template.CompanyId;
                return companyId;
            }
        }

        public UpdateInvoiceViewModel GetNewInvoiceById(int id, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var invoiceInDb = db.Invoices.Find(id);
                var invoice = new UpdateInvoiceViewModel();

                if (invoiceInDb != null && invoiceInDb.Template.CompanyId == companyId && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.New)

                {
                    if (invoiceInDb.Template.IsActive == false)
                    {
                        invoiceInDb.TemplateId = GetNextTemplateBlock(invoiceInDb.Template, companyId, db).Id;
                        db.SaveChanges();
                    }

                    invoice = Mapper.Map<Invoice, UpdateInvoiceViewModel>(invoiceInDb);
                }
                else
                {
                    invoice = null;
                }

                return invoice;
            }
        }

        public Invoice GetInvoiceById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var invoiceInDb = db.Invoices.Find(id);

                if (invoiceInDb != null)

                {
                    return invoiceInDb;
                }
                else
                {
                    return null;
                }
            }
        }

        

        public bool CreateInvoice(CreateInvoiceViewModel model, string staffId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    string lookupCode = GenerateLookupCode();
                    string fileName = "/Files/Invoices/"
                        + DateTime.Now.ToString("yyyyMMdd") + "_"
                        + lookupCode
                        + ".pdf";

                    string fileName2 = "/Files/Invoices/"
                        + DateTime.Now.ToString("yyyyMMdd") + "_2"
                        + lookupCode
                        + ".pdf";

                    string fileName3 = "/Files/Invoices/"
                        + DateTime.Now.ToString("yyyyMMdd") + "_3"
                        + lookupCode
                        + ".pdf";

                    model.LookupCode = lookupCode;
                    model.Type = 2;
                    model.Date = DateTime.Now;
                    model.PaymentStatus = false;
                    model.Status = (int)HiEISUtil.InvoiceStatus.New;
                    model.StaffId = staffId;

                    var invoice = Mapper.Map<CreateInvoiceViewModel, Invoice>(model);
                    var pdfModel = Mapper.Map<CreateInvoiceViewModel, PdfViewModel>(model);
                    invoice.FileUrl = GenerateFinalPdf(fileName, pdfModel,1);
                    invoice.FileUrl2 = GenerateFinalPdf(fileName2, pdfModel, 2);
                    invoice.FileUrl3 = GenerateFinalPdf(fileName3, pdfModel, 3);

                    db.Invoices.Add(invoice);
                    db.SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    //TODO: result = false;
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Generate a PDF file 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="model">PDF model</param>
        /// <returns>File virtual path</returns>
        private string GenerateFinalPdf(string fileName, PdfViewModel model, int type)
        {
            var pages = Math.Ceiling(model.InvoiceItems.Count / 10d);
            var pdf = new PdfViewModel();
            List<string> generatedFiles = new List<string>();

            Mapper.Map(model, pdf);
            fileName = fileName.Replace(".pdf", ""); //remove extension

            for (int i = 0; i < pages; i++)
            {
                bool isLastPage = (i == (pages - 1)) ? true : false;
                pdf.InvoiceItems = model.InvoiceItems
                    .Skip(10 * i)
                    .Take(10)
                    .ToList();
                string newFile = fileName + "_" + i + ".pdf";
                GeneratePdf(newFile, pdf, isLastPage, type);

                string newFileUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, newFile.RemoveFirstSlash());
                generatedFiles.Add(newFileUrl);
            }

            string finalFileUrl = MergePdfFiles(generatedFiles, fileName + ".pdf");
            return finalFileUrl;
        }

        private string MergePdfFiles(List<string> inputFiles, string outputFileName)
        {
            try
            {
                string fileUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFileName.RemoveFirstSlash());
                Document document = new Document();
                PdfCopy copy = new PdfCopy(document, new FileStream(fileUrl, FileMode.Create));
                PdfReader reader;

                document.Open();
                for (int i = 0; i < inputFiles.Count; i++)
                {
                    if (File.Exists(inputFiles[i]))
                    {
                        //create PdfReader object
                        reader = new PdfReader(inputFiles[i]);

                        //merge combine pages
                        for (int page = 1; page <= reader.NumberOfPages; page++)
                            copy.AddPage(copy.GetImportedPage(reader, page));

                    }
                }
                document.Close();

                foreach (var file in inputFiles)
                {
                    //delete the chosen file
                    File.Delete(file);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return outputFileName;
        }

        /// <summary>
        /// Update invoice
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public bool UpdateInvoice(InvoiceViewModel model, int id, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoiceInDb = db.Invoices.Find(id);

                    if (invoiceInDb != null
                        && invoiceInDb.Template.CompanyId == companyId
                        && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.New)
                    {
                        model.Date = DateTime.Now;

                        var pdfModel = Mapper.Map<InvoiceViewModel, PdfViewModel>(model);
                        // generate pdf file
                        var fileUrl = GenerateFinalPdf(invoiceInDb.FileUrl, pdfModel,1);
                        var fileUrl2 = GenerateFinalPdf(invoiceInDb.FileUrl2, pdfModel, 2);
                        var fileUrl3 = GenerateFinalPdf(invoiceInDb.FileUrl3, pdfModel, 3);

                        var newInvoice = Mapper.Map(model, invoiceInDb);
                        newInvoice.FileUrl = fileUrl;
                        newInvoice.FileUrl2 = fileUrl2;
                        newInvoice.FileUrl3 = fileUrl3;
                        // save changes
                        db.SaveChanges();

                        //JobScheduler.AddNumberInvoiceTask(model.DueDate, companyId, invoice.Id);

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

        public bool DeleteInvoice(int id, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoiceInDb = db.Invoices.Find(id);
                    if (invoiceInDb != null
                        && invoiceInDb.Template.CompanyId == companyId
                        && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.New)
                    {
                        string fileUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, invoiceInDb.FileUrl.RemoveFirstSlash());
                        var itemsInDb = db.InvoiceItems.Where(i => i.InvoiceId == id);

                        db.InvoiceItems.RemoveRange(itemsInDb);
                        db.Invoices.Remove(invoiceInDb);

                        db.SaveChanges();

                        //delete pdf
                        if (File.Exists(fileUrl))
                        {
                            File.Delete(fileUrl);
                        }

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

        public bool ConfirmPayment(int id, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    bool numberSuccess = NumberInvoice(id, db, companyId);
                    if (numberSuccess)
                    {
                        var invoiceInDb = db.Invoices.Find(id);
                        invoiceInDb.PaymentStatus = true;
                        invoiceInDb.Status = (int)HiEISUtil.InvoiceStatus.WaitingOnApproval;
                        invoiceInDb.Date = DateTime.Now;


                        var pdfModel = Mapper.Map<Invoice, PdfViewModel>(invoiceInDb);
                        invoiceInDb.FileUrl = GenerateFinalPdf(invoiceInDb.FileUrl, pdfModel,1);
                        invoiceInDb.FileUrl2 = GenerateFinalPdf(invoiceInDb.FileUrl2, pdfModel, 2);
                        invoiceInDb.FileUrl3 = GenerateFinalPdf(invoiceInDb.FileUrl3, pdfModel, 3);

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

        public bool NumberInvoice(int id, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                return this.NumberInvoice(id, db, companyId);
            }
        }

        private bool NumberInvoice(int id, HiEISEntities db, int companyId)
        {
            bool result = false;

            try
            {
                var invoiceInDb = db.Invoices.Find(id);

                if (invoiceInDb != null && invoiceInDb.Template.CompanyId == companyId)
                {
                    if (invoiceInDb.Template.IsActive == false)
                    {
                        invoiceInDb.TemplateId = GetNextTemplateBlock(invoiceInDb.Template, companyId, db).Id;
                        db.SaveChanges();
                    }

                    int currentNo = invoiceInDb.Template.CurrentNo;
                    string newNumber = (currentNo + 1).ToString().PadLeft(7, '0');

                    invoiceInDb.Number = newNumber;

                    //set currentNo mới cho template và check nếu template hết block
                    UpdateTemplateNumber(invoiceInDb.TemplateId, companyId, db);

                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public PdfViewModel SignInvoice(int id, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoiceInDb = db.Invoices.Find(id);

                    if (invoiceInDb != null
                        && invoiceInDb.Template.CompanyId == companyId
                        && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.WaitingOnApproval)
                    {
                        invoiceInDb.Status = (int)HiEISUtil.InvoiceStatus.Approved;
                        invoiceInDb.Date = DateTime.Now;
                        db.SaveChanges();

                        //Digital signature and assign FileUrl here
                        var pdfModel = Mapper.Map<Invoice, PdfViewModel>(invoiceInDb);
                        GenerateFinalPdf(invoiceInDb.FileUrl, pdfModel,1);
                        GenerateFinalPdf(invoiceInDb.FileUrl2, pdfModel, 2);
                        GenerateFinalPdf(invoiceInDb.FileUrl3, pdfModel, 3);

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
                }
            }
        }

        public string GenerateLookupCode()
        {
            int length = 13;
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        private string GeneratePdf(string fileName, PdfViewModel model, bool isLastPage, int type)
        {
            PdfReader pdfReader = null;
            string fileUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.RemoveFirstSlash());
            using (FileStream pdfOutputFile = new FileStream(fileUrl, FileMode.Create))
            {
                try
                {
                    //template
                    var templateBusiness = new TemplateBusiness();
                    var template = templateBusiness.GetTemplateById(model.TemplateId);
                    string path = "";
                    switch(type)
                    {
                        case 1: path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.FileUrl.RemoveFirstSlash()); break;
                        case 2: path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.FileUrl2.RemoveFirstSlash()); break;
                        case 3: path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template.FileUrl3.RemoveFirstSlash()); break;
                    }
                    PdfStamper pdfStamper = null;

                    pdfReader = new PdfReader(path);
                    pdfStamper = new PdfStamper(pdfReader, pdfOutputFile);


                    // Get the form fields
                    AcroFields testForm = pdfStamper.AcroFields;
                    testForm.SetField("Form", model.TemplateForm);
                    testForm.SetField("Serial", model.TemplateSerial);
                    testForm.SetField("Number", model.Number == null ? "" : model.Number);
                    testForm.SetField("Day", model.Date.Day.ToString());
                    testForm.SetField("Month", model.Date.Month.ToString());
                    testForm.SetField("Year", model.Date.Year.ToString());
                    testForm.SetField("Name", model.Name);
                    testForm.SetField("Enterprise", model.Enterprise);
                    testForm.SetField("Address", model.Address);
                    testForm.SetField("Tel", model.Tel);
                    testForm.SetField("Fax", model.Fax);
                    testForm.SetField("BankAccountNumber", model.BankAccountNumber);
                    testForm.SetField("Bank", model.Bank);
                    string paymentMethod = model.PaymentMethod == (int)HiEISUtil.PaymentMethod.Cash ? "Tiền mặt" : "Chuyển khoản";
                    testForm.SetField("PaymentMethod", paymentMethod);
                    if (isLastPage)
                    {
                        testForm.SetField("TaxNo", model.TaxNo);
                        string vat = model.VATRate == -1 ? "0" : (model.VATRate * 100).ToString();
                        testForm.SetField("VATRate", vat);
                        testForm.SetField("SubTotal", model.SubTotal.ToString("#,##0"));
                        testForm.SetField("VATAmount", model.VATAmount.ToString("#,##0"));
                        testForm.SetField("Total", model.Total.ToString("#,##0"));
                        testForm.SetField("AmountWords", model.AmountInWords);
                        //testForm.SetField("SignImage_af_image", "");
                    }

                    for (int i = 0; i < model.InvoiceItems.Count; i++)
                    {
                        var item = model.InvoiceItems[i];
                        var total = item.Quantity * item.UnitPrice;
                        testForm.SetField("ProductName" + i, item.ProductName);
                        testForm.SetField("Unit" + i, item.ProductUnit);
                        testForm.SetField("Quantity" + i, item.Quantity.ToString());
                        testForm.SetField("UnitPrice" + i, item.UnitPrice.ToString("#,##0"));
                        testForm.SetField("ProductTotal" + i, total.ToString("#,##0"));
                    }


                    PdfContentByte overContent = pdfStamper.GetOverContent(1);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return fileName;
        }

        public bool NumberOutOfDateInvoices()
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var dueDate = DateTime.Now;
                    var invoices = db.Invoices
                                        .Where(
                                            a => DbFunctions.TruncateTime(a.DueDate) == dueDate.Date 
                                            && !a.PaymentStatus)
                                        .ToList();
                    var transactionBusiness = new TransactionBusiness();

                    foreach (var i in invoices)
                    {
                        int companyId = i.Staff.CompanyId;

                        //nếu template hết block, set qua block mới
                        if (!i.Template.IsActive)
                        {
                            i.TemplateId = GetNextTemplateBlock(i.Template, companyId, db).Id;
                            db.SaveChanges();
                        }

                        int currentNo = i.Template.CurrentNo;
                        string newNumber = (currentNo + 1).ToString().PadLeft(7, '0');

                        i.Number = newNumber;
                        i.Status = (int)HiEISUtil.InvoiceStatus.WaitingOnApproval;
                        i.Date = DateTime.Now;

                        //generate pdf 
                        var pdfModel = Mapper.Map<Invoice, PdfViewModel>(i);
                        i.FileUrl = GenerateFinalPdf(i.FileUrl, pdfModel,1);
                        i.FileUrl2 = GenerateFinalPdf(i.FileUrl2, pdfModel, 2);
                        i.FileUrl3 = GenerateFinalPdf(i.FileUrl3, pdfModel, 3);

                        //set currentNo mới cho template và check nếu template hết block
                        UpdateTemplateNumber(i.TemplateId, companyId, db);

                        //tạo transaction nợ
                        if (i.CustomerId != null)
                        {
                            CreateTransactionModel transaction = new CreateTransactionModel
                            {
                                Amount = i.Total,
                                CustomerId = i.CustomerId,
                                Note = null,
                                Type = (int)HiEISUtil.TransactionType.Liability
                            };
                            CreateTransaction(transaction, companyId);
                        }
                    }

                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        public string GetInvoiceEmail(int id)
        {
            string email = "";
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoice = db.Invoices.Find(id);
                    email = invoice.Email;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return email;
        }

        public PdfViewModel GetPdfModelById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var invoiceInDb = db.Invoices.Find(id);
                var pdfModel = new PdfViewModel();
                if (invoiceInDb != null)
                {
                    pdfModel = Mapper.Map<Invoice, PdfViewModel>(invoiceInDb);
                }
                else
                {
                    pdfModel = null;
                }

                return pdfModel;
            }
        }

        private Template GetNextTemplateBlock(Template currentTemplate, int companyId, HiEISEntities db)
        {
            var nextBlock = db.Templates
                                .FirstOrDefault(a => a.CompanyId == companyId
                                            && a.Form == currentTemplate.Form.ToUpper()
                                            && a.Serial == currentTemplate.Serial.ToUpper()
                                            && a.BeginNo == currentTemplate.CurrentNo + 1);
            return nextBlock;
        }

        private void UpdateTemplateNumber(int templateId, int companyId, HiEISEntities db)
        {
            var t = db.Templates.Find(templateId);

            t.CurrentNo = t.CurrentNo + 1;
            var lastNo = t.Amount + t.BeginNo - 1;

            if (t.CurrentNo == lastNo)
            {
                t.IsActive = false;
                var nextBlock = GetNextTemplateBlock(t, companyId, db);
                nextBlock.IsActive = true;
            }
            db.SaveChanges();
        }

        private bool CreateTransaction(CreateTransactionModel model, int companyId)
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

        private bool UpdateCompanyCustomerLiability(string customerId, int companyId, int type, decimal amount)
        {
            bool result;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var customer = db.CompanyCustomers.FirstOrDefault(a => a.CompanyId == companyId && a.CustomerId == customerId);
                    if (type == (int)HiEISUtil.TransactionType.Liability)
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
                    //throw;
                }
            }
            return result;
        }

        public List<InvoiceListViewModel> GetInvoiceSign(string username)
        {
            using (var db = new HiEISEntities())
            {
                var user = db.AspNetUsers.FirstOrDefault(a => a.UserName == username);
                var companyBusiness = new CompanyBusiness();
                var thiscompany = companyBusiness.GetCompanyByUsername(user.Id);
                List<InvoiceListViewModel> invoices = new List<InvoiceListViewModel>();
                List<Invoice> listInDb = new List<Invoice>();
                var company = db.Companies.Find(thiscompany.Id);

                if (company != null)
                {
                    listInDb = company.Templates.SelectMany(a => a.Invoices).Where(b => b.Status == (int)HiEISUtil.InvoiceStatus.WaitingOnApproval)
                        .ToList();

                    foreach (var item in listInDb)
                    {
                        item.CodeGuid = Guid.NewGuid().ToString();
                        db.SaveChanges();
                    }

                    invoices = listInDb.Select(Mapper.Map<Invoice, InvoiceListViewModel>).ToList();
                }


                return invoices;
            }
        }

        public PdfViewModel Sign(string invoiceGuid, string fileName, string compGuid)
        {
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoiceInDb = db.Invoices.FirstOrDefault(a => a.CodeGuid == invoiceGuid);

                    if (invoiceInDb != null
                        && invoiceInDb.Template.Company.CodeGuid == compGuid
                        && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.WaitingOnApproval)
                    {
                        invoiceInDb.Status = (int)HiEISUtil.InvoiceStatus.Approved;
                        invoiceInDb.Date = DateTime.Now;
                        invoiceInDb.FileUrl = fileName;
                        db.SaveChanges();
                        var pdfModel = Mapper.Map<Invoice, PdfViewModel>(invoiceInDb);
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
                }
            }
        }

        public PdfViewModel Sign(int Id, string fileName)
        {
            using (var db = new HiEISEntities())
            {
                try
                {
                    var invoiceInDb = db.Invoices.FirstOrDefault(a => a.Id == Id);

                    if (invoiceInDb != null
                        && invoiceInDb.Status == (int)HiEISUtil.InvoiceStatus.WaitingOnApproval)
                    {
                        invoiceInDb.Status = (int)HiEISUtil.InvoiceStatus.Approved;
                        invoiceInDb.Date = DateTime.Now;
                        invoiceInDb.FileUrl = fileName;
                        db.SaveChanges();
                        var pdfModel = Mapper.Map<Invoice, PdfViewModel>(invoiceInDb);
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
                }
            }
        }

        public List<InvoiceListViewModel> GetInvoiceToSignByCompanyGuid(string compGuid)
        {
            using (var db = new HiEISEntities())
            {
                var companyBusiness = new CompanyBusiness();
                List<InvoiceListViewModel> invoices = new List<InvoiceListViewModel>();
                List<Invoice> listInDb = new List<Invoice>();
                var company = db.Companies.FirstOrDefault(a => a.CodeGuid == compGuid);

                if (company != null)
                {
                    listInDb = company.Templates.SelectMany(a => a.Invoices).Where(b => b.Status == (int)HiEISUtil.InvoiceStatus.WaitingOnApproval)
                        .ToList();

                    foreach (var item in listInDb)
                    {
                        item.CodeGuid = Guid.NewGuid().ToString();
                        db.SaveChanges();
                    }

                    invoices = listInDb.Select(Mapper.Map<Invoice, InvoiceListViewModel>).ToList();
                }


                return invoices;
            }
        }
    }

}
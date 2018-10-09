using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using HiEIS.Areas.Admin.Models;
using HiEIS.Areas.Public.Models;
using HiEIS.Entities;
using HiEIS.Models;
using HiEIS.Utils;

namespace HiEIS.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Invoice, InvoiceListViewModel>()
                .ForMember(m => m.FileUrl, vm => vm.MapFrom(m => "/Public/Invoices/GetInvoicePdf?invoiceId=" + m.Id))
                .ReverseMap();
            Mapper.CreateMap<Invoice, InvoiceViewModel>()
                .ReverseMap();
            Mapper.CreateMap<PdfViewModel, PdfViewModel>();
            Mapper.CreateMap<Invoice, CreateInvoiceViewModel>().ReverseMap();
            Mapper.CreateMap<Invoice, UpdateInvoiceViewModel>().ReverseMap();
            Mapper.CreateMap<Invoice, PdfViewModel>().ReverseMap();
            Mapper.CreateMap<InvoiceItem, InvoiceItemViewModel>().ReverseMap();
            Mapper.CreateMap<PdfViewModel, CreateInvoiceViewModel>().ReverseMap();
            Mapper.CreateMap<PdfViewModel, UpdateInvoiceViewModel>().ReverseMap();
            Mapper.CreateMap<PdfViewModel, InvoiceViewModel>().ReverseMap();
            Mapper.CreateMap<ProductViewModel, Product>()
                .ForMember(prod => prod.UnitPrice
                    , opt => opt.MapFrom(model => model.DUnitPrice))
                .ReverseMap();
            Mapper.CreateMap<CreateProductModel, Product>()
                .ForMember(prod => prod.UnitPrice
                    , opt => opt.MapFrom(model => model.DUnitPrice))
                .ReverseMap();
            Mapper.CreateMap<UpdateProductModel, Product>()
                .ForMember(prod => prod.UnitPrice
                    , opt => opt.MapFrom(model => model.DUnitPrice))
                .ReverseMap();
            Mapper.CreateMap<DeleteProductModel, Product>();
            Mapper.CreateMap<Company, TableCompanyModel>()
                .ReverseMap();
            Mapper.CreateMap<Company, CompanyModel>()
                .ReverseMap();
            Mapper.CreateMap<Company, UpdateCompanyModel>()
                .ReverseMap();
            Mapper.CreateMap<Company, CreateCompanyModel>()
                .ReverseMap();
            Mapper.CreateMap<Customer, CreateCustomerModel>()
                .ReverseMap();
            Mapper.CreateMap<Customer, TableCustomerModel>()
                .ReverseMap();
            Mapper.CreateMap<Staff, TableStaffModel>()
                .ForMember(a => a.Roles
                                , opt => opt.MapFrom(a =>
                                            a.AspNetUser.AspNetRoles.Select(b => b.Name).ToList()))
                .ForMember(a => a.UserName
                                , opt => opt.MapFrom(a => a.AspNetUser.UserName))
                .ForMember(a => a.Email
                                , opt => opt.MapFrom(a => a.AspNetUser.Email))
                .ReverseMap();
            Mapper.CreateMap<Staff, UpdateStaffModel>()
                .ForMember(a => a.Roles
                                , opt => opt.MapFrom(a =>
                                            a.AspNetUser.AspNetRoles.Select(b => b.Name).ToList()))
               .ReverseMap();
            Mapper.CreateMap<Staff, StaffAccountModel>()
                .ForMember(a => a.Roles
                                , opt => opt.MapFrom(a =>
                                            a.AspNetUser.AspNetRoles.Select(b => b.Name).ToList()))
                .ForMember(a => a.UserName
                                , opt => opt.MapFrom(a => a.AspNetUser.UserName))
                .ForMember(a => a.Email
                                , opt => opt.MapFrom(a => a.AspNetUser.Email))
                .ReverseMap();
            Mapper.CreateMap<TemplateViewModel, Template>()
                .ReverseMap()
                .ForMember(m => m.FileUrl, vm => vm.MapFrom(m => "/Public/Templates/GetTemplatePdf?templateId=" + m.Id));
            Mapper.CreateMap<CreateTemplateModel, Template>()
                .ReverseMap();
            Mapper.CreateMap<UpdateTemplateModel, Template>()
                .ReverseMap();
            Mapper.CreateMap<EditCustomerProductModel, CustomerProduct>()
                .ReverseMap();
            Mapper.CreateMap<DeleteCustomerProductModel, CustomerProduct>()
                .ReverseMap();
            Mapper.CreateMap<CreateTransactionModel, Transaction>()
                .ReverseMap();
            Mapper.CreateMap<Transaction, TableTransactionModel>()
                .ReverseMap();
            Mapper.CreateMap<Invoice, TableInvoiceModel>()
                .ReverseMap();
            Mapper.CreateMap<ProformaInvoice, ProformaInvoiceListViewModel>()
                .ReverseMap();
            Mapper.CreateMap<CreateProformaViewModel, ProformaInvoice>()
                 .ReverseMap();
            Mapper.CreateMap<ProformaInvoiceItemViewModel, ProformaInvoiceItem>()
                 .ReverseMap();
            Mapper.CreateMap<UpdateProformaViewModel, ProformaInvoice>()
                 .ReverseMap();
            Mapper.CreateMap<ProformaViewPDF, ProformaInvoice>()
                .ReverseMap();
            Mapper.CreateMap<TableProformaInvoiceModel, ProformaInvoice>()
                .ReverseMap();
            Mapper.CreateMap<ProformaViewPDF, CreateProformaViewModel>()
                .ReverseMap();
            Mapper.CreateMap<ProformaViewPDF, UpdateProformaViewModel>()
                .ReverseMap();
            Mapper.CreateMap<ProformaViewPDF, ProformaInvoiceViewModel>()
                .ReverseMap();
            Mapper.CreateMap<TableCompanyLiabilitiesModel, Customer>()
                 .ReverseMap();
            Mapper.CreateMap<TableCustomerLiabilitiesModel, Company>()
                 .ReverseMap();
            Mapper.CreateMap<TableLiabilitiesDetailModel, Transaction>()
                 .ReverseMap();
            Mapper.CreateMap<DetailCustomerLiabilitiesModel, Customer>()
                 .ReverseMap();
            Mapper.CreateMap<DetailCustomerTransactionModel, Transaction>()
                 .ReverseMap();
            Mapper.CreateMap<DetailCompanyLiabilitiesModel, Company>()
                 .ReverseMap();
            Mapper.CreateMap<AspNetUser, ListAccountViewModel>()
                .ForMember(a => a.Roles
                                , opt => opt.MapFrom(a =>
                                            a.AspNetRoles.Select(b => b.Name).ToList()))
                 .ReverseMap();

            Mapper.CreateMap<AspNetUser, AccountCustomerViewModel>()
                               .ReverseMap();
            Mapper.CreateMap<AspNetUser, UpdateCustomerAccount>()
                               .ReverseMap();
            Mapper.CreateMap<AspNetUser, AdminViewModel>()
                               .ReverseMap();
            Mapper.CreateMap<AspNetUser, UpdateAdminAccount>()
                               .ReverseMap();
        }
    }
}
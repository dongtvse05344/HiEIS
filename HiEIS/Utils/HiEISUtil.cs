using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.Utils
{
    public static class HiEISUtil
    {
        [EdmFunction("HiEISModel", "String_Like")]
        public static bool Like(this string fieldValue, string lookup)
        {
            return fieldValue?.ToLower().Contains(lookup?.ToLower()) ?? false;
        }

        public static bool IsInRole(this ClaimsIdentity identity, string role)
        {
            return identity.Claims.Any(a => a.Value == role
                            && a.Type == identity.RoleClaimType);
        }

        public static string RemoveFirstSlash(this string fieldValue)
        {
            if (fieldValue?.IndexOf('/') == 0)
            {
                return fieldValue.Substring(1);
            }
            return fieldValue;
        }

        public static List<SelectListItem> GetPaymentMethods()
        {
            List<SelectListItem> paymentMethods = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Thanh toán bằng tiền mặt", Value = "1" },
                new SelectListItem{ Text = "Thanh toán bằng chuyển khoản", Value = "2" }
            };

            return paymentMethods;
        }

        public static DateTime? ParseDate(this string input, string format)
        {
            var parseRs = DateTime.TryParseExact(
                               input
                               , format
                               , null
                               , System.Globalization.DateTimeStyles.None
                               , out DateTime temp2);
            return (parseRs ? temp2 : null as DateTime?);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
            this IQueryable<TSource> source
            , Expression<Func<TSource, TKey>> keySelector
            , bool isAsc)
        {
            if (isAsc)
            {
                return source.OrderBy(keySelector);
            }
            else
            {
                return source.OrderByDescending(keySelector);
            }
        }


        public static List<SelectListItem> GetVATRates()
        {
            List<SelectListItem> rates = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "0%", Value = "0.00" },
                new SelectListItem{ Text = "5%", Value = "0.05" },
                new SelectListItem{ Text = "10%", Value = "0.10" },
                new SelectListItem{ Text = "Không chịu thuế", Value = "-1.00" }
            };

            return rates;
        }

        public static List<SelectListItem> GetStaffRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Kế toán trưởng", Value = RoleAccountingManager },
                new SelectListItem{ Text = "Kế toán công nợ", Value = RoleLiabilityAccountant },
                new SelectListItem{ Text = "Kế toán thanh toán", Value = RolePayableAccountant },
            };

            return roles;
        }

        public static List<SelectListItem> GetAllStaffRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "Quản lý", Value = RoleManager },
                new SelectListItem{ Text = "Kế toán trưởng", Value = RoleAccountingManager },
                new SelectListItem{ Text = "Kế toán công nợ", Value = RoleLiabilityAccountant },
                new SelectListItem{ Text = "Kế toán thanh toán", Value = RolePayableAccountant },
            };

            return roles;
        }

        public enum InvoiceStatus
        {
            New = 1,
            WaitingOnApproval = 2,
            Approved = 3
        }

        public enum TransactionType
        {
            Liability = 1,
            Payment = 2
        }

        public enum PaymentMethod
        {
            Cash = 1,
            BankTransfer = 2
        }

        public enum ProformaInvoiceStatus
        {
            New = 1,
            Approved = 2
        }

        public const string RoleAdmin = "Admin";
        public const string RoleManager = "Manager";
        public const string RoleAccountingManager = "Accounting Manager";
        public const string RoleLiabilityAccountant = "Liability Accountant";
        public const string RolePayableAccountant = "Payable Accountant";
        public const string RoleCustomer = "Customer";

        public const string RoleStaffs = RoleManager + ","
                                        + RoleAccountingManager + ","
                                        + RoleLiabilityAccountant + ","
                                        + RolePayableAccountant;
        public const string RoleNotAdmin = RoleStaffs + ","
                                        + RoleCustomer;
        public const string RoleNormalAccountants = RoleLiabilityAccountant + ","
                                                    + RolePayableAccountant;
        public const string RoleAccountants = RoleNormalAccountants + ","
                                                + RoleAccountingManager;


    }
}
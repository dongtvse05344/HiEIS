using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.Utils
{
    public class HiEISCompanyController : Controller
    {
        private Entities.Company company = null;
        public Entities.Company Company
        {
            get
            {
                //check authentication and authorized
                if (User.Identity.IsAuthenticated
                    && (User.IsInRole(HiEISUtil.RoleManager)
                            || User.IsInRole(HiEISUtil.RoleAccountingManager)
                            || User.IsInRole(HiEISUtil.RoleLiabilityAccountant) 
                            || User.IsInRole(HiEISUtil.RolePayableAccountant)
                        )
                    )
                {
                    if (this.company == null)
                    {
                        var business = new Businesses.CompanyBusiness();
                        this.company = business.GetCompanyByUsername(User.Identity.GetUserId());
                    }
                }
                return company;
            }
        }

        public string UserId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }
    }
}
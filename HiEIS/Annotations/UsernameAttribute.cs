using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.Annotations
{
    public class UsernameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            var userManager = HttpContext
                .Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByNameAsync(value as string);
            Task.WaitAll(new[] { user });
            if (user.Result == null)
            {
                return true;
            }
            return false;
        }
    }
}
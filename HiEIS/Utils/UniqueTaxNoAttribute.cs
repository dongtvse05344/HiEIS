using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HiEIS.Entities;

namespace HiEIS.Utils
{
    public class UniqueTaxNoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            string strValue = value as string;

            using (var db = new HiEISEntities())
            {
                var company = db.Companies.Where(c => c.TaxNo == strValue).FirstOrDefault();
                if (company == null)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiEIS.Utils
{
    public class HasDigitsOnlyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            string strValue = value as string;

            foreach (char c in strValue)
            {
                if (c < '0' || c > '9')
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
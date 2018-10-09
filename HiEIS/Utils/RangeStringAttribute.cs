using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HiEIS.Utils
{
    public class RangeStringAttribute : RangeAttribute
    {
        private Type type;

        public RangeStringAttribute(double minimum, double maximum, Type type) : base(minimum, maximum)
        {
            this.type = type;
        }
        
        public override bool IsValid(object value)
        {
            dynamic numericValue = 0;
            if (Regex.IsMatch(value.ToString(), "^\\d+(\\.\\d+)?$"))
            {
                numericValue = Convert.ChangeType(value, type);
            }
            return base.IsValid(numericValue as object);
        }
    }
}
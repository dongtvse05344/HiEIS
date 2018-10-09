using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HiEIS.Utils
{
    public class RegexAttribute : ValidationAttribute
    {
        private string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = pattern;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rs = base.IsValid(value, validationContext);
            return rs;
        }
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            return Regex.IsMatch(value.ToString(), this.pattern);
        }
    }
}
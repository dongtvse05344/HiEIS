using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HiEIS.Models
{
    public class ValidationResultModel : List<ValidationModel>
    {
        private IdentityResult createUserResult;

        public ValidationResultModel(IEnumerable<DbEntityValidationResult> entityValidationErrors)
        {
            foreach (var entity in entityValidationErrors.Where(a => !a.IsValid))
            {
                foreach (var error in entity.ValidationErrors)
                {
                    var name = string.Format("{0}.{1}", entity.Entry.Entity.GetType().Name, error.PropertyName);
                    Add(new ValidationModel
                    {
                        name = name,
                        error = error.ErrorMessage
                    });
                }
            }
        }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return;

            foreach (var state in modelState)
            {
                if (modelState[state.Key].Errors.Any())
                {
                    foreach (var error in modelState[state.Key].Errors)
                    {
                        Add(new ValidationModel
                        {
                            name = state.Key,
                            error = error.ErrorMessage
                        });
                    }
                }
            }
        }

        public ValidationResultModel(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.Add(new ValidationModel { error = error});
            }
        }
    }
}
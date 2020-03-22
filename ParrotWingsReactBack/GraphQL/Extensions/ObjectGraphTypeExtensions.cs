using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Extensions
{
    public static class ObjectGraphTypeExtensions
    {
        private const string CurrentUserNotAuthorized = "Current user is not authorized";

        public static bool Authorize(this ObjectGraphType type, IHttpContextAccessor httpContextAccessor, ResolveFieldContext<object> context)
        {
            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Errors.Add(new ExecutionError(CurrentUserNotAuthorized));
                return false;
            }
            return true;
        }

        public static bool ValidateInput(this ObjectGraphType type, ResolveFieldContext<object> context, object model)
        {
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, validationContext, validationResults, true))
                return true;

            foreach (var result in validationResults)
            {
                context.Errors.Add(new ExecutionError(result.ErrorMessage));
            }
            return false;
        }
    }
}

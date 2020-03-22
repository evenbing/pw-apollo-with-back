using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
    }
}

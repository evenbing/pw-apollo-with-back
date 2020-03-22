using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using PW.DataAccess.Interfaces;
using PW.Services.Exceptions;
using PW.Services.Interfaces;
using PW.Web.GraphQL.Extensions;
using PW.Web.GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL
{    
    public class PwQuery : ObjectGraphType
    {        
        private const string CurrentUserNotFoundMessage = "Current user not found";

        public PwQuery(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, ITransactionService transactionService)
        {
            #region Session

            FieldAsync<SessionInfoType>(
                "sessionInfo",
                resolve: async context =>
                {
                    if (!this.Authorize(httpContextAccessor, context)) 
                        return null;

                    var email = httpContextAccessor.HttpContext.User.Identity.Name;
                    var user = await userRepository.GetByEmailAsync(email);
                    if (user == null)
                    {
                        context.Errors.Add(new ExecutionError(CurrentUserNotFoundMessage));
                        return null;
                    }
                    return user;
                }
            );

            #endregion

            #region Users

            FieldAsync<ListGraphType<UserNameOptionType>>(
                "userNameOptions",
                resolve: async context =>
                {
                    if (!this.Authorize(httpContextAccessor, context))
                        return null;

                    var email = httpContextAccessor.HttpContext.User.Identity.Name;
                    var users = await userRepository.FindByAsync(x => x.Email != email);
                    return users;
                }
            );

            #endregion

            #region Transactions

            FieldAsync<ListGraphType<TransactionType>>(
                "transactionInfos",
                resolve: async context =>
                {
                    if (!this.Authorize(httpContextAccessor, context))
                        return null;

                    var email = httpContextAccessor.HttpContext.User.Identity.Name;
                    var transactions = await transactionService.GetTransactionsOrderedByDateAsync(email);
                    return transactions;
                }
            ); 

            #endregion
        }
    }
}

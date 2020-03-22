using GraphQL.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using PW.DataTransferObjects.Transactions;
using PW.DataTransferObjects.Users;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Interfaces;
using PW.Web.GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PW.Web.GraphQL
{    
    public class PwMutation : ObjectGraphType
    {
        public PwMutation(IHttpContextAccessor httpContextAccessor, IMembershipService membershipService, ITransactionService transactionService)
        {
            #region Session

            FieldAsync<SessionInfoType>(
                    "login",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<LoginOptionsInput>> { Name = "loginOptions" }),
                    resolve: async context =>
                    {
                        var loginDto = context.GetArgument<LoginDto>("loginOptions");

                        PwUser user = null;
                        ClaimsPrincipal claimsPrincipal = null;
                        try
                        {
                            user = await membershipService.GetUserAsync(loginDto);
                            claimsPrincipal = membershipService.GetUserClaimsPrincipal(user);
                        }
                        catch (PWException ex)
                        {
                            var a = 0;
                        // return BadRequest(new { errorMessage = ex.Message });
                        }

                        await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        return user;
                    }
                );

            FieldAsync<SessionInfoType>(
                "signUp",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<SignUpOptionsInput>> { Name = "signUpOptions" }),
                resolve: async context =>
                {
                    var signUpDto = context.GetArgument<SignUpDto>("signUpOptions");

                    //if (!ModelState.IsValid)
                    //{
                    //    return BadRequest(InvalidUserDataMessage);
                    //}

                    PwUser user = null;
                    ClaimsPrincipal claimsPrincipal = null;
                    try
                    {
                        user = await membershipService.CreateUserAsync(signUpDto);
                        claimsPrincipal = membershipService.GetUserClaimsPrincipal(user);
                    }
                    catch (PWException ex)
                    {
                        var a = 0;
                        // return BadRequest(new { errorMessage = ex.Message });
                    }

                    await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return user;
                }
            );

            FieldAsync<SessionInfoType>(
               "logout",
               resolve: async context =>
               {
                   await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                   return null;
               }
           );

            #endregion

            #region Transactions

            FieldAsync<ListGraphType<TransactionType>>(
                           "createTransaction",
                           arguments: new QueryArguments(new QueryArgument<NonNullGraphType<NewTransactionInput>> { Name = "newTransaction" }),
                           resolve: async context =>
                           {
                               var createTransactionDto = context.GetArgument<CreateTransactionDto>("newTransaction");
                           //if (!ModelState.IsValid)
                           //{
                           //    return BadRequest(ModelState);
                           //}

                           var payeeEmail = httpContextAccessor.HttpContext.User.Identity.Name;
                               try
                               {
                                   await transactionService.CreateTransactionAsync(payeeEmail, createTransactionDto);
                               }
                               catch (PWException ex)
                               {
                                   var a = 0;
                               //return BadRequest(new { errorMessage = ex.Message });
                           }
                               return null;
                           }
                       ); 

            #endregion
        }
    }
}

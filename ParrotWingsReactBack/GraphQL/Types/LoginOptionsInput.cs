using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{    
    public class LoginOptionsInput : InputObjectGraphType
    {
        public LoginOptionsInput()
        {            
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("password");
        }
    }
}

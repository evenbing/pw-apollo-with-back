using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{    
    public class SignUpOptionsInput : InputObjectGraphType
    {
        public SignUpOptionsInput()
        {
            Field<NonNullGraphType<StringGraphType>>("email");
            Field<NonNullGraphType<StringGraphType>>("userName");
            Field<NonNullGraphType<StringGraphType>>("password");
            Field<NonNullGraphType<StringGraphType>>("confirmPassword");
        }
    }
}

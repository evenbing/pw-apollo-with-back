using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{
    public class NewTransactionInput : InputObjectGraphType
    {
        public NewTransactionInput()
        {
            Field<NonNullGraphType<StringGraphType>>("recipient");
            Field<NonNullGraphType<IntGraphType>>("amount");
        }        
    }
}

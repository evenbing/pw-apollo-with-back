using GraphQL.Types;
using PW.DataTransferObjects.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{
    public class TotalCountType : ObjectGraphType<TotalCountDto>
    {
        public TotalCountType()
        {
            Field(x => x.Count);
        }
    }
}

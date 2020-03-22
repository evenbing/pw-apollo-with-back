using GraphQL.Types;
using PW.DataTransferObjects.Transactions;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{    
    public class TransactionType : ObjectGraphType<TransactionDto>
    {
        public TransactionType()
        {
            Field(x => x.Date);
            Field(x => x.CorrespondentName);
            Field(x => x.Amount);
            Field(x => x.ResultBalance);
        }
    }
}

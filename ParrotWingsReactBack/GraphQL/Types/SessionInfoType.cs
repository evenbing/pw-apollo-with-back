using GraphQL.Types;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{    
    public class SessionInfoType : ObjectGraphType<PwUser>
    {
        public SessionInfoType()
        {            
            Field(x => x.UserName);
            Field(x => x.Balance);
        }
    }
}

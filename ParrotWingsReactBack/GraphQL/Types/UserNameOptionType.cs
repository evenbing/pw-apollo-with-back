using GraphQL.Types;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL.Types
{    
    public class UserNameOptionType : ObjectGraphType<PwUser>
    {
        public UserNameOptionType()
        {
            Field(x => x.UserName);            
        }
    }
}

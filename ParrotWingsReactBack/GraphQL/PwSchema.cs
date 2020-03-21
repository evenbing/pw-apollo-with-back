using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.GraphQL
{    
    public class PwSchema : Schema
    {
        public PwSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<PwQuery>();
        }
    }
}

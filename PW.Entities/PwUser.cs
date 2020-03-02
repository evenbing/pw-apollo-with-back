using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Entities
{
    public class PwUser : BaseEntity
    {        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int Balance { get; set; }

        public ICollection<PwTransaction> PayeeTransactions { get; set; }
        public ICollection<PwTransaction> RecipientTransactions { get; set; }
    }
}

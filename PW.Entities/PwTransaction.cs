using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Entities
{
    public class PwTransaction : BaseEntity
    {        
        public int ResultingPayeeBalance { get; set; }
        public int ResultingRecipientBalance { get; set; }
        public int Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }

        public virtual PwUser Payee { get; set; }
        public virtual PwUser Recipient { get; set; }
    }
}

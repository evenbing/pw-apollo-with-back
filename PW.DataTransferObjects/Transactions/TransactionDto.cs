using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataTransferObjects.Transactions
{
    public class TransactionDto
    {
        public string Date { get; set; }
        public string CorrespondentName { get; set; }
        public int Amount { get; set; }
        public int ResultBalance { get; set; }        
    }
}

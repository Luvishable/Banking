using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking.ENTITIES.ENUMS;

namespace Banking.SERVICES
{
    public class AccountTransaction
    {
        public TransactionType TransactionType { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }

        public AccountTransaction(TransactionType type, decimal amount)
        {
            TransactionType = type;
            Amount = amount;
            Date = DateTime.Now;
        }
    }
}

using Banking.ENTITIES.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.ENTITIES
{
    public class CheckingAccount: BankAccount
    {
        public CheckingAccount(User user, CurrencyType currency)
            : base(user, currency)
        {

        }
    }
}

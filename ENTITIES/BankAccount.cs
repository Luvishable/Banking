using System;
using System.Collections.Generic;
using Banking.ENTITIES.ENUMS;
using Banking.SERVICES;

namespace Banking.ENTITIES
{
    public abstract class BankAccount
    {
        public Guid AccountNumber { get; }
        public CurrencyType Currency { get; }
        public decimal Balance { get; protected set; }
        public User User  {get;}

        protected List<AccountTransaction> accountTransactions = new List<AccountTransaction>();

        protected BankAccount(User user, CurrencyType currency)
        {
            AccountNumber = Guid.NewGuid();
            Currency = currency;
            Balance = 0;
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }
            Balance += amount;

            accountTransactions.Add(new AccountTransaction(TransactionType.Deposit, amount));

        }

        public virtual void Withdraw(decimal amount) 
        {
            if (amount <= 0) 
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }
            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient Balance.");
            }
            Balance -= amount;
            accountTransactions.Add(new AccountTransaction(TransactionType.Withdraw,amount));
        }

        public IReadOnlyList<AccountTransaction> GetTransactionHistory()
        {
            return accountTransactions.AsReadOnly();
        } 



    }
}

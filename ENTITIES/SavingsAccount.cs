using System;
using System.Collections.Generic;
using Banking.ENTITIES.ENUMS;
using Banking.SERVICES;

namespace Banking.ENTITIES
{
    public class SavingsAccount : BankAccount
    {   
        // Faizle ilgili fieldler nullable yapılacak. Böylece işleyen bir faiz sürecinin olup olmadığı kontrol edilebilir.
        public decimal? InterestRate { get; private set; }
        public InterestInterval? InterestInterval { get; private set; }
        public DateTime? InterestStartDate { get; private set; }
        public decimal? Capital { get; private set; }

        public bool IsInterestActive => InterestStartDate.HasValue && InterestRate.HasValue && InterestInterval.HasValue && Capital.HasValue;

        public SavingsAccount(User user, CurrencyType currency)
            : base(user, currency)
        {

        }

        public override void Deposit(decimal amount)
        {
            if (IsInterestActive)
            {
                throw new InvalidOperationException("You can not deposit while interest is active!");
            }
            base.Deposit(amount);
            
        }

        public override void Withdraw(decimal amount)
        {
            if (IsInterestActive)
            {
                if (!IsInterestPeriodComplete())
                    throw new InvalidOperationException("Cannot withdraw until interest period is complete.");
                
                SettleInterest();
            }

            base.Withdraw(amount);
        }


        private bool IsInterestPeriodComplete()
        {
            if (!InterestStartDate.HasValue || InterestInterval.HasValue)
            {
                return false;
            }
            var durationInDays = (int)InterestInterval.Value;
            var completionDate = InterestStartDate.Value.AddDays(durationInDays);

            return DateTime.Now >= completionDate;
        }

        private void ClearInterestState()
        {
            InterestRate = null;
            InterestInterval = null;
            InterestStartDate = null;
            Capital = null;
        }

        public void ApplyInterest(decimal amount, decimal interestRate, InterestInterval interestInterval)
        {
            if (IsInterestActive) 
                throw new InvalidOperationException("An interest process is already active!");
            
            if (amount <= 0)
                throw new ArgumentException("Principal amount must be positive.", nameof(amount));

            if (interestRate <= 0)
                throw new ArgumentException("Interest rate must be positive.", nameof(interestRate));

            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds for interest investment.");

            // Eğer tüm koşullar sağlanıyorsa faiz sürecini başlat
            Capital = amount;
            InterestRate = interestRate;
            InterestInterval = interestInterval;
            InterestStartDate = DateTime.Now;

            // Faize yatırılan parayı Balance'tan düş
            Balance -= amount;

        }

        public void SettleInterest()
        {
            if (!IsInterestActive)
                throw new InvalidOperationException("No active interest process to settle.");

            if (!IsInterestPeriodComplete())
                throw new InvalidOperationException("Interest period has not yet completed.");

            decimal capital = Capital.Value;
            decimal rate = InterestRate.Value;

            decimal interest = capital * rate;
            decimal total = capital + interest;

            Balance += total;

            accountTransactions.Add(new AccountTransaction(TransactionType.InterestSettlement, interest));

            // Para faiziyle birlikte çekildiği için faiz parametrelerini temizle ve hesabı diğer bir faiz sürecine hazırla
            ClearInterestState();

        }

        


    }
}

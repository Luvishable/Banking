using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.ENTITIES
{
    public class User
    {
        private readonly Guid Id;
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; }

        private readonly List<BankAccount> _accounts = new();
        public IReadOnlyList<BankAccount> Accounts => _accounts.AsReadOnly();

        public string FullName => $"{Name} {Surname}";


        public User(string name, string surname, DateTime birthDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            if (!IsAgeGreaterThanEighteen(birthDate))
            {
                throw new ArgumentException("18 yaşından küçükler hesap açamaz");
            }
            BirthDate = birthDate;

        }

        public void AddAccount(BankAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            _accounts.Add(account);
        }

        public List<BankAccount> GetAllBankAccounts()
        {
            return Accounts.ToList();
        }

        public BankAccount GetBankAccount(Guid id)
        {
            if (Accounts.Count == 1)
            {
                return Accounts[0];
            }
            var bankAccount = Accounts.FirstOrDefault(b => b.Equals(id));
            return bankAccount;
        }

        private bool IsAgeGreaterThanEighteen(DateTime birthDate)
        {
            var today = DateTime.Today;
            var ageSpan = today - birthDate;

            var age = ageSpan.TotalDays / 365;

            return age >= 18;
        }




    }
}

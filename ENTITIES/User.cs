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

        public List<BankAccount> BankAccounts { get; set; }


        public User(string name, string surname, DateTime birthDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            if (!IsAgeGreaterThanEighteen(birthDate))
            {
                throw new ArgumentException("18 yaşından küçükler hesap açamaz")
            }
            BirthDate = birthDate;

        }

        public List<BankAccount> GetAllBankAccounts()
        {
            return BankAccounts.ToList();
        }

        public BankAccount GetBankAccount(Guid id)
        {
            if (BankAccounts.Count == 1)
            {
                return BankAccounts[0];
            }
            var bankAccount = BankAccounts.FirstOrDefault(b => b.Equals(id));
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

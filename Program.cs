using Banking.ENTITIES;
using Banking.ENTITIES.ENUMS;
using System;
using System.Collections.Generic;

namespace Banking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User user = new User("Ahmet", "Laçin", new DateTime(1995, 6, 7));
            BankAccount account = null;

            while (true)
            {
                Console.WriteLine("\n=== BANKING SYSTEM ===");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Check Balance");
                Console.WriteLine("5. View Transaction History");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        if (account != null)
                        {
                            Console.WriteLine("You already have an account.");
                            break;
                        }

                        Console.WriteLine("Select account type:");
                        Console.WriteLine("1. Checking Account");
                        Console.WriteLine("2. Savings Account");
                        Console.Write("Choice: ");
                        string accType = Console.ReadLine();

                        Console.WriteLine("Select currency (e.g. TRY, USD, EUR):");
                        string currencyInput = Console.ReadLine().ToUpper();

                        if (!Enum.TryParse(currencyInput, out CurrencyType currency))
                        {
                            Console.WriteLine("Invalid currency.");
                            break;
                        }

                        if (accType == "1")
                            account = new CheckingAccount(user, currency);
                        else if (accType == "2")
                            account = new SavingsAccount(user, currency);
                        else
                        {
                            Console.WriteLine("Invalid account type.");
                            break;
                        }

                        Console.WriteLine($"> Account created with number: {account.AccountNumber}");
                        break;

                    case "2":
                        if (account == null) { Console.WriteLine("Please create an account first."); break; }

                        Console.Write("Enter amount to deposit: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                        {
                            account.Deposit(depositAmount);
                            Console.WriteLine("> Deposit successful.");
                        }
                        else Console.WriteLine("Invalid amount.");
                        break;

                    case "3":
                        if (account == null) { Console.WriteLine("Please create an account first."); break; }

                        Console.Write("Enter amount to withdraw: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                        {
                            try
                            {
                                account.Withdraw(withdrawAmount);
                                Console.WriteLine("> Withdrawal successful.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"> Withdrawal failed: {ex.Message}");
                            }
                        }
                        else Console.WriteLine("Invalid amount.");
                        break;

                    case "4":
                        if (account == null) { Console.WriteLine("Please create an account first."); break; }
                        Console.WriteLine($"> Current Balance: {account.Balance} {account.Currency}");
                        break;

                    case "5":
                        if (account == null) { Console.WriteLine("Please create an account first."); break; }

                        Console.WriteLine("> Transaction History:");
                        foreach (var txn in account.GetTransactionHistory())
                        {
                            Console.WriteLine($"- {txn.Date}: {txn.TransactionType} {txn.Amount} {account.Currency}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
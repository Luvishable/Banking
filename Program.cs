using Banking.ENTITIES;
using Banking.ENTITIES.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User user = new User("Ahmet Faruk", "Laçin", new DateTime(1995, 6, 7));
            List<BankAccount> accounts = new();

            while (true)
            {
                Console.WriteLine("\n=== BANKING SYSTEM ===");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Check Balance");
                Console.WriteLine("5. View Transaction History");
                Console.WriteLine("6. View All My Accounts");
                Console.WriteLine("7. Apply Interest (Savings Only)");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Select account type:");
                        Console.WriteLine("1. Checking Account");
                        Console.WriteLine("2. Savings Account");
                        Console.Write("Choice: ");
                        string accType = Console.ReadLine();

                        Console.Write("Currency (such as TRY, USD, EUR): ");
                        string currencyInput = Console.ReadLine().ToUpper();

                        if (!Enum.TryParse(currencyInput, out CurrencyType currency))
                        {
                            Console.WriteLine("Invalid currency.");
                            break;
                        }

                        BankAccount newAccount;
                        if (accType == "1")
                            newAccount = new CheckingAccount(user, currency);
                        else if (accType == "2")
                            newAccount = new SavingsAccount(user, currency);
                        else
                        {
                            Console.WriteLine("Invalid account type.");
                            break;
                        }

                        accounts.Add(newAccount);
                        Console.WriteLine($"> Account created: {newAccount.AccountNumber}");
                        break;

                    case "2":
                        var depositAccount = SelectAccount(accounts);
                        if (depositAccount == null) break;

                        Console.Write("Amount to deposit: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                        {
                            depositAccount.Deposit(depositAmount);
                            Console.WriteLine("> Deposit successful.");
                        }
                        else Console.WriteLine("Invalid amount.");
                        break;

                    case "3":
                        var withdrawAccount = SelectAccount(accounts);
                        if (withdrawAccount == null) break;

                        Console.Write("Amount to withdraw: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                        {
                            try
                            {
                                withdrawAccount.Withdraw(withdrawAmount);
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
                        var balanceAccount = SelectAccount(accounts);
                        if (balanceAccount == null) break;

                        Console.WriteLine($"> Balance: {balanceAccount.Balance} {balanceAccount.Currency}");
                        break;

                    case "5":
                        var historyAccount = SelectAccount(accounts);
                        if (historyAccount == null) break;

                        Console.WriteLine("> Transaction History:");
                        foreach (var transaction in historyAccount.GetTransactionHistory())
                        {
                            Console.WriteLine($"- {transaction.Date}: {transaction.TransactionType} {transaction.Amount} {historyAccount.Currency}");
                        }
                        break;

                    case "6":
                        if (!accounts.Any())
                        {
                            Console.WriteLine("No accounts available.");
                            break;
                        }

                        Console.WriteLine("> Your Accounts:");
                        foreach (var acc in accounts)
                        {
                            Console.WriteLine($"- {acc.GetType().Name} | {acc.AccountNumber} | {acc.Currency} | Balance: {acc.Balance}");
                        }
                        break;

                    case "7":
                        var savings = SelectAccount(accounts, typeof(SavingsAccount));
                        if (savings == null) break;

                        Console.Write("Principal amount to apply interest on: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal principal))
                        {
                            Console.WriteLine("Invalid amount.");
                            break;
                        }

                        Console.Write("Interest rate (e.g. 0.1 for 10%): ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal rate))
                        {
                            Console.WriteLine("Invalid rate.");
                            break;
                        }

                        Console.WriteLine("Select interest interval (Daily, Monthly, Yearly):");
                        string intervalInput = Console.ReadLine();

                        if (!Enum.TryParse(intervalInput, out InterestInterval interval))
                        {
                            Console.WriteLine("Invalid interval.");
                            break;
                        }

                        try
                        {
                            ((SavingsAccount)savings).ApplyInterest(principal, rate, interval);
                            Console.WriteLine("> Interest applied.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"> Failed to apply interest: {ex.Message}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static BankAccount SelectAccount(List<BankAccount> accounts, Type typeFilter = null)
        {
            if (!accounts.Any())
            {
                Console.WriteLine("No accounts available.");
                return null;
            }

            var filtered = typeFilter == null
                ? accounts
                : accounts.Where(a => a.GetType() == typeFilter).ToList();

            if (!filtered.Any())
            {
                Console.WriteLine("No matching accounts found.");
                return null;
            }

            Console.WriteLine("Select Account:");
            for (int i = 0; i < filtered.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {filtered[i].GetType().Name} - {filtered[i].AccountNumber}");
            }

            Console.Write("Choice: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= filtered.Count)
            {
                return filtered[choice - 1];
            }

            Console.WriteLine("Invalid selection.");
            return null;
        }
    }
}
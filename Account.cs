using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public class Account
    {
        private static int AccountNumberSeed = 100000;

        public string AccountNumber { get; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; } // Savings or Checking
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; set; }

        public Account(string accountHolderName, string accountType, decimal initialDeposit)
        {
            AccountNumber = AccountNumberSeed.ToString();
            AccountNumberSeed++;
            AccountHolderName = accountHolderName;
            AccountType = accountType;
            Balance = initialDeposit;
            Transactions = new List<Transaction>();
            // Log initial deposit transaction
            Transactions.Add(new Transaction(initialDeposit, DateTime.Now, "Initial Deposit"));
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            Transactions.Add(new Transaction(amount, DateTime.Now, "Deposit"));
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                return false; // Insufficient funds
            }
            Balance -= amount;
            Transactions.Add(new Transaction(-amount, DateTime.Now, "Withdrawal"));
            return true;
        }

        public void ApplyInterest(decimal interestRate)
        {
            decimal interest = Balance * interestRate;
            Balance += interest;
            Transactions.Add(new Transaction(interest, DateTime.Now, "Interest Credit"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingApplication
{
    public class BankingSystem
    {
        private List<User> users;
        private User currentUser;

        public BankingSystem()
        {
            users = new List<User>();
            currentUser = null;
        }

        // User Registration
        public void RegisterUser()
        {
            Console.WriteLine("=== User Registration ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            if (users.Any(u => u.Username == username))
            {
                Console.WriteLine("Username already exists. Try a different one.");
                return;
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            users.Add(new User(username, password));
            Console.WriteLine("Registration successful.");
        }

        // User Login
        public bool Login()
        {
            Console.WriteLine("=== User Login ===");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine("Login successful.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
                return false;
            }
        }

        // Account Opening
        public void OpenAccount()
        {
            Console.WriteLine("=== Open New Account ===");
            Console.Write("Enter account holder's name: ");
            string name = Console.ReadLine();
            Console.Write("Enter account type (Savings/Checking): ");
            string type = Console.ReadLine();
            Console.Write("Enter initial deposit amount: ");
            decimal initialDeposit;
            while (!decimal.TryParse(Console.ReadLine(), out initialDeposit) || initialDeposit < 0)
            {
                Console.Write("Invalid amount. Enter a valid initial deposit amount: ");
            }

            Account newAccount = new Account(name, type, initialDeposit);
            currentUser.Accounts.Add(newAccount);

            Console.WriteLine($"Account created successfully. Account Number: {newAccount.AccountNumber}");
        }

        // Transaction Processing
        public void ProcessTransaction()
        {
            Console.WriteLine("=== Process Transaction ===");
            if (!SelectAccount(out Account account))
            {
                return;
            }

            Console.Write("Enter transaction type (Deposit/Withdrawal): ");
            string transactionType = Console.ReadLine().ToLower();

            Console.Write("Enter amount: ");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.Write("Invalid amount. Enter a valid amount: ");
            }

            if (transactionType == "deposit")
            {
                account.Deposit(amount);
                Console.WriteLine("Deposit successful.");
            }
            else if (transactionType == "withdrawal")
            {
                if (account.Withdraw(amount))
                {
                    Console.WriteLine("Withdrawal successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient funds.");
                }
            }
            else
            {
                Console.WriteLine("Invalid transaction type.");
            }
        }

        // Statement Generation
        public void GenerateStatement()
        {
            Console.WriteLine("=== Account Statement ===");
            if (!SelectAccount(out Account account))
            {
                return;
            }

            Console.WriteLine($"Account Number: {account.AccountNumber}");
            Console.WriteLine($"Account Holder: {account.AccountHolderName}");
            Console.WriteLine($"Account Type: {account.AccountType}");
            Console.WriteLine($"Current Balance: {account.Balance:C}");
            Console.WriteLine("Transaction History:");
            Console.WriteLine("ID\tDate\t\tType\t\tAmount");
            foreach (var transaction in account.Transactions)
            {
                string type = transaction.Notes;
                Console.WriteLine($"{transaction.TransactionId}\t{transaction.Date.ToShortDateString()}\t{type}\t\t{transaction.Amount:C}");
            }
        }

        // Interest Calculation
        public void CalculateInterest()
        {
            Console.WriteLine("=== Calculate Interest ===");
            if (!SelectAccount(out Account account))
            {
                return;
            }

            if (account.AccountType.ToLower() != "savings")
            {
                Console.WriteLine("Interest calculation is only applicable for savings accounts.");
                return;
            }

            Console.Write("Enter monthly interest rate (in %): ");
            decimal interestRate;
            while (!decimal.TryParse(Console.ReadLine(), out interestRate) || interestRate < 0)
            {
                Console.Write("Invalid rate. Enter a valid interest rate: ");
            }

            decimal rate = interestRate / 100;
            account.ApplyInterest(rate);

            Console.WriteLine("Interest calculated and added to the account.");
        }

        // Balance Check
        public void CheckBalance()
        {
            Console.WriteLine("=== Check Balance ===");
            if (!SelectAccount(out Account account))
            {
                return;
            }

            Console.WriteLine($"Current balance for Account {account.AccountNumber}: {account.Balance:C}");
        }

        // Logout
        public void Logout()
        {
            currentUser = null;
            Console.WriteLine("Logged out successfully.");
        }

        // Helper Method to Select Account
        private bool SelectAccount(out Account selectedAccount)
        {
            selectedAccount = null;

            if (currentUser.Accounts.Count == 0)
            {
                Console.WriteLine("No accounts found. Please open an account first.");
                return false;
            }

            Console.WriteLine("Select an account by entering the account number:");
            foreach (var acc in currentUser.Accounts)
            {
                Console.WriteLine($"- {acc.AccountNumber} ({acc.AccountType})");
            }

            string accNumber = Console.ReadLine();
            selectedAccount = currentUser.Accounts.FirstOrDefault(a => a.AccountNumber == accNumber);

            if (selectedAccount == null)
            {
                Console.WriteLine("Invalid account number.");
                return false;
            }

            return true;
        }

        // Main Menu
        public void DisplayMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Banking System ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        if (Login())
                        {
                            DisplayUserMenu();
                        }
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // User Menu
        private void DisplayUserMenu()
        {
            while (currentUser != null)
            {
                Console.WriteLine("\n=== User Menu ===");
                Console.WriteLine("1. Open Account");
                Console.WriteLine("2. Process Transaction");
                Console.WriteLine("3. Generate Statement");
                Console.WriteLine("4. Calculate Interest");
                Console.WriteLine("5. Check Balance");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        OpenAccount();
                        break;
                    case "2":
                        ProcessTransaction();
                        break;
                    case "3":
                        GenerateStatement();
                        break;
                    case "4":
                        CalculateInterest();
                        break;
                    case "5":
                        CheckBalance();
                        break;
                    case "6":
                        Logout();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}


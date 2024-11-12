using System;

namespace BankingApplication
{
    public class Transaction
    {
        private static int TransactionIdSeed = 1;

        public int TransactionId { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Notes { get; }

        public Transaction(decimal amount, DateTime date, string notes)
        {
            TransactionId = TransactionIdSeed++;
            Amount = amount;
            Date = date;
            Notes = notes;
        }
    }
}

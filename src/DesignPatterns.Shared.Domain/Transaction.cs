using System;

namespace DesignPatterns.Shared.Domain {
    
    /// <summary>
    /// Simplified representation of a transaction in a financial system.
    /// </summary>
    public class Transaction {
        private Guid id;

        public Transaction(
            DateTimeOffset transactionDate,
            DateTimeOffset valueDate,
            CreditDebitIndicator creditDebitIndicator,
            decimal amount
        ) {
            TransactionDate = transactionDate;
            ValueDate = valueDate;
            CreditDebitIndicator = creditDebitIndicator;
            Amount = amount;
            id = Guid.NewGuid();
        }

        public DateTimeOffset TransactionDate { get; }
        public DateTimeOffset ValueDate { get; }
        public CreditDebitIndicator CreditDebitIndicator { get; }
        public decimal Amount { get; }
    }
}
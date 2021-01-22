using System;

namespace DesignPatterns.Shared.Domain {
    
    /// <summary>
    /// Simplified representation of a transaction in a financial system.
    /// </summary>
    public class Transaction {
        private Guid id;
        private readonly string paymentDetails;

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

        public string PaymentDetails {
            get => paymentDetails;
            init {
                if (value.Length > 100) throw new ArgumentException("Details max length is 100 characters.");
                paymentDetails = value;
            }
        }
    }
}
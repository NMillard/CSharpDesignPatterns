using System;
using DesignPatterns.Shared.Domain;

namespace StorageFormatting {
    public record TransactionRow : StorageBase {
        [Field("AMOUNT", 1)]
        [FieldInfo("AMOUNT", 1)] public decimal Amount { get; init; }

        [FieldInfo("TRANSACTION_DATE", 2, ValueFormatting.DateDDMMYYYY)]
        [Field("TRANSACTION_DATE", 2, typeof(DateToYYYYMMDD))]
        public DateTimeOffset TransactionDate { get; init; }

        [FieldInfo("VALUE_DATE", 3, ValueFormatting.DateDDMMYYYY)]
        [Field("VALUE_DATE", 3, typeof(DateToYYYYMMDD))]
        public DateTimeOffset ValueDate { get; init; }
        
        [FieldInfo("CREDIT_DEBIT_INDICATOR", 4)]
        [Field("CREDIT_DEBIT_INDICATOR", 4)]
        public CreditDebitIndicator CreditDebitIndicator { get; init; }

        [FieldInfo("PAYMENT_DETAILS", 5, ValueFormatting.EscapeQuotes)]
        [Field("PAYMENT_DETAILS", 5, typeof(EscapeQuotesConverter))]
        public string PaymentDetails { get; init; }

        public static implicit operator TransactionRow(Transaction transaction) => new() {
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            ValueDate = transaction.ValueDate,
            CreditDebitIndicator = transaction.CreditDebitIndicator,
            PaymentDetails = transaction.PaymentDetails,
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DesignPatterns.Shared.Domain;

namespace StorageFormatting {
    /**
     * Keeping all classes in one file for simplicity.
     * ----
     * I'd in some cases prefer this in production-ready
     * applications as well.
     */
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfoAttribute : Attribute {
        public FieldInfoAttribute(string header, int columnIndex, ValueFormatting formatting = ValueFormatting.NoFormatting) {
            Header = header;
            ColumnIndex = columnIndex;
            Formatting = formatting;
        }

        public string Header { get; }
        public int ColumnIndex { get; }
        public ValueFormatting Formatting { get; }

        public string Format(object context) => Formatting switch {
            ValueFormatting.EscapeQuotes when context is string value => $"\"{value.Replace("\"", "\"\"")}\"",
            ValueFormatting.DateDDMMYYYY when context is DateTimeOffset date => date.ToString("MM/dd/yyyy"),
            ValueFormatting.NoFormatting => context.ToString(),
            _ => context?.ToString() ?? string.Empty
        };
    }

    public enum ValueFormatting {
        EscapeQuotes,
        DateDDMMYYYY,
        NoFormatting
    }

    public abstract record StorageBase {
        public static Dictionary<PropertyInfo, FieldInfoAttribute> GetFieldValues<T>()
            where T : StorageBase =>
            GetFieldInfo<T>()
                .ToDictionary(x => x.property, x => x.field);

        private static IEnumerable<(PropertyInfo property, FieldInfoAttribute field)> GetFieldInfo<T>() => typeof(T)
            .GetProperties()
            .Where(prop => prop.GetCustomAttribute<FieldInfoAttribute>() is not null)
            .Select(prop => (prop, prop.GetCustomAttribute<FieldInfoAttribute>()));
    }

    public class CsvFormatter {
        public string Generate<T>(IEnumerable<T> rows) where T : StorageBase {
            Dictionary<PropertyInfo, FieldInfoAttribute> fieldInfo = StorageBase.GetFieldValues<T>();

            var stringBuilder = new StringBuilder();

            // Generate header
            stringBuilder.AppendLine(string.Join(",", fieldInfo.Select(info => info.Value.Header)));

            // Generate rows
            foreach (T row in rows) {
                foreach ((PropertyInfo key, FieldInfoAttribute value) in fieldInfo) {
                    stringBuilder.Append(value.Format(key.GetValue(row)));
                    if (fieldInfo.Last().Key != key) stringBuilder.Append(',');
                }

                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }

    public record TransactionRow : StorageBase {
        [FieldInfo("AMOUNT", 1)] public decimal Amount { get; init; }

        [FieldInfo("TRANSACTION_DATE", 2, ValueFormatting.DateDDMMYYYY)]
        public DateTimeOffset TransactionDate { get; init; }

        [FieldInfo("VALUE_DATE", 3, ValueFormatting.DateDDMMYYYY)]
        public DateTimeOffset ValueDate { get; init; }

        [FieldInfo("CREDIT_DEBIT_INDICATOR", 4)]
        public CreditDebitIndicator CreditDebitIndicator { get; init; }

        [FieldInfo("PAYMENT_DETAILS", 5, ValueFormatting.EscapeQuotes)]
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
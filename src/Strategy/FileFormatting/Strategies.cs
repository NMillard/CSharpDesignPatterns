using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DesignPatterns.Shared.Domain;
using Newtonsoft.Json;

/*
 * I'm keeping everything in one file for simplicity.
 */

namespace FileFormatting {

    /// <summary>
    /// Convert <see cref="TransactionHistory"/> into a storable file format.
    /// </summary>
    public interface ITransactionsFormatter {
        public string Convert(TransactionHistory history);
    }

    [FormatterFor(FileType.Json)]
    public class JsonFormatter : ITransactionsFormatter {
        public string Convert(TransactionHistory history) => JsonConvert.SerializeObject(history);
    }

    [FormatterFor(FileType.Csv)]
    public class CsvFormatter : ITransactionsFormatter {
        private readonly StringBuilder stringBuilder = new();
        private TransactionHistory transactionHistory;
        
        public string Convert(TransactionHistory history) {
            transactionHistory = history;
            
            GenerateHeaders();
            GenerateRows();

            return stringBuilder.ToString();
        }
        
        private void GenerateHeaders() => stringBuilder.AppendLine(string.Join(",", typeof(Transaction).GetProperties().Select(prop => prop.Name)));

        private void GenerateRows() {
            // We'll just create CSV this way for simplicity.
            foreach (Transaction transaction in transactionHistory.Transactions) {
                stringBuilder.AppendLine(
                    $"{transaction.TransactionDate.ToString()}," +
                    $"{transaction.ValueDate.ToString()}," +
                    $"{transaction.CreditDebitIndicator:G}," +
                    $"{transaction.Amount:F1}");
            }
        }
    }

    public static class FormatterSelectionExtension {
        public static ITransactionsFormatter PickFormatter(this IEnumerable<ITransactionsFormatter> formatters, FileType fileType) => formatters
            .SingleOrDefault(formatter => formatter.GetType().GetCustomAttribute<FormatterForAttribute>()?.FileType == fileType);
    }
    
    /// <summary>
    /// Specify the file format a <see cref="ITransactionsFormatter"/> can convert to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FormatterForAttribute : Attribute {
        public FormatterForAttribute(FileType fileType) {
            FileType = fileType;
        }

        public FileType FileType { get; }
    }

    /// <summary>
    /// Available formats
    /// </summary>
    public enum FileType {
        Json = 0,
        Csv,
    }

    public class FormatUsingTraditionalBranching {
        public string Format(TransactionHistory history, FileType fileType) {
            string result = string.Empty;
            
            if (fileType == FileType.Csv) {
                var stringBuilder = new StringBuilder();
                
                // Generate header
                stringBuilder.AppendLine(string.Join(",", typeof(Transaction).GetProperties().Select(prop => prop.Name)));
                
                // Generate rows
                foreach (Transaction transaction in history.Transactions) {
                    var row = @$"{transaction.TransactionDate.ToString()},
                              {transaction.ValueDate.ToString()},
                              {transaction.CreditDebitIndicator:G},
                              {transaction.Amount:F1}";
                    stringBuilder.AppendLine(row);
                }

                result = stringBuilder.ToString();
            } else if (fileType == FileType.Json) {
                result = JsonConvert.SerializeObject(history.Transactions);
            }

            return result;
        }
    }
}
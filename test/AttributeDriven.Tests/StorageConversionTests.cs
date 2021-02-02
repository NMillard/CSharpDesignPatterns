using System;
using System.Collections.Generic;
using DesignPatterns.Shared.Domain;
using StorageFormatting;
using Xunit;
using Xunit.Abstractions;

namespace AttributeDriven.Tests {
    public class StorageConversionTests {
        private readonly ITestOutputHelper testOutputHelper;
        public StorageConversionTests(ITestOutputHelper testOutputHelper) {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CanGenerateCsvContent() {
            var transaction = new TransactionRow() {
                Amount = 100m,
                PaymentDetails = "Details",
                TransactionDate = DateTimeOffset.Now,
                ValueDate = DateTimeOffset.Now.AddDays(2),
                CreditDebitIndicator = CreditDebitIndicator.Credit
            };
        }

        [Fact]
        public void GenerateCsvContent() {
            var formatter = new CsvFormatter();

            var transactions = new List<TransactionRow> {
                new() {
                    Amount = 100m,
                    PaymentDetails = "Details",
                    TransactionDate = DateTimeOffset.Now,
                    ValueDate = DateTimeOffset.Now.AddDays(2),
                    CreditDebitIndicator = CreditDebitIndicator.Credit
                },
                new() {
                    Amount = 120.3m,
                    TransactionDate = DateTimeOffset.Now.AddDays(4),
                    ValueDate = DateTimeOffset.Now.AddDays(4),
                    CreditDebitIndicator = CreditDebitIndicator.Debit
                },
                new() {
                    Amount = 5313.61m,
                    PaymentDetails = "More \"details\"...",
                    TransactionDate = DateTimeOffset.Now.AddDays(1),
                    ValueDate = DateTimeOffset.Now.AddDays(6),
                    CreditDebitIndicator = CreditDebitIndicator.Credit
                },
            };

            string csvResult = formatter.Generate(transactions);
            testOutputHelper.WriteLine(csvResult);
        }

        [Fact]
        public void testName() {
            var csv = new CsvFormatter();
            
            var transactions = new List<TransactionRow> {
                new() {
                    Amount = 100m,
                    PaymentDetails = "Details",
                    TransactionDate = DateTimeOffset.Now,
                    ValueDate = DateTimeOffset.Now.AddDays(2),
                    CreditDebitIndicator = CreditDebitIndicator.Credit
                },
                new() {
                    Amount = 120.3m,
                    TransactionDate = DateTimeOffset.Now.AddDays(4),
                    ValueDate = DateTimeOffset.Now.AddDays(4),
                    CreditDebitIndicator = CreditDebitIndicator.Debit
                },
                new() {
                    Amount = 5313.61m,
                    PaymentDetails = "More \"details\"...",
                    TransactionDate = DateTimeOffset.Now.AddDays(1),
                    ValueDate = DateTimeOffset.Now.AddDays(6),
                    CreditDebitIndicator = CreditDebitIndicator.Credit
                },
            };

            string e = csv.Generate2(transactions);
            testOutputHelper.WriteLine(e);
        }
    }
}
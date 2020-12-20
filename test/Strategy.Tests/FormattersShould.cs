using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesignPatterns.Shared.Domain;
using FileFormatting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

/*
 *  These tests are not really meant to act as true tests, that verifies functionality.
 *  ----
 *  They are simply meant to act as lots of runnable programs.
 *  Think of each test as a console app. Try to step thru them with your debugger.
 */

namespace Strategy.Tests {
    public class UnitTest1 {
        
        [Theory]
        [InlineData(FileType.Json)]
        [InlineData(FileType.Csv)]
        public void PickStrategyFromList(FileType fileType) {
            /*
             * Using applied attributes thru the extension method.
             */

            
            // Arrange
            var strategies = new List<IFileFormatter> {
                new CsvFormatter(),
                new JsonFormatter()
            };
            
            var history = new TransactionHistory(new List<Transaction> {
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Credit, 100),
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Debit, 10)
            });

            IFileFormatter formatter = strategies.PickFormatter(fileType);

            // Act
            string result = formatter.Convert(history);
        }

        [Theory]
        [InlineData("csv")]
        [InlineData("json")]
        public void PickStrategyFromDictionary(string fileType) {
            /*
             * Not using attributes or extension methods
             */

            
            // Arrange
            var strategyDictionary = new Dictionary<string, IFileFormatter> { // Use dependency injection in real-world applications.
                {"csv", new CsvFormatter()},
                {"json", new JsonFormatter()}
            };
            
            var history = new TransactionHistory(new List<Transaction> {
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Credit, 100),
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Debit, 10)
            });
            

            IFileFormatter formatter = strategyDictionary[fileType];

            // Act
            string result = formatter.Convert(history);
        }
        
        [Fact]
        public void TransformHistoryToCsv() {
            // Arrange
            var sut = new CsvFormatter();
            var history = new TransactionHistory(new List<Transaction> {
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Credit, 100),
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Debit, 10)
            });

            // Act
            string result = sut.Convert(history);
        }

        [Fact]
        public void TransformHistoryToJson() {
            // Arrange
            var sut = new JsonFormatter();
            
            var history = new TransactionHistory(new List<Transaction> {
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Credit, 100),
                new(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1), CreditDebitIndicator.Debit, 10)
            });

            // Act
            string result = sut.Convert(history);

            // Assert
            JObject jsonObject = JObject.Load(new JsonTextReader(new StringReader(result)));
            
            Assert.NotNull(jsonObject["Transactions"]);
            Assert.True(jsonObject["Transactions"].Count() == history.Transactions.Count());
        }
        
        [Theory]
        [InlineData(typeof(CsvFormatter))]
        [InlineData(typeof(JsonFormatter))]
        public void InjectFormattersIntoDependencyContainer(Type implementationType) {
            // Arrange
            var services = new ServiceCollection();
            
            // Act
            services.AddFileFormatters();
            
            // Assert
            bool hasType = services.Any(t => t.ServiceType == typeof(IFileFormatter) && t.ImplementationType == implementationType);
            Assert.True(hasType);
        }

        [Theory]
        [InlineData(FileType.Json, typeof(JsonFormatter))]
        [InlineData(FileType.Csv, typeof(CsvFormatter))]
        public void PickFormatterFromInjectedServices(FileType fileType, Type type) {
            // Arrange
            var services = new ServiceCollection();
            services.AddFileFormatters();
            
            IEnumerable<IFileFormatter> formatters = services.BuildServiceProvider().GetServices <IFileFormatter>();

            // Act
            IFileFormatter formatter = formatters.PickFormatter(fileType);
            
            Assert.IsType(type, formatter);
        }
    }
}
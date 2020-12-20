using System.Collections.Generic;

namespace DesignPatterns.Shared.Domain {
    public class TransactionHistory {
        public TransactionHistory(IEnumerable<Transaction> transactions) {
            Transactions = transactions;
        }
        
        public IEnumerable<Transaction> Transactions { get; }
    }
}
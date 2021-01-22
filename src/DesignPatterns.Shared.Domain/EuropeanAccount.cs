using System;

namespace DesignPatterns.Shared.Domain {
    public class EuropeanAccount {
        private readonly Guid id;

        public EuropeanAccount() {
            id = Guid.NewGuid();
        }

        /// <summary>
        /// Get or set the account IBAN. 
        /// </summary>
        public string Iban { get; set; }
    }
}
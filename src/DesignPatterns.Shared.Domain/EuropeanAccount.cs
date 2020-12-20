using System;

namespace DesignPatterns.Shared.Domain {
    public class EuropeanAccount {
        private readonly Guid id;

        public EuropeanAccount() {
            id = Guid.NewGuid();
        }

        public string Iban { get; }
    }
}
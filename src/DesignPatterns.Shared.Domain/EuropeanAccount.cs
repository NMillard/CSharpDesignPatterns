using System;

namespace DesignPatterns.Shared.Domain {
    public class EuropeanAccount {
        private readonly Guid id;

        public EuropeanAccount(Iban iban) {
            id = Guid.NewGuid();

            Iban = iban;
        }

        /// <summary>
        /// Get or set the account IBAN. 
        /// </summary>
        public Iban Iban { get; private set; }
    }

    public class Iban {
        private readonly string plainIban;

        private Iban(string iban) {
            if (!Validate(iban)) throw new ArgumentException();
            plainIban = iban;
        }

        private bool Validate(string ibanToValidate) {
            // validation (check length, validate check digits and sum, etc.)
            return true;
        }
        
        public static implicit operator Iban(string iban) {
            return new Iban(iban);
        }
    }
}
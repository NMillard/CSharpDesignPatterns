using System;

namespace DesignPatterns.Shared.Domain {
    public class User {

        // Terrible property - this can be set to anything without reason
        public string Username { get; set; }

        
        // slightly better, but requires additional field
        // and we still wouldn't know why "username2" changed
        private string username2;
        public string Username2 {
            get => username2;
            set {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Must have a value");
                if (value.Length > 50) throw new ArgumentException("Too long");
                username2 = value;
            }
        }

        public string Username3 { get; private set; }

        public void ChangeUsername(string newUsername) {
            ValidateUsername(newUsername);
            // (maybe) add some additional logic
            Username3 = newUsername;
        }
        
        private void ValidateUsername(string username) {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Must have a value");
            if (username.Length > 50) throw new ArgumentException("Too long");
        }
    }
}
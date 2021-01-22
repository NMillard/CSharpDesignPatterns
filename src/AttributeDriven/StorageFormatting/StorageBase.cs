using System;

namespace StorageFormatting {
    /**
     * Keeping all classes in one file for simplicity.
     * ----
     * I'd in some cases prefer this in production-ready
     * applications as well.
     */

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfo : Attribute {
        public string Header { get; set; }
    }
    
    public abstract class StorageBase {
        
    }

    public class TransactionRow {
        
    }
}
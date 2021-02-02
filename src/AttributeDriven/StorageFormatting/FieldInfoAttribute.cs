using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            ValueFormatting.DateDDMMYYYY when context is DateTimeOffset date => date.ToString("yyyy/MM/dd"),
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
        public static IEnumerable<(PropertyInfo property, FieldInfoAttribute field)> GetFieldInfo<T>() => typeof(T)
            .GetProperties()
            .Where(prop => prop.GetCustomAttribute<FieldInfoAttribute>() is not null)
            .Select(prop => (prop, prop.GetCustomAttribute<FieldInfoAttribute>()));

        public static IEnumerable<(PropertyInfo prop, Field? field)> GetFields<T>() => typeof(T)
            .GetProperties()
            .Where(prop => prop.GetCustomAttribute<Field>() is not null)
            .Select(prop => (prop, prop.GetCustomAttribute<Field>()));
    }
}
using System;
using System.Globalization;

namespace StorageFormatting {
    [AttributeUsage(AttributeTargets.Property)]
    public class Field : Attribute {
        public Field(string header, int columnIndex, Type converter = null) {
            Converter = converter ?? typeof(DefaultConverter);
            if (!typeof(ValueConverter).IsAssignableFrom(Converter))
                throw new ArgumentException($"{nameof(converter)} must be a sub-type of {nameof(ValueConverter)}");
            
            Header = header;
            ColumnIndex = columnIndex;
        }

        public string Header { get; }
        public int ColumnIndex { get; }
        public Type Converter { get; }

        public string Format(object context) {
            var converter = Activator.CreateInstance(Converter) as ValueConverter;
            if (converter is null) throw new InvalidOperationException("Converter must have an empty constructor");

            return converter.Convert(context) ?? string.Empty;
        }
    }

    // ReSharper disable once InconsistentNaming
    public class DateToYYYYMMDD : ValueConverter {
        public override string Convert(object context) {
            if (context is not DateTimeOffset date) throw new ArgumentException();
            return date.ToString("yyyy/MM/dd");
        }
    }

    public class DefaultConverter : ValueConverter {
        public override string? Convert(object? context) => context switch {
            string => context.ToString(),
            decimal value => value.ToString("F2", CultureInfo.InvariantCulture),
            _ => context?.ToString()
        };
    }

    public class EscapeQuotesConverter : ValueConverter {
        public override string? Convert(object? context) => $"\"{context?.ToString()?.Replace("\"", "\"\"")}\"";
    }

    public abstract class ValueConverter {
        public abstract string? Convert(object? context);
    }
}
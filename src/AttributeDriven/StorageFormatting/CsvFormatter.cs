using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StorageFormatting {
    public class CsvFormatter {

        public string Generate2<T>(IEnumerable<T> rows) where T : StorageBase {
            var fields = StorageBase.GetFields<T>().ToArray();
            
            var stringBuilder = new StringBuilder();

            // Generate header
            stringBuilder.AppendLine(string.Join(",", fields.Select(info => info.field.Header)));
            
            PropertyInfo lastProperty = fields.Last().prop;
            foreach (T row in rows) {
                foreach ((PropertyInfo currentProperty, Field? field) in StorageBase.GetFields<T>()) {
                    stringBuilder.Append(field.Format(currentProperty.GetValue(row)));
                    if (lastProperty != currentProperty) stringBuilder.Append(',');
                }
                stringBuilder.Append(Environment.NewLine);
            }
            
            return stringBuilder.ToString();
        }
        
        public string Generate<T>(IEnumerable<T> rows) where T : StorageBase {
            var fieldInfo = StorageBase.GetFieldInfo<T>().ToArray();

            var stringBuilder = new StringBuilder();

            // Generate header
            stringBuilder.AppendLine(string.Join(",", fieldInfo.Select(info => info.field.Header)));

            // Generate rows
            PropertyInfo lastProperty = fieldInfo.Last().property;
            foreach (T row in rows) {
                foreach ((PropertyInfo currentProperty, FieldInfoAttribute field) in fieldInfo) {
                    stringBuilder.Append(field.Format(currentProperty.GetValue(row)));
                    if (lastProperty != currentProperty) stringBuilder.Append(',');
                }

                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }
    }
}
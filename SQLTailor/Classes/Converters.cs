using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SQLTailor.Classes {

    public class DatabaseTypeToInt : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            DatabaseType? type = value as DatabaseType?;
            int returnValue;

            if (type != null) {
                returnValue = (int)type;
            } else {
                returnValue = 0;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int returnValue = (int)value;

            return (DatabaseType)returnValue; 
        }
    }

    public class FluentSQLTypeToInt : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            FluentSQLType? type = value as FluentSQLType?;
            int returnValue;

            if (type != null) {
                returnValue = (int)type;
            } else {
                returnValue = 0;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int returnValue = (int)value;

            return (FluentSQLType)returnValue;
        }
    }

}

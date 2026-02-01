using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AРМ_Библиотекаря
{
    public static class BasicFunction
    {
        public static DataTable Select(string selectSQL)
        {
            DataTable dataTable = new DataTable("dataBase");
            SqlConnection sqlConnection = new SqlConnection("server=(localdb)\\MSSQLLocalDB;Trusted_Connection=Yes; DataBase=Librarian_Workplace; TransparentNetworkIPResolution = False; Connect Timeout=30; Max Pool Size=400");
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public static bool validateName(string name)
        {
            if (name.Length == 0) return false;
            name = name.ToLower();
            char[] letters =
                {
                'а', 'б', 'в', 'г', 'д','е', 'ё', 'ж', 'з','и', 'й','к', 'л', 'м','н', 
                'о', 'п', 'р','с', 'т', 'у', 'ф','х', 'ц', 'ч', 'ш','щ', 'ъ', 'ь', 'ы',
                'э', 'ю', 'я', ' '
                };
            foreach (char ch in name)
            
                if (Array.IndexOf(letters, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }

        public static bool validateNumber(string numeric)
        {
            if (numeric.Length == 0) return false;
            numeric = numeric.ToLower();
            char[] numbers =
                {
                '0', '1', '2', '3', '4', '5','6','7','8','9'
                };
            foreach (char ch in numeric)

                if (Array.IndexOf(numbers, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }

        public static bool validateSymbols(string symbol)
        {
            if (symbol.Length == 0) return false;
            symbol = symbol.ToLower();
            char[] symbols =
            {
                '@', '$', '^', '!', '"', '№', ';', ':', '&', '*', '(', ')', 
                '-', '_', '+', '=', '{', '}', '>', '<', '[', ']', '.', '?', 
                ',', '|', '/', '#', '%'
            };
            foreach (char ch in symbol)

                if (Array.IndexOf(symbols, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }

        public static bool validateBirthday(string daySymbols)
        {
            if (daySymbols.Length == 0) return false;
            daySymbols = daySymbols.ToLower();
            char[] symbols =
            {
                '0', '1', '2', '3', '4', '5','6','7','8','9', '-'
            };
            foreach (char ch in daySymbols)

                if (Array.IndexOf(symbols, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }

        public static bool validateAddress(string addressSymbol)
        {
            if (addressSymbol.Length == 0) return false;
            addressSymbol = addressSymbol.ToLower();
            char[] letters =
                {
                'а', 'б', 'в', 'г', 'д','е', 'ё', 'ж', 'з','и', 'й','к', 'л', 'м','н',
                'о', 'п', 'р','с', 'т', 'у', 'ф','х', 'ц', 'ч', 'ш','щ', 'ъ', 'ь', 'ы',
                'э', 'ю', 'я', ' ', '.', '/', '№', '0', '1', '2', '3', '4', '5','6','7','8','9'
                };
            foreach (char ch in addressSymbol)

                if (Array.IndexOf(letters, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }

        public static bool validateBookName(string addressSymbol)
        {
            if (addressSymbol.Length == 0) return false;
            addressSymbol = addressSymbol.ToLower();
            char[] letters =
                {
                'а', 'б', 'в', 'г', 'д','е', 'ё', 'ж', 'з','и', 'й','к', 'л', 'м','н',
                'о', 'п', 'р','с', 'т', 'у', 'ф','х', 'ц', 'ч', 'ш','щ', 'ъ', 'ь', 'ы',
                'э', 'ю', 'я', ' ', '.', '№', ',', '(', ')', '!', '?', '-', '_', ':', '&',
                '+', '=', '0', '1', '2', '3', '4', '5','6','7','8','9'
                };
            foreach (char ch in addressSymbol)

                if (Array.IndexOf(letters, ch, 0) < 0)
                {
                    return false;
                }
            return true;
        }
    }
}

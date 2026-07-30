using System;
using System.Globalization;

namespace IncomeTaxCalculatorApp.Helpers
{
    public static class ConsoleUtils
    {
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();
            return input?.Trim() ?? string.Empty;
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Value required. Please enter a number.");
                    continue;
                }

                if (decimal.TryParse(input.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var value))
                {
                    return value;
                }

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(intercept: true);
        }

        public static string FormatCurrency(decimal amount)
        {
            return amount.ToString("F2", CultureInfo.CurrentCulture);
        }
    }
}

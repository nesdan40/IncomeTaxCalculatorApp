using System;
using IncomeTaxCalculatorApp.Models;
using IncomeTaxCalculatorApp.Services;
using IncomeTaxCalculatorApp.Helpers;

namespace IncomeTaxCalculatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaxPayer? lastTaxPayer = null;

            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("=== Income Tax Calculator ===\n");
                Console.WriteLine("1. Calculate income tax");
                Console.WriteLine("2. View tax summary");
                Console.WriteLine("3. Clear current input");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                var choice = ConsoleUtils.ReadString("Select an option (1-4): ");

                switch (choice.Trim())
                {
                    case "1":
                        lastTaxPayer = ApplicationService.CalculateIncomeTax();
                        break;
                    case "2":
                        ApplicationService.ShowSummary(lastTaxPayer);
                        break;
                    case "3":
                        if (lastTaxPayer == null)
                        {
                            ApplicationService.ShowNoInputToClear();
                        }
                        else
                        {
                            lastTaxPayer = null;
                            ApplicationService.ShowClearedMessage();
                        }
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select 1-4.");
                        ConsoleUtils.PressAnyKeyToContinue();
                        break;
                }

            } while (!exit);
        }
    }
}

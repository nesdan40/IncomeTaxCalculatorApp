using System;
using IncomeTaxCalculatorApp.Models;
using IncomeTaxCalculatorApp.Helpers;
using IncomeTaxCalculatorApp.Services;

namespace IncomeTaxCalculatorApp.Services
{
    public static class ApplicationService
    {
        private const string TaxSlabInfo =
            "Tax slabs (developer-defined):\n" +
            "  0 - 250,000 : 0%\n" +
            "  250,001 - 500,000 : 5% on portion above 250,000\n" +
            "  500,001 - 1,000,000 : 20% on portion above 500,000\n" +
            "  > 1,000,000 : 30% on portion above 1,000,000\n";

        /// <summary>
        /// Runs the Calculate Income Tax flow: collect inputs, validate, compute and display summary.
        /// Returns the computed TaxPayer instance.
        /// </summary>
        public static TaxPayer CalculateIncomeTax()
        {
            Console.Clear();
            Console.WriteLine("=== Calculate Income Tax ===\n");
            Console.WriteLine(TaxSlabInfo);
            Console.WriteLine();

            // Initial collection (parseable values only)
            var name = ConsoleUtils.ReadString("Taxpayer Name: ");
            var pan = ConsoleUtils.ReadString("PAN Number: ");
            var financialYear = ConsoleUtils.ReadString("Financial Year: ");
            var annualIncome = ConsoleUtils.ReadDecimal("Annual Income: ");
            var deductions = ConsoleUtils.ReadDecimal("Eligible Tax Deduction Amount: ");

            // Validate after collection and re-prompt only invalid fields
            while (true)
            {
                var errors = TaxService.ValidateInputs(name, pan, financialYear, annualIncome, deductions);
                if (errors.Count == 0) break;

                Console.Clear();
                Console.WriteLine("=== Input validation errors ===\n");
                foreach (var kv in errors)
                {
                    Console.WriteLine($"{kv.Key}: {kv.Value}");
                }
                Console.WriteLine();

                // Re-prompt only invalid fields
                foreach (var field in errors.Keys)
                {
                    switch (field.ToLowerInvariant())
                    {
                        case "name":
                            name = ConsoleUtils.ReadString("Taxpayer Name: ");
                            break;
                        case "pan":
                            pan = ConsoleUtils.ReadString("PAN Number: ");
                            break;
                        case "financialyear":
                            financialYear = ConsoleUtils.ReadString("Financial Year: ");
                            break;
                        case "annualincome":
                            annualIncome = ConsoleUtils.ReadDecimal("Annual Income: ");
                            break;
                        case "deductions":
                            deductions = ConsoleUtils.ReadDecimal("Eligible Tax Deduction Amount: ");
                            break;
                        default:
                            // unknown field - ignore
                            break;
                    }
                }
            }

            // All validated — perform calculations
            var taxableIncome = TaxService.CalculateTaxableIncome(annualIncome, deductions);
            var taxBefore = TaxService.CalculateTaxBySlabs(annualIncome);
            var taxAfter = TaxService.CalculateTaxBySlabs(taxableIncome);
            var effective = TaxService.ComputeEffectiveTaxPercentage(taxAfter, annualIncome);

            var tp = new TaxPayer
            {
                Name = name,
                PAN = pan.ToUpperInvariant(),
                FinancialYear = financialYear,
                AnnualIncome = annualIncome,
                Deductions = deductions,
                TaxableIncome = taxableIncome,
                TaxBeforeDeductions = taxBefore,
                TaxAfterDeductions = taxAfter,
                EffectiveTaxPercentage = effective
            };

            // Display summary
            Console.Clear();
            Console.WriteLine("=== Tax Calculation Summary ===\n");
            Console.WriteLine($"Taxpayer Name : {tp.Name}");
            Console.WriteLine($"PAN           : {tp.PAN}");
            Console.WriteLine($"Financial Year: {tp.FinancialYear}");
            Console.WriteLine($"\nAnnual Income : {ConsoleUtils.FormatCurrency(tp.AnnualIncome)}");
            Console.WriteLine($"Deductions    : {ConsoleUtils.FormatCurrency(tp.Deductions)}");
            Console.WriteLine($"Taxable Income: {ConsoleUtils.FormatCurrency(tp.TaxableIncome)}");
            Console.WriteLine($"Income Tax    : {ConsoleUtils.FormatCurrency(tp.TaxAfterDeductions)}");
            Console.WriteLine($"Effective Tax : {tp.EffectiveTaxPercentage:F2}%\n");

            Console.WriteLine("Comparison before/after deductions:");
            Console.WriteLine($"Tax on annual income (no deductions): {ConsoleUtils.FormatCurrency(tp.TaxBeforeDeductions)}");
            Console.WriteLine($"Tax after deductions                 : {ConsoleUtils.FormatCurrency(tp.TaxAfterDeductions)}");

            ConsoleUtils.PressAnyKeyToContinue();

            return tp;
        }

        public static void ShowSummary(TaxPayer? tp)
        {
            Console.Clear();
            Console.WriteLine("=== Tax Summary ===\n");
            if (tp == null)
            {
                Console.WriteLine("No tax calculation available. Please calculate income tax first (Option 1).");
            }
            else
            {
                Console.WriteLine($"Taxpayer Name : {tp.Name}");
                Console.WriteLine($"PAN           : {tp.PAN}");
                Console.WriteLine($"Financial Year: {tp.FinancialYear}");
                Console.WriteLine($"\nAnnual Income : {ConsoleUtils.FormatCurrency(tp.AnnualIncome)}");
                Console.WriteLine($"Deductions    : {ConsoleUtils.FormatCurrency(tp.Deductions)}");
                Console.WriteLine($"Taxable Income: {ConsoleUtils.FormatCurrency(tp.TaxableIncome)}");
                Console.WriteLine($"Income Tax    : {ConsoleUtils.FormatCurrency(tp.TaxAfterDeductions)}");
                Console.WriteLine($"Effective Tax : {tp.EffectiveTaxPercentage:F2}%\n");
                Console.WriteLine("Comparison before/after deductions:");
                Console.WriteLine($"Tax on annual income (no deductions): {ConsoleUtils.FormatCurrency(tp.TaxBeforeDeductions)}");
                Console.WriteLine($"Tax after deductions                 : {ConsoleUtils.FormatCurrency(tp.TaxAfterDeductions)}");
            }

            ConsoleUtils.PressAnyKeyToContinue();
        }

        public static void ShowClearedMessage()
        {
            Console.Clear();
            Console.WriteLine("Current input cleared.");
            ConsoleUtils.PressAnyKeyToContinue();
        }

        public static void ShowNoInputToClear()
        {
            Console.Clear();
            Console.WriteLine("No current taxpayer details to clear.");
            ConsoleUtils.PressAnyKeyToContinue();
        }
    }
}

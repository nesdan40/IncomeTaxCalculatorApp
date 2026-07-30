using System;
using System.Collections.Generic;
using System.Globalization;

namespace IncomeTaxCalculatorApp.Services
{
    /// <summary>
    /// Provides tax calculation and validation logic.
    /// Tax slabs used (documented and applied):
    /// - 0 - 250,000 : 0%
    /// - 250,001 - 500,000 : 5% on portion above 250,000
    /// - 500,001 - 1,000,000 : 20% on portion above 500,000
    /// - > 1,000,000 : 30% on portion above 1,000,000
    /// </summary>
    public static class TaxService
    {
        private const decimal Slab1Limit = 250_000m;
        private const decimal Slab2Limit = 500_000m;
        private const decimal Slab3Limit = 1_000_000m;

        public static Dictionary<string, string> ValidateInputs(string name, string pan, string financialYear, decimal annualIncome, decimal deductions)
        {
            var errors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(name))
            {
                errors[nameof(name)] = "Taxpayer name is required.";
            }
            else
            {
                // Name should contain only letters and spaces
                var trimmed = name.Trim();
                if (!System.Text.RegularExpressions.Regex.IsMatch(trimmed, "^[A-Za-z ]+$"))
                {
                    errors[nameof(name)] = "Taxpayer name must contain only alphabetic characters and spaces.";
                }
            }

            if (string.IsNullOrWhiteSpace(financialYear))
            {
                errors[nameof(financialYear)] = "Financial Year is required.";
            }
            else
            {
                // basic pattern: either YYYY or YYYY-YY (e.g., 2025 or 2025-26)
                if (!System.Text.RegularExpressions.Regex.IsMatch(financialYear.Trim(), "^\\d{4}(-\\d{2})?$"))
                {
                    errors[nameof(financialYear)] = "Financial Year should be in format YYYY or YYYY-YY (e.g., 2025 or 2025-26).";
                }
            }

            if (string.IsNullOrWhiteSpace(pan))
            {
                errors[nameof(pan)] = "PAN is required.";
            }
            else
            {
                var normalizedPan = pan.Trim().ToUpperInvariant();
                if (!System.Text.RegularExpressions.Regex.IsMatch(normalizedPan, "^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                {
                    errors[nameof(pan)] = "PAN must follow pattern: 5 letters, 4 digits, 1 letter (example: ABCDE1234F).";
                }
            }

            if (annualIncome < 0)
            {
                errors[nameof(annualIncome)] = "Annual income cannot be negative.";
            }

            if (deductions < 0)
            {
                errors[nameof(deductions)] = "Deductions cannot be negative.";
            }

            if (deductions > annualIncome)
            {
                errors[nameof(deductions)] = "Deductions cannot exceed annual income.";
            }

            return errors;
        }

        public static decimal CalculateTaxableIncome(decimal annualIncome, decimal deductions)
        {
            var taxable = annualIncome - deductions;
            return taxable < 0 ? 0m : taxable;
        }

        public static decimal CalculateTaxBySlabs(decimal taxableIncome)
        {
            if (taxableIncome <= 0) return 0m;

            decimal tax = 0m;

            // slab 1: up to 250,000 — 0%
            if (taxableIncome <= Slab1Limit)
            {
                return 0m;
            }

            // slab 2: 250,001 - 500,000 : 5%
            if (taxableIncome > Slab1Limit)
            {
                var slab2Taxable = Math.Min(taxableIncome, Slab2Limit) - Slab1Limit;
                if (slab2Taxable > 0)
                {
                    tax += slab2Taxable * 0.05m;
                }
            }

            // slab 3: 500,001 - 1,000,000 : 20%
            if (taxableIncome > Slab2Limit)
            {
                var slab3Taxable = Math.Min(taxableIncome, Slab3Limit) - Slab2Limit;
                if (slab3Taxable > 0)
                {
                    tax += slab3Taxable * 0.20m;
                }
            }

            // slab 4: above 1,000,000 : 30%
            if (taxableIncome > Slab3Limit)
            {
                var slab4Taxable = taxableIncome - Slab3Limit;
                tax += slab4Taxable * 0.30m;
            }

            return decimal.Round(tax, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal ComputeEffectiveTaxPercentage(decimal taxAmount, decimal annualIncome)
        {
            if (annualIncome <= 0) return 0m;
            return decimal.Round((taxAmount / annualIncome) * 100m, 2, MidpointRounding.AwayFromZero);
        }

        public static (decimal taxBefore, decimal taxAfter) CompareTaxBeforeAfter(decimal annualIncome, decimal deductions)
        {
            var taxBefore = CalculateTaxBySlabs(annualIncome);
            var taxable = CalculateTaxableIncome(annualIncome, deductions);
            var taxAfter = CalculateTaxBySlabs(taxable);
            return (taxBefore, taxAfter);
        }
    }
}

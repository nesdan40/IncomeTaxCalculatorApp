using System;

namespace IncomeTaxCalculatorApp.Models
{
    public class TaxPayer
    {
        public string Name { get; set; } = string.Empty;
        public string PAN { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public decimal AnnualIncome { get; set; }
        public decimal Deductions { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal TaxBeforeDeductions { get; set; }
        public decimal TaxAfterDeductions { get; set; }
        public decimal EffectiveTaxPercentage { get; set; }
    }
}

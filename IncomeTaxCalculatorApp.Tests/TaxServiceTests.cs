using Microsoft.VisualStudio.TestTools.UnitTesting;
using IncomeTaxCalculatorApp.Services;

namespace IncomeTaxCalculatorApp.Tests
{
    [TestClass]
    public class TaxServiceTests
    {
        [TestMethod]
        public void CalculateTaxBySlabs_Boundaries_Works()
        {
            // 250,000 => 0
            Assert.AreEqual(0m, TaxService.CalculateTaxBySlabs(250_000m));

            // 300,000 => (300k-250k)*5% = 2,500
            Assert.AreEqual(2_500m, TaxService.CalculateTaxBySlabs(300_000m));

            // 750,000 => 250k@5% + 250k@20% = 12,500 + 50,000 = 62,500
            Assert.AreEqual(62_500m, TaxService.CalculateTaxBySlabs(750_000m));

            // 1,500,000 => 250k@5% + 500k@20% + 500k@30% = 12,500 + 100,000 + 150,000 = 262,500
            Assert.AreEqual(262_500m, TaxService.CalculateTaxBySlabs(1_500_000m));
        }

        [TestMethod]
        public void CompareTaxBeforeAfter_Works()
        {
            var (before, after) = TaxService.CompareTaxBeforeAfter(600_000m, 100_000m);
            // before: 600k => 250k@5% + 100k@20% = 12,500 + 20,000 = 32,500
            Assert.AreEqual(32_500m, before);
            // after: taxable = 500k => 250k@5% = 12,500
            Assert.AreEqual(12_500m, after);
        }

        [TestMethod]
        public void ValidateInputs_DetectsErrors()
        {
            var errors = TaxService.ValidateInputs("", "ABC", "", -100m, 200m);
            Assert.IsTrue(errors.ContainsKey("name"));
            Assert.IsTrue(errors.ContainsKey("financialYear"));
            Assert.IsTrue(errors.ContainsKey("pan"));
            Assert.IsTrue(errors.ContainsKey("annualIncome"));
            Assert.IsTrue(errors.ContainsKey("deductions"));
        }
    }
}

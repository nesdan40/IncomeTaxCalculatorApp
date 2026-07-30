# IncomeTaxCalculatorApp

Income Tax Calculator Console Application

Overview

Simple, menu-driven .NET 10 console application that estimates individual income tax using developer-defined slabs. Implements input collection, post-collection validation, slab-based tax computation, and a summary view. Includes basic unit tests for tax calculation logic.

Key features

- Calculate income tax (collects Name, PAN, Financial Year, Annual Income, Eligible Deductions).
- View last tax summary.
- Clear current input (with appropriate messaging if no input exists).
- Input validation (post-collection -- invalid fields are re-prompted only).
- Displays monetary values with two decimal places.
- Bonus: compares tax payable before and after applying deductions.

Tax slab (documented and displayed in the app)

- 0 - 250,000 : 0%
- 250,001 - 500,000 : 5% on portion above 250,000
- 500,001 - 1,000,000 : 20% on portion above 500,000
- Above 1,000,000 : 30% on portion above 1,000,000

Validation rules

- Name: required; alphabetic characters and spaces only (no digits or symbols).
- PAN: required; basic format: 5 letters, 4 digits, 1 letter (example: ABCDE1234F). This is a pattern check only.
- Financial Year: required; format YYYY or YYYY-YY (e.g., 2025 or 2025-26).
- Annual Income: must be a number >= 0.
- Deductions: must be a number >= 0 and <= Annual Income.

Project layout (important files)

- IncomeTaxCalculatorApp/Program.cs
  - Main menu loop (delegates to ApplicationService)
- IncomeTaxCalculatorApp/Services/ApplicationService.cs
  - UI flows: CalculateIncomeTax, ShowSummary, ShowClearedMessage, ShowNoInputToClear
- IncomeTaxCalculatorApp/Services/TaxService.cs
  - Business logic: validation and tax calculation
- IncomeTaxCalculatorApp/Helpers/ConsoleUtils.cs
  - Console input/format helpers
- IncomeTaxCalculatorApp/Models/TaxPayer.cs
  - DTO for input and computed values
- IncomeTaxCalculatorApp.Tests/
  - MSTest project with basic tests for TaxService

Requirements

- .NET 10 SDK (target framework net10.0)
- No external libraries used

Build and run

- Build solution:
  dotnet build

- Run the console app from the project folder:
  dotnet run --project IncomeTaxCalculatorApp/IncomeTaxCalculatorApp.csproj

Usage notes

- The app clears the console when entering each menu option for a clean UI.
- Option 1 (Calculate income tax) shows the static tax slab table, collects all inputs, validates them together, and re-prompts only invalid fields until all inputs are valid.
- All outputs use two decimal places for monetary values.
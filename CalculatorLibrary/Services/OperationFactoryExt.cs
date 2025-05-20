using CalculatorLibrary.Interfacs;
using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Services
{
    public class OperationFactoryExt : IOperationFactoryExt
    {
        public OperationBase CreateOperation(string type, double op1, double op2)
        {
            return type.ToLower() switch
            {
                "add" => new AddOperation { Operand1 = op1, Operand2 = op2 },
                "subtract" => new SubtractOperation { Operand1 = op1, Operand2 = op2 },
                "multiply" => new MultiplyOperation { Operand1 = op1, Operand2 = op2 },
                "divide" => new DivideOperation { Operand1 = op1, Operand2 = op2 },
                _ => throw new InvalidOperationException("Unknown operation type")
            };
        }
    }

}

public static class OperationFactoryV1Extensions
{
    /// <summary>
    /// Solves an arithmetic equation string using DataTable.Compute.
    /// Supports +, -, *, /, and parentheses. Example: "2+3 + (4 * 5)"
    /// </summary>
    /// <param name="factory">The OperationFactory instance (not used, for extension method syntax).</param>
    /// <param name="equation">The equation string.</param>
    /// <returns>The result of the equation.</returns>
    public static double SolveEquationC(this OperationFactoryExt factory, string equation)
    {
        if (string.IsNullOrWhiteSpace(equation))
            throw new ArgumentException("Equation cannot be null or empty.", nameof(equation));

        // Use DataTable.Compute to evaluate the arithmetic expression
        var dt = new DataTable();
        object? result = dt.Compute(equation, string.Empty);

        if (result is null)
            throw new FormatException("Could not compute the equation.");

        // DataTable.Compute returns object, so convert to double
        if (double.TryParse(result.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            return value;

        throw new FormatException("Result is not a valid number.");
    }
}


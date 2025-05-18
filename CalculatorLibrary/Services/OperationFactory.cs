using CalculatorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Services
{
    public static class OperationFactory
    {
        public static OperationBase CreateOperation(string type, double op1, double op2)
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

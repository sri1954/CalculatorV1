using CalculatorLibrary.Interfacs;

namespace CalculatorLibrary.Models
{
    public abstract class OperationBase : IOperation
    {
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }

        public abstract double Execute();
       
    }
}

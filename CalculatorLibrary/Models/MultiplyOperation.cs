namespace CalculatorLibrary.Models
{
    public class MultiplyOperation: OperationBase
    {
        public override double Execute() => Operand1 * Operand2;
    }
}

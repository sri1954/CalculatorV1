namespace CalculatorLibrary.Models
{
    public class SubtractOperation: OperationBase
    {
        public override double Execute() => Operand1 - Operand2;
    }
}

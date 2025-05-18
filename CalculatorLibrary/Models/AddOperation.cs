namespace CalculatorLibrary.Models
{
    public class AddOperation : OperationBase
    {
        public override double Execute() => Operand1 + Operand2;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Models
{
    public class DivideOperation: OperationBase
    {
        public override double Execute() => Operand2 != 0 ? Operand1 / Operand2 : throw new DivideByZeroException();
    }
}

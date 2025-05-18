using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Models
{
    public class SubtractOperation: OperationBase
    {
        public override double Execute() => Operand1 - Operand2;
    }
}

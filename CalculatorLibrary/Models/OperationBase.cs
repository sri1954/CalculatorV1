using CalculatorLibrary.Interfacs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Models
{
    public abstract class OperationBase : IOperation
    {
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }

        public abstract double Execute();
       
    }
}

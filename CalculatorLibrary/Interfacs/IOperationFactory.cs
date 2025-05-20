using CalculatorLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Interfacs
{
    public interface IOperationFactory
    {
        OperationBase CreateOperation(string type, double op1, double op2);
    }
}

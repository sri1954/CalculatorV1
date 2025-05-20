using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Interfacs
{
    public interface IExpressionEvaluator
    {
        double Evaluate(string expression);
    }
}

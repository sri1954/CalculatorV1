using CalculatorLibrary.Interfacs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary.Models
{
    public class ExpressionEvaluator: IExpressionEvaluator
    {
        private int index;
        private string expression = string.Empty;

        public double Evaluate(string expr)
        {
            expression = expr.Replace(" ", "");
            index = 0;
            return ParseExpression();
        }

        private double ParseExpression()
        {
            double result = ParseTerm();

            while (index < expression.Length)
            {
                char op = expression[index];
                if (op != '+' && op != '-') break;

                index++;
                double right = ParseTerm();
                result = op == '+' ? Add(result, right)
                                   : Subtract(result, right);
            }

            return result;
        }

        private double ParseTerm()
        {
            double result = ParseFactor();

            while (index < expression.Length)
            {
                char op = expression[index];
                if (op != '*' && op != '/') break;

                index++;
                double right = ParseFactor();
                result = op == '*' ? Multiply(result, right)
                                   : Divide(result, right);
            }

            return result;
        }

        private double ParseFactor()
        {
            if (expression[index] == '(')
            {
                index++; // skip '('
                double result = ParseExpression();
                index++; // skip ')'
                return result;
            }

            int start = index;
            while (index < expression.Length && (char.IsDigit(expression[index]) || expression[index] == '.'))
                index++;

            return double.Parse(expression.Substring(start, index - start));
        }

        private double Add(double operand1, double operand2)
        {
            var op = new AddOperation { Operand1 = operand1, Operand2 = operand2 };
            return op.Execute();
        }

        private double Subtract(double operand1, double operand2)
        {
            var op = new SubtractOperation { Operand1 = operand1, Operand2 = operand2 };
            return op.Execute();
        }

        private double Multiply(double operand1, double operand2)
        {
            var op = new MultiplyOperation { Operand1 = operand1, Operand2 = operand2 };
            return op.Execute();
        }

        private double Divide(double operand1, double operand2)
        {
            var op = new DivideOperation { Operand1 = operand1, Operand2 = operand2 };
            return op.Execute();
        }
    }
}

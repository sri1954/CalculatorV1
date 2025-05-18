using CalculatorLibrary.Models;

namespace CalculatorAPITests
{
    public class OperationTest
    {
        [Test]
        public void AddOperation_Should_Return_Sum()
        {
            var op = new AddOperation { Operand1 = 15, Operand2 = 5 };
            Assert.That(op.Execute(), Is.EqualTo(20));
        }

        [Test]
        public void SubtractOperation_Should_Return_Difference()
        {
            var op = new SubtractOperation { Operand1 = 15, Operand2 = 5 };
            Assert.That(op.Execute(), Is.EqualTo(10));
        }

        [Test]
        public void MultiplyOperation_Should_Return_Product()
        {
            var op = new MultiplyOperation { Operand1 = 15, Operand2 = 5 };
            Assert.That(op.Execute(), Is.EqualTo(75));
        }

        [Test]
        public void DivideOperation_WithNonZeroDenominator_Should_Return_Quotient()
        {
            var op = new DivideOperation { Operand1 = 15, Operand2 = 5 };
            Assert.That(op.Execute(), Is.EqualTo(3));
        }

        [Test]
        public void DivideOperation_WithZeroDenominator_Should_ThrowException()
        {
            var op = new DivideOperation { Operand1 = 5, Operand2 = 0 };
            Assert.Throws<DivideByZeroException>(() => op.Execute());
        }

    }
}

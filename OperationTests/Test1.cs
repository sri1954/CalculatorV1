
using CalculatorLibrary.Models;

namespace OperationTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void AddOperation_Should_Return_Sum()
        {
            var op = new AddOperation { Operand1 = 15, Operand2 = 5 };
            Assert.AreEqual(20, op.Execute());
        }

        [TestMethod]
        public void SubtractOperation_Should_Return_Difference()
        {
            var op = new SubtractOperation { Operand1 = 15, Operand2 = 5 };
            Assert.AreEqual(10, op.Execute());
        }

        [TestMethod]
        public void MultiplyOperation_Should_Return_Product()
        {
            var op = new MultiplyOperation { Operand1 = 15, Operand2 = 5 };
            Assert.AreEqual(75, op.Execute());
        }

        [TestMethod]
        public void DivideOperation_WithNonZeroDenominator_Should_Return_Quotient()
        {
            var op = new DivideOperation { Operand1 = 15, Operand2 = 5 };
            Assert.AreEqual(3, op.Execute());
        }

        [TestMethod]
        public void DivideOperation_WithZeroDenominator_Should_ThrowException()
        {
            var op = new DivideOperation { Operand1 = 5, Operand2 = 0 };
            Assert.ThrowsException<DivideByZeroException>(() => op.Execute());
        }

    }
}

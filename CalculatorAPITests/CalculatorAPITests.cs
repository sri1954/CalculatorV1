using CalculatorLibrary.Interfacs;
using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using CalculatorWebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;

namespace CalculatorAPITests
{
    public class CalculatorAPITests
    {
        private Mock<IOperationFactoryExt> _mockOperationFactory;
        private Mock<IExpressionEvaluator> _mockExpressionEvaluator;

        private CalculatorController CreateController()
        {
            return new CalculatorController(_mockOperationFactory.Object, _mockExpressionEvaluator.Object);
        }

        [SetUp]
        public void Setup()
        {
            // Initialize the mock for IOperationFactory
            _mockOperationFactory = new Mock<IOperationFactoryExt>();
            // Initialize the mock for IExpressionEvaluator
            _mockExpressionEvaluator = new Mock<IExpressionEvaluator>();
        }

        [Test]
        public void PostModel_ValidAddition_ReturnsOkWithResult()
        {
            // Arrange
            var controller = CreateController(); 
            var request = new CalculationRequest
            {
                operation = "add",
                operand1 = 10,
                operand2 = 5
            };

            // Act
            var result = controller.PostModel(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            if (okResult?.Value is CalculationRequest response) // Combine null checks and type casting
            {
                Assert.That(response.result, Is.EqualTo(15));
            }
        }

        [Test]
        public void DetectXMLJSONFormat_ValidJson_ReturnsJSON()
        {
            // Arrange
            string json = "{\"operation\":\"add\",\"operand1\":1,\"operand2\":2}";

            // Act
            var result = CalculatorController.DetectXMLJSONFormat(json, "JSON");

            // Assert
            Assert.That(result, Is.EqualTo("JSON"));
        }

        [Test]
        public void DetectXMLJSONFormat_ValidXml_ReturnsXML()
        {
            // Arrange
            string xml = "<CalculationRequest><operation>add</operation><operand1>1</operand1><operand2>2</operand2></CalculationRequest>";

            // Act
            var result = CalculatorController.DetectXMLJSONFormat(xml, "XML");

            // Assert
            Assert.That(result, Is.EqualTo("XML"));
        }

        [Test]
        public void DetectXMLJSONFormat_InvalidInput_ReturnsUnknown()
        {
            // Arrange
            string input = "not a valid format";

            // Act
            var resultJson = CalculatorController.DetectXMLJSONFormat(input, "JSON");
            var resultXml = CalculatorController.DetectXMLJSONFormat(input, "XML");

            // Assert
            Assert.That(resultJson, Is.EqualTo("Unknown"));
            Assert.That(resultXml, Is.EqualTo("Unknown"));
        }

        // Helper to set up ControllerContext with a custom request body and content type
        private CalculatorController CreateControllerWithBody(string body, string contentType)
        {
            var controller = new CalculatorController(_mockOperationFactory.Object, _mockExpressionEvaluator.Object); // Pass the mock object
            var context = new DefaultHttpContext();
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            context.Request.ContentType = contentType;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
            return controller;
        }

        [Test]
        public async Task PostJSON_ValidJson_ReturnsOkWithXml()
        {
            // Arrange
            string json = @"{
                    'Operations':{
                    'Operations': {
                        'Operation': [
                            { '@ID': 'add', 'Operand1': 2, 'Operand2': 3 }
                        ]
                    }}
                }".Replace('\'', '"');

            var controller = CreateControllerWithBody(json, "application/json");

            // Act
            var result = await controller.PostJSON();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null); // Ensure 'ok' is not null before accessing its properties
            if (ok != null) // Add null check to avoid CS8602
            {
                Assert.That(ok.Value?.ToString(), Does.Contain("<Result>5</Result>")); // Use null-conditional operator
            }
        }

        [Test]
        public async Task PostJSON_InvalidJson_ReturnsBadRequest()
        {
            var controller = CreateControllerWithBody("not a json", "application/json");
            var result = await controller.PostJSON();
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task PostXml_ValidXml_ReturnsOkWithXml()
        {
            string xml = @"<Root>
                    <Operations>
                        <Operation ID='multiply'>
                            <Operand1>4</Operand1>
                            <Operand2>5</Operand2>
                        </Operation>
                    </Operations>
                </Root>";
            var controller = CreateControllerWithBody(xml, "application/xml");
            var result = await controller.PostXml();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null); // Ensure 'ok' is not null before accessing its properties
            Assert.That(ok!.Value?.ToString(), Does.Contain("<Result>20</Result>")); // Use null-conditional operator
        }

        [Test]
        public async Task PostXml_InvalidXml_ReturnsBadRequest()
        {
            var controller = CreateControllerWithBody("not xml", "application/xml");
            var result = await controller.PostXml();
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task PostXmlOrJson_ValidXml_ReturnsOkWithXml()
        {
            string xml = @"<Root>
                    <Operations>
                        <Operation ID='divide'>
                            <Operand1>20</Operand1>
                            <Operand2>4</Operand2>
                        </Operation>
                    </Operations>
                </Root>";
            var controller = CreateControllerWithBody(xml, "application/xml");
            var result = await controller.PostXmlOrJson();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null); // Ensure 'ok' is not null before accessing its properties
            Assert.That(ok!.Value?.ToString(), Does.Contain("<Result>5</Result>")); // Use null-conditional operator
        }

        [Test]
        public async Task PostXmlOrJson_InvalidContent_ReturnsBadRequest()
        {
            var controller = CreateControllerWithBody("invalid", "application/json");
            var result = await controller.PostXmlOrJson();
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void PostModel_ValidRequest_ReturnsOkWithResult()
        {
            var controller = CreateController();
            var request = new CalculationRequest
            {
                operation = "add",
                operand1 = 7,
                operand2 = 8
            };
            var result = controller.PostModel(request);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var ok = result.Result as OkObjectResult;
            var response = ok!.Value as CalculationRequest;
            Assert.That(response!.result, Is.EqualTo(15));
        }

        [Test]
        public void PostModel_NullRequest_ReturnsBadRequest()
        {
            var controller = CreateController();
            CalculationRequest? request = null; // Use nullable reference type
            var result = controller.PostModel(request!);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void PostModel_EmptyOperation_ReturnsBadRequest()
        {
            var controller = CreateController();
            var request = new CalculationRequest
            {
                operation = "",
                operand1 = 1,
                operand2 = 2
            };
            var result = controller.PostModel(request);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void PostModel_UnknownOperation_ReturnsOkWithErrorMessage()
        {
            var controller = CreateController();
            var request = new CalculationRequest
            {
                operation = "unknown",
                operand1 = 1,
                operand2 = 2
            };
            var result = controller.PostModel(request);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var ok = result.Result as OkObjectResult;
            Assert.That(ok!.Value, Is.TypeOf<string>());
            Assert.That(ok.Value.ToString(), Does.Contain("Unknown operation type"));
        }

        [Test]
        public void RecursionExtension_ValidEquation_ReturnsOkWithResult()
        {
            // Arrange
            var mockFactory = new Mock<IOperationFactoryExt>();
            var mockEvaluator = new Mock<IExpressionEvaluator>();
            mockEvaluator.Setup(e => e.Evaluate("2+3*4")).Returns(14);
            var controller = new CalculatorController(mockFactory.Object, mockEvaluator.Object);

            // Act
            var result = controller.RecursionExtension("2+3*4");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok.Value, Is.EqualTo("Equation 2+3*4 result is 14."));
        }

        [Test]
        public void RecursionExtension_EmptyEquation_ReturnsBadRequest()
        {
            var mockFactory = new Mock<IOperationFactoryExt>();
            var mockEvaluator = new Mock<IExpressionEvaluator>();
            var controller = new CalculatorController(mockFactory.Object, mockEvaluator.Object);

            var result = controller.RecursionExtension("");

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Equation cannot be null or empty."));
        }

        [Test]
        public void DatatableCompute_ValidEquation_ReturnsOkWithResult()
        {
            // Arrange
            var mockEvaluator = new Mock<IExpressionEvaluator>();
            var mockFactory = new Mock<IOperationFactoryExt>();
            var operationFactoryV1 = new Mock<OperationFactoryExt>();
            var controller = new CalculatorController(operationFactoryV1.Object, mockEvaluator.Object);

            // Act
            var result = controller.DatatableCompute("2+3*4");

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var ok = result as OkObjectResult;
            Assert.That(ok.Value, Is.EqualTo("Equation 2+3*4 result is 14."));
        }

        [Test]
        public void DatatableCompute_EmptyEquation_ReturnsBadRequest()
        {
            var mockEvaluator = new Mock<IExpressionEvaluator>();
            var mockFactory = new Mock<IOperationFactoryExt>();
            var operationFactoryV1 = new Mock<OperationFactoryExt>();
            var controller = new CalculatorController(operationFactoryV1.Object, mockEvaluator.Object);

            var result = controller.DatatableCompute("");

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Equation cannot be null or empty."));
        }

        [Test]
        public void DatatableCompute_FactoryNotV1_ReturnsBadRequest()
        {
            var mockFactory = new Mock<IOperationFactoryExt>();
            var mockEvaluator = new Mock<IExpressionEvaluator>();
            var controller = new CalculatorController(mockFactory.Object, mockEvaluator.Object);

            var result = controller.DatatableCompute("2+3");

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("OperationFactory is not of type OperationFactoryV1."));
        }
    }
}

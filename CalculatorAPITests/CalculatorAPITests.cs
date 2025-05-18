using CalculatorLibrary.Models;
using CalculatorWebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CalculatorAPITests
{
    public class CalculatorAPITests
    {
        [Test]
        public void PostModel_ValidAddition_ReturnsOkWithResult()
        {
            // Arrange
            var controller = new CalculatorController();
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
        private static CalculatorController CreateControllerWithBody(string body, string contentType)
        {
            var controller = new CalculatorController();
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
            var controller = new CalculatorController();
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
            var controller = new CalculatorController();
            CalculationRequest? request = null; // Use nullable reference type
            var result = controller.PostModel(request!);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void PostModel_EmptyOperation_ReturnsBadRequest()
        {
            var controller = new CalculatorController();
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
            var controller = new CalculatorController();
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

    }
}

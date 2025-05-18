using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/xml", "application/json")]
    [Produces("application/xml")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class CalculatorController : ControllerBase
    {

        [HttpPost]
        [Route("PostJSON")]

        public async Task<IActionResult> PostJSON()
        {
            // Read the XML string from the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();

            try
            {
                XmlDocument? xmlDoc = JsonConvert.DeserializeXmlNode(jsonString);
                if (xmlDoc == null)
                {
                    throw new InvalidOperationException("Deserialization resulted in a null XmlDocument.");
                }

                // Convert XmlDocument to XElement
                XElement xml = XElement.Parse(xmlDoc.OuterXml);

                // Parse the XML string into an XElement object
                //XElement xml = XElement.Parse(xmlString);

                // lOOP through each operation in the XML
                foreach (var element in xml!.Element("Operations")!.Elements())
                {
                    // GET the operation type and operands from the XML
                    string operation = element.Attribute("ID")?.Value ?? string.Empty;
                    string operand1 = element.Element("Operand1")?.Value ?? "0";
                    string operand2 = element.Element("Operand2")?.Value ?? "0";

                    try
                    {
                        // Validate the operation type
                        if (string.IsNullOrEmpty(operation))
                            throw new InvalidOperationException("Invalid operation type");

                        // DECLARE the result variable
                        double result = 0.0;

                        // Create the operation object using the factory method
                        OperationBase _operation = OperationFactory.CreateOperation(operation, Convert.ToDouble(operand1), Convert.ToDouble(operand2));

                        // Execute the operation
                        result = _operation.Execute();

                        // Update the XML with the result
                        element.Add(new XElement("Result", result));

                    }
                    catch (Exception ex)
                    {
                        // Update the XML with the result
                        element.Add(new XElement("Result", ex.Message));
                    }
                }

                // Return the modified XML as a response
                return Ok(xml);
            }
            catch (System.Xml.XmlException ex)
            {
                return BadRequest($"Invalid XML: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostXML")]

        public async Task<IActionResult> PostXml()
        {
            // Read the XML string from the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var xmlString = await reader.ReadToEndAsync();

            try
            {
                // Parse the XML string into an XElement object
                XElement xml = XElement.Parse(xmlString);

                // lOOP through each operation in the XML
                foreach (var element in xml!.Element("Operations")!.Elements())
                {
                    // GET the operation type and operands from the XML
                    string operation = element.Attribute("ID")?.Value ?? string.Empty;
                    string operand1 = element.Element("Operand1")?.Value ?? "0";
                    string operand2 = element.Element("Operand2")?.Value ?? "0";

                    try
                    {
                        // Validate the operation type
                        if (string.IsNullOrEmpty(operation))
                            throw new InvalidOperationException("Invalid operation type");

                        // DECLARE the result variable
                        double result = 0.0;

                        // Create the operation object using the factory method
                        OperationBase _operation = OperationFactory.CreateOperation(operation, Convert.ToDouble(operand1), Convert.ToDouble(operand2));

                        // Execute the operation
                        result = _operation.Execute();

                        // Update the XML with the result
                        element.Add(new XElement("Result", result));

                    }
                    catch (Exception ex)
                    {
                        // Update the XML with the result
                        element.Add(new XElement("Result", ex.Message));
                    }
                }

                // Return the modified XML as a response
                return Ok(xml);
            }
            catch (System.Xml.XmlException ex)
            {
                return BadRequest($"Invalid XML: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("Compute")]
        [Consumes("application/xml", "application/json")]

        public ActionResult<CalculationRequest> Compute([FromBody] CalculationRequest myrequest)
        {
            double result = 0;
            try
            {
                if (myrequest == null)
                    return BadRequest("Invalid input");

                if (string.IsNullOrEmpty(myrequest.operation))
                    return BadRequest("Invalid operation type");

                OperationBase operation = OperationFactory.CreateOperation(myrequest.operation, myrequest.operand1, myrequest.operand2);

                result = operation.Execute();
                myrequest.result = result;

                return Ok(myrequest);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}

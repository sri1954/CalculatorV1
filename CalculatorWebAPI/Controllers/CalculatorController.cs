using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class CalculatorController : ControllerBase
    {

        [HttpPost]
        [Route("PostJSON")]
        [Consumes("application/json")]

        public async Task<IActionResult> PostJSON()
        {
            string xmlString = string.Empty;

            // Read the XML string from the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();

            try
            {
                // Check if the JSON string is null or empty
                if (string.IsNullOrEmpty(jsonString))
                {
                    return BadRequest("Invalid JSON string");
                }

                // Check if the JSON string is valid
                if (DetectXMLJSONFormat(jsonString, "JSON") != "JSON")
                {
                    return BadRequest("Invalid JSON string");
                }

                // Deserialize the JSON string into an XmlDocument object
                xmlString = GetXmlStringFromJsonString(jsonString);

                // Get IActionResult
                //XElement xml = GetActionResult(xmlDoc.OuterXml);
                XElement xml = GetActionResult(xmlString);

                // Return the modified XML as a response
                return Ok(xml);
            }
            catch (System.Xml.XmlException ex)
            {
                return BadRequest($"Invalid JSON: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostXML")]
        [Consumes("application/xml")]

        public async Task<IActionResult> PostXml()
        {
            string xmlString = string.Empty;

            // Read the XML string from the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            xmlString = await reader.ReadToEndAsync();

            try
            {
                // Check if the XML string is null or empty
                if (string.IsNullOrEmpty(xmlString))
                {
                    return BadRequest("Invalid XML string");
                }

                // Check if the XML string is valid
                if (DetectXMLJSONFormat(xmlString, "XML") != "XML")
                {
                    return BadRequest("Invalid XML string");
                }

                // Get IActionResult
                XElement xml = GetActionResult(xmlString);

                // Return the modified XML as a response
                return Ok(xml);
            }
            catch (System.Xml.XmlException ex)
            {
                return BadRequest($"Invalid XML: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostXMLOrJSON")]
        [Consumes("application/xml", "application/json")]

        public async Task<IActionResult> PostXmlOrJson()
        {
            string xmlString = string.Empty;

            // Get ContentType
            var strContentType = Request.ContentType;

            // Read the XML string from the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string strRequest = await reader.ReadToEndAsync();

            try
            {
                // Check if the XML string is null or empty
                if (string.IsNullOrEmpty(strRequest))
                {
                    return BadRequest("Invalid XML/JSON string");
                }

                switch (strContentType)
                {
                    case "application/json":
                        // Check if the JSON string is valid
                        if (DetectXMLJSONFormat(strRequest, "JSON") != "JSON")
                        {
                            return BadRequest("Invalid JSON string");
                        }

                        // Deserialize the JSON string into an XmlDocument object
                        xmlString = GetXmlStringFromJsonString(strRequest);
                        break;

                    case "application/xml":
                        // Check if the XML string is valid
                        if (DetectXMLJSONFormat(strRequest, "XML") != "XML")
                        {
                            return BadRequest("Invalid XML string");
                        }

                        xmlString = strRequest;
                        break;
                }

                // Get IActionResult
                XElement xml = GetActionResult(xmlString);

                // Return the modified XML as a response
                return Ok(xml);
            }
            catch (System.Xml.XmlException ex)
            {
                return BadRequest($"Invalid XML/JSON: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostModel")]
        [Consumes("application/xml", "application/json")]

        public ActionResult<CalculationRequest> PostModel([FromBody] CalculationRequest myrequest)
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


        public static string DetectXMLJSONFormat(string input, string strType)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Unknown";

            // Check if the input is JSON or XML based on the content type
            if ((strType == "JSON"))
            {
                try
                {
                    var token = JToken.Parse(input);
                    return "JSON";
                }
                catch (JsonReaderException)
                {
                    // Not JSON
                    return "Unknown";
                }
            }

            if (strType == "XML")
            {
                try
                {
                    var xml = XElement.Parse(input);
                    return "XML";
                }
                catch (System.Xml.XmlException)
                {
                    // Not XML
                    return "Unknown";
                }
            }

            return "Unknown";
        }

        private static string GetXmlStringFromJsonString(string strRequest)
        {
            XmlDocument? xmlDoc = null;

            try
            {
                // Deserialize the JSON string into an XmlDocument object
                xmlDoc = JsonConvert.DeserializeXmlNode(strRequest);
                if (xmlDoc == null)
                {
                    throw new InvalidOperationException("Deserialization resulted in a null XmlDocument.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deserializing JSON to XML: {ex.Message}");
            }

            return xmlDoc.OuterXml;
        }

        private static XElement GetActionResult(string xmlString)
        {
            // Parse the XML string into an XElement object
            XElement xml = XElement.Parse(xmlString);

            // LOOP through each operation in the XML
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

            // Return the modified XML
            return xml;
        }
    }
}

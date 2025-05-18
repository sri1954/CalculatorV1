using System.Xml;
using System.Xml.Serialization;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;

namespace CalculatorLibrary.Models
{
    [XmlRoot(ElementName = "CalculationRequest")]
    public class CalculationRequest
    {
        [XmlElement(ElementName = "operation")]

        public string operation { get; set; } = string.Empty; // Default value is an empty string

        [XmlElement(ElementName = "operand1")]
        public double operand1 { get; set; } // Default value is 0.0

        [XmlElement(ElementName = "operand2")]
        public double operand2 { get; set; } // Default value is 0.0

        [XmlElement(ElementName = "result")]
        public double result { get; set; } // Default value is 0.0
    }

    [XmlRoot("Maths")]
    public class Maths
    {
        [XmlArray(ElementName ="operations")]
        [XmlArrayItem(ElementName ="operation")]
        public List<Operation>? operations { get; set; }
    }


    public class Operation
    {
        [XmlElement(ElementName = "id")]
        public string id { get; set; } = string.Empty;

        [XmlElement(ElementName = "operand1")]
        public double operand1 { get; set; }

        [XmlElement(ElementName = "operand2")]
        public double operand2 { get; set; }

        [XmlElement(ElementName = "result")]
        public double result { get; set; }

    }
}

using System.Xml.Serialization;

namespace CalculatorLibrary.Models
{
    [XmlRoot("CalculationResponse")]
    public class CalculationResponse
    {
        public double Result { get; set; } // Default value is 0.0
        public string Message { get; set; } = string.Empty;
    }
}

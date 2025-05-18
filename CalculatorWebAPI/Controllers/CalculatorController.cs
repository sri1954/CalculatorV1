using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Xml.Serialization;

namespace CalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

    public class CalculatorController : ControllerBase
    {
        [HttpPost]
        [Route("ComputeA")]
        [Consumes("application/xml", "application/json")]

        public ActionResult<CalculationRequest> ComputeA([FromBody] Maths myrequest)
        {
            //double result = 0;
            try
            {
                if (myrequest == null)
                    return BadRequest("Invalid input");

                if (myrequest.operations == null)
                    return BadRequest("Invalid operation type");

                //OperationBase operation = OperationFactory.CreateOperation(myrequest.operation, myrequest.operand1, myrequest.operand2);

                //result = operation.Execute();
                //myrequest.result = result;

                return Ok(myrequest);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [Route("ComputeB")]
        [Consumes("application/xml", "application/json")]

        public ActionResult<CalculationRequest> ComputeB([FromBody] CalculationRequest myrequest)
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

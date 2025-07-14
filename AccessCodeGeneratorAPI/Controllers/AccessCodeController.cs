using Microsoft.AspNetCore.Mvc;
using AccessCodeGenerator.API.Models;
using AccessCodeGenerator.API.Services;

namespace AccessCodeGenerator.API.Controllers
{
    /// <summary>  
    /// Controller for generating secure temporary access codes.  
    /// </summary>  
    [ApiController]
    [Route("api/[controller]")]
    public class AccessCodeController : ControllerBase
    {
        private readonly AccessCodeService _service = new();

        [HttpPost("GenerateCode")]
        public ActionResult<GenerateResponse> GenerateCode([FromBody] GenerateRequest request)
        {
            var result = _service.GenerateCode(request);
            return Ok(result);
        }

        [HttpPost("ValidateCode")]
        public ActionResult<ValidateResponse> ValidateCode([FromBody] ValidateRequest request)
        {
            var result = _service.ValidateCode(request.Code);
            return Ok(result);
        }
    }
}

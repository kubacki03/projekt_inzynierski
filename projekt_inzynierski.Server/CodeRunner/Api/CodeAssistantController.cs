using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Application.DTOs;

namespace projekt_inzynierski.Server.CodeRunner.Api
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CodeAssistantController : ControllerBase
    {

        private ICodeAnalyzer _codeAnalyzer;

        public CodeAssistantController(ICodeAnalyzer codeAnalyzer)
        {
            _codeAnalyzer = codeAnalyzer;
        }

        [HttpPost("CheckCode")]
        [EnableRateLimiting("expensive")]
        public async Task<IActionResult> CheckCodeCorrectness(CodeRequest request)
        {
               var response = await _codeAnalyzer.AnalyzeCode(request);


            return  Ok(response);
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Users.Application.Interfaces;

namespace projekt_inzynierski.Server.Content.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
    {

        private readonly ICodeAnalyzer _codeAnalyzer;
        private readonly ISubject _service;
        private readonly IExercise _exerciseService;
        private readonly IUserProgress _userProgressService;
        public ExerciseController(ICodeAnalyzer codeAnalyzer, ISubject service, IExercise exerciseService, IUserProgress userProgress)
        {
            _userProgressService = userProgress;
            _codeAnalyzer = codeAnalyzer;
            _service = service;
            _exerciseService = exerciseService;
        }

        [HttpPost("SaveExerciseSolution")]
        [Authorize]
        public async Task<IActionResult> SaveExerciseSolution(SaveExerciseRequest request)
        {
            var exercise = await _service.GetExerciseById(request.ExerciseId);

            var userId = User.Identity?.Name;
            var isDone = await _exerciseService.IsExerciseDone(request.ExerciseId, userId);


            if (isDone) {
                CodeReviewResult res = new CodeReviewResult { IsDoneGood = true, Review = "Zadanie zostało już wykonane" };

                return Ok(res);
            }
            
            
            var response = await _codeAnalyzer.AnalyzeCode(request.Solution, exercise.Task);
            await _exerciseService.AddUserExercise(userId, request.ExerciseId,response.IsDoneGood, response );
            return Ok(response);

        }


    }
}

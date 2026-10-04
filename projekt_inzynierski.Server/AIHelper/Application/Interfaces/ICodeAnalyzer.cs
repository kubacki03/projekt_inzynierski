using projekt_inzynierski.Server.CodeRunner.Api;
using projekt_inzynierski.Server.Content.Application.DTOs;

namespace projekt_inzynierski.Server.AIHelper.Application.Interfaces
{
    public interface ICodeAnalyzer
    {
        Task<CodeReviewResult> AnalyzeCode(string code, string task);
        Task<CodeReviewResult> AnalyzeCode(CodeRequest request);
    }
}

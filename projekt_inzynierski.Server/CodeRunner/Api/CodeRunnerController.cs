using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;

namespace projekt_inzynierski.Server.CodeRunner.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CodeRunnerController : ControllerBase
    {
        private const int MaxCodeLength = 20_000;
        private static readonly TimeSpan ExecutionTimeout = TimeSpan.FromSeconds(20);

        private readonly IWebHostEnvironment _env;

        private const string CsharpProjectFile = """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <OutputType>Exe</OutputType>
                <TargetFramework>net8.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <InvariantGlobalization>true</InvariantGlobalization>
              </PropertyGroup>
            </Project>
            """;

        public CodeRunnerController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [EnableRateLimiting("expensive")]
        [RequestSizeLimit(MaxCodeLength * 4)]
        public async Task<IActionResult> RunCode([FromBody] CodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code) || request.Code.Length > MaxCodeLength)
                return BadRequest($"Code must be between 1 and {MaxCodeLength} characters.");

            string tempDir = Path.Combine(_env.ContentRootPath, "Temp", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            string output = "";
            string errors = "";

            try
            {
                string dockerImage = "";
                string[] dockerCmd = Array.Empty<string>();
                string fileName = "";

                switch (request.Language.ToLower())
                {
                    case "csharp":
                        dockerImage = "mcr.microsoft.com/dotnet/sdk:8.0";
                        fileName = "Program.cs";
                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, fileName), request.Code);
                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, "app.csproj"), CsharpProjectFile);
                        dockerCmd = new[] { "/bin/bash", "-c", "cd /app && dotnet run" };
                        break;

                    case "java":
                        dockerImage = "openjdk:23-slim";

                        string className = ExtractJavaClassName(request.Code);
                        fileName = $"{className}.java";

                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, fileName), request.Code);
                        dockerCmd = new[] { "/bin/bash", "-c", $"cd /app && javac {fileName} && java {className}" };
                        break;

                    case "python":
                        dockerImage = "python:3.12";
                        fileName = "script.py";
                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, fileName), request.Code);
                        dockerCmd = new[] { "/bin/bash", "-c", "cd /app && python script.py" };
                        break;

                    case "javascript":
                        dockerImage = "node:22";
                        fileName = "script.js";
                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, fileName), request.Code);
                        dockerCmd = new[] { "/bin/sh", "-c", "cd /app && node script.js" };
                        break;

                    case "typescript":
                        dockerImage = "oven/bun:1";
                        fileName = "script.ts";
                        await System.IO.File.WriteAllTextAsync(Path.Combine(tempDir, fileName), request.Code);
                        dockerCmd = new[] { "/bin/sh", "-c", "cd /app && bun run script.ts" };
                        break;

                    default:
                        return BadRequest("Unsupported language.");
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                foreach (var arg in new[]
                {
                    "run", "--rm", "--memory=256m", "--cpus=0.5", "--pids-limit=100",
                    "--network=none", "--security-opt=no-new-privileges",
                    "-v", $"{tempDir}:/app", dockerImage
                })
                {
                    startInfo.ArgumentList.Add(arg);
                }
                foreach (var arg in dockerCmd)
                {
                    startInfo.ArgumentList.Add(arg);
                }

                using var process = new Process { StartInfo = startInfo };
                process.Start();

                var stdoutTask = process.StandardOutput.ReadToEndAsync();
                var stderrTask = process.StandardError.ReadToEndAsync();

                using var cts = new CancellationTokenSource(ExecutionTimeout);
                try
                {
                    await process.WaitForExitAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    process.Kill(entireProcessTree: true);
                    errors += "Execution timed out.\n";
                }

                output = await stdoutTask;
                errors += await stderrTask;
            }
            catch (Exception ex)
            {
                errors += $"Server error: {ex.Message}";
            }
            finally
            {
                try { Directory.Delete(tempDir, true); } catch { }
            }

            return Ok(new { output, errors });
        }

        private string ExtractJavaClassName(string code)
        {
            var match = System.Text.RegularExpressions.Regex.Match(
                code, @"public\s+class\s+(\w+)");
            return match.Success ? match.Groups[1].Value : "Main";
        }
    }

    public class CodeRequest
    {
        public string Code { get; set; } = "";
        public string Language { get; set; } = "csharp";
    }
}
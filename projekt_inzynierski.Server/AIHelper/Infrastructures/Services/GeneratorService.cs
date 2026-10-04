using OpenAI.Chat;
using System.Text.Json;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text;
using System.Text.Json;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.CodeDom.Compiler;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Content.Domain.Repositories;

using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Events;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Models;
namespace projekt_inzynierski.Server.AIHelper.Infrastructures.Services
{
    public class GeneratorService : IAiGenerator
    {
        string apiKey = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY");

        public async Task<List<string>> GenerateSubjects(string courseName, string courseDescription, User user)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

            List<ChatMessage> messages = new()
    {
        new UserChatMessage($"Jestes nauczycielem programowania, przygotuj mi 10 tematów do kursu z {courseName}, dzieki ktoremu uczniowie {courseDescription}. Moje ogólne doświadczenie w programowaniu to {user.Experience} a poziom edukacji to {user.EducationLevel}")
    };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "subject_to_learn",
                    jsonSchema: BinaryData.FromBytes("""
                {
                    "type": "object",
                    "properties": {
                        "subjectsToLearn": {
                            "type": "array",
                            "items": {
                                "type": "object",
                                "properties": {
                                   
                                    "subject": { "type": "string" }
                                },
                                "required": ["subject"],
                                "additionalProperties": false
                            }
                        }
                    },
                    "required": ["subjectsToLearn"],
                    "additionalProperties": false
                }
            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);


            JsonElement subjectsArray = structuredJson.RootElement.GetProperty("subjectsToLearn");

            List<string> subjects = new();

            foreach (JsonElement item in subjectsArray.EnumerateArray())
            {
                subjects.Add(item.GetProperty("subject").GetString()!);
            }
            return subjects;
        }



        public async Task<ContentModel> GenerateContent(string subject, string course, string level, User user, string evaluationResult)
        {

            ChatClient client = new(model: "gpt-4o-mini", apiKey: apiKey);


            List<ChatMessage> messages =
           [

              new UserChatMessage($" Dla tematu {subject} z  kursu {course} na poziomie {level} wygeneruj bardzo rozbudowany i obszerny materiał teoretyczny, 3 zadania praktyczne oraz  quiz sprawdzajacy wiedze niech quiz ma postac pytanie A) odpowiedz B) odpowiedz C) odpowiedz. Twoi uczniowe mówią po polsku ale chcą się nauczyć. Ich ogólne doświadczenie w programowaniu to {user.Experience} a poziom edukacji to {user.EducationLevel} "),
        ];

            if (LessonEvaluationResult.EasierNextLesson.ToString() == evaluationResult)
            {
                messages =
          [

             new UserChatMessage($" Dla tematu {subject} z  kursu {course} na poziomie {level} wygeneruj bardzo rozbudowany materiał teoretyczny, 2 zadania praktyczne oraz quiz sprawdzajacy wiedze niech quiz ma postac pytanie A) odpowiedz B) odpowiedz C) odpowiedz. Twoi uczniowe mówią po polsku ale chcą się nauczyć. W poprzednich zadaniach radzili sobie słabo, niech ta zawartość będzie troche prostsza"),
        ];
            }


            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
           jsonSchemaFormatName: "subject_to_learn",
           jsonSchema: BinaryData.FromBytes("""
        {
            "type": "object",
            "properties": {
                "ContentToLearn": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "title": { "type": "string" },
                            "content": { "type": "string" }
                        },
                        "required": ["title", "content"],
                        "additionalProperties": false
                    }
                },
                "PracticalTasks": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "TaskToDo": { "type": "string" }
                        },
                        "required": ["TaskToDo"],
                        "additionalProperties": false
                    }
                },
                "Quiz": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "properties": {
                            "Question": { "type": "string" },
                            "Answers": {
                                "type": "array",
                                "items": {
                                    "type": "object",
                                    "properties": {
                                        "Text": { "type": "string" },
                                        "IsTrueAnswer": { "type": "boolean" }
                                    },
                                    "required": ["Text", "IsTrueAnswer"],
                                    "additionalProperties": false
                                }
                            }
                        },
                        "required": ["Question", "Answers"],
                        "additionalProperties": false
                    }
                }
            },
            "required": ["ContentToLearn", "PracticalTasks", "Quiz"],
            "additionalProperties": false
        }
        """u8.ToArray()),
           jsonSchemaIsStrict: true)
            };





            ChatCompletion completion = await client.CompleteChatAsync(messages, options);
            string jsonResponse = completion.Content[0].Text;

            var contentModel = JsonSerializer.Deserialize<ContentModel>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            if (contentModel != null)
            {
                foreach (var item in contentModel.ContentToLearn)
                {
                    Console.WriteLine($"Title: {item.Title}");
                    Console.WriteLine($"Content: {item.Content}");
                }

                foreach (var task in contentModel.PracticalTasks)
                {
                    Console.WriteLine($"Task: {task.TaskToDo}");
                }
            }
            return contentModel;
        }







        public async Task<AdaptiveCourseWithSubjectsAndContent> GenerateSubjectFromPdf(string courseName, string courseDescription, User user, string pdfPath)
        {     
            if (!string.IsNullOrWhiteSpace(pdfPath) || File.Exists(pdfPath))
            {
                StringBuilder pdfText = new();


                using (PdfReader reader = new(pdfPath))
                using (PdfDocument pdfDoc = new(reader))
                {
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                        pdfText.AppendLine(pageText);
                    }
                }

                ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

                List<ChatMessage> messages = new()
                {
                    new UserChatMessage($@"
            Jesteś nauczycielem programowania. Na podstawie materiałów z poniższego PDF przygotuj **jeden temat kursu** dla kursu '{courseName}'.
            Opis kursu: {courseDescription}
            Doświadczenie użytkownika: {user.Experience}
            Poziom edukacji: {user.EducationLevel}

            PDF CONTENT:
            {pdfText}
            ")
                };

             
                ChatCompletionOptions options = new()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                        jsonSchemaFormatName: "subject_to_learn",
                        jsonSchema: BinaryData.FromBytes(""" 
                        {
                            "type": "object",
                            "properties": {
                                "subject": { "type": "string" }
                            },
                            "required": ["subject"],
                            "additionalProperties": false
                        }
                        """u8.ToArray()),
                        jsonSchemaIsStrict: true
                    )
                };

               
                ChatCompletion completion = await client.CompleteChatAsync(messages, options);

                
                using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
                string subject = structuredJson.RootElement.GetProperty("subject").GetString()!;


                var content = await GenerateContent(subject, courseName, user.Experience, user, LessonEvaluationResult.Normal.ToString());


                AdaptiveCourseWithSubjectsAndContent xd = new AdaptiveCourseWithSubjectsAndContent
                {
                    Content = content,
                    Subjects = new List<string> { subject }
                    ,
                    FirstSubject = subject
                };

                return xd;
            }
            else
            {
                AdaptiveCourseWithSubjectsAndContent x = new AdaptiveCourseWithSubjectsAndContent
                ();

                return x;

            }
        }

        public async Task<NotificationDto> GenerateSuggestAsync(List<string> problems, string mainCourseTitle)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

            string p = "";
            foreach (var problem in problems)
            {
                p = p + $" {problem}";
            }
            List<ChatMessage> messages = new()
    {
        new UserChatMessage($"Wygeneruj powiadomienie z propopnowanym kursem dzieki ktoremu uzytkowniki przypomni sobie wiedze z takich tematów {p}")
    };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
           jsonSchemaFormatName: "notification",
           jsonSchema: BinaryData.FromBytes("""
        {
            "type": "object",
            "properties": {
                "notification": {
                    "type": "object",
                    "properties": {
                        "notificationContent": { "type": "string" },
                        "title": { "type": "string" },
                        "suggestCourseTitle": { "type": "string" }
                    },
                    "required": ["notificationContent", "title", "suggestCourseTitle"],
                    "additionalProperties": false
                }
            },
            "required": ["notification"],
            "additionalProperties": false
        }
        """u8.ToArray()),
           jsonSchemaIsStrict: true
       )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

            JsonElement notificationJson = structuredJson.RootElement.GetProperty("notification");

            NotificationDto notification = new NotificationDto
            {
                Message = notificationJson.GetProperty("notificationContent").GetString(),
                Title = notificationJson.GetProperty("title").GetString(),
                CourseTitle = notificationJson.GetProperty("suggestCourseTitle").GetString(),

            };
            return notification;
        }



        public async Task<Theory> GenerateTheoryFromPdf(string courseName, string courseDescription, User user, string pdfPath)
        {
            StringBuilder pdfText = new();
            using (PdfReader reader = new(pdfPath))
            using (PdfDocument pdfDoc = new(reader))
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                    pdfText.AppendLine(pageText);
                }
            }

            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

            List<ChatMessage> messages = new() {
                new UserChatMessage($@"
                    Jesteś nauczycielem programowania. Na podstawie materiałów z poniższego PDF przygotuj **jeden materiał teoretyczny** dla kursu '{courseName}'.
                    Opis kursu: {courseDescription}
                    Doświadczenie użytkownika: {user.Experience}
                    Poziom edukacji: {user.EducationLevel}

                    PDF CONTENT:
                    {pdfText}
                    ")
                };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "theory_to_learn",
                    jsonSchema: BinaryData.FromBytes(""" 
            {
                "type": "object",
                "properties": {
                    "name": { "type": "string" },
                    "content": { "type": "string" }
                },
                "required": ["name", "content"],
                "additionalProperties": false
            }
            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
            JsonElement root = structuredJson.RootElement;

            Theory theory = new Theory
            {
                Name = root.GetProperty("name").GetString()!,
                Content = root.GetProperty("content").GetString()!
            };

            return theory;
        }

        public async Task<AdaptiveCourseWithSubjectsAndContent> GeneratePrivateSubjectsAndCourse(AdaptiveCourseDto courseDto)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);
            string skill = "Introduction";
            if (courseDto.User.Points < 500)
            {
                skill = "Beginner";    
            }else if(courseDto.User.Points >= 500 && courseDto.User.Points < 1500)
            {
                skill = "Intermediate";
            }
            else
            {
                skill = "Advanced";
            }

            List<ChatMessage> messages = new() {
                new UserChatMessage($@"Stworz kurs z {courseDto.Technologies} aby uzytkonwik nauczyl sie {courseDto.Description}. Poziom uzytkownika w nauce to {skill} ")
            };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "theory_to_learn",
                    jsonSchema: BinaryData.FromBytes(""" 
            {
                "type": "object",
                "properties": {
                    "courseName": { "type": "string" },
                    "courseDescription": { "type": "string" },
                    "programmingLanguage": {"type": "string" }
                },
                "required": ["courseName", "courseDescription","programmingLanguage"],
                "additionalProperties": false
            }
            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);
            JsonElement root = structuredJson.RootElement;

            var x = new 
            {
                Name = root.GetProperty("courseName").GetString()!,
                Description = root.GetProperty("courseDescription").GetString()!
            };

            Course course = new Course { Description = root.GetProperty("courseDescription").GetString()!, IsPublic = false, Language = root.GetProperty("programmingLanguage").GetString()!, Level = skill, Title = root.GetProperty("courseName").GetString()!, ImageURL = "https://videos.openai.com/vg-assets/assets%2Ftask_01k6zprkm4f55sp4rs3e28yhs0%2F1759853305_img_0.webp?st=2025-10-07T14%3A25%3A56Z&se=2025-10-13T15%3A25%3A56Z&sks=b&skt=2025-10-07T14%3A25%3A56Z&ske=2025-10-13T15%3A25%3A56Z&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skoid=cfbc986b-d2bc-4088-8b71-4f962129715b&skv=2019-02-02&sv=2018-11-09&sr=b&sp=r&spr=https%2Chttp&sig=15VgWwqTn%2FfBxebwVyoR8c0GAbV8RP9nR6HbvhRvsrY%3D&az=oaivgprodscus" };

            var dto = await GenerateSubjects(course.Title, course.Description,courseDto.User);

            AdaptiveCourseWithSubjectsAndContent content = new AdaptiveCourseWithSubjectsAndContent();
            if (string.IsNullOrWhiteSpace(courseDto.PdfPath) || !File.Exists(courseDto.PdfPath))
            {
                 content = await GenerateSubjectFromPdf(x.Name, x.Description, courseDto.User, courseDto.PdfPath);
            }
            content.Subjects.AddRange(dto);
            content.Course = course;
            return content;
            }
    }
}
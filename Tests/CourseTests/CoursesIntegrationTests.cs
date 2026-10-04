using Xunit;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server;
using projekt_inzynierski.Server.Courses.Domain.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class CoursesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CoursesIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCourses_ReturnsCoursesList()
    {
        // Act
        var response = await _client.GetAsync("/api/courses");

        // Assert
        response.EnsureSuccessStatusCode();
        var courses = await response.Content.ReadFromJsonAsync<List<Course>>();
        Xunit.Assert.NotNull(courses);
        Xunit.Assert.True(courses.Count >= 0); 
    }

    [Fact]
    public async Task GetCourseById_ReturnsCourseOrNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/courses/1");

        // Assert
        if (response.IsSuccessStatusCode)
        {
            var course = await response.Content.ReadFromJsonAsync<Course>();
            Xunit.Assert.NotNull(course);
        }
        else
        {
            Xunit.Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

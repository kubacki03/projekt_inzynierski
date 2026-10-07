using System.Net.Http.Json;
using System.Text.Json;
using Tests.Support;
using Xunit;

public class CoursesIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public CoursesIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCourses_ReturnsCoursesList()
    {
        var response = await _client.GetAsync("/FeaturedCourses/Get");

        response.EnsureSuccessStatusCode();
        var courses = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal(JsonValueKind.Array, courses.ValueKind);
    }

    [Fact]
    public async Task GetPagedCourses_ReturnsOk()
    {
        var response = await _client.GetAsync("/FeaturedCourses/paged?pageNumber=1&pageSize=5");

        response.EnsureSuccessStatusCode();
    }
}

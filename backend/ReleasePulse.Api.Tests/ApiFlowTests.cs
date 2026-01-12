using System.Net.Http.Json;
using FluentAssertions;
using ReleasePulse.Api.Dtos;
using ReleasePulse.Api.Models;
using Xunit;

public class ApiFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiFlowTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_work_item_then_list_returns_it()
    {
        var create = new CreateWorkItemDto("Story 1", "desc");
        var resp = await _client.PostAsJsonAsync("/work-items", create);
        var body = await resp.Content.ReadAsStringAsync();
        resp.IsSuccessStatusCode.Should().BeTrue($"Status: {resp.StatusCode}, Body: {body}");


        var list = await _client.GetFromJsonAsync<List<WorkItem>>("/work-items");
        list.Should().NotBeNull();
        list!.Any(x => x.Title == "Story 1").Should().BeTrue();
    }
}

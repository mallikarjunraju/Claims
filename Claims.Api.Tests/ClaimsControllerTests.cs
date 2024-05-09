using Claims.Api.Tests.Fixtures;
using Claims.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Claims.Api.Tests;

public class ClaimsControllerTests : IClassFixture<ClaimsApiFixture>
{
    private readonly ClaimsApiFixture _fixture;

    public ClaimsControllerTests(ClaimsApiFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        _fixture = fixture;
    }

    [Fact]
    public async Task Given_CoverAndCreateClaim_When_GetClaims_ThenAllClaimsReturnedWithNewClaim()
    {
        // Arrange
        string guid = await _fixture.CreateCover();

        var claim = await _fixture.CreateClaim(guid);

        // Act
        var response = await _fixture.Client.GetAsync("/claims");

        // Assert
        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var claims = JsonConvert.DeserializeObject<IEnumerable<Claim>>(await response.Content.ReadAsStringAsync());

        claims.Should().NotBeNullOrEmpty();
        claims!.Where(c => c.Id == claim!.Id).Should().NotBeEmpty();
    }

    [Fact]
    public async Task Given_CoverAndCreateClaim_When_DeleteClaim_ThenClaimDeleted()
    {
        // Arrange
        string guid = await _fixture.CreateCover();

        var claim = await _fixture.CreateClaim(guid);

        // Act
        var response = await _fixture.Client.DeleteAsync($"/claims/{claim.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _fixture.Client.GetAsync($"/claims/{claim.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Given_CoverAndCreateClaim_When_GetClaimById_ThenClaimReturned()
    {
        // Arrange
        string guid = await _fixture.CreateCover();

        var claim = await _fixture.CreateClaim(guid);

        // Act
        var response = await _fixture.Client.GetAsync($"/claims/{claim.Id}");

        // Assert
        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseClaim = JsonConvert.DeserializeObject<Claim>(await response.Content.ReadAsStringAsync());

        responseClaim.Should().BeEquivalentTo(claim, option => option.Excluding(claim => claim.Created));
    }

    [Fact]
    public async Task Given_CoverAndCreateClaim_When_CreateClaim_ThenClaimCreated()
    {
        // Arrange
        string guid = await _fixture.CreateCover();

        var claim = new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = guid,
            Created = DateTime.UtcNow,
            Name = "Test Claim",
            Type = ClaimType.BadWeather,
            DamageCost = 10
        };

        var claimMessage = new HttpRequestMessage(HttpMethod.Post, "/claims")
        {
            Content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _fixture.Client.SendAsync(claimMessage);

        // Assert
        using var _ = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseClaim = JsonConvert.DeserializeObject<Claim>(await response.Content.ReadAsStringAsync());

        responseClaim.Should().BeEquivalentTo(
            claim,
            option => option.Excluding(claim => claim.Created).Excluding(claim => claim.Id));
    }
}

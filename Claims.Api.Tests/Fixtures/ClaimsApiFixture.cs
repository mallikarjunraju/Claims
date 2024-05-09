using Claims.Infrastructure.Services;
using Claims.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace Claims.Api.Tests.Fixtures;

public class ClaimsApiFixture : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public Mock<IGlobalIdGenerator> GlobalIdGeneratorMock { get; } = new();

    public ClaimsApiFixture()
    {
        Client = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton(GlobalIdGeneratorMock.Object));
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Client.Dispose();
        }

        base.Dispose(disposing);
    }

    public void Reset()
    {
        GlobalIdGeneratorMock.Reset();
    }

    public Guid GenerateGuidAndSetupMock()
    {
        var guid = Guid.NewGuid();

        GlobalIdGeneratorMock.Setup(generator => generator.GenerateId()).Returns(guid);

        return guid;
    }

    public async Task<Claim?> CreateClaim(string guid)
    {
        var claim = new Claim
        {
            Id = "123",
            CoverId = guid,
            Created = DateTime.Now,
            Name = "Test",
            Type = ClaimType.Collision,
            DamageCost = 50
        };

        var claimMessage = new HttpRequestMessage(HttpMethod.Post, "/claims")
        {
            Content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json")
        };

        var createdClaimResponse = await Client.SendAsync(claimMessage);

        return JsonConvert.DeserializeObject<Claim>(await createdClaimResponse.Content.ReadAsStringAsync());
    }

    public async Task<string> CreateCover()
    {
        var guid = GenerateGuidAndSetupMock().ToString();

        var cover = new Cover
        {
            Id = guid,
            Type = CoverType.Yacht,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(100),
            Premium = 1111
        };

        var coverMessage = new HttpRequestMessage(HttpMethod.Post, "/covers")
        {
            Content = new StringContent(JsonConvert.SerializeObject(cover), Encoding.UTF8, "application/json")
        };

        var createdCoverResponse = await Client.SendAsync(coverMessage);

        return guid;
    }
}

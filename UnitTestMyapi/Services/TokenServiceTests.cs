using Microsoft.Extensions.Configuration;
using Myapi.Models;
using Myapi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestMyapi.Services;

public class TokenServiceTests
{
    [Fact]
    public void Configuration_ShouldLoadJwtSettings()
    {
        // ARRANGE: Create a fake "in-memory" configuration 
        // This mimics what Azure Key Vault provides in production
        var testSettings = new Dictionary<string, string>
        {
            {"Jwt:Key", "THIS_IS_A_VERY_LONG_SECRET_KEY_THAT_IS_EXACTLY_64_CHARACTERS_LONG_!!"},
            {"Jwt:Issuer", "test-issuer"},
            {"Jwt:Audience", "test-audience"}
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(testSettings)
            .Build();

        var tokenService = new TokenService(config);
        var fakeUser = new User { Id = 1, Username = "testuser", Role = "Admin" };

        // ACT
        var token = tokenService.CreateToken(fakeUser);

        // ASSERT
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }
}
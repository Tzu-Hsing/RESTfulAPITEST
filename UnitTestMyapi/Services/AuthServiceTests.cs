using Microsoft.EntityFrameworkCore;
using Moq;
using Myapi.DAL;
using Myapi.DTOs;
using Myapi.Interfaces;
using Myapi.Models;
using Myapi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTestMyapi.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly AppDbContext _context;
        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            // 1. Setup In-Memory Database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name per test
                .Options;

            _context = new AppDbContext(options);

            // 2. Setup Mock TokenService
            _mockTokenService = new Mock<ITokenService>();

            // 3. Initialize AuthService with fake DB and fake TokenService
            _authService = new AuthService(_context, _mockTokenService.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // ARRANGE
            var password = "password123";
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Tell the fake TokenService what to return when called
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<User>()))
                             .Returns("fake-jwt-token-string");

            var loginRequest = new LoginRequest { Username = "testuser", Password = password };

            // ACT
            var result = await _authService.LoginResponseAsync(loginRequest);

            // ASSERT
            Assert.True(result.Success);
            Assert.Equal("fake-jwt-token-string", result.token);
        }


        [Fact]
        public async Task Register_ShouldReturnFailure_WhenUserAlreadyExists()
        {
            // ARRANGE
            var existingUser = new User { Username = "taken", Email = "taken@test.com" };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var registerRequest = new RegisterRequest { Username = "taken", Email = "new@test.com" };

            // ACT
            var result = await _authService.RegisterRequestAsync(registerRequest);

            // ASSERT
            Assert.False(result.Success);
            Assert.Equal("UserName already registered.", result.Message);
        }
    }
}


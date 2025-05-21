using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia.Business.Models.Registers;
using SocialMedia.Business.Services.Registers;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Tests.RegisterTests.RegisterServiceTests;

[TestFixture]
public class AddRegisterTests
{
    private Mock<SocialMediaDbContext> _dbContextMock;
    private IRegisterService _service;
    private List<Register> _data;

    [SetUp]
    public void Setup()
    {
        SetupData();
        SetupDbMock();
        _service = new RegisterService(_dbContextMock.Object);
    }

    [Test]
    public async Task AddRegister_ShouldAddRegisterToDb()
    {
        // Constants
        const string addedEmail = "email@email.com";
        const string addedFeedback = "Test";
        int expectedNumberOfEntries = _data.Count + 1;
        
        // Arrange
        var registerToAdd = new AddRegisterDTO { Email = addedEmail, Feedback = addedFeedback};

        // Act
        await _service.AddRegister(registerToAdd);

        // Assert
        _data.Count.Should().Be(expectedNumberOfEntries);
        _data.Last().Email.Should().Be(addedEmail);
        _data.Last().Feedback.Should().Be(addedFeedback);
    }

    private void SetupData()
    {
        _data = new List<Register>();
    }
    
    private void SetupDbMock()
    {
        var options = new DbContextOptionsBuilder<SocialMediaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContextMock = new Mock<SocialMediaDbContext>(options);
        _dbContextMock
            .Setup(dbContext => dbContext.Registers)
            .ReturnsDbSet(_data);
        _dbContextMock.Setup(m => m.Registers.AddAsync(It.IsAny<Register>(), It.IsAny<CancellationToken>()))
            .Callback<Register, CancellationToken>((r, _) => _data.Add(r))
            .ReturnsAsync(new EntityEntry<Register>(null!));
    }
}

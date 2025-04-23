using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia.API.Requests;
using SocialMedia.Business.Models.Registers;
using SocialMedia.Business.Services.Example;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Tests.RegisterTests.RegisterValidatorsTests;

[TestFixture]
public class AddRegisterValidatorTests
{
    private AddRegisterRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new AddRegisterRequestValidator();
    }

    [Test]
    public void WhenEmailIsValid_ShouldBeValidated()
    {
        // Constants
        const string addedEmail = "email@email.com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldNotHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenEmailMissesAt_ShouldNotBeValidated()
    {
        // Constants
        const string addedEmail = "emailemail.com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenEmailMissesDot_ShouldNotBeValidated()
    {
        // Constants
        const string addedEmail = "email@emailcom";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenEmailIsEmpty_ShouldNotBeValidated()
    {
        // Constants
        const string addedEmail = "";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenDomainStartsWithNotAllowedCharacter_ShouldNotBeValidated()
    {
        // Constants
        const string addedEmail = "test@-test.com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenDomainEndsWithNotAllowedCharacter_ShouldNotBeValidated()
    {
        // Constants
        const string addedEmail = "test@test.com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
    
    [Test]
    public void WhenLocalPartOfEmailExceedsCharacterLimit_ShouldNotBeValidated()
    {
        // Constants
        const int localPartLengthLimit = 64;
        string addedEmail = new string('a', localPartLengthLimit + 1)  + "@gmail.com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }

    [Test] public void WhenDomainPartOfEmailExceedsCharacterLimit_ShouldNotBeValidated()
    {
        // Constants
        const int domainPartLengthLimit = 255;
        string addedEmail = "local@" + new string('a', domainPartLengthLimit + 1)  + ".com";
        const string addedFeedback = "Test";
        
        // Arrange
        var validRequest = new AddRegisterRequest
        {
            Email = addedEmail,
            Feedback = addedFeedback
        };

        // Act
        var result = _validator.TestValidate(validRequest);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.Email);
    }
}

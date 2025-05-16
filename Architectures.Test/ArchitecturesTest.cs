using NetArchTest.Rules;
using Xunit;

namespace Architectures.Test;

public class CleanArchitectureTests
{
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherLayers()
    {
        // Arrange
        var result = Types.InAssembly(typeof(CleanArchitectureTests).Assembly)
            .That()
            .ResideInNamespace("Domain")
            .ShouldNot()
            .HaveDependencyOnAny("Application", "Infrastructure", "WebAPi.CoworkingSpace")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Domain layer should not depend on other layers.");
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnInfrastructureOrPresentation()
    {
        // Arrange
        var result = Types.InAssembly(typeof(CleanArchitectureTests).Assembly)
            .That()
            .ResideInNamespace("Application")
            .ShouldNot()
            .HaveDependencyOnAny("Infrastructure", "WebAPi.CoworkingSpace")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Application layer should not depend on Infrastructure or Presentation layers.");
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnPresentation()
    {
        // Arrange
        var result = Types.InAssembly(typeof(CleanArchitectureTests).Assembly)
            .That()
            .ResideInNamespace("Infrastructure")
            .ShouldNot()
            .HaveDependencyOn("WebAPi.CoworkingSpace")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Infrastructure layer should not depend on Presentation layer.");
    }
}

using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Architecture;

public sealed class ArchitectureTests
{
    private const string DomainAssembly = "Domain";
    private const string ApplicationAssembly = "Application";
    private const string InfrastructureAssembly = "Infrastructure";
    private const string WebApiAssembly = "WebApi";

    [Fact]
    public void Domain_ShouldNotDependOnApplication()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.BaseEntity).Assembly)
            .ShouldNot()
            .HaveDependencyOn(ApplicationAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Domain layer must not depend on Application layer.");
    }

    [Fact]
    public void Domain_ShouldNotDependOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(Domain.Entities.BaseEntity).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Domain layer must not depend on Infrastructure layer.");
    }

    [Fact]
    public void Application_ShouldNotDependOnInfrastructure()
    {
        var result = Types.InAssembly(typeof(Application.DependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Application layer must not depend on Infrastructure layer.");
    }

    [Fact]
    public void Application_ShouldNotDependOnWebApi()
    {
        var result = Types.InAssembly(typeof(Application.DependencyInjection).Assembly)
            .ShouldNot()
            .HaveDependencyOn(WebApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Application layer must not depend on WebApi layer.");
    }

    [Fact]
    public void CommandHandlers_ShouldBeSealed()
    {
        var result = Types.InAssembly(typeof(Application.DependencyInjection).Assembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Command handlers should be sealed to prevent unintended inheritance.");
    }

    [Fact]
    public void QueryHandlers_ShouldBeSealed()
    {
        var result = Types.InAssembly(typeof(Application.DependencyInjection).Assembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Query handlers should be sealed to prevent unintended inheritance.");
    }
}

using FluentAssertions;
using NetArchTest.Rules;
using Sensus.Domain.Common;

namespace Sensus.ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Sensus.Domain";
    private const string ApplicationNamespace = "Sensus.Application";
    private const string InfrastructureNamespace = "Sensus.Infrastructure";
    private const string ApiNamespace = "Sensus.Api";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        var assembly = typeof(BaseEntity).Assembly;
        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            ApiNamespace
        };

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue(
            "El Dominio no debe depender de ninguna capa externa (Application, Infrastructure, Api)");
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOn_InfrastructureOrApi()
    {
        var assembly = typeof(Sensus.Application.DependencyInjection).Assembly;
        var forbiddenProjects = new[]
        {
            InfrastructureNamespace,
            ApiNamespace
        };

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue(
            "La capa de Aplicación no debe depender de Infraestructura ni de la API");
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOn_Api()
    {
        var assembly = typeof(Sensus.Infrastructure.DependencyInjection).Assembly;

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue(
            "La capa de Infraestructura no debe depender de la capa de API");
    }

    [Fact]
    public void Handlers_Should_Have_NameEndingWithHandler()
    {
        var assembly = typeof(Sensus.Application.DependencyInjection).Assembly;

        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(MediatR.IRequestHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue(
            "Todos los Handlers de MediatR deben terminar con el sufijo 'Handler'");
    }
}

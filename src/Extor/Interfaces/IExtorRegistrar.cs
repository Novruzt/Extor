using System.Reflection;
namespace Extor.Interfaces;
/// <summary>
/// Interface for registering custom exception assemblies and retrieving them.
/// </summary>
public interface IExtorRegistrar
{
    /// <summary>
    /// Registers a custom assembly containing exceptions for handling by Extor.
    /// </summary>
    /// <param name="exceptionType">A type from the assembly containing custom exceptions.</param>
    void RegisterCustomAssembly(Type exceptionType);

    /// <summary>
    /// Retrieves the list of registered custom assemblies.
    /// </summary>
    /// <returns>A read-only list of assemblies that have been registered.</returns>
    IReadOnlyList<Assembly> GetCustomAssemblies();
}



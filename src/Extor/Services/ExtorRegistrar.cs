using Extor.Interfaces;
using System.Reflection;

namespace Extor.Services;

/// <summary>
/// Manages the registration and retrieval of custom assemblies containing exception types.
/// </summary>
public class ExtorRegistrar : IExtorRegistrar
{
    private static readonly List<Assembly> CustomAssemblies = new List<Assembly>();


    /// <summary>
    /// Registers a custom assembly containing exception types.
    /// </summary>
    /// <param name="exceptionType">The type of an exception from the assembly to be registered.</param>
    /// <exception cref="ArgumentException">Thrown if the assembly does not contain at least one non-abstract type derived from <see cref="System.Exception"/>.</exception>
    public void RegisterCustomAssembly(Type exceptionType)
    {
        var assembly = exceptionType.Assembly;
        ValidateAssembly(assembly);
        if (!CustomAssemblies.Contains(assembly))
        {
            CustomAssemblies.Add(assembly);
        }
    }


    /// <summary>
    /// Retrieves the list of custom assemblies containing registered exception types.
    /// </summary>
    /// <returns>A read-only list of custom assemblies.</returns>
    public IReadOnlyList<Assembly> GetCustomAssemblies()
    {
        return CustomAssemblies.AsReadOnly();
    }


    /// <summary>
    /// Validates that the given assembly contains at least one non-abstract type derived from <see cref="System.Exception"/>.
    /// </summary>
    /// <param name="assembly">The assembly to validate.</param>
    /// <exception cref="ArgumentException">Thrown if the assembly does not contain any valid exception types.</exception>
    private static void ValidateAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(Exception).IsAssignableFrom(type) && !type.IsAbstract)
            {
                return; 
            }
        }
        throw new ArgumentException("Assembly must contain at least one non-abstract type derived from Exception.");
    }
}

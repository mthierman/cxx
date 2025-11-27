using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Setup.Configuration;

public static class VisualStudio
{
    private static readonly Lazy<IReadOnlyList<ISetupInstance>> _instances = new(GetAllInstances);

    /// <summary>
    /// All installed Visual Studio instances.
    /// </summary>
    public static IReadOnlyList<ISetupInstance> Instances => _instances.Value;

    /// <summary>
    /// The latest installed Visual Studio instance, regardless of edition.
    /// </summary>
    public static ISetupInstance? Latest => Instances
        .OrderByDescending(i => i.GetInstallationVersion())
        .FirstOrDefault();

    /// <summary>
    /// The installation path of the latest Visual Studio instance.
    /// </summary>
    public static string? InstallPath => Latest?.GetInstallationPath();

    /// <summary>
    /// Returns only Build Tools instances.
    /// </summary>
    public static IReadOnlyList<ISetupInstance> BuildTools => Instances
        .Where(i => i.GetInstallationName().Contains("BuildTools", StringComparison.OrdinalIgnoreCase))
        .ToList();

    /// <summary>
    /// Returns the latest Build Tools instance.
    /// </summary>
    public static ISetupInstance? LatestBuildTools => BuildTools
        .OrderByDescending(i => i.GetInstallationVersion())
        .FirstOrDefault();

    private static IReadOnlyList<ISetupInstance> GetAllInstances()
    {
        var setupConfig = new SetupConfiguration();
        var enumInstances = setupConfig.EnumAllInstances();

        IEnumerable<ISetupInstance> Enumerate()
        {
            var buffer = new ISetupInstance[1];
            int fetched;
            do
            {
                enumInstances.Next(1, buffer, out fetched);
                if (fetched > 0) yield return buffer[0];
            } while (fetched > 0);
        }

        return Enumerate().ToList();
    }
}

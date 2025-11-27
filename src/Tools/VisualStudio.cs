using Microsoft.VisualStudio.Setup.Configuration;

namespace cxx;

public static class VisualStudio
{
    private static readonly Lazy<ISetupInstance?> _latest = new(GetLatestInstance);
    public static ISetupInstance? Latest => _latest.Value;
    public static string? InstallPath => Latest?.GetInstallationPath();

    private static ISetupInstance? GetLatestInstance()
    {
        var setupConfig = new SetupConfiguration();
        var enumInstances = setupConfig.EnumAllInstances();

        ISetupInstance? latest = null;
        ISetupInstance[] buffer = new ISetupInstance[1];
        int fetched;

        do
        {
            enumInstances.Next(1, buffer, out fetched);
            if (fetched > 0)
            {
                var instance = buffer[0];
                if (latest == null || string.Compare(instance.GetInstallationVersion(),
                    latest.GetInstallationVersion(), StringComparison.Ordinal) > 0)
                {
                    latest = instance;
                }
            }
        } while (fetched > 0);

        return latest;
    }
}

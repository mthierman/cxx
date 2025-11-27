using System.Diagnostics;

namespace cxx;

public static class VSWhere
{
    public static async Task<int> Run(params string[]? args)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = Project.Tools.VSWhere,
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = false
        };

        return await ExternalCommand.Run(startInfo, args);
    }
}

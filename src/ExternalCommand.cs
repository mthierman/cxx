using System.Diagnostics;

namespace cxx;

public static class ExternalCommand
{
    public static async Task<int> Run(string? command, params string[] args)
    {
        if (string.IsNullOrEmpty(command))
            throw new ArgumentException("command required", nameof(command));

        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = false
        };

        foreach (var a in args ?? Array.Empty<string>())
            startInfo.ArgumentList.Add(a);

        using var process = Process.Start(startInfo)
                      ?? throw new InvalidOperationException("Failed to start process.");

        await process.WaitForExitAsync();

        return process.ExitCode;
    }

    public static int RunVcpkg(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("vcpkg");

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        startInfo.EnvironmentVariables["VCPKG_DEFAULT_TRIPLET"] = "x64-windows-static-md";
        startInfo.EnvironmentVariables["VCPKG_DEFAULT_HOST_TRIPLET"] = "x64-windows-static-md";

        Process.Start(startInfo)?.WaitForExit();

        return 0;
    }
}

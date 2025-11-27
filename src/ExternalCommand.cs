using System.Diagnostics;

namespace cxx;

public static class ExternalCommand
{
    public static async Task<int> Run(string? command, params string[] arguments)
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

        foreach (var argument in arguments ?? Array.Empty<string>())
            startInfo.ArgumentList.Add(argument);

        using var process = Process.Start(startInfo)
                      ?? throw new InvalidOperationException("Failed to start process.");

        await process.WaitForExitAsync();

        return process.ExitCode;
    }

    public static async Task<int> RunVcpkg(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "vcpkg",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = false
        };

        foreach (var argument in arguments ?? Array.Empty<string>())
            startInfo.ArgumentList.Add(argument);

        startInfo.EnvironmentVariables["VCPKG_DEFAULT_TRIPLET"] = "x64-windows-static-md";
        startInfo.EnvironmentVariables["VCPKG_DEFAULT_HOST_TRIPLET"] = "x64-windows-static-md";

        using var process = Process.Start(startInfo)
                      ?? throw new InvalidOperationException("Failed to start process.");

        await process.WaitForExitAsync();

        return process.ExitCode;
    }
}

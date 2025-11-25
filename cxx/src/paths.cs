using System.Diagnostics;

public static class Paths
{
    private static readonly Lazy<EnvironmentPaths> _environmentPaths = new Lazy<EnvironmentPaths>(Initialize);

    public static string vswhere => _environmentPaths.Value.vswhere;
    public static string msbuild => _environmentPaths.Value.msbuild;
    public static string vcpkg => _environmentPaths.Value.vcpkg;
    public static string root => _environmentPaths.Value.root;
    public static string src => Path.Combine(root, "src");
    public static string build => Path.Combine(root, "build");
    public static string solution_file => Path.Combine(root, "build", "app.slnx");
    public static string project_file => Path.Combine(root, "build", "app.vcxproj");

    private sealed record EnvironmentPaths(
        string root,
        string vswhere,
        string msbuild,
        string vcpkg
    );

    private static EnvironmentPaths Initialize()
    {
        var root_path = LocateRepoRoot();

        if (root_path is null)
            throw new FileNotFoundException("cv.jsonc not found in any parent directory");

        var vswhere_path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            @"Microsoft Visual Studio\Installer\vswhere.exe");

        if (!File.Exists(vswhere_path))
            throw new FileNotFoundException($"vswhere.exe not found: {vswhere_path}");

        var vcpkg_root = Environment.GetEnvironmentVariable("VCPKG_ROOT");

        if (string.IsNullOrWhiteSpace(vcpkg_root))
            throw new Exception("VCPKG_ROOT is not set");

        var vcpkg_path = Path.Combine(vcpkg_root, "vcpkg.exe");

        if (!File.Exists(vcpkg_path))
            throw new FileNotFoundException($"vcpkg.exe not found in VCPKG_ROOT: {vcpkg_path}");

        var msbuild_path = LocateMSBuild(vswhere_path);

        return new(root_path, vswhere_path, msbuild_path, vcpkg_path);
    }

    private static string LocateRepoRoot()
    {
        string? current = Environment.CurrentDirectory;

        while (!string.IsNullOrEmpty(current))
        {
            string manifest = Path.Combine(current, "cv.jsonc");
            if (File.Exists(manifest))
                return current;

            var parent = Directory.GetParent(current);
            if (parent == null)
                break;

            current = parent.FullName;
        }

        return null!;
    }

    private static string LocateMSBuild(string vswherePath)
    {
        var psi = new ProcessStartInfo(vswherePath,
            "-latest -requires Microsoft.Component.MSBuild -find MSBuild\\**\\Bin\\amd64\\MSBuild.exe")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(psi)
            ?? throw new InvalidOperationException("vswhere.exe failed to start");

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        string? found = output
            .Split('\r', '\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(found))
            throw new FileNotFoundException("MSBuild.exe not found");

        return found;
    }
}

using System.CommandLine;
using System.Reflection;

public partial class App
{
    public enum ExitCode : int
    {
        Success = 0,
        GeneralError = 1,
    }
    public static string src_dir { get; } = Path.Combine(Environment.CurrentDirectory, "src");
    public static string build_dir { get; } = Path.Combine(Environment.CurrentDirectory, "build");
    public static string version { get; } = Assembly.GetExecutingAssembly()
                  .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                  .InformationalVersion ?? string.Empty;

    public int run(string[] args)
    {
        return root_command.Parse(args).Invoke();
    }
}

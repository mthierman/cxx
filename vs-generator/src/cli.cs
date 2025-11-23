using System.CommandLine;
using System.Diagnostics;

public class App
{
    public static int run(string[] args)
    {
        RootCommand root_command = new("vs-generator");
        Console.WriteLine(root_command.Description);

        Command gen = new("gen", "Generate build files") { };
        root_command.Subcommands.Add(gen);
        gen.SetAction(async parseResult =>
        {
            await MSBuild.generate_project();
        });

        Command build = new("build", "Build project") { };
        root_command.Subcommands.Add(build);
        build.SetAction(async parseResult =>
        {
            var build_dir = Path.Combine(Environment.CurrentDirectory, "build");

            if (Directory.Exists(build_dir))
            {
                using var process = Process.Start(new ProcessStartInfo() { FileName = "msbuild", WorkingDirectory = build_dir });
                process?.WaitForExit();
            }
        });

        return root_command.Parse(args).Invoke();
    }
}

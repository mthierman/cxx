using System.CommandLine;

public class CLI
{
    public static int parse_args(string[]? args)
    {
        RootCommand root_command = new("vs-generator v0.0.0");
        Console.WriteLine(root_command.Description);

        Command build = new("build", "Build") { };
        root_command.Subcommands.Add(build);

        build.SetAction(async parseResult =>
        {
            await MSBuild.generate_project();
        });

        return root_command.Parse(args!).Invoke();
    }
}

using System.CommandLine;

public partial class App
{
    private static RootCommand root_command { get; } = new RootCommand($"vs-generator {version}");
    private static Dictionary<string, Command> commands = new Dictionary<string, Command>
    {
        ["gen"] = new Command("gen", "Generate build"),
        ["build"] = new Command("build", "Build debug"),
        ["release"] = new Command("release", "Build release"),
        ["clean"] = new Command("clean", "Clean build")
    };

    public static int setup_commands(string[] args)
    {
        foreach (var command in commands.Values)
        {
            root_command.Subcommands.Add(command);
        }

        commands["gen"].SetAction(async parseResult =>
        {
            return (await MSBuild.generate()) ? 0 : 1;
        });


        commands["build"].SetAction(async parseResult =>
        {
            return (await MSBuild.generate() && MSBuild.build(MSBuild.BuildConfiguration.Debug)) ? 0 : 1;
        });

        commands["release"].SetAction(async parseResult =>
        {
            return (await MSBuild.generate() && MSBuild.build(MSBuild.BuildConfiguration.Release)) ? 0 : 1;
        });

        commands["clean"].SetAction(async parseResult =>
        {
            Directory.Delete(build_dir, true);
            return 0;
        });

        return root_command.Parse(args).Invoke();
    }
}

#:package Microsoft.Build@18.0.2
using Microsoft.Build.Construction;

var project = ProjectRootElement.Create();
project.Sdk = "Microsoft.NET.Sdk";
project.AddProperty("OutputType", "Exe");
project.AddProperty("TargetFramework", "net7.0");
var itemGroup = project.AddItemGroup();
itemGroup.AddItem("Compile", "Program.cs");
project.Save("out/app.vcxproj");
Console.WriteLine("Project file generated!");

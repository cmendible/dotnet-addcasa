using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace dotnet_addcasa
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Adding CodeAnalysis and StyleCop...");
            var currentDirectory = Directory.GetCurrentDirectory();
            var projects = Directory.GetFiles(currentDirectory, "*.csproj", SearchOption.AllDirectories);
            foreach (var project in projects)
            {
                Console.WriteLine($"Processing: {project}");

                var content = File.ReadAllText(project);
                var caPattern = "<CodeAnalysisRuleSet>ca.ruleset</CodeAnalysisRuleSet>";
                var hasCAPattern = Regex.IsMatch(content, caPattern);

                if (!hasCAPattern)
                {
                    content = content.Replace(
                        "</Project>",
                        "\t<PropertyGroup>\r\n\t\t<CodeAnalysisRuleSet>ca.ruleset</CodeAnalysisRuleSet>\r\n\t</PropertyGroup>\r\n</Project>");

                    File.WriteAllText(project, content, Encoding.UTF8);

                    RunDotNetAddPackageCommand(project, "Microsoft.CodeAnalysis.FxCopAnalyzers");
                    RunDotNetAddPackageCommand(project, "StyleCop.Analyzers");
                };

                var targetFolder = (new FileInfo(project)).DirectoryName;
                var targetRulesetFile = $"{targetFolder}/ca.ruleset";
                if (!File.Exists(targetRulesetFile))
                {
                    var assembly = typeof(Program).GetTypeInfo().Assembly;
                    var resource = assembly.GetManifestResourceStream("dotnet-addcasa.ca.ruleset");
                    var rulesetContent = string.Empty;
                    using (var reader = new StreamReader(resource))
                    {
                        rulesetContent = reader.ReadToEnd();
                    }

                    File.WriteAllText(targetRulesetFile, rulesetContent, Encoding.UTF8);
                }
            }
            Console.WriteLine("Finished adding CodeAnalysis and StyleCop...");
            return 0;
        }

        private static void RunDotNetAddPackageCommand(string project, string packageName)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"add \"{project}\" package {packageName}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine(output);
        }
    }
}

using System;
using System.IO;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace targets
{
    static partial class Program
    {
        private const string EnvVarMissing = " environment variable is missing. Aborting.";
        private const string PackOutput = "./artifacts/pack";
        private const string PackOutputCopy = "../../nuget";
        private const string PublishOutput = "./artifacts/publish";

        private static class Targets
        {
            public const string CleanBuildOutput = "clean-build-output";
            public const string CleanPackOutput = "clean-pack-output";
            public const string Build = "build";
            public const string Test = "test";
            public const string Pack = "pack";
            public const string CopyPackOutput = "copy-pack-output";
            public const string CleanPublishOutput = "clean-publish-output";
            public const string Publish = "publish";

            public const string CreateDockerImages = "create-docker-images";
            public const string PushDockerImages = "push-docker-images";

            public const string CopyOnly = "copy-only";
        }

        // ReSharper disable once UnusedMember.Local
        static void Main(string[] args)
        {
            var version = Read("dotnet", "minver", noEcho: true, echoPrefix: Prefix).Replace("\r", string.Empty).Replace("\n",string.Empty);
            Console.WriteLine("Version used in Build: {0}", version);
            Console.WriteLine($"##vso[build.updatebuildnumber]{version}");

            Target(Targets.CleanBuildOutput,
                () => { Run("dotnet", "clean -c Release -v m --nologo", echoPrefix: Prefix); });

            Target(Targets.Build,
                DependsOn(Targets.CleanBuildOutput),
                () => { Run("dotnet", "build -c Release --nologo", echoPrefix: Prefix); });

            Target(Targets.Test,
                DependsOn(Targets.Build),
                () => { Run("dotnet", $"test -c Release --no-build", echoPrefix: Prefix); });

            Target(Targets.CleanPublishOutput,
                () =>
                {
                    if (Directory.Exists(PublishOutput))
                    {
                        Directory.Delete(PublishOutput, true);
                    }
                });

            Target(Targets.Publish,
                DependsOn(Targets.CleanPublishOutput),
                () =>
                {
                    Directory.CreateDirectory(PublishOutput);

                    foreach (var fileName in Directory.GetFiles("./src", "*.csproj", SearchOption.AllDirectories))
                    {
                        // Console.WriteLine($"file: {fileName}");

                        var projectFile = new FileInfo(fileName);
                        // Console.WriteLine($"fileInfo.DirectoryName: {projectFile.DirectoryName}");
                        // Console.WriteLine($"fileInfo.Directory.Name: {projectFile.Directory?.Name}");
                        // Console.WriteLine($"fileInfo.FullName: {projectFile.FullName}");

                        var publishDirName = Path.Combine(PublishOutput, projectFile.Directory?.Name!);
                        var publishDir = new DirectoryInfo(publishDirName);

                        // Console.WriteLine($"publishOutput: {publishOutput}");
                        // Console.WriteLine($"publishDir.FullName: {publishDir.FullName}");

                        Console.WriteLine($"Publishing Project: {projectFile.FullName} to dir {publishDir.FullName}");

                        Run("dotnet", $"publish {projectFile.FullName} -c Release --no-build --no-restore --output {publishDir.FullName}", echoPrefix: Prefix);    
                        //File.Copy(file, Path.Combine(packOutputCopy, Path.GetFileName(file)), true);
                    }
                });

            Target(Targets.CleanPackOutput,
                () =>
                {
                    if (Directory.Exists(PackOutput))
                    {
                        Directory.Delete(PackOutput, true);
                    }
                });

            Target(Targets.Pack,
                DependsOn(Targets.Build, Targets.CleanPackOutput),
                () =>
                {
                    Directory.CreateDirectory(PackOutput);

                    //var project = Directory.GetFiles("./src", "*.csproj", SearchOption.TopDirectoryOnly).OrderBy(_ => _).First();
                    //Run("dotnet", $"pack {project} -c Release -o {Directory.CreateDirectory(packOutput).FullName} --no-build --nologo", echoPrefix: Prefix);
                });

            Target(Targets.CopyPackOutput,
                DependsOn(Targets.Pack),
                () =>
                {
                    Directory.CreateDirectory(PackOutputCopy);

                    foreach (var file in Directory.GetFiles(PackOutput))
                    {
                        File.Copy(file, Path.Combine(PackOutputCopy, Path.GetFileName(file)), true);
                    }
                });
            
            Target(Targets.CopyOnly,
                () =>
                {
                    foreach (var fileName in Directory.GetFiles("./src", "copy-only.txt", SearchOption.AllDirectories))
                    {
                        // Console.WriteLine($"file: {fileName}");

                        var projectFile = new FileInfo(fileName);
                        Console.WriteLine($"fileInfo.DirectoryName: {projectFile.DirectoryName}");
                        // Console.WriteLine($"fileInfo.Directory.Name: {projectFile.Directory?.Name}");
                        // Console.WriteLine($"fileInfo.FullName: {projectFile.FullName}");

                        if (projectFile.Directory != null)
                        {
                            var sourceFolder = projectFile.Directory.FullName;
                            var destionationFolder = Path.Combine(PublishOutput, projectFile.Directory?.Name!);
                            Console.WriteLine($"Copy Folder: {sourceFolder} to {destionationFolder}");
                            CopyFolder(sourceFolder, destionationFolder);
                        }
                    }
                });
            
            Target(Targets.CreateDockerImages,
                DependsOn(Targets.Publish),
                () =>
                {
                    foreach (var fileName in Directory.GetFiles(PublishOutput, "Dockerfile", SearchOption.AllDirectories))
                    {
                        var dockerfile = new FileInfo(fileName);
                         Console.WriteLine($"Building Dockerfile Project: {dockerfile.FullName}");
                        
                        var tag = $"{ContainerRegistry.ToLower()}/{Prefix.ToLower()}-{dockerfile.Directory?.Name}:latest";
                        var tag2 = $"{ContainerRegistry.ToLower()}/{Prefix.ToLower()}-{dockerfile.Directory?.Name}:{version}";

                        Run("docker", $"build -t {tag} -t {tag2} .", workingDirectory: dockerfile.Directory?.FullName);
                    }
                });

            Target(Targets.PushDockerImages,
                () =>
                {
                    foreach (var fileName in Directory.GetFiles(PublishOutput, "Dockerfile", SearchOption.AllDirectories))
                    {
                        var dockerfile = new FileInfo(fileName);
                        Console.WriteLine($"Building Dockerfile Project: {dockerfile.FullName}");
                        
                        var tag = $"{ContainerRegistry.ToLower()}/{Prefix.ToLower()}-{dockerfile.Directory?.Name}:latest";
                        var tag2 = $"{ContainerRegistry.ToLower()}/{Prefix.ToLower()}-{dockerfile.Directory?.Name}:{version}";

                        Run("docker", $"push {tag}");
                        Run("docker", $"push {tag2}");
                    }
                });

            Target("default", DependsOn(Targets.Test, Targets.CopyPackOutput, Targets.Publish, Targets.CopyOnly, Targets.CreateDockerImages));


            RunTargetsAndExit(args,
                ex => ex is SimpleExec.NonZeroExitCodeException || ex.Message.EndsWith(EnvVarMissing),
                Prefix);
        }
        
        static void CopyFolder( string sourceFolder, string destFolder )
        {
            if (!Directory.Exists( destFolder ))
                Directory.CreateDirectory( destFolder );
            string[] files = Directory.GetFiles( sourceFolder );
            foreach (string file in files)
            {
                string name = Path.GetFileName( file );
                string dest = Path.Combine( destFolder, name );
                File.Copy( file, dest );
            }
            string[] folders = Directory.GetDirectories( sourceFolder );
            foreach (string folder in folders)
            {
                string name = Path.GetFileName( folder );
                string dest = Path.Combine( destFolder, name );
                CopyFolder( folder, dest );
            }
        }
    }
}
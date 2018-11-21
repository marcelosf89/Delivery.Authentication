using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using System;
using System.IO;

namespace Delivery.Authentication.Performance.Tests
{
    public class ManualConfiguration : ManualConfig
    {
        private const string PATH_RESULT = "path";

        public ManualConfiguration()
        {
            Add(Job.Default
                .With(Runtime.Core)
                .With(CsProjCoreToolchain.NetCoreApp21)
                .WithGcConcurrent(false)
                .With(RunStrategy.Monitoring)
                .WithLaunchCount(0)
                .WithWarmupCount(0)
                .WithTargetCount(10));
            ArtifactsPath = GetPath();
            Add(MemoryDiagnoser.Default);
            Add(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions);
            Add(JsonExporter.Default);
            Add(JsonExporter.Custom("-custom", indentJson: true));
        }

        private string GetPath()
        {
            string pathArgument = ConfigurationHelper.Instance.GetArguments(PATH_RESULT, "perfromance");

            Console.WriteLine($"Seting the default artifacts path in  : '{pathArgument}'");
            if (!Directory.Exists(pathArgument))
            {
                Directory.CreateDirectory(pathArgument);
                Console.WriteLine($"Default artifacts path '{pathArgument}' Created!");
            }
            return pathArgument;
        }
    }
}

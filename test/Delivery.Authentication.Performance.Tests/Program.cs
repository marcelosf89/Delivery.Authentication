using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Delivery.Authentication.Performance.Tests.Controllers;

namespace Delivery.Authentication.Performance.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigurationHelper.GetInstance(args);
            Summary summary = BenchmarkRunner.Run(typeof(UsersControllerTests));
            summary = BenchmarkRunner.Run(typeof(ClaimsControllerTests));
        }
    }
}

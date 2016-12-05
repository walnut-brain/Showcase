using System;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime.Configuration;
using Showcase.GrainInterfaces;

namespace Showcase.SiloDevHost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });

            var config = ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(config);

            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.

            Testing();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        static async Task Testing()
        {
            var showcaseName = "test";
            var test = GrainClient.GrainFactory.GetGrain<IShowcaseSet>(showcaseName);
            Console.WriteLine("Retriving point properties...");
            var props = await test.GetPointProperties();
            foreach(var prop in props)
                Console.WriteLine($"{prop.PropertyName} - {prop.PropertyType}");
            Console.WriteLine("Point properties retrived");

            var pi = await test.RegisterPointProperty("Label", typeof(string));
            Console.WriteLine("Retriving point properties...");
            props = await test.GetPointProperties();
            foreach (var prop in props)
                Console.WriteLine($"{prop.PropertyName} - {prop.PropertyType}");
            Console.WriteLine("Point properties retrived");

            try
            {
                await test.RegisterPointProperty("Label", typeof(int));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.GetBaseException().Message}");
            }

        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        private static OrleansHostWrapper hostWrapper;
    }
}

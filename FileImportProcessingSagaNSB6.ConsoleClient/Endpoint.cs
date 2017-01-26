using System;
using System.Threading.Tasks;
using NServiceBus;

namespace FileImportProcessingSagaNSB6.ConsoleClient
{
    public static class Endpoint
    {
        public static IEndpointInstance Instance { get; private set; }

        public static async Task Init()
        {
            Console.Title = "FileImportProcessingSagaNSB6.ConsoleClient";

            var endpointConfiguration = new EndpointConfiguration("FileImportProcessingSagaNSB6.ConsoleClient");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendOnly();

            Instance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
    }
}

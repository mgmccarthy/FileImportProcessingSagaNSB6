using System;
using System.Threading.Tasks;
using FileImportProcessingSagaNSB6.Messages.Commands;
using NServiceBus;

namespace FileImportProcessingSagaNSB6.ConsoleClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            await Endpoint.Init();

            while (Console.ReadLine() != null)
            {
                var importId = Guid.NewGuid();
                const int totalNumberOfFilesInImport = 1000;

                for (var i = 1; i <= totalNumberOfFilesInImport; i++)
                {
                    await Task.Delay(50);
                    Console.WriteLine("Sending ProcessImportFileRow for Customer: {0}", i);

                    var processFileRow = new ProcessImportFileRow
                    {
                        ImportId = importId,
                        CustomerId = i,
                        CustomerName = $"Customer: {i}",
                        FirstImportRowForThisImport = i == 1,
                        TotalNumberOfFilesInImport = totalNumberOfFilesInImport
                    };

                    await Endpoint.Instance.Send(processFileRow);
                }
            }
        }
    }
}

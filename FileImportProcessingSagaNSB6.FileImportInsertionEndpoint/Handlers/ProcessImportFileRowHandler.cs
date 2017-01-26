using System;
using System.Threading.Tasks;
using FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data;
using FileImportProcessingSagaNSB6.Messages.Commands;
using FileImportProcessingSagaNSB6.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Handlers
{
    public class ProcessImportFileRowHandler : IHandleMessages<ProcessImportFileRow>
    {
        private readonly IDataStore dataStore;

        public ProcessImportFileRowHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task Handle(ProcessImportFileRow message, IMessageHandlerContext context)
        {
            if (message.FirstImportRowForThisImport)
            {
                await context.Publish(new FileImportInitiated { ImportId = message.ImportId, TotalNumberOfFilesInImport = message.TotalNumberOfFilesInImport });
            }
            
            //check/validate import data. In the real world, there would be rules run here, db queries, etc... to determine if this row in the import is successfull or not
            var success = new Random().Next(100) % 2 == 0;
            LogManager.GetLogger(typeof(ProcessImportFileRowHandler)).Warn($"Handling ProcessImportFileRow for Customer: {message.CustomerId}");
            using (var session = dataStore.OpenSession())
            {
                session.Add(new FileImport { Id = Guid.NewGuid(), ImportId = message.ImportId, CustomerId = message.CustomerId, CustomerName = message.CustomerName, Successfull = success });
                await session.SaveChangesAsync();
            }
        }
    }
}

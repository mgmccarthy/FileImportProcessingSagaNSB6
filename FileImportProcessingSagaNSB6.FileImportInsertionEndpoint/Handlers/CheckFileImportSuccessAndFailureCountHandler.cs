using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data;
using FileImportProcessingSagaNSB6.Messages.Commands;
using FileImportProcessingSagaNSB6.Messages.InternalMessages;
using NServiceBus;

namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Handlers
{
    public class CheckFileImportSuccessAndFailureCountHandler : IHandleMessages<CheckFileImportSuccessAndFailureCount>
    {
        private readonly IDataStore dataStore;

        public CheckFileImportSuccessAndFailureCountHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task Handle(CheckFileImportSuccessAndFailureCount message, IMessageHandlerContext context)
        {
            int rowsSucceeded;
            int rowsFailed;
            using (var session = dataStore.OpenSession())
            {
                rowsSucceeded = await session.Query<FileImport>().Where(x => x.ImportId == message.ImportId).CountAsync(x => x.Successfull);
                rowsFailed = await session.Query<FileImport>().Where(x => x.ImportId == message.ImportId).CountAsync(x => !x.Successfull);
            }

            //http://stackoverflow.com/questions/22973996/nservicebus-wcf-service-with-error-system-invalidoperationexception-reply-is-ne
            await context.Reply(new FileImportSuccesAndFailureCount { ImportId = message.ImportId, RowsSucceeded = rowsSucceeded, RowsFailed = rowsFailed });
        }
    }
}

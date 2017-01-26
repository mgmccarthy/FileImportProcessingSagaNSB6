using System;
using System.Threading.Tasks;
using FileImportProcessingSagaNSB6.Messages.Commands;
using FileImportProcessingSagaNSB6.Messages.Events;
using FileImportProcessingSagaNSB6.Messages.InternalMessages;
using NServiceBus;
using NServiceBus.Logging;

namespace FileImportProcessingSagaNSB6.SagaEndpoint
{
    public class ProcessImportFileSaga : Saga<ProcessImportFileSaga.SagaData>,
        IAmStartedByMessages<FileImportInitiated>,
        IHandleMessages<FileImportSuccesAndFailureCount>,
        IHandleTimeouts<ProcessImportFileSaga.TimeoutState>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessImportFileSaga));

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<FileImportInitiated>(msg => msg.ImportId).ToSaga(data => data.ImportId);
        }

        public async Task Handle(FileImportInitiated message, IMessageHandlerContext context)
        {
            Log.Warn("Starting ProcessImportFileSaga via FileImportInitiated");

            Data.ImportId = message.ImportId;
            Data.TotalNumberOfFilesInImport = message.TotalNumberOfFilesInImport;

            await SendCheckFileImportSuccessAndFailureCount(message.ImportId, context);
        }

        public async Task Handle(FileImportSuccesAndFailureCount message, IMessageHandlerContext context)
        {
            Log.Warn("handling FileImportSuccesAndFailureCount");
            Log.Warn($"RowsSucceeded: {message.RowsSucceeded}, RowsFailed: {message.RowsFailed}");

            if (message.RowsSucceeded + message.RowsFailed == Data.TotalNumberOfFilesInImport)
            {
                await context.Publish(new FileImportCompleted { ImportId = message.ImportId });
                Log.Warn("Saga Complete");
                MarkAsComplete();
            }
            else
            {
                await RequestTimeout<TimeoutState>(context, TimeSpan.FromSeconds(5));
            }
        }

        public async Task Timeout(TimeoutState state, IMessageHandlerContext context)
        {
            Log.Warn("Sending CheckFileImportSuccessAndFailureCount.");
            await SendCheckFileImportSuccessAndFailureCount(Data.ImportId, context);
        }

        private async Task SendCheckFileImportSuccessAndFailureCount(Guid importId, IMessageHandlerContext context)
        {
            await context.Send(new CheckFileImportSuccessAndFailureCount { ImportId = importId });
        }

        public class SagaData : ContainSagaData
        {
            public Guid ImportId { get; set; }
            public int TotalNumberOfFilesInImport { get; set; }
        }

        public class TimeoutState { }
    }
}
using System;

namespace FileImportProcessingSagaNSB6.Messages.Commands
{
    public class CheckFileImportSuccessAndFailureCount
    {
        public Guid ImportId { get; set; }
    }
}

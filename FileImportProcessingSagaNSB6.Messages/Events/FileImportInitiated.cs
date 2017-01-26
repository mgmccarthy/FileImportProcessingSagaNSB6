using System;

namespace FileImportProcessingSagaNSB6.Messages.Events
{
    public class FileImportInitiated
    {
        public Guid ImportId { get; set; }
        public int TotalNumberOfFilesInImport { get; set; }
    }
}

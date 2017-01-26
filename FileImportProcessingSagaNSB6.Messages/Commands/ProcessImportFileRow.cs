﻿using System;

namespace FileImportProcessingSagaNSB6.Messages.Commands
{
    public class ProcessImportFileRow
    {
        public Guid ImportId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool FirstImportRowForThisImport { get; set; }
        public int TotalNumberOfFilesInImport { get; set; }
    }
}

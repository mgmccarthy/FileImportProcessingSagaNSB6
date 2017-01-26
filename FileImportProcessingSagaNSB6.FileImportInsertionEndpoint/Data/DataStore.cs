namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data
{
    public class DataStore : IDataStore
    {
        public ISession OpenSession()
        {
            return new Session(new FileImportContext());
        }
    }
}

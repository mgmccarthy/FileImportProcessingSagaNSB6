namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data
{
    public interface IDataStore
    {
        ISession OpenSession();
    }
}

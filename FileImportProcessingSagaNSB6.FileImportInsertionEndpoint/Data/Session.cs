using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data
{
    public class Session : ISession
    {
        public readonly DbContext Context;

        public Session(DbContext context)
        {
            Context = context;
            OnSessionCreated(this);
        }

        public static event EventHandler Created;

        private static void OnSessionCreated(Session session)
        {
            Created?.Invoke(session, EventArgs.Empty);
        }

        public void Add<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
        }

        public void AddOrUpdate<T>(T entity) where T : class
        {
            Context.Set<T>().AddOrUpdate(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Context.Set<T>();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}

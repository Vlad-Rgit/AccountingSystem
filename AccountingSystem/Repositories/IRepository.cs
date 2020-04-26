using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Repositories
{
    interface IRepository : IDisposable
    {
        void Add<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        void AddOrAttach<T>(ref T entity, Func<T, bool> predicate) where T : class;
        void Attach<T>(out T entity, Func<T, bool> predicate) where T : class;
        T AddOrAttach<T>(T entity, Func<T, bool> predicate) where T : class;
        T Attach<T>(Func<T, bool> predicate) where T : class;
        void Attach<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete(object entity);
        void Delete<T>(params object[] keys) where T : class;
        T Get<T>(object key) where T : class;
        T GetWhere<T>(Func<T, bool> predicate) where T : class;
        IList<T> GetAll<T>() where T : class;
        int SaveChanges();
    }
}

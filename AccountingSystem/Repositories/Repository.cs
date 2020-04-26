using AccountingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AccountingSystem.Repositories
{
    public class Repository : IRepository
    {
        public enum Error
        {
            UpdateConcurrency = -1,
            Validation = -2,
            Unexpected = -3
        };



        protected readonly Context _context;

        public Repository()
        {
            _context = new Context();

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        //Добавить в базу если нет записи удовлетворяющей predicate,
        //иначе присоеденить entity к DbContext и присвоить Id
        public void AddOrAttach<T>(ref T entity, Func<T,bool> predicate) where T : class
        {
            DbSet<T> set = _context.Set<T>();

            T test = set.FirstOrDefault(predicate);

            if (test == null)
                set.Add(entity);
            else
                entity = test;
        }


        public T AddOrAttach<T>(T entity, Func<T, bool> predicate) where T : class
        {
            DbSet<T> set = _context.Set<T>();

            T test = set.FirstOrDefault(predicate);

            if (test == null)
                set.Add(entity);           
            else
                entity = test;

            return entity;
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Attach<T>(out T entity, Func<T, bool> predicate) where T : class
        {
            entity = _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Attach<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public void Attach<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
        }

        public void Delete(object entity) 
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete<T>(params object[] keys) where T : class
        {
            T entity =  _context.Set<T>().Find(keys);
            _context.Entry(entity).State = EntityState.Deleted;

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public T Get<T>(object key) where T : class
        {
            return _context.Set<T>().Find(key);
        }

        public IList<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().ToList();
        }

        public T GetWhere<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().FirstOrDefault(predicate);

        }

        public IList<T> GetAllWhere<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate).ToList();

        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch(DBConcurrencyException ex)
            {
                return (int)Error.UpdateConcurrency;
            }
            catch(DbEntityValidationException ex)
            {
                return (int)Error.Validation;
            }
            catch(Exception ex)
            {
                return (int)Error.Validation;
            }
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

 
    }
}

using FileUploadApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LocalDbContext _context = null;
        private readonly DbSet<T> table = null;

        public Repository(LocalDbContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public T Add(T obj)
        {
            table.Add(obj);
            return obj;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        /// <summary>
        /// Get All Method Returns List of Records
        /// </summary>
        /// <param name="includeEntity">Use this Parameter to include specified entities explicitly</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(string includeEntity= "")
        {
            if (includeEntity == null || includeEntity == string.Empty)
            {
                return table.ToList(); 
            }
            else
            {
                return table.Include(includeEntity).ToList();
            }
        }
        /// <summary>
        /// To Get Record from database by id.
        /// </summary>
        /// <param name="id">ID odf the record</param>
        /// <returns></returns>
        public T GetById(object id)
        {
            return table.Find(id);
        }

        /// <summary>
        /// To Update the Record in the Database
        /// </summary>
        /// <param name="obj">Record</param>
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        /// <summary>
        /// Save the Record in the Database.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException dbe)
            {
                dbe.ToString();
            }
        }

        /// <summary>
        /// If Record Exists in the Database based which satisfy the condition
        /// </summary>
        /// <param name="pred">Provide the Condition</param>
        /// <returns></returns>
        public bool IfExist(Func<T, bool>pred)
        {
            return table.Any<T>(pred);
        }
        /// <summary>
        /// Filters/Get the Record based on the Predicate
        /// </summary>
        /// <param name="pred">Condition to Filter Record</param>
        /// <returns></returns>
        public IEnumerable<T> GetRecord(Func<T, bool> pred)
        {
           return table.Where<T>(pred);
        }

    }
}

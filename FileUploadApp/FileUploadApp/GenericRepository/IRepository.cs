using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApp.GenericRepository
{
    public interface IRepository<T> where T :class
    {
        /// <summary>
        /// To Get Record from database by id.
        /// </summary>
        /// <param name="id">ID odf the record</param>
        /// <returns></returns>
        T GetById(object id);
        /// <summary>
        /// Get All Method Returns List of Records
        /// </summary>
        /// <param name="includeEntity">Use this Parameter to include specified entities explicitly</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(string includeEntity = "");

        /// <summary>
        /// Adding the Record in the Database
        /// </summary>
        /// <param name="obj">Record</param>
        /// <returns></returns>

        T Add(T obj);
        /// <summary>
        /// To Update the Record in the Database
        /// </summary>
        /// <param name="obj">Record</param>
        void Update(T obj);
        /// <summary>
        /// Delete the Record from the Database by Id
        /// </summary>
        /// <param name="id">Id</param>
        void Delete(object id);
        /// <summary>
        /// Save the Record in the Database.
        /// </summary>
        void Save();

        /// <summary>
        /// If Record Exists in the Database based which satisfy the condition
        /// </summary>
        /// <param name="pred">Provide the Condition</param>
        /// <returns></returns>
        bool IfExist(Func<T, bool> pred);

        /// <summary>
        /// Filters/Get the Record based on the Predicate
        /// </summary>
        /// <param name="pred">Condition to Filter Record</param>
        /// <returns></returns>
        IEnumerable<T> GetRecord(Func<T, bool> pred);


    }
}

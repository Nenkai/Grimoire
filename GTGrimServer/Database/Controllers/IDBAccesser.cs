using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Controllers
{
    interface IDBManager<T>
    {
        /// <summary>
        /// Adds a new entityto the database.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Row ID.</returns>
        Task<long> AddAsync(T t);

        /// <summary>
        /// Removes an entity from the database.
        /// </summary>
        /// <param name="t"></param>
        Task RemoveAsync(long t);

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIDAsync(long id);

        /// <summary>
        /// Updates the entity in the database.
        /// </summary>
        /// <param name="t"></param>
        Task UpdateAsync(T t);
    }
}

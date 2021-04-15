using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GTGrimServer.Database.Controllers
{
    interface IDBManager<T>
    {
        /// <summary>
        /// Adds a new entity and saves the database.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Row ID.</returns>
        Task<long> AddAsync(T t);

        /// <summary>
        /// Removes an entity and saves the database.
        /// </summary>
        /// <param name="t"></param>
        Task RemoveAsync(ulong t);

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIDAsync(long id);

        /// <summary>
        /// Updates and saves the entity.
        /// </summary>
        /// <param name="t"></param>
        Task UpdateAsync(T t);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GTGrimServer.Database.Controllers
{
    interface IDBManager<T>
    {
        /// <summary>
        /// Adds a new entity and saves the database.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        void Add(T t);

        /// <summary>
        /// Removes an entity and saves the database.
        /// </summary>
        /// <param name="t"></param>
        void Remove(ulong t);

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetByID(long id);

        /// <summary>
        /// Updates and saves the entity.
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);
    }
}

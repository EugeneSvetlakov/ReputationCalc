using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteStorage
{
    public class DbInitializer
    {
        /// <summary>
        /// Starting initializer of Db. If not exist then database and all its schema are created.
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(SqliteDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}

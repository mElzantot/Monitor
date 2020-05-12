using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(params object[] id);
        TEntity Add(TEntity entitiy);
        TEntity Edit(TEntity entitiy);
        bool Delete(params object[] id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Base
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> FindAllAsync();

        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        int GetHashCode();
    }
}
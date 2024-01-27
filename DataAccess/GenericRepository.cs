using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppicationDbContext _dbContext;
        public GenericRepository(AppicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
           _dbContext.Set<T>().Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            if (includes == null) //We are not joining other objects
            {
                if(!trackChanges == null) //is false 
                {
                    return _dbContext.Set<T>()
                        .Where(predicate)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
                else //we are tracking changes
                {
                    return _dbContext.Set<T>()
                       .Where(predicate)
                       .FirstOrDefault();

                }
            }
            else //we have includes to deal with
            {
                //includes == "comma,separate,objects,without,spaces"
               IQueryable<T> queryable =  _dbContext.Set<T>();
                foreach (var includeProperty in includes.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }

                if (!trackChanges == null) //is false 
                {
                    return _dbContext.Set<T>()
                        .Where(predicate)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
                else //we are tracking changes
                {
                    return _dbContext.Set<T>()
                       .Where(predicate)
                       .FirstOrDefault();

                }

            }
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if(predicate != null && includes == null)
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            else if(includes != null) //has includes
            {
                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return queryable.OrderBy(orderBy).ToList();
                }
            }
            else
            {
                if(orderBy == null)
                {
                    return queryable
                        .Where(predicate)
                        .ToList();
                }
                else
                {
                    return queryable
                        .Where(predicate)
                        .OrderBy(orderBy).ToList();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>>? orderBy = null, string? includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null)
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            else if (includes != null) //has includes
            {
                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }
            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return await queryable.OrderBy(orderBy).ToListAsync();
                }
            }
            else
            {
                if (orderBy == null)
                {
                    return await queryable
                        .Where(predicate)
                        .ToListAsync();
                }
                else
                {
                    return await queryable
                        .Where(predicate)
                        .OrderBy(orderBy).ToListAsync();
                }
            }
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string? includes = null)
        {
            if (includes == null) //We are not joining other objects
            {
                if (!trackChanges) //is false 
                {
                    return await _dbContext.Set<T>()
                        .Where(predicate)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                }
                else //we are tracking changes
                {
                    return await _dbContext.Set<T>()
                       .Where(predicate)
                       .FirstOrDefaultAsync();
                }
            }
            else //we have includes to deal with
            {
                //includes == "comma,separate,objects,without,spaces"
                IQueryable<T> queryable = _dbContext.Set<T>();
                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
                if (!trackChanges) //is false 
                {
                    return queryable
                        .Where(predicate)
                        .AsNoTracking()
                        .FirstOrDefault();
                }
                else //we are tracking changes
                {
                    return queryable
                        .Where(predicate)
                        .FirstOrDefault();
                }
            }
        }

        public virtual T GetById(int? id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            //For track changed I'm flagging modified ot the system
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}

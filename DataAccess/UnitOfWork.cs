using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppicationDbContext _dbContext;
        public UnitOfWork(AppicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<Category> _Category;

        public IGenericRepository<Manufacturer> _Manufacturer;

        public IGenericRepository<Category> Category
        {
            get
            {
                if(_Category == null)
                {
                    _Category = new GenericRepository<Category>(_dbContext);
                }
                return _Category;
            }
        }
        public IGenericRepository<Manufacturer> Manufacturer
        {
            get
            {
                if (_Manufacturer == null)
                {
                    _Manufacturer = new GenericRepository<Manufacturer>(_dbContext);
                }
                return _Manufacturer;
            }
        }

        public async Task<int> ComitAsync()
        {
            return await  _dbContext.SaveChangesAsync();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

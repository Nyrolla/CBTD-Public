using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public  interface IUnitOfWork
    {
        public IGenericRepository<Category> Category {  get; }
        public IGenericRepository<Manufacturer> Manufacturer { get; }
        //Add other modesl/tables here are you create them
        //So UnitOfWork has access to them

        //save changes to the data source

        int Commit();

        Task<int> ComitAsync();


    }
}

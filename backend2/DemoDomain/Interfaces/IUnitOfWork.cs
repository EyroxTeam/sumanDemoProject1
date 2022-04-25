using DemoData.Models;

using DemoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        GenericRepository<Role> Roles {get;}
        GenericRepository<Employee> Employees {get;}


        int Complete();
    }
}

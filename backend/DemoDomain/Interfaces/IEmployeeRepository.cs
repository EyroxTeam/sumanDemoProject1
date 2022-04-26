using DemoDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {

  //    public Employee GetEmployeeRecord(int skipCount, int maxResultCount, string search);

    }

}

using DemoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {

         List<Employee> GetAllEmployee(int skipCount, int maxResultCount, string search);
         public string AddEmployee(Employee _object);
         public string UpdateEmployee(Employee _object);
         public string DeleteEmployee(int id);
         Task <Employee> EmployeeGetById(int id);
    }

}

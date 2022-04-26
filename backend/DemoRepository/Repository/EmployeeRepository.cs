using DemoDomain.Interfaces;
using DemoDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRepository.Repository
{
  public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
  {

   
   

    public EmployeeRepository(DemoDbContext context) : base(context)
    {

   
    }


  }
}

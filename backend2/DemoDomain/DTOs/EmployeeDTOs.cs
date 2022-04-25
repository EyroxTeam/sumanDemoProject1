using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.DTOs
{
    public class EmployeeDTOs
    {
        public int Id { get; set; }
    
        public string Name { get; set; }

        public string Email { get; set; }
       
        public int? Age { get; set; }
       
        public string Department { get; set; }
 
        public int? RoleId { get; set; }
    }
}

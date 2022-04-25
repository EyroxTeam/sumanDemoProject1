using DemoData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public interface IRoleRepository: IGenericRepository<Role>
    {
        List<Role> GetAllRole(int skipCount, int maxResultCount, string search);
        public string AddRole(Role _object);
        public string UpdateRole(Role _object);
        public string DeleteRole(int id);
        Task<Role> RoleGetById(int id);
    }
}

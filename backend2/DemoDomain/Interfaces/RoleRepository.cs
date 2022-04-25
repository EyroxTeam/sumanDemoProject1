using DemoData.Models;
using DemoDomain.Interfaces;
using DemoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly IUnitOfWork unitOfWork;
        public RoleRepository(DemoDbContext context, IUnitOfWork unitOfWork) : base(context)
        {

            this.unitOfWork = unitOfWork;
        }
        public List<Role> GetAllRole(int skipCount, int maxResultCount, string search)
        {
            if (maxResultCount == 0)
            {
                maxResultCount = 10;
            }
            string test = string.Empty;
            search = search?.ToLower();
            int totalRecord = unitOfWork.Employees.GetAll().Result.Count();
            if (totalRecord > 0)
            {
                var roles = new List<Role>();
                if (!string.IsNullOrEmpty(search))
                {
                    roles = unitOfWork.Roles.GetAll().Result.Where(a => a.Name.ToLower().Contains(search)
              ).OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x=>x.IsDeleted==false).ToList();
                    return roles;

                }
                else
                {
                    roles = unitOfWork.Roles.GetAll().Result.OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList();
                    return roles;
                }
            }
            else
            {

                return null;

            }
        }
        public string AddRole(Role obj)
        {

            var GetRoles = new List<Role>();

            if (!string.IsNullOrEmpty(obj.Name))

                GetRoles = unitOfWork.Roles.GetAll().Result.ToList();
            if (GetRoles != null)
            {
                var GetEmp = GetRoles.Where(m => m.Name == obj.Name).FirstOrDefault();
                if (GetEmp != null)
                {
                    return "Role Already Exists";
                }
                else
                {
                    obj.CreatedDate = DateTime.Now;
                    obj.IsDeleted = false;
                    var result = unitOfWork.Roles.Add(obj);
                    var Record = unitOfWork.Complete();
                    if (result is not null)
                    {

                        return "Role Added Succeffully";
                    }
                    else
                    {
                        return "Unable to Add";
                    }
                }
            }
            else
            {
                obj.CreatedDate = DateTime.Now;
                obj.IsDeleted = false;
                var result = unitOfWork.Roles.Add(obj);
                unitOfWork.Complete();
                if (result is not null)
                {

                    return "Role Added Succeffully";
                }
                else
                {
                    return "unable to Add";
                }
            }
            return "unable to Add";

        }
        public string UpdateRole(Role obj)
        {
            try
            {
                if (obj.ModifiedBy == null)
                {
                    return "Modifiedby Id Required";
                }
                obj.ModifiedDate = DateTime.Now;
                unitOfWork.Roles.Update(obj);
                unitOfWork.Complete();
                return "Role Updated Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string DeleteRole(int id)
        {
            try
            {
                
                var obj = unitOfWork.Roles.Get(id);
                obj.Result.DeletedDate = DateTime.Now;
                obj.Result.IsDeleted = true;
                unitOfWork.Roles.Update(obj.Result);
                unitOfWork.Complete();
                return "Role Deleted Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Role> RoleGetById(int id)
        {

            var GetRole = unitOfWork.Roles.Get(id);
            if (GetRole != null)
            {
                return await GetRole;
            }
            return null;
        }
    }

}


using DemoData.Models;
using DemoDomain.Interfaces;
using DemoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        private readonly IUnitOfWork unitOfWork;
        public EmployeeRepository(DemoDbContext context, IUnitOfWork unitOfWork) : base(context)
        {

            this.unitOfWork = unitOfWork;
        }
        public List<Employee> GetAllEmployee(int skipCount, int maxResultCount, string search)
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
                var employees = new List<Employee>();
                if (!string.IsNullOrEmpty(search))
                {
                    employees = unitOfWork.Employees.GetAll().Result.Where(a => a.Name.ToLower().Contains(search) || a.Email.ToLower().Contains(search) || a.Department.StartsWith(search)
                    ).OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x=>x.IsDeleted==false).ToList();
                    return employees;

                }
                else
                {
                    employees = unitOfWork.Employees.GetAll().Result.OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList(); ;
                    return employees;
                }
            }
            else
            {

                return null;

            }
        }
        public string AddEmployee(Employee obj)
        {

            var Getemployees = new List<Employee>();
           
            if (!string.IsNullOrEmpty(obj.Email))

            Getemployees = unitOfWork.Employees.GetAll().Result.ToList();
            if (Getemployees != null)
            {
                var GetEmp = Getemployees.Where(m => m.Email == obj.Email).FirstOrDefault();
                if (GetEmp != null)
                {
                    return "Employee Already Exists"; 
                }
                else
                {
                    obj.CreatedDate = DateTime.Now;
                    obj.IsDeleted = false;
                    var result = unitOfWork.Employees.Add(obj);
                    var Record = unitOfWork.Complete();
                    if (result is not null) {

                        return "Employee Added Succeffully";
                    }
                    else {
                        return "Unable to Add";
                    }
                }
            }
            else
            {
                obj.CreatedDate = DateTime.Now;
                obj.IsDeleted = false;
                var result = unitOfWork.Employees.Add(obj);
                unitOfWork.Complete();
                if (result is not null)
                {

                    return "Employee Added Succeffully";
                }
                else
                {
                    return "unable to Add";
                }
            }
            return "unable to Add";

        }
        public string UpdateEmployee(Employee obj)
        {
            try
            {
                if (obj.ModifiedBy == null) 
                {
                   return "Modifiedby Id Required"; 
                }
                obj.ModifiedDate = DateTime.Now;
                unitOfWork.Employees.Update(obj);
                unitOfWork.Complete();
                return "Employee Updated Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string DeleteEmployee(int id)
        {
            try
            {
                var obj = unitOfWork.Employees.Get(id);
                obj.Result.DeletedDate = DateTime.Now;
                obj.Result.IsDeleted = true;
                unitOfWork.Employees.Update(obj.Result);
                unitOfWork.Complete();
                return "Employee Deleted Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Employee> EmployeeGetById(int id) 
        {

            var GetEmployee = unitOfWork.Employees.Get(id);
            if (GetEmployee!=null)
            {
                return await GetEmployee;
            }
            return null;
        }

    }
}

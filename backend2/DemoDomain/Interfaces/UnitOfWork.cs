
using DemoData.Models;

using DemoRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        public DemoDbContext _context;
        public UnitOfWork(DemoDbContext _context)
        {
            this._context = _context;
        }

        // public IRoleRepository Roles { get; } 
        // public IEmployeeRepository Employees { get; }

        private GenericRepository<Role> _Roles;
        public GenericRepository<Role> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    _Roles = new GenericRepository<Role>(_context);
                }
                return _Roles;
            }
        }
        private GenericRepository<Employee> _Employees;
        public GenericRepository<Employee> Employees
        {
            get
            {
                if (_Employees == null)
                {
                    _Employees = new GenericRepository<Employee>(_context);
                }
                return _Employees;
            }
        }

        //public UnitOfWork(DemoDbContext eformDbContext,
        //    IRoleRepository catalogueRepository,IEmployeeRepository employeeRepository)
        //{
        //    this._context = eformDbContext;
        //    this.Roles = catalogueRepository;
        //    this.Employees = employeeRepository;
        //}
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}

using DemoDomain.Interfaces;
using DemoRepository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDomain.Interfaces
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DemoDbContext _context;

        public GenericRepository(DemoDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public async Task<T> Get(string name)
        {
            return await _context.Set<T>().FindAsync(name);
        }
    }
}

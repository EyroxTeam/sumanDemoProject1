using DemoDomain.Interfaces;
using DemoRepository.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoAPIS.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {

            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();

            services.AddDbContext<DemoDbContext>(opt => opt
                .UseSqlServer("Server=DESKTOP-602K13J\\SQLEXPRESS; Database=DemoAPIS;Trusted_Connection=True;"));
            services.AddMvc()
            .AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddMemoryCache();

            return services;
        }

    }
}

using DemoDomain.Interfaces;
using DemoDomain.Repository;
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
                .UseSqlServer("Server=DESKTOP-F6QIM5U; Database=DemoAPIS;Trusted_Connection=True;"));
            services.AddMvc()
            .AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddMemoryCache();

            return services;
        }

    }
}
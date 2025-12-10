using CDS_DAL.Context;
using CDS_DAL.RepositorySQL;
using CDS_Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDB(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SqlDbContext>(opt => opt.UseSqlServer(connectionString));
            services.AddScoped<IDbRepos, DbReposSQL>();

            return services;
        }
    }
}

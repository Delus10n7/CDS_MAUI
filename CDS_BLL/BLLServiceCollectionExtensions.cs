using Microsoft.Extensions.DependencyInjection;
using CDS_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL
{
    public static class BLLServiceCollectionExtensions
    {
        public static IServiceCollection AddDBFromBLL(this IServiceCollection services, string connectionString)
        {
            services.AddDB(connectionString);

            return services;
        }
    }
}

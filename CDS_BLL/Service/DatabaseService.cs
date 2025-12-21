using CDS_DAL.Context;
using CDS_Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlDbContext _context;

        public DatabaseService(SqlDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
        {
            await _context.InitializeDatabaseAsync(cancellationToken);
        }

        public async Task<bool> CheckDatabaseExistsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.DatabaseExistsAsync(cancellationToken);
        }
    }
}

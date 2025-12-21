using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IDatabaseService
    {
        Task InitializeDatabaseAsync(CancellationToken cancellationToken = default);
        Task<bool> CheckDatabaseExistsAsync(CancellationToken cancellationToken = default);
    }
}

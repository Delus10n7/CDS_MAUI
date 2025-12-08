using CDS_DAL.Context;
using CDS_Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_DAL.RepositorySQL
{
    public class ReportRepositorySQL : IReportRepository
    {
        private SqlDbContext db;

        public ReportRepositorySQL(SqlDbContext dbContext)
        {
            this.db = dbContext;
        }


    }
}

using CDS_Interfaces.DTO;
using CDS_Interfaces.Repository;
using CDS_Interfaces.Service;
using CDS_DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_BLL.Service
{
    public class ReportService : IReportService
    {
        private IDbRepos db;
        public ReportService(IDbRepos repos)
        {
            this.db = repos;
        }
    }
}

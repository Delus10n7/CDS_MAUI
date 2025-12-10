using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface IModelService
    {
        List<ModelDTO> GetAllModels();
        ModelDTO GetModel(int Id);
        void CreateModel(ModelDTO m);
        void UpdateModel(ModelDTO m);
        void DeleteModel(int Id);
        bool Save();
    }
}

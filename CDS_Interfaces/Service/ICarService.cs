using CDS_Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_Interfaces.Service
{
    public interface ICarService
    {
        List<CarDTO> GetAllCars();
        CarDTO GetCar(int Id);
        void CreateCar(CarDTO c);
        void UpdateCar(CarDTO c);
        void DeleteCar(int Id);
        bool Save();
    }
}

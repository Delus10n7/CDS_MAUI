using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class OrderModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public string DisplayInfo => $"{Brand} {Model}";
        public string FormattedDate => Date.ToString("dd.MM.yyyy HH:mm");
        public string FormattedPrice => $"{Price:N0} руб.";
        public string ShortDate => Date.ToString("dd.MM.yyyy");
    }
}

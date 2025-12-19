using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class AdditionalServiceItemModel
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public string? FormattedQuantity => Quantity?.ToString() + " шт." ?? string.Empty;
        public string? FormattedPrice => Price?.ToString("N0") + " руб." ?? string.Empty;

        public AdditionalServiceItemModel() { }
        public AdditionalServiceItemModel(string name, int? quantity, decimal? price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}

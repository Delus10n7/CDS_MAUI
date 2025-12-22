using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS_MAUI.Models
{
    public class DiscountModel
    {
        public string Name { get; set; }
        public string Percent { get; set; }

        public string FormattedPercent => Percent + " %";

        public DiscountModel() { }
        public DiscountModel(string discountName, string discountPercent)
        {
            Name = discountName;
            Percent = discountPercent;
        }
    }
}

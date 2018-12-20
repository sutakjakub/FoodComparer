using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodComparer.Data
{
    public class FoodResult
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public EnumProviders ProviderType { get; set; }
    }
}

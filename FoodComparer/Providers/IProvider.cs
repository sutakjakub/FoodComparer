using FoodComparer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodComparer.Providers
{
    public interface IProvider
    {
        IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names);
    }
}

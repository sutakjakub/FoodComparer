using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodComparer.Data
{
    public enum EnumProviders
    {
        [Description("Kolkovna stodůlky")]
        Kolkovna = 1,
        [Description("Pivovar Řeporyje")]
        Reporyje = 2,
        [Description("Restaurant Maják")]
        Majak = 3,
        [Description("Bernard Pub")]
        BernardPub = 4
    }
}

using DevExpress.Mvvm;
using FoodComparer.Data;
using FoodComparer.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FoodComparer.ViewModels
{
    public class ProviderViewModel : ViewModelBase
    {
        public virtual EnumProviders ProviderType { get; protected set; }
        public virtual string Url { get; protected set; }
        public IProvider Provider { get; protected set; }
        public virtual string ExceptionMessage { get; protected set; }

        public virtual string State { get; set; }
        public virtual SolidColorBrush StateForeground { get; set; }

        public ProviderViewModel(IProvider provider)
        {
            Provider = provider;
            ProviderType = (provider as AbstractProvider).ProviderType;
            Url = (provider as AbstractProvider).Url;
            State = "...";
        }

        public void ShowHttpPage()
        {
            Process.Start(Url);
        }

        public IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names)
        {
            try
            {
                State = "...";

                var result = Provider.GetFoodResults(date, names);
                State = "OK";
                StateForeground = Brushes.Green;

                return result;
            }
            catch (Exception ex)
            {
                State = "CHYBA";
                StateForeground = Brushes.Red;
                ExceptionMessage = ex.Message;

                return new List<FoodResult>();
            }
        }
    }
}

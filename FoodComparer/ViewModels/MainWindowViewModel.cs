using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FoodComparer.Data;
using FoodComparer.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodComparer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public virtual ObservableCollection<string> SearchStrings { get; set; }
        public virtual ObservableCollection<FoodResult> Foods { get; set; }

        public virtual ObservableCollection<ProviderViewModel> Providers { get; set; }


        public virtual string Status { get; set; }
        public virtual string SearchString { get; set; }
        public virtual DateTime Date { get; set; }

        public MainWindowViewModel()
        {
            SearchStrings = new ObservableCollection<string>();
            SearchStrings.Add("kachn");
            SearchStrings.Add("král");
            SearchStrings.Add("vývar");

            Date = DateTime.Today;
            Status = "Připraveno";

            InitializeProviders();
        }

        private void InitializeProviders()
        {
            Providers = new ObservableCollection<ProviderViewModel>();

            ProviderViewModel vm = ViewModelSource.Create(() => new ProviderViewModel(new BernardPubProvider(ConfigurationManager.AppSettings["bernardPubProviderUrl"])));
            Providers.Add(vm);
            vm = ViewModelSource.Create(() => new ProviderViewModel(new KolkovnaProvider(ConfigurationManager.AppSettings["kolkovnaProviderUrl"])));
            Providers.Add(vm);
            vm = ViewModelSource.Create(() => new ProviderViewModel(new ReporyjeProvider(ConfigurationManager.AppSettings["reporyjeProviderUrl"])));
            Providers.Add(vm);
            vm = ViewModelSource.Create(() => new ProviderViewModel(new MajakProvider(ConfigurationManager.AppSettings["majakProviderUrl"])));
            Providers.Add(vm);
        }

        public void Add()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchStrings.Add(SearchString);
                SearchString = string.Empty;
            }
        }

        public void Remove(object item)
        {
            SearchStrings.Remove(item as string);
        }

        public Task Search()
        {
            Status = "Vyhledávám ...";

            return Task.Run(() =>
            {
                List<IProvider> providers = new List<IProvider>();

                List<FoodResult> result = new List<FoodResult>();
                foreach (var provider in Providers)
                {
                    try
                    {
                        result.AddRange(provider.GetFoodResults(Date, SearchStrings));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                Foods = new ObservableCollection<FoodResult>(result);
                Status = $"Bylo nalezeno {Foods.Count} jídel v {DateTime.Now.ToLongTimeString()}";
            });
        }
    }
}

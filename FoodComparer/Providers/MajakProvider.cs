using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodComparer.Data;
using HtmlAgilityPack;

namespace FoodComparer.Providers
{
    public class MajakProvider : AbstractProvider, IProvider
    {
        public override EnumProviders ProviderType => EnumProviders.Majak;

        private readonly string tableXPath = @"//*[@id=""content""]/div[2]/table";

        public MajakProvider(string url) : base(url)
        {
        }

        public IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names)
        {
            //download html
            var doc = DownloadHtml();

            //find correct node by date
            return FindByDate(doc, date, names);
        }

        private IList<FoodResult> FindByDate(HtmlDocument doc, DateTime date, IEnumerable<string> names)
        {
            List<FoodResult> result = new List<FoodResult>();

            FoodResult food;
            string name;
            HtmlNodeCollection thCollection;
            bool inDayLoop = false;
            int inOneDay = 9;
            int i = 1;
            foreach (var node in GetNode(doc, tableXPath).SelectNodes("tr"))
            {
                if (!inDayLoop)
                {
                    thCollection = node.SelectNodes("th");
                    if (thCollection?.Count == 2)
                    {
                        if (IsSameDay(date, thCollection[1].InnerText))
                        {
                            inDayLoop = true;
                        }
                    }
                }
                else
                {
                    name = node.SelectNodes("td")[1].InnerText;
                    if (IsContains(names, name))
                    {
                        food = new FoodResult();
                        food.Name = name;
                        food.Price = ConvertToPrice(node.SelectNodes("td")[2].InnerText);
                        food.ProviderType = ProviderType;

                        result.Add(food);
                    }
                    if (i == inOneDay)
                    {
                        break;
                    }
                    i++;
                }
            }

            return result;
        }

        private bool IsSameDay(DateTime date, string dateStr)
        {
            DateTime convertedDate = ConvertToDate(dateStr);
            return date == convertedDate;
        }
    }
}

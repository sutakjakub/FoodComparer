using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodComparer.Data;
using HtmlAgilityPack;

namespace FoodComparer.Providers
{
    public class BernardPubProvider : AbstractProvider, IProvider
    {
        public override EnumProviders ProviderType => EnumProviders.BernardPub;

        private readonly string daysXPath = @"//*[@id=""content""]/section[3]/div/div/div/div/ul/li[{0}]";

        public BernardPubProvider(string url) : base(url)
        {
        }

        public IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names)
        {
            //download html
            var doc = DownloadHtml();

            //find correct node by date
            string id = FindNodeIdByDate(doc, date);
            if (id == null)
                throw new InvalidOperationException($"V jídeláku nebylo nalezeno datum {date.ToShortDateString()}");

            //parse food and use contains for names
            return ConvertToFoodResult(doc, id, names);
        }

        private IList<FoodResult> ConvertToFoodResult(HtmlDocument doc, string id, IEnumerable<string> names)
        {
            List<FoodResult> result = new List<FoodResult>();

            string name;
            FoodResult food;
            var n = GetNode(doc, $@"//*[@id=""{id}""]");
            //n.SelectNodes(@"//*[@id=""{id}""]/div[1]/ul/li[1]/div"
            foreach (var node in n.Descendants("div").Where(d => d.Attributes["class"].Value.Contains("single-food")))
            {
                name = node.SelectSingleNode("strong").InnerText;
                if (IsContains(names, name))
                {
                    food = new FoodResult();
                    food.Name = name;
                    food.Price = ConvertToPrice(node.SelectSingleNode("span[@class='food-price']").InnerText);
                    food.ProviderType = ProviderType;

                    result.Add(food);
                }
            }
            //foreach (var item in collection)
            //{

            //}

            return result;
        }

        private string FindNodeIdByDate(HtmlDocument doc, DateTime date)
        {
            HtmlNode dayNode;
            for (int day = 1; day <= 5; day++)
            {
                dayNode = GetNode(doc, string.Format(daysXPath, day));
                if (dayNode != null && IsSameDay(date, dayNode.SelectSingleNode("span").InnerText))
                {
                    return dayNode.Attributes["data-tab-target"].Value;
                }
            }

            return null;
        }

        private bool IsSameDay(DateTime date, string dateString)
        {
            dateString = $"{dateString.Replace(" ", "")}{DateTime.Now.Year}";

            DateTime convertedDate = ConvertToDate(dateString);
            return date == convertedDate;
        }
    }
}

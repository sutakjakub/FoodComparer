using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodComparer.Data;
using HtmlAgilityPack;

namespace FoodComparer.Providers
{
    public class KolkovnaProvider : AbstractProvider, IProvider
    {
        public override EnumProviders ProviderType => EnumProviders.Kolkovna;

        private readonly string headerXPath = @"//*[@id=""main""]/div/div[1]/article/div[2]/section[{0}]/h2";
        private readonly string tableXPath = @"//*[@id=""main""]/div/div[1]/article/div[2]/section[{0}]/table";

        public KolkovnaProvider(string url) : base(url)
        {
        }

        public IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names)
        {
            //download html
            var doc = DownloadHtml();

            //find correct node by date
            var tableNode = FindByDate(doc, date);

            //parse food and use contains for names
            return ConvertToFoodResult(doc, tableNode, names);
        }

        private IList<FoodResult> ConvertToFoodResult(HtmlDocument doc, HtmlNode tableNode, IEnumerable<string> names)
        {
            List<FoodResult> result = new List<FoodResult>();

            FoodResult food;
            string name;
            foreach (var row in tableNode.SelectNodes("tr"))
            {
                name = row.SelectSingleNode("td[@class='name']").InnerText;
                if (IsContains(names, name))
                {
                    food = new FoodResult();
                    food.Name = name.Split('|')[0];
                    food.Price = ConvertToPrice(row.SelectSingleNode("td[@class='price']").InnerText);
                    food.ProviderType = ProviderType;

                    result.Add(food);
                }
            }

            return result;
        }

        private HtmlNode FindByDate(HtmlDocument doc, DateTime date)
        {
            HtmlNode node;
            for (int sectionNumber = 1; sectionNumber <= 5; sectionNumber++)
            {
                node = GetNode(doc, string.Format(headerXPath, sectionNumber));
                if (node != null)
                {
                    if (IsSameDay(date, node))
                    {
                        return GetNode(doc, string.Format(tableXPath, sectionNumber));
                    }
                }
            }

            return null;
        }

        private bool IsSameDay(DateTime date, HtmlNode node)
        {
            string s = node.InnerText;
            s = s.Replace(" ", "");

            string dateString = s.Split('-')[1];

            DateTime convertedDate = ConvertToDate(dateString);
            return date == convertedDate;
        }
    }
}

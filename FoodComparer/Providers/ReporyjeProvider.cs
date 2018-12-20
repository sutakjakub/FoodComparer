using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FoodComparer.Data;
using HtmlAgilityPack;

namespace FoodComparer.Providers
{
    public class ReporyjeProvider : AbstractProvider, IProvider
    {
        private readonly string divXpath = @"//*[@id=""wrap-content""]/div/div/div/div";

        public ReporyjeProvider(string url) : base(url)
        {
        }

        public override EnumProviders ProviderType => EnumProviders.Reporyje;

        public IList<FoodResult> GetFoodResults(DateTime date, IEnumerable<string> names)
        {
            //download html
            var doc = DownloadHtml();

            IList<HtmlNode> foodNodes = GetFoodNodes(doc);

            //parse food and use contains for names
            return FindFood(foodNodes, names);
        }

        private IList<FoodResult> FindFood(IList<HtmlNode> foodNodes, IEnumerable<string> names)
        {
            List<FoodResult> result = new List<FoodResult>();

            string name;
            FoodResult food;
            foreach (var foodNode in foodNodes)
            {
                name = foodNode.InnerText;

                if (IsContains(names, name))
                {
                    food = new FoodResult();
                    food.Name = Regex.Replace(name.Replace(",-", ""), @"[\d]", string.Empty);

                    food.Price = ConvertToPrice(foodNode.SelectSingleNode("strong").InnerText);
                    food.ProviderType = ProviderType;

                    result.Add(food);
                }
            }

            return result;
        }

        private IList<HtmlNode> GetFoodNodes(HtmlDocument doc)
        {
            var rootNode = GetNode(doc, divXpath);
            return rootNode.SelectNodes("p");
        }
    }
}

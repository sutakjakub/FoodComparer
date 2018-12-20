using FoodComparer.Data;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodComparer.Providers
{
    public abstract class AbstractProvider
    {
        public readonly string Url;
        public abstract EnumProviders ProviderType { get; }

        public AbstractProvider(string url)
        {
            Url = url;
        }

        protected HtmlDocument DownloadHtml()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = hw.Load(new Uri(Url, UriKind.RelativeOrAbsolute));

            return doc;
        }

        protected HtmlNode GetNode(HtmlDocument doc, string xpath)
        {
            try
            {
                return doc.DocumentNode.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        protected double ConvertToPrice(string text)
        {
            var digits = string.Concat(text.Where(p => char.IsDigit(p)));
            return double.Parse(digits);
        }

        protected bool IsContains(IEnumerable<string> names, string innerText)
        {
            var s = innerText.ToLower();
            foreach (var name in names)
            {
                if (s.Contains(name.ToLower())) return true;
            }

            return false;
        }

        protected DateTime ConvertToDate(string dateString)
        {
            return ConvertToDate(dateString, "dd.MM.yyyy");
        }

        protected DateTime ConvertToDate(string dateString, string format)
        {
            return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
        }
    }
}

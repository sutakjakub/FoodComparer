using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodComparer.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace FoodComparer.Providers.Tests
{
    [TestClass()]
    public class MajakProviderTests
    {
        [TestMethod()]
        public void GetFoodResultsTest()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            IProvider provider = new MajakProvider(Path.Combine(path, ConfigurationManager.AppSettings["majakProviderUrl"]));

            var result = provider.GetFoodResults(new DateTime(2018, 12, 13), new List<string>() { "kuře" });
            Assert.AreEqual(3, result.Count);
        }
    }
}
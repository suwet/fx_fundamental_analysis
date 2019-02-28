
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using fx_fundamental_analysis.DataModel;
using System.Text.RegularExpressions;

namespace fx_fundamental_analysis.DataFromWebTradeEconomic
{
    public  class FetchData
    {
        IConfigurationRoot configuration;
        public FetchData(IConfigurationRoot configulation)
        {
            this.configuration = configulation;
        }
        public  List<Indicator>GetIndicatorData (string currency)
        {
            var url = configuration.GetSection($"url:{currency}");
            List<Indicator> results = new List<Indicator>();
            if(!string.IsNullOrEmpty(url.Value))
            {
                  var web = new HtmlAgilityPack.HtmlWeb();
                var doc = web.Load(url.Value);

            var nodes = doc.DocumentNode.SelectNodes("//div[@class='tab-content']//table");
            if(nodes.Count>=2)
            {
                // extract table 1
                string table_1 = nodes[0].OuterHtml;
                var doc_tb1 = new HtmlDocument();
                doc_tb1.LoadHtml(table_1);
                foreach(var tr in doc_tb1.DocumentNode.SelectNodes("//tr"))
                {
                        var dataCells = tr.SelectNodes("td");
                        if (dataCells != null)
                        {
                            var item1 = new Indicator();
                             item1.Currency = currency;
                            //System.Console.WriteLine(dataCells[0].InnerText);
                            item1.Name = dataCells[0].InnerText;
                            item1.Actual = decimal.Parse(dataCells[1].InnerText);
                            item1.Q1 = decimal.Parse(dataCells[2].InnerText);
                            item1.Q2 = decimal.Parse(dataCells[3].InnerText);
                            item1.Q3 = decimal.Parse(dataCells[4].InnerText);
                            item1.Q4 = decimal.Parse(dataCells[5].InnerText);
                            results.Add(item1);
                        }
                } 
                 // extract table 2
                string table_2 = nodes[1].OuterHtml;
                var doc_tb2 = new HtmlDocument();
                doc_tb2.LoadHtml(table_2);
                 foreach(var tr in doc_tb2.DocumentNode.SelectNodes("//tr"))
                {
                        var dataCells = tr.SelectNodes("td");
                        if (dataCells != null)
                        {
                            var item2 = new Indicator();
                            item2.Currency = currency;
                            //System.Console.WriteLine(dataCells[0].InnerText);  Regex.Replace(s, @"\t|\n|\r", "");
                            item2.Name = dataCells[0].InnerText;
                            item2.Actual = decimal.Parse(dataCells[1].InnerText);
                            item2.Q1 = decimal.Parse(dataCells[2].InnerText);
                            item2.Q2 = decimal.Parse(dataCells[3].InnerText);
                            item2.Q3 = decimal.Parse(dataCells[4].InnerText);
                            item2.Q4 = decimal.Parse(dataCells[5].InnerText);
                            results.Add(item2);
                        }
                } 
            }
         
            }

            // put it togethor
            return results;
        }
    }
}
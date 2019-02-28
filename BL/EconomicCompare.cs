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

namespace fx_fundamental_analysis.BL
{
    public  class EconomicCompare
    {
         IConfigurationRoot configuration;
        public EconomicCompare( IConfigurationRoot configuration)
        {
            this.configuration = configuration;

            // indicator to compare
            //IConfigurationSection IndicatorSection = configuration.GetSection("indicator");
            //var indicators = IndicatorSection.AsEnumerable();
            // foreach(var s in indicators)
            // {
            //     System.Console.WriteLine(s.Value);
            // }
        }
        
        public void TestCompare()
        {
            IConfigurationSection IndicatorSection = configuration.GetSection("indicator");
            var indicators = IndicatorSection.AsEnumerable();
             foreach(var n in indicators)
            {
                if(!string.IsNullOrEmpty(n.Value))
                {
                    System.Console.WriteLine(n.Value);
                }
            }
        }
        public  Dictionary<string,string> Compare(List<Indicator> one,List<Indicator> two)
        {
             // indicator to compare
            IConfigurationSection IndicatorSection = configuration.GetSection("indicator");
            var indicators = IndicatorSection.AsEnumerable();
            Dictionary<string,string> result = new Dictionary<string, string>();
            foreach(var n in indicators)
            {
                if(!string.IsNullOrEmpty(n.Value))
                {
                    var left = one.FirstOrDefault(x=>x.Name.Trim().Contains(n.Value));
                    var right = two.FirstOrDefault(x=>x.Name.Trim().Contains(n.Value));
                    if(left != null && right != null)
                    {
                        string cur = WichCurrencyRise(left,right);
                        result.Add(n.Value,cur);
                    }
                }
            }
            return result;
        }

        private string WichCurrencyRise(Indicator left,Indicator right)
        {
                //period
                int c1,c2;
                var period = configuration.GetSection("period");

                if((!string.IsNullOrEmpty(period.Value)) && period.Value == "q1,q2")
                {
                    System.Console.WriteLine("compare q1,q2");
                    c1 = CompareByQuarter1_2(left);
                    c2 = CompareByQuarter1_2(right);
                    
                }
                else if((!string.IsNullOrEmpty(period.Value)) && period.Value == "q2,q3")
                {
                    System.Console.WriteLine("compare q2,q3");
                    c1 = CompareByQuarter2_3(left);
                    c2 = CompareByQuarter2_3(right);
                }
                else 
                {
                    System.Console.WriteLine("compare q3,q4");
                    c1 = CompareByQuarter3_4(left);
                    c2 = CompareByQuarter3_4(right);
                }

                if(c1>c2)
                {
                    // number small will make good for currency
                    if(left.Name.Contains("Unemployment Rate")||right.Name.Contains("Unemployment Rate"))
                    {
                        return right.Currency;
                    }
                    return left.Currency;
                }
                    
                if(c1<c2)
                {
                    if(left.Name.Contains("Unemployment Rate")||right.Name.Contains("Unemployment Rate"))
                    {
                        return left.Currency;
                    }
                    return right.Currency;
                }
                    
                return "equal";
        }
        private int CompareByQuarter1_2(Indicator inc)
        {
            if(inc.Q1<inc.Q2)
            {
                            // up = 1
                return 1;
            }
            else if(inc.Q1>inc.Q2)
            {
                            // down = -1;
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private int CompareByQuarter2_3(Indicator inc)
        {
            if(inc.Q2<inc.Q3)
            {
                            // up = 1
                return 1;
            }
            else if(inc.Q2>inc.Q3)
            {
                            // down = -1;
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private int CompareByQuarter3_4(Indicator inc)
        {
            if(inc.Q3<inc.Q4)
            {
                            // up = 1
                return 1;
            }
            else if(inc.Q3>inc.Q4)
            {
                            // down = -1;
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
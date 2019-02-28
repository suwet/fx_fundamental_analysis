using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.Linq;
using fx_fundamental_analysis.DataFromWebTradeEconomic;
using System.Collections.Generic;
using fx_fundamental_analysis.BL;

namespace fx_fundamental_analysis
{
    class Program
    {
        static void Main(string[] args)
        {
             var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            System.Console.WriteLine("forex fundamental anlysis tool by meephoo epher");
            IConfigurationRoot configuration = builder.Build();
            
            var logFile = System.IO.File.Create("result_compare.txt");
            var logWriter = new System.IO.StreamWriter(logFile);

            // loop currency 
            IConfigurationSection CurrencySection = configuration.GetSection("currency");
            var currencys_pair = CurrencySection.AsEnumerable();
            logWriter.WriteLine("example nzdusd, Interest Rate, usd    mean usd rise");
            logWriter.WriteLine("\n");
            FetchData fd = new FetchData(configuration);
            EconomicCompare cmp = new EconomicCompare(configuration);
            //cmp.TestCompare();
            //return ;

            foreach(var cur in currencys_pair)
            {
                if(!string.IsNullOrEmpty(cur.Value))
                {
                    //System.Console.WriteLine(cur.Value.Substring(0,3));
                    //System.Console.WriteLine(cur.Value.Substring(3));

                    var left_indicator = fd.GetIndicatorData(cur.Value.Substring(0,3));
                    var right_indicator = fd.GetIndicatorData(cur.Value.Substring(3));
                    var compare_result = cmp.Compare(left_indicator,right_indicator);
                    logWriter.Write($"{cur.Value}\t");
                    foreach(var pair in compare_result)
                    {
                        //System.Console.WriteLine($"{pair.Key},{pair.Value}");
                        logWriter.Write($"{pair.Key},{pair.Value}\t");
                    }
                    var summarys = compare_result.GroupBy(x=>x.Value).Select(x=> new {name=x.Key,count=x.Count()});
                    logWriter.Write("\t\t ");
                    foreach(var s in summarys)
                    {
                        logWriter.Write($"{s.name}={s.count}  ");
                    }

                    
                    logWriter.WriteLine("\n");

                }
              
            }
            
            logWriter.Dispose();
            System.Console.WriteLine("Success");
        }
    }
}

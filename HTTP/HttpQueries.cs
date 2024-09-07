using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using DeepValueResearch.Models;


namespace DeepValueResearch.HTTP
{
    internal class HttpQueries
    {
        public static CompanyStat Scrapping() 
        {
            CompanyStat result = new CompanyStat();
            //for each trinquet in csv search the yahoo page
            var url = "https://au.finance.yahoo.com/quote/GOOG/key-statistics/"; //test 
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var PriceBook = doc.DocumentNode.SelectSingleNode("//*[@id=\"Col1-0-KeyStatistics-Proxy\"]/section/div[2]/div[1]/div/div/div/div/table/tbody/tr[7]/td[2]").InnerText;
            var name = doc.DocumentNode.SelectSingleNode("//*[@id=\"quote-header-info\"]/div[2]/div[1]/div[1]/h1").InnerText;

            if (PriceBook != null && double.Parse(PriceBook) < 0.33 )
            {
                Console.WriteLine(PriceBook);
                Console.WriteLine(name);
                result.PriceToBookValue = double.Parse(PriceBook);
                result.CompanyName = name;
            }
            else
            {
                Console.WriteLine("No nodes found");
            }

            return result;

        }
       

        //scrape the page for Price/bookValue
        //return companies with interesting price/bookValue, discard the rest
    }
}

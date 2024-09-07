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
        public static CompanyStat Scrapping(string trinquet) 
        {
            CompanyStat result = new CompanyStat();
            //for each trinquet in csv search the yahoo page
            var url = $"https://au.finance.yahoo.com/quote/{trinquet}/key-statistics/"; 
            var web = new HtmlWeb();
            var doc = web.Load(url);
            Console.WriteLine(doc.DocumentNode);

            try 
            {
                
                var PriceBookNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"Col1-0-KeyStatistics-Proxy\"]/section/div[2]/div[1]/div/div/div/div/table/tbody/tr[7]/td[2]");
                var nameNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"quote-header-info\"]/div[2]/div[1]/div[1]/h1");

                if (PriceBookNode != null )
                {
                    var PriceBook = PriceBookNode.InnerText;
                    var name = nameNode.InnerText;
                    if(double.Parse(PriceBook) > 0.33)
                    {
                        Console.WriteLine($"{name} price to book ratio is above 0.33");
                        Logger.Log($"{name} price to book ratio is above 0.33");
                        return result;
                    }
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
            catch (HtmlWebException ex)
            {
                // Handle exceptions specific to HtmlAgilityPack
                Logger.Debug($"HtmlWebException for {trinquet}: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Logger.Debug($"Error scraping {trinquet}: {ex.Message}");
            }

            return result;


        }
       
    }
}

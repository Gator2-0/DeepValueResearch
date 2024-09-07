using System.Net.Http;
using System.Text.Json;
using DeepValueResearch.HTTP;
using DeepValueResearch.Models;



namespace DeepValue
{
    public static class DeepValue
    {
        public static void Main()
        {
            Console.WriteLine("DeepValue");
            CompanyStat stat = HttpQueries.Scrapping();
            Console.WriteLine(stat.CompanyName + "\n" + stat.PriceToBookValue);


        }

    }
    
}

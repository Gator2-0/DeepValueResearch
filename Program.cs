using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Net.Http;
using System.Text.Json;
using DeepValueResearch.HTTP;
using DeepValueResearch.Models;
using System.Globalization;
using DeepValueResearch;
using System.Reflection.PortableExecutable;
using System.Dynamic;



namespace DeepValue
{
    public static class DeepValue
    {
        public static void Main()
        {
            Logger.Initialize();
            Console.WriteLine("Deep value scrapping has been started.");
            Logger.Log("Deep value scrapping has been started.");

            var inputASXCSV = @"../../../CSV/ASX_Listed_Companies.csv";
            List<CompanyStat> CompaniesList = new List<CompanyStat>();

            HttpQueries.Scrapping("AHI.AX");

            // Open and read the CSV file
            try
            {
                using (var reader = new StreamReader(inputASXCSV)) 
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Console.WriteLine("reading through CSV");
                    var records = csv.GetRecords<dynamic>();
                    foreach (var record in records)
                    {
                        try 
                        {
                            // Cast the dynamic record to a dictionary
                            var dict = (IDictionary<string, object>)record;

                            // Access the ASX code from the dictionary
                            string asxCode = dict["ASX code"].ToString();
                            Logger.Log($"Scraping page for {asxCode}");

                            CompanyStat stat = HttpQueries.Scrapping($"{asxCode}.ax"); //adding ax for aussie companies
                            if (stat.CompanyName != null)
                            {
                                CompaniesList.Add(stat);
                                Logger.Log($"{stat.CompanyName} was added to the output file");
                            }

                            // Pause for 50ms to avoid bein gblocked by yahoo
                            Thread.Sleep(50);

                        }
                        catch (Exception ex)
                        {
                            Logger.Debug(ex.Message);
                        }
                        

                    }


                }

            } catch (Exception ex) 
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Logger.Debug(ex.Message);
            }

            Logger.Log("all companies have been scrapped. Go get rich");
            SaveToCsv( CompaniesList );

        }

        static void SaveToCsv(List<CompanyStat> companyStats)
        {
            var filePath = "companyStats.csv";  // File location

            // delete the old CSV if exist
            if(File.Exists(filePath)) { File.Delete(filePath); };

            using (var writer = new StreamWriter(filePath))
            {
                // Write the header
                writer.WriteLine("Id,CompanyName,PriceToBookValue");

                // Write each company stat as a row
                foreach (var stat in companyStats)
                {
                    writer.WriteLine($"{stat.Id},{stat.CompanyName},{stat.PriceToBookValue}");
                }
            }

            Console.WriteLine("Data saved to companyStats.csv");
            Logger.Log("Data saved to companyStats.csv");
        }

    }
    
}

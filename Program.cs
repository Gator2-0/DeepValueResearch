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



namespace DeepValue
{
    public static class DeepValue
    {
        public static void Main()
        {
            Console.WriteLine("Deep value scrapping has been started.");
            Logger.Log("Deep value scrapping has been started.");

            var inputASXCSV = "./CSV/ASX_Listed_Companies.csv";
            if (inputASXCSV == null) 
            { 
                Console.WriteLine("no valid input CSV ");
                Logger.Debug("No valid input CSV was selected");
                return;
            };
            List<CompanyStat> CompaniesList = new List<CompanyStat>();

            // Open and read the CSV file
            try
            {
                using (var reader = new StreamReader(inputASXCSV)) 
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    Console.WriteLine("reading through CSV");
                    var records = csv.GetRecord<dynamic>();
                    foreach (var record in records)
                    {
                        Logger.Log($"scrapping page for {record["ASX code"]}");
                        CompanyStat stat = HttpQueries.Scrapping(record["ASX code"]);
                        if (stat.CompanyName != null) 
                        {
                            CompaniesList.Add(stat);
                            Logger.Log($"{stat.CompanyName} was added to the output file");
                        }
                        
                    }


                }

            } catch (Exception ex) 
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Logger.Debug(ex.Message);
            }
            
            
            SaveToCsv( CompaniesList );

        }

        static void SaveToCsv(List<CompanyStat> companyStats)
        {
            var filePath = "companyStats.csv";  // File location

            // Open or create the CSV file for writing
            using (var writer = new StreamWriter(filePath))
            {
                // Write the header (optional)
                writer.WriteLine("Id,CompanyName,PriceToBookValue");

                // Write each company stat as a row
                foreach (var stat in companyStats)
                {
                    writer.WriteLine($"{stat.Id},{stat.CompanyName},{stat.PriceToBookValue}");
                }
            }

            Console.WriteLine("Data saved to companyStats.csv");
        }

    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepValueResearch
{
    internal class Logger
    {
        private static string logFilePath = "log.txt";

        public static void Log(string message)
        {
            // Write the message to the log file with a timestamp
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        public static void Debug(string message)
        {
            // Write debug messages to the log file
            Log($"DEBUG: {message}");
        }
    }
}

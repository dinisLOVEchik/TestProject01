using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace LogAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("Введите путь к файлу для чтения:");
            string fileLog = Console.ReadLine();
            Console.WriteLine("Введите путь к файлу для записи:");
            string fileOutput = Console.ReadLine();
            Console.WriteLine("Адрес начинается с :");
            string addressStart = Console.ReadLine();
            Console.WriteLine("Маска адреса :");
            string addressMask = Console.ReadLine();*/

            string fileLog = ConfigurationManager.AppSettings["FileLog"];
            string fileOutput = ConfigurationManager.AppSettings["FileOutput"];
            string addressStart = ConfigurationManager.AppSettings["AddressStart"];
            string addressMask = ConfigurationManager.AppSettings["AddressMask"];
            DateTime timeStart = DateTime.ParseExact(ConfigurationManager.AppSettings["TimeStart"], "yyyy-MM-dd HH:mm:ss", null);
            DateTime timeEnd = DateTime.ParseExact(ConfigurationManager.AppSettings["TimeEnd"], "yyyy-MM-dd HH:mm:ss", null);


            Dictionary<string, DateTime> logs = ReadLogs(fileLog);
            List<string> filteredLogs = FilterLogs(logs, addressStart, addressMask, timeStart, timeEnd);
            Dictionary<string, int> count = CountRequests(filteredLogs);

            WriteOutput(fileOutput, count);
        }

        static Dictionary<string, DateTime> ReadLogs(string fileLog)
        {
            Dictionary<string, DateTime> logs = new Dictionary<string, DateTime>();
            foreach (string line in File.ReadAllLines(fileLog))
            {
                string[] parts = line.Split([':'], 2);
                string ip = parts[0];
                DateTime timestamp = DateTime.ParseExact(parts[1], "yyyy-MM-dd HH:mm:ss", null);
                logs[ip] = timestamp;
            }
            return logs;
        }

        static List<string> FilterLogs(Dictionary<string, DateTime> logs, string addressStart, string addressMask, DateTime timeStart, DateTime timeEnd)
        {
            var filteredLogs = logs.Where(kv =>
                (string.IsNullOrEmpty(addressStart) || kv.Key.StartsWith(addressStart)) &&
                (string.IsNullOrEmpty(addressMask) || string.Compare(kv.Key, addressMask) <= 0) &&
                kv.Value >= timeStart && kv.Value <= timeEnd
            ).Select(kv => kv.Key).ToList();
            return filteredLogs;
        }

        static Dictionary<string, int> CountRequests(List<string> filteredLogs)
        {
            Dictionary<string, int> count = [];
            foreach (string ip in filteredLogs)
            {
                if (count.ContainsKey(ip))
                {
                    count[ip]++;
                }
                else
                {
                    count[ip] = 1;
                }
            }
            return count;
        }

        static void WriteOutput(string fileOutput, Dictionary<string, int> count)
        {
            using (StreamWriter sw = new StreamWriter(fileOutput))
            {
                foreach (var kvp in count)
                {
                    sw.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
        }
    }
}


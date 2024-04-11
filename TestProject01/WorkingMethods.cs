using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject01
{
    public class WorkingMethods
    {
        public Dictionary<string, DateTime> ReadLogs(string fileLog)
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

        public List<string> FilterLogs(Dictionary<string, DateTime> logs, string addressStart, string addressMask, DateTime timeStart, DateTime timeEnd)
        {
            var filteredLogs = logs.Where(kv =>
                (string.IsNullOrEmpty(addressStart) || kv.Key.StartsWith(addressStart)) &&
                (string.IsNullOrEmpty(addressMask) || string.Compare(kv.Key, addressMask) <= 0) &&
                kv.Value >= timeStart && kv.Value <= timeEnd
            ).Select(kv => kv.Key).ToList();
            return filteredLogs;
        }

        public Dictionary<string, int> CountRequests(List<string> filteredLogs)
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

        public void WriteOutput(string fileOutput, Dictionary<string, int> count)
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using TestProject01;
/*Console.WriteLine("Введите путь к файлу для чтения:");
string fileLog = Console.ReadLine();
Console.WriteLine("Введите путь к файлу для записи:");
string fileOutput = Console.ReadLine();
Console.WriteLine("Адрес начинается с :");
string addressStart = Console.ReadLine();
Console.WriteLine("Маска адреса :");
string addressMask = Console.ReadLine();*/

internal class Program
{
    private static void Main(string[] args)
    {
        string fileLog = ConfigurationManager.AppSettings["FileLog"];
        string fileOutput = ConfigurationManager.AppSettings["FileOutput"];
        string addressStart = ConfigurationManager.AppSettings["AddressStart"];
        string addressMask = ConfigurationManager.AppSettings["AddressMask"];
        DateTime timeStart = DateTime.ParseExact(ConfigurationManager.AppSettings["TimeStart"], "yyyy-MM-dd HH:mm:ss", null);
        DateTime timeEnd = DateTime.ParseExact(ConfigurationManager.AppSettings["TimeEnd"], "yyyy-MM-dd HH:mm:ss", null);

        WorkingMethods workingMethods = new WorkingMethods();

        Dictionary<string, DateTime> logs = workingMethods.ReadLogs(fileLog);
        List<string> filteredLogs = workingMethods.FilterLogs(logs, addressStart, addressMask, timeStart, timeEnd);
        Dictionary<string, int> count = workingMethods.CountRequests(filteredLogs);

        workingMethods.WriteOutput(fileOutput, count);
    }
}
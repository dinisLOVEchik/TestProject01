using System.IO;
using TestProject01;
namespace TestProjectTest
{
    [TestFixture]
    public class TestClass
    {
        private WorkingMethods _workingMethods;
        public TestClass()
        {
            _workingMethods = new WorkingMethods();
        }

        [Test]
        public void Test()
        {
            string fileName = "../TestingOutput.txt";
            string[] LogData = new string[]{
            "192.168.1.10:2022-05-15 08:30:15",
            "10.0.0.5:2022-05-15 09:45:20",
            "172.16.0.3:2022-05-15 10:20:35"
            };
            File.WriteAllLines(fileName, LogData);

            Dictionary<string, DateTime> logs = new Dictionary<string, DateTime>();
            foreach (string line in File.ReadAllLines(fileName))
            {
                string[] parts = line.Split([':'], 2);
                string ip = parts[0];
                DateTime timestamp = DateTime.ParseExact(parts[1], "yyyy-MM-dd HH:mm:ss", null);
                logs[ip] = timestamp;
            }
            DateTime expected = DateTime.ParseExact("2022-05-15 08:30:15", "yyyy-MM-dd HH:mm:ss", null);
            DateTime actually = logs.GetValueOrDefault("192.168.1.10");
            Is.EqualTo(expected == actually);
        }

        [Test]
        public void Test01()
        {
            string filePath = "../TestingFileLog.txt";
            Dictionary<string, DateTime> expected = new Dictionary<string, DateTime>
            {
                {"192.168.1.10", new DateTime(2022, 05, 15, 08, 30, 15) },
                { "10.0.0.5", new DateTime(2022, 05, 15, 09, 45, 20) },
                { "172.16.0.3", new DateTime(2022, 05, 15, 10, 20, 35) },
                { "192.168.0.20", new DateTime(2022, 05, 15, 11, 15, 40) },
                { "10.0.0.15", new DateTime(2022, 05, 15, 12, 10, 45) },
                { "172.16.2.5", new DateTime(2022, 05, 15, 13, 25, 50) },
                { "192.168.1.30", new DateTime(2022, 05, 15, 14, 30, 55) },
                { "10.0.0.7", new DateTime(2022, 05, 15, 15, 40, 00) },
                { "172.16.0.8", new DateTime(2022, 05, 15, 16, 50, 05) },
                { "192.168.2.15", new DateTime(2022, 05, 15, 17, 55, 10) }
            };
            Dictionary<string, DateTime> actual = _workingMethods.ReadLogs(filePath);
            Is.Equals(expected, actual);
        }
    }
}

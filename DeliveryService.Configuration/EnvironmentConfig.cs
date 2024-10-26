using DotNetEnv;
using System.Text;

namespace DeliveryService.Configuration
{
    public class EnvironmentConfig
    {
        private readonly string _envFilePath;

        public EnvironmentConfig(string envFilePath)
        {
            _envFilePath = envFilePath;
            Load(envFilePath);
        }

        public void Load(string envFilePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var lines = File.ReadAllLines(envFilePath, Encoding.GetEncoding("windows-1251"));

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                var parts = line.Split(new[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }

        public string Get(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public (string, DateTime, string, string, bool, bool, bool, bool) LoadEnvironmentVariables()
        {
            var environmentConfig = new EnvironmentConfig(_envFilePath);

            string districtArg = environmentConfig.Get("CITY_DISTRICT") ?? "";

            DateTime deliveryDateTimeArg = Convert.ToDateTime(environmentConfig.Get("DELIVERY_DATETIME"));
            
            string deliveryOrderArg = environmentConfig.Get("DELIVERY_ORDER") ?? "output.txt";
            
            string deliveryLogArg = environmentConfig.Get("DELIVERY_LOG") ?? "logs.txt";

            bool isCityDistrictPassed = !string.IsNullOrEmpty(districtArg);
            bool isDeliveryDateTimePassed = !deliveryDateTimeArg.Equals(DateTime.Now);
            bool isDeliveryOrderPassed = !string.IsNullOrEmpty(deliveryOrderArg);
            bool isDeliveryLogPassed = !string.IsNullOrEmpty(deliveryLogArg);

            return (districtArg, deliveryDateTimeArg, deliveryOrderArg, deliveryLogArg,
                isCityDistrictPassed, isDeliveryDateTimePassed, isDeliveryOrderPassed, isDeliveryLogPassed);
        }
    }
}

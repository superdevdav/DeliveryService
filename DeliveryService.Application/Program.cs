using DeliveryService.Data;
using DeliveryService.Core;
using DeliveryService.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DeliveryService.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string inputFilePath = @"..\..\..\input.txt";
            const string envFilePath = @"..\..\..\.env";

            try
            {
                string cityDistrictArg = "";
                DateTime deliveryDateTimeArg = DateTime.Now;
                string deliveryOrderArg = "output.txt"; // Значение по умолчанию для результирующих заказов
                string deliveryLogArg = "logs.txt"; // Значение по умолчанию для логов

                bool isCityDistrictPassed = false;
                bool isDeliveryDateTimePassed = false;
                bool isDeliveryOrderPassed = false;
                bool isDeliveryLogPassed = false;

                // Проверяем наличие аргумента для загрузки переменных окружения
                bool loadEnv = args.Contains("--load-env") || args.Contains("-l");

                if (loadEnv)
                {
                    var environmentConfig = new EnvironmentConfig(envFilePath);

                    (cityDistrictArg, deliveryDateTimeArg, deliveryOrderArg, deliveryLogArg,
                        isCityDistrictPassed, isDeliveryDateTimePassed, isDeliveryOrderPassed, isDeliveryLogPassed)
                        = environmentConfig.LoadEnvironmentVariables();

                    Console.WriteLine("Переменные окружения загружены из .env файла.");
                    Console.WriteLine(cityDistrictArg);
                    Console.WriteLine(deliveryOrderArg);
                    Console.WriteLine(deliveryLogArg);
                }
                else
                {
                    (cityDistrictArg, deliveryDateTimeArg, deliveryOrderArg, deliveryLogArg,
                        isCityDistrictPassed, isDeliveryDateTimePassed, isDeliveryOrderPassed, isDeliveryLogPassed)
                        = ProcessArguments(args);
                }
                
                string logRunAppString = $"Приложение запущено с параметрами: ";

                // Проверка наличия аргумента _cityDistrict
                try
                {
                    if (isCityDistrictPassed)
                    {
                        logRunAppString += $"_cityDistrict {cityDistrictArg}, ";
                    }
                    else
                    {
                        throw new Exception("no _cityDistrict argument");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Проверка наличия аргумента _firstDeliveryDateTime
                try
                {
                    if (isDeliveryDateTimePassed)
                    {
                        logRunAppString += $"_firstDeliveryDateTime {deliveryDateTimeArg}, ";
                    }
                    else
                    {
                        throw new Exception("no _firstDeliveryDateTime argument");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Проверка наличия аргумента _deliveryOrder
                try
                {
                    if (isDeliveryOrderPassed)
                    {
                        logRunAppString += $"_deliveryOrder {deliveryOrderArg}, ";
                    }
                    else
                    {
                        throw new ArgumentException("no _deliveryOrder argument");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                // Проверка наличия аргумента _deliveryLog
                try
                {
                    if (isDeliveryLogPassed)
                    {
                        logRunAppString += $"_deliveryLog {deliveryLogArg}";
                    }
                    else
                    {
                        throw new ArgumentException("no _deliveryLog argument");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }


                // Фабрика логгеров
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(@"..\..\..\" + deliveryLogArg)
                    .CreateLogger();
                
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddSerilog();
                });

                ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

                logger.LogInformation(logRunAppString);

                
                Reader reader = new Reader(loggerFactory);
                var orders = reader.ReadOrdersFromTxt(inputFilePath);

                
                OrderFilter orderFilter = new OrderFilter(loggerFactory);
                var filtereOrders = orderFilter.FilterOrdersByDistrictAndDate(orders, cityDistrictArg, deliveryDateTimeArg);

                
                string outputFilePath = @"..\..\..\" + deliveryOrderArg;
                Writer writer = new Writer(loggerFactory);
                writer.WriteOrdersToTxt(filtereOrders, outputFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private static (string, DateTime, string, string, bool, bool, bool, bool) ProcessArguments(string[] args)
        {
            string districtArg = "";
            DateTime deliveryDateTimeArg = DateTime.Now;
            string deliveryOrderArg = "output.txt"; // Значение по умолчанию для результирующих заказов
            string deliveryLogArg = "logs.txt"; // Значение по умолчанию для логов

            bool isCityDistrictPassed = false;
            bool isDeliveryDateTimePassed = false;
            bool isDeliveryOrderPassed = false;
            bool isDeliveryLogPassed = false;

            for (int i = 0; i < args.Length - 1; i += 1)
            {
                if (args[i] == "_cityDistrict")
                {
                    districtArg = args[i + 1];
                    isCityDistrictPassed = true;
                }

                if (args[i] == "_firstDeliveryDateTime")
                {
                    deliveryDateTimeArg = Convert.ToDateTime(args[i + 1] + " " + args[i + 2]);
                    isDeliveryDateTimePassed = true;
                }

                if (args[i] == "_deliveryOrder")
                {
                    deliveryOrderArg = args[i + 1];
                    isDeliveryOrderPassed = true;
                }

                if (args[i] == "_deliveryLog")
                {
                    deliveryLogArg = args[i + 1];
                    isDeliveryLogPassed = true;
                }
            }

            return (districtArg, deliveryDateTimeArg, deliveryOrderArg, deliveryLogArg,
                isCityDistrictPassed, isDeliveryDateTimePassed, isDeliveryOrderPassed, isDeliveryLogPassed);
        }
    }
}
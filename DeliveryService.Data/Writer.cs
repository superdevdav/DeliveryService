using DeliveryService.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DeliveryService.Data
{
    public class Writer
    {
        private readonly ILogger _logger;

        public Writer(ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory != null)
            {
                _logger = loggerFactory.CreateLogger<Writer>();
            }
            else
            {
                _logger = NullLogger<Writer>.Instance;
            }
        }

        public void WriteOrdersToTxt(List<Order> orders, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (Order order in orders)
                    {
                        writer.WriteLine($"{order.Id},{order.Weight.ToString().Replace(",", ".")},{order.District},{order.DeliveryDateTime}");
                    }
                }

                string fileName = filePath.Split('\\').Last();
                Console.WriteLine($"Результаты успешно записаны в файл {fileName}");
                _logger.LogInformation($"Результаты успешно записаны в файл {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                _logger.LogError($"Ошибка: {ex.Message}");
            }
        }
    }
}

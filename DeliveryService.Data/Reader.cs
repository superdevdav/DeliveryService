using DeliveryService.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Globalization;
using System.Text;

namespace DeliveryService.Data
{
    public class Reader
    {
        private readonly ILogger _logger;
        private readonly Encoding _encoding;

        public Reader(ILoggerFactory loggerFactory = null, Encoding encoding = null)
        {
            if (loggerFactory != null)
            {
                _logger = loggerFactory.CreateLogger<Reader>();
            }
            else
            {
                _logger = NullLogger<Reader>.Instance;
            }

            if (encoding != null)
            {
                _encoding = encoding;
            }
            else
            {
                _encoding = Encoding.UTF8;
            }
        }

        public List<Order> ReadOrdersFromTxt(string filePath)
        {
            var orders = new List<Order>();

            using (var reader = new StreamReader(filePath, _encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    try
                    {
                        string id = parts[0];

                        NumberFormatInfo provider = new NumberFormatInfo();
                        provider.NumberDecimalSeparator = ".";
                        provider.NumberGroupSeparator = ",";

                        double weight = Convert.ToDouble(parts[1], provider);

                        string district = parts[2];

                        DateTime deliveryDateTime = Convert.ToDateTime(parts[3]);

                        // Валидация данных
                        var error = Order.ValidateData(id, weight, district, deliveryDateTime);
                        if (!string.IsNullOrEmpty(error))
                        {
                            throw new Exception(error);
                        }

                        var order = new Order(id, weight, district, deliveryDateTime);

                        orders.Add(order);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка формата данных в строке: {line}. Ошибка: {ex.Message}");
                        _logger.LogError($"Ошибка формата данных в строке: {line}. Ошибка: {ex.Message}");
                        continue;
                    }
                }
            }

            string fileName = filePath.Split('\\').Last();
            _logger.LogInformation($"Успешно прочитан файл {fileName} с {orders.Count} заказами.");

            return orders;
        }
    }
}

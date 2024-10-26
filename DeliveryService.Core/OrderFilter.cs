using DeliveryService.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DeliveryService.Core
{
    public class OrderFilter
    {
        private readonly ILogger _logger;

        const int TIME_DIFFERENCE_MINUTES = 30;
        
        public OrderFilter(ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory != null)
            {
                _logger = loggerFactory.CreateLogger<OrderFilter>();
            }
            else
            {
                _logger = NullLogger<OrderFilter>.Instance;
            }
        }

        public List<Order> FilterOrdersByDistrictAndDate(List<Order> orders, string dictrict, DateTime deliveryDateTime)
        {
            var filtered_orders = new List<Order>();

            _logger.LogInformation($"Начата фильтрация заказов для района {dictrict} с временным интервалом {TIME_DIFFERENCE_MINUTES} минут после {deliveryDateTime}");

            foreach (var order in orders)
            {
                TimeSpan difference = order.DeliveryDateTime - deliveryDateTime;

                if (order.District == dictrict && Math.Abs(difference.TotalMinutes) <= TIME_DIFFERENCE_MINUTES)
                {
                    filtered_orders.Add(order);
                }
            }

            _logger.LogInformation($"Фильтрация завершена. Найдено {filtered_orders.Count} заказов.");

            return filtered_orders;
        }
    }
}

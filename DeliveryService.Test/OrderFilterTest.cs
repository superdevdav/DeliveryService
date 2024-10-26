using DeliveryService.Core.Models;
using DeliveryService.Core;

namespace DeliveryService.Tests
{
    [TestClass]
    public class OrderFilterTest
    {
        [TestMethod]
        public void FilterOrdersByDistrictAndDate_ShouldReturnFilteredOrders_WhenDistrictMatches()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("1", 10, "�����������", new DateTime(2023, 10, 22, 10, 0, 0)),
                new Order("2", 15, "�����������", new DateTime(2023, 10, 22, 10, 10, 0)),
                new Order("3", 20, "�����", new DateTime(2023, 10, 22, 10, 20, 0))
            };
            var filter = new OrderFilter();
            var district = "�����������";
            var deliveryDateTime = new DateTime(2023, 10, 22, 10, 0, 0);

            // Act
            var result = filter.FilterOrdersByDistrictAndDate(orders, district, deliveryDateTime);

            // Assert
            Assert.AreEqual(2, result.Count); // ������ ���� ������ ������ �� "������������"
        }

        [TestMethod]
        public void FilterOrdersByDistrictAndDate_ShouldReturnEmpty_WhenNoOrdersMatchDistrict()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("1", 10, "��������", new DateTime(2023, 10, 22, 10, 0, 0)),
                new Order("2", 15, "��������", new DateTime(2023, 10, 22, 10, 10, 0))
            };
            var filter = new OrderFilter();
            var district = "�����������";
            var deliveryDateTime = new DateTime(2023, 10, 22, 10, 0, 0);

            // Act
            var result = filter.FilterOrdersByDistrictAndDate(orders, district, deliveryDateTime);

            // Assert
            Assert.AreEqual(0, result.Count); // ������� ������ ������
        }

        [TestMethod]
        public void FilterOrdersByDistrictAndDate_ShouldHandleEmptyOrderList()
        {
            // Arrange
            var orders = new List<Order>();  // ������ ������ �������
            var filter = new OrderFilter();
            var district = "�����������";
            var deliveryDateTime = new DateTime(2023, 10, 22, 10, 0, 0);

            // Act
            var result = filter.FilterOrdersByDistrictAndDate(orders, district, deliveryDateTime);

            // Assert
            Assert.AreEqual(0, result.Count);  // ������� ������ ������
        }

        [TestMethod]
        public void FilterOrdersByDistrictAndDate_ShouldFilterByTimeDifference()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("1", 10, "�����������", new DateTime(2023, 10, 22, 9, 45, 0)),
                new Order("2", 15, "�����������", new DateTime(2023, 10, 22, 10, 5, 0)),
                new Order("3", 20, "�����������", new DateTime(2023, 10, 22, 9, 20, 0))  // ��� ��������� 30 �����
            };
            var filter = new OrderFilter();
            var district = "�����������";
            var deliveryDateTime = new DateTime(2023, 10, 22, 10, 0, 0);

            // Act
            var result = filter.FilterOrdersByDistrictAndDate(orders, district, deliveryDateTime);

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}

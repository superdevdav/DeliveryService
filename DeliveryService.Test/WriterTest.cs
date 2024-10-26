using DeliveryService.Core.Models;
using DeliveryService.Data;

namespace DeliveryService.Test
{
    [TestClass]
    public class WriterTest
    {
        [TestMethod]
        public void WriterTest_ShouldWriteToOutputFileOrders_WhenOrderListIsNotEmpty()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("A00001", 10, "Центральный", new DateTime(2023, 10, 22, 10, 0, 0)),
                new Order("A00002", 15, "Центральный", new DateTime(2023, 10, 22, 10, 10, 0)),
                new Order("A00003", 20, "Южный", new DateTime(2023, 10, 22, 10, 20, 0))
            };

            string filePath = @"..\..\..\output_test.txt";

            File.Delete(filePath);

            var writer = new Writer();

            // Act
            writer.WriteOrdersToTxt(orders, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            var fileContent = File.ReadAllLines(filePath);
            Assert.AreEqual(orders.Count, fileContent.Length);
        }

        [TestMethod]
        public void ReaderTest_ShouldReturnEmpty_WhenEmptyFile()
        {
            // Arrange
            var orders = new List<Order>();

            string filePath = @"..\..\..\output_test.txt";

            File.Delete(filePath);

            var writer = new Writer();

            // Act
            writer.WriteOrdersToTxt(orders, filePath);

            // Assert
            Assert.IsTrue(File.Exists(filePath));
            var fileContent = File.ReadAllLines(filePath);
            Assert.AreEqual(0, fileContent.Length);
        }
    }
}

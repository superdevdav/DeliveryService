using DeliveryService.Data;
using System.Text;

namespace DeliveryService.Test
{
    [TestClass]
    public class ReaderTest
    {
        [TestMethod]
        public void ReaderTest_ShouldReadInputFileAndReturnOrders_WhenNotEmptyFile()
        {
            // Arrange
            string filePath = @"..\..\..\input_not_empty.txt";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("windows-1251");

            var reader = new Reader(null, encoding);

            // Act
            var result = reader.ReadOrdersFromTxt(filePath);

            // Assert
            var lines = File.ReadAllLines(filePath);
            var firstOrderIdFromFile = lines.First().Split(',').First();
            var lastOrderIdFromFile = lines.Last().Split(',').First();
            Assert.AreEqual(firstOrderIdFromFile, result.First().Id);
            Assert.AreEqual(lastOrderIdFromFile, result.Last().Id);
        }

        [TestMethod]
        public void ReaderTest_ShouldReturnEmpty_WhenEmptyFile()
        {
            // Arrange
            string filePath = @"..\..\..\input_empty.txt";

            var reader = new Reader();

            // Act
            var result = reader.ReadOrdersFromTxt(filePath);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}

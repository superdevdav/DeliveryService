using System.Text.RegularExpressions;

namespace DeliveryService.Core.Models
{
    public class Order
    {
        private static List<string> districts = new List<string>() { "центральный", "западный", "восточный", "южный", "северный" };

        public string Id { get; }

        public double Weight { get; }

        public string District { get; } = string.Empty;

        public DateTime DeliveryDateTime { get; }

        public Order(string Id, double Weight, string District, DateTime DeliveryDateTime)
        {
            this.Id = Id;
            this.Weight = Weight;
            this.District = District;
            this.DeliveryDateTime = DeliveryDateTime;
        }

        public static string ValidateData(string Id, double Weight, string District, DateTime DeliveryDateTime)
        {
            if (string.IsNullOrEmpty(Id) || !Regex.IsMatch(Id, @"^[A-Z]\d{5}$"))
            {
                return "Id value is incorrect or empty";
            }

            if (Weight <= 0)
            {
                return "Weight can not be negative or zero";
            }
            
            if (string.IsNullOrEmpty(District) || !districts.Contains(District.ToLower()))
            {
                return "District incorrect or empty";
            }

            return string.Empty;
        }
    }
}

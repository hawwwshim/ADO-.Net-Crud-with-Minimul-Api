using System.ComponentModel.DataAnnotations;

namespace CustomerApi.Models
{
    public class Customers
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? ContactName { get; set; }
        public string? Country { get; set; }
    }
}

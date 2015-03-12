using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SellingReport.Models.Models
{
    public class Product
    {
        public Product()
        {
            ProductSellingPlans = new List<ProductSellingPlan>();
            ProductSellingReports = new List<ProductSellingReport>();
        }
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int BottleHeight { get; set; }
        private ICollection<ProductSellingPlan> ProductSellingPlans { get; set; }
        private ICollection<ProductSellingReport> ProductSellingReports { get; set; }
    }
}
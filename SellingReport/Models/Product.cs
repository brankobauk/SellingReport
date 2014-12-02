using System.ComponentModel.DataAnnotations;


namespace SellingReport.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
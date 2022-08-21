using System.Collections.Generic;

namespace Backend.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Ingredients { get; set; }
        public List<Item> Items { get; set; }
    }
}

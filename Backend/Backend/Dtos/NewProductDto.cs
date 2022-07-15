using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dtos
{
    public class NewProductDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Ingredients { get; set; }
    }
}

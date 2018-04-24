using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tai.Api.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
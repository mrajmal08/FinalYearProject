using System;
using System.Collections.Generic;

namespace FinalYearProject.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string ProductVariety { get; set; }
        public string ProductImage { get; set; }
        public bool? ProductDeals { get; set; }
        public string DiscountedPrice { get; set; }
        public string ProductRating { get; set; }
        public string ProductUrl { get; set; }
        public string ProductBrand { get; set; }
        public string ProductViews { get; set; }
        public DateTime? ProductCreatedDate { get; set; }
        public string ProductStatus { get; set; }
        public string ProductMetaTags { get; set; }
        public string ProductMetaDisc { get; set; }
        public int? CategoryId { get; set; }
        public int? WebsiteId { get; set; }

    }
}

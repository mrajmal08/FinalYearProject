using System;
using System.Collections.Generic;

namespace FinalYearProject.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryDisc { get; set; }
        public string CategoryStatus { get; set; }
        public DateTime? CategoryCreatedDate { get; set; }
    }
}

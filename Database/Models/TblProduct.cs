using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblProduct
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string ProductName { get; set; }
    }
}

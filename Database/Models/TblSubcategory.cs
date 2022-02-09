using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblSubcategory
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string SubcategoryName { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class TblVersion
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Ismandatory { get; set; }
    }
}

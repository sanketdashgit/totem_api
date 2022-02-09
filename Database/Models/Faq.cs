using System;
using System.Collections.Generic;

#nullable disable

namespace Totem.Database.Models
{
    public partial class Faq
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
